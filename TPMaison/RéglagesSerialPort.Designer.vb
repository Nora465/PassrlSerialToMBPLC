<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class RéglagesSerialPort
    Inherits System.Windows.Forms.Form

    'Form remplace la méthode Dispose pour nettoyer la liste des composants.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
		Me.TBD_SPName = New System.Windows.Forms.ComboBox()
		Me.BP_ValidParamSP = New System.Windows.Forms.Button()
		Me.TBD_SPBauds = New System.Windows.Forms.ComboBox()
		Me.L_SPName = New System.Windows.Forms.Label()
		Me.TBD_SPParity = New System.Windows.Forms.ComboBox()
		Me.L_SPBauds = New System.Windows.Forms.Label()
		Me.TBD_SPStopBits = New System.Windows.Forms.ComboBox()
		Me.L_SPParity = New System.Windows.Forms.Label()
		Me.TBD_SPDataBits = New System.Windows.Forms.ComboBox()
		Me.L_SPStopBits = New System.Windows.Forms.Label()
		Me.L_SPDataBits = New System.Windows.Forms.Label()
		Me.GroupB_RéglageSP = New System.Windows.Forms.GroupBox()
		Me.GroupB_RéglageSP.SuspendLayout()
		Me.SuspendLayout()
		'
		'TBD_SPName
		'
		Me.TBD_SPName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.TBD_SPName.FormattingEnabled = True
		Me.TBD_SPName.Location = New System.Drawing.Point(151, 25)
		Me.TBD_SPName.Name = "TBD_SPName"
		Me.TBD_SPName.Size = New System.Drawing.Size(140, 21)
		Me.TBD_SPName.TabIndex = 4
		'
		'BP_ValidParamSP
		'
		Me.BP_ValidParamSP.Location = New System.Drawing.Point(80, 190)
		Me.BP_ValidParamSP.Name = "BP_ValidParamSP"
		Me.BP_ValidParamSP.Size = New System.Drawing.Size(140, 37)
		Me.BP_ValidParamSP.TabIndex = 5
		Me.BP_ValidParamSP.Text = "VALIDER CETTE CONFIGURATION"
		Me.BP_ValidParamSP.UseVisualStyleBackColor = True
		'
		'TBD_SPBauds
		'
		Me.TBD_SPBauds.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.TBD_SPBauds.FormattingEnabled = True
		Me.TBD_SPBauds.Items.AddRange(New Object() {"1200", "2400", "4800", "9600", "19220"})
		Me.TBD_SPBauds.Location = New System.Drawing.Point(151, 56)
		Me.TBD_SPBauds.Name = "TBD_SPBauds"
		Me.TBD_SPBauds.Size = New System.Drawing.Size(140, 21)
		Me.TBD_SPBauds.TabIndex = 8
		'
		'L_SPName
		'
		Me.L_SPName.AutoSize = True
		Me.L_SPName.Location = New System.Drawing.Point(20, 28)
		Me.L_SPName.Name = "L_SPName"
		Me.L_SPName.Size = New System.Drawing.Size(107, 13)
		Me.L_SPName.TabIndex = 0
		Me.L_SPName.Text = "Numéro du port COM"
		'
		'TBD_SPParity
		'
		Me.TBD_SPParity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.TBD_SPParity.FormattingEnabled = True
		Me.TBD_SPParity.Items.AddRange(New Object() {"Sans Parité", "Impaire", "Paire"})
		Me.TBD_SPParity.Location = New System.Drawing.Point(151, 87)
		Me.TBD_SPParity.Name = "TBD_SPParity"
		Me.TBD_SPParity.Size = New System.Drawing.Size(140, 21)
		Me.TBD_SPParity.TabIndex = 10
		'
		'L_SPBauds
		'
		Me.L_SPBauds.AutoSize = True
		Me.L_SPBauds.Location = New System.Drawing.Point(16, 59)
		Me.L_SPBauds.Name = "L_SPBauds"
		Me.L_SPBauds.Size = New System.Drawing.Size(116, 13)
		Me.L_SPBauds.TabIndex = 9
		Me.L_SPBauds.Text = "Vitesse de transmission"
		'
		'TBD_SPStopBits
		'
		Me.TBD_SPStopBits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.TBD_SPStopBits.FormattingEnabled = True
		Me.TBD_SPStopBits.Items.AddRange(New Object() {"1", "2"})
		Me.TBD_SPStopBits.Location = New System.Drawing.Point(151, 149)
		Me.TBD_SPStopBits.Name = "TBD_SPStopBits"
		Me.TBD_SPStopBits.Size = New System.Drawing.Size(140, 21)
		Me.TBD_SPStopBits.TabIndex = 11
		'
		'L_SPParity
		'
		Me.L_SPParity.AutoSize = True
		Me.L_SPParity.Location = New System.Drawing.Point(57, 90)
		Me.L_SPParity.Name = "L_SPParity"
		Me.L_SPParity.Size = New System.Drawing.Size(34, 13)
		Me.L_SPParity.TabIndex = 7
		Me.L_SPParity.Text = "Parité"
		'
		'TBD_SPDataBits
		'
		Me.TBD_SPDataBits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.TBD_SPDataBits.FormattingEnabled = True
		Me.TBD_SPDataBits.Items.AddRange(New Object() {"7", "8"})
		Me.TBD_SPDataBits.Location = New System.Drawing.Point(151, 118)
		Me.TBD_SPDataBits.Name = "TBD_SPDataBits"
		Me.TBD_SPDataBits.Size = New System.Drawing.Size(140, 21)
		Me.TBD_SPDataBits.TabIndex = 12
		'
		'L_SPStopBits
		'
		Me.L_SPStopBits.AutoSize = True
		Me.L_SPStopBits.Location = New System.Drawing.Point(11, 152)
		Me.L_SPStopBits.Name = "L_SPStopBits"
		Me.L_SPStopBits.Size = New System.Drawing.Size(126, 13)
		Me.L_SPStopBits.TabIndex = 13
		Me.L_SPStopBits.Text = "Nombre de Bits de STOP"
		'
		'L_SPDataBits
		'
		Me.L_SPDataBits.AutoSize = True
		Me.L_SPDataBits.Location = New System.Drawing.Point(9, 121)
		Me.L_SPDataBits.Name = "L_SPDataBits"
		Me.L_SPDataBits.Size = New System.Drawing.Size(138, 13)
		Me.L_SPDataBits.TabIndex = 14
		Me.L_SPDataBits.Text = "Nombre de Bits de données"
		'
		'GroupB_RéglageSP
		'
		Me.GroupB_RéglageSP.Controls.Add(Me.L_SPDataBits)
		Me.GroupB_RéglageSP.Controls.Add(Me.L_SPStopBits)
		Me.GroupB_RéglageSP.Controls.Add(Me.TBD_SPDataBits)
		Me.GroupB_RéglageSP.Controls.Add(Me.L_SPParity)
		Me.GroupB_RéglageSP.Controls.Add(Me.TBD_SPStopBits)
		Me.GroupB_RéglageSP.Controls.Add(Me.L_SPBauds)
		Me.GroupB_RéglageSP.Controls.Add(Me.TBD_SPParity)
		Me.GroupB_RéglageSP.Controls.Add(Me.L_SPName)
		Me.GroupB_RéglageSP.Controls.Add(Me.TBD_SPBauds)
		Me.GroupB_RéglageSP.Controls.Add(Me.BP_ValidParamSP)
		Me.GroupB_RéglageSP.Controls.Add(Me.TBD_SPName)
		Me.GroupB_RéglageSP.Location = New System.Drawing.Point(35, 6)
		Me.GroupB_RéglageSP.Name = "GroupB_RéglageSP"
		Me.GroupB_RéglageSP.Size = New System.Drawing.Size(308, 241)
		Me.GroupB_RéglageSP.TabIndex = 14
		Me.GroupB_RéglageSP.TabStop = False
		'
		'RéglagesSerialPort
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(383, 256)
		Me.Controls.Add(Me.GroupB_RéglageSP)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "RéglagesSerialPort"
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
		Me.Text = "Réglage des paramètres du Port Série COMx"
		Me.GroupB_RéglageSP.ResumeLayout(False)
		Me.GroupB_RéglageSP.PerformLayout()
		Me.ResumeLayout(False)

	End Sub

	Friend WithEvents TBD_SPName As ComboBox
    Friend WithEvents BP_ValidParamSP As Button
    Friend WithEvents TBD_SPBauds As ComboBox
    Friend WithEvents L_SPName As Label
    Friend WithEvents TBD_SPParity As ComboBox
    Friend WithEvents L_SPBauds As Label
    Friend WithEvents TBD_SPStopBits As ComboBox
    Friend WithEvents L_SPParity As Label
    Friend WithEvents TBD_SPDataBits As ComboBox
    Friend WithEvents L_SPStopBits As Label
    Friend WithEvents L_SPDataBits As Label
    Friend WithEvents GroupB_RéglageSP As GroupBox
End Class
