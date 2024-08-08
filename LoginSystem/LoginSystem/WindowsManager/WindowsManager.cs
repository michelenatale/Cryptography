


namespace michele.natale.LoginSystems.Apps;

/// <summary>
/// Superior entry-level class
/// </summary>
public sealed partial class WindowsManager : IWindowsManager
{
  private readonly FrmMain FrmMain = null!;
  public static string AuthorInfo => AuthorInfos.Author_Info();


  public WindowsManager(FrmMain frm)
  {
    ArgumentNullException.ThrowIfNull(frm, nameof(frm));
    this.FrmMain = frm;
  }

  public void Hide() => this.FrmMain?.Hide();
  public void Close() => this.FrmMain?.Close();
  public void Show() => this.FrmMain?.Show();


}
