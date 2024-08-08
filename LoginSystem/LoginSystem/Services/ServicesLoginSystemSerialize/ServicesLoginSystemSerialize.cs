
using System.Text.Json;

namespace michele.natale.LoginSystems.Services;

partial class AppServices
{
  #region Json Serialization

  /// <summary>
  /// Json Serialize
  /// </summary>
  /// <typeparam name="T">Generic Type</typeparam>
  /// <param name="input">Input</param>
  /// <returns></returns>
  public byte[] SerializeJson<T>(T input) =>
     JsonSerializer.SerializeToUtf8Bytes(input!, JOption);

  /// <summary>
  /// Json Deserialize
  /// </summary>
  /// <typeparam name="T">Generic type</typeparam>
  /// <param name="input">Input</param>
  /// <returns></returns>
  public T? DeserializeJson<T>(byte[] input) =>
    input == Array.Empty<byte>() ? default :
      JsonSerializer.Deserialize<T>(input, JOption);


  /// <summary>
  /// Json Serialize Option
  /// </summary>
  public readonly JsonSerializerOptions JOption =
    new()
    {
      WriteIndented = true
    };
  #endregion  Json Serialization

}
