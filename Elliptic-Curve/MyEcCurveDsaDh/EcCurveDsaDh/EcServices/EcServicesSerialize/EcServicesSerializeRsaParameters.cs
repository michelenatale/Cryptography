

using System.Security.Cryptography;

namespace michele.natale.EcCurveDsaDh;

[Serializable]
public class RsaParametersInfo
{
  public RSAParameters ToRSAParameters
  {
    get
    {
      return this.ToRsaParam();
    }
  }

  public byte[] D { get; set; } = [];
  public byte[] DP { get; set; } = [];
  public byte[] DQ { get; set; } = [];
  public byte[] EXP { get; set; } = [];
  public byte[] IQ { get; set; } = [];
  public byte[] MOD { get; set; } = [];
  public byte[] P { get; set; } = [];
  public byte[] Q { get; set; } = [];

  public RsaParametersInfo(byte[] rsa_serialize)
  {
    var tmp = EcService.DeserializeJson<byte[][]>(rsa_serialize);
    this.D = tmp![0]; this.DP = tmp[1]; this.DQ = tmp[2];
    this.EXP = tmp[3]; this.IQ = tmp[4];
    this.MOD = tmp[5]; this.P = tmp[6]; this.Q = tmp[7];
  }

  public RsaParametersInfo(
    byte[] d, byte[] dp, byte[] dq, byte[] exp,
    byte[] iq, byte[] mod, byte[] p, byte[] q
  )
  {
    this.D = d is null ? [] : d;
    this.DP = dp is null ? [] : dp;
    this.DQ = dq is null ? [] : dq;
    this.EXP = exp is null ? [] : exp;
    this.IQ = iq is null ? [] : iq;
    this.MOD = mod is null ? [] : mod;
    this.P = p is null ? [] : p;
    this.Q = q is null ? [] : q;
  }

  public RsaParametersInfo(RSAParameters rsaparam)
  {
    this.D = rsaparam.D is null ? [] : rsaparam.D!;
    this.DP = rsaparam.DP is null ? [] : rsaparam.DP!;
    this.DQ = rsaparam.DQ is null ? [] : rsaparam.DQ!;
    this.EXP = rsaparam.Exponent is null ? [] : rsaparam.Exponent!;
    this.IQ = rsaparam.InverseQ is null ? [] : rsaparam.InverseQ!;
    this.MOD = rsaparam.Modulus is null ? [] : rsaparam.Modulus!;
    this.P = rsaparam.P is null ? [] : rsaparam.P!;
    this.Q = rsaparam.Q is null ? [] : rsaparam.Q!;
  }

  private byte[] ToSerializeJson()
  {
    var serializer = new byte[][]
    {
      this.D ,this.DP ,this.DQ ,
      this.EXP , this.IQ ,
      this.MOD ,this.P ,this.Q ,
    };
    return EcService.SerializeJson(serializer);
  }

  private RSAParameters ToRsaParam()
  {
    return new RSAParameters()
    {
      D = this.D,
      DP = this.DP,
      DQ = this.DQ,
      Exponent = this.EXP,
      InverseQ = this.IQ,
      Modulus = this.MOD,
      P = this.P,
      Q = this.Q,
    };
  }


  public static byte[] SerializeRsaParam(RSAParameters rsaparam)
  {
    var instance = new RsaParametersInfo(rsaparam);
    return instance.ToSerializeJson();
  }

  public static RSAParameters DeserializeRsaParam(byte[] rsa_serialize)
  {
    var instance = new RsaParametersInfo(rsa_serialize);
    return instance.ToRsaParam();
  }


}
