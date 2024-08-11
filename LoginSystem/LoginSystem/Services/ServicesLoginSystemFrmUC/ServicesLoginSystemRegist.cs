
using michele.natale.Pointers;

namespace michele.natale.LoginSystems.Services;
partial class AppServices
{

  /// <summary>
  ///  Check Arguments
  /// </summary>
  /// <param name="username">Username</param>
  /// <param name="pw">Password</param>
  /// <param name="pwr">Repeat Password</param>
  /// <param name="email">Email</param>
  /// <returns></returns>
  public bool CheckValues(
    string username, UsIPtr<byte> pw,
    UsIPtr<byte> pwr, string email)
  {
    return pw.Equality(pwr) && this.IsValidUnPwMail(username, pw, email);
  }
}
