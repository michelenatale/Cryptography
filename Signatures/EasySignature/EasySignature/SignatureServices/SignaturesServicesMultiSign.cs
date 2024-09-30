

using System.Text;

namespace michele.natale.Cryptography.Signatures.Services;

using Signatures;
using static Cryptography.Randoms.CryptoRandom;

partial class SignatureServices
{

  /// <summary>
  /// Returns the random sample data for the calculation of a signature. 
  /// </summary>
  /// <param name="size">Desired Size</param>
  /// <param name="message">Desired Message</param>
  /// <param name="keysize">Desired KeySize</param>
  /// <param name="extra_force">Desired Signature Force</param>
  /// <returns>SignInfo</returns>
  public static SignInfo[] SignInfoSamples(
    int size, byte[] message, int keysize = 128, bool extra_force = false) =>
    SignInfoSamples(size, Encoding.UTF8.GetString(message), keysize, extra_force);

  /// <summary>
  /// Returns the random sample data for the calculation of a signature. 
  /// </summary>
  /// <param name="size">Desired Size</param>
  /// <param name="message">Desired Message</param>
  /// <param name="keysize">Desired KeySize</param>
  /// <param name="extra_force">Desired Signature Force</param>
  /// <returns>SignInfo</returns>
  public static SignInfo[] SignInfoSamples(
    int size, string message, int keysize = 128, bool extra_force = false)
  {
    var result = new SignInfo[size];
    for (int i = 0; i < size; i++)
    {
      var msg = message;
      var name = $"Name_{i}";
      var seed = RngBytes(SingleSignature.SEED_SIZE);
      var (privk, pubk) = SingleSignature.CreateKeyPair(seed, keysize);

      byte[] sign;
      if (extra_force)
        sign = SingleSignature.Sign(privk, Encoding.UTF8.GetBytes(msg), seed);
      else sign = SingleSignature.Sign(privk, Encoding.UTF8.GetBytes(msg));
      result[i] = new SignInfo(name, seed, msg, pubk, sign, extra_force);
    }
    return result;
  }

}
