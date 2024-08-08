


namespace michele.natale.LoginSystems.Models;

/// <summary>
/// The corresponding options offered by © LoginSystem 2024.
/// </summary>
internal enum AppSettingsInfo : uint
{
  None = 0,
  SetRegistData,        //Regist
  CheckRegistData,      //Login
  SetNewRegistData,     //PwForget
  ChangeRegistData,     //PwChange
  GetAppLoginSetting,
  SetAppLoginSetting,
}
