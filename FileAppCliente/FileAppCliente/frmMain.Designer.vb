<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmMain
    Inherits System.Windows.Forms.Form

    'Form reemplaza a Dispose para limpiar la lista de componentes.
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

    'Requerido por el Diseñador de Windows Forms
    Private components As System.ComponentModel.IContainer

    'NOTA: el Diseñador de Windows Forms necesita el siguiente procedimiento
    'Se puede modificar usando el Diseñador de Windows Forms.  
    'No lo modifique con el editor de código.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.dgcOrigen = New DevExpress.XtraGrid.GridControl()
        Me.GridViewOrigen = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.btnRefresh = New System.Windows.Forms.Button()
        Me.WaitFormManager = New DevExpress.XtraSplashScreen.SplashScreenManager(Me, GetType(Global.FileAppCliente.WaitForm), True, True)
        Me.dgcDestino = New DevExpress.XtraGrid.GridControl()
        Me.GridViewDestino = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.btnAddOrigen = New System.Windows.Forms.Button()
        Me.btnDeleteOrigen = New System.Windows.Forms.Button()
        Me.btnAddDestino = New System.Windows.Forms.Button()
        Me.btnDeleteDestino = New System.Windows.Forms.Button()
        CType(Me.dgcOrigen, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridViewOrigen, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgcDestino, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridViewDestino, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'dgcOrigen
        '
        Me.dgcOrigen.Location = New System.Drawing.Point(12, 12)
        Me.dgcOrigen.MainView = Me.GridViewOrigen
        Me.dgcOrigen.Name = "dgcOrigen"
        Me.dgcOrigen.Size = New System.Drawing.Size(636, 461)
        Me.dgcOrigen.TabIndex = 0
        Me.dgcOrigen.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridViewOrigen})
        '
        'GridViewOrigen
        '
        Me.GridViewOrigen.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
        Me.GridViewOrigen.GridControl = Me.dgcOrigen
        Me.GridViewOrigen.Name = "GridViewOrigen"
        Me.GridViewOrigen.OptionsBehavior.Editable = False
        Me.GridViewOrigen.OptionsView.ShowGroupPanel = False
        '
        'btnRefresh
        '
        Me.btnRefresh.Location = New System.Drawing.Point(12, 703)
        Me.btnRefresh.Name = "btnRefresh"
        Me.btnRefresh.Size = New System.Drawing.Size(80, 30)
        Me.btnRefresh.TabIndex = 2
        Me.btnRefresh.Text = "Actualizar"
        Me.btnRefresh.UseVisualStyleBackColor = True
        '
        'WaitFormManager
        '
        Me.WaitFormManager.ClosingDelay = 500
        '
        'dgcDestino
        '
        Me.dgcDestino.Location = New System.Drawing.Point(654, 12)
        Me.dgcDestino.MainView = Me.GridViewDestino
        Me.dgcDestino.Name = "dgcDestino"
        Me.dgcDestino.Size = New System.Drawing.Size(636, 461)
        Me.dgcDestino.TabIndex = 3
        Me.dgcDestino.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridViewDestino})
        '
        'GridViewDestino
        '
        Me.GridViewDestino.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
        Me.GridViewDestino.GridControl = Me.dgcDestino
        Me.GridViewDestino.Name = "GridViewDestino"
        Me.GridViewDestino.OptionsBehavior.Editable = False
        Me.GridViewDestino.OptionsView.ShowGroupPanel = False
        '
        'btnAddOrigen
        '
        Me.btnAddOrigen.Location = New System.Drawing.Point(12, 495)
        Me.btnAddOrigen.Name = "btnAddOrigen"
        Me.btnAddOrigen.Size = New System.Drawing.Size(125, 30)
        Me.btnAddOrigen.TabIndex = 4
        Me.btnAddOrigen.Text = "Agregar Origen"
        Me.btnAddOrigen.UseVisualStyleBackColor = True
        '
        'btnDeleteOrigen
        '
        Me.btnDeleteOrigen.Location = New System.Drawing.Point(174, 495)
        Me.btnDeleteOrigen.Name = "btnDeleteOrigen"
        Me.btnDeleteOrigen.Size = New System.Drawing.Size(125, 30)
        Me.btnDeleteOrigen.TabIndex = 5
        Me.btnDeleteOrigen.Text = "Eliminar Origen"
        Me.btnDeleteOrigen.UseVisualStyleBackColor = True
        '
        'btnAddDestino
        '
        Me.btnAddDestino.Location = New System.Drawing.Point(654, 495)
        Me.btnAddDestino.Name = "btnAddDestino"
        Me.btnAddDestino.Size = New System.Drawing.Size(125, 30)
        Me.btnAddDestino.TabIndex = 6
        Me.btnAddDestino.Text = "Agregar Destino"
        Me.btnAddDestino.UseVisualStyleBackColor = True
        '
        'btnDeleteDestino
        '
        Me.btnDeleteDestino.Location = New System.Drawing.Point(809, 495)
        Me.btnDeleteDestino.Name = "btnDeleteDestino"
        Me.btnDeleteDestino.Size = New System.Drawing.Size(125, 30)
        Me.btnDeleteDestino.TabIndex = 7
        Me.btnDeleteDestino.Text = "Eliminar Destino"
        Me.btnDeleteDestino.UseVisualStyleBackColor = True
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1387, 745)
        Me.Controls.Add(Me.btnDeleteDestino)
        Me.Controls.Add(Me.btnAddDestino)
        Me.Controls.Add(Me.btnDeleteOrigen)
        Me.Controls.Add(Me.btnAddOrigen)
        Me.Controls.Add(Me.dgcDestino)
        Me.Controls.Add(Me.btnRefresh)
        Me.Controls.Add(Me.dgcOrigen)
        Me.Name = "frmMain"
        Me.Text = "FileAppCliente"
        CType(Me.dgcOrigen, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridViewOrigen, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgcDestino, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridViewDestino, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents dgcOrigen As DevExpress.XtraGrid.GridControl
    Friend WithEvents GridViewOrigen As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents btnRefresh As Button
    Friend WithEvents WaitFormManager As DevExpress.XtraSplashScreen.SplashScreenManager
    Friend WithEvents dgcDestino As DevExpress.XtraGrid.GridControl
    Friend WithEvents GridViewDestino As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents btnAddOrigen As Button
    Friend WithEvents btnDeleteOrigen As Button
    Friend WithEvents btnAddDestino As Button
    Friend WithEvents btnDeleteDestino As Button
End Class
