
using System.Text;


namespace michele.natale.Cryptography;


using static michele.natale.Schnorrs.Services.SchnorrServices;


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
        ? ToIndexHex(RngCryptoInt64(1).First().ToString(), a, islowerhex)
        : ToIndexHex(id, a, islowerhex);
    result.Append($"{idhex}\n");

    result.Append($"{ToHex(message, islowerhex)}\n");

    result.Append(footer_pmei);
    return result.ToString();
  }

  public static (string ID, byte[] Message) FromPmei(
    string message_pmei, string header_pmei, string footer_pmei)
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


  //************** ************** ************** ************** 
  //************** ************** ************** ************** 


  public static (string H, string F) SchnorrPQGHPmeiHF(
    bool timestamp = true, bool _enc = false)
  {
    var enctxt = _enc ? " - ENCRYPTION" : string.Empty;
    var tstamp = timestamp ? $" {DateTimeOffset.UtcNow}" : string.Empty;
    var footer = $"-----END SCHNORR PQGH GROUP{enctxt}-----";
    var header = $"-----BEGIN SCHNORR PQGH GROUP{enctxt}-----{tstamp}";
    return (header, footer);
  }

  public static (string H, string F) SchnorrParamPmeiHF(
    bool timestamp = true, bool _enc = false)
  {
    var enctxt = _enc ? " - ENCRYPTION" : string.Empty;
    var tstamp = timestamp ? $" {DateTimeOffset.UtcNow}" : string.Empty;
    var footer = $"-----END SCHNORR PARAMETERS KEYS{enctxt}-----";
    var header = $"-----BEGIN SCHNORR PARAMETERS KEYS{enctxt}-----{tstamp}";
    return (header, footer);
  }
}
