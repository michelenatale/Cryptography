
//using System.Security.Cryptography;

//namespace michele.natale;


//partial class MsPqcServices
//{

//  public static SlhDsaAlgorithm ToSlhDsaAlgorithm(SLHDsaParam param) => param switch
//  {
//    SLHDsaParam.Slh_Dsa_sha2_128s => SlhDsaAlgorithm.SlhDsaSha2_128s,
//    SLHDsaParam.Slh_Dsa_sha2_128f => SlhDsaAlgorithm. SlhDsaSha2_128f,
//    SLHDsaParam.Slh_Dsa_shake_128s => SlhDsaAlgorithm.SlhDsaShake128s,
//    SLHDsaParam.Slh_Dsa_shake_128f => SlhDsaAlgorithm.SlhDsaShake128f,

//    SLHDsaParam.Slh_Dsa_sha2_192s => SlhDsaAlgorithm.SlhDsaSha2_192s,
//    SLHDsaParam.Slh_Dsa_sha2_192f => SlhDsaAlgorithm.SlhDsaSha2_192f,
//    SLHDsaParam.Slh_Dsa_shake_192s => SlhDsaAlgorithm.SlhDsaShake192s,
//    SLHDsaParam.Slh_Dsa_shake_192f => SlhDsaAlgorithm.SlhDsaShake192f,

//    SLHDsaParam.Slh_Dsa_sha2_256s => SlhDsaAlgorithm.SlhDsaSha2_256s,
//    SLHDsaParam.Slh_Dsa_sha2_256f => SlhDsaAlgorithm.SlhDsaSha2_256f,
//    SLHDsaParam.Slh_Dsa_shake_256s => SlhDsaAlgorithm.SlhDsaShake256s,
//    SLHDsaParam.Slh_Dsa_shake_256f => SlhDsaAlgorithm.SlhDsaShake256f,

//    //SLHDsaParam.Slh_Dsa_sha2_128s_with_sha256     => SlhDsaAlgorithm.slh_dsa_sha2_128s_with_sha256, 
//    //SLHDsaParam.Slh_Dsa_shake_128s_with_shake128  => SlhDsaAlgorithm.slh_dsa_shake_128s_with_shake128,
//    //SLHDsaParam.Slh_Dsa_sha2_128f_with_sha256     => SlhDsaAlgorithm.slh_dsa_sha2_128f_with_sha256,
//    //SLHDsaParam.Slh_Dsa_shake_128f_with_shake128  => SlhDsaAlgorithm.slh_dsa_shake_128f_with_shake128,
//    //SLHDsaParam.Slh_Dsa_sha2_192s_with_sha512     => SlhDsaAlgorithm.slh_dsa_sha2_192s_with_sha512,   
//    //SLHDsaParam.Slh_Dsa_shake_192s_with_shake256  => SlhDsaAlgorithm.slh_dsa_shake_192s_with_shake256,
//    //SLHDsaParam.Slh_Dsa_sha2_192f_with_sha512     => SlhDsaAlgorithm.slh_dsa_sha2_192f_with_sha512,
//    //SLHDsaParam.Slh_Dsa_shake_192f_with_shake256  => SlhDsaAlgorithm.slh_dsa_shake_192f_with_shake256,
//    //SLHDsaParam.Slh_Dsa_sha2_256s_with_sha512     => SlhDsaAlgorithm.slh_dsa_sha2_256s_with_sha512,   
//    //SLHDsaParam.Slh_Dsa_shake_256s_with_shake256  => SlhDsaAlgorithm.slh_dsa_shake_256s_with_shake256,
//    //SLHDsaParam.Slh_Dsa_sha2_256f_with_sha512     => SlhDsaAlgorithm.slh_dsa_sha2_256f_with_sha512,   
//    //SLHDsaParam.Slh_Dsa_shake_256f_with_shake256  =>SlhDsaAlgorithm.slh_dsa_shake_256f_with_shake256,
//    _ => throw new Exception(),
//  };

//  public static SLHDsaParam FromSlhDsaAlgorithm(SlhDsaAlgorithm parameter)
//  {
//    ArgumentNullException.ThrowIfNull(parameter);

//    if (parameter == SlhDsaAlgorithm.SlhDsaSha2_128s) return SLHDsaParam.Slh_Dsa_sha2_128s;
//    else if (parameter == SlhDsaAlgorithm.SlhDsaSha2_128f) return SLHDsaParam.Slh_Dsa_sha2_128f;
//    else if (parameter == SlhDsaAlgorithm.SlhDsaShake128s) return SLHDsaParam.Slh_Dsa_shake_128s;
//    else if (parameter == SlhDsaAlgorithm.SlhDsaShake128f) return SLHDsaParam.Slh_Dsa_shake_128f;

//    else if (parameter == SlhDsaAlgorithm.SlhDsaSha2_192s) return SLHDsaParam.Slh_Dsa_sha2_192s;
//    else if (parameter == SlhDsaAlgorithm.SlhDsaSha2_192f) return SLHDsaParam.Slh_Dsa_sha2_192f;
//    else if (parameter == SlhDsaAlgorithm.SlhDsaShake192s) return SLHDsaParam.Slh_Dsa_shake_192s;
//    else if (parameter == SlhDsaAlgorithm.SlhDsaShake192f) return SLHDsaParam.Slh_Dsa_shake_192f;

//    else if (parameter == SlhDsaAlgorithm.SlhDsaSha2_256s) return SLHDsaParam.Slh_Dsa_sha2_256s;
//    else if (parameter == SlhDsaAlgorithm.SlhDsaSha2_256f) return SLHDsaParam.Slh_Dsa_sha2_256f;
//    else if (parameter == SlhDsaAlgorithm.SlhDsaShake256s) return SLHDsaParam.Slh_Dsa_shake_256s;
//    else if (parameter == SlhDsaAlgorithm.SlhDsaShake256f) return SLHDsaParam.Slh_Dsa_shake_256f;

//    //else if (parameter == SlhDsaAlgorithm.slh_dsa_sha2_128s_with_sha256)     return SLHDsaParam.Slh_Dsa_sha2_128s_with_sha256;
//    //else if (parameter == SlhDsaAlgorithm.slh_dsa_shake_128s_with_shake128)  return SLHDsaParam.Slh_Dsa_shake_128s_with_shake128;
//    //else if (parameter == SlhDsaAlgorithm.slh_dsa_sha2_128f_with_sha256)     return SLHDsaParam.Slh_Dsa_sha2_128f_with_sha256;
//    //else if (parameter == SlhDsaAlgorithm.slh_dsa_shake_128f_with_shake128)  return SLHDsaParam.Slh_Dsa_shake_128f_with_shake128;
//    //else if (parameter == SlhDsaAlgorithm.slh_dsa_sha2_192s_with_sha512)     return SLHDsaParam.Slh_Dsa_sha2_192s_with_sha512;
//    //else if (parameter == SlhDsaAlgorithm.slh_dsa_shake_192s_with_shake256)  return SLHDsaParam.Slh_Dsa_shake_192s_with_shake256;
//    //else if (parameter == SlhDsaAlgorithm.slh_dsa_sha2_192f_with_sha512)     return SLHDsaParam.Slh_Dsa_sha2_192f_with_sha512;
//    //else if (parameter == SlhDsaAlgorithm.slh_dsa_shake_192f_with_shake256)  return SLHDsaParam.Slh_Dsa_shake_192f_with_shake256;
//    //else if (parameter == SlhDsaAlgorithm.slh_dsa_sha2_256s_with_sha512)     return SLHDsaParam.Slh_Dsa_sha2_256s_with_sha512;
//    //else if (parameter == SlhDsaAlgorithm.slh_dsa_shake_256s_with_shake256)  return SLHDsaParam.Slh_Dsa_shake_256s_with_shake256;
//    //else if (parameter == SlhDsaAlgorithm.slh_dsa_sha2_256f_with_sha512)     return SLHDsaParam.Slh_Dsa_sha2_256f_with_sha512;
//    //else if (parameter == SlhDsaAlgorithm.slh_dsa_shake_256f_with_shake256)  return SLHDsaParam.Slh_Dsa_shake_256f_with_shake256;

//    throw new ArgumentOutOfRangeException(nameof(parameter), $"{nameof(parameter)} has failded!");
//  }

//  public static SlhDsaAlgorithm[] ToSlhDsaAlgorithm()
//  {
//    var a = SlhDsaAlgorithm.SlhDsaSha2_128s;
//    var b = SlhDsaAlgorithm.SlhDsaSha2_128f;
//    var c = SlhDsaAlgorithm.SlhDsaShake128s;
//    var d = SlhDsaAlgorithm.SlhDsaShake128f;

//    var e = SlhDsaAlgorithm.SlhDsaSha2_192s;
//    var f = SlhDsaAlgorithm.SlhDsaSha2_192f;
//    var g = SlhDsaAlgorithm.SlhDsaShake192s;
//    var h = SlhDsaAlgorithm.SlhDsaShake192f;

//    var i = SlhDsaAlgorithm.SlhDsaSha2_256s;
//    var j = SlhDsaAlgorithm.SlhDsaSha2_256f;
//    var k = SlhDsaAlgorithm.SlhDsaShake256s;
//    var l = SlhDsaAlgorithm.SlhDsaShake256f;

//    //var m = SlhDsaAlgorithm.slh_dsa_sha2_128s_with_sha256;
//    //var n = SlhDsaAlgorithm.slh_dsa_shake_128s_with_shake128;
//    //var o = SlhDsaAlgorithm.slh_dsa_sha2_128f_with_sha256;
//    //var p = SlhDsaAlgorithm.slh_dsa_shake_128f_with_shake128;
//    //var q = SlhDsaAlgorithm.slh_dsa_sha2_192s_with_sha512;
//    //var r = SlhDsaAlgorithm.slh_dsa_shake_192s_with_shake256;
//    //var s = SlhDsaAlgorithm.slh_dsa_sha2_192f_with_sha512;
//    //var t = SlhDsaAlgorithm.slh_dsa_shake_192f_with_shake256;
//    //var u = SlhDsaAlgorithm.slh_dsa_sha2_256s_with_sha512;
//    //var v = SlhDsaAlgorithm.slh_dsa_shake_256s_with_shake256;
//    //var w = SlhDsaAlgorithm.slh_dsa_sha2_256f_with_sha512;
//    //var x = SlhDsaAlgorithm.slh_dsa_shake_256f_with_shake256;

//    return [a, b, c, d, e, f, g, h, i, j, k, l, /* m, n, o, p, q, r, s, t, u, v, w, x,*/];
//  }
//}



