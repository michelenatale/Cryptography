#include "pch.h"

#include <iostream>
#include <chrono>
#include <vector>
#include <cstring>   // memcpy
#include <cstdlib>   // free

#include "bridge.h"

#include "cerror.h"
#include "variables.h"
#include "usi_ptr_t.h"
#include "crypto_utils_test.h"
#include "crypto_pqc_ml_dsa_utils_test.h"
#include "crypto_pqc_ml_dsa_bytes_test.h"
#include "crypto_pqc_ml_dsa_param_test.h"

namespace michele::natale::Tests
{

  void test_pqc_ml_dsa_create_key_pairs(int rounds)
  {
    std::cout << "test_pqc_ml_dsa_create_key_pairs_aot: ";

    using clock = std::chrono::high_resolution_clock;
    auto start = clock::now();

    for (int i = 0; i < rounds; ++i)
    {
      int pub_key_length = 0;
      int guid_id_length = 0;
      uint8_t mldsa_param = 0;
      int priv_key_length = 0;

      uint8_t* pub_key_ptr = nullptr;
      uint8_t* guid_id_ptr = nullptr;
      uint8_t* priv_key_ptr = nullptr;

      auto err = create_mldsa_key_pair_aot(
        &priv_key_ptr, &priv_key_length,
        &pub_key_ptr, &pub_key_length,
        &guid_id_ptr, &guid_id_length,
        &mldsa_param);

      assert_error(err);

      // privkey → std::vector<uint8_t>
      std::vector<uint8_t> privkey(priv_key_ptr, priv_key_ptr + priv_key_length);
      free_buffer_aot(priv_key_ptr); //free(priv_key_ptr);

      // pubkey → std::vector<uint8_t>
      std::vector<uint8_t> pubkey(pub_key_ptr, pub_key_ptr + pub_key_length);
      free_buffer_aot(pub_key_ptr); //free(pub_key_ptr);

      // guid → std::vector<uint8_t>
      std::vector<uint8_t> guid(guid_id_ptr, guid_id_ptr + guid_id_length);
      free_buffer_aot(guid_id_ptr); //free(guid_id_ptr);

      // param → enum
      auto param = to_ml_dsa_param(mldsa_param);

      if (rounds > 0 && i % std::max(1, rounds / 10) == 0)
        std::cout << ".";
    }

    auto end = clock::now();
    auto ms = std::chrono::duration_cast<std::chrono::milliseconds>(end - start).count();

    std::cout << " rounds = " << rounds
      << "; t = " << ms << "ms"
      << "; td = " << (ms / static_cast<double>(rounds)) << "ms\n";
  }


  void test_pqc_ml_dsa_create_key_pairs_param(int rounds)
  {
    std::cout << "test_pqc_ml_dsa_create_key_pairs_param_aot: ";

    auto params = to_ml_dsa_params(); // {44,65,87}

    using clock = std::chrono::high_resolution_clock;
    auto start = clock::now();

    for (int i = 0; i < rounds; ++i)
    {
      // Zufälligen Parameter auswählen
      int idx = rng_int(0, 3);
      uint8_t param = static_cast<uint8_t>(params[idx]);

      // Native call
      int guid_id_length = 0;
      int pub_key_length = 0;
      int priv_key_length = 0;

      uint8_t* guid_id_ptr = nullptr;
      uint8_t* pub_key_ptr = nullptr;
      uint8_t* priv_key_ptr = nullptr;

      cerror_t err = create_mldsa_key_pair_param_aot(
        param,
        &priv_key_ptr, &priv_key_length,
        &pub_key_ptr, &pub_key_length,
        &guid_id_ptr, &guid_id_length);
      assert_error(err);

      // Kopieren
      std::vector<uint8_t> privkey(priv_key_ptr, priv_key_ptr + priv_key_length);
      free_buffer_aot(priv_key_ptr); //free(priv_key_ptr);

      std::vector<uint8_t> pubkey(pub_key_ptr, pub_key_ptr + pub_key_length);
      free_buffer_aot(pub_key_ptr); //free(pub_key_ptr);

      std::vector<uint8_t> guid(guid_id_ptr, guid_id_ptr + guid_id_length);
      free_buffer_aot(guid_id_ptr); //free(guid_id_ptr);

      if (rounds > 0 && i % std::max(1, rounds / 10) == 0)
        std::cout << ".";
    }

    auto end = clock::now();
    auto ms = std::chrono::duration_cast<std::chrono::milliseconds>(end - start).count();

    std::cout << " rounds = " << rounds
      << "; t = " << ms << "ms"
      << "; td = " << (ms / static_cast<double>(rounds)) << "ms\n";
  }

  void test_pqc_ml_dsa_safe_load_key_pairs(int rounds)
  {
    std::cout << "test_pqc_ml_dsa_safe_load_key_pairs_aot: ";

    using clock = std::chrono::high_resolution_clock;
    auto start = clock::now();

    for (int i = 0; i < rounds; ++i)
    {
      // --- KeyPair erzeugen ---
      int guid_id_length = 0;
      int pub_key_length = 0;
      int priv_key_length = 0;
      uint8_t mldsa_param = 0;

      uint8_t* pub_key_ptr = nullptr;
      uint8_t* guid_id_ptr = nullptr;
      uint8_t* priv_key_ptr = nullptr;

      cerror_t err = create_mldsa_key_pair_aot(
        &priv_key_ptr, &priv_key_length,
        &pub_key_ptr, &pub_key_length,
        &guid_id_ptr, &guid_id_length,
        &mldsa_param);
      assert_error(err);

      std::vector<uint8_t> privkey(priv_key_ptr, priv_key_ptr + priv_key_length);
      free_buffer_aot(priv_key_ptr); //free(priv_key_ptr);

      std::vector<uint8_t> pubkey(pub_key_ptr, pub_key_ptr + pub_key_length);
      free_buffer_aot(pub_key_ptr); //free(pub_key_ptr);

      std::vector<uint8_t> guid_bytes(guid_id_ptr, guid_id_ptr + guid_id_length);
      free_buffer_aot(guid_id_ptr); //free(guid_id_ptr);

      auto param = static_cast<ml_dsa_param>(mldsa_param);

      // --- save private key or not ---
      bool with_priv_key = rng_even();

      const char* kpfile = "mldsa_keypair.key";
      int kpfile_len = (int)strlen(kpfile);

      err = save_pqc_mldsa_key_pair_aot(
        (const uint8_t*)kpfile, kpfile_len,
        privkey.data(), (int)privkey.size(),
        pubkey.data(), (int)pubkey.size(),
        guid_bytes.data(), (int)guid_bytes.size(),
        mldsa_param, with_priv_key);
      assert_error(err);

      // --- load KeyPair ---
      int pub_key_length2 = 0;
      int guid_id_length2 = 0;
      uint8_t mldsa_param2 = 0;
      int priv_key_length2 = 0;

      uint8_t* pub_key_ptr2 = nullptr;
      uint8_t* guid_id_ptr2 = nullptr;
      uint8_t* priv_key_ptr2 = nullptr;

      err = load_pqc_mldsa_key_pair_aot(
        (const uint8_t*)kpfile, kpfile_len,
        &priv_key_ptr2, &priv_key_length2,
        &pub_key_ptr2, &pub_key_length2,
        &guid_id_ptr2, &guid_id_length2,
        &mldsa_param2);
      assert_error(err);

      std::vector<uint8_t> privkey2(priv_key_ptr2, priv_key_ptr2 + priv_key_length2);
      free_buffer_aot(priv_key_ptr2); //free(priv_key_ptr2);

      std::vector<uint8_t> pubkey2(pub_key_ptr2, pub_key_ptr2 + pub_key_length2);
      free_buffer_aot(pub_key_ptr2); //free(pub_key_ptr2);

      std::vector<uint8_t> guid_bytes2(guid_id_ptr2, guid_id_ptr2 + guid_id_length2);
      free_buffer_aot(guid_id_ptr2); //free(guid_id_ptr2);

      auto param2 = static_cast<ml_dsa_param>(mldsa_param2);

      // --- Validierung ---
      if (with_priv_key)
      {
        if (privkey != privkey2)
          throw std::runtime_error("Private key mismatch");
      }
      else
      {
        if (!privkey2.empty())
          throw std::runtime_error("Private key should not be present");
      }

      if (pubkey != pubkey2)
        throw std::runtime_error("Public key mismatch");

      if (guid_bytes != guid_bytes2)
        throw std::runtime_error("GUID mismatch");

      if (param != param2)
        throw std::runtime_error("Parameter mismatch");

      if (rounds > 0 && i % std::max(1, rounds / 10) == 0)
        std::cout << ".";
    }

    auto end = clock::now();
    auto ms = std::chrono::duration_cast<std::chrono::milliseconds>(end - start).count();

    std::cout << " rounds = " << rounds
      << "; t = " << ms << "ms"
      << "; td = " << (ms / static_cast<double>(rounds)) << "ms\n";
  }

  void test_pqc_ml_dsa_single_signature(int rounds)
  {
    std::cout << "test_pqc_ml_dsa_single_signature_aot: ";

    if (rounds < 10) rounds = 10;
    auto algos = to_ml_dsa_params();

    using clock = std::chrono::high_resolution_clock;
    auto start = clock::now();

    for (int i = 0; i < rounds; ++i)
    {
      // Random Message 
      int size = rng_int(10, 128);
      auto message = rng_bytes(size);

      // Random Algo
      int idx = rng_int(0, (int)algos.size());
      auto algo = algos[idx];

      // KeyPair erzeugen
      key_pair_param_info kpip;
      int rnum = rng_int(0, INT32_MAX);

      if ((rnum & 1) == 0)
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

      // Sign
      uint8_t* sign_ptr = nullptr;
      int sign_length = 0;

      auto err = pqc_mldsa_sign_aot(
        message.data(), static_cast<int>(message.size()),
        privk.data(), static_cast<int>(privk.size()),
        from_ml_dsa_param(algo),
        &sign_ptr, &sign_length);
      assert_error(err);

      std::vector<uint8_t> signature(sign_ptr, sign_ptr + sign_length);
      free_buffer_aot(sign_ptr);

      // Verify
      uint8_t ok = pqc_mldsa_verify_aot(
        message.data(), static_cast<int>(message.size()),
        pubk.data(), static_cast<int>(pubk.size()),
        signature.data(), static_cast<int>(signature.size()),
        &err);
      assert_error(err);

      if (!ok)
        throw std::runtime_error("Verify failed");

      if (rounds > 0 && i % std::max(1, rounds / 10) == 0)
        std::cout << ".";
    }

    auto end = clock::now();
    auto ms = std::chrono::duration_cast<std::chrono::milliseconds>(end - start).count();

    std::cout << " rounds = " << rounds
      << "; t = " << ms << "ms"
      << "; td = " << (ms / static_cast<double>(rounds)) << "ms\n";
  }


  void test_pqc_ml_dsa_single_signature_kpi_save_load(int rounds)
  {
    std::cout << "test_pqc_ml_dsa_single_signature_kpi_sve_load_aot: ";

    if (rounds < 10) rounds = 10;
    auto algos = to_ml_dsa_params();

    using clock = std::chrono::high_resolution_clock;
    auto start = clock::now();

    for (int i = 0; i < rounds; ++i)
    {
      // Random Message 
      int size = rng_int(10, 128);
      auto message = rng_bytes(size);

      // Random Algo
      int idx = rng_int(0, (int)algos.size());
      auto algo = algos[idx];

      // KeyPair erzeugen
      key_pair_param_info kppi;
      const char* kpfile = "mldsa_keypair.key";
      int rnum = rng_int(0, INT32_MAX);

      if ((rnum & 1) == 0)
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

      // Sign
      int sign_length = 0;
      uint8_t* sign_ptr = nullptr;

      cerror_t err = pqc_mldsa_sign_aot(
        message.data(), (int)(message.size()),
        privk.data(), (int)(privk.size()),
        from_ml_dsa_param(algo),
        &sign_ptr, &sign_length);
      assert_error(err);

      std::vector<uint8_t> signature(sign_ptr, sign_ptr + sign_length);
      free_buffer_aot(sign_ptr);

      // Verify
      uint8_t ok = pqc_mldsa_verify_aot(
        message.data(), (int)(message.size()),
        pubk.data(), (int)(pubk.size()),
        signature.data(), (int)(signature.size()),
        &err);
      assert_error(err);

      if (!ok)
        throw std::runtime_error("Verify failed");

      if (rounds > 0 && i % std::max(1, rounds / 10) == 0)
        std::cout << ".";
    }

    auto end = clock::now();
    auto ms = std::chrono::duration_cast<std::chrono::milliseconds>(end - start).count();

    std::cout << " rounds = " << rounds
      << "; t = " << ms << "ms"
      << "; td = " << (ms / static_cast<double>(rounds)) << "ms\n";
  }
}