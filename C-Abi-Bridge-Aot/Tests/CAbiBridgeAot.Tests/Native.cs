


using System.Runtime.InteropServices;

namespace michele.natale.Tests;

using CAbiBridge;

internal static partial class Native
{
  const string DllName = "C-Abi-Bridge.Aot.N.dll";

  #region Allocation - Free
  [LibraryImport(DllName, EntryPoint = "free_buffer_aot")]
  internal static partial void FreeBuffer(IntPtr ptr);
  #endregion Allocation - Free

  #region Crypto Symmetric

  #region Aes

  [LibraryImport(DllName, EntryPoint = "aes_encrypt_aot")]
  internal static partial CError AesEncryptAot(
    ReadOnlySpan<byte> bytes, int bytes_length,
    IntPtr key, int key_length,
    ReadOnlySpan<byte> associated, int associated_length,
    out IntPtr output, out int output_length);

  [LibraryImport(DllName, EntryPoint = "aes_decrypt_aot")]
  internal static partial CError AesDecryptAot(
    ReadOnlySpan<byte> bytes, int bytes_length,
    IntPtr key, int key_length,
    ReadOnlySpan<byte> associated, int associated_length,
    out IntPtr output, out int output_length);

  [LibraryImport(DllName, EntryPoint = "aes_encrypt_file_aot")]
  public static partial CError AesEncryptFileAot(
   ReadOnlySpan<byte> src, int src_length,
   ReadOnlySpan<byte> dest, int dest_length,
   IntPtr key, int key_length,
   ReadOnlySpan<byte> associated, int associated_length);

  [LibraryImport(DllName, EntryPoint = "aes_decrypt_file_aot")]
  public static partial CError AesDecryptFileAot(
   ReadOnlySpan<byte> src, int src_length,
   ReadOnlySpan<byte> dest, int dest_length,
   IntPtr key, int key_length,
   ReadOnlySpan<byte> associated, int associated_length);

  #endregion Aes

  #region AesGcm

  [LibraryImport(DllName, EntryPoint = "aes_gcm_encrypt_aot")]
  internal static partial CError AesGcmEncryptAot(
    ReadOnlySpan<byte> bytes, int bytes_length,
    IntPtr key, int key_length,
    ReadOnlySpan<byte> associated, int associated_length,
    out IntPtr output, out int output_length);

  [LibraryImport(DllName, EntryPoint = "aes_gcm_decrypt_aot")]
  internal static partial CError AesGcmDecryptAot(
    ReadOnlySpan<byte> bytes, int bytes_length,
    IntPtr key, int key_length,
    ReadOnlySpan<byte> associated, int associated_length,
    out IntPtr output, out int output_length);

  [LibraryImport(DllName, EntryPoint = "aes_gcm_encrypt_file_aot")]
  public static partial CError AesGcmEncryptFileAot(
   ReadOnlySpan<byte> src, int src_length,
   ReadOnlySpan<byte> dest, int dest_length,
   IntPtr key, int key_length,
   ReadOnlySpan<byte> associated, int associated_length);

  [LibraryImport(DllName, EntryPoint = "aes_gcm_decrypt_file_aot")]
  public static partial CError AesGcmDecryptFileAot(
   ReadOnlySpan<byte> src, int src_length,
   ReadOnlySpan<byte> dest, int dest_length,
   IntPtr key, int key_length,
   ReadOnlySpan<byte> associated, int associated_length);

  #endregion AesGcm

  #region ChaCha20Poly1305

  [LibraryImport(DllName, EntryPoint = "chacha20_poly1305_encrypt_aot")]
  internal static partial CError ChaCha20Poly1305EncryptAot(
    ReadOnlySpan<byte> bytes, int bytes_length,
    IntPtr key, int key_length,
    ReadOnlySpan<byte> associated, int associated_length,
    out IntPtr output, out int output_length);

  [LibraryImport(DllName, EntryPoint = "chacha20_poly1305_decrypt_aot")]
  internal static partial CError ChaCha20Poly1305DecryptAot(
    ReadOnlySpan<byte> bytes, int bytes_length,
    IntPtr key, int key_length,
    ReadOnlySpan<byte> associated, int associated_length,
    out IntPtr output, out int output_length);

  [LibraryImport(DllName, EntryPoint = "chacha20_poly1305_encrypt_file_aot")]
  public static partial CError ChaCha20Poly1305EncryptFileAot(
   ReadOnlySpan<byte> src, int src_length,
   ReadOnlySpan<byte> dest, int dest_length,
   IntPtr key, int key_length,
   ReadOnlySpan<byte> associated, int associated_length);

  [LibraryImport(DllName, EntryPoint = "chacha20_poly1305_decrypt_file_aot")]
  public static partial CError ChaCha20Poly1305DecryptFileAot(
   ReadOnlySpan<byte> src, int src_length,
   ReadOnlySpan<byte> dest, int dest_length,
   IntPtr key, int key_length,
   ReadOnlySpan<byte> associated, int associated_length);

  #endregion ChaCha20Poly1305

  #endregion Crypto Symmetric

  #region Crypto Hash - Hmac

  #region Crypto Hash

  [LibraryImport(DllName, EntryPoint = "sha_256_hash_data_aot")]
  internal static partial CError Sha256HashDataAot(
    ReadOnlySpan<byte> bytes, int bytes_length,
    out IntPtr output, out int output_length);

  [LibraryImport(DllName, EntryPoint = "sha_384_hash_data_aot")]
  public static partial CError Sha384HashDataAot(
    ReadOnlySpan<byte> bytes, int bytes_length,
    out IntPtr output, out int output_length);

  [LibraryImport(DllName, EntryPoint = "sha_512_hash_data_aot")]
  public static partial CError Sha512HashDataAot(
    ReadOnlySpan<byte> bytes, int bytes_length,
    out IntPtr output, out int output_length);

  [LibraryImport(DllName, EntryPoint = "sha1_hash_data_aot")]
  public static partial CError Sha1HashDataAot(
    ReadOnlySpan<byte> bytes, int bytes_length,
    out IntPtr output, out int output_length);

  [LibraryImport(DllName, EntryPoint = "md5_hash_data_aot")]
  public static partial CError Md5HashDataAot(
    ReadOnlySpan<byte> bytes, int bytes_length,
    out IntPtr output, out int output_length);

  [LibraryImport(DllName, EntryPoint = "sha3_256_hash_data_aot")]
  internal static partial CError Sha3256HashDataAot(
   ReadOnlySpan<byte> bytes, int bytes_length,
   out IntPtr output, out int output_length);

  [LibraryImport(DllName, EntryPoint = "sha3_384_hash_data_aot")]
  public static partial CError Sha3384HashDataAot(
    ReadOnlySpan<byte> bytes, int bytes_length,
    out IntPtr output, out int output_length);

  [LibraryImport(DllName, EntryPoint = "sha3_512_hash_data_aot")]
  public static partial CError Sha3512HashDataAot(
    ReadOnlySpan<byte> bytes, int bytes_length,
    out IntPtr output, out int output_length);

  [LibraryImport(DllName, EntryPoint = "shake_128_hash_data_aot")]
  internal static partial CError Shake128HashDataAot(
    ReadOnlySpan<byte> bytes, int bytes_length,
    int output_length, out IntPtr output);

  [LibraryImport(DllName, EntryPoint = "shake_256_hash_data_aot")]
  internal static partial CError Shake256HashDataAot(
    ReadOnlySpan<byte> bytes, int bytes_length,
    int output_length, out IntPtr output);

  #endregion Crypto Hash

  #region Crypto Hmac
  [LibraryImport(DllName, EntryPoint = "hmac_sha_256_hash_data_aot")]
  public static partial CError HmacSha256HashDataAot(
    ReadOnlySpan<byte> bytes, int bytes_length,
    ReadOnlySpan<byte> key, int key_length,
    out IntPtr output, out int output_length);

  [LibraryImport(DllName, EntryPoint = "hmac_sha_384_hash_data_aot")]
  public static partial CError HmacSha384HashDataAot(
    ReadOnlySpan<byte> bytes, int bytes_length,
    ReadOnlySpan<byte> key, int key_length,
    out IntPtr output, out int output_length);

  [LibraryImport(DllName, EntryPoint = "hmac_sha_512_hash_data_aot")]
  public static partial CError HmacSha512HashDataAot(
    ReadOnlySpan<byte> bytes, int bytes_length,
    ReadOnlySpan<byte> key, int key_length,
    out IntPtr output, out int output_length);

  [LibraryImport(DllName, EntryPoint = "hmac_sha1_hash_data_aot")]
  public static partial CError HmacSha1HashDataAot(
    ReadOnlySpan<byte> bytes, int bytes_length,
    ReadOnlySpan<byte> key, int key_length,
    out IntPtr output, out int output_length);

  [LibraryImport(DllName, EntryPoint = "hmac_md5_hash_data_aot")]
  public static partial CError HmacMd5HashDataAot(
    ReadOnlySpan<byte> bytes, int bytes_length,
    ReadOnlySpan<byte> key, int key_length,
    out IntPtr output, out int output_length);


  [LibraryImport(DllName, EntryPoint = "hmac_sha3_256_hash_data_aot")]
  public static partial CError HmacSha3256HashDataAot(
    ReadOnlySpan<byte> bytes, int bytes_length,
    ReadOnlySpan<byte> key, int key_length,
    out IntPtr output, out int output_length);

  [LibraryImport(DllName, EntryPoint = "hmac_sha3_384_hash_data_aot")]
  public static partial CError HmacSha3384HashDataAot(
    ReadOnlySpan<byte> bytes, int bytes_length,
    ReadOnlySpan<byte> key, int key_length,
    out IntPtr output, out int output_length);

  [LibraryImport(DllName, EntryPoint = "hmac_sha3_512_hash_data_aot")]
  public static partial CError HmacSha3512HashDataAot(
    ReadOnlySpan<byte> bytes, int bytes_length,
    ReadOnlySpan<byte> key, int key_length,
    out IntPtr output, out int output_length);

  #endregion Crypto Hmac

  #endregion Crypto Hash - Hmac

  #region Crypto Random

  #region Bytes

  [LibraryImport(DllName, EntryPoint = "rng_crypto_bytes_aot")]
  internal static partial CError RngCryptoBytesAot(int size, out IntPtr out_ptr);

  [LibraryImport(DllName, EntryPoint = "fill_crypto_bytes_aot")]
  internal static partial CError FillCryptoBytesAot(ReadOnlySpan<byte> bytes, int length);

  #endregion Bytes

  #region Bools

  [LibraryImport(DllName, EntryPoint = "next_crypto_bool_aot")]
  [return: MarshalAs(UnmanagedType.I1)]
  internal static partial bool NextCryptoBoolAot(out CError err);

  [LibraryImport(DllName, EntryPoint = "rng_crypto_bool_aot")]
  internal static partial CError RngCryptoBoolAot(int size, out IntPtr out_ptr);

  #endregion Bools

  #region Ints32s

  [LibraryImport(DllName, EntryPoint = "next_crypto_int32_aot")]
  internal static partial int NextCryptoInt32Aot(out CError err);

  [LibraryImport(DllName, EntryPoint = "next_crypto_int32_max_aot")]
  internal static partial int NextCryptoInt32MaxAot(int max, out CError err);

  [LibraryImport(DllName, EntryPoint = "next_crypto_int32_min_max_aot")]
  internal static partial int NextCryptoInt32MinMaxAot(int min, int max, out CError err);

  [LibraryImport(DllName, EntryPoint = "rng_crypto_int32_aot")]
  internal static partial CError RngCryptoInt32Aot(int size, out IntPtr out_ptr);

  [LibraryImport(DllName, EntryPoint = "rng_crypto_int32_max_aot")]
  internal static partial CError RngCryptoInt32MaxAot(int size, int max, out IntPtr out_ptr);

  [LibraryImport(DllName, EntryPoint = "rng_crypto_int32_min_max_aot")]
  internal static partial CError RngCryptoInt32MinMaxAot(int size, int min, int max, out IntPtr out_ptr);

  #endregion Ints32s

  #region Ints64s

  [LibraryImport(DllName, EntryPoint = "next_crypto_int64_aot")]
  internal static partial long NextCryptoInt64Aot(out CError err);

  [LibraryImport(DllName, EntryPoint = "next_crypto_int64_max_aot")]
  internal static partial long NextCryptoInt64MaxAot(long max, out CError err);

  [LibraryImport(DllName, EntryPoint = "next_crypto_int64_min_max_aot")]
  internal static partial long NextCryptoInt64MinMaxAot(long min, long max, out CError err);

  [LibraryImport(DllName, EntryPoint = "rng_crypto_int64_aot")]
  internal static partial CError RngCryptoInt64Aot(int size, out IntPtr out_ptr);

  [LibraryImport(DllName, EntryPoint = "rng_crypto_int64_max_aot")]
  internal static partial CError RngCryptoInt64MaxAot(int size, long max, out IntPtr out_ptr);

  [LibraryImport(DllName, EntryPoint = "rng_crypto_int64_min_max_aot")]
  internal static partial CError RngCryptoInt64MinMaxAot(int size, long min, long max, out IntPtr out_ptr);

  #endregion Ints64s

  #region Doubles

  [LibraryImport(DllName, EntryPoint = "next_crypto_double_aot")]
  internal static partial double NextCryptoDoubleAot(out CError err);

  [LibraryImport(DllName, EntryPoint = "next_crypto_double_max_aot")]
  internal static partial double NextCryptoDoubleMaxAot(double max, out CError err);

  [LibraryImport(DllName, EntryPoint = "next_crypto_double_min_max_aot")]
  internal static partial double NextCryptoDoubleMinMaxAot(double min, double max, out CError err);

  [LibraryImport(DllName, EntryPoint = "rng_crypto_double_aot")]
  internal static partial CError RngCryptoDoubleAot(int size, out IntPtr out_ptr);

  [LibraryImport(DllName, EntryPoint = "rng_crypto_double_max_aot")]
  internal static partial CError RngCryptoDoubleMaxAot(int size, double max, out IntPtr out_ptr);

  [LibraryImport(DllName, EntryPoint = "rng_crypto_double_min_max_aot")]
  internal static partial CError RngCryptoDoubleMinMaxAot(int size, double min, double max, out IntPtr out_ptr);

  #endregion Doubles

  # region Singles

  [LibraryImport(DllName, EntryPoint = "next_crypto_single_aot")]
  internal static partial float NextCryptoSingleAot(out CError err);

  [LibraryImport(DllName, EntryPoint = "next_crypto_single_max_aot")]
  internal static partial float NextCryptoSingleMaxAot(float max, out CError err);

  [LibraryImport(DllName, EntryPoint = "next_crypto_single_min_max_aot")]
  internal static partial float NextCryptoSingleMinMaxAot(float min, float max, out CError err);

  [LibraryImport(DllName, EntryPoint = "rng_crypto_single_aot")]
  internal static partial CError RngCryptoSingleAot(int size, out IntPtr out_ptr);

  [LibraryImport(DllName, EntryPoint = "rng_crypto_single_max_aot")]
  internal static partial CError RngCryptoSingleMaxAot(int size, float max, out IntPtr out_ptr);

  [LibraryImport(DllName, EntryPoint = "rng_crypto_single_min_max_aot")]
  internal static partial CError RngCryptoSingleMinMaxAot(int size, float min, float max, out IntPtr out_ptr);

  #endregion Singles

  #region Decimals

  [LibraryImport(DllName, EntryPoint = "next_crypto_decimal_aot")]
  internal static partial decimal NextCryptoDecimalAot(out CError err);

  [LibraryImport(DllName, EntryPoint = "next_crypto_decimal_max_aot")]
  internal static partial decimal NextCryptoDecimalMaxAot(decimal max, out CError err);

  [LibraryImport(DllName, EntryPoint = "next_crypto_decimal_min_max_aot")]
  internal static partial decimal NextCryptoDecimalMinMaxAot(decimal min, decimal max, out CError err);

  [LibraryImport(DllName, EntryPoint = "rng_crypto_decimal_aot")]
  internal static partial CError RngCryptoDecimalAot(int size, out IntPtr out_ptr);

  [LibraryImport(DllName, EntryPoint = "rng_crypto_decimal_max_aot")]
  internal static partial CError RngCryptoDecimalMaxAot(int size, decimal max, out IntPtr out_ptr);

  [LibraryImport(DllName, EntryPoint = "rng_crypto_decimal_min_max_aot")]
  internal static partial CError RngCryptoDecimalMinMaxAot(int size, decimal min, decimal max, out IntPtr out_ptr);

  #endregion Decimals

  #endregion Crypto Random

  #region Crypto PQC

  #region Crypto PQC Cryption

  #region Crypto PQC Cryption ML-KEM

  #region Crypto PQC Cryption ML-KEM Bytes

  [LibraryImport(DllName, EntryPoint = "create_mlkem_key_pair_aot")]
  internal static partial CError CreateMlKemKeyPairAot(
    out IntPtr priv_key_ptr, out int priv_key_length,
    out IntPtr pub_key_ptr, out int pub_key_length,
    out IntPtr guid_id_ptr, out int guid_id_length,
    out byte mlkem_param, out byte crypto_algo);

  [LibraryImport(DllName, EntryPoint = "create_mlkem_key_pair_param_aot")]
  internal static partial CError CreateMlKemKeyPairParamAot(
    byte mlkem_param, byte crypto_algo,
    out IntPtr priv_key_ptr, out int priv_key_length,
    out IntPtr pub_key_ptr, out int pub_key_length,
    out IntPtr guid_id_ptr, out int guid_id_length);

  [LibraryImport(DllName, EntryPoint = "save_pqc_mlkem_key_pair_aot")]
  internal static partial CError SavePqcMlKemKeyPairAot(
    ReadOnlySpan<byte> src, int src_length,
    ReadOnlySpan<byte> priv_key, int priv_key_length,
    ReadOnlySpan<byte> pub_key, int pub_key_length,
    ReadOnlySpan<byte> guid_id, int guid_id_length,
    byte mlkem_param, byte crypto_algo,
    [MarshalAs(UnmanagedType.U1)] bool save_private_key);

  [LibraryImport(DllName, EntryPoint = "load_pqc_mlkem_key_pair_aot")]
  internal static partial CError LoadPqcMlKemKeyPairAot(
    ReadOnlySpan<byte> src, int src_length,
    out IntPtr priv_key_ptr, out int priv_key_length,
    out IntPtr pub_key_ptr, out int pub_key_length,
    out IntPtr guid_id_ptr, out int guid_id_length,
    out byte mlkem_param, out byte crypto_algo);

  [LibraryImport(DllName, EntryPoint = "to_pqc_mlkem_capsulation_from_pub_key_aot")]
  internal static partial CError ToPqcMlKemCapsulationFromPubKeyAot(
    ReadOnlySpan<byte> alice_public_key, int alice_public_key_length,
    byte mlkem_param,
    out IntPtr shared_key_ptr, out int shared_key_length,
    out IntPtr capsulation_ptr, out int capsulation_length);

  [LibraryImport(DllName, EntryPoint = "to_pqc_mlkem_shared_key_from_private_key_aot")]
  internal static partial CError ToPqcMlKemSharedKeyFromPrivateKeyAot(
    ReadOnlySpan<byte> alice_private_key, int alice_private_key_length,
    ReadOnlySpan<byte> capsulation, int capsulation_length,
    byte mlkem_param, out IntPtr shared, out int shared_length);

  [LibraryImport(DllName, EntryPoint = "pqc_mlkem_encryption_aot")]
  internal static partial CError PqcMlKemEncryptionAot(
    ReadOnlySpan<byte> message, int message_length,
    ReadOnlySpan<byte> private_key, int private_key_length,
    ReadOnlySpan<byte> capsulation, int capsulation_length,
    ReadOnlySpan<byte> associated, int associated_length,
    byte mlkem_param, byte crypto_algo,
    out IntPtr cipher, out int cipher_length);

  [LibraryImport(DllName, EntryPoint = "pqc_mlkem_decryption_aot")]
  internal static partial CError PqcMlKemDecryptionAot(
    ReadOnlySpan<byte> cipher, int cipher_length,
    ReadOnlySpan<byte> shared_key, int shared_key_length,
    ReadOnlySpan<byte> associated, int associated_length,
    byte mlkem_param, byte crypto_algo,
    out IntPtr decipher_ptr, out int decipher_length);

  #endregion Crypto PQC Cryption ML-KEM Bytes

  #region Crypto PQC Cryption ML-KEM Files

  [LibraryImport(DllName, EntryPoint = "pqc_mlkem_encryption_file_aot")]
  internal static partial CError PqcMlKemEncryptionFileAot(
    ReadOnlySpan<byte> src_file, int src_file_length,
    ReadOnlySpan<byte> dest_file, int dest_file_length,
    ReadOnlySpan<byte> private_key, int private_key_length,
    ReadOnlySpan<byte> capsulation, int capsulation_length,
    ReadOnlySpan<byte> associated, int associated_length,
    byte mlkem_param, byte crypto_algo);

  [LibraryImport(DllName, EntryPoint = "pqc_mlkem_decryption_file_aot")]
  internal static partial CError PqcMlKemDecryptionFileAot(
    ReadOnlySpan<byte> src_file, int src_file_length,
    ReadOnlySpan<byte> dest_file, int dest_file_length,
    ReadOnlySpan<byte> shared_key, int shared_key_length,
    ReadOnlySpan<byte> associated, int associated_length);


  //[LibraryImport(DllName, EntryPoint = "pqc_mlkem_encryption_kpi_file_aot")]
  //internal static partial CError PqcMlKemEncryptionKpiFileAot(
  //  ReadOnlySpan<byte> src_file, int src_file_length,
  //  ReadOnlySpan<byte> dest_file, int dest_file_length,
  //  ReadOnlySpan<byte> key_pair_file, int key_pair_file_length,
  //  ReadOnlySpan<byte> associated, int associated_length);

  //[LibraryImport(DllName, EntryPoint = "pqc_mlkem_decryption_kpi_file_aot")]
  //internal static partial CError PqcMlKemDecryptionKpiFileAot(
  //  ReadOnlySpan<byte> src_file, int src_file_length,
  //  ReadOnlySpan<byte> dest_file, int dest_file_length,
  //  ReadOnlySpan<byte> key_pair_file, int key_pair_file_length,
  //  ReadOnlySpan<byte> associated, int associated_length);

  #endregion Crypto PQC Cryption ML-KEM Files

  #endregion Crypto PQC Cryption ML-KEM

  #endregion Crypto PQC Cryption

  #region Crypto PQC Signatur

  #region Crypto PQC Signatur Stateless

  #region Crypto PQC Signatur ML-DSA

  #region Crypto PQC Signatur ML-DSA Bytes

  [LibraryImport(DllName, EntryPoint = "create_mldsa_key_pair_aot")]
  internal static partial CError CreateMlDsaKeyPairAot(
   out IntPtr priv_key_ptr, out int priv_key_length,
   out IntPtr pub_key_ptr, out int pub_key_length,
   out IntPtr guid_id_ptr, out int guid_id_length,
   out byte mldsa_param);

  [LibraryImport(DllName, EntryPoint = "create_mldsa_key_pair_param_aot")]
  internal static partial CError CreateMlDsaKeyPairParamAot(
    byte mldsa_param,
    out IntPtr priv_key_ptr, out int priv_key_length,
    out IntPtr pub_key_ptr, out int pub_key_length,
    out IntPtr guid_id_ptr, out int guid_id_length);

  [LibraryImport(DllName, EntryPoint = "save_pqc_mldsa_key_pair_aot")]
  internal static partial CError SavePqcMlDsaKeyPairAot(
    ReadOnlySpan<byte> src, int src_length,
    ReadOnlySpan<byte> priv_key, int priv_key_length,
    ReadOnlySpan<byte> pub_key, int pub_key_length,
    ReadOnlySpan<byte> guid_id, int guid_id_length,
    byte mldsa_param,
    [MarshalAs(UnmanagedType.U1)] bool save_private_key);

  [LibraryImport(DllName, EntryPoint = "load_pqc_mldsa_key_pair_aot")]
  internal static partial CError LoadPqcMlDsaKeyPairAot(
    ReadOnlySpan<byte> src, int src_length,
    out IntPtr priv_key_ptr, out int priv_key_length,
    out IntPtr pub_key_ptr, out int pub_key_length,
    out IntPtr guid_id_ptr, out int guid_id_length,
    out byte mldsa_param);

  [LibraryImport(DllName, EntryPoint = "pqc_mldsa_sign_aot")]
  internal static partial CError PqcMlDsaSignAot(
    ReadOnlySpan<byte> message_ptr, int message_length,
    ReadOnlySpan<byte> priv_key_ptr, int priv_key_length,
    byte mldsa_param, out IntPtr sign_ptr, out int sign_length);

  [LibraryImport(DllName, EntryPoint = "pqc_mldsa_verify_aot")]
  [return: MarshalAs(UnmanagedType.I1)]
  internal static partial bool PqcMlDsaVerifyAot(
    ReadOnlySpan<byte> message_ptr, int message_length,
    ReadOnlySpan<byte> public_key_ptr, int public_key_length,
    ReadOnlySpan<byte> signature_ptr, int signature_length,
    out CError cerror);

  #endregion Crypto PQC Signatur ML-DSA Bytes

  #region Crypto PQC Signatur ML-DSA Files

  [LibraryImport(DllName, EntryPoint = "pqc_mldsa_sign_file_aot")]
  internal static partial CError PqcMlDsaSignFileAot(
    ReadOnlySpan<byte> src_file_ptr, int src_file_length,
    ReadOnlySpan<byte> private_key_ptr, int private_key_length,
    byte mldsa_param, out IntPtr sign_ptr, out int sign_length);

  [LibraryImport(DllName, EntryPoint = "pqc_mldsa_verify_file_aot")]
  [return: MarshalAs(UnmanagedType.U1)]
  internal static partial bool PqcMlDsaVerifyFileAot(
    ReadOnlySpan<byte> src_file_ptr, int src_file_length,
    ReadOnlySpan<byte> public_key_ptr, int public_key_length,
    ReadOnlySpan<byte> signature_ptr, int signature_length,
    out CError cerror);

  #endregion Crypto PQC Signatur ML-DSA Files

  #region Crypto PQC Multi Signatur ML-DSA

  [LibraryImport(DllName, EntryPoint = "pqc_mldsa_multi_sign_aot")]
  internal static unsafe partial CError PqcMlDsaMultiSignAot(
      byte* message_ptr, int message_length,

      byte** guid_ptr, int* guid_length, int guid_count,
      byte** sign_ptr, int* sign_length, int sign_count,
      byte** public_key_ptr, int* public_key_length, int public_key_count,
      byte** signer_name_ptr, int* signer_name_length, int signer_name_count,
      byte** project_name_ptr, int* project_name_length, int project_name_count,

      byte* sign_algo_ptr, int sign_algo_count,
      byte* mldsa_param_ptr, int mldsa_param_count,

      out IntPtr multi_sign_ptr, out int multi_sign_length,
      out IntPtr multi_private_key_ptr, out int multi_private_key_length,
      out IntPtr multi_public_key_ptr, out int multi_public_key_length);



  [LibraryImport(DllName, EntryPoint = "pqc_mldsa_multi_sign_file_aot")]
  internal static unsafe partial CError PqcMlDsaMultiSignFileAot(
      byte* src_file_ptr, int src_file_length,

      byte** guid_ptr, int* guid_length, int guid_count,
      byte** sign_ptr, int* sign_length, int sign_count,
      byte** public_key_ptr, int* public_key_length, int public_key_count,
      byte** signer_name_ptr, int* signer_name_length, int signer_name_count,
      byte** project_name_ptr, int* project_name_length, int project_name_count,

      byte* sign_algo_ptr, int sign_algo_count,
      byte* mldsa_param_ptr, int mldsa_param_count,

      out IntPtr multi_sign_ptr, out int multi_sign_length,
      out IntPtr multi_private_key_ptr, out int multi_private_key_length,
      out IntPtr multi_public_key_ptr, out int multi_public_key_length);

  #endregion Crypto PQC Multi Signatur ML-DSA

  #endregion Crypto PQC Signatur ML-DSA

  #region Crypto PQC Signatur SLH-DSA

  #region Crypto PQC Signatur SLH-DSA Bytes

  //stuff

  #endregion Crypto PQC Signatur SLH-DSA Bytes

  #region Crypto PQC Signatur SLH-DSA Files

  //stuff

  #endregion Crypto PQC Signatur SLH-DSA Files

  #endregion Crypto PQC Signatur SLH-DSA

  #endregion Crypto PQC Signatur Stateless

  #endregion Crypto PQC Signatur

  #endregion Crypto PQC

  #region Convert - Encoding

  #region Base64

  [LibraryImport(DllName, EntryPoint = "to_base_64_utf8_aot")]
  internal static partial CError ToBase64Aot(
    ReadOnlySpan<byte> bytes, int bytes_length,
    out IntPtr output, out int output_length);

  [LibraryImport(DllName, EntryPoint = "from_base_64_utf8_aot")]
  public static partial CError FromBase64Aot(
    ReadOnlySpan<byte> bytes, int bytes_length,
    out IntPtr output, out int output_length);

  #endregion Base64

  #region Hex

  [LibraryImport(DllName, EntryPoint = "to_hex_utf8_aot")]
  internal static partial CError ToHexAot(
    ReadOnlySpan<byte> bytes, int bytes_length,
    out IntPtr output, out int output_length,
    [MarshalAs(UnmanagedType.U1)] bool lower = true);

  [LibraryImport(DllName, EntryPoint = "from_hex_utf8_aot")]
  public static partial CError FromHexAot(
    ReadOnlySpan<byte> bytes, int bytes_length,
    out IntPtr output, out int output_length);

  #endregion Hex

  #region BaseX

  [LibraryImport(DllName, EntryPoint = "converter_2_256_le_aot")]
  internal static partial CError Converter_2_256_LE_Aot(
    ReadOnlySpan<byte> base_x_le, int base_x_length,
    int start_base, int target_base,
    out IntPtr output, out int output_length);

  [LibraryImport(DllName, EntryPoint = "to_base_x_2_256_le_aot")]
  internal static partial CError ToBaseX_2_256_LE_Aot(
    ReadOnlySpan<byte> bytes_base10_le, int bytes_length, int target_base,
    out IntPtr output, out int output_length);

  [LibraryImport(DllName, EntryPoint = "to_base_x_utf8_2_256_le_aot")]
  internal static partial CError ToBaseXUtf8_2_256_LE_Aot(
    ReadOnlySpan<byte> bytes_base10_utf8_le, int bytes_utf8_length, int target_base,
    out IntPtr output, out int output_length);

  [LibraryImport(DllName, EntryPoint = "from_base_x_2_256_le_aot")]
  internal static partial CError FromBaseX_2_256_LE_Aot(
    ReadOnlySpan<byte> bytes_basex_le, int bytes_length, int from_base_x,
    out IntPtr output, out int output_length);

  #endregion BaseX

  #endregion Convert - Encoding

  #region Compress


  #endregion Compress

  #region Serialize


  #endregion Serialize
}
