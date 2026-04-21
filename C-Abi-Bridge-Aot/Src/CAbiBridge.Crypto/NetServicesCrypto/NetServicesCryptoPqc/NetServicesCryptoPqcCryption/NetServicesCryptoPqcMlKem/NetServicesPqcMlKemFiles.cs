
//ML-KEM (Kyber)
//Module-Lattice-Based
//FIPS PUB 203
//https://nvlpubs.nist.gov/nistpubs/FIPS/NIST.FIPS.203.ipd.pdf


using System.Security.Cryptography;


namespace michele.natale;

using Pointers;

partial class NetServicesCrypto
{
  //public async static Task PqcMlKemEncryptionKpiFileAsync(
  //  string src_file, string dest_file,
  //  string keypairfile, ReadOnlyMemory<byte> associated)
  //{
  //  AssertPqcMlKemEncryptionFile(src_file, dest_file, keypairfile);
  //  await MlKemEncryptionFileAsync(src_file, dest_file, keypairfile, associated);
  //}

  public async static Task PqcMlKemEncryptionFileAsync(
    string src_file, string dest_file,
    UsIPtr<byte> private_key, ReadOnlyMemory<byte> capsulation,
    ReadOnlyMemory<byte> associated, MLKemAlgorithm mlkem_param,
    CryptionAlgorithm crypto_algo, CancellationToken ct = default)
  {
    AssertPqcMlKemEncryptionFile(src_file, dest_file, private_key, capsulation);

    //var kem = MLKem.ImportDecapsulationKey(mlkem_param, private_key.ToArray());
    //var pubkey = kem.ExportEncapsulationKey();

    await MlKemEncryptionFileAsync(
      src_file, dest_file, private_key, capsulation,
      associated, mlkem_param, crypto_algo, ct);
  }

  //public async static Task PqcMlKemDecryptionKpiFileAsync(
  //  string src_file, string dest_file, string key_pair_file,
  //  ReadOnlyMemory<byte> associated)
  //{
  //  AssertPqcMlKemDecryptionFile(src_file, dest_file, key_pair_file);
  //  await MlKemDecryptionFileAsync(src_file, dest_file, key_pair_file, associated);
  //}

  public async static Task PqcMlKemDecryptionFileAsync(
    string src_file, string dest_file,
    UsIPtr<byte> shared_key, ReadOnlyMemory<byte> associated,
    CancellationToken ct = default)
  {
    AssertPqcMlKemDecryptionFile(src_file, dest_file, shared_key);
    await MlKemDecryptionFileAsync(src_file, dest_file, shared_key, associated, ct);
  }

  private static void AssertPqcMlKemEncryptionFile(
   string src_file, string dest_file, string key_pair_file)
  {
    if (File.Exists(dest_file)) File.Delete(dest_file);

    if (File.Exists(dest_file))
      throw new ArgumentException(
        $"The file '{nameof(dest_file)}' cannot be deleted!", nameof(dest_file));

    if (!File.Exists(src_file))
      throw new FileNotFoundException(nameof(src_file));

    if (!File.Exists(key_pair_file))
      throw new FileNotFoundException(nameof(key_pair_file));
  }

  private static void AssertPqcMlKemEncryptionFile(
    string src_file, string dest_file,
    UsIPtr<byte> private_key, ReadOnlyMemory<byte> capsulation)
  {
    if (File.Exists(dest_file)) File.Delete(dest_file);

    if (!File.Exists(src_file))
      throw new FileNotFoundException(nameof(src_file));

    if (File.Exists(dest_file))
      throw new ArgumentException(
        $"The file '{nameof(dest_file)}' cannot be deleted!", nameof(dest_file));

    if (private_key.IsEmpty || private_key.Length == 0)
      throw new ArgumentNullException(nameof(private_key));

    if (capsulation.IsEmpty || capsulation.Length == 0)
      throw new ArgumentNullException(nameof(capsulation));
  }

  private static void AssertPqcMlKemDecryptionFile(
    string src_file, string dest_file, string key_pair_file) =>
      AssertPqcMlKemEncryptionFile(src_file, dest_file, key_pair_file);

  private static void AssertPqcMlKemDecryptionFile(
    string src_file, string dest_file, UsIPtr<byte> shared_key)
  {
    if (File.Exists(dest_file)) File.Delete(dest_file);

    if (!File.Exists(src_file))
      throw new FileNotFoundException(nameof(src_file));

    if (File.Exists(dest_file))
      throw new ArgumentException(
        $"The file '{nameof(dest_file)}' cannot be deleted!", nameof(dest_file));

    if (shared_key.IsEmpty || shared_key.Length == 0)
      throw new ArgumentNullException(nameof(shared_key));
  }
}
