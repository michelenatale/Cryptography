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
#include "variables.h"
#include "usi_ptr_t.h"
#include "convert_encoding_test.h"
#include "crypto_pqc_ml_kem_bytes_test.h"
#include "crypto_pqc_ml_kem_bytes_file_test.h"


namespace michele::natale::Tests
{

  static const uint8_t str_t[] = u8"© Michele Natale 2026";

  void test_pqc_ml_kem_create_key_pairs(int rounds)
  {
    using namespace std::chrono;
    std::cout << "test_pqc_ml_kem_create_key_pairs_aot: ";

    auto start = high_resolution_clock::now();

    for (int i = 0; i < rounds; ++i)
    {
      int pub_key_length = 0;
      int guid_id_length = 0;
      int priv_key_length = 0;

      uint8_t mlkem_param = 0;
      uint8_t crypto_algo = 0;

      uint8_t* guid_id_ptr = nullptr;
      uint8_t* pub_key_ptr = nullptr;
      uint8_t* priv_key_ptr = nullptr;

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

      CryptionAlgorithm cryptoalgo = static_cast<CryptionAlgorithm>(crypto_algo);
      MLKemParam param = to_ml_kem_algorithm(static_cast<MLKemParam>(mlkem_param));

      if (i % (rounds / 10) == 0)
        std::cout << ".";
    }

    auto end = high_resolution_clock::now();
    double t = (double)duration_cast<milliseconds>(end - start).count();


    std::cout << " rounds = " << rounds << "; t = " << t
      << "ms; td = " << (t / rounds) << "ms\n";
  }

  void test_pqc_ml_kem_create_key_pairs_param(int rounds)
  {
    using namespace std::chrono;
    std::cout << "test_pqc_ml_kem_create_key_pairs_param_aot: ";

    auto start = high_resolution_clock::now();

    for (int i = 0; i < rounds; ++i)
    {
      // ML-KEM Parameter Auswahl 
      MLKemParam mlkem_param_arr[] = 
      {
        MLKemParam::Ml_Kem_512,
        MLKemParam::Ml_Kem_768,
        MLKemParam::Ml_Kem_1024,
      };
      int idx = rand_int(0, 3);
      uint8_t param = static_cast<uint8_t>(
        from_ml_kem_algorithm(mlkem_param_arr[idx]));

      // CryptionAlgorithm Auswahl 
      CryptionAlgorithm cas[] = 
      {
        CryptionAlgorithm::AES,
        CryptionAlgorithm::AES_GCM,
        CryptionAlgorithm::CHACHA20_POLY1305,
      };

      idx = rand_int(0, 3);
      uint8_t cryptoalgo = static_cast<uint8_t>(cas[idx]);

      int pub_key_length = 0;
      int guid_id_length = 0;
      int priv_key_length = 0;

      uint8_t* pub_key_ptr = nullptr;
      uint8_t* guid_id_ptr = nullptr;
      uint8_t* priv_key_ptr = nullptr;

      auto err = create_mlkem_key_pair_param_aot(
        param, cryptoalgo,
        &priv_key_ptr, &priv_key_length,
        &pub_key_ptr, &pub_key_length,
        &guid_id_ptr, &guid_id_length);
      assert_error(err);

      usi_ptr_t privkey(to_bytes(priv_key_ptr, priv_key_length));
      free_buffer_aot(priv_key_ptr);

      auto pubkey = to_bytes(pub_key_ptr, pub_key_length);
      free_buffer_aot(pub_key_ptr);

      auto guid_bytes = to_bytes(guid_id_ptr, guid_id_length);
      free_buffer_aot(guid_id_ptr);

      if (i % (rounds / 10) == 0)
        std::cout << ".";
    }

    auto end = high_resolution_clock::now();
    double t = (double)duration_cast<milliseconds>(end - start).count();
    std::cout << " rounds = " << rounds << "; t = " << t
      << "ms; td = " << (t / rounds) << "ms\n";
  }

  void test_pqc_ml_kem_safe_load_key_pairs(int rounds)
  {
    using namespace std::chrono;
    std::cout << "test_pqc_ml_kem_safe_load_key_pairs_Aot: ";

    auto start = high_resolution_clock::now();
    static const uint8_t kpfile[] = "mlkem_keypair.key";

    for (int i = 0; i < rounds; ++i)
    {
      int guid_id_length = 0;
      int pub_key_length = 0;
      int priv_key_length = 0;

      uint8_t mlkem_param = 0;
      uint8_t crypto_algo = 0;

      uint8_t* pub_key_ptr = nullptr;
      uint8_t* guid_id_ptr = nullptr;
      uint8_t* priv_key_ptr = nullptr;

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

      CryptionAlgorithm cryptoalgo = static_cast<CryptionAlgorithm>(crypto_algo);
      MLKemParam param = to_ml_kem_algorithm(static_cast<MLKemParam>(mlkem_param));

      bool with_priv_key = rand_even();

      err = save_pqc_mlkem_key_pair_aot(
        kpfile, sizeof(kpfile) - 1,
        privkey.ToBytes(), privkey.Length(),
        pubkey.data(), (int)(pubkey.size()),
        guid_bytes.data(), (int)(guid_bytes.size()),
        mlkem_param, crypto_algo,
        with_priv_key ? 1 : 0);
      assert_error(err);

      int guid_id_length2 = 0;
      int pub_key_length2 = 0;
      int priv_key_length2 = 0;

      uint8_t mlkem_param2 = 0;
      uint8_t crypto_algo2 = 0;

      uint8_t* guid_id_ptr2 = nullptr;
      uint8_t* pub_key_ptr2 = nullptr;
      uint8_t* priv_key_ptr2 = nullptr;

      err = load_pqc_mlkem_key_pair_aot(
        kpfile, sizeof(kpfile) - 1,
        &priv_key_ptr2, &priv_key_length2,
        &pub_key_ptr2, &pub_key_length2,
        &guid_id_ptr2, &guid_id_length2,
        &mlkem_param2, &crypto_algo2);
      assert_error(err);

      usi_ptr_t privkey2(to_bytes(priv_key_ptr2, priv_key_length2));
      free_buffer_aot(priv_key_ptr2);

      auto pubkey2 = to_bytes(pub_key_ptr2, pub_key_length2);
      free_buffer_aot(pub_key_ptr2);

      auto guid_bytes2 = to_bytes(guid_id_ptr2, guid_id_length2);
      free_buffer_aot(guid_id_ptr2);

      CryptionAlgorithm cryptoalgo2 = static_cast<CryptionAlgorithm>(crypto_algo2);
      MLKemParam param2 = to_ml_kem_algorithm(static_cast<MLKemParam>(mlkem_param2));

      if (with_priv_key && !privkey.Equality(privkey2))
        throw std::runtime_error("privkey mismatch");
      else if (!with_priv_key && privkey2.Length() != 0)
        throw std::runtime_error("privkey should be empty");

      if (pubkey.size() != pubkey2.size() ||
        !std::equal(pubkey.begin(), pubkey.end(), pubkey2.begin()))
        throw std::runtime_error("pubkey mismatch");

      if (guid_bytes.size() != guid_bytes2.size() ||
        !std::equal(guid_bytes.begin(), guid_bytes.end(), guid_bytes2.begin()))
        throw std::runtime_error("guid mismatch");

      if (cryptoalgo != cryptoalgo2)
        throw std::runtime_error("cryptoalgo mismatch");

      if (param != param2)
        throw std::runtime_error("param mismatch");

      if (i % (rounds / 10) == 0)
        std::cout << ".";
    }

    auto end = high_resolution_clock::now();
    double t = (double)duration_cast<milliseconds>(end - start).count();
    std::cout << " rounds = " << rounds << "; t = " << t
      << "ms; td = " << (t / rounds) << "ms\n\n";
  }

  void test_pqc_ml_kem_capsulation_shared_key_with_public_key(int rounds)
  {
    using namespace std::chrono;
    std::cout << "test_pqc_ml_kem_capsulation_shared_key_with_public_key_aot: ";

    auto start = high_resolution_clock::now();

    for (int i = 0; i < rounds; ++i)
    {
      uint8_t mlkem_param = 0;
      uint8_t crypto_algo = 0;

      int alice_pub_key_length = 0;
      int alice_guid_id_length = 0;
      int alice_priv_key_length = 0;

      uint8_t* alice_pub_key_ptr = nullptr;
      uint8_t* alice_guid_id_ptr = nullptr;
      uint8_t* alice_priv_key_ptr = nullptr;

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

      auto aliceguid_bytes = to_bytes(alice_guid_id_ptr, alice_guid_id_length);
      free_buffer_aot(alice_guid_id_ptr);

      CryptionAlgorithm cryptoalgo = static_cast<CryptionAlgorithm>(crypto_algo);
      MLKemParam param = to_ml_kem_algorithm(static_cast<MLKemParam>(mlkem_param));

      int bob_shared_key_length = 0;
      int bob_capsulation_length = 0;

      uint8_t* bob_shared_key_ptr = nullptr;
      uint8_t* bob_capsulation_ptr = nullptr;

      err = to_pqc_mlkem_capsulation_from_pub_key_aot(
        alicepubkey.data(), (int)(alicepubkey.size()),
        mlkem_param,
        &bob_shared_key_ptr, &bob_shared_key_length,
        &bob_capsulation_ptr, &bob_capsulation_length);
      assert_error(err);

      usi_ptr_t bobsharedkey(to_bytes(bob_shared_key_ptr, bob_shared_key_length));
      free_buffer_aot(bob_shared_key_ptr);

      auto bobcapsulation = to_bytes(bob_capsulation_ptr, bob_capsulation_length);
      free_buffer_aot(bob_capsulation_ptr);

      if (bobsharedkey.IsEmpty())
        throw std::runtime_error("bobsharedkey empty");

      if (is_null_or_empty(bobcapsulation))
        throw std::runtime_error("bobcapsulation empty");

      if (i % (rounds / 10) == 0)
        std::cout << ".";
    }

    auto end = high_resolution_clock::now();
    double t = (double)duration_cast<milliseconds>(end - start).count();
    std::cout << " rounds = " << rounds << "; t = " << t
      << "ms; td = " << (t / rounds) << "ms\n";
  }

  void test_pqc_ml_kem_sharedKey_from_capsualtion_private_key(int rounds)
  {
    using namespace std::chrono;
    std::cout << "test_pqc_ml_kem_sharedKey_from_capsualtion_private_key_aot: ";

    auto start = high_resolution_clock::now();

    for (int i = 0; i < rounds; ++i)
    {
      uint8_t mlkem_param = 0;
      uint8_t crypto_algo = 0;

      int alice_pub_key_length = 0;
      int alice_guid_id_length = 0;
      int alice_priv_key_length = 0;

      uint8_t* alice_guid_id_ptr = nullptr;
      uint8_t* alice_pub_key_ptr = nullptr;
      uint8_t* alice_priv_key_ptr = nullptr;

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

      auto aliceguid_bytes = to_bytes(alice_guid_id_ptr, alice_guid_id_length);
      free_buffer_aot(alice_guid_id_ptr);

      CryptionAlgorithm cryptoalgo = static_cast<CryptionAlgorithm>(crypto_algo);
      MLKemParam param = to_ml_kem_algorithm(static_cast<MLKemParam>(mlkem_param));

      int bob_shared_key_length = 0;
      int bob_capsulation_length = 0;

      uint8_t* bob_shared_key_ptr = nullptr;
      uint8_t* bob_capsulation_ptr = nullptr;

      err = to_pqc_mlkem_capsulation_from_pub_key_aot(
        alicepubkey.data(), (int)(alicepubkey.size()),
        mlkem_param,
        &bob_shared_key_ptr, &bob_shared_key_length,
        &bob_capsulation_ptr, &bob_capsulation_length);
      assert_error(err);

      usi_ptr_t bobsharedkey(to_bytes(bob_shared_key_ptr, bob_shared_key_length));
      free_buffer_aot(bob_shared_key_ptr);

      auto bobcapsulation = to_bytes(bob_capsulation_ptr, bob_capsulation_length);
      free_buffer_aot(bob_capsulation_ptr);

      int alice_shared_length = 0;
      uint8_t* alice_shared_key_ptr = nullptr;

      err = to_pqc_mlkem_shared_key_from_private_key_aot(
        aliceprivkey.ToBytes(), aliceprivkey.Length(),
        bobcapsulation.data(), (int)(bobcapsulation.size()),
        mlkem_param,
        &alice_shared_key_ptr, &alice_shared_length);
      assert_error(err);

      usi_ptr_t alicesharedkey(to_bytes(alice_shared_key_ptr, alice_shared_length));
      free_buffer_aot(alice_shared_key_ptr);

      if (!bobsharedkey.Equality(alicesharedkey))
        throw std::runtime_error("shared key mismatch");

      if (i % (rounds / 10) == 0)
        std::cout << ".";
    }

    auto end = high_resolution_clock::now();
    double t = (double)duration_cast<milliseconds>(end - start).count();
    std::cout << " rounds = " << rounds << "; t = " << t
      << "ms; td = " << (t / rounds) << "ms\n\n";
  }

  void test_pqc_ml_kem_enc_decryption_bytes(int rounds)
  {
    using namespace std::chrono;
    std::cout << "test_pqc_ml_kem_enc_decryption_bytes_aot: ";

    auto start = high_resolution_clock::now();

    for (int i = 0; i < rounds; ++i)
    {
      uint8_t mlkem_param = 0;
      uint8_t crypto_algo = 0;

      int alice_guid_id_length = 0;
      int alice_pub_key_length = 0;
      int alice_priv_key_length = 0;

      uint8_t* alice_guid_id_ptr = nullptr;
      uint8_t* alice_pub_key_ptr = nullptr;
      uint8_t* alice_priv_key_ptr = nullptr;

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

      auto aliceguid_bytes = to_bytes(alice_guid_id_ptr, alice_guid_id_length);
      free_buffer_aot(alice_guid_id_ptr);

      CryptionAlgorithm cryptoalgo = static_cast<CryptionAlgorithm>(crypto_algo);
      MLKemParam param = to_ml_kem_algorithm(static_cast<MLKemParam>(mlkem_param));

      int bob_shared_key_length = 0;
      int bob_capsulation_length = 0;
      uint8_t* bob_shared_key_ptr = nullptr;
      uint8_t* bob_capsulation_ptr = nullptr;

      err = to_pqc_mlkem_capsulation_from_pub_key_aot(
        alicepubkey.data(), (int)(alicepubkey.size()),
        mlkem_param,
        &bob_shared_key_ptr, &bob_shared_key_length,
        &bob_capsulation_ptr, &bob_capsulation_length);
      assert_error(err);

      usi_ptr_t bobsharedkey(to_bytes(bob_shared_key_ptr, bob_shared_key_length));
      free_buffer_aot(bob_shared_key_ptr);

      auto bobcapsulation = to_bytes(bob_capsulation_ptr, bob_capsulation_length);
      free_buffer_aot(bob_capsulation_ptr);

      using namespace michele::natale::Cpp::Services;
      int size = rand_int(PQC_ML_KEM_MIN_PLAIN_SIZE, 128);
      auto message = rng_bytes(size);

      size = rand_int(PQC_ML_KEM_MIN_PLAIN_SIZE, 64);
      std::vector<uint8_t> associated;
      if (rand_even())
        associated.assign(str_t, str_t + sizeof(str_t) - 1);
      else associated = rng_bytes(size);

      uint8_t* cipher_ptr = nullptr;
      int cipher_length = 0;

      err = pqc_mlkem_encryption_aot(
        message.data(), (int)(message.size()),
        aliceprivkey.ToBytes(), aliceprivkey.Length(),
        bobcapsulation.data(), (int)(bobcapsulation.size()),
        associated.data(), (int)(associated.size()),
        mlkem_param, crypto_algo,
        &cipher_ptr, &cipher_length);
      assert_error(err);

      auto cipher = to_bytes(cipher_ptr, cipher_length);
      free_buffer_aot(cipher_ptr);

      uint8_t* decipher_ptr = nullptr;
      int decipher_length = 0;

      err = pqc_mlkem_decryption_aot(
        cipher.data(), (int)(cipher.size()),
        bobsharedkey.ToBytes(), bobsharedkey.Length(),
        associated.data(), (int)(associated.size()),
        mlkem_param, crypto_algo,
        &decipher_ptr, &decipher_length);
      assert_error(err);

      auto decipher = to_bytes(decipher_ptr, decipher_length);
      free_buffer_aot(decipher_ptr);

      if (message.size() != decipher.size() ||
        !std::equal(message.begin(), message.end(), decipher.begin()))
        throw std::runtime_error("message mismatch");

      if (i % (rounds / 10) == 0)
        std::cout << ".";
    }

    auto end = high_resolution_clock::now();
    double t = (double)duration_cast<milliseconds>(end - start).count();
    std::cout << " rounds = " << rounds << "; t = " << t
      << "ms; td = " << (t / rounds) << "ms\n";
  }

  void test_pqc_ml_kem_enc_decryption_bytes_stress()
  {
    using namespace std::chrono;
    std::cout << "test_pqc_ml_kem_enc_decryption_bytes_stress_aot: ";

    auto start = high_resolution_clock::now();

    uint8_t mlkem_param = 0;
    uint8_t crypto_algo = 0;

    int alice_guid_id_length = 0;
    int alice_pub_key_length = 0;
    int alice_priv_key_length = 0;

    uint8_t* alice_guid_id_ptr = nullptr;
    uint8_t* alice_pub_key_ptr = nullptr;
    uint8_t* alice_priv_key_ptr = nullptr;

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

    auto aliceguid_bytes = to_bytes(alice_guid_id_ptr, alice_guid_id_length);
    free_buffer_aot(alice_guid_id_ptr);

    CryptionAlgorithm cryptoalgo = static_cast<CryptionAlgorithm>(crypto_algo);
    MLKemParam param = to_ml_kem_algorithm(static_cast<MLKemParam>(mlkem_param));

    int bob_shared_key_length = 0;
    int bob_capsulation_length = 0;

    uint8_t* bob_shared_key_ptr = nullptr;
    uint8_t* bob_capsulation_ptr = nullptr;

    err = to_pqc_mlkem_capsulation_from_pub_key_aot(
      alicepubkey.data(), alice_pub_key_length,
      mlkem_param,
      &bob_shared_key_ptr, &bob_shared_key_length,
      &bob_capsulation_ptr, &bob_capsulation_length);
    assert_error(err);

    usi_ptr_t bobsharedkey(to_bytes(bob_shared_key_ptr, bob_shared_key_length));
    free_buffer_aot(bob_shared_key_ptr);

    auto bobcapsulation = to_bytes(bob_capsulation_ptr, bob_capsulation_length);
    free_buffer_aot(bob_capsulation_ptr);

    //IMPORTANT: The minimum length must be at least 8, 
    //           and the maximum length must not exceed 2^20.
    using namespace michele::natale::Cpp::Services;
    int msize = PQC_ML_KEM_MAX_PLAIN_SIZE; //2^20
    auto message = rng_bytes(msize);

    int size = rand_int(PQC_ML_KEM_MIN_PLAIN_SIZE, 64);
    std::vector<uint8_t> associated;
    if (rand_even())
      associated.assign(str_t, str_t + sizeof(str_t) - 1);
    else associated = rng_bytes(size);

    int cipher_length = 0;
    uint8_t* cipher_ptr = nullptr;

    err = pqc_mlkem_encryption_aot(
      message.data(), msize,
      aliceprivkey.ToBytes(), alice_priv_key_length,
      bobcapsulation.data(), bob_capsulation_length,
      associated.data(), size,
      mlkem_param, crypto_algo,
      &cipher_ptr, &cipher_length);
    assert_error(err);

    auto cipher = to_bytes(cipher_ptr, cipher_length);
    free_buffer_aot(cipher_ptr);

    int decipher_length = 0;
    uint8_t* decipher_ptr = nullptr;

    err = pqc_mlkem_decryption_aot(
      cipher.data(), cipher_length,
      bobsharedkey.ToBytes(), bob_shared_key_length,
      associated.data(), size,
      mlkem_param, crypto_algo,
      &decipher_ptr, &decipher_length);
    assert_error(err);

    auto decipher = to_bytes(decipher_ptr, decipher_length);
    free_buffer_aot(decipher_ptr);

    if (message.size() != decipher.size() ||
      !std::equal(message.begin(), message.end(), decipher.begin()))
      throw std::runtime_error("message mismatch");

    auto end = high_resolution_clock::now();
    double t = (double)duration_cast<milliseconds>(end - start).count();

    std::cout << " cryptoalgo = " << (int)(cryptoalgo)
      << "; mlkemparam = " << (int)(param)
      << "; size = " << msize
      << "; t = " << t << "ms\n\n";
  }
}