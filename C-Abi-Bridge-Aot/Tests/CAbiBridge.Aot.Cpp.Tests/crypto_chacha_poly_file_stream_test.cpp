#include "pch.h"

#include <vector>
#include <string> 
#include <random>
#include <fstream>
#include <chrono>
#include <iostream>
#include <cstring>
#include <stdexcept>

#include "bright.h"
#include "variables.h"
#include "crypto_chacha_poly_test.h"


namespace michele::natale::Tests
{

  void test_chacha_poly_file(int rounds)
  {
    using namespace std::chrono;
    using namespace michele::natale::Cpp;

    std::cout << "test_chacha_poly_file_aot: ";

    const std::string src = "data";
    const std::string dest = "cipher";
    const std::string srcr = "datar";

    auto start = high_resolution_clock::now();

    int max = (1 << 21) + 1024;
    std::mt19937 rng(std::random_device{}());
    std::uniform_int_distribution<int> dist(0, max);
    std::uniform_int_distribution<int> dist_assoc(1, 64);

    for (int i = 0; i < rounds; i++)
    {
      // Datei mit Zufallsgröße erzeugen
      int flength = dist(rng);

      set_rng_file_data(src, flength);

      // Associated Data
      int assoc_len = dist_assoc(rng);
      auto associated = rng_bytes(assoc_len);

      // Key
      std::vector<uint8_t> key(Services::CHACHA_POLY_MAX_KEY_SIZE);
      auto keydata = rng_bytes(key.size());
      std::copy(keydata.begin(), keydata.end(), key.begin());

      // Encrypt
      auto err = chacha20_poly1305_encrypt_file_aot(
        (const uint8_t*)src.c_str(), (int)src.size(),
        (const uint8_t*)dest.c_str(), (int)dest.size(),
        key.data(), (int)key.size(),
        associated.data(), (int)associated.size());
      assert_error(err);

      // Decrypt
      err = chacha20_poly1305_decrypt_file_aot(
        (const uint8_t*)dest.c_str(), (int)dest.size(),
        (const uint8_t*)srcr.c_str(), (int)srcr.size(),
        key.data(), (int)key.size(),
        associated.data(), (int)associated.size());
      assert_error(err);

      // Vergleich
      if (!file_equals(src, srcr))
        throw std::runtime_error("ChaCha20Poly1305 file mismatch");

      if (i % (rounds / 10) == 0)
        std::cout << ".";
    }

    auto end = high_resolution_clock::now();
    auto ms = duration_cast<milliseconds>(end - start).count();

    std::cout << " rounds = " << rounds
      << ", t = " << ms << "ms"
      << ", td = " << (ms / rounds) << "ms\n";
  }
}