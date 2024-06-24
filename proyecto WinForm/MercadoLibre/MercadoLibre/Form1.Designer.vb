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
        btnEnviar = New Button()
        dgvResult = New DataGridView()
        CType(dgvResult, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' btnEnviar
        ' 
        btnEnviar.Location = New Point(56, 37)
        btnEnviar.Name = "btnEnviar"
        btnEnviar.Size = New Size(75, 23)
        btnEnviar.TabIndex = 1
        btnEnviar.Text = "Enviar"
        btnEnviar.UseVisualStyleBackColor = True
        ' 
        ' dgvResult
        ' 
        dgvResult.AllowUserToAddRows = False
        dgvResult.AllowUserToDeleteRows = False
        dgvResult.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvResult.Location = New Point(56, 66)
        dgvResult.Name = "dgvResult"
        dgvResult.ReadOnly = True
        dgvResult.Size = New Size(1204, 291)
        dgvResult.TabIndex = 2
        ' 
        ' Form1
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1295, 450)
        Controls.Add(dgvResult)
        Controls.Add(btnEnviar)
        Name = "Form1"
        Text = "Form1"
        CType(dgvResult, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
    End Sub
    Friend WithEvents btnEnviar As Button
    Friend WithEvents dgvResult As DataGridView

End Class
