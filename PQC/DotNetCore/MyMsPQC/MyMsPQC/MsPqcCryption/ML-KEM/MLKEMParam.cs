



using System.Text.Json.Serialization;

namespace michele.natale.MsPqcs;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum MLKemParam : byte
{
  [JsonInclude] Ml_Kem_512 = 0,
  [JsonInclude] Ml_Kem_768,
  [JsonInclude] Ml_Kem_1024
}
