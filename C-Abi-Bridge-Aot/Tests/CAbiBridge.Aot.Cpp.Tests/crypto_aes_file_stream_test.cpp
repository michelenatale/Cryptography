#include "pch.h"

#include <iostream>
#include <vector> 
#include <string> 
#include <chrono>      
#include <fstream>
#include "bridge.h"

#include "crypto_aes_test.h"
#include "crypto_utils_test.h"



namespace michele::natale::Tests {

  void test_aes_file(int rounds)
  {
    std::cout << "test_aes_file_aot: ";

    const std::string src = "data";
    const std::string dest = "cipher";
    const std::string srcr = "datar";

    auto start = std::chrono::high_resolution_clock::now();

    int max = (1 << 21) + 1024;

    for (int i = 0; i < rounds; i++)
    {
      // zufällige Dateigröße
      int flength = rng_int(1, max);

      // Datei mit Zufallsdaten erzeugen
      set_rng_file_data(src, flength);

      // Associated Data 
      auto associated = rng_bytes(rng_int(1, 64));
      
      // Key
      auto key = rng_bytes(32); // AES-256

      // --- Encrypt ---
      auto err = aes_encrypt_file_aot(
        reinterpret_cast<const uint8_t*>(src.data()), (int)src.size(),
        reinterpret_cast<const uint8_t*>(dest.data()), (int)dest.size(),
        key.data(), (int)key.size(),
        associated.data(), (int)associated.size());
      assert_error(err);

      // --- Decrypt ---
      err = aes_decrypt_file_aot(
        reinterpret_cast<const uint8_t*>(dest.data()), (int)dest.size(),
        reinterpret_cast<const uint8_t*>(srcr.data()), (int)srcr.size(),
        key.data(), (int)key.size(),
        associated.data(), (int)associated.size());
      assert_error(err);

      // Dateien vergleichen
      if (!equal_files_aot(
        reinterpret_cast<const uint8_t*>(src.data()), (int)src.length(),
        reinterpret_cast<const uint8_t*>(srcr.data()), (int)srcr.length(), &err))
      {
        std::cerr << "AES file mismatch\n";
        std::abort();
      }
      assert_error(err);

      if (i % (rounds / 10) == 0)
        std::cout << ".";
    }

    auto end = std::chrono::high_resolution_clock::now();
    auto ms = std::chrono::duration_cast<std::chrono::milliseconds>(end - start).count();

    std::cout << " rounds = " << rounds
      << ", t = " << ms << "ms"
      << ", td = " << (ms / rounds) << "ms\n";
  }
}