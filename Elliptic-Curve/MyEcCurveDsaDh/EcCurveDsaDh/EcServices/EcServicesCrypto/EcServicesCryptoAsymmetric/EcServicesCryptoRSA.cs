

using System.Security.Cryptography;

namespace michele.natale.EcCurveDsaDh;

partial class EcService
{
  public static byte[] EncryptionRsa(ReadOnlySpan<byte> bytes, ReadOnlySpan<byte> public_key_serialize)
  {
    var rsapi = new RsaParametersInfo(public_key_serialize.ToArray());
    return EncryptionRsa(bytes, rsapi.ToRSAParameters);
  }

  public static byte[] EncryptionRsa(ReadOnlySpan<byte> data, RSAParameters publickey)
  {
    using var rsa = RSA.Create();
    rsa.ImportParameters(publickey);

    var result = rsa.Encrypt(data, RSAEncryptionPadding.OaepSHA256);
    return result;
  }

  public static byte[] DecryptionRsa(ReadOnlySpan<byte> data, ReadOnlySpan<byte> private_key_serialize)
  {
    var rsapi = new RsaParametersInfo(private_key_serialize.ToArray());
    return DecryptionRsa(data, rsapi.ToRSAParameters);
  }

  public static byte[] DecryptionRsa(ReadOnlySpan<byte> data, RSAParameters privatekey)
  {
    using var rsa = RSA.Create();
    rsa.ImportParameters(privatekey);
    return rsa.Decrypt(data, RSAEncryptionPadding.OaepSHA256);
  }


}


