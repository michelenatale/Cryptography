Option Strict On
Option Explicit On

Namespace LoginSystemApp

  <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
  Partial Class MyFrmApp
    Inherits FrmCustomerBase

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
      Me.TlpMain = New TableLayoutPanel()
      Me.BtToLI = New Button()
      Me.BtSetLI = New Button()
      Me.label1 = New Label()
      Me.TbMPw = New TextBox()
      Me.TlpMain.SuspendLayout()
      Me.SuspendLayout()
      ' 
      ' TlpMain
      ' 
      Me.TlpMain.ColumnCount = 1
      Me.TlpMain.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100F))
      Me.TlpMain.Controls.Add(Me.BtToLI, 0, 1)
      Me.TlpMain.Controls.Add(Me.BtSetLI, 0, 2)
      Me.TlpMain.Controls.Add(Me.label1, 0, 4)
      Me.TlpMain.Controls.Add(Me.TbMPw, 0, 5)
      Me.TlpMain.Dock = DockStyle.Fill
      Me.TlpMain.Location = New Point(0, 0)
      Me.TlpMain.Name = "TlpMain"
      Me.TlpMain.RowCount = 7
      Me.TlpMain.RowStyles.Add(New RowStyle(SizeType.Percent, 50F))
      Me.TlpMain.RowStyles.Add(New RowStyle(SizeType.Absolute, 50F))
      Me.TlpMain.RowStyles.Add(New RowStyle(SizeType.Absolute, 50F))
      Me.TlpMain.RowStyles.Add(New RowStyle(SizeType.Absolute, 20F))
      Me.TlpMain.RowStyles.Add(New RowStyle(SizeType.Absolute, 50F))
      Me.TlpMain.RowStyles.Add(New RowStyle(SizeType.Absolute, 150F))
      Me.TlpMain.RowStyles.Add(New RowStyle(SizeType.Percent, 50F))
      Me.TlpMain.Size = New Size(682, 466)
      Me.TlpMain.TabIndex = 4
      ' 
      ' BtToLI
      ' 
      Me.BtToLI.Anchor = AnchorStyles.None
      Me.BtToLI.Location = New Point(191, 76)
      Me.BtToLI.Name = "BtToLI"
      Me.BtToLI.Size = New Size(300, 43)
      Me.BtToLI.TabIndex = 0
      Me.BtToLI.Text = "To Login Information"
      Me.BtToLI.UseVisualStyleBackColor = True
      ' 
      ' BtSetLI
      ' 
      Me.BtSetLI.Anchor = AnchorStyles.None
      Me.BtSetLI.Location = New Point(191, 126)
      Me.BtSetLI.Name = "BtSetLI"
      Me.BtSetLI.Size = New Size(300, 43)
      Me.BtSetLI.TabIndex = 0
      Me.BtSetLI.Text = "Set Login Information"
      Me.BtSetLI.UseVisualStyleBackColor = True
      ' 
      ' label1
      ' 
      Me.label1.Anchor = AnchorStyles.None
      Me.label1.AutoSize = True
      Me.label1.Location = New Point(228, 204)
      Me.label1.Name = "label1"
      Me.label1.Size = New Size(225, 27)
      Me.label1.TabIndex = 2
      Me.label1.Text = "My Masterpassword"
      Me.label1.TextAlign = ContentAlignment.MiddleCenter
      ' 
      ' TbMPw
      ' 
      Me.TbMPw.Dock = DockStyle.Fill
      Me.TbMPw.Location = New Point(50, 246)
      Me.TbMPw.Margin = New Padding(50, 3, 50, 50)
      Me.TbMPw.Multiline = True
      Me.TbMPw.Name = "TbMPw"
      Me.TbMPw.Size = New Size(582, 97)
      Me.TbMPw.TabIndex = 1
      Me.TbMPw.Text = "[ ... ]"
      Me.TbMPw.TextAlign = HorizontalAlignment.Center
      ' 
      ' MyFrmApp
      ' 
      Me.AutoScaleDimensions = New SizeF(13F, 26F)
      Me.AutoScaleMode = AutoScaleMode.Font
      Me.BackColor = Color.White
      Me.StartPosition = FormStartPosition.CenterScreen
      Me.ClientSize = New Size(682, 466)
      Me.Controls.Add(Me.TlpMain)
      Me.Font = New Font("Arial", 14F)
      Me.Margin = New Padding(5, 4, 5, 4)
      Me.Name = "MyFrmApp"
      Me.Text = "MyFrmApp"
      Me.TlpMain.ResumeLayout(False)
      Me.TlpMain.PerformLayout()
      Me.ResumeLayout(False)
    End Sub

    Private WithEvents TlpMain As TableLayoutPanel
    Private WithEvents BtToLI As Button
    Private WithEvents BtSetLI As Button
    Private WithEvents label1 As Label
    Private WithEvents TbMPw As TextBox

  End Class

End Namespace