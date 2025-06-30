

//Diese Enum funktionert, da JSON Probleme hat, die Klasse
//'MLDSAParameters' zu serialisieren.

//Siehe auch
//...\BcPqcServices\BcPqcServicesUtils\BcPqcServicesUtils.cs


using System.Text.Json.Serialization;

namespace michele.natale.BcPqcs;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SLHDsaParam : byte
{
  [JsonInclude] Slh_Dsa_sha2_128s = 0,
  [JsonInclude] Slh_Dsa_sha2_128f,
  [JsonInclude] Slh_Dsa_shake_128s,
  [JsonInclude] Slh_Dsa_shake_128f,

  [JsonInclude] Slh_Dsa_sha2_192s,
  [JsonInclude] Slh_Dsa_sha2_192f,
  [JsonInclude] Slh_Dsa_shake_192s,
  [JsonInclude] Slh_Dsa_shake_192f,

  [JsonInclude] Slh_Dsa_sha2_256s,
  [JsonInclude] Slh_Dsa_sha2_256f,
  [JsonInclude] Slh_Dsa_shake_256s,
  [JsonInclude] Slh_Dsa_shake_256f,

  //[JsonInclude] Slh_Dsa_sha2_128s_with_sha256,
  //[JsonInclude] Slh_Dsa_shake_128s_with_shake128,
  //[JsonInclude] Slh_Dsa_sha2_128f_with_sha256,
  //[JsonInclude] Slh_Dsa_shake_128f_with_shake128,
  //[JsonInclude] Slh_Dsa_sha2_192s_with_sha512,
  //[JsonInclude] Slh_Dsa_shake_192s_with_shake256,
  //[JsonInclude] Slh_Dsa_sha2_192f_with_sha512,
  //[JsonInclude] Slh_Dsa_shake_192f_with_shake256,
  //[JsonInclude] Slh_Dsa_sha2_256s_with_sha512,
  //[JsonInclude] Slh_Dsa_shake_256s_with_shake256,
  //[JsonInclude] Slh_Dsa_sha2_256f_with_sha512,
  //[JsonInclude] Slh_Dsa_shake_256f_with_shake256,
}

