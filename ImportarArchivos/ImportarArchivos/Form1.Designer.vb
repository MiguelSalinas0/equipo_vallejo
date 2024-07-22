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
        lvDestino = New ListView()
        btnAgregarDestino = New Button()
        btnEliminarDestino = New Button()
        btnEliminarArchivo = New Button()
        btnAgregarArchivo = New Button()
        lvOrigen = New ListView()
        btnCopiar = New Button()
        Label1 = New Label()
        Label2 = New Label()
        lbEstado = New Label()
        SuspendLayout()
        ' 
        ' lvDestino
        ' 
        lvDestino.CheckBoxes = True
        lvDestino.Location = New Point(575, 86)
        lvDestino.Name = "lvDestino"
        lvDestino.Size = New Size(471, 329)
        lvDestino.TabIndex = 0
        lvDestino.UseCompatibleStateImageBehavior = False
        ' 
        ' btnAgregarDestino
        ' 
        btnAgregarDestino.Location = New Point(575, 441)
        btnAgregarDestino.Name = "btnAgregarDestino"
        btnAgregarDestino.Size = New Size(123, 65)
        btnAgregarDestino.TabIndex = 1
        btnAgregarDestino.Text = "Agregar destino"
        btnAgregarDestino.UseVisualStyleBackColor = True
        ' 
        ' btnEliminarDestino
        ' 
        btnEliminarDestino.Location = New Point(717, 441)
        btnEliminarDestino.Name = "btnEliminarDestino"
        btnEliminarDestino.Size = New Size(117, 65)
        btnEliminarDestino.TabIndex = 2
        btnEliminarDestino.Text = "Eliminar destino"
        btnEliminarDestino.UseVisualStyleBackColor = True
        ' 
        ' btnEliminarArchivo
        ' 
        btnEliminarArchivo.Location = New Point(179, 441)
        btnEliminarArchivo.Name = "btnEliminarArchivo"
        btnEliminarArchivo.Size = New Size(117, 65)
        btnEliminarArchivo.TabIndex = 5
        btnEliminarArchivo.Text = "Eliminar directorio"
        btnEliminarArchivo.UseVisualStyleBackColor = True
        ' 
        ' btnAgregarArchivo
        ' 
        btnAgregarArchivo.Location = New Point(37, 441)
        btnAgregarArchivo.Name = "btnAgregarArchivo"
        btnAgregarArchivo.Size = New Size(123, 65)
        btnAgregarArchivo.TabIndex = 4
        btnAgregarArchivo.Text = "Agregar directorio"
        btnAgregarArchivo.UseVisualStyleBackColor = True
        ' 
        ' lvOrigen
        ' 
        lvOrigen.CheckBoxes = True
        lvOrigen.Location = New Point(36, 86)
        lvOrigen.Name = "lvOrigen"
        lvOrigen.Size = New Size(471, 329)
        lvOrigen.TabIndex = 3
        lvOrigen.UseCompatibleStateImageBehavior = False
        ' 
        ' btnCopiar
        ' 
        btnCopiar.Location = New Point(36, 557)
        btnCopiar.Name = "btnCopiar"
        btnCopiar.Size = New Size(109, 65)
        btnCopiar.TabIndex = 6
        btnCopiar.Text = "Copiar"
        btnCopiar.UseVisualStyleBackColor = True
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Font = New Font("Segoe UI", 14.25F, FontStyle.Bold)
        Label1.Location = New Point(36, 44)
        Label1.Name = "Label1"
        Label1.Size = New Size(154, 25)
        Label1.TabIndex = 7
        Label1.Text = "Origen de datos"
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Font = New Font("Segoe UI", 14.25F, FontStyle.Bold)
        Label2.Location = New Point(575, 44)
        Label2.Name = "Label2"
        Label2.Size = New Size(161, 25)
        Label2.TabIndex = 8
        Label2.Text = "Destino de datos"
        ' 
        ' lbEstado
        ' 
        lbEstado.AutoSize = True
        lbEstado.Font = New Font("Segoe UI", 14.25F, FontStyle.Italic, GraphicsUnit.Point, CByte(0))
        lbEstado.Location = New Point(604, 574)
        lbEstado.Name = "lbEstado"
        lbEstado.Size = New Size(0, 25)
        lbEstado.TabIndex = 9
        ' 
        ' Form1
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        AutoSize = True
        ClientSize = New Size(1111, 634)
        Controls.Add(lbEstado)
        Controls.Add(Label2)
        Controls.Add(Label1)
        Controls.Add(btnCopiar)
        Controls.Add(btnEliminarArchivo)
        Controls.Add(btnAgregarArchivo)
        Controls.Add(lvOrigen)
        Controls.Add(btnEliminarDestino)
        Controls.Add(btnAgregarDestino)
        Controls.Add(lvDestino)
        Name = "Form1"
        Text = "Form1"
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents lvDestino As ListView
    Friend WithEvents btnAgregarDestino As Button
    Friend WithEvents btnEliminarDestino As Button
    Friend WithEvents btnEliminarArchivo As Button
    Friend WithEvents btnAgregarArchivo As Button
    Friend WithEvents lvOrigen As ListView
    Friend WithEvents btnCopiar As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents lbEstado As Label

End Class
