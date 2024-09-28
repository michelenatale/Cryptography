
using System.Text.Json;

namespace michele.natale.Schnorrs.Services;

partial class SchnorrServices
{
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
