
using michele.natale.LoginSystems.Views;

namespace michele.natale.LoginSystems.Apps;

partial class FrmMain
{
  /// <summary>
  ///  Required designer variable.
  /// </summary>
  private System.ComponentModel.IContainer components = null;

  /// <summary>
  ///  Clean up any resources being used.
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
  ///  Required method for Designer support - do not modify
  ///  the contents of this method with the code editor.
  /// </summary>
  private void InitializeComponent()
  {
    this.components = new System.ComponentModel.Container();
    this.BsViewModel = new BindingSource(this.components);
    ((System.ComponentModel.ISupportInitialize)this.BsViewModel).BeginInit();
    this.SuspendLayout();
    // 
    // FrmMain
    // 
    this.AutoScaleDimensions = new SizeF(13F, 26F);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.BackColor = Color.White;
    this.ClientSize = new Size(732, 653);
    this.Font = new Font("Arial", 14F);
    this.Margin = new Padding(5, 4, 5, 4);
    this.Name = "FrmMain";
    this.StartPosition = FormStartPosition.CenterScreen;
    this.Text = "© LoginSystem 2024 created by © Michele Natale 2024";
    this.FormClosed += this.FrmMain_FormClosed;
    ((System.ComponentModel.ISupportInitialize)this.BsViewModel).EndInit();
    this.ResumeLayout(false);
  }
  #endregion

  private UcMainEx UcMain;
  private UcLoginEx UcLogin;
  private UcRegistEx UcRegist;
  private UcPwChangeEx UcPwChange;
  private UcPwForgetEx UcPwForget;
  private BindingSource BsViewModel;
}
