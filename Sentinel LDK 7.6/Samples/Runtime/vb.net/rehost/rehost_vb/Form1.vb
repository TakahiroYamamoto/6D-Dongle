Imports Aladdin.HASP
Imports Microsoft.VisualC
Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.Drawing
Imports System.IO
Imports System.Windows.Forms
Imports System.Xml
Namespace hasp_rehost
    Public Class Form1
        Inherits Form
        ' Methods
        Public Sub New()
            Me.InitializeComponent()
        End Sub
        ' Sample Vendor Code
        Private vendorCode As String = "AzIceaqfA1hX5wS+M8cGnYh5ceevUnOZIzJBbXFD6dgf3tBkb9cvUF/Tkd/iKu2fsg9wAysYKw7RMAsVvIp4KcXle/v1RaXrLVnNBJ2H2DmrbUMOZbQUFXe698qmJsqNpLXRA367xpZ54i8kC5DTXwDhfxWTOZrBrh5sRKHcoVLumztIQjgWh37AzmSd1bLOfUGI0xjAL9zJWO3fRaeB0NS2KlmoKaVT5Y04zZEc06waU2r6AU2Dc4uipJqJmObqKM+tfNKAS0rZr5IudRiC7pUwnmtaHRe5fgSI8M7yvypvm+13Wm4Gwd4VnYiZvSxf8ImN3ZOG9wEzfyMIlH2+rKPUVHI+igsqla0Wd9m7ZUR9vFotj1uYV0OzG7hX0+huN2E/IdgLDjbiapj1e2fKHrMmGFaIvI6xzzJIQJF9GiRZ7+0jNFLKSyzX/K3JAyFrIPObfwM+y+zAgE1sWcZ1YnuBhICyRHBhaJDKIZL8MywrEfB2yF+R3k9wFG1oN48gSLyfrfEKuB/qgNp+BeTruWUk0AwRE9XVMUuRbjpxa4YA67SKunFEgFGgUfHBeHJTivvUl0u4Dki1UKAT973P+nXy2O0u239If/kRpNUVhMg8kpk7s8i6Arp7l/705/bLCx4kN5hHHSXIqkiG9tHdeNV8VYo5+72hgaCx3/uVoVLmtvxbOIvo120uTJbuLVTvT8KtsOlb3DxwUrwLzaEMoAQAFk6Q9bNipHxfkRQER4kR7IYTMzSoW5mxh3H9O8Ge5BqVeYMEW36q9wnOYfxOLNw6yQMf8f9sJN4KhZty02xm707S7VEfJJ1KNq7b5pP/3RjE0IKtB2gE6vAPRvRLzEohu0m7q1aUp8wAvSiqjZy7FLaTtLEApXYvLvz6PEJdj4TegCZugj7c8bIOEqLXmloZ6EgVnjQ7/ttys7VFITB3mazzFiyQuKf4J6+b/a/Y"


        Private WithEvents OpenH2H As System.Windows.Forms.OpenFileDialog
        Friend WithEvents ButtonRehost As System.Windows.Forms.Button
        Private WithEvents ComboRemoteDestination As System.Windows.Forms.ComboBox
        Friend WithEvents RadioRemote As System.Windows.Forms.RadioButton
        Private WithEvents SaveH2H As System.Windows.Forms.SaveFileDialog
        Friend WithEvents RadioLocal As System.Windows.Forms.RadioButton
        Private WithEvents ComboHaspId As System.Windows.Forms.ComboBox
        Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
        Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
        Private WithEvents label1 As System.Windows.Forms.Label
        Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Dim RehostHaspId As New ArrayList
            Dim info As String = Nothing
            Dim scope As String = "<?xml version=""1.0"" encoding=""UTF-8"" ?><haspscope> <hasp type=""HASP-SL-AdminMode"" /> <hasp type=""HASP-SL-UserMode"" /></haspscope>"

            Dim format As String = "<haspformat root=""haspscope"">    <hasp>       <attribute name=""id"" />  <attribute name=""rehost"" />  <attribute name=""type"" />      </hasp></haspformat>"
            Dim status As Aladdin.HASP.HaspStatus


            ' retrieve accessible Products
            status = Hasp.GetInfo(scope, format, Me.vendorCode, info)
            If (status <> HaspStatus.StatusOk) Then
                MessageBox.Show("Error while retrieving products Errorcode :" + status.ToString())
            Else
                Dim document As New XmlDocument
                document.LoadXml(info)
                Dim nodeList As XmlNodeList = document.DocumentElement.SelectNodes("/haspscope/hasp")
                Dim iCtr As Integer
                ' add each Product to DataSource
                For iCtr = 0 To nodeList.Count - 1
                    Dim attributes As XmlAttributeCollection = nodeList.Item(iCtr).Attributes
                    If attributes("rehost_enduser_managed").Value = "true" Then
                        RehostHaspId.Add(New HaspId(attributes.ItemOf("id").Value + " (" + attributes.ItemOf("type").Value + ") "))
                    End If
                Next iCtr
                If (RehostHaspId.Count > 0) Then
                    Me.ComboHaspId.DataSource = RehostHaspId
                End If
            End If
            If (Me.ComboHaspId.DataSource Is Nothing) Then
                Me.ButtonRehost.Enabled = False
                Else
                    Me.ButtonRehost.Enabled = True
                End If
        End Sub
        Friend WithEvents ButtonAttach As System.Windows.Forms.Button

        Private Sub ButtonRehost_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonRehost.Click
            Dim info As String = Nothing
            Dim scope As String
            Dim status As Aladdin.HASP.HaspStatus

            If Me.RadioRemote.Checked And Me.RadioRemote.Enabled = True Then
                scope = ("<?xml version=""1.0"" encoding=""UTF-8"" ?><haspscope><license_manager hostname=""" & Me.ComboRemoteDestination.Text & """ /></haspscope>")
            Else
                scope = "<?xml version=""1.0"" encoding=""UTF-8"" ?><haspscope>    <license_manager hostname =""localhost"" /></haspscope>"
            End If
            Dim format As String = "<?xml version=""1.0"" encoding=""UTF-8"" ?><haspformat root=""location"">   <license_manager>      <attribute name=""id"" />      <attribute name=""time"" />      <element name=""hostname"" />      <element name=""version"" />      <element name=""host_fingerprint"" />   </license_manager></haspformat>"
            ' get selected rehost recipients
            status = Hasp.GetInfo(scope, format, Me.vendorCode, info)
            If (status <> HaspStatus.StatusOk) Then
                MessageBox.Show("Error while resolve selected destination Errorcode :" + status.ToString())
            End If
            Dim v2c As String = Nothing
            Dim rehost_recipient As String = info
            Dim rehost_action As String = ("<?xml version=""1.0"" encoding=""UTF-8"" ?><rehost><hasp id=""" & DirectCast(Me.ComboHaspId.SelectedItem, HaspId).getId & """ /></rehost>")
            Dim rehost_scope As String = ("<haspscope><hasp id=""" & DirectCast(Me.ComboHaspId.SelectedItem, HaspId).getId & """ /></haspscope>")

            ' rehost V2C license
            status = Hasp.Transfer(rehost_action, rehost_scope, Me.vendorCode, rehost_recipient, v2c)
            If (status <> HaspStatus.StatusOk) Then
                MessageBox.Show("Error while calling hasp_transfer for rehost Errorcode :" + status.ToString())
            ElseIf (Me.SaveH2H.ShowDialog = System.Windows.Forms.DialogResult.OK) Then
                ' write V2C data to file
                Dim writer As New StreamWriter(Me.SaveH2H.FileName)
                writer.Write(v2c)
                writer.Close()
            End If
        End Sub
        Private Sub InitializeComponent()
            Me.SaveH2H = New System.Windows.Forms.SaveFileDialog
            Me.ButtonAttach = New System.Windows.Forms.Button
            Me.ButtonRehost = New System.Windows.Forms.Button
            Me.ComboRemoteDestination = New System.Windows.Forms.ComboBox
            Me.RadioRemote = New System.Windows.Forms.RadioButton
            Me.OpenH2H = New System.Windows.Forms.OpenFileDialog
            Me.RadioLocal = New System.Windows.Forms.RadioButton
            Me.ComboHaspId = New System.Windows.Forms.ComboBox
            Me.label1 = New System.Windows.Forms.Label
            Me.GroupBox1 = New System.Windows.Forms.GroupBox
            Me.GroupBox2 = New System.Windows.Forms.GroupBox
            Me.GroupBox1.SuspendLayout()
            Me.GroupBox2.SuspendLayout()
            Me.SuspendLayout()
            '
            'SaveH2H
            '
            Me.SaveH2H.DefaultExt = "h2h"
            Me.SaveH2H.FileName = "rehost_sample.h2h"
            Me.SaveH2H.Filter = "h2h File|*.h2h|All files|*.*"
            Me.SaveH2H.Title = "Save H2H file"
            '
            'ButtonAttach
            '
            Me.ButtonAttach.Location = New System.Drawing.Point(145, 34)
            Me.ButtonAttach.Name = "ButtonAttach"
            Me.ButtonAttach.Size = New System.Drawing.Size(135, 28)
            Me.ButtonAttach.TabIndex = 52
            Me.ButtonAttach.Text = "Apply License"
            Me.ButtonAttach.UseVisualStyleBackColor = True
            '
            'ButtonRehost
            '
            Me.ButtonRehost.Location = New System.Drawing.Point(145, 134)
            Me.ButtonRehost.Name = "ButtonRehost"
            Me.ButtonRehost.Size = New System.Drawing.Size(135, 28)
            Me.ButtonRehost.TabIndex = 47
            Me.ButtonRehost.Text = "Rehost License"
            Me.ButtonRehost.UseVisualStyleBackColor = True
            '
            'ComboRemoteDestination
            '
            Me.ComboRemoteDestination.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.ComboRemoteDestination.Enabled = False
            Me.ComboRemoteDestination.FormattingEnabled = True
            Me.ComboRemoteDestination.Location = New System.Drawing.Point(228, 65)
            Me.ComboRemoteDestination.Name = "ComboRemoteDestination"
            Me.ComboRemoteDestination.Size = New System.Drawing.Size(262, 21)
            Me.ComboRemoteDestination.TabIndex = 46
            '
            'RadioRemote
            '
            Me.RadioRemote.AutoSize = True
            Me.RadioRemote.Location = New System.Drawing.Point(19, 79)
            Me.RadioRemote.Name = "RadioRemote"
            Me.RadioRemote.Size = New System.Drawing.Size(108, 17)
            Me.RadioRemote.TabIndex = 45
            Me.RadioRemote.Text = "Remote recipient:"
            Me.RadioRemote.UseVisualStyleBackColor = True
            '
            'OpenH2H
            '
            Me.OpenH2H.DefaultExt = "h2h"
            Me.OpenH2H.FileName = "rehost_sample.h2h"
            Me.OpenH2H.Filter = "h2h File|*.h2h|All files|*.*"
            Me.OpenH2H.Title = "Open H2H file"
            '
            'RadioLocal
            '
            Me.RadioLocal.AutoSize = True
            Me.RadioLocal.Checked = True
            Me.RadioLocal.Location = New System.Drawing.Point(19, 56)
            Me.RadioLocal.Name = "RadioLocal"
            Me.RadioLocal.Size = New System.Drawing.Size(188, 17)
            Me.RadioLocal.TabIndex = 44
            Me.RadioLocal.TabStop = True
            Me.RadioLocal.Text = "Local recipient (test purposes only)"
            Me.RadioLocal.UseVisualStyleBackColor = True
            '
            'ComboHaspId
            '
            Me.ComboHaspId.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.ComboHaspId.FormattingEnabled = True
            Me.ComboHaspId.Location = New System.Drawing.Point(228, 16)
            Me.ComboHaspId.Name = "ComboHaspId"
            Me.ComboHaspId.Size = New System.Drawing.Size(262, 21)
            Me.ComboHaspId.TabIndex = 43
            '
            'label1
            '
            Me.label1.AutoSize = True
            Me.label1.Location = New System.Drawing.Point(19, 19)
            Me.label1.Name = "label1"
            Me.label1.Size = New System.Drawing.Size(70, 13)
            Me.label1.TabIndex = 42
            Me.label1.Text = "Select Key Id"
            '
            'GroupBox1
            '
            Me.GroupBox1.Controls.Add(Me.label1)
            Me.GroupBox1.Controls.Add(Me.RadioLocal)
            Me.GroupBox1.Controls.Add(Me.ButtonRehost)
            Me.GroupBox1.Controls.Add(Me.RadioRemote)
            Me.GroupBox1.Controls.Add(Me.ComboRemoteDestination)
            Me.GroupBox1.Controls.Add(Me.ComboHaspId)
            Me.GroupBox1.Location = New System.Drawing.Point(12, 12)
            Me.GroupBox1.Name = "GroupBox1"
            Me.GroupBox1.Size = New System.Drawing.Size(515, 179)
            Me.GroupBox1.TabIndex = 53
            Me.GroupBox1.TabStop = False
            Me.GroupBox1.Text = "Host"
            '
            'GroupBox2
            '
            Me.GroupBox2.Controls.Add(Me.ButtonAttach)
            Me.GroupBox2.Location = New System.Drawing.Point(12, 198)
            Me.GroupBox2.Name = "GroupBox2"
            Me.GroupBox2.Size = New System.Drawing.Size(515, 100)
            Me.GroupBox2.TabIndex = 54
            Me.GroupBox2.TabStop = False
            Me.GroupBox2.Text = "Recipient"
            '
            'Form1
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(546, 308)
            Me.Controls.Add(Me.GroupBox2)
            Me.Controls.Add(Me.GroupBox1)
            Me.Name = "Form1"
            Me.Text = "Sentinel LDK Rehost Program Sample "
            Me.GroupBox1.ResumeLayout(False)
            Me.GroupBox1.PerformLayout()
            Me.GroupBox2.ResumeLayout(False)
            Me.ResumeLayout(False)

        End Sub

        Private Sub RadioLocal_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioLocal.CheckedChanged
            Me.ComboRemoteDestination.Enabled = False

            If (Me.ComboHaspId.DataSource Is Nothing) Then
                Me.ButtonRehost.Enabled = False
            Else
                Me.ButtonRehost.Enabled = True
            End If
        End Sub

        Private Sub RadioRemote_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioRemote.CheckedChanged
            Me.ComboRemoteDestination.Enabled = True
            Dim destinations As New ArrayList
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
                    destinations.Add(node.InnerText)
                Next iCtr
                If (destinations.Count > 0) Then
                    Me.ComboRemoteDestination.DataSource = destinations
                End If
            End If

            If (Me.ComboHaspId.DataSource Is Nothing Or Me.ComboRemoteDestination Is Nothing) Then
                Me.ButtonRehost.Enabled = False
            Else
                Me.ButtonRehost.Enabled = True
            End If
        End Sub

        Private Sub ButtonAttach_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAttach.Click
            ' select V2C file which should be attached
            Dim status As Aladdin.HASP.HaspStatus
            If (Me.OpenH2H.ShowDialog = System.Windows.Forms.DialogResult.OK) Then
                ' read data from V2C file
                Dim reader As New StreamReader(Me.OpenH2H.FileName)
                Dim v2c As String = reader.ReadToEnd
                reader.Close()
                Dim acknowledge As String = Nothing
                ' update/attach retrieved data
                status = Hasp.Update(v2c, acknowledge)
                If (status <> HaspStatus.StatusOk) Then
                    MessageBox.Show("Error while attach license (hasp_update) Errorcode:" + status.ToString())
                End If
            End If
        End Sub

        Private Sub ComboHaspId_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboHaspId.SelectedIndexChanged
            Dim slUsermode As String = "HASP-SL-UserMode"

            If (ComboHaspId.SelectedValue.ToString().IndexOf(slUsermode) = -1) Then

                RadioRemote.Enabled = True
                ComboRemoteDestination.Enabled = True

            Else

                RadioRemote.Enabled = False
                ComboRemoteDestination.Enabled = False

            End If
        End Sub
    End Class
End Namespace

