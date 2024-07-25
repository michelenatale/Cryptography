


using System.Text; 


namespace michele.natale.EcCurveDsaDh;


//Private Message Encryption Information
public class PMEI
{
  public static string ToPmei(
    ReadOnlySpan<byte> message, string header_pmei,
    string footer_pmei, bool islowerhex = true,
    string id = "")
  {
    var result = new StringBuilder();
    result.Append($"{header_pmei}\n");

    var a = "ID = ";
    var idhex = string.IsNullOrEmpty(id)
        ? ToIndexHex(EcService.NextInt64().ToString(), a, islowerhex)
        : ToIndexHex(id, a, islowerhex);
    result.Append($"{idhex}\n");

    result.Append($"{ToHex(message, islowerhex)}\n");

    result.Append(footer_pmei);
    return result.ToString();
  }

  public static (string ID, byte[] Message) FromPmei(
    string message_pmei, string header_pmei,
    string footer_pmei)
  {
    if (message_pmei.StartsWith(header_pmei))
      if (message_pmei.EndsWith(footer_pmei))
      {
        var hex = message_pmei
         .Split('\n', StringSplitOptions.RemoveEmptyEntries);
        var id = FromIndexHex(hex[1]);
        return (id, Convert.FromHexString(hex[2]));
      }
    throw new ArgumentException($"{nameof(message_pmei)} format is false");
  }

  public static string SavePmeiToFile(
    //Hier mit filename
    string filename, ReadOnlySpan<byte> message,
    string header_pmei, string footer_pmei,
    bool to_lower_hex = true)
  {
    var id = Path.GetFileNameWithoutExtension(filename);
    EcService.SaveToFile(
     filename, ToPmei(message, header_pmei,
     footer_pmei, to_lower_hex, id));
    return id;
  }

  public static string SavePmeiToFile(
    //Hier ohne filename, filename = index
    ReadOnlySpan<byte> message, string header_pmei,
    string footer_pmei, string folder, string fileext,
    bool to_lower_hex = true)
  {
    var id = NewFileId(folder, fileext);
    EcService.SaveToFile(
     id.FileName, ToPmei(message, header_pmei,
     footer_pmei, to_lower_hex));
    return id.ID.ToString();
  }

  public static (string ID, byte[] Message) LoadPmeiFromFile(
    string filename, string header_pmei, string footer_pmei)
  {
    var message_pmei = EcService.LoadFromFile(filename);
    return FromPmei(message_pmei, header_pmei, footer_pmei);
  }

  private static (long ID, string FileName) NewFileId(
    string foldername, string extname)
  {
    //Erzeugt ein neuer Dateiname, und prüft
    //ob die Datei schon vorhanden ist.
    string ext = extname, dir = foldername;
    var newfn = EcService.NextInt64();
    var result = Path.Combine(dir!, newfn.ToString() + ext);
    while (File.Exists(result))
    {
      newfn = EcService.NextInt64();
      result = Path.Combine(dir!, newfn.ToString() + ext);
    }
    return (newfn, result);
  }

  private static string ToHex(ReadOnlySpan<byte> data, bool islower = true) =>
    islower ? Convert.ToHexString(data).ToLower()
    : Convert.ToHexString(data).ToUpper();

  private static string ToHex(string data, bool islower = true)
  {
    var result = Encoding.UTF8.GetBytes(data);
    return ToHex(result, islower);
  }

  private static byte[] FromHex(string hexstr) =>
    Convert.FromHexString(hexstr);

  private static string FromHexToString(string hexstr) =>
    Encoding.UTF8.GetString(FromHex(hexstr));


  private static string ToIndexHex(
    string idx, string frontadd = "ID = ", bool islowerhex = true) =>
      ToHex(frontadd + idx, islowerhex);

  private static string FromIndexHex(string hexstr, string frontadd = "ID = ")
  {
    var str = FromHexToString(hexstr);
    var i = str.IndexOf(frontadd);
    return str.Substring(i, str.Length - i);
  }

  public static (string H, string F) EcMasterKeyPmeiHF()
  {
    var footer = "-----END EC MASTER KEY-----";
    var header = "-----BEGIN EC MASTER KEY-----";
    return (header, footer);
  }

  public static (string H, string F) EcPrivateKeyPmeiHF(bool _enc = true)
  {
    var enctxt = _enc ? " - ENCRYPTION" : string.Empty;
    var footer = $"-----END EC PRIVATE KEY{enctxt}-----";
    var header = $"-----BEGIN EC PRIVATE KEY{enctxt}-----";
    return (header, footer);
  }

  public static (string H, string F) EcPublicKeyPmeiHF(bool timestamp = true)
  {
    var tstamp = timestamp ? $" {DateTimeOffset.UtcNow}" : string.Empty;
    var footer = "-----END EC PUBLIC KEY-----";
    var header = $"-----BEGIN EC PUBLIC KEY-----{tstamp}";
    return (header, footer);
  }

  public static (string H, string F) EcSignaturPmeiHF(bool timestamp = true, bool _enc = true)
  {
    var enctxt = _enc ? " - ENCRYPTION" : string.Empty;
    var tstamp = timestamp ? $" {DateTimeOffset.UtcNow}" : string.Empty;
    var footer = $"-----END EC DSA KEY{enctxt}-----";
    var header = $"-----BEGIN EC DSA KEY{enctxt}-----{tstamp}";
    return (header, footer);
  }

  public static (string H, string F) RsaSignaturPmeiHF(bool timestamp = true, bool _enc = true)
  {
    var enctxt = _enc ? " - ENCRYPTION" : string.Empty;
    var tstamp = timestamp ? $" {DateTimeOffset.UtcNow}" : string.Empty;
    var footer = $"-----END RSA SIGN KEY{enctxt}-----";
    var header = $"-----BEGIN EC SIGN KEY{enctxt}-----{tstamp}";
    return (header, footer);
  }

  public static (string H, string F) RsaPublicKeyPmeiHF(bool timestamp = true)
  {
    var tstamp = timestamp ? $" {DateTimeOffset.UtcNow}" : string.Empty;
    var footer = "-----END RSA PUBLIC KEY-----";
    var header = $"-----BEGIN RSA PUBLIC KEY-----{tstamp}";
    return (header, footer);
  }

  public static (string H, string F) RsaPrivateKeyPmeiHF(bool timestamp = true, bool _enc = true)
  {
    var enctxt = _enc ? " - ENCRYPTION" : string.Empty;
    var tstamp = timestamp ? $" {DateTimeOffset.UtcNow}" : string.Empty;
    var footer = $"-----END RSA PRIVATE KEY{enctxt}-----";
    var header = $"-----BEGIN RSA PRIVATE KEY{enctxt}-----{tstamp}";
    return (header, footer);
  }
}
