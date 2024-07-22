Imports System.IO
Imports System.Windows.Forms

Public Class Form1

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        lvOrigen.View = View.Details
        lvOrigen.Columns.Add("Path", 100)

        lvOrigen.Columns.Add("Nombre Carpeta", 100)
        lvOrigen.Columns.Add("Tamaño (MB)", 100)

    End Sub

    Private Sub btnOrigen_Click(sender As Object, e As EventArgs) Handles btnOrigen.Click

        Dim fbdGuardar As New FolderBrowserDialog
        Dim origen As String

        If fbdGuardar.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
            origen = fbdGuardar.SelectedPath

            ' Mostrar la carpeta seleccionada en lvOrigen
            Dim item As New ListViewItem(origen)

            item.SubItems.Add(IO.Path.GetFileName(origen))

            ' Calcular y mostrar el tamaño de la carpeta en MB
            Dim sizeInMB As Double = GetDirectorySize(origen)
            item.SubItems.Add(sizeInMB.ToString("N2"))

            lvOrigen.Items.Add(item)

        End If

    End Sub

    ' Función para calcular el tamaño total de una carpeta y subcarpetas
    Private Function GetDirectorySize(ByVal dirPath As String) As Double
        Dim size As Long = 0

        ' Obtener archivos en la carpeta actual
        Dim files As String() = Directory.GetFiles(dirPath)
        For Each file As String In files
            Dim fileInfo As New FileInfo(file)
            size += fileInfo.Length
        Next

        ' Obtener subcarpetas y recursivamente calcular su tamaño
        Dim subDirs As String() = Directory.GetDirectories(dirPath)
        For Each subDir As String In subDirs
            size += GetDirectorySize(subDir)
        Next

        ' Convertir de bytes a megabytes
        Dim sizeInMB As Double = size / (1024.0 * 1024.0)

        Return sizeInMB
    End Function

    Private Sub btnCopiar_Click(sender As Object, e As EventArgs) Handles btnCopiar.Click

        For Each item As ListViewItem In lvOrigen.Items
            ' Verificamos si el checkbox está marcado para este elemento
            If item.Checked Then
                ' Agregamos el texto del elemento a alguna estructura de datos o hacemos algo con él
                MsgBox(item.Text) ' Por ejemplo, mostrar en un MessageBox
            End If
        Next

    End Sub
End Class
