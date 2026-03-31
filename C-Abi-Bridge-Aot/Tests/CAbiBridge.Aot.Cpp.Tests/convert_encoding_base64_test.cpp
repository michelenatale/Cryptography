#include "pch.h"

#include <chrono>
#include <iostream>
#include <random>
#include <vector>
#include <cstring>
#include <stdexcept>

#include "bright.h"
#include "crypto_utils_test.h"
#include "convert_encoding_test.h"
#include "convert_encoding_utils_test.h"


namespace michele::natale::Tests
{

  static void test_base64(int rounds)
  {
    using namespace std::chrono;

    std::cout << "test_base64_aot: ";

    auto start = high_resolution_clock::now();
    static thread_local std::mt19937 rng{ std::random_device{}() };
    std::uniform_int_distribution<int> size_dist(1, 128);

    for (int i = 0; i < rounds; i++)
    {
      int size = size_dist(rng);
      auto bytes = rng_bytes(size);

      // --- ToBase64Aot ---
      int out_len = 0;
      uint8_t* out_ptr = nullptr;

      auto err = to_base_64_utf8_aot(
        bytes.data(), (int)bytes.size(), &out_ptr, &out_len);
      assert_error(err);

      auto data = to_bytes(out_ptr, out_len);
      free_buffer_aot(out_ptr);

      // reference Base64
      std::string b64_str = encode64(bytes.data(), bytes.size());
      std::vector<uint8_t> b64(b64_str.begin(), b64_str.end());

      if (out_len != (int)b64.size())
        throw std::runtime_error("Length mismatch");

      if (!std::equal(b64.begin(), b64.end(), data.begin()))
        throw std::runtime_error("Data mismatch");

      // --- FromBase64Aot ---
      out_ptr = nullptr;
      out_len = 0;

      err = from_base_64_utf8_aot(b64.data(), (int)b64.size(),
        &out_ptr, &out_len);
      assert_error(err);

      auto decoded = to_bytes(out_ptr, out_len);
      free_buffer_aot(out_ptr);

      if (decoded.size() != bytes.size())
        throw std::runtime_error("Decoded length mismatch");

      if (!std::equal(decoded.begin(), decoded.end(), bytes.begin()))
        throw std::runtime_error("Decoded data mismatch");

      if (i % (rounds / 10) == 0)
        std::cout << ".";
    }

    auto end = std::chrono::high_resolution_clock::now();
    double ms = std::chrono::duration<double, std::milli>(end - start).count();


    std::cout << " rounds = " << rounds
      << "; t = " << ms << "ms; td = " << (ms / rounds) << "ms\n";
  }


  void start_convert_encoding_base64_native(int rounds)
  {
    test_base64(rounds);
  }

}