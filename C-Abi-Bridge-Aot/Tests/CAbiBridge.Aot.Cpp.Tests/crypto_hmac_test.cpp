#include "pch.h"

#include <vector>
#include <string>
#include <chrono>
#include <iostream>

#include "bridge.h"
#include "cerror.h"
#include "ref_hash_hmac.h"
#include "convert_encoding_test.h"

namespace michele::natale::Tests
{
  template<typename RefHmacFunc>
  static void test_hmac(
    const char* name, int rounds,
    cerror_t(*native)(const uint8_t*,
      int, const uint8_t*,
      int, uint8_t**, int*),
    RefHmacFunc ref_hmac)
  {
    using namespace std::chrono;

    std::cout << name << "_hmac_aot: ";

    auto start = high_resolution_clock::now();

    for (int i = 0; i < rounds; i++)
    {
      //int size = ;
      auto key = rng_bytes(rng_int(1, 128));
      auto bytes = rng_bytes(rng_int(1, 128));

      int out_len = 0; void* out_ptr = nullptr;
      auto err = native(bytes.data(), (int)bytes.size(),
        key.data(), (int)key.size(), (uint8_t**)(&out_ptr), &out_len);
      assert_error(err);

      auto data = to_bytes(out_ptr, out_len);
      free_buffer_aot(out_ptr);

      auto ref = ref_hmac(key, bytes);

      if (ref.size() != data.size())
        throw std::runtime_error("Length mismatch");

      if (!std::equal(ref.begin(), ref.end(), data.begin()))
        throw std::runtime_error("HMAC mismatch");

      if (i % (rounds / 10) == 0)
        std::cout << ".";
    }

    auto end = std::chrono::high_resolution_clock::now();
    double ms = std::chrono::duration<double, std::milli>(end - start).count();

    std::cout << " rounds = " << rounds
      << "; t = " << ms << "ms; td = " << (ms / rounds) << "ms\n";
  }


  void start_crypto_hmac_native(int rounds)
  {
    test_hmac("test_md5", rounds, hmac_md5_hash_data_aot, ref_md5_hmac);
    test_hmac("test_sha1", rounds, hmac_sha1_hash_data_aot, ref_sha1_hmac);

    test_hmac("test_sha256", rounds, hmac_sha_256_hash_data_aot, ref_sha_256_hmac);
    test_hmac("test_sha384", rounds, hmac_sha_384_hash_data_aot, ref_sha_384_hmac);
    test_hmac("test_sha512", rounds, hmac_sha_512_hash_data_aot, ref_sha_512_hmac);

    test_hmac("test_sha3_256", rounds, hmac_sha3_256_hash_data_aot, ref_sha3_256_hmac);
    test_hmac("test_sha3_384", rounds, hmac_sha3_384_hash_data_aot, ref_sha3_384_hmac);
    test_hmac("test_sha3_512", rounds, hmac_sha3_512_hash_data_aot, ref_sha3_512_hmac);

    std::cout << "\n";
  }
}