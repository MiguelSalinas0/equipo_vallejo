Imports System.Threading
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid

Public Class frmMain

    Dim dtOrigenes As DataTable
    Dim dtDestinos As DataTable

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
    Sub ActualizarDestinos()

        dtDestinos = ConexionBBDD.ConexionSQL.EjecutarSP("sp_FileAppCliente_ObtenerDestinosXOrigen", GridViewOrigen.GetFocusedRowCellValue("ID"))
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

        ActualizarDestinos()

    End Sub

    Private Sub btnDeleteDestino_Click(sender As Object, e As EventArgs) Handles btnDeleteDestino.Click

        ConexionBBDD.ConexionSQL.EjecutarSP("sp_FileAppCliente_EliminarDestino", GridViewDestino.GetFocusedRowCellValue("ID"))


    End Sub

    Private Sub btnDeleteOrigen_Click(sender As Object, e As EventArgs) Handles btnDeleteOrigen.Click

        ConexionBBDD.ConexionSQL.EjecutarSP("", GridViewOrigen.GetFocusedRowCellValue("ID"))

    End Sub

    Private Sub btnAddOrigen_Click(sender As Object, e As EventArgs) Handles btnAddOrigen.Click

        'Defino Variables
        Dim fbdSeleccionarOrigen As New FolderBrowserDialog
        Dim strPath As String
        Dim strNombre As String

        'Obtengo Path
        If fbdSeleccionarOrigen.ShowDialog = System.Windows.Forms.DialogResult.OK Then

            'Path Ingresado

            'Asigno Variables
            strPath = fbdSeleccionarOrigen.SelectedPath

            'Obtengo Nombre
FlagNom:    strNombre = InputBox("Ingrese Nombre:", "FileApp", "Nombre")

            'Valido Nombre Ingresado
            If ValidarNombre(strNombre) Then

                'Nombre Validado



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
        ActualizarDestinos()
    End Sub
End Class
