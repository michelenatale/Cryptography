#include "pch.h"

#include <chrono>
#include <iostream>
#include <random>
#include <vector>
#include <cstring>
#include <stdexcept>

#include "bridge.h"
#include "crypto_utils_test.h"
#include "convert_encoding_test_start.h"
#include "convert_encoding_utils_test.h"


namespace michele::natale::Tests
{
  static void test_hex(int rounds)
  {
    using namespace std::chrono;

    std::cout << "test_hex_aot: ";

    auto start = high_resolution_clock::now();
    static thread_local std::mt19937 rng{ std::random_device{}() };
    std::uniform_int_distribution<int> size_dist(1, 128);

    for (int i = 0; i < rounds; i++)
    {
      int size = size_dist(rng);
      auto bytes = rng_bytes(size);

      bool lower = (rng() % 2) == 0;

      // --- ToHexAot ---
      uint8_t* hex_ptr = nullptr;
      int hex_len = 0;

      auto err = to_hex_utf8_aot(bytes.data(), (int)bytes.size(), &hex_ptr, &hex_len, lower);
      assert_error(err);

      auto data = to_bytes(hex_ptr, hex_len);
      free_buffer_aot(hex_ptr);

      // reference hex
      std::string hex_str = lower
        ? hex_str_lower(bytes.data(), bytes.size())
        : hex_str_upper(bytes.data(), bytes.size());
      std::vector<uint8_t> hex(hex_str.begin(), hex_str.end());

      if (hex_len != (int)hex.size())
        throw std::runtime_error("Length mismatch");

      if (!std::equal(hex.begin(), hex.end(), data.begin()))
        throw std::runtime_error("Data mismatch");

      // --- FromHexAot ---
      hex_ptr = nullptr;
      hex_len = 0;

      err = from_hex_utf8_aot(hex.data(), (int)hex.size(),
        &hex_ptr, &hex_len);
      assert_error(err);

      auto decoded = to_bytes(hex_ptr, hex_len);
      free_buffer_aot(hex_ptr);

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


  void start_convert_encoding_hex_native(int rounds)
  {
    test_hex(rounds);
  }
}