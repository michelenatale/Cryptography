

using michele.natale.LoginSystems.ViewModels;


namespace michele.natale.LoginSystems.Views;

partial class UcRegistEx
{
  /// <summary> 
  /// Erforderliche Designervariable.
  /// </summary>
  private System.ComponentModel.IContainer components = null;

  /// <summary> 
  /// Verwendete Ressourcen bereinigen.
  /// </summary>
  /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
  protected override void Dispose(bool disposing)
  {
    if (disposing && (components != null))
    {
      this.OnClose();
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
    this.LbUserName = new Label();
    this.TbUserName = new TextBox();
    this.LbPw = new Label();
    this.TbPw = new TextBox();
    this.LlGetLogin = new LinkLabel();
    this.LlPwForget = new LinkLabel();
    this.BtRegist = new Button();
    this.TbEmail = new TextBox();
    this.LbEMail = new Label();
    this.LbRepeatPw = new Label();
    this.TbRepeatPw = new TextBox();
    this.BtCancel = new Button();
    this.BsUcRegist = new BindingSource(this.components);
    this.TlbRegist.SuspendLayout();
    ((System.ComponentModel.ISupportInitialize)this.BsUcRegist).BeginInit();
    this.SuspendLayout();
    // 
    // TlbRegist
    // 
    this.TlbRegist.ColumnCount = 4;
    this.TlbRegist.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
    this.TlbRegist.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 300F));
    this.TlbRegist.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 300F));
    this.TlbRegist.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
    this.TlbRegist.Controls.Add(this.LbUserName, 1, 1);
    this.TlbRegist.Controls.Add(this.TbUserName, 1, 2);
    this.TlbRegist.Controls.Add(this.LbPw, 1, 4);
    this.TlbRegist.Controls.Add(this.TbPw, 1, 5);
    this.TlbRegist.Controls.Add(this.LlGetLogin, 1, 15);
    this.TlbRegist.Controls.Add(this.LlPwForget, 1, 14);
    this.TlbRegist.Controls.Add(this.BtRegist, 1, 13);
    this.TlbRegist.Controls.Add(this.TbEmail, 1, 11);
    this.TlbRegist.Controls.Add(this.LbEMail, 1, 10);
    this.TlbRegist.Controls.Add(this.LbRepeatPw, 1, 7);
    this.TlbRegist.Controls.Add(this.TbRepeatPw, 1, 8);
    this.TlbRegist.Controls.Add(this.BtCancel, 2, 13);
    this.TlbRegist.Dock = DockStyle.Fill;
    this.TlbRegist.Location = new Point(0, 0);
    this.TlbRegist.Name = "TlbRegist";
    this.TlbRegist.RowCount = 17;
    this.TlbRegist.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
    this.TlbRegist.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
    this.TlbRegist.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
    this.TlbRegist.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
    this.TlbRegist.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
    this.TlbRegist.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
    this.TlbRegist.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
    this.TlbRegist.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
    this.TlbRegist.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
    this.TlbRegist.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
    this.TlbRegist.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
    this.TlbRegist.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
    this.TlbRegist.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
    this.TlbRegist.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));
    this.TlbRegist.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
    this.TlbRegist.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
    this.TlbRegist.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
    this.TlbRegist.Size = new Size(660, 670);
    this.TlbRegist.TabIndex = 0;
    // 
    // LbUserName
    // 
    this.LbUserName.Anchor = AnchorStyles.None;
    this.LbUserName.AutoSize = true;
    this.TlbRegist.SetColumnSpan(this.LbUserName, 2);
    this.LbUserName.Location = new Point(269, 26);
    this.LbUserName.Name = "LbUserName";
    this.LbUserName.Size = new Size(122, 27);
    this.LbUserName.TabIndex = 100;
    this.LbUserName.Text = "Username";
    // 
    // TbUserName
    // 
    this.TbUserName.Anchor = AnchorStyles.None;
    this.TlbRegist.SetColumnSpan(this.TbUserName, 2);
    this.TbUserName.Location = new Point(45, 73);
    this.TbUserName.Name = "TbUserName";
    this.TbUserName.Size = new Size(570, 34);
    this.TbUserName.TabIndex = 1;
    this.TbUserName.TextAlign = HorizontalAlignment.Center;
    // 
    // LbPw
    // 
    this.LbPw.Anchor = AnchorStyles.None;
    this.LbPw.AutoSize = true;
    this.TlbRegist.SetColumnSpan(this.LbPw, 2);
    this.LbPw.Location = new Point(271, 146);
    this.LbPw.Name = "LbPw";
    this.LbPw.Size = new Size(117, 27);
    this.LbPw.TabIndex = 101;
    this.LbPw.Text = "Password";
    // 
    // TbPw
    // 
    this.TbPw.Anchor = AnchorStyles.None;
    this.TlbRegist.SetColumnSpan(this.TbPw, 2);
    this.TbPw.Location = new Point(45, 193);
    this.TbPw.Name = "TbPw";
    this.TbPw.PasswordChar = '*';
    this.TbPw.Size = new Size(570, 34);
    this.TbPw.TabIndex = 2;
    this.TbPw.TextAlign = HorizontalAlignment.Center;
    this.TbPw.UseSystemPasswordChar = true;
    // 
    // LlGetLogin
    // 
    this.LlGetLogin.Anchor = AnchorStyles.None;
    this.LlGetLogin.AutoSize = true;
    this.TlbRegist.SetColumnSpan(this.LlGetLogin, 2);
    this.LlGetLogin.Font = new Font("Arial", 14F, FontStyle.Italic);
    this.LlGetLogin.Location = new Point(117, 616);
    this.LlGetLogin.Name = "LlGetLogin";
    this.LlGetLogin.Size = new Size(426, 28);
    this.LlGetLogin.TabIndex = 8;
    this.LlGetLogin.TabStop = true;
    this.LlGetLogin.Text = "Have you already created an account ?";
    this.LlGetLogin.TextAlign = ContentAlignment.MiddleCenter;
    this.LlGetLogin.LinkClicked += this.UcRegist_Click;
    // 
    // LlPwForget
    // 
    this.LlPwForget.Anchor = AnchorStyles.None;
    this.LlPwForget.AutoSize = true;
    this.TlbRegist.SetColumnSpan(this.LlPwForget, 2);
    this.LlPwForget.Enabled = false;
    this.LlPwForget.Font = new Font("Arial", 14F, FontStyle.Italic);
    this.LlPwForget.Location = new Point(133, 566);
    this.LlPwForget.Name = "LlPwForget";
    this.LlPwForget.Size = new Size(393, 28);
    this.LlPwForget.TabIndex = 7;
    this.LlPwForget.TabStop = true;
    this.LlPwForget.Text = "Have you forgotten your password ?";
    this.LlPwForget.TextAlign = ContentAlignment.MiddleCenter;
    this.LlPwForget.LinkClicked += this.UcRegist_Click;
    // 
    // BtRegist
    // 
    this.BtRegist.Anchor = AnchorStyles.None;
    this.BtRegist.Location = new Point(60, 503);
    this.BtRegist.Name = "BtRegist";
    this.BtRegist.Size = new Size(240, 43);
    this.BtRegist.TabIndex = 5;
    this.BtRegist.Text = "Regist";
    this.BtRegist.UseVisualStyleBackColor = true;
    // 
    // TbEmail
    // 
    this.TbEmail.Anchor = AnchorStyles.None;
    this.TlbRegist.SetColumnSpan(this.TbEmail, 2);
    this.TbEmail.Location = new Point(45, 433);
    this.TbEmail.Name = "TbEmail";
    this.TbEmail.Size = new Size(570, 34);
    this.TbEmail.TabIndex = 4;
    this.TbEmail.TextAlign = HorizontalAlignment.Center;
    // 
    // LbEMail
    // 
    this.LbEMail.Anchor = AnchorStyles.None;
    this.LbEMail.AutoSize = true;
    this.TlbRegist.SetColumnSpan(this.LbEMail, 2);
    this.LbEMail.Location = new Point(294, 386);
    this.LbEMail.Name = "LbEMail";
    this.LbEMail.Size = new Size(71, 27);
    this.LbEMail.TabIndex = 103;
    this.LbEMail.Text = "EMail";
    // 
    // LbRepeatPw
    // 
    this.LbRepeatPw.Anchor = AnchorStyles.None;
    this.LbRepeatPw.AutoSize = true;
    this.TlbRegist.SetColumnSpan(this.LbRepeatPw, 2);
    this.LbRepeatPw.Location = new Point(229, 266);
    this.LbRepeatPw.Name = "LbRepeatPw";
    this.LbRepeatPw.Size = new Size(201, 27);
    this.LbRepeatPw.TabIndex = 102;
    this.LbRepeatPw.Text = "Repeat Password";
    // 
    // TbRepeatPw
    // 
    this.TbRepeatPw.Anchor = AnchorStyles.None;
    this.TlbRegist.SetColumnSpan(this.TbRepeatPw, 2);
    this.TbRepeatPw.Location = new Point(45, 313);
    this.TbRepeatPw.Name = "TbRepeatPw";
    this.TbRepeatPw.PasswordChar = '*';
    this.TbRepeatPw.Size = new Size(570, 34);
    this.TbRepeatPw.TabIndex = 3;
    this.TbRepeatPw.TextAlign = HorizontalAlignment.Center;
    this.TbRepeatPw.UseSystemPasswordChar = true;
    // 
    // BtCancel
    // 
    this.BtCancel.Anchor = AnchorStyles.None;
    this.BtCancel.Location = new Point(360, 503);
    this.BtCancel.Name = "BtCancel";
    this.BtCancel.Size = new Size(240, 43);
    this.BtCancel.TabIndex = 6;
    this.BtCancel.Text = "Cancel";
    this.BtCancel.UseVisualStyleBackColor = true;
    // 
    // UcRegistEx
    // 
    this.AutoScaleDimensions = new SizeF(13F, 26F);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.BackColor = Color.Transparent;
    this.Controls.Add(this.TlbRegist);
    this.Enabled = false;
    this.Font = new Font("Arial", 14F);
    this.Margin = new Padding(5, 4, 5, 4);
    this.Name = "UcRegistEx";
    this.Size = new Size(660, 670);
    this.TlbRegist.ResumeLayout(false);
    this.TlbRegist.PerformLayout();
    ((System.ComponentModel.ISupportInitialize)this.BsUcRegist).EndInit();
    this.ResumeLayout(false);
  }

  #endregion

  private Button BtRegist;
  private Label LbPw;
  private TextBox TbPw;
  private Label LbEMail;
  private TextBox TbEmail; 
  private Label LbUserName;
  private TextBox TbUserName;
  private LinkLabel LlPwForget;
  private LinkLabel LlGetLogin;
  private TableLayoutPanel TlbRegist;
  private Label LbRepeatPw;
  private TextBox TbRepeatPw;
  private Button BtCancel;

  private BindingSource BsUcRegist { get; set; } = null!;
}
