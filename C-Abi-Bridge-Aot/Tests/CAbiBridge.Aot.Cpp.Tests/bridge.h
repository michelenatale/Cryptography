#pragma once

#include <cstdint>
#include <cstddef>

#include "cerror.h"
#include "cabi_exp_imp.h"

#ifndef __BRIDGE_AOT_H__
#define __BRIDGE_AOT_H__

//#pragma comment(lib, "C-Abi-Bridge.Aot.lib")

// ---------------------------------------------------------
// Allocation
// ---------------------------------------------------------

CABI_IMPORT void free_buffer_aot(void* ptr);

// ---------------------------------------------------------
// PQC Utils
// ---------------------------------------------------------

CABI_IMPORT bool equal_files_aot(
  const uint8_t* file_path_left_ptr, int file_path_left_length,
  const uint8_t* file_path_right_ptr, int file_path_right_length,
  cerror_t* cerror);

// ---------------------------------------------------------
// Rfc2898DeriveBytes.Pbkdf2
// ---------------------------------------------------------

CABI_IMPORT cerror_t pbkdf2_aot(
  const uint8_t* pw, int pwLen,
  const uint8_t* salt, int saltLen,
  int iterations,
  uint8_t* output, int outputLen);

// ---------------------------------------------------------
// AES
// ---------------------------------------------------------

CABI_IMPORT cerror_t aes_encrypt_aot(
  const std::uint8_t* bytes, int bytes_length,
  const void* key, int key_length,
  const std::uint8_t* associated, int associated_length,
  void** output, int* output_length);

CABI_IMPORT cerror_t aes_decrypt_aot(
  const std::uint8_t* bytes, int bytes_length,
  const void* key, int key_length,
  const std::uint8_t* associated, int associated_length,
  void** output, int* output_length);

CABI_IMPORT cerror_t aes_encrypt_file_aot(
  const std::uint8_t* src, int src_length,
  const std::uint8_t* dest, int dest_length,
  const void* key, int key_length,
  const std::uint8_t* associated, int associated_length);

CABI_IMPORT cerror_t aes_decrypt_file_aot(
  const std::uint8_t* src, int src_length,
  const std::uint8_t* dest, int dest_length,
  const void* key, int key_length,
  const std::uint8_t* associated, int associated_length);

// ---------------------------------------------------------
// AES‑GCM
// ---------------------------------------------------------

CABI_IMPORT cerror_t aes_gcm_encrypt_aot(
  const std::uint8_t* bytes, int bytes_length,
  const void* key, int key_length,
  const std::uint8_t* associated, int associated_length,
  void** output, int* output_length);

CABI_IMPORT cerror_t aes_gcm_decrypt_aot(
  const std::uint8_t* bytes, int bytes_length,
  const void* key, int key_length,
  const std::uint8_t* associated, int associated_length,
  void** output, int* output_length);

CABI_IMPORT cerror_t aes_gcm_encrypt_file_aot(
  const std::uint8_t* src, int src_length,
  const std::uint8_t* dest, int dest_length,
  const void* key, int key_length,
  const std::uint8_t* associated, int associated_length);

CABI_IMPORT cerror_t aes_gcm_decrypt_file_aot(
  const std::uint8_t* src, int src_length,
  const std::uint8_t* dest, int dest_length,
  const void* key, int key_length,
  const std::uint8_t* associated, int associated_length);

// ---------------------------------------------------------
// ChaCha20‑Poly1305
// ---------------------------------------------------------

CABI_IMPORT cerror_t chacha20_poly1305_encrypt_aot(
  const std::uint8_t* bytes, int bytes_length,
  const void* key, int key_length,
  const std::uint8_t* associated, int associated_length,
  void** output, int* output_length);

CABI_IMPORT cerror_t chacha20_poly1305_decrypt_aot(
  const std::uint8_t* bytes, int bytes_length,
  const void* key, int key_length,
  const std::uint8_t* associated, int associated_length,
  void** output, int* output_length);

CABI_IMPORT cerror_t chacha20_poly1305_encrypt_file_aot(
  const std::uint8_t* src, int src_length,
  const std::uint8_t* dest, int dest_length,
  const void* key, int key_length,
  const std::uint8_t* associated, int associated_length);

CABI_IMPORT cerror_t chacha20_poly1305_decrypt_file_aot(
  const std::uint8_t* src, int src_length,
  const std::uint8_t* dest, int dest_length,
  const void* key, int key_length,
  const std::uint8_t* associated, int associated_length);

// ---------------------------------------------------------
// Crypto Random
// ---------------------------------------------------------

// Bytes
CABI_IMPORT cerror_t rng_crypto_bytes_aot(int size, void** out_ptr);
CABI_IMPORT cerror_t fill_crypto_bytes_aot(const std::uint8_t* bytes, int length);

// Bools
CABI_IMPORT bool next_crypto_bool_aot(cerror_t* err);
CABI_IMPORT cerror_t rng_crypto_bool_aot(int size, void** out_ptr);

// Int32
CABI_IMPORT int next_crypto_int32_aot(cerror_t* err);
CABI_IMPORT int next_crypto_int32_max_aot(int max, cerror_t* err);
CABI_IMPORT int next_crypto_int32_min_max_aot(int min, int max, cerror_t* err);
CABI_IMPORT cerror_t rng_crypto_int32_aot(int size, void** out_ptr);
CABI_IMPORT cerror_t rng_crypto_int32_max_aot(int size, int max, void** out_ptr);
CABI_IMPORT cerror_t rng_crypto_int32_min_max_aot(int size, int min, int max, void** out_ptr);

// Int64
CABI_IMPORT int64_t next_crypto_int64_aot(cerror_t* err);
CABI_IMPORT int64_t next_crypto_int64_max_aot(int64_t max, cerror_t* err);
CABI_IMPORT int64_t next_crypto_int64_min_max_aot(int64_t min, int64_t max, cerror_t* err);
CABI_IMPORT cerror_t rng_crypto_int64_aot(int size, void** out_ptr);
CABI_IMPORT cerror_t rng_crypto_int64_max_aot(int size, int64_t max, void** out_ptr);
CABI_IMPORT cerror_t rng_crypto_int64_min_max_aot(int size, int64_t min, int64_t max, void** out_ptr);

// Double
CABI_IMPORT double next_crypto_double_aot(cerror_t* err);
CABI_IMPORT double next_crypto_double_max_aot(double max, cerror_t* err);
CABI_IMPORT double next_crypto_double_min_max_aot(double min, double max, cerror_t* err);
CABI_IMPORT cerror_t rng_crypto_double_aot(int size, void** out_ptr);
CABI_IMPORT cerror_t rng_crypto_double_max_aot(int size, double max, void** out_ptr);
CABI_IMPORT cerror_t rng_crypto_double_min_max_aot(int size, double min, double max, void** out_ptr);

// Float
CABI_IMPORT float next_crypto_single_aot(cerror_t* err);
CABI_IMPORT float next_crypto_single_max_aot(float max, cerror_t* err);
CABI_IMPORT float next_crypto_single_min_max_aot(float min, float max, cerror_t* err);
CABI_IMPORT cerror_t rng_crypto_single_aot(int size, void** out_ptr);
CABI_IMPORT cerror_t rng_crypto_single_max_aot(int size, float max, void** out_ptr);
CABI_IMPORT cerror_t rng_crypto_single_min_max_aot(int size, float min, float max, void** out_ptr);

//// Decimal (C# decimal → 128‑bit struct → hier als 16‑Byte Buffer)
//struct Decimal128 { std::uint8_t bytes[16]; };
//
//CABI_IMPORT Decimal128 next_crypto_decimal_aot(cerror_t* err);
//CABI_IMPORT Decimal128 next_crypto_decimal_max_aot(Decimal128 max, cerror_t* err);
//CABI_IMPORT Decimal128 next_crypto_decimal_min_max_aot(Decimal128 min, Decimal128 max, cerror_t* err);
//CABI_IMPORT cerror_t rng_crypto_decimal_aot(int size, void** out_ptr);
//CABI_IMPORT cerror_t rng_crypto_decimal_max_aot(int size, Decimal128 max, void** out_ptr);
//CABI_IMPORT cerror_t rng_crypto_decimal_min_max_aot(int size, Decimal128 min, Decimal128 max, void** out_ptr);

// ---------------------------------------------------------
// Convert Encoding
// ---------------------------------------------------------

CABI_IMPORT cerror_t to_base_64_utf8_aot(const uint8_t* data, int len, uint8_t** out_ptr, int* out_len);
CABI_IMPORT cerror_t from_base_64_utf8_aot(const uint8_t* data, int len, uint8_t** out_ptr, int* out_len);
CABI_IMPORT cerror_t to_hex_utf8_aot(const uint8_t* data, int len, uint8_t** out_ptr, int* out_len, bool lower);
CABI_IMPORT cerror_t from_hex_utf8_aot(const uint8_t* data, int len, uint8_t** out_ptr, int* out_len);

CABI_IMPORT cerror_t  converter_2_256_le_aot(const uint8_t* base_x_le, int base_x_length, int start_base, int target_base, uint8_t** output, int* output_length);
CABI_IMPORT cerror_t  to_base_x_2_256_le_aot(const uint8_t* bytes_base10_le, int bytes_length, int target_base, uint8_t** output, int* output_length);
CABI_IMPORT cerror_t  to_base_x_utf8_2_256_le_aot(const uint8_t* bytes_base10_utf8_le, int bytes_utf8_length, int target_base, uint8_t** output, int* output_length);
CABI_IMPORT cerror_t  from_base_x_2_256_le_aot(const uint8_t* bytes_basex_le, int bytes_length, int from_base_x, uint8_t** output, int* output_length);

// ---------------------------------------------------------
// Hash / Hmac
// ---------------------------------------------------------

CABI_IMPORT cerror_t md5_hash_data_aot(const uint8_t*, int, uint8_t**, int*);
CABI_IMPORT cerror_t sha1_hash_data_aot(const uint8_t*, int, uint8_t**, int*);

CABI_IMPORT cerror_t sha_256_hash_data_aot(const uint8_t*, int, uint8_t**, int*);
CABI_IMPORT cerror_t sha_384_hash_data_aot(const uint8_t*, int, uint8_t**, int*);
CABI_IMPORT cerror_t sha_512_hash_data_aot(const uint8_t*, int, uint8_t**, int*);

CABI_IMPORT cerror_t sha3_256_hash_data_aot(const uint8_t*, int, uint8_t**, int*);
CABI_IMPORT cerror_t sha3_384_hash_data_aot(const uint8_t*, int, uint8_t**, int*);
CABI_IMPORT cerror_t sha3_512_hash_data_aot(const uint8_t*, int, uint8_t**, int*);

CABI_IMPORT cerror_t shake_128_hash_data_aot(const uint8_t*, int, int, uint8_t**);
CABI_IMPORT cerror_t shake_256_hash_data_aot(const uint8_t*, int, int, uint8_t**);

CABI_IMPORT cerror_t hmac_md5_hash_data_aot(const uint8_t*, int, const uint8_t*, int, uint8_t**, int*);
CABI_IMPORT cerror_t hmac_sha1_hash_data_aot(const uint8_t*, int, const uint8_t*, int, uint8_t**, int*);

CABI_IMPORT cerror_t hmac_sha_256_hash_data_aot(const uint8_t*, int, const uint8_t*, int, uint8_t**, int*);
CABI_IMPORT cerror_t hmac_sha_384_hash_data_aot(const uint8_t*, int, const uint8_t*, int, uint8_t**, int*);
CABI_IMPORT cerror_t hmac_sha_512_hash_data_aot(const uint8_t*, int, const uint8_t*, int, uint8_t**, int*);

CABI_IMPORT cerror_t hmac_sha3_256_hash_data_aot(const uint8_t*, int, const uint8_t*, int, uint8_t**, int*);
CABI_IMPORT cerror_t hmac_sha3_384_hash_data_aot(const uint8_t*, int, const uint8_t*, int, uint8_t**, int*);
CABI_IMPORT cerror_t hmac_sha3_512_hash_data_aot(const uint8_t*, int, const uint8_t*, int, uint8_t**, int*);

// ---------------------------------------------------------
// PQC Ml-Kem
// ---------------------------------------------------------

CABI_IMPORT cerror_t create_mlkem_key_pair_aot(
  uint8_t** priv_key_ptr, int* priv_key_length,
  uint8_t** pub_key_ptr, int* pub_key_length,
  uint8_t** guid_id_ptr, int* guid_id_length,
  uint8_t* mlkem_param, uint8_t* crypto_algo);

CABI_IMPORT cerror_t create_mlkem_key_pair_param_aot(
  uint8_t mlkem_param, uint8_t crypto_algo,
  uint8_t** priv_key_ptr, int* priv_key_length,
  uint8_t** pub_key_ptr, int* pub_key_length,
  uint8_t** guid_id_ptr, int* guid_id_length);

CABI_IMPORT cerror_t save_pqc_mlkem_key_pair_aot(
  const uint8_t* file, int file_len,
  const uint8_t* priv_key, int priv_key_len,
  const uint8_t* pub_key, int pub_key_len,
  const uint8_t* guid, int guid_len,
  uint8_t mlkem_param, uint8_t crypto_algo,
  int with_priv_key);

CABI_IMPORT cerror_t load_pqc_mlkem_key_pair_aot(
  const uint8_t* file, int file_len,
  uint8_t** priv_key_ptr, int* priv_key_length,
  uint8_t** pub_key_ptr, int* pub_key_length,
  uint8_t** guid_id_ptr, int* guid_id_length,
  uint8_t* mlkem_param, uint8_t* crypto_algo);

CABI_IMPORT cerror_t to_pqc_mlkem_capsulation_from_pub_key_aot(
  const uint8_t* pub_key, int pub_key_len,
  uint8_t mlkem_param,
  uint8_t** shared_key_ptr, int* shared_key_len,
  uint8_t** caps_ptr, int* caps_len);

CABI_IMPORT cerror_t to_pqc_mlkem_shared_key_from_private_key_aot(
  const uint8_t* priv_key, int priv_key_len,
  const uint8_t* caps, int caps_len,
  uint8_t mlkem_param,
  uint8_t** shared_key_ptr, int* shared_key_len);

CABI_IMPORT cerror_t pqc_mlkem_encryption_aot(
  const uint8_t* msg, int msg_len,
  const uint8_t* priv_key, int priv_key_len,
  const uint8_t* caps, int caps_len,
  const uint8_t* associated, int associated_len,
  uint8_t mlkem_param, uint8_t crypto_algo,
  uint8_t** cipher_ptr, int* cipher_len);

CABI_IMPORT cerror_t pqc_mlkem_decryption_aot(
  const uint8_t* cipher, int cipher_len,
  const uint8_t* shared_key, int shared_key_len,
  const uint8_t* associated, int associated_len,
  uint8_t mlkem_param, uint8_t crypto_algo,
  uint8_t** plain_ptr, int* plain_len);

CABI_IMPORT cerror_t pqc_mlkem_encryption_file_aot(
  const uint8_t* src_file_ptr, int src_file_length,
  const uint8_t* dest_file_ptr, int dest_file_length,
  const uint8_t* private_key_ptr, int private_key_length,
  const uint8_t* capsulation_ptr, int capsulation_length,
  const uint8_t* associated_ptr, int associated_length,
  uint8_t mlkem_param, uint8_t crypto_algo);

CABI_IMPORT cerror_t pqc_mlkem_decryption_file_aot(
  const uint8_t* src_file_ptr, int src_file_length,
  const uint8_t* dest_file_ptr, int dest_file_length,
  const uint8_t* shared_key_ptr, int shared_key_length,
  const uint8_t* associated_ptr, int associated_length);


// ---------------------------------------------------------
// PQC ML-DSA KeyPair (Bytes)
// ---------------------------------------------------------

CABI_IMPORT cerror_t create_mldsa_key_pair_aot(
  uint8_t** priv_key_ptr, int* priv_key_length,
  uint8_t** pub_key_ptr, int* pub_key_length,
  uint8_t** guid_id_ptr, int* guid_id_length,
  uint8_t* mldsa_param);

CABI_IMPORT cerror_t create_mldsa_key_pair_param_aot(
  uint8_t   mldsa_param,
  uint8_t** priv_key_ptr, int* priv_key_length,
  uint8_t** pub_key_ptr, int* pub_key_length,
  uint8_t** guid_id_ptr, int* guid_id_length);

CABI_IMPORT cerror_t save_pqc_mldsa_key_pair_aot(
  const uint8_t* src, int src_length,
  const uint8_t* priv_key, int priv_key_length,
  const uint8_t* pub_key, int pub_key_length,
  const uint8_t* guid_id, int guid_id_length,
  uint8_t        mldsa_param,
  uint8_t        save_private_key);

CABI_IMPORT cerror_t load_pqc_mldsa_key_pair_aot(
  const uint8_t* src, int src_length,
  uint8_t** priv_key_ptr, int* priv_key_length,
  uint8_t** pub_key_ptr, int* pub_key_length,
  uint8_t** guid_id_ptr, int* guid_id_length,
  uint8_t* mldsa_param);

// ---------------------------------------------------------
// PQC ML-DSA Sign / Verify (Bytes)
// ---------------------------------------------------------

CABI_IMPORT cerror_t pqc_mldsa_sign_aot(
  const uint8_t* message_ptr, int message_length,
  const uint8_t* priv_key_ptr, int priv_key_length,
  uint8_t mldsa_param,
  uint8_t** sign_ptr, int* sign_length);

CABI_IMPORT uint8_t pqc_mldsa_verify_aot(
  const uint8_t* message_ptr, int message_length,
  const uint8_t* public_key_ptr, int public_key_length,
  const uint8_t* signature_ptr, int signature_length,
  cerror_t* cerror);

// ---------------------------------------------------------
// PQC ML-DSA Sign / Verify (Files)
// ---------------------------------------------------------

CABI_IMPORT cerror_t pqc_mldsa_sign_file_aot(
  const uint8_t* src_file_ptr, int src_file_length,
  const uint8_t* private_key_ptr, int private_key_length,
  uint8_t mldsa_param,
  uint8_t** sign_ptr, int* sign_length);

CABI_IMPORT uint8_t pqc_mldsa_verify_file_aot(
  const uint8_t* src_file_ptr, int src_file_length,
  const uint8_t* public_key_ptr, int public_key_length,
  const uint8_t* signature_ptr, int signature_length,
  cerror_t* cerror);

// ---------------------------------------------------------
// PQC ML-DSA Multi-Sign
// ---------------------------------------------------------

CABI_IMPORT cerror_t pqc_mldsa_multi_sign_aot(
  const uint8_t* message_ptr, int message_length,

  uint8_t** guid_ptr, int* guid_length, int guid_count,
  uint8_t** sign_ptr, int* sign_length, int sign_count,
  uint8_t** public_key_ptr, int* public_key_length, int public_key_count,
  uint8_t** signer_name_ptr, int* signer_name_length, int signer_name_count,
  uint8_t** project_name_ptr, int* project_name_length, int project_name_count,

  const uint8_t* sign_algo_ptr, int sign_algo_count,
  const uint8_t* mldsa_param_ptr, int mldsa_param_count,

  uint8_t** multi_sign_ptr, int* multi_sign_length,
  uint8_t** multi_private_key_ptr, int* multi_private_key_length,
  uint8_t** multi_public_key_ptr, int* multi_public_key_length);

CABI_IMPORT cerror_t pqc_mldsa_multi_sign_file_aot(
  const uint8_t* src_file_ptr, int src_file_length,

  uint8_t** guid_ptr, int* guid_length, int guid_count,
  uint8_t** sign_ptr, int* sign_length, int sign_count,
  uint8_t** public_key_ptr, int* public_key_length, int public_key_count,
  uint8_t** signer_name_ptr, int* signer_name_length, int signer_name_count,
  uint8_t** project_name_ptr, int* project_name_length, int project_name_count,

  const uint8_t* sign_algo_ptr, int sign_algo_count,
  const uint8_t* mldsa_param_ptr, int mldsa_param_count,

  uint8_t** multi_sign_ptr, int* multi_sign_length,
  uint8_t** multi_private_key_ptr, int* multi_private_key_length,
  uint8_t** multi_public_key_ptr, int* multi_public_key_length);


// ---------------------------------------------------------
// Crypto Primes
// ---------------------------------------------------------

CABI_IMPORT uint64_t next_crypto_primes_min_max_uint64_aot(
  int32_t miller_rabin_rounds,
  uint64_t min, uint64_t max,
  cerror_t* err);

CABI_IMPORT cerror_t next_crypto_primes_min_max_aot(
  int32_t miller_rabin_rounds,
  uint8_t* min, int32_t min_length,
  uint8_t* max, int32_t max_length,
  uint8_t** output, int32_t* output_length);

CABI_IMPORT cerror_t next_crypto_primes_aot(
  int32_t miller_rabin_rounds,
  int32_t bit_prime_length,
  uint8_t** output, int32_t* output_length);

CABI_IMPORT cerror_t rng_crypto_primes_min_max_uint64_aot(
  int32_t miller_rabin_rounds, int32_t counts,
  uint64_t min, uint64_t max,
  uint64_t** output);

CABI_IMPORT cerror_t rng_crypto_primes_min_max_aot(
  int32_t miller_rabin_rounds, int32_t counts,
  uint8_t* min, int32_t min_length,
  uint8_t* max, int32_t max_length,
  uint8_t** output,
  int32_t** output_lengths);

CABI_IMPORT cerror_t rng_crypto_primes_aot(
  int32_t miller_rabin_rounds,
  int32_t bit_prime_length, int32_t counts,
  uint8_t** output, int32_t** output_length);

// ---------------------------------------------------------
// Compression
// ---------------------------------------------------------

  CABI_IMPORT cerror_t compress_message_gzip_aot(
    const uint8_t* bytes_ptr, int32_t bytes_length,
    uint8_t compresslevel,
    uint8_t** out_ptr, int32_t* out_length);

  CABI_IMPORT cerror_t decompress_message_gzip_aot(
    const uint8_t* bytes_ptr, int32_t length,
    uint8_t** out_ptr, int32_t* out_length);

  CABI_IMPORT cerror_t compress_file_gzip_aot(
    const uint8_t* src_utf8_ptr, int32_t src_length,
    const uint8_t* dest_utf8_ptr, int32_t dest_length,
    uint8_t compresslevel);

  CABI_IMPORT cerror_t compress_file_buffer_size_gzip_aot(
    const uint8_t* src_utf8_ptr, int32_t src_length,
    const uint8_t* dest_utf8_ptr, int32_t dest_length,
    int32_t buffersize, uint8_t compresslevel);

  CABI_IMPORT cerror_t decompress_file_gzip_aot(
    const uint8_t* src_utf8_ptr, int32_t src_length,
    const uint8_t* dest_utf8_ptr, int32_t dest_length);

  CABI_IMPORT cerror_t decompress_file_buffer_size_gzip_aot(
    const uint8_t* src_utf8_ptr, int32_t src_length,
    const uint8_t* dest_utf8_ptr, int32_t dest_length,
    int32_t buffersize);

  CABI_IMPORT cerror_t compress_message_brotli_aot(
    const uint8_t* bytes_ptr, int32_t bytes_length,
    uint8_t compresslevel,
    uint8_t** out_ptr, int32_t* out_length);

  CABI_IMPORT cerror_t decompress_message_brotli_aot(
    const uint8_t* bytes_ptr, int32_t length,
    uint8_t** out_ptr, int32_t* out_length);

  CABI_IMPORT cerror_t compress_file_brotli_aot(
    const uint8_t* src_utf8_ptr, int32_t src_length,
    const uint8_t* dest_utf8_ptr, int32_t dest_length,
    uint8_t compresslevel);

  CABI_IMPORT cerror_t compress_file_buffer_size_brotli_aot(
    const uint8_t* src_utf8_ptr, int32_t src_length,
    const uint8_t* dest_utf8_ptr, int32_t dest_length,
    int32_t buffersize, uint8_t compresslevel);

  CABI_IMPORT cerror_t decompress_file_brotli_aot(
    const uint8_t* src_utf8_ptr, int32_t src_length,
    const uint8_t* dest_utf8_ptr, int32_t dest_length);

  CABI_IMPORT cerror_t decompress_file_buffer_size_brotli_aot(
    const uint8_t* src_utf8_ptr, int32_t src_length,
    const uint8_t* dest_utf8_ptr, int32_t dest_length,
    int32_t buffersize);

  CABI_IMPORT cerror_t pack_file_aot(
    const uint8_t** pack_list_utf8_ptr, const int32_t* pack_list_length, int32_t pack_list_count,
    const uint8_t* archiv_path_utf8_ptr, int32_t archiv_path_length,
    uint8_t compresstype,
    int64_t* total_file_size, int64_t* total_compress_size);

  CABI_IMPORT cerror_t pack_file_bs_cl_aot(
    const uint8_t** pack_list_utf8_ptr, const int32_t* pack_list_length, int32_t pack_list_count,
    const uint8_t* archiv_path_utf8_ptr, int32_t archiv_path_length,
    uint8_t compresstype, int32_t buffersize, uint8_t compresslevel,
    int64_t* total_file_size, int64_t* total_compress_size);

  CABI_IMPORT cerror_t pack_archiv_aot(
    const uint8_t* src_folder_utf8_ptr, int32_t src_folder_length,
    const uint8_t* dest_folder_utf8_ptr, int32_t dest_folder_length,
    uint8_t compresstype,
    int64_t* total_file_size, int64_t* total_compress_size);

  CABI_IMPORT cerror_t pack_archiv_bs_cl_aot(
    const uint8_t* src_folder_utf8_ptr, int32_t src_folder_length,
    const uint8_t* dest_folder_utf8_ptr, int32_t dest_folder_length,
    uint8_t compresstype, int32_t buffersize, uint8_t compresslevel,
    int64_t* total_file_size, int64_t* total_compress_size);

  CABI_IMPORT cerror_t unpack_file_archiv_aot(
    const uint8_t* archiv_path_ptr, int32_t archiv_path_length,
    const uint8_t* output_folder_ptr, int32_t output_folder_length);

  CABI_IMPORT cerror_t unpack_file_archiv_bs_aot(
    const uint8_t* archiv_path_ptr, int32_t archiv_path_length,
    const uint8_t* output_folder_ptr, int32_t output_folder_length,
    int32_t buffersize);


#endif