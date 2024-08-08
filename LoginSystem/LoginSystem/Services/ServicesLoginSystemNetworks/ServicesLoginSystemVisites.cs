
using System.Diagnostics;

namespace michele.natale.LoginSystems.Services;

partial class AppServices
{
  private readonly static string LinkValue =
    "https://github.com/michelenatale";

  /// <summary>
  /// Start a Hompage nto the Browser
  /// </summary>
  public void MyHomePageUrl()
  {
    string url_address = LinkValue;
    OpenUrl(url_address);
  }

  /// <summary>
  /// Start a Hompage nto the Browser
  /// </summary>
  /// <param name="url_address">Url Address</param>
  public void OpenUrl(string url_address)
  {
    try
    {
      VisitLink(url_address);
    }
    catch (Exception ex)
    {
      MessageBox.Show("Unable to open link that was clicked.\n" + ex.Message);
    }
  }

  /// <summary>
  /// Start a Hompage nto the Browser
  /// </summary>
  /// <param name="url_address"></param>
  private void VisitLink(string url_address)
  {
    Process.Start(new ProcessStartInfo
    {
      FileName = url_address,
      UseShellExecute = true
    });
  }

}
