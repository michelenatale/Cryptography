
using michele.natale.Pointers;

namespace michele.natale.LoginSystems.Services;

partial class AppServices
{
  //Hier kommen noch die 3 Authentifizierungen rein.

  public bool CheckValuesLogin(
      string uname_email, UsIPtr<byte> pw,
      out (string UName, UsIPtr<byte> Pw, string EMail, UsIPtr<byte> _) result)
  {

    result = default;
    if (pw is null || pw.IsEmpty) return false;
    if (string.IsNullOrEmpty(uname_email)) return false;

    var email = string.Empty;
    var uname = uname_email.ToLower()?.Trim()!;
    if (uname.Contains('@')) { email = uname; uname = string.Empty; }

    if (CheckValuesLogin(uname, pw, email, out result))
      return ShowMessageLogin(uname, email) == DialogResult.OK;

    MessageBox.Show("Not all inputs are correct.");
    return false;
  }

  /// <summary>
  /// Check Arguments
  /// </summary>
  /// <param name="username">Username</param>
  /// <param name="pw">Password</param>
  /// <param name="email">EMail</param> 
  /// <param name="result">Result</param>
  /// <returns></returns>
  public bool CheckValuesLogin(
  string username, UsIPtr<byte> pw, string email,
  out (string UName, UsIPtr<byte> Pw, string EMail, UsIPtr<byte> _) result)
  {
    result = default;
    if (IsValidUnPwMailLogin(username, pw, email))
    {
      result = (username, pw, email, UsIPtr<byte>.Empty);
      return true;
    }

    return false;
  }
}
