
using Org.BouncyCastle.Crypto.Parameters;

namespace michele.natale.Services;

using BcPqcs;

partial class BcPqcServices
{

  public static MLDsaParameters ToMLDsaParameters(MLDsaParam param) => param switch
  {
    MLDsaParam.Ml_Dsa_44 => MLDsaParameters.ml_dsa_44,
    MLDsaParam.Ml_Dsa_65 => MLDsaParameters.ml_dsa_65,
    MLDsaParam.Ml_Dsa_87 => MLDsaParameters.ml_dsa_87,
    //MLDsaParam.Ml_Dsa_44_with_sha512 => MLDsaParameters.ml_dsa_44_with_sha512,
    //MLDsaParam.Ml_Dsa_65_with_sha512 => MLDsaParameters.ml_dsa_65_with_sha512,
    //MLDsaParam.Ml_Dsa_87_with_sha512 => MLDsaParameters.ml_dsa_87_with_sha512,
    _ => throw new Exception(),
  };

  public static MLDsaParam FromMLDsaParameters(MLDsaParameters parameter)
  {
    ArgumentNullException.ThrowIfNull(parameter);

    if (parameter == MLDsaParameters.ml_dsa_44) return MLDsaParam.Ml_Dsa_44;
    else if (parameter == MLDsaParameters.ml_dsa_65) return MLDsaParam.Ml_Dsa_65;
    else if (parameter == MLDsaParameters.ml_dsa_87) return MLDsaParam.Ml_Dsa_87;
    //else if (parameter == MLDsaParameters.ml_dsa_44_with_sha512) return MLDsaParam.Ml_Dsa_44_with_sha512;
    //else if (parameter == MLDsaParameters.ml_dsa_65_with_sha512) return MLDsaParam.Ml_Dsa_65_with_sha512;
    //else if (parameter == MLDsaParameters.ml_dsa_87_with_sha512) return MLDsaParam.Ml_Dsa_87_with_sha512;

    throw new ArgumentOutOfRangeException(nameof(parameter), $"{nameof(parameter)} has failded!");
  }

  public static MLDsaParameters[] ToMLDsaParameters()
  {
    var a = MLDsaParameters.ml_dsa_44;
    var b = MLDsaParameters.ml_dsa_65;
    var c = MLDsaParameters.ml_dsa_87;
    //var d = MLDsaParameters.ml_dsa_44_with_sha512;
    //var e = MLDsaParameters.ml_dsa_65_with_sha512;
    //var f = MLDsaParameters.ml_dsa_87_with_sha512;
    return [a, b, c, /*d, e, f*/];
  }
}
