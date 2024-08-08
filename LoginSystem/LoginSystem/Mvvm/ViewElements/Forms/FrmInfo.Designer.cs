
using LoginSystem.Properties;

namespace michele.natale.LoginSystems.Views;
 

partial class FrmInfo
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
    this.TlpStartMain = new TableLayoutPanel();
    this.TlpStartMainBottom = new TableLayoutPanel();
    this.LlMnHomepage = new LinkLabel();
    this.PbMain = new PictureBox();
    this.TlpStartMain.SuspendLayout();
    this.TlpStartMainBottom.SuspendLayout();
    ((System.ComponentModel.ISupportInitialize)this.PbMain).BeginInit();
    this.SuspendLayout();
    // 
    // TlpStartMain
    // 
    this.TlpStartMain.BackColor = Color.Transparent;
    this.TlpStartMain.ColumnCount = 1;
    this.TlpStartMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
    this.TlpStartMain.Controls.Add(this.TlpStartMainBottom, 0, 2);
    this.TlpStartMain.Controls.Add(this.PbMain, 0, 0);
    this.TlpStartMain.Dock = DockStyle.Fill;
    this.TlpStartMain.Location = new Point(0, 0);
    this.TlpStartMain.Name = "TlpStartMain";
    this.TlpStartMain.RowCount = 3;
    this.TlpStartMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 160F));
    this.TlpStartMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
    this.TlpStartMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 80F));
    this.TlpStartMain.Size = new Size(632, 268);
    this.TlpStartMain.TabIndex = 100;
    // 
    // TlpStartMainBottom
    // 
    this.TlpStartMainBottom.ColumnCount = 3;
    this.TlpStartMainBottom.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
    this.TlpStartMainBottom.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 300F));
    this.TlpStartMainBottom.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
    this.TlpStartMainBottom.Controls.Add(this.LlMnHomepage, 1, 1);
    this.TlpStartMainBottom.Dock = DockStyle.Fill;
    this.TlpStartMainBottom.Location = new Point(3, 183);
    this.TlpStartMainBottom.Name = "TlpStartMainBottom";
    this.TlpStartMainBottom.RowCount = 3;
    this.TlpStartMainBottom.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
    this.TlpStartMainBottom.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
    this.TlpStartMainBottom.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
    this.TlpStartMainBottom.Size = new Size(626, 82);
    this.TlpStartMainBottom.TabIndex = 102;
    // 
    // LlMnHomepage
    // 
    this.LlMnHomepage.Anchor = AnchorStyles.None;
    this.LlMnHomepage.AutoSize = true;
    this.LlMnHomepage.Font = new Font("Arial", 14F);
    this.LlMnHomepage.LinkBehavior = LinkBehavior.NeverUnderline;
    this.LlMnHomepage.LinkColor = Color.Navy;
    this.LlMnHomepage.Location = new Point(184, 27);
    this.LlMnHomepage.Name = "LlMnHomepage";
    this.LlMnHomepage.Size = new Size(258, 27);
    this.LlMnHomepage.TabIndex = 8;
    this.LlMnHomepage.TabStop = true;
    this.LlMnHomepage.Text = "michele natale - github";
    this.LlMnHomepage.LinkClicked += this.LlMnHomepage_LinkClicked;
    this.LlMnHomepage.MouseEnter += this.LlMnHomepage_MouseEnter;
    this.LlMnHomepage.MouseLeave += this.LlMnHomepage_MouseLeave;
    // 
    // PbMain
    // 
    this.PbMain.Dock = DockStyle.Fill;
    this.PbMain.Image = Resources.myLogo;
    this.PbMain.InitialImage = Resources.myLogo;
    this.PbMain.Location = new Point(3, 3);
    this.PbMain.Name = "PbMain";
    this.PbMain.Size = new Size(626, 154);
    this.PbMain.SizeMode = PictureBoxSizeMode.CenterImage;
    this.PbMain.TabIndex = 103;
    this.PbMain.TabStop = false;
    this.PbMain.Click += this.FrmInfo_Click;
    // 
    // FrmInfo
    // 
    this.AutoScaleDimensions = new SizeF(13F, 26F);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.BackColor = Color.White;
    //this.BackgroundImage = Resources.PlanetaryView_2_700_400;
    this.BackgroundImageLayout = ImageLayout.Stretch;
    this.ClientSize = new Size(632, 268);
    this.Controls.Add(this.TlpStartMain);
    this.Font = new Font("Arial", 14F);
    this.Margin = new Padding(5, 4, 5, 4);
    this.MaximumSize = new Size(790, 390);
    this.MinimumSize = new Size(650, 315);
    this.Name = "FrmInfo";
    this.StartPosition = FormStartPosition.CenterScreen;
    this.Text = "© LoginSystem 2024 - Created by © Michele Natale 2024";
    this.TlpStartMain.ResumeLayout(false);
    this.TlpStartMainBottom.ResumeLayout(false);
    this.TlpStartMainBottom.PerformLayout();
    ((System.ComponentModel.ISupportInitialize)this.PbMain).EndInit();
    this.ResumeLayout(false);
  }

  #endregion

  private LinkLabel LlMnHomepage;
  private TableLayoutPanel TlpStartMain;
  private TableLayoutPanel TlpStartMainBottom;
  private PictureBox PbMain;
}