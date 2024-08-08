

 

namespace michele.natale.LoginSystems;

using Models;


/// <summary>
/// One of the classes that will be used for the registry.
/// </summary>
internal sealed class RegistDataInfo
{
  public object Tag { get; set; } = null!;
  public bool CorrectExecution { get; set; } = true;
  public AppLoginSettings AppSettings { get; set; } = null!;
  public AppSettingsInfo ASInfo { get; set; } = AppSettingsInfo.None!;
  public static RegistDataInfo Empty => new();
}
