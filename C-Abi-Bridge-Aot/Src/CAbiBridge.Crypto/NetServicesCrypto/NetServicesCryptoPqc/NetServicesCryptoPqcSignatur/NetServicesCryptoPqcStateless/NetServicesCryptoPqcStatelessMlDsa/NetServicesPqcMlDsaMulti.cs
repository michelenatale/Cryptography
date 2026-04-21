

using michele.natale.Pointers;
using System.Security.Cryptography;

namespace michele.natale;

partial class NetServicesCrypto
{
  public static (byte[] Sign, UsIPtr<byte> PrivKey, byte[] PubKey, MLDsaAlgorithm Param) MultiSignatur(
    ReadOnlySpan<byte> message, ReadOnlySpan<byte[]> guids,
    ReadOnlySpan<byte[]> signs, ReadOnlySpan<byte[]> publickeys,
    ReadOnlySpan<byte[]> signersnames, ReadOnlySpan<byte[]> projectnames,
    ReadOnlySpan<byte> sign_algo, ReadOnlySpan<byte> mldsa_param)
  {
    AssertPqcMlDsaMultiSignAot(message, guids, signs, publickeys,
      signersnames, projectnames, sign_algo, mldsa_param);

    CheckVerifies(message, signs, publickeys, mldsa_param);

    //Now Create the Multi-Sign-Verify
    var kpi = CreateMlDsaKeyPair();
    var seed = ToMultHashSeed(message, guids, signs, publickeys,
      signersnames, projectnames, sign_algo, mldsa_param);

    var mldsa_algo = Enum.GetValues<MLDsaParam>();
    var idx = RandomNumberGenerator.GetInt32(mldsa_algo.Length);
    var param = ToMLDsaAlgorithm(mldsa_algo[idx]);

    using var mldsa = MLDsa.ImportMLDsaPrivateSeed(param, seed);

    var pubkey = mldsa.ExportMLDsaPublicKey();
    var privkey = new UsIPtr<byte>(mldsa.ExportMLDsaPrivateKey());

    var signatur = Sign(message, privkey, param);
    var verifier = Verify(message, pubkey, signatur, param);
    if (verifier)
      return (signatur, privkey, pubkey, param);

    throw new NotImplementedException(
      $"MultiSign: Multi-Signatur-Verify has failed!",
      new Exception($"{nameof(verifier)} == false"));
  }

  public async static Task<(byte[] Sign, UsIPtr<byte> PrivKey, byte[] PubKey, MLDsaAlgorithm Param)> MultiSignaturFileAsync(
    string filename, ReadOnlyMemory<byte[]> guids,
    ReadOnlyMemory<byte[]> signs, ReadOnlyMemory<byte[]> publickeys,
    ReadOnlyMemory<byte[]> signersnames, ReadOnlyMemory<byte[]> projectnames,
    ReadOnlyMemory<byte> sign_algo, ReadOnlyMemory<byte> mldsa_param,
    CancellationToken ct = default)
  {
    AssertPqcMlDsaMultiSignFileAot(filename, guids, signs, publickeys,
      signersnames, projectnames, sign_algo, mldsa_param);

    await CheckVerifiesAsync(filename, signs, publickeys, mldsa_param, ct);

    //Now Create the Multi-Sign-Verify
    var seed = await ToMultHashSeedAsync(filename, guids, signs, publickeys,
      signersnames, projectnames, sign_algo, mldsa_param, 32, ct);

    var mldsa_algo = Enum.GetValues<MLDsaParam>();
    var idx = RandomNumberGenerator.GetInt32(mldsa_algo.Length);
    var param = ToMLDsaAlgorithm(mldsa_algo[idx]);

    using var mldsa = MLDsa.ImportMLDsaPrivateSeed(param, seed);

    var pubkey = mldsa.ExportMLDsaPublicKey();
    var privkey = new UsIPtr<byte>(mldsa.ExportMLDsaPrivateKey());

    var signatur = await SignFileAsync(filename, privkey, param, ct);
    var verifier = await VerifyFileAsync(filename, pubkey, signatur, param, ct);
    if (verifier)
      return (signatur, privkey, pubkey, param);

    throw new NotImplementedException(
      $"MultiSign: Multi-Signatur-Verify has failed!",
      new Exception($"{nameof(verifier)} == false"));

  }

  private static byte[] ToMultHashSeed(
    ReadOnlySpan<byte> message, ReadOnlySpan<byte[]> guids,
    ReadOnlySpan<byte[]> signs, ReadOnlySpan<byte[]> publickeys,
    ReadOnlySpan<byte[]> signernames, ReadOnlySpan<byte[]> projectnames,
    ReadOnlySpan<byte> sign_algos, ReadOnlySpan<byte> mldsa_params)
  {
    var hash_length = 32;
    var count = guids.Length;
    var hashs = new List<byte[]>(count);
    for (var i = 0; i < count; i++)
      hashs.Add(ToHashShake256(message, guids[i],
      signs[i], publickeys[i], signernames[i], projectnames[i],
      sign_algos[i], mldsa_params[i], hash_length));

    return Shake256.HashData(Concat([.. hashs]), hash_length);
  }

  private async static Task<byte[]> ToMultHashSeedAsync(
   string filename, ReadOnlyMemory<byte[]> guids,
   ReadOnlyMemory<byte[]> signs, ReadOnlyMemory<byte[]> publickeys,
   ReadOnlyMemory<byte[]> signernames, ReadOnlyMemory<byte[]> projectnames,
   ReadOnlyMemory<byte> sign_algos, ReadOnlyMemory<byte> mldsa_params,
   int hash_length = 32, CancellationToken ct = default)
  {
    var count = guids.Length;
    var hashs = new List<byte[]>(count);
    for (var i = 0; i < count; i++)
      hashs.Add(await ToHashShake256Async(filename,
        guids.Span[i], signs.Span[i], publickeys.Span[i],
        signernames.Span[i], projectnames.Span[i],
        sign_algos.Span[i], mldsa_params.Span[i], hash_length, ct));

    return Shake256.HashData(Concat([.. hashs]), hash_length);
  }

  private static void CheckVerifies(
    ReadOnlySpan<byte> message, ReadOnlySpan<byte[]> signs,
    ReadOnlySpan<byte[]> publickeys, ReadOnlySpan<byte> mldsa_param)
  {
    var count = signs.Length;
    for (var i = 0; i < count; i++)
    {
      //The first character in the 'sign[x]' is the mldsa-algo
      var signature = signs[i].AsSpan(1);
      var param = ToMLDsaAlgorithm((MLDsaParam)signs[i][0]);
      if (param != ToMLDsaAlgorithm((MLDsaParam)mldsa_param[i]))
        throw new ArgumentException(
          $"MultiSign: {nameof(mldsa_param)}[{i}] != param_algo => {param.Name}",
          nameof(mldsa_param));

      if (Verify(message, publickeys[i], signature, param))
        continue;

      throw new InvalidOperationException(
        $"MultiSign: {nameof(signs)} not verify!");
    }
  }

  private async static Task CheckVerifiesAsync(
    string filename, ReadOnlyMemory<byte[]> signs,
    ReadOnlyMemory<byte[]> publickeys,
    ReadOnlyMemory<byte> mldsa_param,
    CancellationToken ct = default)
  {
    var count = signs.Length;
    for (var i = 0; i < count; i++)
    {
      //The first character in the 'sign[x]' is the mldsa-algo
      ReadOnlyMemory<byte> signature = signs.Span[i].AsMemory(1);
      var param = ToMLDsaAlgorithm((MLDsaParam)signs.Span[i][0]);
      if (param != ToMLDsaAlgorithm((MLDsaParam)mldsa_param.Span[i]))
        throw new ArgumentException(
          $"MultiSign: {nameof(mldsa_param)}[{i}] != param_algo => {param.Name}",
          nameof(mldsa_param));

      if (await VerifyFileAsync(filename, publickeys.Span[i], signature, param, ct))
        continue;

      throw new InvalidOperationException(
        $"MultiSign: {nameof(signs)} not verify!");
    }
  }

  private static byte[] ToHashShake256(
    ReadOnlySpan<byte> message, ReadOnlySpan<byte> guid,
    ReadOnlySpan<byte> sign, ReadOnlySpan<byte> publickey,
    ReadOnlySpan<byte> signersname, ReadOnlySpan<byte> projectname,
    byte sign_algo, byte mldsa_param, int hash_length = 32)
  {

    var sign_algo_param = new byte[2];
    sign_algo_param[0] = sign_algo;
    sign_algo_param[1] = mldsa_param;

    var a = Shake256.HashData(message, hash_length);
    var b = Shake256.HashData(sign_algo_param, hash_length);
    var c = Shake256.HashData(guid, hash_length);
    var d = Shake256.HashData(sign, hash_length);
    var e = Shake256.HashData(publickey, hash_length);
    var f = Shake256.HashData(signersname, hash_length);
    var g = Shake256.HashData(projectname, hash_length);

    return Shake256.HashData(Concat(a, b, c, d, e, f, g), hash_length);
  }

  private async static Task<byte[]> ToHashShake256Async(
    string filename, ReadOnlyMemory<byte> guid,
    ReadOnlyMemory<byte> sign, ReadOnlyMemory<byte> publickey,
    ReadOnlyMemory<byte> signersname, ReadOnlyMemory<byte> projectname,
    byte sign_algo, byte mldsa_param, int hash_length = 32,
    CancellationToken ct = default)
  {
    var sign_algo_param = new byte[2];
    sign_algo_param[0] = sign_algo;
    sign_algo_param[1] = mldsa_param;

    var a = await ComputeHashFileAsync(filename,
      HashAlgorithmType.Shake256, hash_length, ct);
    var b = Shake256.HashData(sign_algo_param, hash_length);
    var c = Shake256.HashData(guid.Span, hash_length);
    var d = Shake256.HashData(sign.Span, hash_length);
    var e = Shake256.HashData(publickey.Span, hash_length);
    var f = Shake256.HashData(signersname.Span, hash_length);
    var g = Shake256.HashData(projectname.Span, hash_length);

    return Shake256.HashData(Concat(a, b, c, d, e, f, g), hash_length);
  }

  private static byte[] Concat(params byte[][] bytes)
  {
    var count = bytes.Length;
    var size = bytes.Sum(x => x.Length);
    var result = new byte[size];
    for (int i = 0, start = 0; i < count; i++, start += bytes[i - 1].Length)
      Array.Copy(bytes[i], 0, result, start, bytes[i].Length);

    return result;
  }


  private static void AssertPqcMlDsaMultiSignAot(
    ReadOnlySpan<byte> message, ReadOnlySpan<byte[]> guids,
    ReadOnlySpan<byte[]> signs, ReadOnlySpan<byte[]> pubkeys,
    ReadOnlySpan<byte[]> signer_names, ReadOnlySpan<byte[]> project_names,
    ReadOnlySpan<byte> sign_algos, ReadOnlySpan<byte> mldsa_param)
  {
    var count = guids.Length;

    if (IsNullOrEmpty(message))
      throw new ArgumentNullException(nameof(message),
        $"AssertPqcMlDsaMultiSignAot: {nameof(message)} == null!");

    // ********* ********* ********* ********* ********* ********* ********* 

    if (IsNullOrEmpty(guids))
      throw new ArgumentNullException(nameof(guids),
        $"AssertPqcMlDsaMultiSignAot: {nameof(guids)} == default!");

    if (guids.Length != count)
      throw new ArgumentOutOfRangeException(nameof(guids),
        $"AssertPqcMlDsaMultiSignAot: {nameof(guids)}.Length != {count}!");

    // ********* ********* ********* ********* ********* ********* ********* 

    if (IsNullOrEmpty(signs))
      throw new ArgumentNullException(nameof(signs),
        $"AssertPqcMlDsaMultiSignAot: {nameof(signs)} == null!");

    if (signs.Length != count)
      throw new ArgumentOutOfRangeException(nameof(signs),
        $"AssertPqcMlDsaMultiSignAot: {nameof(signs)}.Length != {count}!");

    // ********* ********* ********* ********* ********* ********* ********* 

    if (IsNullOrEmpty(pubkeys))
      throw new ArgumentNullException(nameof(pubkeys),
        $"AssertPqcMlDsaMultiSignAot: {nameof(pubkeys)} == null!");

    if (pubkeys.Length != count)
      throw new ArgumentOutOfRangeException(nameof(pubkeys),
        $"AssertPqcMlDsaMultiSignAot: {nameof(pubkeys)}.Length != {count}!");

    // ********* ********* ********* ********* ********* ********* ********* 

    if (IsNullOrEmpty(signer_names))
      throw new ArgumentNullException(nameof(signer_names),
        $"AssertPqcMlDsaMultiSignAot: {nameof(signer_names)} == null!");

    if (signer_names.Length != count)
      throw new ArgumentOutOfRangeException(nameof(signer_names),
        $"AssertPqcMlDsaMultiSignAot: {nameof(signer_names)}.Length != {count}!");

    // ********* ********* ********* ********* ********* ********* ********* 

    if (IsNullOrEmpty(project_names))
      throw new ArgumentNullException(nameof(project_names),
        $"AssertPqcMlDsaMultiSignAot: {nameof(project_names)} == null!");

    if (project_names.Length != count)
      throw new ArgumentOutOfRangeException(nameof(project_names),
        $"AssertPqcMlDsaMultiSignAot: {nameof(project_names)}.Length != {count}!");

    // ********* ********* ********* ********* ********* ********* ********* 

    if (IsNullOrEmpty(sign_algos))
      throw new ArgumentNullException(nameof(sign_algos),
        $"AssertPqcMlDsaMultiSignAot: {nameof(sign_algos)} == null!");

    if (sign_algos.Length != count)
      throw new ArgumentOutOfRangeException(nameof(sign_algos),
        $"AssertPqcMlDsaMultiSignAot: {nameof(sign_algos)}.Length != {count}!");

    // ********* ********* ********* ********* ********* ********* ********* 

    if (IsNullOrEmpty(mldsa_param))
      throw new ArgumentNullException(nameof(mldsa_param),
        $"AssertPqcMlDsaMultiSignAot: {nameof(mldsa_param)} == null!");

    if (mldsa_param.Length != count)
      throw new ArgumentOutOfRangeException(nameof(mldsa_param),
        $"AssertPqcMlDsaMultiSignAot: {nameof(mldsa_param)}.Length != {count}!");
  }

  private static void AssertPqcMlDsaMultiSignFileAot(
    string filename, ReadOnlyMemory<byte[]> guids,
    ReadOnlyMemory<byte[]> signs, ReadOnlyMemory<byte[]> pubkeys,
    ReadOnlyMemory<byte[]> signer_names, ReadOnlyMemory<byte[]> project_names,
    ReadOnlyMemory<byte> sign_algos, ReadOnlyMemory<byte> mldsa_param)
  {
    var count = guids.Length;

    if (string.IsNullOrEmpty(filename))
      throw new ArgumentNullException(nameof(filename),
        $"AssertPqcMlDsaMultiSignAot: {nameof(filename)} == null!");

    if (!File.Exists(filename))
      throw new FileNotFoundException($"{nameof(filename)} =\n{filename}");

    // ********* ********* ********* ********* ********* ********* ********* 

    if (IsNullOrEmpty(guids.Span))
      throw new ArgumentNullException(nameof(guids),
        $"AssertPqcMlDsaMultiSignAot: {nameof(guids)} == default!");

    if (guids.Length != count)
      throw new ArgumentOutOfRangeException(nameof(guids),
        $"AssertPqcMlDsaMultiSignAot: {nameof(guids)}.Length != {count}!");

    // ********* ********* ********* ********* ********* ********* ********* 

    if (IsNullOrEmpty(signs.Span))
      throw new ArgumentNullException(nameof(signs),
        $"AssertPqcMlDsaMultiSignAot: {nameof(signs)} == null!");

    if (signs.Length != count)
      throw new ArgumentOutOfRangeException(nameof(signs),
        $"AssertPqcMlDsaMultiSignAot: {nameof(signs)}.Length != {count}!");

    // ********* ********* ********* ********* ********* ********* ********* 

    if (IsNullOrEmpty(pubkeys.Span))
      throw new ArgumentNullException(nameof(pubkeys),
        $"AssertPqcMlDsaMultiSignAot: {nameof(pubkeys)} == null!");

    if (pubkeys.Length != count)
      throw new ArgumentOutOfRangeException(nameof(pubkeys),
        $"AssertPqcMlDsaMultiSignAot: {nameof(pubkeys)}.Length != {count}!");

    // ********* ********* ********* ********* ********* ********* ********* 

    if (IsNullOrEmpty(signer_names.Span))
      throw new ArgumentNullException(nameof(signer_names),
        $"AssertPqcMlDsaMultiSignAot: {nameof(signer_names)} == null!");

    if (signer_names.Length != count)
      throw new ArgumentOutOfRangeException(nameof(signer_names),
        $"AssertPqcMlDsaMultiSignAot: {nameof(signer_names)}.Length != {count}!");

    // ********* ********* ********* ********* ********* ********* ********* 

    if (IsNullOrEmpty(project_names.Span))
      throw new ArgumentNullException(nameof(project_names),
        $"AssertPqcMlDsaMultiSignAot: {nameof(project_names)} == null!");

    if (project_names.Length != count)
      throw new ArgumentOutOfRangeException(nameof(project_names),
        $"AssertPqcMlDsaMultiSignAot: {nameof(project_names)}.Length != {count}!");

    // ********* ********* ********* ********* ********* ********* ********* 

    if (IsNullOrEmpty(sign_algos.Span))
      throw new ArgumentNullException(nameof(sign_algos),
        $"AssertPqcMlDsaMultiSignAot: {nameof(sign_algos)} == null!");

    if (sign_algos.Length != count)
      throw new ArgumentOutOfRangeException(nameof(sign_algos),
        $"AssertPqcMlDsaMultiSignAot: {nameof(sign_algos)}.Length != {count}!");

    // ********* ********* ********* ********* ********* ********* ********* 

    if (IsNullOrEmpty(mldsa_param.Span))
      throw new ArgumentNullException(nameof(mldsa_param),
        $"AssertPqcMlDsaMultiSignAot: {nameof(mldsa_param)} == null!");

    if (mldsa_param.Length != count)
      throw new ArgumentOutOfRangeException(nameof(mldsa_param),
        $"AssertPqcMlDsaMultiSignAot: {nameof(mldsa_param)}.Length != {count}!");
  }

  private static bool IsNullOrEmpty(string[] strs)
  {
    if (strs == null) return true;

    foreach (var str in strs)
      if (string.IsNullOrEmpty(str))
        return true;

    return false;
  }

  private static bool IsNullOrEmpty(ReadOnlySpan<byte[]> bytes)
  {
    if (bytes.IsEmpty) return true;

    foreach (var data in bytes)
      if (IsNullOrEmpty(data))
        return true;

    return false;
  }

  private static bool IsNullOrEmpty(ReadOnlySpan<byte> bytes)
  {
    if (bytes.IsEmpty) return true;
    return bytes.Length == 0;
  }

  private static bool IsDefaultOrEmpty(ReadOnlySpan<Guid> guids)
  {
    if (guids.IsEmpty) return true;

    foreach (var data in guids)
      if (IsDefaultOrEmpty(data))
        return true;

    return false;
  }

  private static bool IsDefaultOrEmpty(Guid guid)
  {
    return guid == Guid.Empty;
  }
}
