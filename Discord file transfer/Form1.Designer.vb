<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog()
        Me.ConvertFromDTP = New System.Windows.Forms.Button()
        Me.ConvertToDTP = New System.Windows.Forms.Button()
        Me.FolderBrowserDialog1 = New System.Windows.Forms.FolderBrowserDialog()
        Me.CheckBox1 = New System.Windows.Forms.CheckBox()
        Me.SuspendLayout()
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'ConvertFromDTP
        '
        Me.ConvertFromDTP.Location = New System.Drawing.Point(12, 12)
        Me.ConvertFromDTP.Name = "ConvertFromDTP"
        Me.ConvertFromDTP.Size = New System.Drawing.Size(107, 22)
        Me.ConvertFromDTP.TabIndex = 0
        Me.ConvertFromDTP.Text = "Convert from DTP"
        Me.ConvertFromDTP.UseVisualStyleBackColor = True
        '
        'ConvertToDTP
        '
        Me.ConvertToDTP.Location = New System.Drawing.Point(125, 12)
        Me.ConvertToDTP.Name = "ConvertToDTP"
        Me.ConvertToDTP.Size = New System.Drawing.Size(107, 22)
        Me.ConvertToDTP.TabIndex = 1
        Me.ConvertToDTP.Text = "Convert to DTP"
        Me.ConvertToDTP.UseVisualStyleBackColor = True
        '
        'CheckBox1
        '
        Me.CheckBox1.AutoSize = True
        Me.CheckBox1.Location = New System.Drawing.Point(13, 40)
        Me.CheckBox1.Name = "CheckBox1"
        Me.CheckBox1.Size = New System.Drawing.Size(78, 17)
        Me.CheckBox1.TabIndex = 2
        Me.CheckBox1.Text = "Nitro Mode"
        Me.CheckBox1.UseVisualStyleBackColor = True
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(247, 62)
        Me.Controls.Add(Me.CheckBox1)
        Me.Controls.Add(Me.ConvertToDTP)
        Me.Controls.Add(Me.ConvertFromDTP)
        Me.Name = "Form1"
        Me.Text = "File splitter"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents OpenFileDialog1 As OpenFileDialog
    Friend WithEvents SaveFileDialog1 As SaveFileDialog
    Friend WithEvents ConvertFromDTP As Button
    Friend WithEvents ConvertToDTP As Button
    Friend WithEvents FolderBrowserDialog1 As FolderBrowserDialog
    Friend WithEvents CheckBox1 As CheckBox
End Class
