''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Copyright (C) 2014, SafeNet, Inc. All rights reserved.
'
'
'
' 
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Imports System
Imports Aladdin.HASP


Public Class HaspDemoC2v
    Inherits HaspDemo

    Public Sub New(ByVal textHistory As System.Windows.Forms.TextBox)
        MyBase.New(textHistory)
    End Sub

    'Prints the footer message into the 
    'referenced TextBox
    Protected Overloads Sub Footer()
        FormHaspDemo.WriteToTextbox(textHistory, "Generation of Status Information completed" + _
            ControlChars.CrLf)

    End Sub

    'Prints the header into the 
    'referenced(TextBox)
    Protected Overloads Sub Header()
        If (textHistory IsNot Nothing AndAlso &HFFFF < textHistory.TextLength) Then
            textHistory.Clear()
        End If

        FormHaspDemo.WriteToTextbox(textHistory, "____________________________________________________________" + _
            ControlChars.CrLf + _
            String.Format("Generation of Status Information started ({0})", _
                                          DateTime.Now.ToString()) + _
            ControlChars.CrLf + ControlChars.CrLf)
    End Sub

    'Performs a login using the default feature
    'and retrieves the update information using
    'Sentinel Hasp's GetInfo method.
    Public Overloads Function RunDemo() As String
        Header()

        Dim info As String = ""

        Verbose("Retrieving Update Information")

        'now get the update information
        Dim status As HaspStatus = Hasp.GetInfo(localScope, Hasp.UpdateInfo, VendorCode.Code, info)
        ReportStatus(status)

        If (HaspStatus.StatusOk = status) Then
            Verbose(info.Replace("\n", "\r\n     "))
        Else
            Verbose("")
        End If

        Footer()
        Return info
    End Function
End Class
