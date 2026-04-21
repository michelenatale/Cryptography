#include "pch.h"

#include <vector>
#include <string>
#include <chrono>
#include <iostream>

#include "bridge.h"
#include "cerror.h"
#include "usi_ptr_t.h"
#include "variables.h"
#include "crypto_utils_test.h"
#include "convert_encoding_test.h"
#include "crypto_pqc_ml_kem_file_test.h"
#include "crypto_pqc_ml_kem_bytes_file_test.h"

namespace michele::natale::Tests
{
  static const uint8_t str_t[] = u8"© Michele Natale 2026";

  struct loaded_key_pair_t
  {
    usi_ptr_t PrivKey;
    std::vector<uint8_t> PubKey;
    std::vector<uint8_t> GuidBytes;
    MLKemParam Param = MLKemParam::Ml_Kem_768;
    CryptionAlgorithm CryptoAlgo = CryptionAlgorithm::AES_GCM;
  };

  //Prototypes
  loaded_key_pair_t load_key_pair(const uint8_t* kpfile, int kpfile_len);
  void create_save_key_pair(const uint8_t* kpfile, int kpfile_len, bool with_priv_key = true);

  void test_pqc_ml_kem_enc_decryption_file(int rounds)
  {
    using namespace std::chrono;
    std::cout << "test_pqc_ml_kem_enc_decryption_file_aot: ";

    const std::string src = "data";
    const std::string dest = "cipher";
    const std::string srcr = "datar";

    auto start = high_resolution_clock::now();

    for (int i = 0; i < rounds; ++i)
    {
      // random file length
      int max = (1 << 21) + 1024;
      int flength = rng_int(1, max);
      set_rng_file_data(src, flength);

      // --- Alice creates keypair ---
      uint8_t mlkem_param = 0, crypto_algo = 0;
      int alice_priv_key_length = 0, alice_pub_key_length = 0, alice_guid_id_length = 0;
      uint8_t* alice_priv_key_ptr = nullptr, * alice_pub_key_ptr = nullptr, * alice_guid_id_ptr = nullptr;

      auto err = create_mlkem_key_pair_aot(
        &alice_priv_key_ptr, &alice_priv_key_length,
        &alice_pub_key_ptr, &alice_pub_key_length,
        &alice_guid_id_ptr, &alice_guid_id_length,
        &mlkem_param, &crypto_algo);
      assert_error(err);

      usi_ptr_t aliceprivkey(to_bytes(alice_priv_key_ptr, alice_priv_key_length));
      free_buffer_aot(alice_priv_key_ptr);

      auto alicepubkey = to_bytes(alice_pub_key_ptr, alice_pub_key_length);
      free_buffer_aot(alice_pub_key_ptr);

      auto aliceguid = to_bytes(alice_guid_id_ptr, alice_guid_id_length);
      free_buffer_aot(alice_guid_id_ptr);

      CryptionAlgorithm cryptoalgo = static_cast<CryptionAlgorithm>(crypto_algo);
      MLKemParam param = to_ml_kem_algorithm(static_cast<MLKemParam>(mlkem_param));

      // --- Bob derives shared key + capsulation ---
      uint8_t* bob_shared_key_ptr = nullptr, * bob_capsulation_ptr = nullptr;
      int bob_shared_key_length = 0, bob_capsulation_length = 0;

      err = to_pqc_mlkem_capsulation_from_pub_key_aot(
        alicepubkey.data(), alice_pub_key_length, mlkem_param,
        &bob_shared_key_ptr, &bob_shared_key_length,
        &bob_capsulation_ptr, &bob_capsulation_length);
      assert_error(err);

      usi_ptr_t bobsharedkey(to_bytes(bob_shared_key_ptr, bob_shared_key_length));
      free_buffer_aot(bob_shared_key_ptr);

      auto bobcapsulation = to_bytes(bob_capsulation_ptr, bob_capsulation_length);
      free_buffer_aot(bob_capsulation_ptr);

      // associated data
      using namespace michele::natale::Cpp::Services;
      int size = rng_int(PQC_ML_KEM_MIN_PLAIN_SIZE, 64);
      std::vector<uint8_t> associated;
      if (rng_even())
        associated.assign(str_t, str_t + sizeof(str_t) - 1);
      else associated = rng_bytes(size);

      // --- Alice encrypts file ---
      err = pqc_mlkem_encryption_file_aot(
        reinterpret_cast<const uint8_t*>(src.data()), (int)src.size(),
        reinterpret_cast<const uint8_t*>(dest.data()), (int)dest.size(),
        aliceprivkey.ToBytes(), aliceprivkey.Length(),
        bobcapsulation.data(), (int)bobcapsulation.size(),
        associated.data(), (int)associated.size(),
        mlkem_param, crypto_algo);
      assert_error(err);

      // --- Bob decrypts file ---
      err = pqc_mlkem_decryption_file_aot(
        reinterpret_cast<const uint8_t*>(dest.data()), (int)dest.size(),
        reinterpret_cast<const uint8_t*>(srcr.data()), (int)srcr.size(),
        bobsharedkey.ToBytes(), bobsharedkey.Length(),
        associated.data(), (int)associated.size());
      assert_error(err);

      if (!equal_files_aot(
        reinterpret_cast<const uint8_t*>(src.data()), (int)src.length(),
        reinterpret_cast<const uint8_t*>(srcr.data()), (int)srcr.length(), &err))
        throw std::runtime_error("file mismatch");
      assert_error(err);

      if (i % (rounds / 10) == 0)
        std::cout << ".";
    }

    auto end = high_resolution_clock::now();
    double t = duration<double, std::milli>(end - start).count();
    std::cout << " rounds = " << rounds << ", t = " << t << "ms, td = " << (t / rounds) << "ms\n";
  }

  void test_pqc_ml_kem_enc_decryption_kpf_file(int rounds)
  {
    using namespace std::chrono;
    std::cout << "test_pqc_ml_kem_enc_decryption_kpf_file_aot: ";

    static const uint8_t alice_kpfile[] = "alice_mlkem_keypair.key";
    create_save_key_pair(alice_kpfile, sizeof(alice_kpfile) - 1);

    const std::string src = "data";
    const std::string dest = "cipher";
    const std::string srcr = "datar";

    auto start = high_resolution_clock::now();

    for (int i = 0; i < rounds; ++i)
    {
      int max = (1 << 21) + 1024;
      int flength = rng_int(0, max);
      set_rng_file_data(src, flength);

      // load Alice keypair
      auto alice = load_key_pair(alice_kpfile, sizeof(alice_kpfile) - 1);
      usi_ptr_t& aliceprivkey = alice.PrivKey;
      auto& alicepubkey = alice.PubKey;
      MLKemParam param = alice.Param;
      CryptionAlgorithm cryptoalgo = alice.CryptoAlgo;

      // associated
      using namespace michele::natale::Cpp::Services;
      int size = rng_int(PQC_ML_KEM_MIN_PLAIN_SIZE, 64);
      std::vector<uint8_t> associated;
      if (rng_even())
        associated.assign(str_t, str_t + sizeof(str_t) - 1);
      else associated = rng_bytes(size);

      // Bob derives shared key + capsulation
      uint8_t* bob_shared_key_ptr = nullptr, * bob_capsulation_ptr = nullptr;
      int bob_shared_key_length = 0, bob_capsulation_length = 0;

      auto err = to_pqc_mlkem_capsulation_from_pub_key_aot(
        alicepubkey.data(), (int)alicepubkey.size(),
        static_cast<uint8_t>(param),
        &bob_shared_key_ptr, &bob_shared_key_length,
        &bob_capsulation_ptr, &bob_capsulation_length);
      assert_error(err);

      usi_ptr_t bobsharedkey(to_bytes(bob_shared_key_ptr, bob_shared_key_length));
      free_buffer_aot(bob_shared_key_ptr);

      auto bobcapsulation = to_bytes(bob_capsulation_ptr, bob_capsulation_length);
      free_buffer_aot(bob_capsulation_ptr);

      // Alice encrypts file
      err = pqc_mlkem_encryption_file_aot(
        reinterpret_cast<const uint8_t*>(src.data()), (int)src.size(),
        reinterpret_cast<const uint8_t*>(dest.data()), (int)dest.size(),
        aliceprivkey.ToBytes(), aliceprivkey.Length(),
        bobcapsulation.data(), (int)bobcapsulation.size(),
        associated.data(), (int)associated.size(),
        static_cast<uint8_t>(param),
        static_cast<uint8_t>(cryptoalgo));
      assert_error(err);

      // Bob decrypts file
      err = pqc_mlkem_decryption_file_aot(
        reinterpret_cast<const uint8_t*>(dest.data()), (int)dest.size(),
        reinterpret_cast<const uint8_t*>(srcr.data()), (int)srcr.size(),
        bobsharedkey.ToBytes(), (int)bobsharedkey.Length(),
        associated.data(), (int)associated.size());
      assert_error(err);

      if (!equal_files_aot(
        reinterpret_cast<const uint8_t*>(src.data()), (int)src.length(),
        reinterpret_cast<const uint8_t*>(srcr.data()), (int)srcr.length(), &err))
      throw std::runtime_error("file mismatch");
      assert_error(err);

      if (i % (rounds / 10) == 0)
        std::cout << ".";
    }

    auto end = high_resolution_clock::now();
    double t = duration<double, std::milli>(end - start).count();
    std::cout << " rounds = " << rounds << ", t = " << t << "ms, td = " << (t / rounds) << "ms\n\n";
  }

  void create_save_key_pair(const uint8_t* kpfile, int kpfile_len, bool with_priv_key)
  {
    uint8_t* priv_key_ptr = nullptr, * pub_key_ptr = nullptr, * guid_id_ptr = nullptr;
    int priv_key_length = 0, pub_key_length = 0, guid_id_length = 0;
    uint8_t mlkem_param = 0, crypto_algo = 0;

    auto err = create_mlkem_key_pair_aot(
      &priv_key_ptr, &priv_key_length,
      &pub_key_ptr, &pub_key_length,
      &guid_id_ptr, &guid_id_length,
      &mlkem_param, &crypto_algo);
    assert_error(err);

    usi_ptr_t privkey(to_bytes(priv_key_ptr, priv_key_length));
    free_buffer_aot(priv_key_ptr);

    auto pubkey = to_bytes(pub_key_ptr, pub_key_length);
    free_buffer_aot(pub_key_ptr);

    auto guid_bytes = to_bytes(guid_id_ptr, guid_id_length);
    free_buffer_aot(guid_id_ptr);

    err = save_pqc_mlkem_key_pair_aot(
      kpfile, kpfile_len,
      privkey.ToBytes(), privkey.Length(),
      pubkey.data(), (int)pubkey.size(),
      guid_bytes.data(), (int)guid_bytes.size(),
      mlkem_param, crypto_algo,
      with_priv_key ? 1 : 0);
    assert_error(err);
  }

  loaded_key_pair_t load_key_pair(const uint8_t* kpfile, int kpfile_len)
  {
    uint8_t* priv_key_ptr = nullptr, * pub_key_ptr = nullptr, * guid_id_ptr = nullptr;
    int priv_key_length = 0, pub_key_length = 0, guid_id_length = 0;
    uint8_t mlkem_param = 0, crypto_algo = 0;

    auto err = load_pqc_mlkem_key_pair_aot(
      kpfile, kpfile_len,
      &priv_key_ptr, &priv_key_length,
      &pub_key_ptr, &pub_key_length,
      &guid_id_ptr, &guid_id_length,
      &mlkem_param, &crypto_algo);
    assert_error(err);

    loaded_key_pair_t out;

    out.PrivKey = usi_ptr_t(to_bytes(priv_key_ptr, priv_key_length));
    free_buffer_aot(priv_key_ptr);

    out.PubKey = to_bytes(pub_key_ptr, pub_key_length);
    free_buffer_aot(pub_key_ptr);

    out.GuidBytes = to_bytes(guid_id_ptr, guid_id_length);
    free_buffer_aot(guid_id_ptr);

    out.Param = to_ml_kem_algorithm(static_cast<MLKemParam>(mlkem_param));
    out.CryptoAlgo = static_cast<CryptionAlgorithm>(crypto_algo);

    return out;
  }
}