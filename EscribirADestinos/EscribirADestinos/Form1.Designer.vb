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
        dgcMain = New DevExpress.XtraGrid.GridControl()
        GridView = New DevExpress.XtraGrid.Views.Grid.GridView()
        CType(dgcMain, ComponentModel.ISupportInitialize).BeginInit()
        CType(GridView, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' lvOrigen
        ' 
        lvOrigen.CheckBoxes = True
        lvOrigen.HideSelection = True
        lvOrigen.Location = New Point(20, 399)
        lvOrigen.Name = "lvOrigen"
        lvOrigen.Size = New Size(369, 145)
        lvOrigen.TabIndex = 1
        lvOrigen.UseCompatibleStateImageBehavior = False
        ' 
        ' btnOrigen
        ' 
        btnOrigen.Location = New Point(20, 550)
        btnOrigen.Name = "btnOrigen"
        btnOrigen.Size = New Size(177, 50)
        btnOrigen.TabIndex = 2
        btnOrigen.Text = "Seleccionar Carpeta Origen"
        btnOrigen.UseVisualStyleBackColor = True
        ' 
        ' lvDestino
        ' 
        lvDestino.CheckBoxes = True
        lvDestino.Location = New Point(424, 399)
        lvDestino.Name = "lvDestino"
        lvDestino.Size = New Size(369, 145)
        lvDestino.TabIndex = 3
        lvDestino.UseCompatibleStateImageBehavior = False
        ' 
        ' btnDestino
        ' 
        btnDestino.Location = New Point(424, 550)
        btnDestino.Name = "btnDestino"
        btnDestino.Size = New Size(177, 50)
        btnDestino.TabIndex = 4
        btnDestino.Text = "Seleccionar Carpeta Destino"
        btnDestino.UseVisualStyleBackColor = True
        ' 
        ' btnCopiar
        ' 
        btnCopiar.Location = New Point(20, 622)
        btnCopiar.Name = "btnCopiar"
        btnCopiar.Size = New Size(100, 30)
        btnCopiar.TabIndex = 5
        btnCopiar.Text = "Copiar"
        btnCopiar.UseVisualStyleBackColor = True
        ' 
        ' dgcMain
        ' 
        dgcMain.Location = New Point(12, 12)
        dgcMain.MainView = GridView
        dgcMain.Name = "dgcMain"
        dgcMain.Size = New Size(806, 340)
        dgcMain.TabIndex = 6
        dgcMain.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {GridView})
        ' 
        ' GridView
        ' 
        GridView.GridControl = dgcMain
        GridView.Name = "GridView"
        GridView.OptionsView.ShowGroupPanel = False
        ' 
        ' Form1
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(830, 680)
        Controls.Add(dgcMain)
        Controls.Add(btnCopiar)
        Controls.Add(btnDestino)
        Controls.Add(lvDestino)
        Controls.Add(btnOrigen)
        Controls.Add(lvOrigen)
        Name = "Form1"
        Text = "Form1"
        CType(dgcMain, ComponentModel.ISupportInitialize).EndInit()
        CType(GridView, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
    End Sub

    Friend WithEvents CheckedListBox1 As CheckedListBox
    Friend WithEvents lvOrigen As ListView
    Friend WithEvents btnOrigen As Button
    Friend WithEvents lvDestino As ListView
    Friend WithEvents btnDestino As Button
    Friend WithEvents btnCopiar As Button
    Friend WithEvents dgcMain As DevExpress.XtraGrid.GridControl
    Friend WithEvents GridView As DevExpress.XtraGrid.Views.Grid.GridView

End Class
