

namespace michele.natale.LoginSystems.Settings;

using Models;

/// <summary>
/// Sets the start options that are used when starting © LoginSystem 2024.
/// </summary>
internal sealed class StartSettings
{

  /// <summary>
  /// Standard default.
  /// </summary>
  public FrmCommands Frm_Cmd { get; set; } = FrmCommands.UcLogin;

  /// <summary>
  /// C-Tor
  /// </summary>
  public StartSettings()
  {
  }

  /// <summary>
  /// C-Tor
  /// </summary>
  /// <param name="fcrt">Corresponding form that is requested. </param>
  public StartSettings(FrmCommands fcrt)
  {
    this.Frm_Cmd = fcrt;
  }


}
