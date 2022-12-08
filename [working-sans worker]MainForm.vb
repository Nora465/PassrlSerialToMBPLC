'TODO    gérer le multi thread : une partie pour gérer la partie IHM/SP et une autre pour la partie TCPIP

' Versions :
' .NET Framework  4.7.2
' Newtonsoft.Json 12.0.3

' Matos utilisé :
' SP Simulé : Hercules et com0com (simulation d'une liaison COM3 -> COM4)
' API		: Schneider M221CE24R

' Test: 
' Le 11/07/2020 à 00h31 : Fonctionnel à 100% (il manque le "WriteMultipleBits/Mots" de la lib EncapsulationModbusTCPIP)

Imports System.IO.Ports
Imports System.IO
Imports System.Net.Sockets
Imports Newtonsoft.Json
Imports System.Threading

Public Class MainForm

#Region "Définitions Globales"
	'Port RS232 pour le scanneur de code barre
	Public SPScanneurCbarres As New SerialPort

	'Thread de gestion du Socket TCP (pour éviter de bloquer le thread principal)
	'Dim ThreadTCPIP As New 



	'Ma lib d'encapsulation Modbus TCP
	Dim TrameModbus As New ClassEncapsulModBusTCPIP()

	'Variables Globales
	'Dim IsSocketParamsValid As Boolean = False
	Dim isSocketParamsLocked As Boolean = False

#End Region


#Region "IHM\Globale : Load-Unload fenetre principale / Update de l'IHM / Reset des compteurs"
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

		'TODO remplacer cette ligne par thread.abort

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

		'ajout d

	End Sub

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

	'Fonction de vérification des paramètres (IP et Addresse %MW)
	''' <summary>
	''' Vérifie les paramètres du socket API
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

#End Region




#Region "Process\Partie RS232 (Scanneur de Codes Barre)"

	'Récupére la valeur du code barre et le numéro de bac (retourne un "Tuples" avec les 2 valeurs)
	Public Function GetSPBarcode(ByRef SP As SerialPort) As (barCodeValue As Byte, bacNumber As Byte, success As Boolean)

		Dim barCodeValue, bacNumber As Byte

		If Not SP.IsOpen() Then
			MsgBox("Le port COM n'est pas ouvert." + vbCrLf + "Ouvrez-le avant d'utiliser la fonction GetSPBarCode !", MsgBoxStyle.Critical)
			Return (0, 0, False) 'Success = false
		End If

		'Lecture du code barre depuis le Serial Port
		'Si on scanne avant qu'on soit connecté au SP, on lit tout les codes-barres en même temps (et ça plante)
		'Workaround : on lit le premier nombre, puis on supprime tout le buffer d'entrée pour rattraper le retard
		Try
			barCodeValue = CByte(SP.ReadLine()) 'CByte() => enlève le retour à la ligne (à la fin de la trame)
			SP.DiscardInBuffer() 'Suppression du buffer d'entrée restant
		Catch er As ArithmeticException
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
			Case Else
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

		Invoke(New MethodInvoker(AddressOf EventSPDataReceived))
	End Sub

	'Event de reception des données du Port COM : envoi de la valeur du bac sur l'API
	Private Sub EventSPDataReceived()

		'VERIF : Toutes les interfaces sont connectés/validés
		If Not SPScanneurCbarres.IsOpen Or Not isSocketParamsLocked Then
			SPScanneurCbarres.DiscardInBuffer()
			MsgBox("Un scan a été réalisé, mais les paramètres de l'API ne sont pas validés")
			Exit Sub
		End If

		'----------------------------------------------------------------
		'Récupération du code barre & du numéro de bac
		Dim dataOfSP = GetSPBarcode(SPScanneurCbarres)

		'Si l'opération a échouée => on sors du Sub
		If Not dataOfSP.success Then
			Exit Sub
		End If

		'----------------------------------------------------------------
		'Envoi le code barre à l'API
		Dim dataOfAPIResponse = SendBarCodeToTheAPI(dataOfSP.barCodeValue)

		'Si l'opération a échoué => on sors du Sub
		If Not dataOfAPIResponse.success Then
			Exit Sub
		End If

		'----------------------------------------------------------------
		'Vérification de la cohérence de la réponse de l'API
		Dim reponseAPIOK As Boolean = AnalyseResponseFromAPI(dataOfSP.barCodeValue, dataOfAPIResponse.trameReponse)

		'Si l'opération a échoué => on sors du Sub
		If Not reponseAPIOK Then
			Exit Sub
		End If

		'----------------------------------------------------------------
		'Mise à jour de l'IHM (compteurs//affichage ...)
		UpdateIHM(dataOfSP.barCodeValue, dataOfSP.bacNumber)

	End Sub

#End Region

#Region "Process\Modbus TCPIP // Echange avec l'API"


	''' <summary>
	''' Construit la trame TCP pour écrire le code barre dans l'API, puis l'envoie
	''' </summary>
	''' <param name="barCodeNumber"> Numéro du Code Barre </param>
	''' <returns name="trameReponse"> Trame de réponse de l'API (pour vérification) </returns>
	''' <returns name="success"> Succès de l'opération </returns>
	Private Function SendBarCodeToTheAPI(ByVal barCodeNumber As Byte) As (trameReponse As Byte(), success As Boolean)
		Dim socketAPI As New TcpClient()
		Dim APIStream As NetworkStream

		'Définition du Socket & Connection (Port 502 : Modbus TCP/IP)
		'TODO si l'API n'est pas connecté, le timeout est de 10 secondes environ (donc bloque le thread principal) => dans un autre thread
		Try
			socketAPI.Connect(TB_IP_API.Text, 502) '502
			APIStream = socketAPI.GetStream()
		Catch er As Exception
			MsgBox("Echec de la connexion à l'API" & vbCrLf & er.Message)
			Return ({0}, False) 'Success = False
		End Try

		'Création du buffer & Envoi vers l'API
		Dim BufferToSend As Byte() = TrameModbus.WriteOneWord(TB_AddrMotAPI.Text, barCodeNumber)
		APIStream.Write(BufferToSend, 0, BufferToSend.Length)

		'lecture de la trame de réponse, pour vérifier
		Dim TrameReponse(15) As Byte

		'Attente de la trame de retour (si on attent > 3 secondes, on déclenche une erreur "l'API ne répond pas"
		Dim IsAPIResponding As Boolean = Threading.SpinWait.SpinUntil(Function() socketAPI.Available > 0, 3000)

		If Not IsAPIResponding Then
			MsgBox("L'API ne répond pas" & vbCrLf & "Vérifiez l'IP, et la connexion de l'API")
			Return ({0}, False) 'Success = False
		End If

		'Lecture de tous les octets de la trame de retour
		APIStream.Read(TrameReponse, 0, socketAPI.Available)

		'Fermeture des connexions
		APIStream.Close()
		socketAPI.Close()

		Return (TrameReponse, True) 'Success = True

	End Function

	''' <summary>
	''' Analyse la trame de réponse de l'API, pour vérifier que l'écriture de la donnée s'est bien déroulé
	''' </summary>
	''' <param name="trameReponse"> Trame de réponse de l'API (pour vérification) </param>
	''' <returns> Succés de l'opération </returns>
	Private Function AnalyseResponseFromAPI(ByRef barCodeNumber As Byte, ByRef trameReponse As Byte()) As Boolean

		'Mise en forme des données
		Dim MBAPHeader As Byte() = trameReponse.Take(7).ToArray()
		Dim PDU As Byte() = trameReponse.Skip(7).Take(5).ToArray()

		'Vérifs MBAP Header
		'on va dire que le transactionID est le bon (car s'il était mauvais, il faudrait tous refaire x)

		'Si le champ "Length" (UnitID + PDU) vaut 3, il y a une erreur (si =6, pas d'erreur)
		If (trameReponse(5) = 3) Then
			Dim ExceptionCode As Byte = trameReponse(8)
			MsgBox("Erreur dans la réponse de l'API" & vbCrLf & "Code Exception : " & ExceptionCode)
			Return False

		ElseIf (trameReponse(5) = 6) Then
			Return True
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
	'Vérification de la validité d'une adresse IP
	'
	'la fonction "TryParse" de la classe AdressIP accepte l'IP "5" en admettant qu'on veut dire "0.0.0.5"
	'sauf que c'est débile, donc j'ai créé une fonction plus "correcte"
	'
	'@param : <String> L'IP qu'on veut tester
	'@return : <boolean> validité de l'IP
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
