



namespace michele.natale.LoginSystems.Apps;

using Models;
using static Services.AppServices;


partial class WindowsManager
{
  public static void SetStartConfig()
  {
    var frmc = !File.Exists(AppServicesHolder.DataSettingFile) ? FrmCommands.UcRegist : FrmCommands.UcLogin;
    SetStartConfig(frmc);
  }

  internal static void SetStartConfig(FrmCommands fcrt)
  {
    AppServicesHolder.SetStartConfiguration(fcrt);

    if (!File.Exists(AppServicesHolder.RTableSettingFile))
      AppServicesHolder.SetRTable();

    //if (!File.Exists(DataSettingFile))
    //  SetNewRngSeedSalt();

    if (!File.Exists(AppServicesHolder.SaltSettingFile))
      AppServicesHolder.SetSaltSetting();
  }

}
