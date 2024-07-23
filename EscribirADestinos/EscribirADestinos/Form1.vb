Imports System.IO
Imports System.Windows.Forms

Public Class Form1

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        lvOrigen.View = View.Details
        lvOrigen.Columns.Add("Path", 150)
        lvOrigen.Columns.Add("Nombre Carpeta", 100)
        lvOrigen.Columns.Add("Tamaño (MB)", 100)

        lvDestino.View = View.Details
        lvDestino.Columns.Add("Path", 150)
        lvDestino.Columns.Add("Nombre Carpeta", 100)

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

        Dim itemDestino As ListViewItem = lvDestino.Items.Item(0)

        For Each item As ListViewItem In lvOrigen.Items
            If item.Checked Then
                CopyFolder(item.Text, itemDestino.Text)
                MsgBox($"Carpeta '{item.Text}' copiada exitosamente a '{itemDestino.Text}'.")
            End If
        Next

    End Sub

    Private Sub btnDestino_Click(sender As Object, e As EventArgs) Handles btnDestino.Click

        Dim fbdDestino As New FolderBrowserDialog
        Dim destino As String

        If fbdDestino.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
            destino = fbdDestino.SelectedPath

            Dim item As New ListViewItem(destino)
            item.SubItems.Add(IO.Path.GetFileName(destino))
            lvDestino.Items.Add(item)

        End If

    End Sub

    Private Sub CopyFolder(ByVal origen As String, ByVal destino As String)
        ' Obtenemos la información del directorio origen
        Dim directorioOrigen As New DirectoryInfo(origen)

        ' Verificamos si el directorio origen existe
        If Not directorioOrigen.Exists Then
            Throw New DirectoryNotFoundException($"Directorio origen no encontrado: {origen}")
        End If

        ' Creamos el directorio destino si no existe
        If Not Directory.Exists(destino) Then
            Directory.CreateDirectory(destino)
        End If

        ' Obtenemos todos los archivos del directorio actual
        For Each archivo As FileInfo In directorioOrigen.GetFiles()
            Dim nuevoArchivo As String = Path.Combine(destino, archivo.Name)
            archivo.CopyTo(nuevoArchivo, True)
        Next

        ' Obtenemos todos los subdirectorios del directorio actual
        For Each subDirectorio As DirectoryInfo In directorioOrigen.GetDirectories()
            Dim nuevoDirectorio As String = Path.Combine(destino, subDirectorio.Name)
            ' Llamamos recursivamente a la función para copiar los subdirectorios
            CopyFolder(subDirectorio.FullName, nuevoDirectorio)
        Next
    End Sub

End Class
