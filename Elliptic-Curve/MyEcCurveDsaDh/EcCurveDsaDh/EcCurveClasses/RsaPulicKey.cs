

using System.Text;
using System.Security.Cryptography;

namespace michele.natale.EcCurveDsaDh;

public partial class RsaPublicKey
{
  public string RsaIndex = string.Empty;
  public RSAParameters PublicKey { get; private set; } = new();

  public static string Rsa_PublicKey_Footer_Pmei(bool timestamp = true) =>
    PMEI.RsaPublicKeyPmeiHF(timestamp).F;
  public static string Rsa_PublicKey_Header_Pmei(bool timestamp = true) =>
    PMEI.RsaPublicKeyPmeiHF(timestamp).H;

  public (string Index, RSAParameters RsaPublicKey) ToRsaPublicKeyData =>
    (this.RsaIndex, this.PublicKey);

  public RsaPublicKey(RsaPublicKey rsa_pub_key)
  {
    this.RsaIndex = rsa_pub_key.RsaIndex;
    this.PublicKey = rsa_pub_key.PublicKey;
  }


  public RsaPublicKey(RSAParameters rsaparam, string index = "")
  {
    this.RsaIndex = index;
    this.PublicKey = new RSAParameters
    {
      Exponent = rsaparam.Exponent,
      Modulus = rsaparam.Modulus,
    };
  }

  public RsaPublicKey(string pub_key_pmei)
  {
    var pub_key = FromRsaPublicKeyPmei(pub_key_pmei);
    this.RsaIndex = pub_key.RsaIndex;
    this.PublicKey = pub_key.PublicKey;
  }

  public override string ToString()
  {
    var sb = new StringBuilder();
    sb.AppendLine($"PublicKey.Exponent: {Convert.ToHexString(this.PublicKey.Exponent!)}");
    sb.AppendLine($"PublicKey.Modulus: {Convert.ToHexString(this.PublicKey.Modulus!)}");
    return sb.ToString();
  }

  public static RsaPublicKey ToRsaPublicKey(RSAParameters keypair) =>
     new(new RSAParameters
     {
       Modulus = keypair.Modulus is null ? [] : keypair.Modulus,
       Exponent = keypair.Exponent is null ? [] : keypair.Exponent,
     });

  public static (string Index, string RsaPmei) ToRsaPublicKeyPmei(RsaPublicKey pupkey)
  {
    var index = Encoding.UTF8.GetBytes(pupkey.RsaIndex);
    var rsaparam = RsaParametersInfo.SerializeRsaParam(pupkey.PublicKey);
    var bytes = EcService.SerializeJson(new[] { index, rsaparam });

    var (h, f) = (Rsa_PublicKey_Header_Pmei(true), Rsa_PublicKey_Footer_Pmei(true));
    var pmei = PMEI.ToPmei(bytes, h, f, true, pupkey.RsaIndex);
    return (pupkey.RsaIndex, pmei);
  }

  public static byte[] ToRsaPublicKeyPmeiUtf8(RsaPublicKey rsapupkey) =>
    Encoding.UTF8.GetBytes(ToRsaPublicKeyPmei(rsapupkey).RsaPmei);

  public static string ToRsaPublicKeyPmei(RSAParameters rsaparam, string index = "")
  {
    var pubkey = new RsaPublicKey(rsaparam, index);
    return ToRsaPublicKeyPmei(pubkey).RsaPmei;
  }

  public static byte[] ToRsaPublicKeyPmeiUtf8(RSAParameters rsapupkey, string index = "") =>
    Encoding.UTF8.GetBytes(ToRsaPublicKeyPmei(rsapupkey, index));

  public static RsaPublicKey Copy(RsaPublicKey rsa_pub_key) =>
    new(rsa_pub_key);

  public static RsaPublicKey FromRsaPublicKeyPmei(string rsa_pmei)
  {
    var (h, f) = (Rsa_PublicKey_Header_Pmei(false), Rsa_PublicKey_Footer_Pmei(false));
    var (id, msg) = PMEI.FromPmei(rsa_pmei, h, f);
    var data = EcService.DeserializeJson<byte[][]>(msg);

    var index = Encoding.UTF8.GetString(data!.First());
    var rsaparam = RsaParametersInfo.DeserializeRsaParam(data!.Last());
    return new RsaPublicKey(rsaparam, index);
  }

  public static RsaPublicKey FromRsaPublicKeyPmeiUtf8(byte[] rsa_pmei) =>
    FromRsaPublicKeyPmei(Encoding.UTF8.GetString(rsa_pmei));

  public static void SavePublicKeyToFile(RsaPublicKey pupkey, string username)
  {
    var ext = ".pup";
    var filepath = EcSettings.ToEcCurrentFolderUser(username);
    var filename = Path.Combine(filepath, pupkey.RsaIndex + ext);

    var index = Encoding.UTF8.GetBytes(pupkey.RsaIndex);
    var rsaparam = RsaParametersInfo.SerializeRsaParam(pupkey.PublicKey);
    var bytes = EcService.SerializeJson(new[] { index, rsaparam });

    var (h, f) = (Rsa_PublicKey_Footer_Pmei(true), Rsa_PublicKey_Footer_Pmei(true));
    var _ = PMEI.SavePmeiToFile(filename, bytes, h, f); //_ = idx = filename
  }

  public static RsaPublicKey LoadPublicKeyFromFile(string filename)
  {
    var ext = ".pup";
    if (Path.GetExtension(filename) != ext)
      throw new ArgumentException($"{nameof(filename)} is failed");

    var data = EcService.LoadFromFile(filename);
    return FromRsaPublicKeyPmei(data);
  }
}


