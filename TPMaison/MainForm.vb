' Réalisé en tant que DM Maison (Cours Informatique & Communication industrielle) - LPro DEA - 2019/2020
'
' Versions :
' .NET Framework  4.7.2
' Newtonsoft.Json 12.0.3

' Matos utilisé :
' SP Simulé : Hercules et com0com (simulation d'une liaison COM3 -> COM4)
' API		: Schneider M221CE24R (et Hercules pour la simu)

' Test: 
' 11/07/2020 à 00h31 : ok sans le bgWorker (il manque le "WriteMultipleBits/Mots" de la lib EncapsulationModbusTCPIP)
' 14/07/2020 à 19h32 : Ok avec le bgWorker (manque tjs le writeMultipleBits/Mots dans la lib)

Imports System.IO               'Pour le SP / lecture du fichier paramDefault
Imports System.Net.Sockets      'Pour la connexion TCP
Imports Newtonsoft.Json         'pour lire les params par défauts
Imports System.ComponentModel   'pour le bgWorker

Public Class MainForm

#Region "Définitions Globales"
	'Port RS232 pour le scanneur de code barre
	Public SPScanneurCbarres As New Ports.SerialPort

	'Ma lib d'encapsulation Modbus TCP
	Dim TrameModbus As New ClassEncapsulModBusTCPIP()

	''' <summary>
	''' Permet de faire le lien entre le thread principal et le Thread de gestion TCPIP (backgroundWorker)
	''' </summary>
	Class bgWorker_Args
		'------- Variables Publiques ----------------
		Public barCode As Byte
		Public BacNumber As Byte
		Public SPSuccess As Boolean

		Public ResponseBuffer As Byte()
		Public TCPSuccess As Boolean

		'--------- Constructeur -------------------
		Sub New(ByRef SPData As ValueTuple(Of Byte, Byte, Boolean))
			barCode = SPData.Item1
			BacNumber = SPData.Item2
			SPSuccess = SPData.Item3
		End Sub

	End Class

	Dim isSocketParamsLocked As Boolean = False
	Dim isAPISimulated As Boolean = False '(on vérifie pas la réponse)

#End Region


#Region "IHM\Globale : Load-Unload fenetre principale / Update de l'IHM / Lock des BP / Reset des compteurs"
	'Chargement de la fenêtre principale (MainForm) (= lancement du programme)
	Private Sub ChargementMainForm(sender As Object, e As EventArgs) Handles MyBase.Load

		'Grisage des BPs du SP (en attendant le réglage des paramètres)
		BP_DéconnexSP.Enabled = False
		BP_DéconnexSP.Enabled = False

		'Ajout d'un gestionnaire d'évenement pour l'event
		AddHandler SPScanneurCbarres.DataReceived, AddressOf Handle_InvokerSPEventDataReceived

		'Réglages par défaut du Port COM 
		Dim ConfFile As StreamReader

		Try
			ConfFile = New StreamReader("../../../configFile.json")
		Catch er As FileNotFoundException
			MsgBox("Fichier de configuration ""configFile.json"" NOT FOUND" & vbCrLf & "Les interface ne seront pas configurées !")
			Exit Sub
		End Try

		Dim jsonString As String = ConfFile.ReadToEnd()
		ConfFile.Close()
		Dim jsonObject As Linq.JObject = JsonConvert.DeserializeObject(jsonString)

		Dim ParamsSP As Linq.JToken = jsonObject.SelectToken("SP")
		Dim ParamsTCPIP As Linq.JToken = jsonObject.SelectToken("TCPIP")

		SPScanneurCbarres.PortName = ParamsSP.SelectToken("PortName")
		SPScanneurCbarres.BaudRate = CInt(ParamsSP.SelectToken("BaudRate"))
		SPScanneurCbarres.DataBits = CInt(ParamsSP.SelectToken("DataBits"))
		SPScanneurCbarres.Parity = CInt(ParamsSP.SelectToken("Parity"))
		SPScanneurCbarres.StopBits = CInt(ParamsSP.SelectToken("StopBits"))

		'Réglages par défaut du socket API
		TB_IP_API.Text = ParamsTCPIP.SelectToken("IP")
		TB_AddrMotAPI.Text = ParamsTCPIP.SelectToken("AddrMW")

		'Ne pas ajouter de lignes après celle ci (si erreur dans le try..catch, ça ferme le reste de la fonction)
		'-------------------------------------------------------------------------------------------------------

	End Sub

	'Fermeture de la fenêtre principale (MainForm) (= Fermeture du programme)
	Private Sub FermetureMainForm(sender As Object, e As EventArgs) Handles MyBase.Closed
		'Fermeture du port COM et déconnexion du Socket TCP
		If SPScanneurCbarres.IsOpen Then SPScanneurCbarres.Close()

		BgWorker_TCPIP.CancelAsync()

	End Sub

	'Met à jour les valeur de l'IHM
	Public Sub UpdateIHM(ByVal CodeBarNumber As Byte, ByVal BacNumber As Byte)
		'Incrémentation du compteur de Bac
		Select Case BacNumber
			Case 1
				TB_nbrBac1.Text = CByte(TB_nbrBac1.Text) + 1
				Panel_Bac1.BackColor = Color.Chartreuse
				Panel_Bac2.BackColor = Color.LightGray
				Panel_Bac3.BackColor = Color.LightGray
			Case 2
				TB_nbrBac2.Text = CByte(TB_nbrBac2.Text) + 1
				Panel_Bac1.BackColor = Color.LightGray
				Panel_Bac2.BackColor = Color.Chartreuse
				Panel_Bac3.BackColor = Color.LightGray
			Case 3
				TB_nbrBac3.Text = CByte(TB_nbrBac3.Text) + 1
				Panel_Bac1.BackColor = Color.LightGray
				Panel_Bac2.BackColor = Color.LightGray
				Panel_Bac3.BackColor = Color.Chartreuse
		End Select

		'Ajout du code barre dans l'historique
		TB_history.AppendText(CStr(TimeString) + " => Code Barre : " + CStr(CodeBarNumber) + vbCrLf)

	End Sub

	'Empêche la modification des paramètres pendant la communication SP ou TCP
	Public Sub LockModifBP(ByVal lock As Boolean)

		'si on ré-active les BP, on affiche "En Attente..."
		If Not lock Then
			TB_Status.BackColor = Color.LightGreen
			TB_Status.Text = "Attente..."
		End If

		'Activation/Désactivation des BP
		BP_DéconnexSP.Enabled = Not lock
		BP_ModifySocketParams.Enabled = Not lock
		BP_RstCounters.Enabled = Not lock
		BP_ToggleAPISimu.Enabled = Not lock
	End Sub
	'Reset des compteurs de bac, et de l'historique
	Private Sub ResetCounters(sender As Object, e As EventArgs) Handles BP_RstCounters.Click
		TB_nbrBac1.Text = 0
		TB_nbrBac2.Text = 0
		TB_nbrBac3.Text = 0

		Panel_Bac1.BackColor = Color.LightGray
		Panel_Bac2.BackColor = Color.LightGray
		Panel_Bac3.BackColor = Color.LightGray

		TB_history.Clear()
	End Sub
#End Region

#Region "IHM\RS232 : Fenêtre des paramètres / Connexion-Déconnexion"
	'Ouverture de la fenêtre de modification des paramètres du SP
	Private Sub OuvertureWindowsParamètresSP(sender As Object, e As EventArgs) Handles BP_ModifyParams.Click
		'Ouverture de la fenêtre de réglage des paramètres du Port COM
		RéglagesSerialPort.ShowDialog() 'affiche comme une fenêtre modale (= qui garde le focus par rapport à la MainForm)
	End Sub

	'Ouverture du Port COM
	Private Sub OuvertureDuSP(sender As Object, e As EventArgs) Handles BP_connectToSP.Click
		'Connexion au port COM
		Try
			SPScanneurCbarres.Open()
		Catch ex As Exception
			MsgBox("[Connex SP] ERREUR : " + ex.Message)
			Exit Sub
		End Try

		'On modifie le grisage des BP de connexion/déco
		GroupB_PortCOM.BackColor = Color.LightGreen
		BP_connectToSP.Enabled = False
		BP_DéconnexSP.Enabled = True
		BP_ModifyParams.Enabled = False

	End Sub

	'Fermeture du Port COM
	Private Sub FermetureDuSP(sender As Object, e As EventArgs) Handles BP_DéconnexSP.Click
		'Fermeture du port COM
		Try
			SPScanneurCbarres.Close()
		Catch ex As Exception
			MsgBox("[Déconnex SP] ERREUR : " + ex.Message)
		End Try

		'On modifie le grisage des BP de connexion/déco (si le port s'est fermé)
		If Not SPScanneurCbarres.IsOpen() Then
			GroupB_PortCOM.BackColor = Color.Salmon
			BP_connectToSP.Enabled = True
			BP_DéconnexSP.Enabled = False
			BP_ModifyParams.Enabled = True
		End If
	End Sub
#End Region

#Region "IHM\Modbus TCPIP : vérif des Inputs / Grisage des boutons de validation des paramètres"
	' Validation des paramètres
	Private Sub ValidationParamètresSocket(sender As Object, e As EventArgs) Handles BP_ValidSockeParams.Click

		'Ping de l'adresse IP pour vérifier qu'elle existe
		If Not My.Computer.Network.Ping(TB_IP_API.Text) Then
			MsgBox("Le ping a échoué !" & vbCrLf & "Vérifiez que l'API est bien connecté, ainsi que l'adresse IP")
			Exit Sub
		End If

		'On modifie le grisage des BP de connexion/déco (si les paramètres TCP sont corrects)
		isSocketParamsLocked = True
		BP_ValidSockeParams.Enabled = False
		BP_ModifySocketParams.Enabled = True
		TB_IP_API.Enabled = False
		TB_AddrMotAPI.Enabled = False
		GroupB_Automate.BackColor = Color.LightGreen

	End Sub

	' Invalidation des paramètres
	Private Sub AutorisationDeModifParamètresSocket(sender As Object, e As EventArgs) Handles BP_ModifySocketParams.Click

		'On modifie le grisage des BP de connexion/déco
		isSocketParamsLocked = False
		BP_ValidSockeParams.Enabled = True
		BP_ModifySocketParams.Enabled = False
		TB_IP_API.Enabled = True
		TB_AddrMotAPI.Enabled = True
		GroupB_Automate.BackColor = Color.Salmon

	End Sub

	'Events : TB_IP_API OU TB_AddrMotAPI a changé => On vérifie que les Inputs sont valides
	Private Sub ModifTexte_TB_IP(sender As Object, e As EventArgs) Handles TB_IP_API.TextChanged
		'Valide/Invalide le bouton de validation des paramètres
		BP_ValidSockeParams.Enabled = CheckSocketParam(TB_IP_API.Text, TB_AddrMotAPI.Text)
	End Sub
	Private Sub ModifTexte_TB_AddrMW(sender As Object, e As EventArgs) Handles TB_AddrMotAPI.TextChanged
		'Valide/Invalide le bouton de validation des paramètres
		BP_ValidSockeParams.Enabled = CheckSocketParam(TB_IP_API.Text, TB_AddrMotAPI.Text)
	End Sub

	''' <summary>
	''' Vérifie les paramètres du socket API (IP et Addresse %MW)
	''' </summary>
	''' <param name="IPAdress"> Adresse IP à vérifier </param>
	''' <param name="AddrMW"> Adresse %MW à vérifier </param>
	''' <returns></returns>
	Private Function CheckSocketParam(IPAdress As String, AddrMW As Object) 'AddrMW : <Object> car peut être vide (Vide != int)
		Dim RetValid As Boolean

		'Try..Catch permet d'éviter l'erreur si la TB AddrMW est vide 
		Try
			RetValid = (0 <= AddrMW < 8000) AndAlso TryToParseIP(TB_IP_API.Text)
		Catch
			RetValid = False
		End Try

		Return RetValid
	End Function

	Private Sub BP_ToggleAPISimu__CLICK(sender As Object, e As EventArgs) Handles BP_ToggleAPISimu.Click
		Dim msg As String = "En mode ""API Simulé"", le programme ne vérifie pas la cohérence de la réponse API" & vbCrLf
		msg &= If(isAPISimulated, "Voulez-vous le désactiver ?", "Voulez-vous l'activer ?")

		'Affichage du message & demande d'activation/désactivation
		Dim result As MsgBoxResult = MsgBox(msg, MsgBoxStyle.YesNo)

		If (result = vbYes) Then
			BP_ToggleAPISimu.BackColor = If(isAPISimulated, Color.OrangeRed, Color.GreenYellow)
			BP_ToggleAPISimu.Text = If(isAPISimulated, "Simu API : NON", "Simu API : OUI")
			isAPISimulated = Not isAPISimulated
		End If
	End Sub

#End Region

	'-------------------------------------------------------------------------------
	'-------------------------------------------------------------------------------

#Region "Process\Partie RS232 (Scanneur de Codes Barre)"

	'Récupére la valeur du code barre et le numéro de bac (retourne un "Tuples" avec les 2 valeurs)
	Public Function GetSPBarcode(ByRef SP As Ports.SerialPort) As (barCodeValue As Byte, bacNumber As Byte, success As Boolean)

		Dim barCodeValue, bacNumber As Byte

		'Lecture du code barre depuis le Serial Port
		'Si on scanne avant qu'on soit connecté au SP, on lit tout les codes-barres en même temps (et ça plante)
		'Workaround : on lit le premier nombre, puis on supprime tout le buffer d'entrée pour rattraper le retard
		Try
			barCodeValue = CByte(SP.ReadLine()) 'CByte() => enlève le retour à la ligne (à la fin de la trame)
			SP.DiscardInBuffer() 'Suppression du buffer d'entrée restant

		Catch er As ArithmeticException
			TB_history.AppendText(CStr(TimeString) + " => ERR CBarre > 255" + vbCrLf)
			MsgBox("Le code barre scanné est trop grand (max = 255)", MsgBoxStyle.Exclamation)
			Return (0, 0, False) 'Success = false

		Catch er As Exception
			MsgBox("Erreur sur la lecture sur le port COM" & vbCrLf & er.Message)
			Return (0, 0, False) 'Success = false

		End Try

		'Détermination du numéro de bac
		Select Case barCodeValue
			Case 1 To 15 : bacNumber = 1
			Case 16 To 25 : bacNumber = 2
			Case > 25 : bacNumber = 3
			Case 0
				TB_history.AppendText(CStr(TimeString) + " => ERR CBarre = 0" + vbCrLf)
				MsgBox("Le code barre (" & barCodeValue & ") est en dehors des plages du BAC 1, 2 ou 3", MsgBoxStyle.Exclamation)
				Return (0, 0, False) 'Success = false
		End Select

		Return (barCodeValue, bacNumber, True) 'Success = true

	End Function

	'Invoker => Renvoie vers EventSPDataReceived()) (un peu chelou, il doit y avoir un moyen de faire plus smooth ?)
	Private Sub Handle_InvokerSPEventDataReceived()
		'Permet de bypass le fait que l'event se déclenche sur un autre thread (et rend impossible la modification du MainForm)
		'====> voir fonction EventSPDataReceived()
		'plus d'info -> voir avant dernier paraph tout en bas : 
		'https://docs.microsoft.com/fr-fr/dotnet/api/system.io.ports.serialport.datareceived?view=netframework-4.8

		Invoke(New MethodInvoker(AddressOf EventSP_DataReceived))
	End Sub

	'Event de reception des données du Port COM : envoi de la valeur du bac sur l'API
	Private Sub EventSP_DataReceived()

		'----------------------------------------------------------------
		'vérif de la connexion/validité des interfaces
		'----------------------------------------------------------------
		If Not SPScanneurCbarres.IsOpen Or Not isSocketParamsLocked Then
			SPScanneurCbarres.DiscardInBuffer()
			MsgBox("Un scan a été réalisé, mais les paramètres de l'API ne sont pas validés")
			Exit Sub
		End If

		LockModifBP(True)

		'----------------------------------------------------------------
		'Récupération du code barre & du numéro de bac
		'----------------------------------------------------------------
		Dim dataOfSP = GetSPBarcode(SPScanneurCbarres)

		'Si l'opération a échouée => on sors du Sub
		If Not dataOfSP.success Then
			LockModifBP(False)
			Exit Sub
		End If

		'----------------------------------------------------------------
		'Démarre le travail du Worker (envois le code barre à l'API, vérifie sa réponse, et mets à jour l'IHM)
		'Passage de la structure 
		'----------------------------------------------------------------
		Dim ArgsForBgWorker As New bgWorker_Args(dataOfSP)

		'On lance le worker s'il n'est pas déjà lancé
		If Not BgWorker_TCPIP.IsBusy Then BgWorker_TCPIP.RunWorkerAsync(ArgsForBgWorker)

	End Sub

#End Region

#Region "Process\Modbus TCPIP (Echange avec l'API)"

#Region "Gestion Thread Séparé (BackGroundWorker)"

	'S'execute dans le thread séparé
	Private Sub bgWorker_DoTCPWork(ByVal sender As Object, ByVal e As DoWorkEventArgs) Handles BgWorker_TCPIP.DoWork
		Dim worker As BackgroundWorker = CType(sender, BackgroundWorker)
		Dim ArgsForbgWorker As bgWorker_Args = e.Argument

		'Envoie le code bar sur l'API, et récupère la trame de retour pour vérification
		SendBarCodeToTheAPI(worker, ArgsForbgWorker)

		'Si l'envoi du buffer TCP a échoué => on arrête le thread
		If Not ArgsForbgWorker.TCPSuccess Then
			'echec de connex ou API répond pas
			e.Cancel = True
			Exit Sub
		End If

		'----------------------------------------------------------------
		'Vérification de la cohérence de la réponse de l'API
		Dim reponseAPIOK As Boolean = AnalyseResponseFromAPI(worker, ArgsForbgWorker)

		'Si le réponse est incohérente => on arrête le thread
		If Not reponseAPIOK Then
			e.Cancel = True
			Exit Sub
		End If

		'Passe le résultat dans le thread principal
		e.Result = ArgsForbgWorker

	End Sub

	'S'execute dans le thread PRINCIPAL
	Private Sub bgWorker_TCPWorkCompleted(ByVal sender As Object, ByVal e As RunWorkerCompletedEventArgs) Handles BgWorker_TCPIP.RunWorkerCompleted

		'Déverrouillage des boutons de modification
		LockModifBP(False)

		'Gestion de l'annulation et de l'erreur
		If (e.Cancelled = True) Then
			Exit Sub
		ElseIf (e.Error IsNot Nothing) Then
			MsgBox(e.Error.Message, MsgBoxStyle.Critical + MsgBoxStyle.SystemModal)
			Exit Sub
		End If

		Dim ArgsForbgWorker As bgWorker_Args = e.Result

		'Mise à jour de l'IHM (compteurs//affichage ...)
		UpdateIHM(ArgsForbgWorker.barCode, ArgsForbgWorker.BacNumber)

	End Sub

	'
	Private Sub bgWorker_ReportProgress(ByVal sender As Object, ByVal e As ProgressChangedEventArgs) Handles BgWorker_TCPIP.ProgressChanged
		TB_Status.BackColor = Color.Tomato
		TB_Status.Text = CStr(e.UserState)
	End Sub
#End Region

	''' <summary>
	''' Construit la trame TCP pour écrire le code barre dans l'API, puis l'envoie
	''' </summary>
	''' <param name="bgWorkerData"> Données du SP : Numéro du Code Barre // Numéro du bac </param>
	''' <returns name="success"> Succès de l'opération </returns>
	Private Function SendBarCodeToTheAPI(ByRef worker As BackgroundWorker, ByRef bgWorkerData As bgWorker_Args) As Boolean
		Dim socketAPI As New TcpClient()
		Dim APIStream As NetworkStream

		'Connection du client TCP (Port 502 : Modbus TCP/IP)
		worker.ReportProgress(Nothing, "Tentative de connexion sur l'API...")
		Dim AsyncState As IAsyncResult = socketAPI.BeginConnect(TB_IP_API.Text, 502, Nothing, Nothing)

		'attente de 5sec, ou de la validation de la connexion
		AsyncState.AsyncWaitHandle.WaitOne(5000, True)

		If socketAPI.Connected Then
			'termine l'opération asynchrone
			socketAPI.EndConnect(AsyncState)
			APIStream = socketAPI.GetStream()
		Else
			MsgBox("Echec de la connexion à l'API", MsgBoxStyle.SystemModal)
			socketAPI.Close()
			Return bgWorkerData.TCPSuccess = False
		End If

		'Création du buffer & Envoi vers l'API
		Dim BufferToSend As Byte() = TrameModbus.WriteOneWord(TB_AddrMotAPI.Text, bgWorkerData.barCode)
		APIStream.Write(BufferToSend, 0, BufferToSend.Length)

		If (isAPISimulated) Then
			bgWorkerData.TCPSuccess = True
			Return True
		End If

		'Attente de la trame de retour (si on atteint > 3 secondes, on déclenche une erreur "l'API ne répond pas")
		worker.ReportProgress(Nothing, "Attente de réponse de l'API...")
		Dim IsAPIResponding As Boolean = Threading.SpinWait.SpinUntil(Function() socketAPI.Available > 0, 3000)

		If Not IsAPIResponding Then
			MsgBox("L'API ne répond pas" & vbCrLf & "Vérifiez l'IP, et la connexion de l'API", MsgBoxStyle.SystemModal)
			Return bgWorkerData.TCPSuccess = False
		End If

		'lecture de tous les octets de la trame de réponse, pour vérification
		Dim TrameReponse(15) As Byte
		APIStream.Read(TrameReponse, 0, socketAPI.Available)

		'Fermeture des connexions
		APIStream.Close()
		socketAPI.Close()

		bgWorkerData.ResponseBuffer = TrameReponse
		bgWorkerData.TCPSuccess = True
		Return True

	End Function

	''' <summary>
	''' Analyse la trame de réponse de l'API, pour vérifier que l'écriture de la donnée s'est bien déroulé
	''' </summary>
	''' <param name="bgWorkerData"> Structure contenant les données du SP et du TCP </param>
	''' <returns name="success"> Succés de l'opération </returns>
	Private Function AnalyseResponseFromAPI(ByRef worker As BackgroundWorker, ByRef bgWorkerData As bgWorker_Args) As Boolean

		'Mise en forme des données
		'Dim MBAPHeader As Byte() = bgWorkerData.ResponseBuffer.Take(7).ToArray()
		'Dim PDU As Byte() = bgWorkerData.ResponseBuffer.Skip(7).Take(5).ToArray()

		If (isAPISimulated) Then Return True

		'Vérifs MBAP Header
		'on va dire que le transactionID est le bon (car s'il était mauvais, il faudrait refaire tout le code de reception x)

		'Si le champ "Length" (UnitID + PDU) vaut 6, pas d'erreur (si =3 (ou autre, erreur)
		If (bgWorkerData.ResponseBuffer(5) = 6) Then
			Return True

		Else
			Dim ExceptionCode As Byte = bgWorkerData.ResponseBuffer(8)
			MsgBox("Erreur dans la réponse de l'API" & vbCrLf & "Code Exception : " & ExceptionCode, MsgBoxStyle.SystemModal)
			Return False
		End If

		''Vérifs PDU
		'Dim ReturnedAddrMW As UInt16 = (CDbl(trameReponse(8)) << 8) + trameReponse(9)

		'Dim ReturnedValueMW As UInt16 = (CDbl(trameReponse(10)) << 8) + trameReponse(11)

		'If ReturnedAddrMW = TB_AddrMotAPI.Text AndAlso ReturnedValueMW = barCodeNumber Then
		'	Return True 'Success = True
		'Else
		'	'TODO mettre plus d'info si une erreur se produit
		'	MsgBox("La réponse de l'API ne correspond pas" & vbCrLf & vbCrLf & "")
		'	Return False 'Success = False
		'End If
	End Function

	''' <summary>
	''' Vérification de la validité d'une adresse IP
	''' la fonction "TryParse" de la classe AdressIP accepte l'IP "5" en admettant qu'on veut dire "0.0.0.5"
	''' sauf que c'est débile, donc j'ai créé une fonction plus "correcte"
	''' </summary>
	''' <param name="IP"> Adresse IP à tester </param>
	''' <returns name="isValid"> Validitié de l'IP </returns>
	Function TryToParseIP(ByVal IP As String) As Boolean
		Dim TabOfIPBytes As String()

		'On partage la chaine en plusieurs sous-chaîne selon le séparateur "."
		TabOfIPBytes = IP.Split(".")

		'Si on a pas 4 octets => IP non valide
		If (TabOfIPBytes.Length <> 4) Then
			Return 0
		End If

		'on passe dans tout le tableau, en regardant chaque sous-chaine obtenue grace a IP.split()
		For i = 0 To TabOfIPBytes.Length - 1
			'Si la sous-chaine n'est pas un nombre => IP non valide
			If (Not IsNumeric(TabOfIPBytes(i))) Then
				Return 0
			End If

			'Si le nombre dépasse la taille max d'un octet => IP non valide
			If (TabOfIPBytes(i) > Byte.MaxValue) Then
				Return 0
			End If
		Next

		'Si tout les test ont été passés avec succés, on retourne 1
		Return 1
	End Function

#End Region

End Class
