


namespace michele.natale.LoginSystems.Views;
 

partial class UcPwForgetEx
{
  /// <summary> 
  /// Erforderliche Designervariable.
  /// </summary>
  private System.ComponentModel.IContainer components = null;

  /// <summary> 
  /// Verwendete Ressourcen bereinigen.
  /// </summary>
  /// <param name="disposing">
  /// True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.
  /// </param>
  protected override void Dispose(bool disposing)
  {
    this.OnClose();
    if (disposing && (components != null))
    {
      components.Dispose();
    }
    base.Dispose(disposing);
  }

  #region Vom Komponenten-Designer generierter Code

  /// <summary> 
  /// Erforderliche Methode für die Designerunterstützung. 
  /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
  /// </summary>
  private void InitializeComponent()
  {
    this.components = new System.ComponentModel.Container();
    this.TlbRegist = new TableLayoutPanel();
    this.TlpButtons = new TableLayoutPanel();
    this.BtOk = new Button();
    this.BtCancel = new Button();
    this.LbPwForgetEMail = new Label();
    this.TbEmail = new TextBox();
    this.label4 = new Label();
    this.label3 = new Label();
    this.label2 = new Label();
    this.TbHost = new TextBox();
    this.label5 = new Label();
    this.TbUsername = new TextBox();
    this.TbPw = new TextBox();
    this.label6 = new Label();
    this.TbPort = new TextBox();
    this.label1 = new Label();
    this.label7 = new Label();
    this.TbRPw = new TextBox();
    this.CbSsl = new CheckBox();
    this.CbDelivery = new ComboBox();
    this.LlLogin = new LinkLabel();
    this.LlRegist = new LinkLabel();
    this.BsUcPwForget = new BindingSource(this.components);
    this.TlbRegist.SuspendLayout();
    this.TlpButtons.SuspendLayout();
    ((System.ComponentModel.ISupportInitialize)this.BsUcPwForget).BeginInit();
    this.SuspendLayout();
    // 
    // TlbRegist
    // 
    this.TlbRegist.ColumnCount = 4;
    this.TlbRegist.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
    this.TlbRegist.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200F));
    this.TlbRegist.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 400F));
    this.TlbRegist.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
    this.TlbRegist.Controls.Add(this.TlpButtons, 1, 9);
    this.TlbRegist.Controls.Add(this.LbPwForgetEMail, 1, 1);
    this.TlbRegist.Controls.Add(this.TbEmail, 2, 1);
    this.TlbRegist.Controls.Add(this.label4, 1, 8);
    this.TlbRegist.Controls.Add(this.label3, 1, 7);
    this.TlbRegist.Controls.Add(this.label2, 1, 6);
    this.TlbRegist.Controls.Add(this.TbHost, 2, 6);
    this.TlbRegist.Controls.Add(this.label5, 1, 2);
    this.TlbRegist.Controls.Add(this.TbUsername, 2, 2);
    this.TlbRegist.Controls.Add(this.TbPw, 2, 3);
    this.TlbRegist.Controls.Add(this.label6, 1, 3);
    this.TlbRegist.Controls.Add(this.TbPort, 2, 5);
    this.TlbRegist.Controls.Add(this.label1, 1, 5);
    this.TlbRegist.Controls.Add(this.label7, 1, 4);
    this.TlbRegist.Controls.Add(this.TbRPw, 2, 4);
    this.TlbRegist.Controls.Add(this.CbSsl, 2, 8);
    this.TlbRegist.Controls.Add(this.CbDelivery, 2, 7);
    this.TlbRegist.Controls.Add(this.LlLogin, 1, 11);
    this.TlbRegist.Controls.Add(this.LlRegist, 1, 12);
    this.TlbRegist.Dock = DockStyle.Fill;
    this.TlbRegist.Location = new Point(0, 0);
    this.TlbRegist.Name = "TlbRegist";
    this.TlbRegist.RowCount = 14;
    this.TlbRegist.RowStyles.Add(new RowStyle(SizeType.Percent, 49.9999962F));
    this.TlbRegist.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
    this.TlbRegist.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
    this.TlbRegist.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
    this.TlbRegist.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
    this.TlbRegist.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
    this.TlbRegist.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
    this.TlbRegist.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
    this.TlbRegist.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
    this.TlbRegist.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
    this.TlbRegist.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));
    this.TlbRegist.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
    this.TlbRegist.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
    this.TlbRegist.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
    this.TlbRegist.Size = new Size(630, 600);
    this.TlbRegist.TabIndex = 100;
    // 
    // TlpButtons
    // 
    this.TlpButtons.ColumnCount = 2;
    this.TlbRegist.SetColumnSpan(this.TlpButtons, 2);
    this.TlpButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
    this.TlpButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
    this.TlpButtons.Controls.Add(this.BtOk, 0, 0);
    this.TlpButtons.Controls.Add(this.BtCancel, 1, 0);
    this.TlpButtons.Dock = DockStyle.Fill;
    this.TlpButtons.Location = new Point(18, 412);
    this.TlpButtons.Name = "TlpButtons";
    this.TlpButtons.RowCount = 1;
    this.TlbRegist.SetRowSpan(this.TlpButtons, 2);
    this.TlpButtons.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
    this.TlpButtons.Size = new Size(594, 74);
    this.TlpButtons.TabIndex = 101;
    // 
    // BtOk
    // 
    this.BtOk.Anchor = AnchorStyles.None;
    this.BtOk.Location = new Point(28, 15);
    this.BtOk.Name = "BtOk";
    this.BtOk.Size = new Size(240, 43);
    this.BtOk.TabIndex = 9;
    this.BtOk.Text = "Request a Reset";
    this.BtOk.UseVisualStyleBackColor = true;
    // 
    // BtCancel
    // 
    this.BtCancel.Anchor = AnchorStyles.None;
    this.BtCancel.Location = new Point(325, 15);
    this.BtCancel.Name = "BtCancel";
    this.BtCancel.Size = new Size(240, 43);
    this.BtCancel.TabIndex = 9;
    this.BtCancel.Text = "Cancel";
    this.BtCancel.UseVisualStyleBackColor = true;
    // 
    // LbPwForgetEMail
    // 
    this.LbPwForgetEMail.Anchor = AnchorStyles.Left;
    this.LbPwForgetEMail.AutoSize = true;
    this.LbPwForgetEMail.Location = new Point(18, 20);
    this.LbPwForgetEMail.Name = "LbPwForgetEMail";
    this.LbPwForgetEMail.Size = new Size(165, 27);
    this.LbPwForgetEMail.TabIndex = 101;
    this.LbPwForgetEMail.Text = "EMail From/To";
    // 
    // TbEmail
    // 
    this.TbEmail.Anchor = AnchorStyles.None;
    this.TbEmail.Location = new Point(230, 17);
    this.TbEmail.Name = "TbEmail";
    this.TbEmail.Size = new Size(370, 34);
    this.TbEmail.TabIndex = 1;
    this.TbEmail.TextAlign = HorizontalAlignment.Center;
    // 
    // label4
    // 
    this.label4.Anchor = AnchorStyles.Left;
    this.label4.AutoSize = true;
    this.label4.Location = new Point(18, 370);
    this.label4.Name = "label4";
    this.label4.Size = new Size(122, 27);
    this.label4.TabIndex = 108;
    this.label4.Text = "EnableSsl";
    // 
    // label3
    // 
    this.label3.Anchor = AnchorStyles.Left;
    this.label3.AutoSize = true;
    this.label3.Location = new Point(18, 320);
    this.label3.Name = "label3";
    this.label3.Size = new Size(177, 27);
    this.label3.TabIndex = 107;
    this.label3.Text = "DeliveryMethod";
    // 
    // label2
    // 
    this.label2.Anchor = AnchorStyles.Left;
    this.label2.AutoSize = true;
    this.label2.Location = new Point(18, 270);
    this.label2.Name = "label2";
    this.label2.Size = new Size(61, 27);
    this.label2.TabIndex = 106;
    this.label2.Text = "Host";
    // 
    // TbHost
    // 
    this.TbHost.Anchor = AnchorStyles.None;
    this.TbHost.Location = new Point(230, 267);
    this.TbHost.Name = "TbHost";
    this.TbHost.Size = new Size(370, 34);
    this.TbHost.TabIndex = 6;
    this.TbHost.TextAlign = HorizontalAlignment.Center;
    // 
    // label5
    // 
    this.label5.Anchor = AnchorStyles.Left;
    this.label5.AutoSize = true;
    this.label5.Location = new Point(18, 70);
    this.label5.Name = "label5";
    this.label5.Size = new Size(122, 27);
    this.label5.TabIndex = 102;
    this.label5.Text = "Username";
    // 
    // TbUsername
    // 
    this.TbUsername.Anchor = AnchorStyles.None;
    this.TbUsername.Location = new Point(230, 67);
    this.TbUsername.Name = "TbUsername";
    this.TbUsername.Size = new Size(370, 34);
    this.TbUsername.TabIndex = 2;
    this.TbUsername.TextAlign = HorizontalAlignment.Center;
    // 
    // TbPw
    // 
    this.TbPw.Anchor = AnchorStyles.None;
    this.TbPw.Location = new Point(230, 117);
    this.TbPw.Name = "TbPw";
    this.TbPw.PasswordChar = '*';
    this.TbPw.Size = new Size(370, 34);
    this.TbPw.TabIndex = 3;
    this.TbPw.TextAlign = HorizontalAlignment.Center;
    this.TbPw.UseSystemPasswordChar = true;
    // 
    // label6
    // 
    this.label6.Anchor = AnchorStyles.Left;
    this.label6.AutoSize = true;
    this.label6.Location = new Point(18, 120);
    this.label6.Name = "label6";
    this.label6.Size = new Size(117, 27);
    this.label6.TabIndex = 103;
    this.label6.Text = "Password";
    // 
    // TbPort
    // 
    this.TbPort.Anchor = AnchorStyles.None;
    this.TbPort.Location = new Point(230, 217);
    this.TbPort.Name = "TbPort";
    this.TbPort.Size = new Size(370, 34);
    this.TbPort.TabIndex = 5;
    this.TbPort.TextAlign = HorizontalAlignment.Center;
    // 
    // label1
    // 
    this.label1.Anchor = AnchorStyles.Left;
    this.label1.AutoSize = true;
    this.label1.Location = new Point(18, 220);
    this.label1.Name = "label1";
    this.label1.Size = new Size(56, 27);
    this.label1.TabIndex = 105;
    this.label1.Text = "Port";
    // 
    // label7
    // 
    this.label7.Anchor = AnchorStyles.Left;
    this.label7.AutoSize = true;
    this.label7.Location = new Point(18, 170);
    this.label7.Name = "label7";
    this.label7.Size = new Size(129, 27);
    this.label7.TabIndex = 104;
    this.label7.Text = "Repeat Pw";
    // 
    // TbRPw
    // 
    this.TbRPw.Anchor = AnchorStyles.None;
    this.TbRPw.Location = new Point(230, 167);
    this.TbRPw.Name = "TbRPw";
    this.TbRPw.PasswordChar = '*';
    this.TbRPw.Size = new Size(370, 34);
    this.TbRPw.TabIndex = 4;
    this.TbRPw.TextAlign = HorizontalAlignment.Center;
    this.TbRPw.UseSystemPasswordChar = true;
    // 
    // CbSsl
    // 
    this.CbSsl.Anchor = AnchorStyles.None;
    this.CbSsl.AutoSize = true;
    this.CbSsl.Checked = true;
    this.CbSsl.CheckState = CheckState.Checked;
    this.CbSsl.Location = new Point(406, 375);
    this.CbSsl.Name = "CbSsl";
    this.CbSsl.Size = new Size(18, 17);
    this.CbSsl.TabIndex = 8;
    this.CbSsl.TextAlign = ContentAlignment.MiddleCenter;
    this.CbSsl.UseVisualStyleBackColor = true;
    // 
    // CbDelivery
    // 
    this.CbDelivery.Anchor = AnchorStyles.None;
    this.CbDelivery.DrawMode = DrawMode.OwnerDrawFixed;
    this.CbDelivery.DropDownStyle = ComboBoxStyle.DropDownList;
    this.CbDelivery.FormattingEnabled = true;
    this.CbDelivery.Items.AddRange(new object[] { "Network  ", "SpecifiedPickupDirectory", "PickupDirectoryFromIis" });
    this.CbDelivery.Location = new Point(230, 320);
    this.CbDelivery.Name = "CbDelivery";
    this.CbDelivery.Size = new Size(370, 35);
    this.CbDelivery.TabIndex = 7;
    this.CbDelivery.DrawItem += this.CbDelivery_DrawItem;
    // 
    // LlLogin
    // 
    this.LlLogin.Anchor = AnchorStyles.None;
    this.LlLogin.AutoSize = true;
    this.TlbRegist.SetColumnSpan(this.LlLogin, 2);
    this.LlLogin.Font = new Font("Arial", 14F, FontStyle.Italic);
    this.LlLogin.Location = new Point(102, 500);
    this.LlLogin.Name = "LlLogin";
    this.LlLogin.Size = new Size(426, 28);
    this.LlLogin.TabIndex = 10;
    this.LlLogin.TabStop = true;
    this.LlLogin.Text = "Have you already created an account ?";
    this.LlLogin.TextAlign = ContentAlignment.MiddleCenter;
    this.LlLogin.LinkClicked += this.UcPwForget_Click;
    // 
    // LlRegist
    // 
    this.LlRegist.Anchor = AnchorStyles.None;
    this.LlRegist.AutoSize = true;
    this.TlbRegist.SetColumnSpan(this.LlRegist, 2);
    this.LlRegist.Enabled = false;
    this.LlRegist.Font = new Font("Arial", 14F, FontStyle.Italic);
    this.LlRegist.Location = new Point(105, 550);
    this.LlRegist.Name = "LlRegist";
    this.LlRegist.Size = new Size(420, 28);
    this.LlRegist.TabIndex = 11;
    this.LlRegist.TabStop = true;
    this.LlRegist.Text = "Have you not created an account yet ?";
    this.LlRegist.TextAlign = ContentAlignment.MiddleCenter;
    this.LlRegist.LinkClicked += this.UcPwForget_Click;
    // 
    // UcPwForgetEx
    // 
    this.AutoScaleDimensions = new SizeF(13F, 26F);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.BackColor = Color.Transparent;
    this.Controls.Add(this.TlbRegist);
    this.Enabled = false;
    this.Font = new Font("Arial", 14F);
    this.Margin = new Padding(5, 4, 5, 4);
    this.Name = "UcPwForgetEx";
    this.Size = new Size(630, 600);
    this.Load += this.UcPwForget_Load;
    this.TlbRegist.ResumeLayout(false);
    this.TlbRegist.PerformLayout();
    this.TlpButtons.ResumeLayout(false);
    ((System.ComponentModel.ISupportInitialize)this.BsUcPwForget).EndInit();
    this.ResumeLayout(false);
  }

  #endregion

  private Label label1;
  private Label label2;
  private Label label3;
  private Label label4;
  private Label label5;
  private Label label6;
  private Label label7;
  private CheckBox CbSsl;
  private Button BtOk;
  private ComboBox CbDelivery;
  private TextBox TbPw;
  private Label LbPwForgetEMail;
  private TextBox TbRPw;
  private TextBox TbPort;
  private TextBox TbHost;
  private TextBox TbEmail; 
  private TextBox TbUsername;
  private LinkLabel LlRegist;
  private TableLayoutPanel TlbRegist;
  private LinkLabel LlLogin;
  private TableLayoutPanel TlpButtons;
  private Button BtCancel;
  private BindingSource BsUcPwForget { get; set; } = null!;
}

