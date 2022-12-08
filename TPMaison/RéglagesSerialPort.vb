Imports System.IO.Ports
Public Class RéglagesSerialPort
    Dim SPMain As SerialPort = MainForm.SPScanneurCbarres

#Region "Evenements Essentiels" 'Tous les évenements essentiels au fonctionnement du programme
    Private Sub BP_ValidParamSP_Click(sender As Object, e As EventArgs) Handles BP_ValidParamSP.Click

        ' Sauvegarde des paramètres
        SPMain.PortName = TBD_SPName.SelectedItem
        SPMain.BaudRate = TBD_SPBauds.SelectedItem
        SPMain.DataBits = TBD_SPDataBits.SelectedItem
        SPMain.StopBits = TBD_SPStopBits.SelectedItem
        'on utilise l'index pour récupérer un nbre entre 0 et 2 qui correspond à la propriété <parity>
        SPMain.Parity = TBD_SPParity.SelectedIndex

        'Enable du BP de connexion au Port COM
        MainForm.BP_connectToSP.Enabled = True

        'Fermeture fenêtre
        Me.Close()
    End Sub


#End Region 'END REGION : Event Essentiels

#Region "Fonctions" 'Les fonctions ré-utilisables
    '
    ' Description : Permet d'actualiser les Port COMx disponibles
    ' Argument : [ByRef] <ComboBox> TBD qu'on veut mettre à jour
    Private Sub UpdateAvailableSPs(ByRef TBD_NumDuSP As ComboBox)
        'listage des ports COM disponibles
        Dim ListPortSerie As String() = SerialPort.GetPortNames()

        'on récupère l'item selectionné pour plus tard
        Dim SPSelected As String = TBD_NumDuSP.SelectedItem

        'on supprime tous les items déjà écrits
        TBD_NumDuSP.Items.Clear()

        'On ajoute tous les ports séries disponibles (try..catch au cas où ya aucun port COM => exception for)
        Try
            For i As Byte = 0 To ListPortSerie.Length - 1
                TBD_NumDuSP.Items.Add(ListPortSerie(i))
            Next
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
            Exit Sub
        End Try


        'si rien n'était selectionné, alors on selectionne le premier index
        If (SPSelected = Nothing) Then
            TBD_NumDuSP.SelectedIndex = 0
            'Sinon, on selectionne l'ancien index
        Else
            TBD_NumDuSP.SelectedItem = SPSelected
        End If


    End Sub

#End Region 'END REGION : Fonctions ré-utilisables

#Region "Evenements Non-Essentiels" 'Tous les évenements non-essentiels au fonctionnement du programme
    'donc : l'affichage // le grisage/dégrisage

    Private Sub TBD_SPName_MouseClick(sender As Object, e As EventArgs) Handles TBD_SPName.MouseClick
		'Mise à jour des Port COM disponibles
		UpdateAvailableSPs(TBD_SPName)
	End Sub

	Private Sub RéglagesSerialPort_Load(sender As Object, e As EventArgs) Handles MyBase.Load
		'Mise à jour des Port COM disponibles
		UpdateAvailableSPs(TBD_SPName)

		'Affectation des paramètres actuels du SP dans les TextBoxDéroulante
		TBD_SPName.SelectedItem = CStr(SPMain.PortName)
		TBD_SPBauds.SelectedItem = CStr(SPMain.BaudRate)
		TBD_SPDataBits.SelectedItem = CStr(SPMain.DataBits)
		TBD_SPParity.SelectedIndex = CStr(SPMain.Parity)
		TBD_SPStopBits.SelectedItem = CStr(SPMain.StopBits)
	End Sub

#End Region 'END REGION : Event Non-Essentiels
End Class