
#include "pch.h"
//#include <cstdlib>
//
//#include "bridge.h"



//inline cerror_t make_ok()
//{
//  return { static_cast<int32_t>(cerror_code_t::Ok), nullptr };
//}
//
//inline cerror_t make_error(cerror_code_t code, const char* msg)
//{
//  return { static_cast<int32_t>(code), msg };
//}
//
//// ---------------------------------------------------------
//// Allocation
//// ---------------------------------------------------------
//
//CABI_EXPORT void free_buffer_aot(void* ptr)
//{
//  // TODO: echte Freigabe-Strategie (muss zu deiner Allokation passen)
//  std::free(ptr);
//}

// ---------------------------------------------------------
// AES
// ---------------------------------------------------------

//cerror_t aes_encrypt_aot(
//  const std::uint8_t* bytes, int bytes_length,
//  const void* key, int key_length,
//  const std::uint8_t* associated, int associated_length,
//  void** output, int* output_length)
//{
//  // TODO: implement AES encryption
//  if (!bytes || bytes_length <= 0)
//    return make_error(cerror_code_t::InvalidLength, "bytes is null or length <= 0");
//
//  if (!key || key_length <= 0)
//    return make_error(cerror_code_t::InvalidLength, "key is null or length <= 0");
//
//  if (!output || !output_length)
//    return make_error(cerror_code_t::NullPointer, "output or output_length is null");
//
//  // TODO: echte AES-Implementierung
//  (void)associated;
//  (void)associated_length;
//
//  return make_ok();
//}
//
//cerror_t aes_decrypt_aot(
//  const std::uint8_t* bytes, int bytes_length,
//  const void* key, int key_length,
//  const std::uint8_t* associated, int associated_length,
//  void** output, int* output_length)
//{
//  // TODO: implement AES decryption
//  (void)bytes; (void)bytes_length;
//  (void)key; (void)key_length;
//  (void)associated; (void)associated_length;
//  (void)output; (void)output_length;
//
//  return make_ok();
//}
//
////extern "C" CABI_EXPORT cerror_t aes_encrypt_file_aot(
////  const std::uint8_t* src, int src_length,
////  const std::uint8_t* dest, int dest_length,
////  const void* key, int key_length,
////  const std::uint8_t* associated, int associated_length);
////
////extern "C" CABI_EXPORT cerror_t aes_decrypt_file_aot(
////  const std::uint8_t* src, int src_length,
////  const std::uint8_t* dest, int dest_length,
////  const void* key, int key_length,
////  const std::uint8_t* associated, int associated_length);
//
//cerror_t aes_encrypt_file_aot(
//  const std::uint8_t* src, int src_length,
//  const std::uint8_t* dest, int dest_length,
//  const void* key, int key_length,
//  const std::uint8_t* associated, int associated_length)
//{
//  // TODO: implement AES file encryption
//  (void)src; (void)src_length;
//  (void)dest; (void)dest_length;
//  (void)key; (void)key_length;
//  (void)associated; (void)associated_length;
//
//  return make_ok();
//}
//
//cerror_t aes_decrypt_file_aot(
//  const std::uint8_t* src, int src_length,
//  const std::uint8_t* dest, int dest_length,
//  const void* key, int key_length,
//  const std::uint8_t* associated, int associated_length)
//{
//  // TODO: implement AES file decryption
//  (void)src; (void)src_length;
//  (void)dest; (void)dest_length;
//  (void)key; (void)key_length;
//  (void)associated; (void)associated_length;
//
//  return make_ok();
//}

//// ---------------------------------------------------------
//// AES‑GCM
//// ---------------------------------------------------------
//
//CABI_EXPORT cerror_t aes_gcm_encrypt_aot(
//  const std::uint8_t* bytes, int bytes_length,
//  const void* key, int key_length,
//  const std::uint8_t* associated, int associated_length,
//  void** output, int* output_length)
//{
//  // TODO: implement AES‑GCM encryption
//  (void)bytes; (void)bytes_length;
//  (void)key; (void)key_length;
//  (void)associated; (void)associated_length;
//  (void)output; (void)output_length;
//  return cerror_t::Error;
//}
//
//CABI_EXPORT cerror_t aes_gcm_decrypt_aot(
//  const std::uint8_t* bytes, int bytes_length,
//  const void* key, int key_length,
//  const std::uint8_t* associated, int associated_length,
//  void** output, int* output_length)
//{
//  // TODO: implement AES‑GCM decryption
//  (void)bytes; (void)bytes_length;
//  (void)key; (void)key_length;
//  (void)associated; (void)associated_length;
//  (void)output; (void)output_length;
//  return cerror_t::Error;
//}
//
//CABI_EXPORT cerror_t aes_gcm_encrypt_file_aot(
//  const std::uint8_t* src, int src_length,
//  const std::uint8_t* dest, int dest_length,
//  const void* key, int key_length,
//  const std::uint8_t* associated, int associated_length)
//{
//  // TODO: implement AES‑GCM file encryption
//  (void)src; (void)src_length;
//  (void)dest; (void)dest_length;
//  (void)key; (void)key_length;
//  (void)associated; (void)associated_length;
//  return cerror_t::Error;
//}
//
//CABI_EXPORT cerror_t aes_gcm_decrypt_file_aot(
//  const std::uint8_t* src, int src_length,
//  const std::uint8_t* dest, int dest_length,
//  const void* key, int key_length,
//  const std::uint8_t* associated, int associated_length)
//{
//  // TODO: implement AES‑GCM file decryption
//  (void)src; (void)src_length;
//  (void)dest; (void)dest_length;
//  (void)key; (void)key_length;
//  (void)associated; (void)associated_length;
//  return cerror_t::Error;
//}
//
//// ---------------------------------------------------------
//// ChaCha20‑Poly1305
//// ---------------------------------------------------------
//
//CABI_EXPORT cerror_t chacha20_poly1305_encrypt_aot(
//  const std::uint8_t* bytes, int bytes_length,
//  const void* key, int key_length,
//  const std::uint8_t* associated, int associated_length,
//  void** output, int* output_length)
//{
//  // TODO: implement ChaCha20‑Poly1305 encryption
//  (void)bytes; (void)bytes_length;
//  (void)key; (void)key_length;
//  (void)associated; (void)associated_length;
//  (void)output; (void)output_length;
//  return cerror_t::Error;
//}
//
//CABI_EXPORT cerror_t chacha20_poly1305_decrypt_aot(
//  const std::uint8_t* bytes, int bytes_length,
//  const void* key, int key_length,
//  const std::uint8_t* associated, int associated_length,
//  void** output, int* output_length)
//{
//  // TODO: implement ChaCha20‑Poly1305 decryption
//  (void)bytes; (void)bytes_length;
//  (void)key; (void)key_length;
//  (void)associated; (void)associated_length;
//  (void)output; (void)output_length;
//  return cerror_t::Error;
//}
//
//CABI_EXPORT cerror_t chacha20_poly1305_encrypt_file_aot(
//  const std::uint8_t* src, int src_length,
//  const std::uint8_t* dest, int dest_length,
//  const void* key, int key_length,
//  const std::uint8_t* associated, int associated_length)
//{
//  // TODO: implement ChaCha20‑Poly1305 file encryption
//  (void)src; (void)src_length;
//  (void)dest; (void)dest_length;
//  (void)key; (void)key_length;
//  (void)associated; (void)associated_length;
//  return cerror_t::Error;
//}
//
//CABI_EXPORT cerror_t chacha20_poly1305_decrypt_file_aot(
//  const std::uint8_t* src, int src_length,
//  const std::uint8_t* dest, int dest_length,
//  const void* key, int key_length,
//  const std::uint8_t* associated, int associated_length)
//{
//  // TODO: implement ChaCha20‑Poly1305 file decryption
//  (void)src; (void)src_length;
//  (void)dest; (void)dest_length;
//  (void)key; (void)key_length;
//  (void)associated; (void)associated_length;
//  return cerror_t::Error;
//}
//
//// ---------------------------------------------------------
//// Crypto Random – Bytes
//// ---------------------------------------------------------
//
//CABI_EXPORT cerror_t rng_crypto_bytes_aot(int size, void** out_ptr)
//{
//  // TODO: implement random byte buffer
//  (void)size; (void)out_ptr;
//  return cerror_t::Error;
//}
//
//CABI_EXPORT cerror_t fill_crypto_bytes_aot(const std::uint8_t* bytes, int length)
//{
//  // TODO: implement fill with crypto random
//  (void)bytes; (void)length;
//  return cerror_t::Error;
//}
//
//// ---------------------------------------------------------
//// Crypto Random – Bools
//// ---------------------------------------------------------
//
//CABI_EXPORT bool next_crypto_bool_aot(cerror_t* err)
//{
//  // TODO: implement next random bool
//  if (err) *err = cerror_t::Error;
//  return false;
//}
//
//CABI_EXPORT cerror_t rng_crypto_bool_aot(int size, void** out_ptr)
//{
//  // TODO: implement random bool array
//  (void)size; (void)out_ptr;
//  return cerror_t::Error;
//}
//
//// ---------------------------------------------------------
//// Crypto Random – Int32
//// ---------------------------------------------------------
//
//CABI_EXPORT int next_crypto_int32_aot(cerror_t* err)
//{
//  // TODO: implement next random int32
//  if (err) *err = cerror_t::Error;
//  return 0;
//}
//
//CABI_EXPORT int next_crypto_int32_max_aot(int max, cerror_t* err)
//{
//  // TODO: implement next random int32 [0, max)
//  (void)max;
//  if (err) *err = cerror_t::Error;
//  return 0;
//}
//
//CABI_EXPORT int next_crypto_int32_min_max_aot(int min, int max, cerror_t* err)
//{
//  // TODO: implement next random int32 [min, max)
//  (void)min; (void)max;
//  if (err) *err = cerror_t::Error;
//  return 0;
//}
//
//CABI_EXPORT cerror_t rng_crypto_int32_aot(int size, void** out_ptr)
//{
//  // TODO: implement random int32 array
//  (void)size; (void)out_ptr;
//  return cerror_t::Error;
//}
//
//CABI_EXPORT cerror_t rng_crypto_int32_max_aot(int size, int max, void** out_ptr)
//{
//  // TODO: implement random int32 array [0, max)
//  (void)size; (void)max; (void)out_ptr;
//  return cerror_t::Error;
//}
//
//CABI_EXPORT cerror_t rng_crypto_int32_min_max_aot(int size, int min, int max, void** out_ptr)
//{
//  // TODO: implement random int32 array [min, max)
//  (void)size; (void)min; (void)max; (void)out_ptr;
//  return cerror_t::Error;
//}
//
//// ---------------------------------------------------------
//// Crypto Random – Int64
//// ---------------------------------------------------------
//
//CABI_EXPORT long next_crypto_int64_aot(cerror_t* err)
//{
//  // TODO: implement next random int64
//  if (err) *err = cerror_t::Error;
//  return 0;
//}
//
//CABI_EXPORT long next_crypto_int64_max_aot(long max, cerror_t* err)
//{
//  // TODO: implement next random int64 [0, max)
//  (void)max;
//  if (err) *err = cerror_t::Error;
//  return 0;
//}
//
//CABI_EXPORT long next_crypto_int64_min_max_aot(long min, long max, cerror_t* err)
//{
//  // TODO: implement next random int64 [min, max)
//  (void)min; (void)max;
//  if (err) *err = cerror_t::Error;
//  return 0;
//}
//
//CABI_EXPORT cerror_t rng_crypto_int64_aot(int size, void** out_ptr)
//{
//  // TODO: implement random int64 array
//  (void)size; (void)out_ptr;
//  return cerror_t::Error;
//}
//
//CABI_EXPORT cerror_t rng_crypto_int64_max_aot(int size, long max, void** out_ptr)
//{
//  // TODO: implement random int64 array [0, max)
//  (void)size; (void)max; (void)out_ptr;
//  return cerror_t::Error;
//}
//
//CABI_EXPORT cerror_t rng_crypto_int64_min_max_aot(int size, long min, long max, void** out_ptr)
//{
//  // TODO: implement random int64 array [min, max)
//  (void)size; (void)min; (void)max; (void)out_ptr;
//  return cerror_t::Error;
//}
//
//// ---------------------------------------------------------
//// Crypto Random – Double
//// ---------------------------------------------------------
//
//CABI_EXPORT double next_crypto_double_aot(cerror_t* err)
//{
//  // TODO: implement next random double
//  if (err) *err = cerror_t::Error;
//  return 0.0;
//}
//
//CABI_EXPORT double next_crypto_double_max_aot(double max, cerror_t* err)
//{
//  // TODO: implement next random double [0, max)
//  (void)max;
//  if (err) *err = cerror_t::Error;
//  return 0.0;
//}
//
//CABI_EXPORT double next_crypto_double_min_max_aot(double min, double max, cerror_t* err)
//{
//  // TODO: implement next random double [min, max)
//  (void)min; (void)max;
//  if (err) *err = cerror_t::Error;
//  return 0.0;
//}
//
//CABI_EXPORT cerror_t rng_crypto_double_aot(int size, void** out_ptr)
//{
//  // TODO: implement random double array
//  (void)size; (void)out_ptr;
//  return cerror_t::Error;
//}
//
//CABI_EXPORT cerror_t rng_crypto_double_max_aot(int size, double max, void** out_ptr)
//{
//  // TODO: implement random double array [0, max)
//  (void)size; (void)max; (void)out_ptr;
//  return cerror_t::Error;
//}
//
//CABI_EXPORT cerror_t rng_crypto_double_min_max_aot(int size, double min, double max, void** out_ptr)
//{
//  // TODO: implement random double array [min, max)
//  (void)size; (void)min; (void)max; (void)out_ptr;
//  return cerror_t::Error;
//}
//
//// ---------------------------------------------------------
//// Crypto Random – Float (Single)
//// ---------------------------------------------------------
//
//CABI_EXPORT float next_crypto_single_aot(cerror_t* err)
//{
//  // TODO: implement next random float
//  if (err) *err = cerror_t::Error;
//  return 0.0f;
//}
//
//CABI_EXPORT float next_crypto_single_max_aot(float max, cerror_t* err)
//{
//  // TODO: implement next random float [0, max)
//  (void)max;
//  if (err) *err = cerror_t::Error;
//  return 0.0f;
//}
//
//CABI_EXPORT float next_crypto_single_min_max_aot(float min, float max, cerror_t* err)
//{
//  // TODO: implement next random float [min, max)
//  (void)min; (void)max;
//  if (err) *err = cerror_t::Error;
//  return 0.0f;
//}
//
//CABI_EXPORT cerror_t rng_crypto_single_aot(int size, void** out_ptr)
//{
//  // TODO: implement random float array
//  (void)size; (void)out_ptr;
//  return cerror_t::Error;
//}
//
//CABI_EXPORT cerror_t rng_crypto_single_max_aot(int size, float max, void** out_ptr)
//{
//  // TODO: implement random float array [0, max)
//  (void)size; (void)max; (void)out_ptr;
//  return cerror_t::Error;
//}
//
//CABI_EXPORT cerror_t rng_crypto_single_min_max_aot(int size, float min, float max, void** out_ptr)
//{
//  // TODO: implement random float array [min, max)
//  (void)size; (void)min; (void)max; (void)out_ptr;
//  return cerror_t::Error;
//}
//
//// ---------------------------------------------------------
//// Crypto Random – Decimal (als 128‑Bit Buffer)
//// ---------------------------------------------------------
//
//CABI_EXPORT Decimal128 next_crypto_decimal_aot(cerror_t* err)
//{
//  // TODO: implement next random decimal
//  if (err) *err = cerror_t::Error;
//  Decimal128 d{};
//  return d;
//}
//
//CABI_EXPORT Decimal128 next_crypto_decimal_max_aot(Decimal128 max, cerror_t* err)
//{
//  // TODO: implement next random decimal [0, max)
//  (void)max;
//  if (err) *err = cerror_t::Error;
//  Decimal128 d{};
//  return d;
//}
//
//CABI_EXPORT Decimal128 next_crypto_decimal_min_max_aot(Decimal128 min, Decimal128 max, cerror_t* err)
//{
//  // TODO: implement next random decimal [min, max)
//  (void)min; (void)max;
//  if (err) *err = cerror_t::Error;
//  Decimal128 d{};
//  return d;
//}
//
//CABI_EXPORT cerror_t rng_crypto_decimal_aot(int size, void** out_ptr)
//{
//  // TODO: implement random decimal array
//  (void)size; (void)out_ptr;
//  return cerror_t::Error;
//}
//
//CABI_EXPORT cerror_t rng_crypto_decimal_max_aot(int size, Decimal128 max, void** out_ptr)
//{
//  // TODO: implement random decimal array [0, max)
//  (void)size; (void)max; (void)out_ptr;
//  return cerror_t::Error;
//}
//
//CABI_EXPORT cerror_t rng_crypto_decimal_min_max_aot(int size, Decimal128 min, Decimal128 max, void** out_ptr)
//{
//  // TODO: implement random decimal array [min, max)
//  (void)size; (void)min; (void)max; (void)out_ptr;
//  return cerror_t::Error;
//}