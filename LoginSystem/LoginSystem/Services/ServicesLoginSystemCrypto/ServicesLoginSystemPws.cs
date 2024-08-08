
using michele.natale.Pointers;
using System.Text;

namespace michele.natale.LoginSystems.Services;
partial class AppServices
{
  /// <summary>
  /// MIN_PW_LENGTH = 10
  /// </summary>
  public const int MIN_PW_LENGTH = 10;

  /// <summary>
  /// MAX_PW_LENGTH = 128
  /// </summary>
  public const int MAX_PW_LENGTH = 128;

  /// <summary>
  /// MIN_USER_NAME_LENGTH = 10
  /// </summary>
  public const int MIN_USER_NAME_LENGTH = 10;

  /// <summary>
  /// MAX_USER_NAME_LENGTH = 32
  /// </summary>
  public const int MAX_USER_NAME_LENGTH = 32;

  /// <summary>
  /// Check is Username valid
  /// </summary>
  /// <param name="username">Username as string</param>
  /// <returns></returns>
  public bool IsValidUserName(string username)
  {
    if (username.Contains('@')) return false;
    return IsValidUserName(Encoding.UTF8.GetBytes(username.Trim()));
  }

  /// <summary>
  /// Check is Username valid
  /// </summary>
  /// <param name="bytes">Username as Array of byte</param>
  /// <returns></returns>
  public bool IsValidUserName(byte[] bytes)
  {
    return bytes.Length >= MIN_USER_NAME_LENGTH &&
           bytes.Length <= MAX_USER_NAME_LENGTH;
  }

  /// <summary>
  /// Check ist Password valid
  /// </summary>
  /// <param name="pw">Password as string</param>
  /// <returns></returns>
  public bool IsValidPw(string pw)
  {
    return IsValidPw(Encoding.UTF8.GetBytes(pw.Trim()));
  }

  /// <summary>
  /// Check is Password valid
  /// </summary>
  /// <param name="bytes">Password as Array of Bytes</param>
  /// <returns></returns>
  public bool IsValidPw(byte[] bytes)
  {
    return bytes.Length >= MIN_PW_LENGTH &&
           bytes.Length <= MAX_PW_LENGTH;
  }

  /// <summary>
  /// Check is Password valid
  /// </summary>
  /// <param name="bytes">Password as Array of UsIPtr(Of Byte)</param>
  /// <returns></returns>
  public bool IsValidPw(UsIPtr<byte> bytes)
  {
    return IsValidPw(bytes.ToArray());
  }

  ///// <summary>
  ///// Check is Username, Password and Mail valid
  ///// </summary>
  ///// <param name="uc">Username</param>
  ///// <param name="pw">Password</param>
  ///// <param name="mail">EMail</param>
  ///// <returns></returns>
  //public static bool IsValidUcPwMail(
  //  string uc, string pw, string mail)
  //{
  //  if (IsValidPw(pw))
  //    if (IsValidUserName(uc))
  //      return IsValidEmail(mail);
  //  return false;
  //}

  /// <summary>
  /// Check is Username, Password and Mail valid
  /// </summary>
  /// <param name="un">Username</param>
  /// <param name="pw">Password</param>
  /// <param name="mail">EMail</param>
  /// <returns></returns>
  public bool IsValidUnPwMail(
  string un, UsIPtr<byte> pw, string mail)
  {
    if (IsValidPw(pw))
      if (IsValidUserName(un))
        return IsValidEmail(mail);
    return false;
  }

  ///// <summary>
  ///// Check is Username, Password and Mail valid
  ///// </summary>
  ///// <param name="un">Username</param>
  ///// <param name="pw">Password</param>
  ///// <param name="mail">EMail</param>
  ///// <returns></returns>
  //public static bool IsValidUcPwMailLogin(string un, string pw, string mail)
  //{
  //  if (!string.IsNullOrEmpty(un) && !string.IsNullOrEmpty(mail)) return false;
  //  if (string.IsNullOrEmpty(un) && string.IsNullOrEmpty(mail)) return false;

  //  if (!string.IsNullOrEmpty(un) && !IsValidUserName(un)) return false;
  //  if (!string.IsNullOrEmpty(mail) && !IsValidEmail(mail)) return false;
  //  return IsValidPw(pw);
  //}

  /// <summary>
  /// Check is Username, Password and Mail valid
  /// </summary>
  /// <param name="uc">Username</param>
  /// <param name="pw">Password</param>
  /// <param name="mail">EMail</param>
  /// <returns></returns>
  public bool IsValidUnPwMailLogin(
    string un, UsIPtr<byte> pw, string mail)
  {
    if (!string.IsNullOrEmpty(un) && !string.IsNullOrEmpty(mail)) return false;
    if (string.IsNullOrEmpty(un) && string.IsNullOrEmpty(mail)) return false;

    if (!string.IsNullOrEmpty(un) && !IsValidUserName(un)) return false;
    if (!string.IsNullOrEmpty(mail) && !IsValidEmail(mail)) return false;
    return IsValidPw(pw);
  }
}
