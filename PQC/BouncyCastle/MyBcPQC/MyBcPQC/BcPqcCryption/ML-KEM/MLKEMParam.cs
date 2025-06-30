

//Diese Enum funktionert, da JSON Probleme hat, die Klasse
//'MLKemParameters' zu serialisieren.

//Siehe auch
//...\BcPqcServices\BcPqcServicesUtils\BcPqcServicesUtils.cs


using System.Text.Json.Serialization;

namespace michele.natale.BcPqcs;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum MLKemParam : byte
{
  [JsonInclude] Ml_Kem_512 = 0,
  [JsonInclude] Ml_Kem_768,
  [JsonInclude] Ml_Kem_1024
}
