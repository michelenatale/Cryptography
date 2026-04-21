//ML-DSA
//Module-Lattice-Based
//FIPS PUB 204 
//https://nvlpubs.nist.gov/nistpubs/fips/nist.fips.204.pdf


using System.Security.Cryptography;

namespace michele.natale;

using Pointers;

partial class NetServicesCrypto : IMlDsaEx
{
  #region ML-DSA Data Sign / Verify

  #region ML-DSA Data Sign 

  public static byte[] Sign(
    ReadOnlySpan<byte> message,
    ReadOnlySpan<byte> privkey, MLDsaParam mldsa_param)
  {
    using var pk = new UsIPtr<byte>(privkey);
    return Sign(message, pk, ToMLDsaAlgorithm(mldsa_param));
  }

  public static byte[] Sign(
    ReadOnlySpan<byte> message,
    UsIPtr<byte> privkey, MLDsaAlgorithm mldsa_algo)
  {
    AssertSign(message, privkey);

    using var signer = MLDsa.
      ImportMLDsaPrivateKey(mldsa_algo, privkey.ToBytes());

    //Sobald 'SignPreHash' aktiv ist, wird dieses vorgehen
    //wahrscheinlich am sinnvollsten sein.

    //var oid = new Oid(HashAlgorithmName.SHA512.Name!);
    //Span<byte> result = new byte[param.SignatureSizeInBytes];
    //return signer.SignPreHash(hash,oid.Value);

    return signer.SignData(message.ToArray());
  }

  #endregion ML-DSA Data Sign

  #region ML-DSA Data Verify 

  public static bool Verify(
    ReadOnlySpan<byte> message,
    ReadOnlySpan<byte> pubkey,
    ReadOnlySpan<byte> signature,
    MLDsaParam mldsa_param) =>
      Verify(message, pubkey, signature,
        ToMLDsaAlgorithm(mldsa_param));

  public static bool Verify(
    ReadOnlySpan<byte> message,
    ReadOnlySpan<byte> pubkey,
    ReadOnlySpan<byte> signature,
    MLDsaAlgorithm algo)
  {
    AssertVerify(message, pubkey, signature);

    using var verifier = MLDsa.
      ImportMLDsaPublicKey(algo, pubkey);

    return verifier.VerifyData(message, signature);
  }

  #endregion ML-DSA Data Verify

  #endregion ML-DSA Data Sign / Verify

  #region ML-DSA File Sign / Verify

  #region ML-DSA File Sign


  public static async Task<byte[]> SignFileAsync(
     string datapath, UsIPtr<byte> privkey,
     MLDsaAlgorithm algo, CancellationToken ct = default)
  {
    AssertSignFile(datapath, privkey);

    static async Task<byte[]> LoadOrHashAsync(string path, int hashlength = 64, CancellationToken token = default)
    {
      // Local function: Load or hash file
      var length = new FileInfo(path).Length;

      if (length < ML_DSA_MAX_FILE_SIZE_CHANCE)
        // Small file → load completely
        return await File.ReadAllBytesAsync(path, token).ConfigureAwait(false);

      // Large file → Hash
      return await ComputeHashFileAsync(path, HashAlgorithmType.Shake256, hashlength, token);
    }

    var data = await LoadOrHashAsync(datapath, 64, ct).ConfigureAwait(false);
    using var signer = MLDsa.ImportMLDsaPrivateKey(algo, privkey.ToBytes());

    return signer.SignData(data);
  }

  #endregion ML-DSA File Sign

  #region ML-DSA File Verify

  public async static Task<bool> VerifyFileAsync(
    string datapath, ReadOnlyMemory<byte> pubkey,
    ReadOnlyMemory<byte> signature, MLDsaAlgorithm algo,
    CancellationToken ct = default)
  {
    AssertVerifyFile(datapath, pubkey);

    static async Task<byte[]> LoadOrHashAsync(string path, int hashlength = 64, CancellationToken token = default)
    {
      // Local function: Load or hash file
      var length = new FileInfo(path).Length;

      if (length < ML_DSA_MAX_FILE_SIZE_CHANCE) //64 MB
        // Small file → load completely
        return await File.ReadAllBytesAsync(path, token).ConfigureAwait(false);

      // Large file → Hash
      return await ComputeHashFileAsync(path, HashAlgorithmType.Shake256, hashlength, token);
    }

    var data = await LoadOrHashAsync(datapath, 64, ct).ConfigureAwait(false);
    using var verifier = MLDsa.ImportMLDsaPublicKey(algo, pubkey.Span);

    return verifier.VerifyData(data, signature.Span);
  }

  #endregion ML-DSA File Verify

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


  #region  Asserts


  private static void AssertSign(
    ReadOnlySpan<byte> message, UsIPtr<byte> privkey)
  {
    if (message.IsEmpty || message.Length == 0)
      throw new ArgumentNullException(nameof(message));

    if (message.Length < ML_KEM_MIN_PLAIN_SIZE)
      throw new ArgumentOutOfRangeException(nameof(message),
        $"mlkem-message.Length >= {ML_KEM_MIN_PLAIN_SIZE} <= {ML_KEM_MAX_PLAIN_SIZE}");

    if (message.Length > ML_KEM_MAX_PLAIN_SIZE)
      throw new ArgumentOutOfRangeException(nameof(message),
        $"mlkem-message.Length >= {ML_KEM_MIN_PLAIN_SIZE} <= {ML_KEM_MAX_PLAIN_SIZE}");

    if (privkey.IsEmpty || privkey.Length == 0)
      throw new ArgumentNullException(nameof(privkey),
        $"mldsa-{nameof(privkey)} is null or length == 0");
  }

  private static void AssertVerify(
    ReadOnlySpan<byte> message,
    ReadOnlySpan<byte> pubkey,
    ReadOnlySpan<byte> signature)
  {
    if (message.IsEmpty || message.Length == 0)
      throw new ArgumentNullException(nameof(message));

    if (message.Length < ML_KEM_MIN_PLAIN_SIZE)
      throw new ArgumentOutOfRangeException(nameof(message),
        $"mlkem-message.Length >= {ML_KEM_MIN_PLAIN_SIZE} <= {ML_KEM_MAX_PLAIN_SIZE}");

    if (message.Length > ML_KEM_MAX_PLAIN_SIZE)
      throw new ArgumentOutOfRangeException(nameof(message),
        $"mlkem-message.Length >= {ML_KEM_MIN_PLAIN_SIZE} <= {ML_KEM_MAX_PLAIN_SIZE}");

    if (pubkey.IsEmpty || pubkey.Length == 0)
      throw new ArgumentNullException(nameof(pubkey),
        $"mldsa-{nameof(pubkey)} is null or length == 0");

    if (signature.IsEmpty || signature.Length == 0)
      throw new ArgumentNullException(nameof(signature),
        $"mldsa-{nameof(signature)} is null or length == 0");
  }

  private static void AssertSignFile(
    string datapath, UsIPtr<byte> privkey)
  {
    var finfo = new FileInfo(datapath);

    if (!finfo.Exists)
      throw new FileNotFoundException(datapath);

    if (finfo.Length < ML_KEM_MIN_PLAIN_SIZE)
      throw new ArgumentOutOfRangeException(nameof(datapath),
        $"mlkem-message-file.Length min. {ML_KEM_MIN_PLAIN_SIZE}.");

    if (privkey.IsEmpty || privkey.Length == 0)
      throw new ArgumentNullException(nameof(privkey),
        $"mldsa-{nameof(privkey)} is null or length == 0");
  }


  private static void AssertVerifyFile(
    string datapath, ReadOnlyMemory<byte> pubkey)
  {
    var finfo = new FileInfo(datapath);

    if (!finfo.Exists)
      throw new FileNotFoundException(datapath);

    if (finfo.Length < ML_KEM_MIN_PLAIN_SIZE)
      throw new ArgumentOutOfRangeException(nameof(datapath),
        $"mlkem-message-file.Length min. {ML_KEM_MIN_PLAIN_SIZE}.");

    if (pubkey.IsEmpty || pubkey.Length == 0)
      throw new ArgumentNullException(nameof(pubkey),
        $"mldsa-{nameof(pubkey)} is null or length == 0");
  }
  #endregion  Asserts
}
