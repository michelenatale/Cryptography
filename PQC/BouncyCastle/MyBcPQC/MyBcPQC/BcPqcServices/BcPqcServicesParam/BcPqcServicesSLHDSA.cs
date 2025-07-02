
using Org.BouncyCastle.Crypto.Parameters;

namespace michele.natale.Services;

using BcPqcs;

partial class BcPqcServices
{

  public static SlhDsaParameters ToSLHDsaParameters(SLHDsaParam param) => param switch
  {
    SLHDsaParam.Slh_Dsa_sha2_128s => SlhDsaParameters.slh_dsa_sha2_128s,
    SLHDsaParam.Slh_Dsa_sha2_128f => SlhDsaParameters.slh_dsa_sha2_128f,
    SLHDsaParam.Slh_Dsa_shake_128s => SlhDsaParameters.slh_dsa_shake_128s,
    SLHDsaParam.Slh_Dsa_shake_128f => SlhDsaParameters.slh_dsa_shake_128f,

    SLHDsaParam.Slh_Dsa_sha2_192s => SlhDsaParameters.slh_dsa_sha2_192s,
    SLHDsaParam.Slh_Dsa_sha2_192f => SlhDsaParameters.slh_dsa_sha2_192f,
    SLHDsaParam.Slh_Dsa_shake_192s => SlhDsaParameters.slh_dsa_shake_192s,
    SLHDsaParam.Slh_Dsa_shake_192f => SlhDsaParameters.slh_dsa_shake_192f,

    SLHDsaParam.Slh_Dsa_sha2_256s => SlhDsaParameters.slh_dsa_sha2_256s,
    SLHDsaParam.Slh_Dsa_sha2_256f => SlhDsaParameters.slh_dsa_sha2_256f,
    SLHDsaParam.Slh_Dsa_shake_256s => SlhDsaParameters.slh_dsa_shake_256s,
    SLHDsaParam.Slh_Dsa_shake_256f => SlhDsaParameters.slh_dsa_shake_256f,

    //SLHDsaParam.Slh_Dsa_sha2_128s_with_sha256     => SlhDsaParameters.slh_dsa_sha2_128s_with_sha256, 
    //SLHDsaParam.Slh_Dsa_shake_128s_with_shake128  =>SlhDsaParameters.slh_dsa_shake_128s_with_shake128,
    //SLHDsaParam.Slh_Dsa_sha2_128f_with_sha256     =>SlhDsaParameters.slh_dsa_sha2_128f_with_sha256,
    //SLHDsaParam.Slh_Dsa_shake_128f_with_shake128  =>SlhDsaParameters.slh_dsa_shake_128f_with_shake128,
    //SLHDsaParam.Slh_Dsa_sha2_192s_with_sha512     =>SlhDsaParameters.slh_dsa_sha2_192s_with_sha512,   
    //SLHDsaParam.Slh_Dsa_shake_192s_with_shake256  =>SlhDsaParameters.slh_dsa_shake_192s_with_shake256,
    //SLHDsaParam.Slh_Dsa_sha2_192f_with_sha512     =>SlhDsaParameters.slh_dsa_sha2_192f_with_sha512,
    //SLHDsaParam.Slh_Dsa_shake_192f_with_shake256  =>SlhDsaParameters.slh_dsa_shake_192f_with_shake256,
    //SLHDsaParam.Slh_Dsa_sha2_256s_with_sha512     =>SlhDsaParameters.slh_dsa_sha2_256s_with_sha512,   
    //SLHDsaParam.Slh_Dsa_shake_256s_with_shake256  =>SlhDsaParameters.slh_dsa_shake_256s_with_shake256,
    //SLHDsaParam.Slh_Dsa_sha2_256f_with_sha512     =>SlhDsaParameters.slh_dsa_sha2_256f_with_sha512,   
    //SLHDsaParam.Slh_Dsa_shake_256f_with_shake256  =>SlhDsaParameters.slh_dsa_shake_256f_with_shake256,
    _ => throw new Exception(),
  };

  public static SLHDsaParam FromSLHDsaParameters(SlhDsaParameters parameter)
  {
    ArgumentNullException.ThrowIfNull(parameter);

    if (parameter == SlhDsaParameters.slh_dsa_sha2_128s) return SLHDsaParam.Slh_Dsa_sha2_128s;
    else if (parameter == SlhDsaParameters.slh_dsa_sha2_128f) return SLHDsaParam.Slh_Dsa_sha2_128f;
    else if (parameter == SlhDsaParameters.slh_dsa_shake_128s) return SLHDsaParam.Slh_Dsa_shake_128s;
    else if (parameter == SlhDsaParameters.slh_dsa_shake_128f) return SLHDsaParam.Slh_Dsa_shake_128f;

    else if (parameter == SlhDsaParameters.slh_dsa_sha2_192s) return SLHDsaParam.Slh_Dsa_sha2_192s;
    else if (parameter == SlhDsaParameters.slh_dsa_sha2_192f) return SLHDsaParam.Slh_Dsa_sha2_192f;
    else if (parameter == SlhDsaParameters.slh_dsa_shake_192s) return SLHDsaParam.Slh_Dsa_shake_192s;
    else if (parameter == SlhDsaParameters.slh_dsa_shake_192f) return SLHDsaParam.Slh_Dsa_shake_192f;

    else if (parameter == SlhDsaParameters.slh_dsa_sha2_256s) return SLHDsaParam.Slh_Dsa_sha2_256s;
    else if (parameter == SlhDsaParameters.slh_dsa_sha2_256f) return SLHDsaParam.Slh_Dsa_sha2_256f;
    else if (parameter == SlhDsaParameters.slh_dsa_shake_256s) return SLHDsaParam.Slh_Dsa_shake_256s;
    else if (parameter == SlhDsaParameters.slh_dsa_shake_256f) return SLHDsaParam.Slh_Dsa_shake_256f;

    //else if (parameter == SlhDsaParameters.slh_dsa_sha2_128s_with_sha256)     return SLHDsaParam.Slh_Dsa_sha2_128s_with_sha256;
    //else if (parameter == SlhDsaParameters.slh_dsa_shake_128s_with_shake128)  return SLHDsaParam.Slh_Dsa_shake_128s_with_shake128;
    //else if (parameter == SlhDsaParameters.slh_dsa_sha2_128f_with_sha256)     return SLHDsaParam.Slh_Dsa_sha2_128f_with_sha256;
    //else if (parameter == SlhDsaParameters.slh_dsa_shake_128f_with_shake128)  return SLHDsaParam.Slh_Dsa_shake_128f_with_shake128;
    //else if (parameter == SlhDsaParameters.slh_dsa_sha2_192s_with_sha512)     return SLHDsaParam.Slh_Dsa_sha2_192s_with_sha512;
    //else if (parameter == SlhDsaParameters.slh_dsa_shake_192s_with_shake256)  return SLHDsaParam.Slh_Dsa_shake_192s_with_shake256;
    //else if (parameter == SlhDsaParameters.slh_dsa_sha2_192f_with_sha512)     return SLHDsaParam.Slh_Dsa_sha2_192f_with_sha512;
    //else if (parameter == SlhDsaParameters.slh_dsa_shake_192f_with_shake256)  return SLHDsaParam.Slh_Dsa_shake_192f_with_shake256;
    //else if (parameter == SlhDsaParameters.slh_dsa_sha2_256s_with_sha512)     return SLHDsaParam.Slh_Dsa_sha2_256s_with_sha512;
    //else if (parameter == SlhDsaParameters.slh_dsa_shake_256s_with_shake256)  return SLHDsaParam.Slh_Dsa_shake_256s_with_shake256;
    //else if (parameter == SlhDsaParameters.slh_dsa_sha2_256f_with_sha512)     return SLHDsaParam.Slh_Dsa_sha2_256f_with_sha512;
    //else if (parameter == SlhDsaParameters.slh_dsa_shake_256f_with_shake256)  return SLHDsaParam.Slh_Dsa_shake_256f_with_shake256;

    throw new ArgumentOutOfRangeException(nameof(parameter), $"{nameof(parameter)} has failded!");
  }

  public static SlhDsaParameters[] ToSLHDsaParameters()
  {
    var a = SlhDsaParameters.slh_dsa_sha2_128s;
    var b = SlhDsaParameters.slh_dsa_sha2_128f;
    var c = SlhDsaParameters.slh_dsa_shake_128s;
    var d = SlhDsaParameters.slh_dsa_shake_128f;

    var e = SlhDsaParameters.slh_dsa_sha2_192s;
    var f = SlhDsaParameters.slh_dsa_sha2_192f;
    var g = SlhDsaParameters.slh_dsa_shake_192s;
    var h = SlhDsaParameters.slh_dsa_shake_192f;

    var i = SlhDsaParameters.slh_dsa_sha2_256s;
    var j = SlhDsaParameters.slh_dsa_sha2_256f;
    var k = SlhDsaParameters.slh_dsa_shake_256s;
    var l = SlhDsaParameters.slh_dsa_shake_256f;

    //var m = SlhDsaParameters.slh_dsa_sha2_128s_with_sha256;
    //var n = SlhDsaParameters.slh_dsa_shake_128s_with_shake128;
    //var o = SlhDsaParameters.slh_dsa_sha2_128f_with_sha256;
    //var p = SlhDsaParameters.slh_dsa_shake_128f_with_shake128;
    //var q = SlhDsaParameters.slh_dsa_sha2_192s_with_sha512;
    //var r = SlhDsaParameters.slh_dsa_shake_192s_with_shake256;
    //var s = SlhDsaParameters.slh_dsa_sha2_192f_with_sha512;
    //var t = SlhDsaParameters.slh_dsa_shake_192f_with_shake256;
    //var u = SlhDsaParameters.slh_dsa_sha2_256s_with_sha512;
    //var v = SlhDsaParameters.slh_dsa_shake_256s_with_shake256;
    //var w = SlhDsaParameters.slh_dsa_sha2_256f_with_sha512;
    //var x = SlhDsaParameters.slh_dsa_shake_256f_with_shake256;

    return [a, b, c, d, e, f, g, h, i, j, k, l, /* m, n, o, p, q, r, s, t, u, v, w, x,*/];
  }
}



