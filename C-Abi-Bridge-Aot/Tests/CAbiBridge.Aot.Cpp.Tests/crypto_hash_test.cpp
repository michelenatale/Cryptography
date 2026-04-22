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
  template<typename RefHashFunc>
  static void test_hash(
    const char* name,
    int rounds,
    cerror_t(*native)(const uint8_t*,
      int, uint8_t**, int*),
    RefHashFunc ref_hash)
  {
    using namespace std::chrono;

    std::cout << name << "_hash_aot: ";

    auto start = high_resolution_clock::now();

    for (int i = 0; i < rounds; i++)
    {
      int size = rng_int(1, 128);
      auto bytes = rng_bytes(size);

      int out_len = 0; void* out_ptr = nullptr;
      auto err = native(bytes.data(), size, (uint8_t**)(&out_ptr), &out_len);
      assert_error(err);

      auto data = to_bytes(out_ptr, out_len);
      free_buffer_aot(out_ptr);

      auto ref = ref_hash(bytes);

      if (ref.size() != data.size())
        throw std::runtime_error("Length mismatch");

      if (!std::equal(ref.begin(), ref.end(), data.begin()))
        throw std::runtime_error("Hash mismatch");

      if (i % (rounds / 10) == 0)
        std::cout << ".";
    }

    auto end = std::chrono::high_resolution_clock::now();
    double ms = std::chrono::duration<double, std::milli>(end - start).count();

    std::cout << " rounds = " << rounds
      << "; t = " << ms << "ms; td = " << (ms / rounds) << "ms\n";
  }

  template<typename RefHashFunc>
  static void test_hash(
    const char* name,
    int rounds,
    int outlength,
    cerror_t(*native)(const uint8_t*, int, int, uint8_t**),
    RefHashFunc ref_hash)
  {
    using namespace std::chrono;

    std::cout << name << "_hash_aot: ";

    auto start = high_resolution_clock::now();

    for (int i = 0; i < rounds; i++)
    {
      int size = rng_int(1, 128);
      auto bytes = rng_bytes(size);

      uint8_t* out_ptr = nullptr;
      auto err = native(bytes.data(), size, outlength, &out_ptr);
      assert_error(err);

      std::vector<uint8_t> data(out_ptr, out_ptr + outlength);
      free_buffer_aot(out_ptr);

      // Referenzfunktion bekommt ebenfalls die Länge
      auto ref = ref_hash(bytes, outlength);

      if (ref.size() != data.size())
        throw std::runtime_error("Length mismatch");

      if (!std::equal(ref.begin(), ref.end(), data.begin()))
        throw std::runtime_error("Hash mismatch");

      if (i % std::max(1, rounds / 10) == 0)
        std::cout << ".";
    }

    auto end = high_resolution_clock::now();
    double ms = duration<double, std::milli>(end - start).count();

    std::cout << " rounds = " << rounds
      << "; t = " << ms << "ms; td = " << (ms / rounds) << "ms\n";
  }

  void start_crypto_hash_native(int rounds)
  {
    test_hash("test_md5", rounds, md5_hash_data_aot, ref_md5_hash);
    test_hash("test_sha1", rounds, sha1_hash_data_aot, ref_sha1_hash);

    test_hash("test_sha256", rounds, sha_256_hash_data_aot, ref_sha_256_hash);
    test_hash("test_sha384", rounds, sha_384_hash_data_aot, ref_sha_384_hash);
    test_hash("test_sha512", rounds, sha_512_hash_data_aot, ref_sha_512_hash);

    test_hash("test_sha3_256", rounds, sha3_256_hash_data_aot, ref_sha3_256_hash);
    test_hash("test_sha3_384", rounds, sha3_384_hash_data_aot, ref_sha3_384_hash);
    test_hash("test_sha3_512", rounds, sha3_512_hash_data_aot, ref_sha3_512_hash);

    test_hash("test_shake_128", rounds, 32, shake_128_hash_data_aot, ref_shake_128_hash);
    test_hash("test_shake_256", rounds, 64, shake_256_hash_data_aot, ref_shake_256_hash);

    std::cout << "\n";
  }
}