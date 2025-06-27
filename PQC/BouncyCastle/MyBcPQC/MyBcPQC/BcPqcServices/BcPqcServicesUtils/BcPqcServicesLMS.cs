
using Org.BouncyCastle.Pqc.Crypto.Lms;
using Org.BouncyCastle.Security;

namespace michele.natale.Services;

using System;
using System.Text.Json.Serialization;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum LmsParam : byte
{
  lms_sha256_h5_w1 = 0,
  lms_sha256_h5_w2,
  lms_sha256_h5_w4,
  lms_sha256_h5_w8,
  lms_sha256_h10_w1,
  lms_sha256_h10_w2,
  lms_sha256_h10_w4,
  lms_sha256_h10_w8,
  lms_sha256_h15_w1,
  lms_sha256_h15_w2,
  lms_sha256_h15_w4,
  lms_sha256_h15_w8,
  lms_sha256_h20_w1,
  lms_sha256_h20_w2,
  lms_sha256_h20_w4,
  lms_sha256_h20_w8,
  lms_sha256_h25_w1,
  lms_sha256_h25_w2,
  lms_sha256_h25_w4,
  lms_sha256_h25_w8,
  //lms_sha256_h5_w8_h5_w8,
  //lms_sha256_h10_w4_h5_w8,
  //lms_sha256_h10_w8_h5_w8,
  //lms_sha256_h10_w2_h10_w2,
  //lms_sha256_h10_w4_h10_w4,
  //lms_sha256_h10_w8_h10_w8,
  //lms_sha256_h15_w8_h5_w8,
  //lms_sha256_h15_w8_h10_w8,
  //lms_sha256_h15_w8_h15_w8,
  //lms_sha256_h20_w8_h5_w8,
  //lms_sha256_h20_w8_h10_w8,
  //lms_sha256_h20_w8_h15_w8,
  //lms_sha256_h20_w8_h20_w8,
}

partial class BcPqcServices
{
  public static LmsKeyGenerationParameters ToLmsKeyGenerationParameter(LmsParam parameters, SecureRandom rand)
  {
    return parameters switch
    {
      LmsParam.lms_sha256_h5_w1 => new LmsKeyGenerationParameters(new LmsParameters(LMSigParameters.lms_sha256_n32_h5, LMOtsParameters.sha256_n32_w1), rand),
      LmsParam.lms_sha256_h5_w2 => new LmsKeyGenerationParameters(new LmsParameters(LMSigParameters.lms_sha256_n32_h5, LMOtsParameters.sha256_n32_w2), rand),
      LmsParam.lms_sha256_h5_w4 => new LmsKeyGenerationParameters(new LmsParameters(LMSigParameters.lms_sha256_n32_h5, LMOtsParameters.sha256_n32_w4), rand),
      LmsParam.lms_sha256_h5_w8 => new LmsKeyGenerationParameters(new LmsParameters(LMSigParameters.lms_sha256_n32_h5, LMOtsParameters.sha256_n32_w8), rand),
      LmsParam.lms_sha256_h10_w1 => new LmsKeyGenerationParameters(new LmsParameters(LMSigParameters.lms_sha256_n32_h10, LMOtsParameters.sha256_n32_w1), rand),
      LmsParam.lms_sha256_h10_w2 => new LmsKeyGenerationParameters(new LmsParameters(LMSigParameters.lms_sha256_n32_h10, LMOtsParameters.sha256_n32_w2), rand),
      LmsParam.lms_sha256_h10_w4 => new LmsKeyGenerationParameters(new LmsParameters(LMSigParameters.lms_sha256_n32_h10, LMOtsParameters.sha256_n32_w4), rand),
      LmsParam.lms_sha256_h10_w8 => new LmsKeyGenerationParameters(new LmsParameters(LMSigParameters.lms_sha256_n32_h10, LMOtsParameters.sha256_n32_w8), rand),
      LmsParam.lms_sha256_h15_w1 => new LmsKeyGenerationParameters(new LmsParameters(LMSigParameters.lms_sha256_n32_h15, LMOtsParameters.sha256_n32_w1), rand),
      LmsParam.lms_sha256_h15_w2 => new LmsKeyGenerationParameters(new LmsParameters(LMSigParameters.lms_sha256_n32_h15, LMOtsParameters.sha256_n32_w2), rand),
      LmsParam.lms_sha256_h15_w4 => new LmsKeyGenerationParameters(new LmsParameters(LMSigParameters.lms_sha256_n32_h15, LMOtsParameters.sha256_n32_w4), rand),
      LmsParam.lms_sha256_h15_w8 => new LmsKeyGenerationParameters(new LmsParameters(LMSigParameters.lms_sha256_n32_h15, LMOtsParameters.sha256_n32_w8), rand),
      LmsParam.lms_sha256_h20_w1 => new LmsKeyGenerationParameters(new LmsParameters(LMSigParameters.lms_sha256_n32_h20, LMOtsParameters.sha256_n32_w1), rand),
      LmsParam.lms_sha256_h20_w2 => new LmsKeyGenerationParameters(new LmsParameters(LMSigParameters.lms_sha256_n32_h20, LMOtsParameters.sha256_n32_w2), rand),
      LmsParam.lms_sha256_h20_w4 => new LmsKeyGenerationParameters(new LmsParameters(LMSigParameters.lms_sha256_n32_h20, LMOtsParameters.sha256_n32_w4), rand),
      LmsParam.lms_sha256_h20_w8 => new LmsKeyGenerationParameters(new LmsParameters(LMSigParameters.lms_sha256_n32_h20, LMOtsParameters.sha256_n32_w8), rand),
      LmsParam.lms_sha256_h25_w1 => new LmsKeyGenerationParameters(new LmsParameters(LMSigParameters.lms_sha256_n32_h25, LMOtsParameters.sha256_n32_w1), rand),
      LmsParam.lms_sha256_h25_w2 => new LmsKeyGenerationParameters(new LmsParameters(LMSigParameters.lms_sha256_n32_h25, LMOtsParameters.sha256_n32_w2), rand),
      LmsParam.lms_sha256_h25_w4 => new LmsKeyGenerationParameters(new LmsParameters(LMSigParameters.lms_sha256_n32_h25, LMOtsParameters.sha256_n32_w4), rand),
      LmsParam.lms_sha256_h25_w8 => new LmsKeyGenerationParameters(new LmsParameters(LMSigParameters.lms_sha256_n32_h25, LMOtsParameters.sha256_n32_w8), rand),
      //lms_sha256_h5_w8_h5_w8,
      //lms_sha256_h10_w4_h5_w8,
      //lms_sha256_h10_w8_h5_w8,
      //lms_sha256_h10_w2_h10_w2,
      //lms_sha256_h10_w4_h10_w4,
      //lms_sha256_h10_w8_h10_w8,
      //lms_sha256_h15_w8_h5_w8,
      //lms_sha256_h15_w8_h10_w8,
      //lms_sha256_h15_w8_h15_w8,
      //lms_sha256_h20_w8_h5_w8,
      //lms_sha256_h20_w8_h10_w8,
      //lms_sha256_h20_w8_h15_w8,
      //lms_sha256_h20_w8_h20_w8,
      _ => throw new ArgumentOutOfRangeException(
            nameof(parameters), $"{nameof(ToLmsKeyGenerationParameters)};{nameof(parameters)} has failed!")
    };
  }

  public static LmsParam FromLmsKeyGenerationParameters(LmsKeyGenerationParameters parameter)
  {
    ArgumentNullException.ThrowIfNull(parameter);

    var lmsp = Enum.GetValues<LmsParam>();
    var parameters = ToLmsKeyGenerationParameters();

    for (var i = 0; i < parameters.Length; i++)
      if (parameters[i] == parameter) return lmsp[i];

    throw new ArgumentOutOfRangeException(nameof(parameter), $"{nameof(parameter)} has failded!");
  }

  public static LmsParam[] ToLmsParam()
  {
    var a = LmsParam.lms_sha256_h5_w1;
    var b = LmsParam.lms_sha256_h5_w2;
    var c = LmsParam.lms_sha256_h5_w4;
    var d = LmsParam.lms_sha256_h5_w8;
    var e = LmsParam.lms_sha256_h10_w1;
    var f = LmsParam.lms_sha256_h10_w2;
    var g = LmsParam.lms_sha256_h10_w4;
    var h = LmsParam.lms_sha256_h10_w8;
    var i = LmsParam.lms_sha256_h15_w1;
    var j = LmsParam.lms_sha256_h15_w2;
    var k = LmsParam.lms_sha256_h15_w4;
    var l = LmsParam.lms_sha256_h15_w8;
    var m = LmsParam.lms_sha256_h20_w1;
    var n = LmsParam.lms_sha256_h20_w2;
    var o = LmsParam.lms_sha256_h20_w4;
    var p = LmsParam.lms_sha256_h20_w8;
    var q = LmsParam.lms_sha256_h25_w1;
    var r = LmsParam.lms_sha256_h25_w2;
    var s = LmsParam.lms_sha256_h25_w4;
    var t = LmsParam.lms_sha256_h25_w8;
    //var u = LmsParam.lms_sha256_h5_w8_h5_w8;
    //var v = LmsParam.lms_sha256_h10_w4_h5_w8;
    //var w = LmsParam.lms_sha256_h10_w8_h5_w8;
    //var x = LmsParam.lms_sha256_h10_w2_h10_w2;
    //var y = LmsParam.lms_sha256_h10_w4_h10_w4;
    //var z = LmsParam.lms_sha256_h10_w8_h10_w8;
    //var aa = LmsParam.lms_sha256_h15_w8_h5_w8;
    //var ab = LmsParam.lms_sha256_h15_w8_h10_w;
    //var ac = LmsParam.lms_sha256_h15_w8_h15_w;
    //var ad = LmsParam.lms_sha256_h20_w8_h5_w8;
    //var ae = LmsParam.lms_sha256_h20_w8_h10_w;
    //var af = LmsParam.lms_sha256_h20_w8_h15_w;
    //var ag = LmsParam.lms_sha256_h20_w8_h20_w;
    return [a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p, q, r, s, t, /*u, v, w, x, y, z, aa, ab, ac, ad, ae, af, ag*/];
  }

  public static LmsKeyGenerationParameters[] ToLmsKeyGenerationParameters()
  {
    var lmsp = ToLmsParam();
    return [.. lmsp.Select(x => ToLmsKeyGenerationParameter(x, null!))];
  }
}



//lms_sha256_h5_w1	 	 	   
//lms_sha256_h5_w2	 	 	   
//lms_sha256_h5_w4	 	 	   
//lms_sha256_h5_w8	 	 	   
//lms_sha256_h10_w1	 	 	   
//lms_sha256_h10_w2	 	 	   
//lms_sha256_h10_w4	 	 	   
//lms_sha256_h10_w8	 	 	   
//lms_sha256_h15_w1	 	 	   
//lms_sha256_h15_w2	 	 	   
//lms_sha256_h15_w4	 	 	   
//lms_sha256_h15_w8	 	 	   
//lms_sha256_h20_w1	 	 	   
//lms_sha256_h20_w2	 	 	   
//lms_sha256_h20_w4	 	 	   
//lms_sha256_h20_w8	 	 	   
//lms_sha256_h25_w1	 	 	   
//lms_sha256_h25_w2	 	 	   
//lms_sha256_h25_w4	 	 	   
//lms_sha256_h25_w8	 	 	   
//lms_sha256_h5_w8_h5_w8	 
//lms_sha256_h10_w4_h5_w8	 
//lms_sha256_h10_w8_h5_w8	 
//lms_sha256_h10_w2_h10_w2
//lms_sha256_h10_w4_h10_w4
//lms_sha256_h10_w8_h10_w8
//lms_sha256_h15_w8_h5_w8	 
//lms_sha256_h15_w8_h10_w8
//lms_sha256_h15_w8_h15_w8
//lms_sha256_h20_w8_h5_w8	 
//lms_sha256_h20_w8_h10_w8
//lms_sha256_h20_w8_h15_w8
//lms_sha256_h20_w8_h20_w8