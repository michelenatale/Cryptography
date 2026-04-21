#include "pch.h"

#include <vector>
#include <string> 
#include <fstream>
#include <chrono>
#include <iostream>

#include "bridge.h"
#include "variables.h"
#include "crypto_aes_gcm_test.h"


namespace michele::natale::Tests
{

  void test_aes_gcm_file(int rounds)
  {
    using namespace std::chrono;
    using namespace michele::natale::Cpp;

    std::cout << "test_aes_gcm_file_aot: ";

    const std::string src = "data";
    const std::string dest = "cipher";
    const std::string srcr = "datar";

    auto start = high_resolution_clock::now();


    int max = (1 << 21) + 1024;
    for (int i = 0; i < rounds; i++)
    {
      // Datei mit Zufallsgröße erzeugen 
      int flength = rng_int(1, max);
      set_rng_file_data(src, flength);

      // Associated Data
      auto associated = rng_bytes(rng_int(1, 64));

      // Key
      std::vector<uint8_t> key(Services::AES_GCM_MAX_KEY_SIZE);
      auto keydata = rng_bytes(key.size());
      std::copy(keydata.begin(), keydata.end(), key.begin());

      // Encrypt
      auto err = aes_gcm_encrypt_file_aot(
        (const uint8_t*)src.c_str(), (int)src.size(),
        (const uint8_t*)dest.c_str(), (int)dest.size(),
        key.data(), (int)key.size(),
        associated.data(), (int)associated.size());
      assert_error(err);

      // Decrypt
      err = aes_gcm_decrypt_file_aot(
        (const uint8_t*)dest.c_str(), (int)dest.size(),
        (const uint8_t*)srcr.c_str(), (int)srcr.size(),
        key.data(), (int)key.size(),
        associated.data(), (int)associated.size());
      assert_error(err);

      // Vergleich
      if (!equal_files_aot(
        reinterpret_cast<const uint8_t*>(src.data()), (int)src.length(),
        reinterpret_cast<const uint8_t*>(srcr.data()), (int)srcr.length(), &err))
        throw std::runtime_error("AES-GCM file mismatch");
      assert_error(err);

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