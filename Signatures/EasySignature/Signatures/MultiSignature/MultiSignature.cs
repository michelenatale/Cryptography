
using System.Text;

namespace michele.natale.Signatures;

using static Cryptography.Randoms.CryptoRandom;

public class MultiSignature
{

  public static SignInfo[] SignInfoSamples(int size, byte[] message) =>
    SignInfoSamples(size, Encoding.UTF8.GetString(message));

  public static SignInfo[] SignInfoSamples(
    int size, string message, int keysize = 128)
  {
    var result = new SignInfo[size];
    for (int i = 0; i < size; i++)
    {
      var msg = message;
      var name = $"Name_{i}";
      var seed = RngBytes(SingleSignature.SEED_SIZE);
      var (privk, pubk) = SingleSignature.CreateKeyPair(seed, keysize);
      var sign = SingleSignature.Sign(privk, Encoding.UTF8.GetBytes(msg));
      result[i] = new SignInfo(name, seed, msg, pubk, sign);
    }
    return result;
  }
}
