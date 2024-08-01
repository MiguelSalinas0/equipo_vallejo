﻿
Imports System.IO
Imports System.Threading
Imports DevExpress.Utils.CommonDialogs
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Imports Microsoft.VisualBasic.Devices

Public Class frmMain

    Dim Usuario As String
    Dim idUsuario As Integer
    Dim dtUsuario As DataTable
    Dim dtOrigenes As DataTable
    Dim dtDestinos As DataTable

    Dim gvOrigen As String
    Dim gvDestino As String = ""


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

        Me.Usuario = frmLogin.Usuario
        dtUsuario = ConexionBBDD.ConexionSQL.EjecutarSP("[sp_FileAppCliente_ObtenerDatosUsuario]", Usuario)
        idUsuario = dtUsuario.Rows(0).Item("idUsuario")

        Me.Text += " Usuario: " & Usuario

        ActualizarOrigenes()

    End Sub
    Sub ActualizarOrigenes()

        dtOrigenes = ConexionBBDD.ConexionSQL.EjecutarSP("sp_FileAppCliente_ObtenerOrigenes", idUsuario)
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

        gvOrigen = GridViewOrigen.GetFocusedRowCellValue("idOrigen")

        If Not gvOrigen Is vbNullString Then

            ActualizarDestinos(GridViewOrigen.GetFocusedRowCellValue("idOrigen"))

        End If

    End Sub

    Private Sub btnDeleteDestino_Click(sender As Object, e As EventArgs) Handles btnDeleteDestino.Click


        gvDestino = GridViewDestino.GetFocusedRowCellValue("idDestino")

        If Not gvDestino Is Nothing Then

            ConexionBBDD.ConexionSQL.EjecutarSP("sp_FileAppCliente_EliminarDestino", gvDestino)

        End If

        If Not gvOrigen Is vbNullString Then

            ActualizarDestinos(gvOrigen)

        End If

    End Sub

    Private Sub btnDeleteOrigen_Click(sender As Object, e As EventArgs) Handles btnDeleteOrigen.Click

        gvOrigen = GridViewOrigen.GetFocusedRowCellValue("idOrigen")

        If Not gvOrigen Is vbNullString Then

            ConexionBBDD.ConexionSQL.EjecutarSP("sp_FileAppCliente_EliminarOrigen", gvOrigen)
            ActualizarOrigenes()

        End If

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

            AddOrigen(strPath)

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

        gvOrigen = GridViewOrigen.GetFocusedRowCellValue("idOrigen")

        If Not gvOrigen Is vbNullString Then

            ActualizarDestinos(GridViewOrigen.GetFocusedRowCellValue("idOrigen"))

        End If


    End Sub
    Sub InsertarLog(ByVal origen As String, destino As String, desc As String, est As String)
        Dim fechayhora As DateTime = DateTime.Now

        ConexionBBDD.ConexionSQL.EjecutarSP("sp_FileAppCliente_InsertarLog",
            idUsuario,
            origen,
            destino,
            desc,
            fechayhora,
            est
        )

    End Sub
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
                    'Log Error al copiar archivo
                    InsertarLog(origen, destino, ex.Message, "Error")
                    'MsgBox("Error al copiar archivo: " & vbCrLf & ex.Message)
                End Try
            Next

            ' Informar que la copia se ha completado
            'CerrarEspera()
            'MsgBox("Copia finalizada exitosamente.")
        Catch ex As DirectoryNotFoundException
            ' Manejar la excepción cuando el directorio de origen no se encuentra
            'Log Error de directorio de origen
            InsertarLog(origen, destino, ex.Message, "Error")
            'MsgBox("No se encontró el directorio de origen: " & ex.Message)
        Catch ex As Exception
            ' Manejar cualquier otra excepción que pueda ocurrir
            'Log Error copiar Archivo
            InsertarLog(origen, destino, ex.Message, "Error")
            'MsgBox("Ocurrió un error al copiar los archivos: " & ex.Message)
        End Try

    End Sub
    Private Sub btnCopy_Click(sender As Object, e As EventArgs) Handles btnCopy.Click

        Try

            If dtOrigenes IsNot Nothing AndAlso dtOrigenes.Rows.Count > 0 Then

                For Each rowOrigen As DataRow In dtOrigenes.Rows

                    Dim strPrimerDestino As String = vbNullString

                    If Not gvOrigen Is vbNullString Then

                        ActualizarDestinos(rowOrigen("idOrigen"))

                    End If

                    If dtDestinos IsNot Nothing AndAlso dtDestinos.Rows.Count > 0 Then

                        For Each rowDestino As DataRow In dtDestinos.Rows

                            If strPrimerDestino Is vbNullString Then

                                AbrirEspera("Desde: " & rowOrigen("Directorio") & vbCrLf & "Hasta: " & rowDestino("Directorio"))
                                'Log de Inicio
                                InsertarLog(rowOrigen("Directorio"), rowDestino("Directorio"), "Inicio Copia", "Ok")
                                strPrimerDestino = rowDestino("Directorio")
                                CopiarDatos(rowOrigen("Directorio"), rowDestino("Directorio"))
                                'Log de Fin
                                InsertarLog(rowOrigen("Directorio"), rowDestino("Directorio"), "Fin Copia", "Ok")

                            Else

                                AbrirEspera("Copiando desde: " & strPrimerDestino & vbCrLf & "Hasta: " & rowDestino("Directorio"))
                                'Log de Inicio
                                InsertarLog(strPrimerDestino, rowDestino("Directorio"), "Inicio Copia", "Ok")
                                CopiarDatos(strPrimerDestino, rowDestino("Directorio"))
                                'Log de Fin
                                InsertarLog(strPrimerDestino, rowDestino("Directorio"), "Fin Copia", "Ok")


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
        Dim idOrigen As Integer

        If Not gvOrigen Is vbNullString Then

            idOrigen = GridViewOrigen.GetFocusedRowCellValue("idOrigen")

        Else

            Exit Sub

        End If

        'Obtengo Path
        If fbdSeleccionarDestino.ShowDialog = System.Windows.Forms.DialogResult.OK Then

            'Asigno Variables
            strPath = fbdSeleccionarDestino.SelectedPath

            AddDestino(strPath, idOrigen)


        Else

            'Path No Ingresado

        End If


    End Sub

    Private Sub btnAddOrigenManual_Click(sender As Object, e As EventArgs) Handles btnAddOrigenManual.Click
        Dim strPath As String

        'Ingresa direccion
        strPath = InputBox("Ingrese direccion de Origen: ", "FileApp")

        If Not Directory.Exists(strPath) Or strPath Is vbNullString Then
            'Termina
            Exit Sub
        Else
            If Directory.Exists(strPath) Then
                MessageBox.Show("La carpeta existe.")
                AddOrigen(strPath)
            Else
                If MsgBox("La carpeta no ha sido encontrado, desea agregarlo de todos modos?.", MsgBoxStyle.YesNoCancel) = MsgBoxResult.Yes Then
                    MessageBox.Show("La ruta ha sido creada.")
                    AddOrigen(strPath)
                End If
            End If

        End If

    End Sub

    Sub AddOrigen(ByVal strPath As String)
        Dim strNombre As String

FlagNom: strNombre = InputBox("Ingrese Nombre:", "FileApp", "Nombre")

        'Valido Nombre Ingresado
        If ValidarNombre(strNombre) Then

            'Nombre Validado
            ConexionBBDD.ConexionSQL.EjecutarSP("sp_FileAppCliente_InsertarOrigen", strNombre, strPath, idUsuario)


        Else

            'Fallo Validacion
            If MsgBox("El nombre ingresado es invalido, ¿Deséa continuar?", MsgBoxStyle.RetryCancel) = MsgBoxResult.Retry Then

                GoTo FlagNom

            Else

                'nO DESEO cONTINUAR


            End If

        End If
    End Sub

    Sub AddDestino(ByVal strPath As String, ByVal idOrigen As String)
        Dim strNombre As String

FlagNom: strNombre = InputBox("Ingrese Nombre:", "FileApp", "Nombre")

        'Valido Nombre Ingresado
        If ValidarNombre(strNombre) Then

            'Nombre Validado
            ConexionBBDD.ConexionSQL.EjecutarSP("sp_FileAppCliente_InsertarDestinoXOrigen", idOrigen, strNombre, strPath)

            If Not gvOrigen Is vbNullString Then

                ActualizarDestinos(idOrigen)

            End If

        Else

            'Fallo Validacion
            If MsgBox("El nombre ingresado es invalido, ¿Deséa continuar?", MsgBoxStyle.RetryCancel) = MsgBoxResult.Retry Then

                GoTo FlagNom

            Else

                'No continuar


            End If

        End If
    End Sub

    Private Sub AddDestinoManual_Click(sender As Object, e As EventArgs) Handles AddDestinoManual.Click
        Dim strPath As String

        Dim idOrigen As Integer

        If Not gvOrigen Is vbNullString Then

            idOrigen = GridViewOrigen.GetFocusedRowCellValue("idOrigen")

        Else

            Exit Sub

        End If
        'Ingresa direccion
        strPath = InputBox("Ingrese direccion de Destino: ", "FileApp")

        If Not Directory.Exists(strPath) Or strPath Is vbNullString Then
            'Termina
            Exit Sub
        Else
            'Verifica que exista directorio
            If Directory.Exists(strPath) Then
                MessageBox.Show("La carpeta existe.")
                AddDestino(strPath, idOrigen)
            Else 'Pregunta si desea agregar de todas formas
                If MsgBox("La carpeta no ha sido encontrada, desea agregarla de todos modos?.", MsgBoxStyle.YesNoCancel) = MsgBoxResult.Yes Then
                    MessageBox.Show("La ruta ha sido creada.")
                    AddDestino(strPath, idOrigen)
                End If
            End If

        End If
    End Sub
End Class

