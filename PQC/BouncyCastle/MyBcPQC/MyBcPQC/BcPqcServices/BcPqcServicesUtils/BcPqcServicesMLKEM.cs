
using Org.BouncyCastle.Crypto.Parameters;

namespace michele.natale.Services;

using BcPqcs;

partial class BcPqcServices
{

  public static MLKemParameters ToMLKemParameters(MLKemParam param) => param switch
  {
    MLKemParam.Ml_Kem_512 => MLKemParameters.ml_kem_512,
    MLKemParam.Ml_Kem_768 => MLKemParameters.ml_kem_768,
    MLKemParam.Ml_Kem_1024 => MLKemParameters.ml_kem_1024,
    _ => throw new Exception(),
  };



  public static MLKemParam FromMLKemParameters(MLKemParameters parameter)
  {
    ArgumentNullException.ThrowIfNull(parameter);

    if (parameter == MLKemParameters.ml_kem_512) return MLKemParam.Ml_Kem_512;
    else if (parameter == MLKemParameters.ml_kem_768) return MLKemParam.Ml_Kem_768;
    else if (parameter == MLKemParameters.ml_kem_1024) return MLKemParam.Ml_Kem_1024;

    throw new ArgumentOutOfRangeException(nameof(parameter), $"{nameof(parameter)} has failded!");
  }



  public static MLKemParameters[] ToMLKemParameters()
  {
    var a = MLKemParameters.ml_kem_512;
    var b = MLKemParameters.ml_kem_768;
    var c = MLKemParameters.ml_kem_1024;

    return [a, b, c];
  }
}
