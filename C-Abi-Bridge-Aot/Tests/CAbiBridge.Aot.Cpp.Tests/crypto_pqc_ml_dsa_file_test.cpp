#include "pch.h"

#include <iostream>
#include <vector>
#include <chrono>
#include <cstdlib>
#include <cstring>
#include <cstdio>

#include "bridge.h"
#include "cerror.h"
#include "variables.h"
#include "usi_ptr_t.h"
#include "ml_dsa_param.h"
#include "crypto_utils_test.h"
#include "crypto_pqc_ml_dsa_file_test.h"
#include "crypto_pqc_ml_dsa_utils_test.h"
#include "crypto_pqc_ml_dsa_param_test.h"


namespace michele::natale::Tests
{
  void test_pqc_ml_dsa_single_signature_file(int rounds)
  {
    std::cout << "test_pqc_ml_dsa_single_signature_file_aot: ";

    if (rounds < 10) rounds = 10;
    const char* srcfile = "data";
    auto algos = to_ml_dsa_params();

    using clock = std::chrono::high_resolution_clock;
    auto start = clock::now();

    for (int i = 0; i < rounds; ++i)
    {
      int max = (1 << 21) + 1024;
      int size = rng_int(0, max);
      set_rng_file_data(srcfile, size);

      int idx = rng_int(0, (int)algos.size());
      auto algo = algos[idx];

      key_pair_param_info kpip;
      bool tf = rng_even();
      if (tf)
      {
        kpip = create_native_ml_dsa_key_pair();
        algo = kpip.algo;
      }
      else
      {
        kpip = create_native_ml_dsa_key_pair(algo);
      }

      auto& pubk = kpip.pub_key;
      auto& privk = kpip.priv_key;
      std::vector<uint8_t> filenameBytes(strlen(srcfile));
      std::memcpy(filenameBytes.data(), srcfile, filenameBytes.size());

      int sign_length = 0;
      uint8_t* sign_ptr = nullptr;

      cerror_t err = pqc_mldsa_sign_file_aot(
        filenameBytes.data(), static_cast<int>(filenameBytes.size()),
        privk.data(), static_cast<int>(privk.size()),
        from_ml_dsa_param(algo),
        &sign_ptr, &sign_length);

      assert_error(err);

      std::vector<uint8_t> signature(sign_ptr, sign_ptr + sign_length);
      free_buffer_aot(sign_ptr);

      uint8_t ok = pqc_mldsa_verify_file_aot(
        filenameBytes.data(), static_cast<int>(filenameBytes.size()),
        pubk.data(), static_cast<int>(pubk.size()),
        signature.data(), static_cast<int>(signature.size()),
        &err);
      assert_error(err);

      if (!ok)
        throw std::runtime_error("Verify (file) failed");

      if (rounds > 0 && i % std::max(1, rounds / 10) == 0)
        std::cout << ".";
    }

    auto end = clock::now();
    auto ms = std::chrono::duration_cast<std::chrono::milliseconds>(end - start).count();

    std::remove(srcfile);

    std::cout << " rounds = " << rounds
      << "; t = " << ms << "ms"
      << "; td = " << (ms / static_cast<double>(rounds)) << "ms\n";
  }

  void test_pqc_ml_dsa_single_signature_kpi_save_load_file(int rounds)
  {
    std::cout << "test_pqc_ml_dsa_single_signature_kpi_save_load_file_aot: ";

    if (rounds < 10) rounds = 10;

    const char* srcfile = "data";
    const char* kpfile = "mldsa_keypair.key";
    auto algos = to_ml_dsa_params();

    using clock = std::chrono::high_resolution_clock;
    auto start = clock::now();

    for (int i = 0; i < rounds; ++i)
    {
      int max = (1 << 21) + 1024;
      int size = rng_int(1000, max);
      set_rng_file_data(srcfile, size);

      auto idx = rng_int(0, (int)algos.size());
      auto algo = algos[idx];

      key_pair_param_info kppi;
      auto tf = rng_even();
      if (tf)
      {
        kppi = create_native_ml_dsa_key_pair();
        algo = kppi.algo;
      }
      else
      {
        kppi = create_native_ml_dsa_key_pair(algo);
      }

      auto& pubk = kppi.pub_key;
      auto& privk = kppi.priv_key;
      save_native_ml_dsa_key_pair(kppi, kpfile, true);
      auto info = load_native_ml_dsa_key_pair(kpfile);
      if (!kppi.Equals(info))
        throw std::runtime_error("KeyPairInfo mismatch (file)");

      std::vector<uint8_t> filenameBytes(strlen(srcfile));
      std::memcpy(filenameBytes.data(), srcfile, filenameBytes.size());

      uint8_t* sign_ptr = nullptr;
      int sign_length = 0;

      cerror_t err = pqc_mldsa_sign_file_aot(
        filenameBytes.data(), static_cast<int>(filenameBytes.size()),
        privk.data(), static_cast<int>(privk.size()),
        from_ml_dsa_param(algo),
        &sign_ptr, &sign_length);

      assert_error(err);

      std::vector<uint8_t> signature(sign_ptr, sign_ptr + sign_length);
      free_buffer_aot(sign_ptr);

      uint8_t ok = pqc_mldsa_verify_file_aot(
        filenameBytes.data(), static_cast<int>(filenameBytes.size()),
        pubk.data(), static_cast<int>(pubk.size()),
        signature.data(), static_cast<int>(signature.size()),
        &err);
      assert_error(err);

      if (!ok)
        throw std::runtime_error("Verify (file, kpi save/load) failed");

      if (rounds > 0 && i % std::max(1, rounds / 10) == 0)
        std::cout << ".";
    }

    auto end = clock::now();
    auto ms = std::chrono::duration_cast<std::chrono::milliseconds>(end - start).count();

    std::remove(srcfile);
    std::cout << " rounds = " << rounds
      << "; t = " << ms << "ms"
      << "; td = " << (ms / static_cast<double>(rounds)) << "ms\n";
  }
}