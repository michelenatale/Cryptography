//ML-DSA
//Module-Lattice-Based
//FIPS PUB 204 
//https://nvlpubs.nist.gov/nistpubs/fips/nist.fips.204.pdf


using System.Security.Cryptography;


namespace michele.natale.MsPqcs;


using Pointers; 


/// <summary>
/// Provides signature methods around the ML-DSA algorithm.
/// </summary>
public sealed class MlDsaEx : IMlDsaEx
{

  #region ML-DSA Data Sign / Verify

  #region ML-DSA Data Sign 

  public static byte[] Sign(
    MlDsaKeyPairInfo info,
    ReadOnlySpan<byte> message)
  {
    if (message.IsEmpty || message.Length == 0)
      throw new ArgumentNullException(nameof(message));

    using var privkey = info.ToPrivKey();
    var hash = SHA512.HashData(message);

    using var signer = MLDsa.ImportMLDsaPrivateKey(
      info.ToAlgo(), privkey.ToBytes());

    return signer.SignData(hash);
  }

  public static byte[] Sign(
    MLDsa mldsa, ReadOnlySpan<byte> message)
  {
    if (message.IsEmpty || message.Length == 0)
      throw new ArgumentNullException(nameof(message));

    var hash = SHA512.HashData(message);
    using var signer = MLDsa.ImportMLDsaPrivateKey(
      mldsa.Algorithm, mldsa.ExportMLDsaPrivateKey());

    return signer.SignData(hash);
  }

  public static byte[] Sign(
    MLDsaAlgorithm algo, UsIPtr<byte> privkey,
    ReadOnlySpan<byte> message)
  {
    if (message.IsEmpty || message.Length == 0)
      throw new ArgumentNullException(nameof(message));

    var hash = SHA512.HashData(message);
    using var signer = MLDsa.ImportMLDsaPrivateKey(
      algo, privkey.ToBytes());

    return signer.SignData(hash);
  }

  #endregion ML-DSA Data Sign

  #region ML-DSA Data Verify 

  public static bool Verify(
    MlDsaKeyPairInfo info,
    ReadOnlySpan<byte> signature,
    ReadOnlySpan<byte> message)
  {
    if (message.IsEmpty || message.Length == 0)
      throw new ArgumentNullException(nameof(message));

    if (signature.IsEmpty || signature.Length == 0)
      throw new ArgumentNullException(nameof(signature));

    var hash = SHA512.HashData(message);

    using var verifier = MLDsa.ImportMLDsaPublicKey(
      info.ToAlgo(), info.PubKey);

    return verifier.VerifyData(hash, signature);
  }

  public static bool Verify(
    MLDsa mldsa, ReadOnlySpan<byte> signature,
    ReadOnlySpan<byte> message)
  {
    if (message.IsEmpty || message.Length == 0)
      throw new ArgumentNullException(nameof(message));

    if (signature.IsEmpty || signature.Length == 0)
      throw new ArgumentNullException(nameof(signature));

    var hash = SHA512.HashData(message);

    using var verifier = MLDsa.ImportMLDsaPublicKey(
      mldsa.Algorithm, mldsa.ExportMLDsaPublicKey());

    return verifier.VerifyData(hash, signature);
  }

  public static bool Verify(
    MLDsaAlgorithm algo,
    ReadOnlySpan<byte> pubkey,
    ReadOnlySpan<byte> signature,
    ReadOnlySpan<byte> message)
  {
    if (message.IsEmpty || message.Length == 0)
      throw new ArgumentNullException(nameof(message));

    if (signature.IsEmpty || signature.Length == 0)
      throw new ArgumentNullException(nameof(signature));

    var hash = SHA512.HashData(message);

    using var verifier = MLDsa.ImportMLDsaPublicKey(
      algo, pubkey);

    return verifier.VerifyData(hash, signature);
  }

  #endregion ML-DSA Data Verify

  #endregion ML-DSA Data Sign / Verify

  #region ML-DSA File Sign / Verify

  #region ML-DSA File Sign

  public async static Task<byte[]> SignSha512Async(
    MlDsaKeyPairInfo info, string datapath)
  {
    if (!File.Exists(datapath))
      throw new FileNotFoundException(datapath);

    using var sha = SHA512.Create();
    using var privkey = info.ToPrivKey();
    var hash = await ComputeHashAsync(datapath, sha);

    using var signer = MLDsa.ImportMLDsaPrivateKey(
      info.ToAlgo(), privkey.ToBytes());

    return signer.SignData(hash);
  }

  public async static Task<byte[]> SignSha512Async(
    MLDsa mldsa, UsIPtr<byte> privkey, string datapath)
  {
    if (!File.Exists(datapath))
      throw new FileNotFoundException(datapath);

    using var sha = SHA512.Create();
    var hash = await ComputeHashAsync(datapath, sha);

    using var signer = MLDsa.ImportMLDsaPrivateKey(
     mldsa.Algorithm, privkey.ToBytes());

    return signer.SignData(hash);
  }

  public async static Task<byte[]> SignSha512Async(
    MLDsaAlgorithm algo, UsIPtr<byte> privkey, string datapath)
  {
    if (!File.Exists(datapath))
      throw new FileNotFoundException(datapath);

    using var sha = SHA512.Create();
    var hash = await ComputeHashAsync(datapath, sha);

    using var signer = MLDsa.ImportMLDsaPrivateKey(
      algo, privkey.ToBytes());

    return signer.SignData(hash);
  }


  #endregion ML-DSA File Sign

  #region ML-DSA File Verify

  public async static Task<bool> VerifySha512Async(
    MlDsaKeyPairInfo info, ReadOnlyMemory<byte> signature,
    string datapath)
  {
    if (!File.Exists(datapath))
      throw new FileNotFoundException();

    using var sha = SHA512.Create();
    var hash = await ComputeHashAsync(datapath, sha);

    using var verifier = MLDsa.ImportMLDsaPublicKey(
      info.ToAlgo(), info.PubKey);

    return verifier.VerifyData(hash, signature.Span);
  }

  public async static Task<bool> VerifySha512Async(
    MLDsa mldsa, ReadOnlyMemory<byte> signature, string datapath)
  {
    if (!File.Exists(datapath))
      throw new FileNotFoundException();

    var pubkey = mldsa.ExportMLDsaPublicKey();

    using var sha = SHA512.Create();
    var hash = await ComputeHashAsync(datapath, sha);

    using var verifier = MLDsa.ImportMLDsaPublicKey(
      mldsa.Algorithm, pubkey);

    return verifier.VerifyData(hash, signature.Span);
  }

  public async static Task<bool> VerifySha512Async(
    MLDsaAlgorithm algo,ReadOnlyMemory<byte> pubkey,
    ReadOnlyMemory<byte> signature,string datapath)
  {

    if (!File.Exists(datapath))
      throw new FileNotFoundException();

    using var sha = SHA512.Create();
    var hash = await ComputeHashAsync(datapath, sha);

    using var verifier = MLDsa.ImportMLDsaPublicKey(
      algo, pubkey.Span);

    return verifier.VerifyData(hash, signature.Span);
  }

  #endregion ML-DSA File Verify

  #region ML-DSA File Hash
  private static async Task<byte[]> ComputeHashAsync(
    string path, HashAlgorithm hash)
  {
    await using var fs = new FileStream(
        path,
        FileMode.Open,
        FileAccess.Read,
        FileShare.Read,
        bufferSize: 81920,
        useAsync: true);

    return await hash.ComputeHashAsync(fs);
  }
  #endregion  ML-DSA File Hash

  #endregion ML-DSA File Sign / Verify


  #region ML-DSA KeyPair Generate 

  public static (UsIPtr<byte> PrivKey, byte[] PubKey) ToKeyPair(MLDsa mldsa)
  {
    var pub = mldsa.ExportMLDsaPublicKey();
    var priv = mldsa.ExportMLDsaPrivateKey();

    //Man könnte hier ganz sicher noch eine Verkryptung einbauen.

    var result = (new UsIPtr<byte>(priv), pub);
    Array.Clear(priv);
    return result;
  }

  private static MLDsa ToMLDsaPub(MlDsaKeyPairInfo info) =>
    ToMLDsaPub(info.ToAlgo(), info.PubKey);

  private static MLDsa ToMLDsaPriv(MlDsaKeyPairInfo info)
  {
    using var privkey = info.ToPrivKey();
    return ToMLDsaPriv(info.ToAlgo(), privkey);
  }

  private static MLDsa ToMLDsaPub(
    MLDsaAlgorithm algo, ReadOnlySpan<byte> pubkey) =>
      MLDsa.ImportMLDsaPublicKey(algo, pubkey);

  private static MLDsa ToMLDsaPriv(
    MLDsaAlgorithm algo, UsIPtr<byte> privkey) =>
      MLDsa.ImportMLDsaPrivateKey(algo, privkey.ToBytes());

  #endregion ML-DSA KeyPair Generate

  #region Utils

  public static MLDsaAlgorithm ToMLDsaParameter(int idx)
  {
    var a = MLDsaAlgorithm.MLDsa44;
    var b = MLDsaAlgorithm.MLDsa65;
    var c = MLDsaAlgorithm.MLDsa87;
    MLDsaAlgorithm[] result = [a, b, c,];

    return result[idx];
  }

  public static int ToIndex(MLDsaAlgorithm parameter)
  {
    var a = MLDsaAlgorithm.MLDsa44;
    var b = MLDsaAlgorithm.MLDsa65;
    var c = MLDsaAlgorithm.MLDsa87;
    MLDsaAlgorithm[] parameters = [a, b, c,];

    var result = Array.IndexOf(parameters, parameter);
    if (result >= 0) return result;

    throw new KeyNotFoundException(nameof(parameter));
  }

  #endregion  Utils

}
