

namespace michele.natale.SignatureServices;


public class SignaturesServices
{

  public static string ToRngName(int size = 10) =>
    string.Join("", Random.Shared.GetItems<char>(
      ToAlphaString(), size));


  public static string ToAlphaString()
  {
    var alpha_numeric = "0123456789";
    var alpha_lower = "abcdefghijklmnopqrstuvwxyz";
    var alpha_upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    return alpha_numeric + alpha_lower + alpha_upper;
  }
}
