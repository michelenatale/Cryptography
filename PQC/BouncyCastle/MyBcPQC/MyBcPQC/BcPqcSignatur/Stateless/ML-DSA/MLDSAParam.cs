

//Diese Enum funktionert, da JSON Probleme hat, die Klasse
//'MLDSAParameters' zu serialisieren.

//Siehe auch
//...\BcPqcServices\BcPqcServicesUtils\BcPqcServicesUtils.cs


using System.Text.Json.Serialization;

namespace michele.natale.BcPqcs;


/// <summary>
/// ML-DSA-Params
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum MLDsaParam : byte
{
  [JsonInclude] Ml_Dsa_44 = 0,
  [JsonInclude] Ml_Dsa_65,
  [JsonInclude] Ml_Dsa_87,
  //[JsonInclude] Ml_Dsa_44_with_sha512,
  //[JsonInclude] Ml_Dsa_65_with_sha512,
  //[JsonInclude] Ml_Dsa_87_with_sha512,
}