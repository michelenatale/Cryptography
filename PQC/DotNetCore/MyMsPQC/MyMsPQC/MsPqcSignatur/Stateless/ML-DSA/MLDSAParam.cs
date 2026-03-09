 

using System.Text.Json.Serialization;

namespace michele.natale.MsPqcs;


/// <summary>
/// ML-DSA-Params
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum MLDsaParam : byte
{
  [JsonInclude] Ml_Dsa_44 = 0,
  [JsonInclude] Ml_Dsa_65,
  [JsonInclude] Ml_Dsa_87,
}