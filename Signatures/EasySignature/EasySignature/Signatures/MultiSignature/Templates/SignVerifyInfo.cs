

using System.Security.Cryptography;

namespace michele.natale.Cryptography.Signatures;


/// <summary>
/// Customer-Class for Multi-Sign and Multi-Verify
/// <para>
/// If you are in possession of the PrivateKey (for extra_force 
/// also the Seed) and the Message, a Multi-Sign can be generated.
/// </para>
/// If you are in possession of the PublicKey, the Message and the 
/// Sign, a Multi-Verify can be tested.
/// </summary>
public class SignVerifyInfo
{
  /// <summary>
  /// Shows the generated Multi-Signature.
  /// </summary>
  public byte[] MultiSign { get; set; } = [];

  /// <summary>
  /// Indicates the signature strength with which the 
  /// calculation was made. 
  /// <para>True for Extra-Force, otherwise false for Normal Force.</para> 
  /// </summary>
  public bool ExtraForce { get; set; } = false;

  /// <summary>
  /// Shows the result of the Multi-Verify.
  /// </summary>
  public bool MultiVerify { get; set; } = false;

  /// <summary>
  /// For basic data (MultiSignInfo) for the Signature calculation 
  /// </summary>
  public MultiSignInfo[] MultiSignInfos { get; set; } = [];

  /// <summary>
  /// C-Tor
  /// </summary>
  /// <param name="multi_sign_infos">Desired MultiSignInfo</param>
  /// <param name="extra_force">Desired Force</param>
  public SignVerifyInfo(MultiSignInfo[] multi_sign_infos, bool extra_force = false)
  {
    //Check all message are Equals
    var msg = multi_sign_infos.First().Message;
    var istrue = multi_sign_infos.All(x => x.Message.SequenceEqual(msg));
    if (istrue)
    {
      this.ExtraForce = extra_force;
      this.MultiSignInfos = multi_sign_infos;
      this.MultiSign = Multi_Sign(multi_sign_infos, extra_force);
      this.MultiVerify = Multi_Verifiy(multi_sign_infos, this.MultiSign, extra_force);
    }
  }

  /// <summary>
  /// Calculates the Multi-Signature based on the basic data 
  /// (MultiSignInfo) and returns it as a byte array.
  /// </summary>
  /// <param name="multi_sign_infos">Desired MultiSignInfo</param>
  /// <param name="extra_force">Desired Signature Force</param>
  /// <returns>Signature as Array of byte</returns>
  public static byte[] Multi_Sign(
    MultiSignInfo[] multi_sign_infos, bool extra_force = false)
  {
    var msg = multi_sign_infos.First().Message;
    var istrue = multi_sign_infos.All(x => x.Message.SequenceEqual(msg));
    if (!istrue) return [];

    var sgns = multi_sign_infos.OrderBy(x => x.Sign).ToArray();
    var signs = sgns.Select(s => Convert.FromHexString(s.Sign)).ToArray();

    var max_length = signs.Max(x => x.Length);
    var n_bytes = BitConverter.GetBytes(signs.Length * max_length);
    if (BitConverter.IsLittleEndian) Array.Reverse(n_bytes);

    if (extra_force)
    {
      var buffer = new byte[max_length + 8 + n_bytes.Length];
      var result = new byte[signs.Length * MultiSignature.SIGN_SIZE];
      for (var i = 0; i < signs.Length; i++)
      {
        var sum = signs[i].Sum(x => x);
        var sum_bytes = BitConverter.GetBytes(sum);
        if (BitConverter.IsLittleEndian) Array.Reverse(sum_bytes);
        var cnt_bytes = BitConverter.GetBytes(i);
        if (BitConverter.IsLittleEndian) Array.Reverse(cnt_bytes);

        signs[i].CopyTo(buffer, 0);
        sum_bytes.CopyTo(buffer, signs[i].Length);
        cnt_bytes.CopyTo(buffer, signs[i].Length + sum_bytes.Length);
        n_bytes.CopyTo(buffer, signs[i].Length + sum_bytes.Length + cnt_bytes.Length);

        var h1 = SHA512.HashData(signs[i]);
        var h2 = HMACSHA512.HashData(signs[i], buffer)
          .Concat(HMACSHA512.HashData(h1, buffer))
          .Select((x, i) => (byte)(x ^ h1[i % h1.Length])).ToArray();
        h2.CopyTo(result, i * h2.Length);
      }

      var third_length = signs.First().Length / 3;
      return SHA512.HashData(result.Take(2 * third_length).ToArray())
        .Concat(SHA512.HashData(result.Skip(third_length).ToArray())).ToArray();
    }
    else
    {
      var result = new byte[max_length];
      for (var i = 0; i < max_length; i++)
        result[i % result.Length] ^= n_bytes[i % n_bytes.Length];
      foreach (var sn in signs)
        for (var i = 0; i < max_length; i++)
          result[i % result.Length] ^= sn[i % sn.Length];
      var third_length = signs.First().Length / 3;
      return SHA512.HashData(result.Take(2 * third_length).ToArray())
        .Concat(SHA512.HashData(result.Skip(third_length).ToArray())).ToArray();
    }
  }

  /// <summary>
  /// Calculates the Multi-Verification and returns true if 
  /// the test is positive, otherwise false.
  /// </summary>
  /// <param name="multi_sign_infos">Desired MultiSignInfo</param>
  /// <param name="multi_signatures">The Multi-Signature required for the Multi-Verification</param>
  /// <param name="extra_force">Desird MultiSign-Force</param>
  /// <returns>true if the test is positive, otherwise false.</returns>
  public static bool Multi_Verifiy(MultiSignInfo[] multi_sign_infos, byte[] multi_signatures, bool extra_force = false) =>
   multi_signatures.SequenceEqual(Multi_Sign(multi_sign_infos, extra_force));

}
