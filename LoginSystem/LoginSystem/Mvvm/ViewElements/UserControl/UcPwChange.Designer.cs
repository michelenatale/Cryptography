
 
namespace michele.natale.LoginSystems.Views;

partial class UcPwChangeEx
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
    this.LbPw = new Label();
    this.TbPw = new TextBox();
    this.LbRPw = new Label();
    this.TbRPw = new TextBox();
    this.tableLayoutPanel1 = new TableLayoutPanel();
    this.BtPwChange = new Button();
    this.BtPwCancel = new Button();
    this.BsUcPwChange = new BindingSource(this.components);
    this.TlpMain.SuspendLayout();
    this.tableLayoutPanel1.SuspendLayout();
    ((System.ComponentModel.ISupportInitialize)this.BsUcPwChange).BeginInit();
    this.SuspendLayout();
    // 
    // TlpMain
    // 
    this.TlpMain.ColumnCount = 3;
    this.TlpMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
    this.TlpMain.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 600F));
    this.TlpMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
    this.TlpMain.Controls.Add(this.LbPw, 1, 1);
    this.TlpMain.Controls.Add(this.TbPw, 1, 2);
    this.TlpMain.Controls.Add(this.LbRPw, 1, 4);
    this.TlpMain.Controls.Add(this.TbRPw, 1, 5);
    this.TlpMain.Controls.Add(this.tableLayoutPanel1, 1, 7);
    this.TlpMain.Dock = DockStyle.Fill;
    this.TlpMain.Location = new Point(0, 0);
    this.TlpMain.Name = "TlpMain";
    this.TlpMain.RowCount = 9;
    this.TlpMain.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
    this.TlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
    this.TlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
    this.TlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
    this.TlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
    this.TlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
    this.TlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
    this.TlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 80F));
    this.TlpMain.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
    this.TlpMain.Size = new Size(630, 500);
    this.TlpMain.TabIndex = 101;
    // 
    // LbPw
    // 
    this.LbPw.Anchor = AnchorStyles.None;
    this.LbPw.AutoSize = true;
    this.LbPw.Location = new Point(229, 101);
    this.LbPw.Name = "LbPw";
    this.LbPw.Size = new Size(171, 27);
    this.LbPw.TabIndex = 101;
    this.LbPw.Text = "New Password";
    // 
    // TbPw
    // 
    this.TbPw.Anchor = AnchorStyles.None;
    this.TbPw.Location = new Point(30, 148);
    this.TbPw.Name = "TbPw";
    this.TbPw.PasswordChar = '*';
    this.TbPw.Size = new Size(570, 34);
    this.TbPw.TabIndex = 1;
    this.TbPw.TextAlign = HorizontalAlignment.Center;
    this.TbPw.UseSystemPasswordChar = true;
    // 
    // LbRPw
    // 
    this.LbRPw.Anchor = AnchorStyles.None;
    this.LbRPw.AutoSize = true;
    this.LbRPw.Location = new Point(214, 221);
    this.LbRPw.Name = "LbRPw";
    this.LbRPw.Size = new Size(201, 27);
    this.LbRPw.TabIndex = 102;
    this.LbRPw.Text = "Repeat Password";
    // 
    // TbRPw
    // 
    this.TbRPw.Anchor = AnchorStyles.None;
    this.TbRPw.Location = new Point(30, 268);
    this.TbRPw.Name = "TbRPw";
    this.TbRPw.PasswordChar = '*';
    this.TbRPw.Size = new Size(570, 34);
    this.TbRPw.TabIndex = 2;
    this.TbRPw.TextAlign = HorizontalAlignment.Center;
    this.TbRPw.UseSystemPasswordChar = true;
    // 
    // tableLayoutPanel1
    // 
    this.tableLayoutPanel1.ColumnCount = 2;
    this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
    this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
    this.tableLayoutPanel1.Controls.Add(this.BtPwChange, 0, 0);
    this.tableLayoutPanel1.Controls.Add(this.BtPwCancel, 1, 0);
    this.tableLayoutPanel1.Dock = DockStyle.Fill;
    this.tableLayoutPanel1.Location = new Point(18, 333);
    this.tableLayoutPanel1.Name = "tableLayoutPanel1";
    this.tableLayoutPanel1.RowCount = 1;
    this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
    this.tableLayoutPanel1.Size = new Size(594, 74);
    this.tableLayoutPanel1.TabIndex = 104;
    // 
    // BtPwChange
    // 
    this.BtPwChange.Anchor = AnchorStyles.None;
    this.BtPwChange.Location = new Point(28, 15);
    this.BtPwChange.Name = "BtPwChange";
    this.BtPwChange.Size = new Size(240, 43);
    this.BtPwChange.TabIndex = 103;
    this.BtPwChange.Text = "Password Change";
    this.BtPwChange.UseVisualStyleBackColor = true;
    // 
    // BtPwCancel
    // 
    this.BtPwCancel.Anchor = AnchorStyles.None;
    this.BtPwCancel.Location = new Point(325, 15);
    this.BtPwCancel.Name = "BtPwCancel";
    this.BtPwCancel.Size = new Size(240, 43);
    this.BtPwCancel.TabIndex = 103;
    this.BtPwCancel.Text = "Cancel";
    this.BtPwCancel.UseVisualStyleBackColor = true;
    // 
    // UcPwChangeEx
    // 
    this.AutoScaleDimensions = new SizeF(13F, 26F);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.BackColor = Color.Transparent;
    this.Controls.Add(this.TlpMain);
    this.Font = new Font("Arial", 14F);
    this.Margin = new Padding(5, 4, 5, 4);
    this.Name = "UcPwChangeEx";
    this.Size = new Size(630, 500);
    this.TlpMain.ResumeLayout(false);
    this.TlpMain.PerformLayout();
    this.tableLayoutPanel1.ResumeLayout(false);
    ((System.ComponentModel.ISupportInitialize)this.BsUcPwChange).EndInit();
    this.ResumeLayout(false);
  }

  #endregion

  private Label LbPw;
  private Label LbRPw;
  private TextBox TbPw;
  private TextBox TbRPw;
  private Button BtPwChange;
  private TableLayoutPanel TlpMain; 
  private TableLayoutPanel tableLayoutPanel1;
  private Button BtPwCancel;
  private BindingSource BsUcPwChange { get; set; } = null!;
}
