namespace LoginSystemApp;

partial class MyFrmApp
{
  /// <summary>
  /// Required designer variable.
  /// </summary>
  private System.ComponentModel.IContainer components = null;

  /// <summary>
  /// Clean up any resources being used.
  /// </summary>
  /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
  protected override void Dispose(bool disposing)
  {
    if (disposing && (components != null))
    {
      components.Dispose();
    }
    base.Dispose(disposing);
  }

  #region Windows Form Designer generated code

  /// <summary>
  /// Required method for Designer support - do not modify
  /// the contents of this method with the code editor.
  /// </summary>
  private void InitializeComponent()
  {
    this.BtToLI = new Button();
    this.BtSetLI = new Button();
    this.TbMPw = new TextBox();
    this.label1 = new Label();
    this.TlpMain = new TableLayoutPanel();
    this.TlpMain.SuspendLayout();
    this.SuspendLayout();
    // 
    // BtToLI
    // 
    this.BtToLI.Anchor = AnchorStyles.None;
    this.BtToLI.Location = new Point(191, 76);
    this.BtToLI.Name = "BtToLI";
    this.BtToLI.Size = new Size(300, 43);
    this.BtToLI.TabIndex = 0;
    this.BtToLI.Text = "To Login Information";
    this.BtToLI.UseVisualStyleBackColor = true;
    this.BtToLI.Click += this.BtToLI_Click;
    // 
    // BtSetLI
    // 
    this.BtSetLI.Anchor = AnchorStyles.None;
    this.BtSetLI.Location = new Point(191, 126);
    this.BtSetLI.Name = "BtSetLI";
    this.BtSetLI.Size = new Size(300, 43);
    this.BtSetLI.TabIndex = 0;
    this.BtSetLI.Text = "Set Login Information";
    this.BtSetLI.UseVisualStyleBackColor = true;
    this.BtSetLI.Click += this.BtSetLI_Click;
    // 
    // TbMPw
    // 
    this.TbMPw.Dock = DockStyle.Fill;
    this.TbMPw.Location = new Point(50, 246);
    this.TbMPw.Margin = new Padding(50, 3, 50, 50);
    this.TbMPw.Multiline = true;
    this.TbMPw.Name = "TbMPw";
    this.TbMPw.Size = new Size(582, 97);
    this.TbMPw.TabIndex = 1;
    this.TbMPw.Text = "[ ... ]";
    this.TbMPw.TextAlign = HorizontalAlignment.Center;
    // 
    // label1
    // 
    this.label1.Anchor = AnchorStyles.None;
    this.label1.AutoSize = true;
    this.label1.Location = new Point(228, 204);
    this.label1.Name = "label1";
    this.label1.Size = new Size(225, 27);
    this.label1.TabIndex = 2;
    this.label1.Text = "My Masterpassword";
    this.label1.TextAlign = ContentAlignment.MiddleCenter;
    // 
    // TlpMain
    // 
    this.TlpMain.ColumnCount = 1;
    this.TlpMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
    this.TlpMain.Controls.Add(this.BtToLI, 0, 1);
    this.TlpMain.Controls.Add(this.BtSetLI, 0, 2);
    this.TlpMain.Controls.Add(this.label1, 0, 4);
    this.TlpMain.Controls.Add(this.TbMPw, 0, 5);
    this.TlpMain.Dock = DockStyle.Fill;
    this.TlpMain.Location = new Point(0, 0);
    this.TlpMain.Name = "TlpMain";
    this.TlpMain.RowCount = 7;
    this.TlpMain.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
    this.TlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
    this.TlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
    this.TlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
    this.TlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
    this.TlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 150F));
    this.TlpMain.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
    this.TlpMain.Size = new Size(682, 466);
    this.TlpMain.TabIndex = 3;
    // 
    // MyFrmApp
    // 
    this.AutoScaleDimensions = new SizeF(13F, 26F);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.BackColor = Color.White;
    this.ClientSize = new Size(682, 466);
    this.Controls.Add(this.TlpMain);
    this.Font = new Font("Arial", 14F);
    this.Margin = new Padding(5, 4, 5, 4);
    this.Name = "MyFrmApp";
    this.StartPosition = FormStartPosition.CenterScreen;
    this.Text = "MyFrmApp";
    this.FormClosed += this.MyFrmApp_FormClosed;
    this.TlpMain.ResumeLayout(false);
    this.TlpMain.PerformLayout();
    this.ResumeLayout(false);
  }

  #endregion

  private Button BtToLI;
  private Button BtSetLI;
  private TextBox TbMPw;
  private Label label1;
  private TableLayoutPanel TlpMain;
}