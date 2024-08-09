
namespace michele.natale.LoginSystems.Apps;

using Models;
using static Services.AppServices;

partial class WindowsManager
{

  /// <summary>
  /// The interface provided for the customer for fetching the AppLoginSettings protocol.
  /// </summary>
  /// <param name="result">AppLoginSettings protocol</param>
  /// <returns></returns>
  public bool To_App_Login_Setting(out AppLoginSettings result)
  {
    result = AppLoginSettings.Empty;
    if (this.FrmMain.DialogResult != DialogResult.OK)
      return false;

    var rsi = new RegistDataInfo
    {
      ASInfo = AppSettingsInfo.GetAppLoginSetting,
    };

    AppServicesHolder.ToRegistDataInfo(rsi);
    if (rsi.CorrectExecution)
    {
      result = rsi.AppSettings;
      return true;
    }
    return false;
  }

  /// <summary>
  /// The interface provided for the customer for saving the AppLoginSettings protocol.
  /// </summary>
  /// <param name="app_settings">AppLoginSettings protocol</param>
  /// <returns></returns>
  public bool Set_App_Login_Setting(AppLoginSettings app_settings)
  {
    if (this.FrmMain.DialogResult != DialogResult.OK)
      return false;

    var rsi = new RegistDataInfo
    {
      AppSettings = app_settings,
      ASInfo = AppSettingsInfo.SetAppLoginSetting,
    };

    AppServicesHolder.ToRegistDataInfo(rsi);
    return rsi.CorrectExecution;
  }

  /// <summary>
  /// This ensures that all data that needs to be saved has also been saved.
  /// </summary>
  public void FrmClose() => this.FrmMain.Close();


}
