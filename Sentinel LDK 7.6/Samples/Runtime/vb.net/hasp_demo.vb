''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Copyright (C) 2014, SafeNet, Inc. All rights reserved.
'
'
'
' 
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Imports System.IO
Imports System.Text
Imports Aladdin.HASP


Public Class FormHaspDemo
    Inherits System.Windows.Forms.Form

    Private current As System.Windows.Forms.Cursor

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()
        InitializeComponent()
        current = Nothing
    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents textHistory As System.Windows.Forms.TextBox
    Friend WithEvents groupHistory As System.Windows.Forms.GroupBox
    Friend WithEvents buttonClear As System.Windows.Forms.Button
    Friend WithEvents groupAPIdemo As System.Windows.Forms.GroupBox
    Friend WithEvents groupAccess As System.Windows.Forms.GroupBox
    Friend WithEvents radioBoth As System.Windows.Forms.RadioButton
    Friend WithEvents radioLocal As System.Windows.Forms.RadioButton
    Friend WithEvents buttonRun As System.Windows.Forms.Button
    Friend WithEvents buttonClose As System.Windows.Forms.Button
    Friend WithEvents groupRUS As System.Windows.Forms.GroupBox
    Friend WithEvents buttonV2C As System.Windows.Forms.Button
    Friend WithEvents buttonC2V As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormHaspDemo))
        Me.textHistory = New System.Windows.Forms.TextBox
        Me.groupHistory = New System.Windows.Forms.GroupBox
        Me.buttonClear = New System.Windows.Forms.Button
        Me.groupAPIdemo = New System.Windows.Forms.GroupBox
        Me.groupAccess = New System.Windows.Forms.GroupBox
        Me.radioBoth = New System.Windows.Forms.RadioButton
        Me.radioLocal = New System.Windows.Forms.RadioButton
        Me.buttonRun = New System.Windows.Forms.Button
        Me.buttonClose = New System.Windows.Forms.Button
        Me.groupRUS = New System.Windows.Forms.GroupBox
        Me.buttonV2C = New System.Windows.Forms.Button
        Me.buttonC2V = New System.Windows.Forms.Button
        Me.groupHistory.SuspendLayout()
        Me.groupAPIdemo.SuspendLayout()
        Me.groupAccess.SuspendLayout()
        Me.groupRUS.SuspendLayout()
        Me.SuspendLayout()
        '
        'textHistory
        '
        Me.textHistory.BackColor = System.Drawing.SystemColors.Info
        Me.textHistory.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.textHistory.ForeColor = System.Drawing.SystemColors.InfoText
        Me.textHistory.Location = New System.Drawing.Point(305, 35)
        Me.textHistory.Multiline = True
        Me.textHistory.Name = "textHistory"
        Me.textHistory.ReadOnly = True
        Me.textHistory.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.textHistory.Size = New System.Drawing.Size(384, 264)
        Me.textHistory.TabIndex = 3
        Me.textHistory.WordWrap = False
        '
        'groupHistory
        '
        Me.groupHistory.Controls.Add(Me.buttonClear)
        Me.groupHistory.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.groupHistory.Location = New System.Drawing.Point(289, 11)
        Me.groupHistory.Name = "groupHistory"
        Me.groupHistory.Size = New System.Drawing.Size(416, 344)
        Me.groupHistory.TabIndex = 3
        Me.groupHistory.TabStop = False
        Me.groupHistory.Text = "Demo History"
        '
        'buttonClear
        '
        Me.buttonClear.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.buttonClear.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.buttonClear.Location = New System.Drawing.Point(120, 304)
        Me.buttonClear.Name = "buttonClear"
        Me.buttonClear.Size = New System.Drawing.Size(200, 24)
        Me.buttonClear.TabIndex = 0
        Me.buttonClear.Text = "Clear &History"
        '
        'groupAPIdemo
        '
        Me.groupAPIdemo.Controls.Add(Me.groupAccess)
        Me.groupAPIdemo.Controls.Add(Me.buttonRun)
        Me.groupAPIdemo.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.groupAPIdemo.Location = New System.Drawing.Point(9, 11)
        Me.groupAPIdemo.Name = "groupAPIdemo"
        Me.groupAPIdemo.Size = New System.Drawing.Size(264, 176)
        Me.groupAPIdemo.TabIndex = 1
        Me.groupAPIdemo.TabStop = False
        Me.groupAPIdemo.Text = "API Demo"
        '
        'groupAccess
        '
        Me.groupAccess.Controls.Add(Me.radioBoth)
        Me.groupAccess.Controls.Add(Me.radioLocal)
        Me.groupAccess.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.groupAccess.Location = New System.Drawing.Point(16, 24)
        Me.groupAccess.Name = "groupAccess"
        Me.groupAccess.Size = New System.Drawing.Size(232, 96)
        Me.groupAccess.TabIndex = 0
        Me.groupAccess.TabStop = False
        Me.groupAccess.Text = "Access Mode"
        '
        'radioBoth
        '
        Me.radioBoth.Checked = True
        Me.radioBoth.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.radioBoth.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.radioBoth.Location = New System.Drawing.Point(16, 56)
        Me.radioBoth.Name = "radioBoth"
        Me.radioBoth.Size = New System.Drawing.Size(112, 24)
        Me.radioBoth.TabIndex = 2
        Me.radioBoth.TabStop = True
        Me.radioBoth.Text = "Local and &Network"
        '
        'radioLocal
        '
        Me.radioLocal.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.radioLocal.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.radioLocal.Location = New System.Drawing.Point(16, 24)
        Me.radioLocal.Name = "radioLocal"
        Me.radioLocal.Size = New System.Drawing.Size(104, 24)
        Me.radioLocal.TabIndex = 0
        Me.radioLocal.Text = "&Local only"
        '
        'buttonRun
        '
        Me.buttonRun.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.buttonRun.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.buttonRun.Location = New System.Drawing.Point(28, 136)
        Me.buttonRun.Name = "buttonRun"
        Me.buttonRun.Size = New System.Drawing.Size(208, 24)
        Me.buttonRun.TabIndex = 1
        Me.buttonRun.Text = "&Run Demo"
        '
        'buttonClose
        '
        Me.buttonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.buttonClose.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.buttonClose.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.buttonClose.Location = New System.Drawing.Point(41, 315)
        Me.buttonClose.Name = "buttonClose"
        Me.buttonClose.Size = New System.Drawing.Size(200, 24)
        Me.buttonClose.TabIndex = 0
        Me.buttonClose.Text = "&Close"
        '
        'groupRUS
        '
        Me.groupRUS.Controls.Add(Me.buttonV2C)
        Me.groupRUS.Controls.Add(Me.buttonC2V)
        Me.groupRUS.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.groupRUS.Location = New System.Drawing.Point(9, 203)
        Me.groupRUS.Name = "groupRUS"
        Me.groupRUS.Size = New System.Drawing.Size(264, 96)
        Me.groupRUS.TabIndex = 2
        Me.groupRUS.TabStop = False
        Me.groupRUS.Text = "Remote Update System"
        '
        'buttonV2C
        '
        Me.buttonV2C.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.buttonV2C.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.buttonV2C.Location = New System.Drawing.Point(28, 56)
        Me.buttonV2C.Name = "buttonV2C"
        Me.buttonV2C.Size = New System.Drawing.Size(208, 24)
        Me.buttonV2C.TabIndex = 1
        Me.buttonV2C.Text = "&Update (V2C) ..."
        '
        'buttonC2V
        '
        Me.buttonC2V.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.buttonC2V.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.buttonC2V.Location = New System.Drawing.Point(28, 24)
        Me.buttonC2V.Name = "buttonC2V"
        Me.buttonC2V.Size = New System.Drawing.Size(208, 24)
        Me.buttonC2V.TabIndex = 0
        Me.buttonC2V.Text = "&Generate Status Information (C2V) ..."
        '
        'FormHaspDemo
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(714, 367)
        Me.Controls.Add(Me.textHistory)
        Me.Controls.Add(Me.groupHistory)
        Me.Controls.Add(Me.groupAPIdemo)
        Me.Controls.Add(Me.buttonClose)
        Me.Controls.Add(Me.groupRUS)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "FormHaspDemo"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.Text = "Sentinel LDK Demo for Microsoft Visual Basic .NET"
        Me.groupHistory.ResumeLayout(False)
        Me.groupAPIdemo.ResumeLayout(False)
        Me.groupAccess.ResumeLayout(False)
        Me.groupRUS.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Public Shared Sub Main(ByVal cmdArgs() As String)
        If (cmdArgs.Length > 0 AndAlso cmdArgs(0) = "/q") Then
            Dim demo As HaspDemo = New HaspDemo(Nothing)
            demo.RunDemo(HaspDemo.localScope)
            Return
        End If
        Application.EnableVisualStyles()
        Application.Run(New FormHaspDemo)
    End Sub

    Private WriteOnly Property LockDialog() As Boolean
        Set(ByVal Value As Boolean)
            If ((Not Value) And (Nothing Is current)) Then
                Return
            End If

            If (Value) Then
                current = Me.Cursor
                Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
            Else
                Me.Cursor = current
                current = Nothing
            End If

            Me.groupAPIdemo.Enabled = Not Value
            Me.groupRUS.Enabled = Not Value
            Me.buttonClear.Enabled = Not Value
            Me.Refresh()
        End Set
    End Property

    '"Generate Status Information..." button handler
    Private Sub buttonC2V_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonC2V.Click
        'create an "Save As" dialog instance.
        Dim saveDlg As SaveFileDialog = New SaveFileDialog
        saveDlg.Title = "Save Generated Status Information As"
        saveDlg.Filter = "Customer To Vendor Files (*.c2v)|*.c2v|All Files (*.*)|*.*"
        saveDlg.FilterIndex = 1
        saveDlg.DefaultExt = "c2v"
        saveDlg.FileName = "hasp_demo.c2v"


        If (DialogResult.OK <> saveDlg.ShowDialog()) Then
            Return
        End If

        LockDialog = True

        'get the update information from the key
        Dim c2v As HaspDemoC2v = New HaspDemoC2v(Me.textHistory)
        Dim info As String = c2v.RunDemo()

        LockDialog = False

        If (0 = info.Length) Then
            Return
        End If
        Try
            'save the info in a file
            Dim aStream As Stream = saveDlg.OpenFile()
            If (Nothing Is aStream) Then
                MessageBox.Show(Me, _
                 "Failed to save file " + _
                 ControlChars.Quote + _
                 saveDlg.FileName + _
                 ControlChars.Quote + _
                 ".", _
                 "Save Generated Status Information", _
                 MessageBoxButtons.OK, _
                 MessageBoxIcon.Error)

                Return
            End If

            Dim writer As StreamWriter = New StreamWriter(aStream)
            writer.Write(info)
            writer.Close()
            aStream.Close()

            FormHaspDemo.WriteToTextbox(textHistory, "Status Information written to: " + _
                                ControlChars.Quote + _
                                saveDlg.FileName + _
                                ControlChars.Quote + _
                                ControlChars.CrLf)

        Catch xcpt As System.Exception
            MessageBox.Show(Me, _
                            "Failed to save file.", _
                            "Save Generated Status Information", _
                            MessageBoxButtons.OK, _
                            MessageBoxIcon.Error)
        End Try
    End Sub

    'Clears the "History" text box.
    Private Sub buttonClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonClear.Click
        Me.textHistory.Clear()
    End Sub

    'Closes the dialog
    Private Sub buttonClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonClose.Click
        Me.Close()
    End Sub

    '"Run Demo" button handler
    Private Sub buttonRun_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonRun.Click
        'Create the options
        Dim scope As String = HaspDemo.defaultScope
        If (Me.radioLocal.Checked) Then
            scope = HaspDemo.localScope
        End If

        LockDialog = True

        'run the Sentinel HASP API demo
        Dim demo As HaspDemo = New HaspDemo(Me.textHistory)
        demo.RunDemo(scope)

        LockDialog = False
    End Sub

    '"Update Sentinel HASP..." button handler
    Private Sub buttonV2C_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonV2C.Click
        'create an instance of the "File Open" dialog
        Dim openDlg As OpenFileDialog = New OpenFileDialog
        openDlg.Title = "Open HASP Update File"
        openDlg.Filter = "Vendor To Customer Files (*.v2c)|*.v2c|All Files (*.*)|*.*"
        openDlg.FilterIndex = 1
        openDlg.DefaultExt = "v2c"

        If (DialogResult.OK <> openDlg.ShowDialog()) Then
            Return
        End If

        Dim info As String = Nothing

        Try
            'open the file and read its contents
            Dim aStream As Stream = openDlg.OpenFile()
            If (Nothing Is aStream) Then
                MessageBox.Show(Me, _
                            "Failed to open file " + openDlg.FileName + ".", _
                            "HASP Update", _
                            MessageBoxButtons.OK, _
                            MessageBoxIcon.Error)

                Return
            End If

            Dim reader As StreamReader = New StreamReader(aStream)

            Dim size As Int64 = aStream.Length
            Dim chars As Char() = New Char(size) {}

            reader.Read(chars, 0, chars.Length)
            info = New String(chars)

            reader.Close()
            aStream.Close()
        Catch xpt As System.Exception
            MessageBox.Show(Me, _
                         "Failed to open file.", _
                         "HASP Update", _
                         MessageBoxButtons.OK, _
                         MessageBoxIcon.Error)

            Return
        End Try

        LockDialog = True

        'perform a key update
        Dim v2c As HaspDemoV2c = New HaspDemoV2c(Me.textHistory)
        v2c.RunDemo(info)

        LockDialog = False
    End Sub
    Public Shared Sub WriteToTextbox(ByVal textHistory As TextBox, ByVal text As String)
        If (textHistory Is Nothing) Then
            Console.WriteLine(text)
            Return
        End If
        textHistory.Text += text
        textHistory.SelectionStart = textHistory.TextLength
        textHistory.ScrollToCaret()
        textHistory.Refresh()
    End Sub
End Class
