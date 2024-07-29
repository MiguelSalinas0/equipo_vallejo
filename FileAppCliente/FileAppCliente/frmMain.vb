Imports System.IO
Imports System.Threading
Imports DevExpress.Utils.CommonDialogs
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Imports Microsoft.VisualBasic.Devices

Public Class frmMain

    Dim dtOrigenes As DataTable
    Dim dtDestinos As DataTable
    Dim gvOrigen As String
    Dim gvDestino As String


    Private Sub AbrirEspera(ByVal Descripcion As String)

        'Abro waitform
        If WaitFormManager.IsSplashFormVisible = False Then

            WaitFormManager.ShowWaitForm()
            WaitFormManager.SetWaitFormDescription(Descripcion)

        End If

    End Sub
    Private Sub CerrarEspera()

        'Cierro waitform
        If WaitFormManager.IsSplashFormVisible Then

            WaitFormManager.CloseWaitForm()

        End If

    End Sub
    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        'Cargo Origenes
        ActualizarOrigenes()

    End Sub
    Sub ActualizarOrigenes()

        dtOrigenes = ConexionBBDD.ConexionSQL.EjecutarSP("sp_FileAppCliente_ObtenerOrigenes")
        dgcOrigen.DataSource = dtOrigenes
        GridViewOrigen.BestFitColumns()

    End Sub
    Sub ActualizarDestinos(ByVal strOrigen As String)

        dtDestinos = ConexionBBDD.ConexionSQL.EjecutarSP("sp_FileAppCliente_ObtenerDestinosXOrigen", strOrigen)
        dgcDestino.DataSource = dtDestinos
        GridViewDestino.BestFitColumns()

    End Sub
    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click

        AbrirEspera("Cargando Datos.")

        'Cargo Origenes
        ActualizarOrigenes()


        CerrarEspera()

    End Sub
    Private Sub dgcOrigen_Click(sender As Object, e As EventArgs) Handles dgcOrigen.Click

        ActualizarDestinos(GridViewOrigen.GetFocusedRowCellValue("ID"))

    End Sub

    Private Sub btnDeleteDestino_Click(sender As Object, e As EventArgs) Handles btnDeleteDestino.Click

        gvDestino = GridViewDestino.GetFocusedRowCellValue("ID")

        ConexionBBDD.ConexionSQL.EjecutarSP("sp_FileAppCliente_EliminarDestino", gvDestino)

    End Sub

    Private Sub btnDeleteOrigen_Click(sender As Object, e As EventArgs) Handles btnDeleteOrigen.Click

        ConexionBBDD.ConexionSQL.EjecutarSP("", gvOrigen)

    End Sub

    Private Sub btnAddOrigen_Click(sender As Object, e As EventArgs) Handles btnAddOrigen.Click

        'Defino Variables
        Dim fbdSeleccionarOrigen As New XtraFolderBrowserDialog
        fbdSeleccionarOrigen.DialogStyle = FolderBrowserDialogStyle.Wide

        Dim strPath As String
        Dim strNombre As String

        'Obtengo Path

        If fbdSeleccionarOrigen.ShowDialog() = System.Windows.Forms.DialogResult.OK Then

            'Path Ingresado

            'Asigno Variables
            strPath = fbdSeleccionarOrigen.SelectedPath

            'Obtengo Nombre
FlagNom:    strNombre = InputBox("Ingrese Nombre:", "FileApp", "Nombre")

            'Valido Nombre Ingresado
            If ValidarNombre(strNombre) Then

                'Nombre Validado
                ConexionBBDD.ConexionSQL.EjecutarSP("sp_FileAppCliente_InsertarOrigen", strNombre, strPath)


            Else

                'Fallo Validacion
                If MsgBox("El nombre ingresado es invalido, ¿Deséa continuar?", MsgBoxStyle.RetryCancel) = MsgBoxResult.Retry Then

                    GoTo FlagNom

                Else

                    'nO DESEO cONTINUAR


                End If

            End If


        Else

            'Path No Ingresado

        End If
        ActualizarOrigenes()
    End Sub

    Function ValidarNombre(ByVal nombre As String) As Boolean

        'Valido nombre
        If Trim(nombre) = vbNullString Then

            'Fallo Validacion
            Return False

        Else

            'Devolver OK' 
            Return True

        End If

    End Function

    Private Sub GridViewOrigen_FocusedRowChanged(sender As Object, e As FocusedRowChangedEventArgs) Handles GridViewOrigen.FocusedRowChanged
        ActualizarDestinos(GridViewOrigen.GetFocusedRowCellValue("ID"))
    End Sub



    'Private Sub CopyFolder(ByVal origen As String, ByVal destino As String)
    '    'Try
    '    ' Obtenemos la información del directorio origen
    '    Dim directorioOrigen As New DirectoryInfo(origen)
    '        Dim directorioDestino As New DirectoryInfo(destino)

    '        ' Verificamos si el directorio origen existe
    '        If Not directorioOrigen.Exists Then
    '            Throw New DirectoryNotFoundException($"Directorio origen no encontrado: {origen}")
    '        End If

    '        '' Creamos el directorio destino si no existe
    '        'If Not Directory.Exists(destino) Then
    '        '    Directory.CreateDirectory(destino)
    '        'End If

    '        ' Obtenemos todos los archivos del directorio actual
    '        For Each archivo As FileInfo In directorioOrigen.GetFiles()
    '            Dim nuevoArchivo As String = Path.Combine(destino, archivo.Name)
    '            archivo.CopyTo(nuevoArchivo, True)
    '        Next

    '        ' Obtenemos todos los subdirectorios del directorio actual
    '        For Each subDirectorio As DirectoryInfo In directorioOrigen.GetDirectories()
    '            Dim nuevoDirectorio As String = Path.Combine(destino, subDirectorio.Name)
    '            ' Llamamos recursivamente a la función para copiar los subdirectorios
    '            CopyFolder(subDirectorio.FullName, nuevoDirectorio)
    '        Next
    '    'Catch ex As Exception

    '    'End Try
    'End Sub

    Public Sub CopiarDatos(origen As String, destino As String)
        Try
            ' Verificar si el directorio de origen existe
            If Not Directory.Exists(origen) Then
                Throw New DirectoryNotFoundException("El directorio de origen no existe: " & origen)
            End If

            ' Crear el directorio de destino si no existe
            'If Not Directory.Exists(destino) Then
            '    Directory.CreateDirectory(destino)
            'End If

            ' Obtener los subdirectorios y archivos en el directorio de origen
            Dim directories As String() = Directory.GetDirectories(origen, "*", SearchOption.AllDirectories)
            Dim files As String() = Directory.GetFiles(origen, "*", SearchOption.AllDirectories)

            ' Crear subdirectorios en el directorio de destino
            For Each directorio In directories
                ' Obtener la ruta relativa del subdirectorio
                Dim relativePath As String = directorio.Substring(origen.Length + 1)
                ' Crear el subdirectorio en el destino
                Directory.CreateDirectory(Path.Combine(destino, relativePath))
            Next

            ' Copiar archivos al directorio de destino
            For Each file In files
                Try

                    ' Obtener la ruta relativa del archivo
                    Dim relativePath As String = file.Substring(origen.Length + 1)
                    ' Copiar el archivo al destino
                    IO.File.Copy(file, Path.Combine(destino, relativePath), True)
                Catch ex As Exception
                    MsgBox("Error al copiar archivo: " & vbCrLf & ex.Message)
                End Try
            Next

            ' Informar que la copia se ha completado
            CerrarEspera()
            MsgBox("Copia finalizada exitosamente.")
        Catch ex As DirectoryNotFoundException
            ' Manejar la excepción cuando el directorio de origen no se encuentra
            MsgBox("No se encontró el directorio de origen: " & ex.Message)
        Catch ex As Exception
            ' Manejar cualquier otra excepción que pueda ocurrir
            MsgBox("Ocurrió un error al copiar los archivos: " & ex.Message)
        End Try
    End Sub



    Private Sub btnCopy_Click(sender As Object, e As EventArgs) Handles btnCopy.Click

        'contador

        Try

            If dtOrigenes IsNot Nothing AndAlso dtOrigenes.Rows.Count > 0 Then
                For Each rowOrigen As DataRow In dtOrigenes.Rows

                    Dim strPrimerDestino As String = vbNullString


                    ActualizarDestinos(rowOrigen("ID"))
                    If dtDestinos IsNot Nothing AndAlso dtDestinos.Rows.Count > 0 Then

                        For Each rowDestino As DataRow In dtDestinos.Rows

                            If strPrimerDestino Is vbNullString Then

                                AbrirEspera("Desde: " & rowOrigen("Directorio") & vbCrLf & "Hasta: " & rowDestino("Directorio"))

                                strPrimerDestino = rowDestino("Directorio")
                                CopiarDatos(rowOrigen("Directorio"), rowDestino("Directorio"))

                            Else

                                AbrirEspera("Copiando desde: " & strPrimerDestino & vbCrLf & "Hasta: " & rowDestino("Directorio"))

                                CopiarDatos(strPrimerDestino, rowDestino("Directorio"))

                            End If

                            CerrarEspera()
                        Next

                    End If

                Next
            End If

        Catch ex As Exception
            MsgBox("Ocurrio un error")
        End Try


    End Sub

    Private Sub btnAddDestino_Click(sender As Object, e As EventArgs) Handles btnAddDestino.Click

        'Defino Variables
        Dim fbdSeleccionarDestino As New XtraFolderBrowserDialog
        fbdSeleccionarDestino.DialogStyle = FolderBrowserDialogStyle.Wide
        Dim strPath As String
        Dim strNombre As String
        Dim idOrigen As Integer

        idOrigen = GridViewOrigen.GetFocusedRowCellValue("ID")

        'Obtengo Path
        If fbdSeleccionarDestino.ShowDialog = System.Windows.Forms.DialogResult.OK Then

            'Asigno Variables
            strPath = fbdSeleccionarDestino.SelectedPath

FlagNom:    strNombre = InputBox("Ingrese Nombre:", "FileApp", "Nombre")

            'Valido Nombre Ingresado
            If ValidarNombre(strNombre) Then

                'Nombre Validado
                ConexionBBDD.ConexionSQL.EjecutarSP("sp_FileAppCliente_InsertarDestinoXOrigen", idOrigen, strNombre, strPath)
                ActualizarDestinos(idOrigen)

            Else

                'Fallo Validacion
                If MsgBox("El nombre ingresado es invalido, ¿Deséa continuar?", MsgBoxStyle.RetryCancel) = MsgBoxResult.Retry Then

                    GoTo FlagNom

                Else

                    'nO DESEO cONTINUAR


                End If

            End If


        Else

            'Path No Ingresado

        End If


    End Sub

    Private Sub XtraFolderBrowserDialog1_HelpRequest(sender As Object, e As EventArgs)

    End Sub
End Class
