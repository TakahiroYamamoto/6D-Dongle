''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Copyright (C) 2014, SafeNet, Inc. All rights reserved.
'
'
'
' 
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Imports System
Imports Aladdin.HASP


Public Class HaspDemoV2c
    Inherits HaspDemo

    Public Sub New(ByVal textHistory As System.Windows.Forms.TextBox)
        MyBase.New(textHistory)
    End Sub

    'Prints the footer message into the
    'referenced TextBox
    Protected Overloads Sub Footer()
        FormHaspDemo.WriteToTextbox(textHistory, "Sentinel LDK Update completed" + ControlChars.CrLf)
    End Sub

    'Prints the header message into the
    'referenced TextBox
    Protected Overloads Sub Header()
        If (textHistory IsNot Nothing AndAlso &HFFFF < textHistory.TextLength) Then
            textHistory.Clear()
        End If

        FormHaspDemo.WriteToTextbox(textHistory, "____________________________________________________________" + _
            ControlChars.CrLf + _
            String.Format("Sentinel LDK Update started ({0})", _
                                    DateTime.Now.ToString()) + _
            ControlChars.CrLf + ControlChars.CrLf)

    End Sub

    'Updates the key using the passed update string
    'and writes the returned acknowledge (if available)
    'into the referenced TextBox.
    Public Overloads Sub RunDemo(ByVal update As String)
        Header()

        'print the update string
        Verbose("Update information:")
        Verbose(update.Replace(ControlChars.Lf, _
                ControlChars.CrLf + "    "))
        Verbose("")

        Dim ack As String = Nothing

        'perform the update
        'please note that the Hasp's Update method is
        'static.
        Dim status As HaspStatus = Hasp.Update(update, ack)
        ReportStatus(status)

        If (HaspStatus.StatusOk = status) Then
            'print the acknowledgement
            'the return of an ack. is controlled via the
            'update package.
            Verbose("Acknowledge information:")

            If (String.IsNullOrEmpty(ack)) Then
                Verbose("Not available")
            Else
                Verbose(ack.Replace(ControlChars.Lf, _
                                    ControlChars.CrLf + "     "))
            End If
        End If

        Verbose("")
        Footer()
    End Sub
End Class
