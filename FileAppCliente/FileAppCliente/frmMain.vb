﻿Imports System.Threading
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

End Class
