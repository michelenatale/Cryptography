
//using System.Text.Json;
//using System.Text.Json.Serialization;
//using System.Text.Json.Serialization.Metadata;

//namespace michele.natale;

//using Services.PQCs;

//[JsonSerializable(typeof(MlKemKeyPairInfo))]
//public partial class NetServicesSerialize : JsonSerializerContext
//{
//  public static readonly JsonSerializerOptions Options =
//      new()
//      {
//        WriteIndented = true,
//        TypeInfoResolver = Default
//      };

//  // -------------------------------
//  // AOT-SICHERE SERIALISIERUNG
//  // -------------------------------
//  public static byte[] SerializeJson<T>(T input)
//  {
//    JsonTypeInfo<T> info = Default.GetTypeInfo<T>();
//    return JsonSerializer.SerializeToUtf8Bytes(input!, info);
//  }

//  // -------------------------------
//  // AOT-SICHERE DESERIALISIERUNG
//  // -------------------------------
//  public static T? DeserializeJson<T>(ReadOnlySpan<byte> input)
//  {
//    if (input.Length == 0)
//      return default;

//    JsonTypeInfo<T> info = NetServicesSerialize.Default.GetTypeInfo<T>();
//    return JsonSerializer.Deserialize(input, info);
//  }
//}
