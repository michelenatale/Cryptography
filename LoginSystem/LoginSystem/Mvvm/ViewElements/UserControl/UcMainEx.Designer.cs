
 

namespace michele.natale.LoginSystems.Views;

partial class UcMainEx
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
    this.BtChangePw = new Button();
    this.LbMain = new Label();
    this.label2 = new Label();
    this.BtInfoLoginSystem = new Button();
    this.BsUcMain = new BindingSource(this.components);
    this.TlpMain.SuspendLayout();
    ((System.ComponentModel.ISupportInitialize)this.BsUcMain).BeginInit();
    this.SuspendLayout();
    // 
    // TlpMain
    // 
    this.TlpMain.ColumnCount = 1;
    this.TlpMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
    this.TlpMain.Controls.Add(this.BtChangePw, 0, 1);
    this.TlpMain.Controls.Add(this.LbMain, 0, 0);
    this.TlpMain.Controls.Add(this.label2, 0, 3);
    this.TlpMain.Controls.Add(this.BtInfoLoginSystem, 0, 2);
    this.TlpMain.Dock = DockStyle.Fill;
    this.TlpMain.Location = new Point(0, 0);
    this.TlpMain.Name = "TlpMain";
    this.TlpMain.RowCount = 4;
    this.TlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 150F));
    this.TlpMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
    this.TlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 90F));
    this.TlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 80F));
    this.TlpMain.Size = new Size(633, 534);
    this.TlpMain.TabIndex = 0;
    // 
    // BtChangePw
    // 
    this.BtChangePw.Anchor = AnchorStyles.Bottom;
    this.BtChangePw.Location = new Point(156, 311);
    this.BtChangePw.Margin = new Padding(10);
    this.BtChangePw.Name = "BtChangePw";
    this.BtChangePw.Size = new Size(320, 43);
    this.BtChangePw.TabIndex = 1;
    this.BtChangePw.Text = "Change Password";
    this.BtChangePw.UseVisualStyleBackColor = true;
    // 
    // LbMain
    // 
    this.LbMain.Dock = DockStyle.Fill;
    this.LbMain.Font = new Font("Arial", 32F, FontStyle.Bold);
    this.LbMain.ForeColor = Color.Teal;
    this.LbMain.Location = new Point(3, 0);
    this.LbMain.Name = "LbMain";
    this.LbMain.Size = new Size(627, 150);
    this.LbMain.TabIndex = 0;
    this.LbMain.Text = "© LoginSystem 2024";
    this.LbMain.TextAlign = ContentAlignment.MiddleCenter;
    // 
    // label2
    // 
    this.label2.Dock = DockStyle.Fill;
    this.label2.Font = new Font("Arial", 14F, FontStyle.Bold | FontStyle.Italic);
    this.label2.ForeColor = Color.DarkGoldenrod;
    this.label2.Location = new Point(3, 454);
    this.label2.Name = "label2";
    this.label2.Padding = new Padding(3, 3, 3, 6);
    this.label2.Size = new Size(627, 80);
    this.label2.TabIndex = 0;
    this.label2.Text = "© Michele Natale 2024";
    this.label2.TextAlign = ContentAlignment.BottomRight;
    // 
    // BtInfoLoginSystem
    // 
    this.BtInfoLoginSystem.Anchor = AnchorStyles.Top;
    this.BtInfoLoginSystem.Location = new Point(156, 374);
    this.BtInfoLoginSystem.Margin = new Padding(10);
    this.BtInfoLoginSystem.Name = "BtInfoLoginSystem";
    this.BtInfoLoginSystem.Size = new Size(320, 43);
    this.BtInfoLoginSystem.TabIndex = 1;
    this.BtInfoLoginSystem.Text = "Info © LoginSystem 2024";
    this.BtInfoLoginSystem.UseVisualStyleBackColor = true;
    // 
    // UcMainEx
    // 
    this.AutoScaleDimensions = new SizeF(13F, 26F);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.BackColor = Color.Transparent;
    this.BackgroundImage = LoginSystem.Properties.Resources.watermark;
    this.Controls.Add(this.TlpMain);
    this.DoubleBuffered = true;
    this.Font = new Font("Arial", 14F);
    this.Margin = new Padding(5, 4, 5, 4);
    this.Name = "UcMainEx";
    this.Size = new Size(633, 534);
    this.TlpMain.ResumeLayout(false);
    ((System.ComponentModel.ISupportInitialize)this.BsUcMain).EndInit();
    this.ResumeLayout(false);
  }

  #endregion

  private TableLayoutPanel TlpMain;
  private Label LbMain;
  private Label label2;
  private Button BtChangePw;
  private Button BtInfoLoginSystem;
  private BindingSource BsUcMain { get; set; } = null!;
}
