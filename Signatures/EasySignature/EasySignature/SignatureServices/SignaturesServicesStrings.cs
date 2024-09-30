

namespace michele.natale.Cryptography.Signatures.Services;


partial class SignatureServices
{

  /// <summary>
  /// Returns random letters.
  /// </summary>
  /// <param name="size">Desired Size</param>
  /// <returns>string</returns>
  public static string ToRngName(int size = 10) =>
    string.Join("", Random.Shared.GetItems<char>(
      ToAlphaString(), size));

  /// <summary>
  /// Returns alphanumeric characters.
  /// </summary>
  /// <returns>string</returns>
  public static string ToAlphaString()
  {
    var alpha_numeric = "0123456789";
    var alpha_lower = "abcdefghijklmnopqrstuvwxyz";
    var alpha_upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    return alpha_numeric + alpha_lower + alpha_upper;
  }
}
