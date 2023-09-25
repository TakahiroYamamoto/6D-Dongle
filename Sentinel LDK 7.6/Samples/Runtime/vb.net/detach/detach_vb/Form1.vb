Imports Aladdin.HASP
Imports Microsoft.VisualC
Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.Drawing
Imports System.IO
Imports System.Windows.Forms
Imports System.Xml

Namespace detach_vb
    Public Class Form1
        Inherits Form
        ' Methods
        Public Sub New()
            Me.InitializeComponent()
        End Sub

        ' "Attach License" button
        Private Sub ButtonAttach_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonAttach.Click
            Dim status As Aladdin.HASP.HaspStatus
            ' select H2R file which should be attached
            If (Me.OpenH2R.ShowDialog = System.Windows.Forms.DialogResult.OK) Then
                ' read data from H2R file
                Dim reader As New StreamReader(Me.OpenH2R.FileName)
                Dim h2r As String = reader.ReadToEnd
                reader.Close()
                Dim acknowledge As String = Nothing
                ' update/attach retrieved data
                status = Hasp.Update(h2r, acknowledge)
                If (status <> HaspStatus.StatusOk) Then
                    MessageBox.Show("Error while attach license (hasp_update)Errorcode :" + status.ToString())
                End If
            End If
        End Sub

        ' "Detach License" button
        Private Sub ButtonDetach_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ButtonDetach.Click
            Dim info As String = Nothing
            Dim scope As String
            Dim status As Aladdin.HASP.HaspStatus
            If Me.RadioRemote.Checked Then
                scope = ("<?xml version=""1.0"" encoding=""UTF-8"" ?><haspscope><license_manager hostname=""" & Me.ComboRemoteDestination.Text & """ /></haspscope>")
            Else
                scope = "<?xml version=""1.0"" encoding=""UTF-8"" ?><haspscope>    <license_manager hostname =""localhost"" /></haspscope>"
            End If
            Dim format As String = "<?xml version=""1.0"" encoding=""UTF-8"" ?><haspformat root=""location"">   <license_manager>      <attribute name=""id"" />      <attribute name=""time"" />      <element name=""hostname"" />      <element name=""version"" />      <element name=""host_fingerprint"" />   </license_manager></haspformat>"
            ' get selected detach recipients
            status = Hasp.GetInfo(scope, format, Me.vendorCode, info)
            If (status <> HaspStatus.StatusOk) Then
                MessageBox.Show("Error while resolve selected destination Errorcode :" + status.ToString())
            End If
            If IsNothing(Me.ComboProduct.SelectedItem) Then
                MessageBox.Show("No product selected for detaching!")
                Return
            End If
            Dim h2r As String = Nothing
            Dim detach_recipient As String = info
            Dim detach_action As String = ("<?xml version=""1.0"" encoding=""UTF-8"" ?><detach><duration>" & Me.NumericDuration.Value & "</duration></detach>")
            Dim detach_scope As String = ("<haspscope><product id=""" & DirectCast(Me.ComboProduct.SelectedItem, Product).getId & """ /></haspscope>")
            ' detach H2R license


            status = Hasp.Transfer(detach_action, detach_scope, Me.vendorCode, detach_recipient, h2r)
            If (status <> HaspStatus.StatusOk) Then
                MessageBox.Show("Error while calling hasp_detach Errorcode:" + status.ToString())
            ElseIf (Me.SaveH2R.ShowDialog = System.Windows.Forms.DialogResult.OK) Then
                ' write H2R data to file
                Dim writer As New StreamWriter(Me.SaveH2R.FileName)
                writer.Write(h2r)
                writer.Close()
            End If
        End Sub

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If (disposing AndAlso (Not Me.components Is Nothing)) Then
                Me.components.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

        ' load handler for main form
        Private Sub Form1_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
            Dim DetachableProducts As New ArrayList
            Dim info As String = Nothing
            Dim status As Aladdin.HASP.HaspStatus
            Dim scope As String = "<?xml version=""1.0"" encoding=""UTF-8"" ?><haspscope> <hasp type=""HASP-SL-AdminMode"" /> <hasp type=""HASP-SL-UserMode"" /></haspscope>"

            Dim format As String = "<haspformat root=""haspscope"">    <hasp>       <attribute name=""id"" />  <attribute name=""detachable"" />      </hasp></haspformat>"
            ' retrieve accessible Products
            status = Hasp.GetInfo(scope, format, Me.vendorCode, info)
            If (status <> HaspStatus.StatusOk) Then
                MessageBox.Show("HASP Error while retrieving products Errorcode:" + status.ToString())
            Else
                Dim document As New XmlDocument
                document.LoadXml(info)
                Dim nodeList As XmlNodeList = document.DocumentElement.SelectNodes("/haspscope/product")
                Dim iCtr As Integer
                ' add each Product to DataSource
                For iCtr = 0 To nodeList.Count - 1
                    Dim attributes As XmlAttributeCollection = nodeList.Item(iCtr).Attributes
                    If attributes("detachable").Value = "true" Then
                        DetachableProducts.Add(New Product(attributes.ItemOf("id").Value, attributes.ItemOf("name").Value))
                    End If
                Next iCtr
                Me.ComboProduct.DataSource = DetachableProducts
            End If
            If (Me.ComboProduct.DataSource Is Nothing) Then
                Me.ButtonDetach.Enabled = False
            Else
                Me.ButtonDetach.Enabled = True
            End If

        End Sub

        Private Sub InitializeComponent()
            Me.OpenH2R = New System.Windows.Forms.OpenFileDialog
            Me.SaveH2R = New System.Windows.Forms.SaveFileDialog
            Me.label5 = New System.Windows.Forms.Label
            Me.NumericDuration = New System.Windows.Forms.NumericUpDown
            Me.ButtonAttach = New System.Windows.Forms.Button
            Me.ButtonDetach = New System.Windows.Forms.Button
            Me.ComboRemoteDestination = New System.Windows.Forms.ComboBox
            Me.RadioRemote = New System.Windows.Forms.RadioButton
            Me.RadioLocal = New System.Windows.Forms.RadioButton
            Me.ComboProduct = New System.Windows.Forms.ComboBox
            Me.label1 = New System.Windows.Forms.Label
            Me.GroupBox1 = New System.Windows.Forms.GroupBox
            Me.GroupBox2 = New System.Windows.Forms.GroupBox
            CType(Me.NumericDuration, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.GroupBox1.SuspendLayout()
            Me.GroupBox2.SuspendLayout()
            Me.SuspendLayout()
            '
            'OpenH2R
            '
            Me.OpenH2R.DefaultExt = "h2r"
            Me.OpenH2R.FileName = "detach_sample.h2r"
            Me.OpenH2R.Filter = "h2r File|*.h2r|All files|*.*"
            Me.OpenH2R.Title = "Open H2R file"
            '
            'SaveH2R
            '
            Me.SaveH2R.DefaultExt = "h2r"
            Me.SaveH2R.FileName = "detach_sample.h2r"
            Me.SaveH2R.Filter = "h2r File|*.h2r|All files|*.*"
            Me.SaveH2R.Title = "Save H2R file"
            '
            'label5
            '
            Me.label5.AutoSize = True
            Me.label5.Location = New System.Drawing.Point(6, 140)
            Me.label5.Name = "label5"
            Me.label5.Size = New System.Drawing.Size(114, 13)
            Me.label5.TabIndex = 23
            Me.label5.Text = "Detach Duration (sec):"
            '
            'NumericDuration
            '
            Me.NumericDuration.AccessibleDescription = ""
            Me.NumericDuration.AccessibleName = ""
            Me.NumericDuration.Location = New System.Drawing.Point(136, 133)
            Me.NumericDuration.Maximum = New Decimal(New Integer() {1000000, 0, 0, 0})
            Me.NumericDuration.Minimum = New Decimal(New Integer() {60, 0, 0, 0})
            Me.NumericDuration.Name = "NumericDuration"
            Me.NumericDuration.Size = New System.Drawing.Size(86, 20)
            Me.NumericDuration.TabIndex = 24
            Me.NumericDuration.Tag = ""
            Me.NumericDuration.ThousandsSeparator = True
            Me.NumericDuration.Value = New Decimal(New Integer() {60, 0, 0, 0})
            '
            'ButtonAttach
            '
            Me.ButtonAttach.Location = New System.Drawing.Point(173, 33)
            Me.ButtonAttach.Name = "ButtonAttach"
            Me.ButtonAttach.Size = New System.Drawing.Size(135, 28)
            Me.ButtonAttach.TabIndex = 19
            Me.ButtonAttach.Text = "Attach License"
            Me.ButtonAttach.UseVisualStyleBackColor = True
            '
            'ButtonDetach
            '
            Me.ButtonDetach.Location = New System.Drawing.Point(290, 132)
            Me.ButtonDetach.Name = "ButtonDetach"
            Me.ButtonDetach.Size = New System.Drawing.Size(135, 28)
            Me.ButtonDetach.TabIndex = 18
            Me.ButtonDetach.Text = "Detach License"
            Me.ButtonDetach.UseVisualStyleBackColor = True
            '
            'ComboRemoteDestination
            '
            Me.ComboRemoteDestination.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.ComboRemoteDestination.Enabled = False
            Me.ComboRemoteDestination.FormattingEnabled = True
            Me.ComboRemoteDestination.Location = New System.Drawing.Point(232, 87)
            Me.ComboRemoteDestination.Name = "ComboRemoteDestination"
            Me.ComboRemoteDestination.Size = New System.Drawing.Size(235, 21)
            Me.ComboRemoteDestination.TabIndex = 17
            '
            'RadioRemote
            '
            Me.RadioRemote.AutoSize = True
            Me.RadioRemote.Location = New System.Drawing.Point(6, 91)
            Me.RadioRemote.Name = "RadioRemote"
            Me.RadioRemote.Size = New System.Drawing.Size(108, 17)
            Me.RadioRemote.TabIndex = 16
            Me.RadioRemote.Text = "Remote recipient:"
            Me.RadioRemote.UseVisualStyleBackColor = True
            '
            'RadioLocal
            '
            Me.RadioLocal.AutoSize = True
            Me.RadioLocal.Checked = True
            Me.RadioLocal.Location = New System.Drawing.Point(6, 68)
            Me.RadioLocal.Name = "RadioLocal"
            Me.RadioLocal.Size = New System.Drawing.Size(188, 17)
            Me.RadioLocal.TabIndex = 15
            Me.RadioLocal.TabStop = True
            Me.RadioLocal.Text = "Local recipient (test purposes only)"
            Me.RadioLocal.UseVisualStyleBackColor = True
            '
            'ComboProduct
            '
            Me.ComboProduct.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.ComboProduct.FormattingEnabled = True
            Me.ComboProduct.Location = New System.Drawing.Point(232, 32)
            Me.ComboProduct.Name = "ComboProduct"
            Me.ComboProduct.Size = New System.Drawing.Size(235, 21)
            Me.ComboProduct.TabIndex = 14
            '
            'label1
            '
            Me.label1.AutoSize = True
            Me.label1.Location = New System.Drawing.Point(6, 32)
            Me.label1.Name = "label1"
            Me.label1.Size = New System.Drawing.Size(136, 13)
            Me.label1.TabIndex = 13
            Me.label1.Text = "Select detachable Product:"
            '
            'GroupBox1
            '
            Me.GroupBox1.Controls.Add(Me.label1)
            Me.GroupBox1.Controls.Add(Me.RadioLocal)
            Me.GroupBox1.Controls.Add(Me.ButtonDetach)
            Me.GroupBox1.Controls.Add(Me.NumericDuration)
            Me.GroupBox1.Controls.Add(Me.label5)
            Me.GroupBox1.Controls.Add(Me.RadioRemote)
            Me.GroupBox1.Controls.Add(Me.ComboProduct)
            Me.GroupBox1.Controls.Add(Me.ComboRemoteDestination)
            Me.GroupBox1.Location = New System.Drawing.Point(12, 6)
            Me.GroupBox1.Name = "GroupBox1"
            Me.GroupBox1.Size = New System.Drawing.Size(494, 190)
            Me.GroupBox1.TabIndex = 25
            Me.GroupBox1.TabStop = False
            Me.GroupBox1.Text = "Host"
            '
            'GroupBox2
            '
            Me.GroupBox2.Controls.Add(Me.ButtonAttach)
            Me.GroupBox2.Location = New System.Drawing.Point(12, 202)
            Me.GroupBox2.Name = "GroupBox2"
            Me.GroupBox2.Size = New System.Drawing.Size(504, 98)
            Me.GroupBox2.TabIndex = 26
            Me.GroupBox2.TabStop = False
            Me.GroupBox2.Text = "Recipient"
            '
            'Form1
            '
            Me.ClientSize = New System.Drawing.Size(518, 312)
            Me.Controls.Add(Me.GroupBox2)
            Me.Controls.Add(Me.GroupBox1)
            Me.MaximizeBox = False
            Me.Name = "Form1"
            Me.Text = "Sentinel LDK Detach Product Sample"
            CType(Me.NumericDuration, System.ComponentModel.ISupportInitialize).EndInit()
            Me.GroupBox1.ResumeLayout(False)
            Me.GroupBox1.PerformLayout()
            Me.GroupBox2.ResumeLayout(False)
            Me.ResumeLayout(False)

        End Sub

        ' "Local recipient (test purposes only)" radio button
        Private Sub RadioLocal_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles RadioLocal.CheckedChanged
            Me.ComboRemoteDestination.Enabled = False

            If (Me.ComboProduct.DataSource Is Nothing) Then
                Me.ButtonDetach.Enabled = False
            Else
                Me.ButtonDetach.Enabled = True
            End If
        End Sub

        ' "Remote recipient" radio button
        Private Sub RadioRemote_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles RadioRemote.CheckedChanged
            Me.ComboRemoteDestination.Enabled = True
            Dim destionations As New ArrayList
            Dim info As String = Nothing
            Dim status As Aladdin.HASP.HaspStatus
            Dim scope As String = "<?xml version=""1.0"" encoding=""UTF-8"" ?><haspscope/>"
            Dim format As String = "<?xml version=""1.0"" encoding=""UTF-8"" ?><haspformat root=""location"">   <license_manager>      <attribute name=""id"" />      <attribute name=""time"" />      <element name=""hostname"" />      <element name=""version"" />      <element name=""host_fingerprint"" />   </license_manager></haspformat>"
            ' retrieve XML list of accessible recipients
            status = Hasp.GetInfo(scope, format, Me.vendorCode, info)
            If (status <> HaspStatus.StatusOk) Then
                MessageBox.Show("Error while getting destinations Errorcode:" + status.ToString())
            Else
                Dim document As New XmlDocument
                document.LoadXml(info)
                Dim nodeList As XmlNodeList = document.DocumentElement.SelectNodes("/location/license_manager/hostname")
                Dim iCtr As Integer
                ' extract hostname for each recipient in the XML result
                For iCtr = 0 To nodeList.Count - 1
                    Dim node As XmlNode = nodeList.Item(iCtr)
                    destionations.Add(node.InnerText)
                Next iCtr
                Me.ComboRemoteDestination.DataSource = destionations
            End If

            If (Me.ComboProduct.DataSource Is Nothing Or Me.ComboRemoteDestination Is Nothing) Then
                Me.ButtonDetach.Enabled = False
            Else
                Me.ButtonDetach.Enabled = True
            End If
        End Sub


        ' Fields
        Private ComboProduct As ComboBox
        Private ComboRemoteDestination As ComboBox
        Private components As IContainer
        Private label1 As Label
        Private label5 As Label
        Private NumericDuration As NumericUpDown
        Private OpenH2R As OpenFileDialog
        Private SaveH2R As SaveFileDialog
        
        ' Sample Vendor Code
        Private vendorCode As String = "AzIceaqfA1hX5wS+M8cGnYh5ceevUnOZIzJBbXFD6dgf3tBkb9cvUF/Tkd/iKu2fsg9wAysYKw7RMAsVvIp4KcXle/v1RaXrLVnNBJ2H2DmrbUMOZbQUFXe698qmJsqNpLXRA367xpZ54i8kC5DTXwDhfxWTOZrBrh5sRKHcoVLumztIQjgWh37AzmSd1bLOfUGI0xjAL9zJWO3fRaeB0NS2KlmoKaVT5Y04zZEc06waU2r6AU2Dc4uipJqJmObqKM+tfNKAS0rZr5IudRiC7pUwnmtaHRe5fgSI8M7yvypvm+13Wm4Gwd4VnYiZvSxf8ImN3ZOG9wEzfyMIlH2+rKPUVHI+igsqla0Wd9m7ZUR9vFotj1uYV0OzG7hX0+huN2E/IdgLDjbiapj1e2fKHrMmGFaIvI6xzzJIQJF9GiRZ7+0jNFLKSyzX/K3JAyFrIPObfwM+y+zAgE1sWcZ1YnuBhICyRHBhaJDKIZL8MywrEfB2yF+R3k9wFG1oN48gSLyfrfEKuB/qgNp+BeTruWUk0AwRE9XVMUuRbjpxa4YA67SKunFEgFGgUfHBeHJTivvUl0u4Dki1UKAT973P+nXy2O0u239If/kRpNUVhMg8kpk7s8i6Arp7l/705/bLCx4kN5hHHSXIqkiG9tHdeNV8VYo5+72hgaCx3/uVoVLmtvxbOIvo120uTJbuLVTvT8KtsOlb3DxwUrwLzaEMoAQAFk6Q9bNipHxfkRQER4kR7IYTMzSoW5mxh3H9O8Ge5BqVeYMEW36q9wnOYfxOLNw6yQMf8f9sJN4KhZty02xm707S7VEfJJ1KNq7b5pP/3RjE0IKtB2gE6vAPRvRLzEohu0m7q1aUp8wAvSiqjZy7FLaTtLEApXYvLvz6PEJdj4TegCZugj7c8bIOEqLXmloZ6EgVnjQ7/ttys7VFITB3mazzFiyQuKf4J6+b/a/Y"

        Friend WithEvents ButtonAttach As System.Windows.Forms.Button
        Friend WithEvents ButtonDetach As System.Windows.Forms.Button
        Friend WithEvents RadioLocal As System.Windows.Forms.RadioButton
        Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
        Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
        Friend WithEvents RadioRemote As System.Windows.Forms.RadioButton
    End Class
End Namespace

