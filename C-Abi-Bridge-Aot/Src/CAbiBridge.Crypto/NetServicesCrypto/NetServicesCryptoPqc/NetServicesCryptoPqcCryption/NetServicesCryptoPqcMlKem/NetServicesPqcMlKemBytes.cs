//ML-KEM (Kyber)
//Module-Lattice-Based
//FIPS PUB 203
//https://nvlpubs.nist.gov/nistpubs/FIPS/NIST.FIPS.203.ipd.pdf


using System.Security.Cryptography;

namespace michele.natale;

using Pointers;

partial class NetServicesCrypto
{
  private readonly static byte[] AssociatKey =
    "© michele natale 2025"u8.ToArray();

  public static MlKemKeyPairInfo CreateMlKemKeyPair()
  {
    var mlkem_param = ToMLKemAlgorithm();

    //ML-KEM Parameter select. Only the first 3 algos
    var idx = RandomNumberGenerator.GetInt32(mlkem_param.Length);
    var param = FromMLKemAlgorithm(mlkem_param[idx]);

    //Symmetric CryptionAlgorithm select
    var cas = Enum.GetValues<CryptionAlgorithm>();
    idx = RandomNumberGenerator.GetInt32(cas.Length);
    var cryptoalgo = cas[idx];

    return CreateMlKemKeyPair(param, cryptoalgo);
  }

  public static MlKemKeyPairInfo CreateMlKemKeyPair(
    MLKemParam mlkem_param, CryptionAlgorithm crypto_algo)
  {
    var param = ToMLKemAlgorithm(mlkem_param);

    //Create legal ML-KEM-KeyPair
    using var kem = MLKem.GenerateKey(param);
    var (privk, pubk) = ToKeyPair(kem);

    var result = new MlKemKeyPairInfo(
      pubk, privk.ToBytes(), param, crypto_algo);

    if (result.Id == Guid.Empty)
      result.Id = Guid.NewGuid();

    return result;
  }

  public static void SaveMlKemKeyPair(
    string keypairfile, ReadOnlySpan<byte> priv_key,
    ReadOnlySpan<byte> pub_key, ReadOnlySpan<byte> guid_id,
    MLKemAlgorithm mlkem_algo, CryptionAlgorithm crypto_algo,
    bool save_private_key)
  {
    AssertSaveMlKemKeyPair(keypairfile, priv_key, pub_key, guid_id);

    var guid = new Guid(guid_id);
    using var mlkeminfo = new MlKemKeyPairInfo(
      guid, pub_key, priv_key, mlkem_algo, crypto_algo);
    mlkeminfo.SaveKeyPair(keypairfile, save_private_key);
  }

  public static MlKemKeyPairInfo LoadMlKemKeyPair(string keypairfile)
  {
    AssertLoadMlKemKeyPair(keypairfile);
    return MlKemKeyPairInfo.Load_KeyPair(keypairfile);
  }

  public static (UsIPtr<byte> SharedKey, byte[] Capsualtion) ToPqcMlKemCapsulationFromPubKey(
    ReadOnlySpan<byte> alice_pub_key, MLKemAlgorithm mlkem_param)
  {
    AssertToPqcMlKemCapsulationFromPubKey(alice_pub_key);

    // Bob encapsulates a new shared secret key using alice's public key
    using var alice = MLKem.ImportEncapsulationKey(mlkem_param, alice_pub_key);
    var sharedkey = ToSharedKey(alice, out var capsulation);

    return (sharedkey, capsulation);
  }

  public static UsIPtr<byte> ToPqcMlKemSharedKeyFromPrivateKey(
    ReadOnlySpan<byte> capsulation, ReadOnlySpan<byte> alice_private_key,
    MLKemAlgorithm mlkem_param)
  {
    AssertToPqcMlKemSharedKeyFromPrivateKey(capsulation, alice_private_key);

    var alice = MLKem.ImportDecapsulationKey(mlkem_param, alice_private_key);
    var alice_secret_shared_key = alice.Decapsulate(capsulation.ToArray());

    var sharedkey = new UsIPtr<byte>(alice_secret_shared_key);
    Array.Clear(alice_secret_shared_key);

    return sharedkey;
  }

  public static byte[] PqcMlKemEncryption(
    ReadOnlySpan<byte> message, ReadOnlySpan<byte> capsulation,
    UsIPtr<byte> privatekey, ReadOnlySpan<byte> associated,
    MLKemAlgorithm mlkem_param, CryptionAlgorithm cryptoalgo)
  {
    AssertPqcMlKemEncryption(message, capsulation, privatekey);
    var associat = associated.Length == 0 ? AssociatKey : associated;

    using var alice = MLKem.ImportDecapsulationKey(mlkem_param, privatekey.ToBytes());
    var alice_secret_shared_key = alice.Decapsulate(capsulation.ToArray());

    using var sharedkey = new UsIPtr<byte>(alice_secret_shared_key);
    Array.Clear(alice_secret_shared_key);

    return EncryptionWithCryptionAlgo(
      message, sharedkey, associat, cryptoalgo);
  }

  public static byte[] PqcMlKemDecryption(
    ReadOnlySpan<byte> ciphertext, UsIPtr<byte> sharedkey,
    ReadOnlySpan<byte> associated,
    MLKemAlgorithm mlkem_param, CryptionAlgorithm cryptoalgo)
  {
    AssertPqcMlKemDecryption(ciphertext, sharedkey);
    var associat = associated.IsEmpty ? AssociatKey : associated;

    return DecryptionWithCryptionAlgo(
      ciphertext, sharedkey, associat, cryptoalgo);
  }

  private static void AssertSaveMlKemKeyPair(
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

  private static void AssertLoadMlKemKeyPair(string keypairfile)
  {
    if (!File.Exists(keypairfile))
      throw new FileNotFoundException(nameof(keypairfile));
  }

  private static void AssertToPqcMlKemCapsulationFromPubKey(
    ReadOnlySpan<byte> alice_pub_key)
  {
    if (alice_pub_key.IsEmpty || alice_pub_key.Length == 0)
      throw new ArgumentNullException(nameof(alice_pub_key));
  }

  private static void AssertToPqcMlKemSharedKeyFromPrivateKey(
    ReadOnlySpan<byte> capsulation, ReadOnlySpan<byte> alice_private_key)
  {
    if (capsulation.IsEmpty || capsulation.Length == 0)
      throw new ArgumentNullException(nameof(capsulation));

    if (alice_private_key.IsEmpty || alice_private_key.Length == 0)
      throw new ArgumentNullException(nameof(alice_private_key));
  }

  private static void AssertPqcMlKemEncryption(
    ReadOnlySpan<byte> message, ReadOnlySpan<byte> capsulation,
    UsIPtr<byte> privatekey)
  {
    if (message.IsEmpty || message.Length == 0)
      throw new ArgumentNullException(nameof(message));

    if (message.Length < ML_KEM_MIN_PLAIN_SIZE)
      throw new ArgumentNullException(nameof(message),
        $"mlkem-message.Length >= {ML_KEM_MIN_PLAIN_SIZE} <= {ML_KEM_MAX_PLAIN_SIZE}");

    if (message.Length > ML_KEM_MAX_PLAIN_SIZE)
      throw new ArgumentNullException(nameof(message),
        $"mlkem-message.Length >= {ML_KEM_MIN_PLAIN_SIZE} <= {ML_KEM_MAX_PLAIN_SIZE}");

    if (privatekey.IsEmpty || privatekey.Length == 0)
      throw new ArgumentNullException(nameof(privatekey));

    if (capsulation.IsEmpty || capsulation.Length == 0)
      throw new ArgumentNullException(nameof(capsulation));
  }

  private static void AssertPqcMlKemDecryption(
    ReadOnlySpan<byte> ciphertext, UsIPtr<byte> sharedkey)
  {
    if (ciphertext.IsEmpty || ciphertext.Length == 0)
      throw new ArgumentNullException(nameof(ciphertext));

    if (sharedkey.IsEmpty || sharedkey.Length == 0)
      throw new ArgumentNullException(nameof(sharedkey));
  }

}
