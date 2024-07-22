Imports System.Windows.Forms.VisualStyles.VisualStyleElement

Public Class Form1
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'lvOrigen.Dock = DockStyle.Fill
        lvOrigen.View = View.Details
        lvOrigen.Columns.Add("", 40)
        lvOrigen.Columns.Add("Directorio", 129, HorizontalAlignment.Left)
        lvOrigen.Columns.Add("Path", 120, HorizontalAlignment.Left)
        lvOrigen.Columns.Add("Path", 120, HorizontalAlignment.Left)
    End Sub

    Private Sub btnAgregarArchivo_Click(sender As Object, e As EventArgs) Handles btnAgregarArchivo.Click
        Dim fbdDirectorio As New FolderBrowserDialog

        If fbdDirectorio.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
            AgregarDirectorio(fbdDirectorio.SelectedPath)
        End If
    End Sub


    Private Function AgregarDirectorio(ByVal path As String)

        Dim lvItem As New ListViewItem

        lvItem.SubItems.Add(IO.Path.GetFileName(path))
        lvItem.SubItems.Add(path)
        lvItem.SubItems.Add(23)

        lvOrigen.Items.Add(lvItem)

    End Function
End Class
