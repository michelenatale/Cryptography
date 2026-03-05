


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

  #region Crypto

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

  #endregion Crypto

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

  #region Compress


  #endregion Compress

  #region Serialize


  #endregion Serialize
}
