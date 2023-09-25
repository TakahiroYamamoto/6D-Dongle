Imports System
Namespace hasp_rehost
    Friend Class HaspId
        ' Methods
        Public Sub New(ByVal aId As String)
            Me.id = aId
        End Sub

        Public Function getId() As String
            Return Me.id
        End Function

        Public Overrides Function ToString() As String
            Return Me.id
        End Function

        ' Fields
        Private id As String

    End Class

End Namespace