
//using System.Security.Cryptography;

//namespace michele.natale;


//partial class NetServicesCrypto
//{ 
//  public static MLDsaAlgorithm ToMLDsaAlgorithm(
//    MLDsaParam param) => param switch
//  {
//    MLDsaParam.Ml_Dsa_44 => MLDsaAlgorithm.MLDsa44,
//    MLDsaParam.Ml_Dsa_65 => MLDsaAlgorithm.MLDsa65,
//    MLDsaParam.Ml_Dsa_87 => MLDsaAlgorithm.MLDsa87,
//    _ => throw new Exception(),
//  };

//  public static MLDsaParam FromMLDsaAlgorithm(MLDsaAlgorithm parameter)
//  {
//    ArgumentNullException.ThrowIfNull(parameter);

//    if (parameter == MLDsaAlgorithm.MLDsa44) return MLDsaParam.Ml_Dsa_44;
//    else if (parameter == MLDsaAlgorithm.MLDsa65) return MLDsaParam.Ml_Dsa_65;
//    else if (parameter == MLDsaAlgorithm.MLDsa87) return MLDsaParam.Ml_Dsa_87;

//    throw new ArgumentOutOfRangeException(nameof(parameter), $"{nameof(parameter)} has failded!");
//  }

//  public static MLDsaAlgorithm[] ToMLDsaAlgorithm()
//  {
//    var a = MLDsaAlgorithm.MLDsa44;
//    var b = MLDsaAlgorithm.MLDsa65;
//    var c = MLDsaAlgorithm.MLDsa87; 
//    return [a, b, c,];
//  }
//}
