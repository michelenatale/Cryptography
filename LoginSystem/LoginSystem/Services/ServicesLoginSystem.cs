
namespace michele.natale.LoginSystems.Services;

/// <summary>
/// Offers various methods for handling the login process.
/// </summary>
internal sealed partial class AppServices
{
  /// <summary>
  /// The Service Holder
  /// </summary>
  public static AppServices AppServicesHolder { get; private set; } = new();

}
