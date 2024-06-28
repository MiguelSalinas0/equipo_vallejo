<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        cmbSucursales = New ComboBox()
        txtRespuesta = New TextBox()
        SuspendLayout()
        ' 
        ' cmbSucursales
        ' 
        cmbSucursales.FormattingEnabled = True
        cmbSucursales.Location = New Point(343, 74)
        cmbSucursales.Name = "cmbSucursales"
        cmbSucursales.Size = New Size(121, 23)
        cmbSucursales.TabIndex = 0
        ' 
        ' txtRespuesta
        ' 
        txtRespuesta.Location = New Point(118, 123)
        txtRespuesta.Multiline = True
        txtRespuesta.Name = "txtRespuesta"
        txtRespuesta.ScrollBars = ScrollBars.Vertical
        txtRespuesta.Size = New Size(534, 282)
        txtRespuesta.TabIndex = 1
        ' 
        ' Form1
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1251, 450)
        Controls.Add(txtRespuesta)
        Controls.Add(cmbSucursales)
        Name = "Form1"
        Text = "Form1"
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents cmbSucursales As ComboBox
    Friend WithEvents txtRespuesta As TextBox

End Class
