Imports System
Imports System.Windows.Forms

Namespace detach_vb
    Friend Class Program
        ' Methods
        <STAThread> _
        Private Shared Sub Main()
            Application.EnableVisualStyles
            Application.SetCompatibleTextRenderingDefault(False)
            Application.Run(New Form1)
        End Sub

    End Class
End Namespace

