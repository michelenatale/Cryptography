#include "pch.h"

#include <vector>
#include <string>
#include <chrono>
#include <iostream>
#include <random>
#include <stdexcept>
#include <cstring>

#include "bright.h"
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
    static thread_local std::mt19937 rng{ std::random_device{}() };
    std::uniform_int_distribution<int> size_dist(1, 128);

    for (int i = 0; i < rounds; i++)
    {
      int size = size_dist(rng);
      auto bytes = rng_bytes(size);

      int out_len = 0;
      uint8_t* out_ptr = nullptr;

      auto err = native(bytes.data(), size, &out_ptr, &out_len);
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


  void start_crypto_hash_native(int rounds)
  {
    test_hash("test_md5", rounds, md5_hash_data_aot, ref_md5_hash);
    test_hash("test_sha1", rounds, sha1_hash_data_aot, ref_sha1_hash);
    test_hash("test_sha256", rounds, sha_256_hash_data_aot, ref_sha_256_hash);
    test_hash("test_sha384", rounds, sha_384_hash_data_aot, ref_sha_384_hash);
    test_hash("test_sha512", rounds, sha_512_hash_data_aot, ref_sha_512_hash);

    std::cout << "\n";
  }
}