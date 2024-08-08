


namespace michele.natale.LoginSystems.Views;
  
partial class UcLoginEx
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
    this.TlpMain = new TableLayoutPanel();
    this.LbUserName = new Label();
    this.TbUserName = new TextBox();
    this.LbPw = new Label();
    this.TbPw = new TextBox();
    this.BtLogin = new Button();
    this.LlPwForget = new LinkLabel();
    this.LlRegist = new LinkLabel();
    this.BtCancel = new Button();
    this.BsUcLogin = new BindingSource(this.components);
    this.TlpMain.SuspendLayout();
    ((System.ComponentModel.ISupportInitialize)this.BsUcLogin).BeginInit();
    this.SuspendLayout();
    // 
    // TlpMain
    // 
    this.TlpMain.ColumnCount = 4;
    this.TlpMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
    this.TlpMain.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 300F));
    this.TlpMain.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 300F));
    this.TlpMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
    this.TlpMain.Controls.Add(this.LbUserName, 1, 1);
    this.TlpMain.Controls.Add(this.TbUserName, 1, 2);
    this.TlpMain.Controls.Add(this.LbPw, 1, 4);
    this.TlpMain.Controls.Add(this.TbPw, 1, 5);
    this.TlpMain.Controls.Add(this.BtLogin, 1, 7);
    this.TlpMain.Controls.Add(this.LlPwForget, 1, 8);
    this.TlpMain.Controls.Add(this.LlRegist, 1, 9);
    this.TlpMain.Controls.Add(this.BtCancel, 2, 7);
    this.TlpMain.Dock = DockStyle.Fill;
    this.TlpMain.Location = new Point(0, 0);
    this.TlpMain.Name = "TlpMain";
    this.TlpMain.RowCount = 11;
    this.TlpMain.RowStyles.Add(new RowStyle(SizeType.Percent, 49.9999962F));
    this.TlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
    this.TlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
    this.TlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
    this.TlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
    this.TlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
    this.TlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
    this.TlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));
    this.TlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
    this.TlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
    this.TlpMain.RowStyles.Add(new RowStyle(SizeType.Percent, 50.0000076F));
    this.TlpMain.Size = new Size(630, 600);
    this.TlpMain.TabIndex = 100;
    // 
    // LbUserName
    // 
    this.LbUserName.Anchor = AnchorStyles.None;
    this.LbUserName.AutoSize = true;
    this.TlpMain.SetColumnSpan(this.LbUserName, 2);
    this.LbUserName.Location = new Point(159, 110);
    this.LbUserName.Name = "LbUserName";
    this.LbUserName.Size = new Size(311, 27);
    this.LbUserName.TabIndex = 101;
    this.LbUserName.Text = "Email Address or Username";
    // 
    // TbUserName
    // 
    this.TbUserName.Anchor = AnchorStyles.None;
    this.TlpMain.SetColumnSpan(this.TbUserName, 2);
    this.TbUserName.Location = new Point(30, 157);
    this.TbUserName.Name = "TbUserName";
    this.TbUserName.Size = new Size(570, 34);
    this.TbUserName.TabIndex = 1;
    this.TbUserName.TextAlign = HorizontalAlignment.Center;
    // 
    // LbPw
    // 
    this.LbPw.Anchor = AnchorStyles.None;
    this.LbPw.AutoSize = true;
    this.TlpMain.SetColumnSpan(this.LbPw, 2);
    this.LbPw.Location = new Point(256, 230);
    this.LbPw.Name = "LbPw";
    this.LbPw.Size = new Size(117, 27);
    this.LbPw.TabIndex = 102;
    this.LbPw.Text = "Password";
    // 
    // TbPw
    // 
    this.TbPw.Anchor = AnchorStyles.None;
    this.TlpMain.SetColumnSpan(this.TbPw, 2);
    this.TbPw.Location = new Point(30, 277);
    this.TbPw.Name = "TbPw";
    this.TbPw.PasswordChar = '*';
    this.TbPw.Size = new Size(570, 34);
    this.TbPw.TabIndex = 2;
    this.TbPw.TextAlign = HorizontalAlignment.Center;
    this.TbPw.UseSystemPasswordChar = true;
    // 
    // BtLogin
    // 
    this.BtLogin.Anchor = AnchorStyles.None;
    this.BtLogin.Location = new Point(45, 347);
    this.BtLogin.Name = "BtLogin";
    this.BtLogin.Size = new Size(240, 43);
    this.BtLogin.TabIndex = 103;
    this.BtLogin.Text = "Login";
    this.BtLogin.UseVisualStyleBackColor = true;
    // 
    // LlPwForget
    // 
    this.LlPwForget.Anchor = AnchorStyles.None;
    this.LlPwForget.AutoSize = true;
    this.TlpMain.SetColumnSpan(this.LlPwForget, 2);
    this.LlPwForget.Font = new Font("Arial", 14F, FontStyle.Italic);
    this.LlPwForget.Location = new Point(118, 410);
    this.LlPwForget.Name = "LlPwForget";
    this.LlPwForget.Size = new Size(393, 28);
    this.LlPwForget.TabIndex = 104;
    this.LlPwForget.TabStop = true;
    this.LlPwForget.Text = "Have you forgotten your password ?";
    this.LlPwForget.TextAlign = ContentAlignment.MiddleCenter;
    this.LlPwForget.LinkClicked += this.UcLogin_Click;
    // 
    // LlRegist
    // 
    this.LlRegist.Anchor = AnchorStyles.None;
    this.LlRegist.AutoSize = true;
    this.TlpMain.SetColumnSpan(this.LlRegist, 2);
    this.LlRegist.Enabled = false;
    this.LlRegist.Font = new Font("Arial", 14F, FontStyle.Italic);
    this.LlRegist.Location = new Point(105, 460);
    this.LlRegist.Name = "LlRegist";
    this.LlRegist.Size = new Size(420, 28);
    this.LlRegist.TabIndex = 104;
    this.LlRegist.TabStop = true;
    this.LlRegist.Text = "Have you not created an account yet ?";
    this.LlRegist.TextAlign = ContentAlignment.MiddleCenter;
    this.LlRegist.LinkClicked += this.UcLogin_Click;
    // 
    // BtCancel
    // 
    this.BtCancel.Anchor = AnchorStyles.None;
    this.BtCancel.Location = new Point(345, 347);
    this.BtCancel.Name = "BtCancel";
    this.BtCancel.Size = new Size(240, 43);
    this.BtCancel.TabIndex = 103;
    this.BtCancel.Text = "Cancel";
    this.BtCancel.UseVisualStyleBackColor = true;
    // 
    // UcLoginEx
    // 
    this.AutoScaleDimensions = new SizeF(13F, 26F);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.BackColor = Color.Transparent;
    this.Controls.Add(this.TlpMain);
    this.Enabled = false;
    this.Font = new Font("Arial", 14F);
    this.Margin = new Padding(5, 4, 5, 4);
    this.Name = "UcLoginEx";
    this.Size = new Size(630, 600);
    this.TlpMain.ResumeLayout(false);
    this.TlpMain.PerformLayout();
    ((System.ComponentModel.ISupportInitialize)this.BsUcLogin).EndInit();
    this.ResumeLayout(false);
  }

  #endregion

  private Label LbPw;
  private TextBox TbPw;
  private Button BtLogin;
  private Button BtCancel;
  private Label LbUserName;
  private TextBox TbUserName;
  private LinkLabel LlRegist;
  private LinkLabel LlPwForget;
  private TableLayoutPanel TlpMain; 

  private BindingSource BsUcLogin { get; set; } = null!;

}
