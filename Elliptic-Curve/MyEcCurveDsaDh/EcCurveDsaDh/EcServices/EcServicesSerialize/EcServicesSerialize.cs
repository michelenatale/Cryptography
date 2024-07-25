

using System.Text.Json;
using System.Security.Cryptography;

namespace michele.natale.EcCurveDsaDh;

partial class EcService
{

  #region  EcCurve Serialization
  internal static byte[] SerializeEcCurve(ECCurve curve)
  {
    var oid = curve.Oid is null ? new Oid() : curve.Oid;
    return SerializeJson(oid);
  }


  internal static ECCurve DeserializeEcCurve(byte[] curve)
  {
    var oid = DeserializeJson<Oid>(curve);
    if (oid!.Value is null && oid.FriendlyName is null)
      return new ECCurve();
    return ECCurve.CreateFromOid(oid);
  }
  #endregion  EcCurve Serialization

  #region Json Serialization

  public static byte[] SerializeJson<T>(T input) =>
     JsonSerializer.SerializeToUtf8Bytes(input!, JOption);


  public static T? DeserializeJson<T>(byte[] input) =>
    input == Array.Empty<byte>() ? default :
      JsonSerializer.Deserialize<T>(input, JOption);


  public static readonly JsonSerializerOptions JOption =
    new()
    {
      WriteIndented = true
    };
  #endregion  Json Serialization
}
