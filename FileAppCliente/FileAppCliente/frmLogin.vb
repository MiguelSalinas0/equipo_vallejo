
Imports DevExpress.CodeParser
Imports DevExpress.XtraReports.Design

Public Class frmLogin

    Public Usuario As String
    Public Perfil As String

    Private Sub AbrirEspera(ByVal Descripcion As String)

        If WaitFormManager.IsSplashFormVisible = False Then

            WaitFormManager.ShowWaitForm()
            WaitFormManager.SetWaitFormDescription(Descripcion)

        End If

    End Sub
    Private Sub CerrarEspera()

        If WaitFormManager.IsSplashFormVisible Then

            WaitFormManager.CloseWaitForm()

        End If

    End Sub

    Private Sub OK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK.Click

        AbrirEspera("Validando Datos.")

        Dim dtResult As DataTable = ConexionBBDD.ConexionSQL.EjecutarSP("[sp_FileAppCliente_ObtenerUsuario]", UsernameTextBox.Text, PasswordTextBox.Text)

        CerrarEspera()

        If dtResult.Rows(0).Item(0) = "OK" Then

            Usuario = UsernameTextBox.Text
            My.Settings.Usuario = Usuario
            My.Settings.Save()

            frmMain.Show()
            Me.Close()

        Else

            MsgBox(dtResult.Rows(0).Item(0), MsgBoxStyle.Critical, "")

        End If

    End Sub

    Private Sub Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel.Click

        Me.Close()

    End Sub

    Private Sub frmLogin_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        UsernameTextBox.Text = My.Settings.usuario

        If UsernameTextBox.Text <> "" Then

            PasswordTextBox.Focus()

        End If

    End Sub

End Class
