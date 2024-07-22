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
        btnAgregarDestino.Size = New Size(109, 65)
        btnAgregarDestino.TabIndex = 1
        btnAgregarDestino.Text = "Agregar destino"
        btnAgregarDestino.UseVisualStyleBackColor = True
        ' 
        ' btnEliminarDestino
        ' 
        btnEliminarDestino.Location = New Point(717, 441)
        btnEliminarDestino.Name = "btnEliminarDestino"
        btnEliminarDestino.Size = New Size(103, 65)
        btnEliminarDestino.TabIndex = 2
        btnEliminarDestino.Text = "Eliminar destino"
        btnEliminarDestino.UseVisualStyleBackColor = True
        ' 
        ' btnEliminarArchivo
        ' 
        btnEliminarArchivo.Location = New Point(179, 441)
        btnEliminarArchivo.Name = "btnEliminarArchivo"
        btnEliminarArchivo.Size = New Size(103, 65)
        btnEliminarArchivo.TabIndex = 5
        btnEliminarArchivo.Text = "Eliminar Archivo"
        btnEliminarArchivo.UseVisualStyleBackColor = True
        ' 
        ' btnAgregarArchivo
        ' 
        btnAgregarArchivo.Location = New Point(37, 441)
        btnAgregarArchivo.Name = "btnAgregarArchivo"
        btnAgregarArchivo.Size = New Size(109, 65)
        btnAgregarArchivo.TabIndex = 4
        btnAgregarArchivo.Text = "Agregar archivo"
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
        ' Form1
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1111, 634)
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
    End Sub

    Friend WithEvents lvDestino As ListView
    Friend WithEvents btnAgregarDestino As Button
    Friend WithEvents btnEliminarDestino As Button
    Friend WithEvents btnEliminarArchivo As Button
    Friend WithEvents btnAgregarArchivo As Button
    Friend WithEvents lvOrigen As ListView
    Friend WithEvents btnCopiar As Button

End Class
