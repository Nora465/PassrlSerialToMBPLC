<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class MainForm
    Inherits System.Windows.Forms.Form

    'Form remplace la méthode Dispose pour nettoyer la liste des composants.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Requise par le Concepteur Windows Form
    Private components As System.ComponentModel.IContainer

    'REMARQUE : la procédure suivante est requise par le Concepteur Windows Form
    'Elle peut être modifiée à l'aide du Concepteur Windows Form.  
    'Ne la modifiez pas à l'aide de l'éditeur de code.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Me.GroupB_PortCOM = New System.Windows.Forms.GroupBox()
		Me.BP_DéconnexSP = New System.Windows.Forms.Button()
		Me.BP_connectToSP = New System.Windows.Forms.Button()
		Me.BP_ModifyParams = New System.Windows.Forms.Button()
		Me.GroupB_Automate = New System.Windows.Forms.GroupBox()
		Me.Label7 = New System.Windows.Forms.Label()
		Me.TB_AddrMotAPI = New System.Windows.Forms.TextBox()
		Me.BP_ModifySocketParams = New System.Windows.Forms.Button()
		Me.L_MW = New System.Windows.Forms.Label()
		Me.TB_IP_API = New System.Windows.Forms.TextBox()
		Me.L_EntrerAddrAPI = New System.Windows.Forms.Label()
		Me.L_AddrMWAPI = New System.Windows.Forms.Label()
		Me.BP_ValidSockeParams = New System.Windows.Forms.Button()
		Me.NotifyIcon1 = New System.Windows.Forms.NotifyIcon(Me.components)
		Me.Label1 = New System.Windows.Forms.Label()
		Me.Label2 = New System.Windows.Forms.Label()
		Me.Label3 = New System.Windows.Forms.Label()
		Me.TB_nbrBac1 = New System.Windows.Forms.TextBox()
		Me.TB_nbrBac2 = New System.Windows.Forms.TextBox()
		Me.TB_nbrBac3 = New System.Windows.Forms.TextBox()
		Me.TB_history = New System.Windows.Forms.TextBox()
		Me.BP_RstCounters = New System.Windows.Forms.Button()
		Me.Panel_Bac1 = New System.Windows.Forms.Panel()
		Me.Label4 = New System.Windows.Forms.Label()
		Me.Panel_Bac2 = New System.Windows.Forms.Panel()
		Me.Label5 = New System.Windows.Forms.Label()
		Me.Panel_Bac3 = New System.Windows.Forms.Panel()
		Me.Label6 = New System.Windows.Forms.Label()
		Me.BgWorker_TCPIP = New System.ComponentModel.BackgroundWorker()
		Me.Label8 = New System.Windows.Forms.Label()
		Me.TB_Status = New System.Windows.Forms.TextBox()
		Me.BP_ToggleAPISimu = New System.Windows.Forms.Button()
		Me.GroupB_PortCOM.SuspendLayout()
		Me.GroupB_Automate.SuspendLayout()
		Me.Panel_Bac1.SuspendLayout()
		Me.Panel_Bac2.SuspendLayout()
		Me.Panel_Bac3.SuspendLayout()
		Me.SuspendLayout()
		'
		'GroupB_PortCOM
		'
		Me.GroupB_PortCOM.BackColor = System.Drawing.Color.Salmon
		Me.GroupB_PortCOM.Controls.Add(Me.BP_DéconnexSP)
		Me.GroupB_PortCOM.Controls.Add(Me.BP_connectToSP)
		Me.GroupB_PortCOM.Controls.Add(Me.BP_ModifyParams)
		Me.GroupB_PortCOM.Location = New System.Drawing.Point(12, 12)
		Me.GroupB_PortCOM.Name = "GroupB_PortCOM"
		Me.GroupB_PortCOM.Size = New System.Drawing.Size(160, 159)
		Me.GroupB_PortCOM.TabIndex = 14
		Me.GroupB_PortCOM.TabStop = False
		Me.GroupB_PortCOM.Text = "Configuration du Port COM"
		'
		'BP_DéconnexSP
		'
		Me.BP_DéconnexSP.Enabled = False
		Me.BP_DéconnexSP.Location = New System.Drawing.Point(9, 113)
		Me.BP_DéconnexSP.Name = "BP_DéconnexSP"
		Me.BP_DéconnexSP.Size = New System.Drawing.Size(140, 39)
		Me.BP_DéconnexSP.TabIndex = 7
		Me.BP_DéconnexSP.Text = "Déconnexion du Port COM"
		Me.BP_DéconnexSP.UseVisualStyleBackColor = True
		'
		'BP_connectToSP
		'
		Me.BP_connectToSP.Enabled = False
		Me.BP_connectToSP.Location = New System.Drawing.Point(9, 67)
		Me.BP_connectToSP.Name = "BP_connectToSP"
		Me.BP_connectToSP.Size = New System.Drawing.Size(140, 37)
		Me.BP_connectToSP.TabIndex = 5
		Me.BP_connectToSP.Text = "Connexion au Port COM"
		Me.BP_connectToSP.UseVisualStyleBackColor = True
		'
		'BP_ModifyParams
		'
		Me.BP_ModifyParams.Location = New System.Drawing.Point(9, 23)
		Me.BP_ModifyParams.Name = "BP_ModifyParams"
		Me.BP_ModifyParams.Size = New System.Drawing.Size(140, 34)
		Me.BP_ModifyParams.TabIndex = 8
		Me.BP_ModifyParams.Text = "Modifier les paramètres"
		Me.BP_ModifyParams.UseVisualStyleBackColor = True
		'
		'GroupB_Automate
		'
		Me.GroupB_Automate.BackColor = System.Drawing.Color.Salmon
		Me.GroupB_Automate.Controls.Add(Me.Label7)
		Me.GroupB_Automate.Controls.Add(Me.TB_AddrMotAPI)
		Me.GroupB_Automate.Controls.Add(Me.BP_ModifySocketParams)
		Me.GroupB_Automate.Controls.Add(Me.L_MW)
		Me.GroupB_Automate.Controls.Add(Me.TB_IP_API)
		Me.GroupB_Automate.Controls.Add(Me.L_EntrerAddrAPI)
		Me.GroupB_Automate.Controls.Add(Me.L_AddrMWAPI)
		Me.GroupB_Automate.Controls.Add(Me.BP_ValidSockeParams)
		Me.GroupB_Automate.Location = New System.Drawing.Point(190, 12)
		Me.GroupB_Automate.Name = "GroupB_Automate"
		Me.GroupB_Automate.Size = New System.Drawing.Size(182, 159)
		Me.GroupB_Automate.TabIndex = 15
		Me.GroupB_Automate.TabStop = False
		Me.GroupB_Automate.Text = "Configuration de la connexion API"
		'
		'Label7
		'
		Me.Label7.AutoSize = True
		Me.Label7.Location = New System.Drawing.Point(94, 136)
		Me.Label7.Name = "Label7"
		Me.Label7.Size = New System.Drawing.Size(46, 13)
		Me.Label7.TabIndex = 11
		Me.Label7.Text = "(< 8000)"
		'
		'TB_AddrMotAPI
		'
		Me.TB_AddrMotAPI.BackColor = System.Drawing.SystemColors.Control
		Me.TB_AddrMotAPI.Location = New System.Drawing.Point(42, 132)
		Me.TB_AddrMotAPI.Name = "TB_AddrMotAPI"
		Me.TB_AddrMotAPI.Size = New System.Drawing.Size(50, 20)
		Me.TB_AddrMotAPI.TabIndex = 9
		'
		'BP_ModifySocketParams
		'
		Me.BP_ModifySocketParams.Enabled = False
		Me.BP_ModifySocketParams.Location = New System.Drawing.Point(93, 65)
		Me.BP_ModifySocketParams.Name = "BP_ModifySocketParams"
		Me.BP_ModifySocketParams.Size = New System.Drawing.Size(81, 39)
		Me.BP_ModifySocketParams.TabIndex = 10
		Me.BP_ModifySocketParams.Text = "Modifier les paramètres"
		Me.BP_ModifySocketParams.UseVisualStyleBackColor = True
		'
		'L_MW
		'
		Me.L_MW.AutoSize = True
		Me.L_MW.Location = New System.Drawing.Point(9, 135)
		Me.L_MW.Name = "L_MW"
		Me.L_MW.Size = New System.Drawing.Size(35, 13)
		Me.L_MW.TabIndex = 8
		Me.L_MW.Text = "%MW"
		'
		'TB_IP_API
		'
		Me.TB_IP_API.BackColor = System.Drawing.SystemColors.Control
		Me.TB_IP_API.ForeColor = System.Drawing.Color.Black
		Me.TB_IP_API.Location = New System.Drawing.Point(9, 39)
		Me.TB_IP_API.Name = "TB_IP_API"
		Me.TB_IP_API.Size = New System.Drawing.Size(165, 20)
		Me.TB_IP_API.TabIndex = 6
		'
		'L_EntrerAddrAPI
		'
		Me.L_EntrerAddrAPI.AutoSize = True
		Me.L_EntrerAddrAPI.Location = New System.Drawing.Point(6, 22)
		Me.L_EntrerAddrAPI.Name = "L_EntrerAddrAPI"
		Me.L_EntrerAddrAPI.Size = New System.Drawing.Size(168, 13)
		Me.L_EntrerAddrAPI.TabIndex = 0
		Me.L_EntrerAddrAPI.Text = "Entrez l'Adresse IP de l'Automate :"
		'
		'L_AddrMWAPI
		'
		Me.L_AddrMWAPI.AutoSize = True
		Me.L_AddrMWAPI.Location = New System.Drawing.Point(6, 117)
		Me.L_AddrMWAPI.Name = "L_AddrMWAPI"
		Me.L_AddrMWAPI.Size = New System.Drawing.Size(171, 13)
		Me.L_AddrMWAPI.TabIndex = 7
		Me.L_AddrMWAPI.Text = "Adresse du mot automate à écrire :"
		'
		'BP_ValidSockeParams
		'
		Me.BP_ValidSockeParams.Enabled = False
		Me.BP_ValidSockeParams.Location = New System.Drawing.Point(9, 65)
		Me.BP_ValidSockeParams.Name = "BP_ValidSockeParams"
		Me.BP_ValidSockeParams.Size = New System.Drawing.Size(81, 39)
		Me.BP_ValidSockeParams.TabIndex = 5
		Me.BP_ValidSockeParams.Text = "Valider les paramètres"
		Me.BP_ValidSockeParams.UseVisualStyleBackColor = True
		'
		'NotifyIcon1
		'
		Me.NotifyIcon1.Text = "NotifyIcon1"
		Me.NotifyIcon1.Visible = True
		'
		'Label1
		'
		Me.Label1.AutoSize = True
		Me.Label1.BackColor = System.Drawing.Color.PowderBlue
		Me.Label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 23.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Label1.Location = New System.Drawing.Point(5, 30)
		Me.Label1.Name = "Label1"
		Me.Label1.Size = New System.Drawing.Size(135, 37)
		Me.Label1.TabIndex = 20
		Me.Label1.Text = "BAC n°1"
		'
		'Label2
		'
		Me.Label2.AutoSize = True
		Me.Label2.BackColor = System.Drawing.Color.PowderBlue
		Me.Label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 23.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Label2.Location = New System.Drawing.Point(6, 31)
		Me.Label2.Name = "Label2"
		Me.Label2.Size = New System.Drawing.Size(135, 37)
		Me.Label2.TabIndex = 21
		Me.Label2.Text = "BAC n°2"
		'
		'Label3
		'
		Me.Label3.AutoSize = True
		Me.Label3.BackColor = System.Drawing.Color.PowderBlue
		Me.Label3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 23.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Label3.Location = New System.Drawing.Point(5, 31)
		Me.Label3.Name = "Label3"
		Me.Label3.Size = New System.Drawing.Size(135, 37)
		Me.Label3.TabIndex = 22
		Me.Label3.Text = "BAC n°3"
		'
		'TB_nbrBac1
		'
		Me.TB_nbrBac1.Location = New System.Drawing.Point(53, 75)
		Me.TB_nbrBac1.Name = "TB_nbrBac1"
		Me.TB_nbrBac1.ReadOnly = True
		Me.TB_nbrBac1.Size = New System.Drawing.Size(43, 20)
		Me.TB_nbrBac1.TabIndex = 29
		Me.TB_nbrBac1.Text = "0"
		Me.TB_nbrBac1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
		'
		'TB_nbrBac2
		'
		Me.TB_nbrBac2.Location = New System.Drawing.Point(53, 76)
		Me.TB_nbrBac2.Name = "TB_nbrBac2"
		Me.TB_nbrBac2.ReadOnly = True
		Me.TB_nbrBac2.Size = New System.Drawing.Size(43, 20)
		Me.TB_nbrBac2.TabIndex = 30
		Me.TB_nbrBac2.Text = "0"
		Me.TB_nbrBac2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
		'
		'TB_nbrBac3
		'
		Me.TB_nbrBac3.Location = New System.Drawing.Point(53, 76)
		Me.TB_nbrBac3.Name = "TB_nbrBac3"
		Me.TB_nbrBac3.ReadOnly = True
		Me.TB_nbrBac3.Size = New System.Drawing.Size(43, 20)
		Me.TB_nbrBac3.TabIndex = 31
		Me.TB_nbrBac3.Text = "0"
		Me.TB_nbrBac3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
		'
		'TB_history
		'
		Me.TB_history.Location = New System.Drawing.Point(537, 17)
		Me.TB_history.Multiline = True
		Me.TB_history.Name = "TB_history"
		Me.TB_history.ReadOnly = True
		Me.TB_history.Size = New System.Drawing.Size(156, 284)
		Me.TB_history.TabIndex = 32
		'
		'BP_RstCounters
		'
		Me.BP_RstCounters.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.BP_RstCounters.Location = New System.Drawing.Point(408, 83)
		Me.BP_RstCounters.Name = "BP_RstCounters"
		Me.BP_RstCounters.Size = New System.Drawing.Size(94, 57)
		Me.BP_RstCounters.TabIndex = 33
		Me.BP_RstCounters.Text = "Reset les Compteurs " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "et l'historique"
		Me.BP_RstCounters.UseMnemonic = False
		Me.BP_RstCounters.UseVisualStyleBackColor = True
		'
		'Panel_Bac1
		'
		Me.Panel_Bac1.BackColor = System.Drawing.Color.LightGray
		Me.Panel_Bac1.Controls.Add(Me.Label4)
		Me.Panel_Bac1.Controls.Add(Me.TB_nbrBac1)
		Me.Panel_Bac1.Controls.Add(Me.Label1)
		Me.Panel_Bac1.Location = New System.Drawing.Point(17, 197)
		Me.Panel_Bac1.Name = "Panel_Bac1"
		Me.Panel_Bac1.Size = New System.Drawing.Size(145, 105)
		Me.Panel_Bac1.TabIndex = 34
		'
		'Label4
		'
		Me.Label4.AutoSize = True
		Me.Label4.Location = New System.Drawing.Point(11, 9)
		Me.Label4.Name = "Label4"
		Me.Label4.Size = New System.Drawing.Size(122, 13)
		Me.Label4.TabIndex = 30
		Me.Label4.Text = "Code barre entre 1 et 15"
		'
		'Panel_Bac2
		'
		Me.Panel_Bac2.BackColor = System.Drawing.Color.LightGray
		Me.Panel_Bac2.Controls.Add(Me.Label5)
		Me.Panel_Bac2.Controls.Add(Me.TB_nbrBac2)
		Me.Panel_Bac2.Controls.Add(Me.Label2)
		Me.Panel_Bac2.Location = New System.Drawing.Point(196, 196)
		Me.Panel_Bac2.Name = "Panel_Bac2"
		Me.Panel_Bac2.Size = New System.Drawing.Size(145, 105)
		Me.Panel_Bac2.TabIndex = 35
		'
		'Label5
		'
		Me.Label5.AutoSize = True
		Me.Label5.Location = New System.Drawing.Point(9, 9)
		Me.Label5.Name = "Label5"
		Me.Label5.Size = New System.Drawing.Size(128, 13)
		Me.Label5.TabIndex = 31
		Me.Label5.Text = "Code barre entre 16 et 25"
		'
		'Panel_Bac3
		'
		Me.Panel_Bac3.BackColor = System.Drawing.Color.LightGray
		Me.Panel_Bac3.Controls.Add(Me.Label6)
		Me.Panel_Bac3.Controls.Add(Me.TB_nbrBac3)
		Me.Panel_Bac3.Controls.Add(Me.Label3)
		Me.Panel_Bac3.Location = New System.Drawing.Point(374, 196)
		Me.Panel_Bac3.Name = "Panel_Bac3"
		Me.Panel_Bac3.Size = New System.Drawing.Size(145, 105)
		Me.Panel_Bac3.TabIndex = 36
		'
		'Label6
		'
		Me.Label6.AutoSize = True
		Me.Label6.Location = New System.Drawing.Point(8, 9)
		Me.Label6.Name = "Label6"
		Me.Label6.Size = New System.Drawing.Size(129, 13)
		Me.Label6.TabIndex = 37
		Me.Label6.Text = "Code barre supérieur à 15"
		'
		'BgWorker_TCPIP
		'
		Me.BgWorker_TCPIP.WorkerReportsProgress = True
		Me.BgWorker_TCPIP.WorkerSupportsCancellation = True
		'
		'Label8
		'
		Me.Label8.AutoSize = True
		Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Label8.ForeColor = System.Drawing.Color.Blue
		Me.Label8.Location = New System.Drawing.Point(408, 10)
		Me.Label8.Name = "Label8"
		Me.Label8.Size = New System.Drawing.Size(96, 20)
		Me.Label8.TabIndex = 38
		Me.Label8.Text = "Etat Actuel :"
		'
		'TB_Status
		'
		Me.TB_Status.BackColor = System.Drawing.Color.LightGreen
		Me.TB_Status.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.TB_Status.ForeColor = System.Drawing.SystemColors.WindowText
		Me.TB_Status.Location = New System.Drawing.Point(385, 34)
		Me.TB_Status.Multiline = True
		Me.TB_Status.Name = "TB_Status"
		Me.TB_Status.ReadOnly = True
		Me.TB_Status.Size = New System.Drawing.Size(135, 40)
		Me.TB_Status.TabIndex = 39
		Me.TB_Status.Text = "Attente..."
		Me.TB_Status.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
		'
		'BP_ToggleAPISimu
		'
		Me.BP_ToggleAPISimu.BackColor = System.Drawing.Color.OrangeRed
		Me.BP_ToggleAPISimu.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.BP_ToggleAPISimu.Location = New System.Drawing.Point(403, 146)
		Me.BP_ToggleAPISimu.Name = "BP_ToggleAPISimu"
		Me.BP_ToggleAPISimu.Size = New System.Drawing.Size(103, 25)
		Me.BP_ToggleAPISimu.TabIndex = 40
		Me.BP_ToggleAPISimu.Text = "Simu API : NON"
		Me.BP_ToggleAPISimu.UseVisualStyleBackColor = False
		'
		'MainForm
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(699, 308)
		Me.Controls.Add(Me.BP_ToggleAPISimu)
		Me.Controls.Add(Me.TB_Status)
		Me.Controls.Add(Me.Label8)
		Me.Controls.Add(Me.Panel_Bac3)
		Me.Controls.Add(Me.Panel_Bac2)
		Me.Controls.Add(Me.Panel_Bac1)
		Me.Controls.Add(Me.BP_RstCounters)
		Me.Controls.Add(Me.TB_history)
		Me.Controls.Add(Me.GroupB_Automate)
		Me.Controls.Add(Me.GroupB_PortCOM)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
		Me.MaximizeBox = False
		Me.Name = "MainForm"
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.Text = "Passerelle RS232 <=> Modbus TCP"
		Me.GroupB_PortCOM.ResumeLayout(False)
		Me.GroupB_Automate.ResumeLayout(False)
		Me.GroupB_Automate.PerformLayout()
		Me.Panel_Bac1.ResumeLayout(False)
		Me.Panel_Bac1.PerformLayout()
		Me.Panel_Bac2.ResumeLayout(False)
		Me.Panel_Bac2.PerformLayout()
		Me.Panel_Bac3.ResumeLayout(False)
		Me.Panel_Bac3.PerformLayout()
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub

	Friend WithEvents GroupB_PortCOM As GroupBox
    Friend WithEvents BP_connectToSP As Button
    Friend WithEvents BP_DéconnexSP As Button
    Friend WithEvents GroupB_Automate As GroupBox
    Friend WithEvents L_EntrerAddrAPI As Label
    Friend WithEvents BP_ValidSockeParams As Button
    Friend WithEvents BP_ModifyParams As Button
    Friend WithEvents L_MW As Label
    Friend WithEvents L_AddrMWAPI As Label
    Friend WithEvents TB_IP_API As TextBox
    Friend WithEvents NotifyIcon1 As NotifyIcon
    Friend WithEvents BP_ModifySocketParams As Button
    Friend WithEvents TB_AddrMotAPI As TextBox
	Friend WithEvents Label1 As Label
	Friend WithEvents Label2 As Label
	Friend WithEvents Label3 As Label
	Protected WithEvents TB_nbrBac1 As TextBox
	Protected WithEvents TB_nbrBac2 As TextBox
	Protected WithEvents TB_nbrBac3 As TextBox
	Friend WithEvents TB_history As TextBox
	Friend WithEvents BP_RstCounters As Button
	Friend WithEvents Panel_Bac1 As Panel
	Friend WithEvents Panel_Bac2 As Panel
	Friend WithEvents Panel_Bac3 As Panel
	Friend WithEvents Label4 As Label
	Friend WithEvents Label5 As Label
	Friend WithEvents Label6 As Label
	Friend WithEvents Label7 As Label
	Friend WithEvents BgWorker_TCPIP As System.ComponentModel.BackgroundWorker
	Friend WithEvents Label8 As Label
	Friend WithEvents TB_Status As TextBox
	Friend WithEvents BP_ToggleAPISimu As Button
End Class
