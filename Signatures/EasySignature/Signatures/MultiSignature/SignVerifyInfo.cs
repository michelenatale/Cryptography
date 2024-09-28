


using System.Security.Cryptography;
using System.Text;

namespace michele.natale.Signatures;

public class SignVerifyInfo
{
  public byte[] MultiSign { get; set; } = [];
  public bool ExtraForce { get; set; } = false;
  public bool MultiVerify { get; set; } = false;
  public MultiSignInfo[] MultiSignInfos { get; set; } = [];

  public SignVerifyInfo(MultiSignInfo[] multi_sign_infos, bool extra_force = false)
  {
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

  public static byte[] Multi_Sign(MultiSignInfo[] multi_sign_infos, bool extra_force = false)
  {
    var msg = multi_sign_infos.First().Message;
    var istrue = multi_sign_infos.All(x => x.Message.SequenceEqual(msg));
    if (!istrue) return [];

    var sgns = multi_sign_infos.OrderBy(x => x.Sign).ToArray();
    var signs = sgns
      .Select(s => Convert.FromHexString(s.Sign)).ToArray();

    var max_length = signs.Max(x => x.Length);

    if (extra_force)
    {
      var buffer = new byte[max_length + 8];
      var result = new byte[signs.Length * max_length];
      for (var i = 0; i < signs.Length; i++)
      {
        var sum = signs[i].Sum(x => x);
        var sum_bytes = BitConverter.GetBytes(i);
        if (BitConverter.IsLittleEndian) Array.Reverse(sum_bytes);
        var cnt_bytes = BitConverter.GetBytes(i);
        if (BitConverter.IsLittleEndian) Array.Reverse(cnt_bytes);

        signs[i].CopyTo(buffer, 0);
        sum_bytes.CopyTo(buffer, signs[i].Length);
        cnt_bytes.CopyTo(buffer, signs[i].Length + sum_bytes.Length);

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
      foreach (var sn in signs)
      {
        for (var i = 0; i < max_length; i++)
          result[i % result.Length] ^= sn[i % sn.Length];
      }
      var third_length = signs.First().Length / 3;
      return SHA512.HashData(result.Take(2 * third_length).ToArray())
        .Concat(SHA512.HashData(result.Skip(third_length).ToArray())).ToArray();
    }
  }


  public static bool Multi_Verifiy(MultiSignInfo[] multi_sign_infos, byte[] multi_signatures, bool extra_force = false) =>
   multi_signatures.SequenceEqual(Multi_Sign(multi_sign_infos, extra_force));

}
