using System.Security.Cryptography;

namespace michele.natale.Schnorrs.EcServices;

partial class EcSchnorrServices
{

  public readonly static HashAlgorithmName DEFAULT_H_NAME = HashAlgorithmName.SHA512;

  public static byte[] HashDataAlgo(
    ReadOnlySpan<byte> data, HashAlgorithmName hname)
  {
    if (hname == default) hname = DEFAULT_H_NAME;
    return hname switch
    {
      var obj when obj == HashAlgorithmName.MD5 => MD5.HashData(data),
      var obj when obj == HashAlgorithmName.SHA1 => SHA1.HashData(data),
      var obj when obj == HashAlgorithmName.SHA256 => SHA256.HashData(data),
      var obj when obj == HashAlgorithmName.SHA384 => SHA384.HashData(data),
      var obj when obj == HashAlgorithmName.SHA512 => SHA512.HashData(data),
      var obj when obj == HashAlgorithmName.SHA3_256 => SHA3_256.HashData(data),
      var obj when obj == HashAlgorithmName.SHA3_384 => SHA3_384.HashData(data),
      var obj when obj == HashAlgorithmName.SHA3_512 => SHA3_512.HashData(data),
      _ => throw new ArgumentException($"{nameof(hname)} = {hname} has failed!"),
    };
  }

}