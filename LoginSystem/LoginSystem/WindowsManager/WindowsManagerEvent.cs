
namespace michele.natale.LoginSystems.Apps;

using Models;
using static Services.AppServices;

partial class WindowsManager
{
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



}
