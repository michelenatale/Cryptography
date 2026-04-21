 

using System.Text.Json.Serialization;

namespace michele.natale;


/// <summary>
/// ML-DSA-Params
/// </summary>
public enum MLDsaParam : byte
{
  [JsonInclude] Ml_Dsa_44 = 0,
  [JsonInclude] Ml_Dsa_65,
  [JsonInclude] Ml_Dsa_87,
}

