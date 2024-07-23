Imports System.IO
Imports System.Threading
Imports System.Windows.Forms.VisualStyles.VisualStyleElement

Public Class Form1

    Dim origenesDeDatos As New List(Of String)
    Dim destinosDeDatos As New List(Of String)

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lvOrigen.View = View.Details
        lvOrigen.Columns.Add("", 40)
        lvOrigen.Columns.Add("Directorio", 129, HorizontalAlignment.Left)
        lvOrigen.Columns.Add("Path", 120, HorizontalAlignment.Left)

        lvDestino.View = View.Details
        lvDestino.Columns.Add("", 40)
        lvDestino.Columns.Add("Directorio", 129, HorizontalAlignment.Left)
        lvDestino.Columns.Add("Path", 120, HorizontalAlignment.Left)
        VerificarOrigenesYDestinos()
    End Sub

    Private Sub btnAgregarArchivo_Click(sender As Object, e As EventArgs) Handles btnAgregarArchivo.Click
        Dim fbdDirectorio As New FolderBrowserDialog

        If fbdDirectorio.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
            AgregarDirectorio(fbdDirectorio.SelectedPath)
            VerificarOrigenesYDestinos()
        End If
    End Sub

    Private Sub VerificarOrigenesYDestinos()
        btnCopiar.Enabled = If(origenesDeDatos.Count > 0 And destinosDeDatos.Count > 0, True, False)
    End Sub


    Private Function AgregarDirectorio(ByVal path As String) As Boolean

        If Not origenesDeDatos.Contains(path) Then
            Dim lvItem As New ListViewItem

            lvItem.SubItems.Add(IO.Path.GetFileName(path))
            lvItem.SubItems.Add(path)

            lvOrigen.Items.Add(lvItem)

            origenesDeDatos.Add(path)

            Return True
        End If

        Return False

    End Function

    Private Function AgregarDestino(ByVal path As String)

        If Not destinosDeDatos.Contains(path) Then
            Dim lvItem As New ListViewItem

            lvItem.SubItems.Add(IO.Path.GetFileName(path))
            lvItem.SubItems.Add(path)

            lvDestino.Items.Add(lvItem)

            destinosDeDatos.Add(path)

            Return True
        End If

        Return False

    End Function

    Public Sub CopiarDatos(origenes As List(Of String), destinos As List(Of String))
        Try
            ' Check if each source directory exists
            For Each orgPath In origenes
                If Not Directory.Exists(orgPath) Then
                    Throw New DirectoryNotFoundException("Source directory does not exist: " & orgPath)
                End If
            Next

            ' Create the directories in the destination paths if they do not exist
            For Each destPath In destinos
                If Not Directory.Exists(destPath) Then
                    Directory.CreateDirectory(destPath)
                End If
            Next

            ' Process each source path
            For Each orgPath In origenes
                ' Get the subdirectories and files in the current source directory
                Dim directories As String() = Directory.GetDirectories(orgPath, "*", SearchOption.AllDirectories)
                Dim files As String() = Directory.GetFiles(orgPath, "*", SearchOption.AllDirectories)

                ' Create subdirectories in the destination paths
                For Each directorio In directories
                    Dim relativePath As String = directorio.Substring(orgPath.Length + 1)
                    For Each destPath In destinos
                        Directory.CreateDirectory(Path.Combine(destPath, relativePath))
                    Next
                Next

                ' Copy files to the destination paths
                For Each file In files
                    Dim relativePath As String = file.Substring(orgPath.Length + 1)
                    For Each destPath In destinos
                        IO.File.Copy(file, Path.Combine(destPath, relativePath), True)
                    Next
                Next

                ActualizarEstado("Copia finalizada", Color.Green)
            Next
        Catch ex As DirectoryNotFoundException
            ' Manejar excepción de directorio no encontrado
            ActualizarEstado("No se encontró uno de los directorios de origen.", Color.Red)
        Catch ex As Exception
            ' Manejar cualquier otra excepción
            ActualizarEstado("Ocurrió un error al copiar los archivos.", Color.Red)
        End Try
    End Sub


    Private Sub btnAgregarDestino_Click(sender As Object, e As EventArgs) Handles btnAgregarDestino.Click
        Dim fbdDirectorio As New FolderBrowserDialog

        If fbdDirectorio.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
            AgregarDestino(fbdDirectorio.SelectedPath)
            VerificarOrigenesYDestinos()
        End If
    End Sub

    Private Sub btnCopiar_Click(sender As Object, e As EventArgs) Handles btnCopiar.Click
        CopiarDatos(origenesDeDatos, destinosDeDatos)
    End Sub

    Private Sub ActualizarEstado(ByVal nuevoEstado As String, ByVal color As Color)
        lbEstado.ForeColor = color
        lbEstado.Text = nuevoEstado
    End Sub

    Private Sub EliminarElementosMarcados(ByRef listView As System.Windows.Forms.ListView, ByRef items As List(Of String))
        ' Itera sobre los elementos del ListView en orden inverso para evitar problemas al eliminar
        For i As Integer = listView.Items.Count - 1 To 0 Step -1
            If listView.Items(i).Checked Then
                ' Eliminar el elemento de la lista de cadenas
                Dim r = items.Remove(listView.Items(i).SubItems(2).Text)
                ' Eliminar el elemento del ListView
                listView.Items.RemoveAt(i)
            End If
        Next

        VerificarOrigenesYDestinos()
    End Sub

    Private Sub btnEliminarArchivo_Click(sender As Object, e As EventArgs) Handles btnEliminarArchivo.Click
        EliminarElementosMarcados(lvOrigen, origenesDeDatos)
    End Sub

    Private Sub btnEliminarDestino_Click(sender As Object, e As EventArgs) Handles btnEliminarDestino.Click
        EliminarElementosMarcados(lvDestino, destinosDeDatos)
    End Sub
End Class
