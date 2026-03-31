
using System.Security.Cryptography;

namespace michele.natale.Tests;

partial class CryptoPqcMlKemTest
{
  public static MLKemAlgorithm[] ToMLKemAlgorithm()
  {
    var a = MLKemAlgorithm.MLKem512;
    var b = MLKemAlgorithm.MLKem768;
    var c = MLKemAlgorithm.MLKem1024;

    return [a, b, c];
  }

  public static MLKemAlgorithm ToMLKemAlgorithm(
    MLKemParam param) => param switch
    {
      MLKemParam.Ml_Kem_512 => MLKemAlgorithm.MLKem512,
      MLKemParam.Ml_Kem_768 => MLKemAlgorithm.MLKem768,
      MLKemParam.Ml_Kem_1024 => MLKemAlgorithm.MLKem1024,
      _ => throw new Exception(),
    };


  public static MLKemParam FromMLKemAlgorithm(
    MLKemAlgorithm parameter)
  {
    ArgumentNullException.ThrowIfNull(parameter);

    if (parameter == MLKemAlgorithm.MLKem512) return MLKemParam.Ml_Kem_512;
    else if (parameter == MLKemAlgorithm.MLKem768) return MLKemParam.Ml_Kem_768;
    else if (parameter == MLKemAlgorithm.MLKem1024) return MLKemParam.Ml_Kem_1024;

    throw new ArgumentOutOfRangeException(nameof(parameter), $"{nameof(parameter)} has failded!");
  }
}
