

//PMEI = Private Message Encryption Information

using System.Text;
using System.Security.Cryptography;

namespace michele.natale.EcCurveDsaDh;

public class EcPublicKey
{
  public string EcIndex = string.Empty;
  public ECParameters PublicKey { get; private set; } = new();

  public static string Ec_PublicKey_Footer_Pmei(bool timestamp = true) =>
    PMEI.EcPublicKeyPmeiHF(timestamp).F;
  public static string Ec_PublicKey_Header_Pmei(bool timestamp = true) =>
    PMEI.EcPublicKeyPmeiHF(timestamp).H;


  public EcPublicKey()
  {
  }

  public EcPublicKey(string ec_pukkey_pmei)
  {
    var data = FromEcPublicKeyPmei(ec_pukkey_pmei);
    this.PublicKey = data.PublicKey;
    this.EcIndex = data.EcIndex;
  }

  public EcPublicKey(byte[] ec_pukkey_pmei_utf8) =>
    this.PublicKey = FromEcPublicKeyPmeiUtf8(ec_pukkey_pmei_utf8).PublicKey;

  public EcPublicKey(EcPublicKey ecpupkey)
  {
    this.EcIndex = ecpupkey.EcIndex;
    this.PublicKey = ecpupkey.PublicKey;
  }

  public EcPublicKey(ECParameters keypair, string index = "")
  {
    this.EcIndex = index;
    this.PublicKey = new ECParameters
    {
      Q = new ECPoint()
      {
        X = [.. keypair.Q.X!],
        Y = [.. keypair.Q.Y!],
      },
      Curve = EcService.CopyOrEmpty(keypair.Curve),
    };
  }

  public EcPublicKey((string index, ECParameters keypair) data, bool _explicit = false)
  {
    this.EcIndex = data.index;//funkst nicht
    this.EcIndex = string.Join("", data.index.ToCharArray());
    this.PublicKey = new ECParameters
    {
      Q = new ECPoint()
      {
        X = [.. data.keypair.Q.X!],
        Y = [.. data.keypair.Q.Y!],
      },
      Curve = _explicit ? new ECCurve() : data.keypair.Curve,
    };
  }

  public EcPublicKey(ECCurve ecurve, string index = "", bool _explicit = false)
  {
    var keypair = EcService.GenerateEcDhKeyPair(ecurve).PrivateKey;
    this.EcIndex = index;
    this.PublicKey = new ECParameters
    {
      Q = new ECPoint()
      {
        X = [.. keypair.Q.X!],
        Y = [.. keypair.Q.Y!],
      },
      Curve = _explicit ? new ECCurve() : keypair.Curve,
    };
  }

  public ECPoint ToPublicKeyEcPoint() =>
    ToPublicKeyEcPoint(this.PublicKey);

  public (byte[] X, byte[] Y) ToPublicKeyXYBytes() =>
    ToPublicKeyXYBytes(this.PublicKey);

  //public void SavePublicKeyToFile() =>
  //  SavePublicKeyToFile(this, string.Empty);
  public void SavePublicKeyToFile(string username) =>
    SavePublicKeyToFile(this, username);

  public static EcPublicKey ToPublicKey(ECParameters ecparam, string index = "", bool _explicit = false)
  {
    if (_explicit) return new EcPublicKey(ecparam, index);
    var pupkey = new ECParameters
    {
      Q = new ECPoint()
      {
        X = [.. ecparam.Q.X!],
        Y = [.. ecparam.Q.Y!],
      },
      Curve = _explicit ? new ECCurve() : ecparam.Curve,
    };
    return new EcPublicKey(pupkey, index);
  }

  public override string ToString()
  {
    var sb = new StringBuilder();
    sb.AppendLine($"ID = {this.EcIndex}");
    sb.AppendLine($"Curve Oid.Value = {this.PublicKey.Curve.Oid.Value}");
    sb.AppendLine($"Curve Oid.FriendlyName = {this.PublicKey.Curve.Oid.FriendlyName}");
    sb.AppendLine($"PublicKey X = {string.Join(" ", this.PublicKey.Q.X!)}");
    sb.AppendLine($"PublicKey Y = {string.Join(" ", this.PublicKey.Q.Y!)}");
    return sb.ToString();
  }

  public static string ToEcPublicKeyPmei(EcPublicKey ecpupkey, bool _explicit = false) =>
    ToEcPublicKeyPmei(ecpupkey.PublicKey, ecpupkey.EcIndex, _explicit);

  public static byte[] ToEcPublicKeyPmeiUtf8(EcPublicKey ecpupkey, bool _explicit = false) =>
    Encoding.UTF8.GetBytes(ToEcPublicKeyPmei(ecpupkey, _explicit));

  public static string ToEcPublicKeyPmei(ECParameters ecparam, string index = "", bool _explicit = false)
  {
    var ecp = EcService.Copy(ecparam, _explicit);
    var bytes = EcParametersInfo.SerializeEcParam(ecp);
    var (h, f) = (Ec_PublicKey_Header_Pmei(), Ec_PublicKey_Footer_Pmei());
    return PMEI.ToPmei(bytes, h, f, true, index);
  }

  public static byte[] ToEcPublicKeyPmeiUtf8(ECParameters ecpupkey, string index, bool _explicit = false) =>
    Encoding.UTF8.GetBytes(ToEcPublicKeyPmei(ecpupkey, index, _explicit));

  public static EcPublicKey FromEcPublicKeyPmei(string ecparams_pmei)
  {
    var (h, f) = (Ec_PublicKey_Header_Pmei(false), Ec_PublicKey_Footer_Pmei(false));
    var (idx, msg) = PMEI.FromPmei(ecparams_pmei, h, f);
    idx = idx["Id = ".Length..];
    var result = new EcPublicKey((idx, EcParametersInfo.DeserializeEcParam(msg)));
    return result;
  }
  public static EcPublicKey FromEcPublicKeyPmeiUtf8(byte[] ecparamspem) =>
    FromEcPublicKeyPmei(Encoding.UTF8.GetString(ecparamspem));

  public static ECPoint ToPublicKeyEcPoint(EcPublicKey ecpubkey) =>
     EcService.ToPublicKeyEcPoint(ecpubkey.PublicKey);

  public static (byte[] X, byte[] Y) ToPublicKeyXYBytes(EcPublicKey ecpubkey) =>
    ToPublicKeyXYBytes(ecpubkey.PublicKey);

  public static ECPoint ToPublicKeyEcPoint(ECParameters ecpubkey) =>
     EcService.ToPublicKeyEcPoint(ecpubkey);

  public static (byte[] X, byte[] Y) ToPublicKeyXYBytes(ECParameters ecpubkey) =>
    ([.. ecpubkey.Q.X], [.. ecpubkey.Q.Y]);

  public static byte[] ToPublicKeyConcatBytes(EcPublicKey pubkey, byte prefix) =>
    ToPublicKeyConcatBytes(pubkey.PublicKey.Q, prefix);
  public static byte[] ToPublicKeyConcatBytes(ECPoint pubkey, byte prefix) =>
    ToPuplicKeyConcat(pubkey, prefix);

  public static (string Index, ECParameters EcPublicKey) ToPublicKeyData(EcPublicKey keypair) =>
    (keypair.EcIndex, keypair.PublicKey);

  public static ECParameters ToPublicKeyInformation(ECParameters keypair, bool _explicit) =>
     new()
     {
       Q = new ECPoint()
       {
         X = [.. keypair.Q.X!],
         Y = [.. keypair.Q.Y!],
       },
       Curve = _explicit ? new ECCurve() : keypair.Curve,
     };

  public static byte[] ToPuplicKeyConcat(ECPoint q, byte prefix = 0x04) =>
     EcParametersInfo.ToPuplicKeyConcat(q, prefix);

  public static void SavePublicKeyToFile(EcPublicKey pupkey, string username)
  {
    var ext = ".pup";
    var filepath = EcSettings.ToEcCurrentFolderUser(username);
    var bytes = EcParametersInfo.SerializeEcParam(pupkey.PublicKey);
    var (h, f) = (Ec_PublicKey_Header_Pmei(), Ec_PublicKey_Footer_Pmei());
    PMEI.SavePmeiToFile(bytes, h, f, filepath, ext);
  }

  public static EcPublicKey LoadPublicKeyFromFile(string filename)
  {
    var ext = ".pup";
    if (Path.GetExtension(filename) != ext)
      throw new ArgumentException($"{nameof(filename)} is failed!");

    var (h, f) = (Ec_PublicKey_Header_Pmei(false), Ec_PublicKey_Footer_Pmei(false));
    var (id, msg) = PMEI.LoadPmeiFromFile(filename, h, f);
    var ecparam = EcParametersInfo.DeserializeEcParam(msg);
    return new EcPublicKey(ecparam, id);
  }
}
