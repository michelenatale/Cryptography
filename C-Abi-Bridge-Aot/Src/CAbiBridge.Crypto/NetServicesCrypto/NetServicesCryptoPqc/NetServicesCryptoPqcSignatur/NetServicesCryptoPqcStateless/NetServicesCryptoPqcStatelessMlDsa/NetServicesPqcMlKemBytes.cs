//ML-DSA
//Module-Lattice-Based
//FIPS PUB 204 
//https://nvlpubs.nist.gov/nistpubs/fips/nist.fips.204.pdf

using System.Security.Cryptography;

namespace michele.natale;

using Pointers;

partial class NetServicesCrypto
{
  public static MlDsaKeyPairInfo CreateMlDsaKeyPair()
  {
    var mldsa_param = ToMLDsaAlgorithm();

    //ML-DSA Parameter select. Only the first 3 algos
    var idx = RandomNumberGenerator.GetInt32(mldsa_param.Length);
    var param = FromMLDsaAlgorithm(mldsa_param[idx]);

    return CreateMlDsaKeyPair(param);
  }

  public static MlDsaKeyPairInfo CreateMlDsaKeyPair(
    MLDsaParam mldsa_param)
  {
    var param = ToMLDsaAlgorithm(mldsa_param);

    //Create legal ML-DSA-KeyPair
    using var dsa = MLDsa.GenerateKey(param);
    var (privk, pubk) = ToKeyPair(dsa);

    var result = new MlDsaKeyPairInfo(
      pubk, privk.ToBytes(), param);

    if (result.Id == Guid.Empty)
      result.Id = Guid.NewGuid();

    return result;
  }

  public static void SaveMlDsaKeyPair(
    string keypairfile, ReadOnlySpan<byte> priv_key,
    ReadOnlySpan<byte> pub_key, ReadOnlySpan<byte> guid_id,
    MLDsaAlgorithm mldsa_algo, bool save_private_key)
  {
    AssertSaveMlDsaKeyPair(keypairfile, priv_key, pub_key, guid_id);

    var guid = new Guid(guid_id);
    using var mldsainfo = new MlDsaKeyPairInfo(
      guid, pub_key, priv_key, mldsa_algo);
    mldsainfo.SaveKeyPair(keypairfile, save_private_key);
  }

  public static MlDsaKeyPairInfo LoadMlDsaKeyPair(string keypairfile)
  {
    AssertLoadMlDsaKeyPair(keypairfile);
    return MlDsaKeyPairInfo.Load_KeyPair(keypairfile);
  }



  private static void AssertSaveMlDsaKeyPair(
   string keypairfile, ReadOnlySpan<byte> priv_key,
   ReadOnlySpan<byte> pub_key, ReadOnlySpan<byte> guid_id)
  {
    if (File.Exists(keypairfile)) File.Delete(keypairfile);

    if (File.Exists(keypairfile))
      throw new ArgumentException(
        $"The file '{nameof(keypairfile)}' cannot be deleted!",
          nameof(keypairfile));

    if (priv_key.IsEmpty || priv_key.Length == 0)
      throw new ArgumentNullException(nameof(priv_key));

    if (pub_key.IsEmpty || pub_key.Length == 0)
      throw new ArgumentNullException(nameof(pub_key));

    if (guid_id.IsEmpty || guid_id.Length == 0)
      throw new ArgumentNullException(nameof(guid_id));
  }

  private static void AssertLoadMlDsaKeyPair(string keypairfile)
  {
    if (!File.Exists(keypairfile))
      throw new FileNotFoundException(nameof(keypairfile));
  }

}
