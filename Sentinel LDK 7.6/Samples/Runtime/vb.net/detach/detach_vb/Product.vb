Imports System

Namespace detach_vb
    Friend Class Product
        ' Methods
        Public Sub New(ByVal aId As String, ByVal aName As String)
            Me.id = aId
            Me.name = aName
        End Sub

        Public Function getId() As String
            Return Me.id
        End Function

        Public Function getName() As String
            Return Me.name
        End Function

        Public Overrides Function ToString() As String
            Return (Me.name & " (" & Me.id & ")")
        End Function


        ' Fields
        Private id As String
        Private name As String
    End Class
End Namespace

