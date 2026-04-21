#include "pch.h"

#include <chrono>
#include <iostream> 
#include <vector>
#include <cstring>
#include <stdexcept>
#include <algorithm>
#include <openssl/bn.h> 

#include "bridge.h" 
#include "test_servises.h"
#include "crypto_utils_test.h" 


namespace michele::natale::test_service
{
  using namespace michele::natale::Tests;

  std::vector<uint8_t> converter_2_256_le_s(
    const uint8_t* bytes, int length,
    int start_base, int target_base)
  {
    int out_length = 0;
    uint8_t* out_ptr = nullptr;

    auto err = converter_2_256_le_aot(
      bytes, length,
      start_base, target_base,
      &out_ptr, &out_length);

    assert_error(err);

    std::vector<uint8_t> result(out_ptr, out_ptr + out_length);
    free_buffer_aot(out_ptr);

    return result;
  }

  std::vector<uint8_t> converter_2_256_le_s(
    const BIGNUM* bi, int start_base, int target_base)
  {
    // 1. Länge in Bytes bestimmen
    int num_bytes = BN_num_bytes(bi);

    // 2. Big-endian Bytes holen
    std::vector<uint8_t> be(num_bytes); 
    BN_bn2lebinpad(bi, be.data(), num_bytes);

    // 3. Reverse → Little-endian (wie C# 
    // BigInteger.ToByteArray().Reverse())
    std::reverse(be.begin(), be.end());

    // 4. Native-Call
    int out_length = 0;
    uint8_t* out_ptr = nullptr;

    auto err = converter_2_256_le_aot(
      be.data(), (int)be.size(),
      start_base, target_base,
      &out_ptr, &out_length);

    assert_error(err);

    // 5. Ergebnis kopieren
    std::vector<uint8_t> result(out_ptr, out_ptr + out_length);

    // 6. FreeBuffer wie in C#
    free_buffer_aot(out_ptr);

    return result;
  }

  std::vector<uint8_t> to_base_x_2_256_le_s(
    const uint8_t* number, int number_length,
    int target_base)
  {
    int out_length = 0;
    uint8_t* out_ptr = nullptr;

    auto err = to_base_x_2_256_le_aot(
      number, number_length, target_base,
      &out_ptr, &out_length);

    assert_error(err);

    std::vector<uint8_t> result(out_ptr, out_ptr + out_length);
    free_buffer_aot(out_ptr);

    return result;
  }

  std::vector<uint8_t> to_base_x_2_256_le_s(
    const std::string& number, int target_base)
  {
    auto bytes = reinterpret_cast<const uint8_t*>(number.data());
    return to_base_x_utf8_2_256_le_s(bytes, (int)number.length(), target_base);
  }

  std::vector<uint8_t> to_base_x_2_256_le_s(
    const BIGNUM* bi, int target_base)
  {
    char* dec = BN_bn2dec(bi);
    std::string str_number(dec);
    OPENSSL_free(dec);

    return to_base_x_2_256_le_s(str_number, target_base);
  }

  /* std::vector<uint8_t> to_base_x_2_256_le_s(
     const std::string& number, int target_base)
   {
     int out_length = 0;
     uint8_t* out_ptr = nullptr;

     auto err = to_base_x_2_256_le_aot(
       reinterpret_cast<const uint8_t*>(number.data()),
       number.size(), target_base,
       &out_ptr, &out_length);

     assert_error(err);

     std::vector<uint8_t> result(out_ptr, out_ptr + out_length);
     free_buffer_aot(out_ptr);

     return result;
   }*/

   //std::vector<uint8_t> to_base_x_2_256_le_s(
   //  const BIGNUM* bi, int target_base)
   //{
   //  // 1. BIGNUM → Dezimalstring
   //  char* dec = BN_bn2dec(bi);

   //  if (!dec)
   //    throw std::runtime_error("BN_bn2dec failed");

   //  // 2. In std::string übernehmen
   //  std::string number(dec);
   //  OPENSSL_free(dec);

   //  // 3. String → UTF‑8‑Bytes
   //  const uint8_t* bytes = reinterpret_cast<const uint8_t*>(number.data());
   //  int length = (int)number.size();

   //  // 4. Native‑Call
   //  uint8_t* out_ptr = nullptr;
   //  int out_length = 0;

   //  auto err = to_base_x_2_256_le_aot(
   //    bytes, length,
   //    target_base,
   //    &out_ptr, &out_length);

   //  assert_error(err);

   //  // 5. Ergebnis kopieren
   //  std::vector<uint8_t> result(out_ptr, out_ptr + out_length);

   //  // 6. Buffer freigeben
   //  free_buffer_aot(out_ptr);

   //  return result;
   //}

  std::vector<uint8_t> to_base_x_utf8_2_256_le_s(
    const uint8_t* bytes, int length, int target_base)
  {
    int out_length = 0;
    uint8_t* out_ptr = nullptr;

    auto err = to_base_x_utf8_2_256_le_aot(
      bytes, length, target_base,
      &out_ptr, &out_length);

    assert_error(err);

    std::vector<uint8_t> result(out_ptr, out_ptr + out_length);
    free_buffer_aot(out_ptr);

    return result;
  }


  std::vector<uint8_t> from_base_x_2_256_le_s(
    const uint8_t* bytes, int length, int from_base_x)
  {
    int out_length = 0;
    uint8_t* out_ptr = nullptr;

    auto err = from_base_x_2_256_le_aot(
      bytes, length, from_base_x,
      &out_ptr, &out_length);

    assert_error(err);

    std::vector<uint8_t> result(out_ptr, out_ptr + out_length);
    free_buffer_aot(out_ptr);

    return result;
  }


  std::pair<int, int> rng_bases_2_256()
  {
    cerror_t err; int targetbase;
    int startbase = next_crypto_int32_min_max_aot(2, 256, &err);
    assert_error(err);

    do {
      targetbase = next_crypto_int32_min_max_aot(2, 256, &err);
      assert_error(err);
    } while (targetbase == startbase);

    return { startbase, targetbase };
  }
}