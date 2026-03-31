

using System.Text.Json;
using System.Text.Json.Serialization;

namespace michele.natale;


public partial class NetServicesCrypto
{
  [JsonSerializable(typeof(MlKemKeyPairInfo))]
  public partial class JSSerializer : JsonSerializerContext
  {
    // -------------------------
    //  AOT-SAFE SERIALIZATION
    // -------------------------
    public static byte[] SerializeJson(MlKemKeyPairInfo input)
    {
      var info = Default.MlKemKeyPairInfo;
      return JsonSerializer.SerializeToUtf8Bytes(input, info);
    }

    // -------------------------
    // AOT-SAFE DESERIALIZATION
    // -------------------------
    public static MlKemKeyPairInfo? DeserializeJson(ReadOnlySpan<byte> input)
    {
      if (input.Length == 0)
        return default;

      var info = Default.MlKemKeyPairInfo;
      return JsonSerializer.Deserialize(input, info);
    }

    //public static new readonly JsonSerializerOptions JOptions =
    //    new()
    //    {
    //      WriteIndented = true,
    //      TypeInfoResolver = Default,
    //    };
  }
}



