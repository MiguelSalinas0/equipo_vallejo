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
        lvOrigen = New ListView()
        btnOrigen = New Button()
        lvDestino = New ListView()
        btnDestino = New Button()
        btnCopiar = New Button()
        SuspendLayout()
        ' 
        ' lvOrigen
        ' 
        lvOrigen.CheckBoxes = True
        lvOrigen.HideSelection = True
        lvOrigen.Location = New Point(26, 29)
        lvOrigen.Name = "lvOrigen"
        lvOrigen.Size = New Size(369, 474)
        lvOrigen.TabIndex = 1
        lvOrigen.UseCompatibleStateImageBehavior = False
        ' 
        ' btnOrigen
        ' 
        btnOrigen.Location = New Point(26, 509)
        btnOrigen.Name = "btnOrigen"
        btnOrigen.Size = New Size(177, 50)
        btnOrigen.TabIndex = 2
        btnOrigen.Text = "Seleccionar Carpeta Origen"
        btnOrigen.UseVisualStyleBackColor = True
        ' 
        ' lvDestino
        ' 
        lvDestino.CheckBoxes = True
        lvDestino.Location = New Point(430, 29)
        lvDestino.Name = "lvDestino"
        lvDestino.Size = New Size(369, 474)
        lvDestino.TabIndex = 3
        lvDestino.UseCompatibleStateImageBehavior = False
        ' 
        ' btnDestino
        ' 
        btnDestino.Location = New Point(430, 509)
        btnDestino.Name = "btnDestino"
        btnDestino.Size = New Size(177, 50)
        btnDestino.TabIndex = 4
        btnDestino.Text = "Seleccionar Carpeta Destino"
        btnDestino.UseVisualStyleBackColor = True
        ' 
        ' btnCopiar
        ' 
        btnCopiar.Location = New Point(26, 604)
        btnCopiar.Name = "btnCopiar"
        btnCopiar.Size = New Size(100, 30)
        btnCopiar.TabIndex = 5
        btnCopiar.Text = "Copiar"
        btnCopiar.UseVisualStyleBackColor = True
        ' 
        ' Form1
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(830, 680)
        Controls.Add(btnCopiar)
        Controls.Add(btnDestino)
        Controls.Add(lvDestino)
        Controls.Add(btnOrigen)
        Controls.Add(lvOrigen)
        Name = "Form1"
        Text = "Form1"
        ResumeLayout(False)
    End Sub

    Friend WithEvents CheckedListBox1 As CheckedListBox
    Friend WithEvents lvOrigen As ListView
    Friend WithEvents btnOrigen As Button
    Friend WithEvents lvDestino As ListView
    Friend WithEvents btnDestino As Button
    Friend WithEvents btnCopiar As Button

End Class
