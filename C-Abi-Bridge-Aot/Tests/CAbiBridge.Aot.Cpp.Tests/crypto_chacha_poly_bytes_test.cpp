#include "pch.h"

#include <vector>
#include <random>
#include <chrono>
#include <iostream>
#include <cstring>
#include <stdexcept>

#include "bright.h"
#include "variables.h"
#include "crypto_chacha_poly_test.h"


namespace michele::natale::Tests
{
  using namespace michele::natale::Cpp;
  void test_chacha_poly_bytes(int rounds)
  {
    using namespace std::chrono;

    std::cout << "test_chacha_poly_bytes_aot: ";

    std::mt19937 rng(std::random_device{}());
    std::uniform_int_distribution<int> pw_dist(
      Services::MIN_PW_SIZE, Services::MAX_PW_SIZE);
    std::uniform_int_distribution<int> salt_dist(
      Services::MIN_SALT_SIZE, Services::MAX_SALT_SIZE);
    std::uniform_int_distribution<int> plain_dist(
      Services::CHACHA_POLY_MIN_PLAIN_SIZE, 
      Services::CHACHA_POLY_MAX_PLAIN_SIZE);

    auto start = high_resolution_clock::now();

    for (int i = 0; i < rounds; i++)
    {
      int pw_size = pw_dist(rng);
      auto pw = rng_bytes(pw_size);

      int salt_size = salt_dist(rng);
      auto salt = rng_bytes(salt_size);

      std::vector<uint8_t> key(Services::CHACHA_POLY_MAX_KEY_SIZE);
      auto err = pbkdf2_aot(
        pw.data(), (int)pw.size(),
        salt.data(), (int)salt.size(),
        Services::MIN_ITERATION,
        key.data(), (int)key.size());
      assert_error(err);

      auto nonce = rng_bytes(Services::CHACHA_POLY_NONCE_SIZE);
      auto associat = std::vector<uint8_t>(
        reinterpret_cast<const uint8_t*>("© Michele Natale 2021"),
        reinterpret_cast<const uint8_t*>("© Michele Natale 2021") + strlen("© Michele Natale 2021"));

      int plain_size = plain_dist(rng);
      auto plain = rng_bytes(plain_size);

      // --- Encrypt ---
      void* cipher_ptr = nullptr;
      int cipher_len = 0;

      err = chacha20_poly1305_encrypt_aot(
        plain.data(), (int)plain.size(),
        key.data(), (int)key.size(),
        associat.data(), (int)associat.size(),
        &cipher_ptr, &cipher_len);
      assert_error(err);

      if (!cipher_ptr)
        throw std::runtime_error("Null pointer returned");

      std::vector<uint8_t> cipher((uint8_t*)cipher_ptr, (uint8_t*)cipher_ptr + cipher_len);
      free_buffer_aot(cipher_ptr);

      // --- Decrypt ---
      void* decipher_ptr = nullptr;
      int decipher_len = 0;

      err = chacha20_poly1305_decrypt_aot(
        cipher.data(), (int)cipher.size(),
        key.data(), (int)key.size(),
        associat.data(), (int)associat.size(),
        &decipher_ptr, &decipher_len);
      assert_error(err);

      if (!decipher_ptr)
        throw std::runtime_error("Null pointer returned");

      std::vector<uint8_t> decipher((uint8_t*)decipher_ptr, (uint8_t*)decipher_ptr + decipher_len);
      free_buffer_aot(decipher_ptr);

      // Compare
      if (decipher.size() != plain.size() ||
        memcmp(decipher.data(), plain.data(), plain.size()) != 0)
      {
        throw std::runtime_error("Chacha20Poly1305 mismatch");
      }

      if (i % (rounds / 10) == 0)
        std::cout << ".";
    }

    auto end = high_resolution_clock::now();
    auto ms = duration_cast<milliseconds>(end - start).count();

    std::cout << " rounds = " << rounds
      << "; t = " << ms << "ms; td = " << (ms / rounds) << "ms\n";
  }


  void test_chacha_poly_bytes_stress()
  {
    using namespace std::chrono;

    std::cout << "test_chacha_poly_bytes_stress_aot: ";

    std::mt19937 rng(std::random_device{}());
    std::uniform_int_distribution<int> pw_dist(
      Services::MIN_PW_SIZE, Services::MAX_PW_SIZE);
    std::uniform_int_distribution<int> salt_dist(
      Services::MIN_SALT_SIZE, Services::MAX_SALT_SIZE);

    auto start = high_resolution_clock::now();

    int pw_size = pw_dist(rng);
    auto pw = rng_bytes(pw_size);

    int salt_size = salt_dist(rng);
    auto salt = rng_bytes(salt_size);

    std::vector<uint8_t> key(Services::CHACHA_POLY_MAX_KEY_SIZE);
    auto err = pbkdf2_aot(
      pw.data(), (int)pw.size(),
      salt.data(), (int)salt.size(),
      Services::MIN_ITERATION,
      key.data(), (int)key.size());
    assert_error(err);

    auto nonce = rng_bytes(Services::CHACHA_POLY_NONCE_SIZE);
    auto plain = rng_bytes(Services::CHACHA_POLY_MAX_PLAIN_SIZE);

    auto associat = std::vector<uint8_t>(
      reinterpret_cast<const uint8_t*>("© Michele Natale 2021"),
      reinterpret_cast<const uint8_t*>("© Michele Natale 2021") + strlen("© Michele Natale 2021"));

    // Encrypt
    void* cipher_ptr = nullptr;
    int cipher_len = 0;

    err = chacha20_poly1305_encrypt_aot(
      plain.data(), (int)plain.size(),
      key.data(), (int)key.size(),
      associat.data(), (int)associat.size(),
      &cipher_ptr, &cipher_len);
    assert_error(err);

    if (!cipher_ptr)
      throw std::runtime_error("Null pointer returned");

    std::vector<uint8_t> cipher(
      (uint8_t*)cipher_ptr, (uint8_t*)cipher_ptr + cipher_len);
    free_buffer_aot(cipher_ptr);

    // Decrypt
    void* decipher_ptr = nullptr;
    int decipher_len = 0;

    err = chacha20_poly1305_decrypt_aot(
      cipher.data(), (int)cipher.size(),
      key.data(), (int)key.size(),
      associat.data(), (int)associat.size(),
      &decipher_ptr, &decipher_len);
    assert_error(err);

    if (!decipher_ptr)
      throw std::runtime_error("Null pointer returned");

    std::vector<uint8_t> decipher(
      (uint8_t*)decipher_ptr, (uint8_t*)decipher_ptr + decipher_len);
    free_buffer_aot(decipher_ptr);

    auto end = high_resolution_clock::now();
    auto ms = duration_cast<milliseconds>(end - start).count();

    if (decipher.size() != plain.size() ||
      memcmp(decipher.data(), plain.data(), plain.size()) != 0)
    {
      throw std::runtime_error("Chacha20Poly1305 mismatch");
    }

    std::cout << "t = " << ms << "ms\n";
  }
}