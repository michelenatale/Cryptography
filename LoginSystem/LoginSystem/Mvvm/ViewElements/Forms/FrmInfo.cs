

namespace michele.natale.LoginSystems.Views;

using LoginSystem.Properties;
using LoginSystems.Services;

/// <summary>
/// A Forms as info box.
/// </summary>
internal partial class FrmInfo : Form
{

  /// <summary>
  /// C-Tor
  /// </summary>
  public FrmInfo()
  {
    this.InitializeComponent();

    this.Icon = Resources.myLogo64;
    this.DialogResult = DialogResult.None;
  }

  /// <summary>
  /// All click events that are integrated in the InfoForm.
  /// </summary>
  /// <param name="sender">The sender of the Event.</param>
  /// <param name="e">The Parameter of the Event.</param>
  private void FrmInfo_Click(object sender, EventArgs e)
  {
    switch (sender)
    {
      case PictureBox obj when obj == this.PbMain:
        AppServices.AppServicesHolder.MyHomePageUrl(); break;
    }
  }

  /// <summary>
  /// A Event for the LinkLabel.
  /// </summary>
  /// <param name="sender">The sender of the Event.</param>
  /// <param name="e">The Parameter of the Event.</param>
  private void LlMnHomepage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
  {
    this.LlMnHomepage.LinkVisited = true;

    AppServices.AppServicesHolder.MyHomePageUrl();

    e.Link!.Visited = true;
  }

  /// <summary>
  /// A Event for the LinkLabel.
  /// </summary>
  /// <param name="sender">The sender of the Event.</param>
  /// <param name="e">The Parameter of the Event.</param>
  private void LlMnHomepage_MouseEnter(object sender, EventArgs e)
  {
    var llabel = (LinkLabel)sender;
    var lfont = llabel.Font;
    llabel.Font = new Font(lfont.FontFamily, lfont.Size, FontStyle.Italic);
  }

  /// <summary>
  /// A Event for the LinkLabel.
  /// </summary>
  /// <param name="sender">The sender of the Event.</param>
  /// <param name="e">The Parameter of the Event.</param>
  private void LlMnHomepage_MouseLeave(object sender, EventArgs e)
  {
    var llabel = (LinkLabel)sender;
    var lfont = llabel.Font;
    llabel.Font = new Font(lfont.FontFamily, lfont.Size, FontStyle.Regular);
  }

}
