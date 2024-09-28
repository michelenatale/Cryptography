


using System.Security.Cryptography;

namespace michele.natale.Signatures;

public class SignVerifyInfo
{

  public byte[] MultiSign { get; set; } = [];
  public bool MultiVerify { get; set; } = false;
  public MultiSignInfo[] MultiSignInfos { get; set; } = [];

  public SignVerifyInfo(MultiSignInfo[] multi_sign_infos)
  {
    var msg = multi_sign_infos.First().Message;
    var istrue = multi_sign_infos.All(x => x.Message.SequenceEqual(msg));
    if (istrue)
    {
      this.MultiSignInfos = multi_sign_infos;
      this.MultiSign = Multi_Sign(multi_sign_infos);
      this.MultiVerify = Multi_Verifiy(multi_sign_infos, this.MultiSign);
    }
  }

  public static byte[] Multi_Sign(MultiSignInfo[] multi_sign_infos)
  {
    var msg = multi_sign_infos.First().Message;
    var istrue = multi_sign_infos.All(x => x.Message.SequenceEqual(msg));
    if (!istrue) return [];

    var sgns = multi_sign_infos.OrderBy(x => x.Sign).ToArray();
    var signs = sgns
      .Select(s => Convert.FromHexString(s.Sign)).ToArray();

    var max_length = signs.Max(x => x.Length);
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


  public static bool Multi_Verifiy(MultiSignInfo[] multi_sign_infos, byte[] multi_signatures) =>
   multi_signatures.SequenceEqual(Multi_Sign(multi_sign_infos));

}
