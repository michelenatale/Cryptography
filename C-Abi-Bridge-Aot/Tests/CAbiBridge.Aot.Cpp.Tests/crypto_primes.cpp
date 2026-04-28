#include "pch.h"

#include <span>
#include <cstdint>
#include <stdexcept>
#include <iostream>
#include <cstring>          // memcpy
#include <utility>          // std::pair
#include <random>           // mt19937, random_device
#include <chrono>           // timing
#include <openssl/bn.h>     // BIGNUM, BN_new, BN_free, BN_bin2bn, BN_bn2bin, BN_is_prime_ex, BN_set_word, BN_cmp, BN_num_bits
#include <openssl/rand.h>   // RAND_bytes


#include "bridge.h"
#include "bn_ctx_x.h"
#include "crypto_primes.h"
#include "bn_prime_wrapper.h"
#include "bit_prime_length.h"
#include "crypto_utils_test.h"
#include "primality_confidence.h"


namespace michele::natale::Tests
{
  uint64_t next_uint64();
  BNPtr random_bigint(int byte_count);
  bit_prime_length random_bit_prime_length();
  std::pair<uint64_t, uint64_t> to_min_max_uint64();
  primality_confidence random_primality_confidence();
  std::vector<uint8_t> bn_to_bytes(const BIGNUM* bn);
  BNPtr bn_from_bytes(const uint8_t* data, int length);
  bit_prime_length random_bit_prime_length(int firt_x);
  std::pair<BNPtr, BNPtr> to_min_max_bigint(int byte_count = 32);
  bool is_mr_prime_uint64(uint64_t candidate, primality_confidence confidence);
  bool is_mr_prime_bn(const BIGNUM* candidate, primality_confidence confidence);
    

  void test_next_crypto_primes_min_max_uint64(int rounds)
  {
    std::cout << "test_next_crypto_primes_min_max_uint64_aot: ";

    auto start = std::chrono::steady_clock::now();

    for (int i = 0; i < rounds; ++i)
    {
      // 1. generate min/max (neatly encapsulated)
      auto [min, max] = to_min_max_uint64();

      // 2. random mr rounds
      primality_confidence mrr = random_primality_confidence();

      // 3. call NativeAOT
      cerror_t err;
      uint64_t prime = next_crypto_primes_min_max_uint64_aot(
        static_cast<int32_t>(mrr), min, max, &err);
      assert_error(err);

      // 4. check range 
      if (prime < min || prime > max)
        throw std::runtime_error("Prime out of range");

      // 5. check with Miller–Rabin 
      if (!is_mr_prime_uint64(prime, mrr))
        throw std::runtime_error("Prime failed MR test");


      if (i % std::max(1, rounds / 10) == 0)
        std::cout << ".";
    }

    //Timming
    auto end = std::chrono::steady_clock::now();
    auto ms = std::chrono::duration_cast<std::chrono::milliseconds>(end - start).count();

    std::cout
      << " rounds = " << rounds
      << "; t = " << ms << "ms"
      << "; td = " << (ms / static_cast<double>(rounds)) << "ms\n";
  }


  void test_next_crypto_primes_min_max_biginteger(int rounds)
  {
    std::cout << "test_next_crypto_primes_min_max_biginteger_aot: ";

    auto start = std::chrono::steady_clock::now();

    for (int i = 0; i < rounds; ++i)
    {
      // 1. create random  (5–31)
      cerror_t err;
      int byte_count = next_crypto_int32_min_max_aot(5, 31, &err);
      assert_error(err);

      // 2. create min/max BigInteger
      auto [min_bn, max_bn] = to_min_max_bigint(byte_count);

      // 3. rounds of MR
      primality_confidence mrr = random_primality_confidence();

      // 4. BigInteger → Bytes
      std::vector<uint8_t> min_bytes = bn_to_bytes(min_bn.get());
      std::vector<uint8_t> max_bytes = bn_to_bytes(max_bn.get());

      // 5. call NativeAOT 
      int32_t out_len = 0; uint8_t* out_ptr = nullptr;

      err = next_crypto_primes_min_max_aot(
        static_cast<int32_t>(mrr),
        min_bytes.data(), static_cast<int32_t>(min_bytes.size()),
        max_bytes.data(), static_cast<int32_t>(max_bytes.size()),
        &out_ptr, &out_len);
      assert_error(err);

      if (!out_ptr || out_len <= 0)
        throw std::runtime_error("Invalid output from next_crypto_primes_min_max_aot");

      // 6. Bytes → BIGNUM
      BNPtr prime_bn = bn_from_bytes(out_ptr, out_len);

      // reset buffer
      free_buffer_aot(out_ptr);

      // 7. check Range
      if (BN_cmp(prime_bn.get(), min_bn.get()) < 0 ||
        BN_cmp(prime_bn.get(), max_bn.get()) > 0)
        throw std::runtime_error("Prime out of range (BigInteger)");

      // 8. check prime with Miller–Rabin 
      if (!is_mr_prime_bn(prime_bn.get(), mrr))
        throw std::runtime_error("Prime failed MR test (BigInteger)");


      if (i % std::max(1, rounds / 10) == 0)
        std::cout << ".";
    }

    // 10. Timming
    auto end = std::chrono::steady_clock::now();
    auto ms = std::chrono::duration_cast<std::chrono::milliseconds>(end - start).count();

    std::cout
      << " rounds = " << rounds
      << "; t = " << ms << "ms"
      << "; td = " << (ms / static_cast<double>(rounds)) << "ms\n";
  }

  void test_next_crypto_primes_bits_biginteger(int rounds)
  {
    std::cout << "test_next_crypto_primes_bits_biginteger_aot: ";

    auto start = std::chrono::steady_clock::now();

    for (int i = 0; i < rounds; ++i)
    {
      // --- 1. Select bits (only the first 10, as in C#) ---
      bit_prime_length bits = random_bit_prime_length(10);

      // --- 2. Select MR rounds ---
      primality_confidence mrr = random_primality_confidence();

      // --- 3. call NativeAOT-Function  ---
      int32_t out_len = 0; uint8_t* out_ptr = nullptr;
      cerror_t err = next_crypto_primes_aot(
        static_cast<int32_t>(mrr), static_cast<int32_t>(bits),
        &out_ptr, &out_len);
      assert_error(err);

      if (!out_ptr || out_len <= 0)
        throw std::runtime_error("Invalid output from next_crypto_primes_bits_aot");

      // --- 4. Bytes → BIGNUM ---
      BNPtr prime_bn = bn_from_bytes(out_ptr, out_len);

      // Buffer freigeben
      free_buffer_aot(out_ptr);

      // --- 5. check bitlength ---
      int prime_bits = BN_num_bits(prime_bn.get());
      if (prime_bits != static_cast<int>(bits))
        throw std::runtime_error("Prime has wrong bit length");

      // --- 6. check prime with Miller–Rabin ---
      if (!is_mr_prime_bn(prime_bn.get(), mrr))
        throw std::runtime_error("Prime failed MR test");

      if (i % std::max(1, rounds / 10) == 0)
        std::cout << ".";
    }

    // --- 8. Timekeeping ---
    auto end = std::chrono::steady_clock::now();
    auto ms = std::chrono::duration_cast<std::chrono::milliseconds>(end - start).count();

    std::cout
      << " rounds = " << rounds
      << "; t = " << ms << "ms"
      << "; td = " << (ms / static_cast<double>(rounds)) << "ms\n";
  }

  void test_rng_crypto_primes_min_max_uint64(int rounds)
  {
    std::cout << "test_rng_crypto_primes_min_max_uint64_aot: ";

    auto start = std::chrono::steady_clock::now();

    for (int i = 0; i < rounds; ++i)
    {
      // 1. create number of primes(10–100)
      cerror_t err;
      int count = next_crypto_int32_min_max_aot(10, 101, &err);
      assert_error(err);

      // 2. create min/max 
      auto [min_val, max_val] = to_min_max_uint64();

      // 3. select MR rounds
      primality_confidence mrr = random_primality_confidence();

      // 4. NativeAOT-Funktion aufrufen
      uint64_t* out_ptr = nullptr;
      err = rng_crypto_primes_min_max_uint64_aot(
        static_cast<int32_t>(mrr), count,
        min_val, max_val, &out_ptr);
      assert_error(err);

      if (!out_ptr)
        throw std::runtime_error("rng_crypto_primes_minmax_uint64_aot returned null");

      // 5. to span
      std::span<uint64_t> primes(out_ptr, count);

      // 6. validation
      for (uint64_t prime : primes)
      {
        if (prime < min_val || prime > max_val)
          throw std::runtime_error("Prime out of range (UInt64)");

        if (!is_mr_prime_uint64(prime, mrr))
          throw std::runtime_error("Prime failed MR test (UInt64)");
      }

      // 7. reset buffer
      free_buffer_aot(out_ptr);


      if (i % std::max(1, rounds / 10) == 0)
        std::cout << ".";
    }

    // 9. Timming
    auto end = std::chrono::steady_clock::now();
    auto ms = std::chrono::duration_cast<std::chrono::milliseconds>(end - start).count();

    std::cout
      << " rounds = " << rounds
      << "; t = " << ms << "ms"
      << "; td = " << (ms / static_cast<double>(rounds)) << "ms\n";
  }

  void test_rng_crypto_primes_min_max_biginteger(int rounds)
  {
    std::cout << "test_rng_crypto_primes_min_max_biginteger_aot: ";

    auto start = std::chrono::steady_clock::now();
    long total_counts = 0, total_byte_lengths = 0;

    for (int i = 0; i < rounds; ++i)
    {
      // 1. Number of primes (10–100)
      cerror_t err;
      int counts = next_crypto_int32_min_max_aot(10, 101, &err);
      assert_error(err);

      total_counts += counts;

      // 2. byte-length (5–15)
      int byte_count = next_crypto_int32_min_max_aot(5, 15, &err);
      assert_error(err);

      total_byte_lengths += byte_count;

      // 3. create min/max BigInteger 
      auto [min_bn, max_bn] = to_min_max_bigint(byte_count);

      // 4. convert min/max to bytes (little-endian)
      std::vector<uint8_t> min_bytes = bn_to_bytes(min_bn.get());
      std::vector<uint8_t> max_bytes = bn_to_bytes(max_bn.get());

      // 5. MR-Runden auswählen
      primality_confidence mrr = random_primality_confidence();

      // 6. call NativeAOT-function
      uint8_t* out_ptr = nullptr;
      int32_t* out_lengths_ptr = nullptr;
      err = rng_crypto_primes_min_max_aot(
        static_cast<int32_t>(mrr), counts,
        min_bytes.data(), static_cast<int32_t>(min_bytes.size()),
        max_bytes.data(), static_cast<int32_t>(max_bytes.size()),
        &out_ptr, &out_lengths_ptr);
      assert_error(err);

      if (!out_ptr || !out_lengths_ptr)
        throw std::runtime_error("Invalid output from rng_crypto_primes_minmax_aot");

      uint8_t* buffer = static_cast<uint8_t*>(out_ptr);
      int* lengths = static_cast<int*>(out_lengths_ptr);

      // 7. extract byte-arrays
      std::vector<std::vector<uint8_t>> primes_bytes(counts);

      int offset = 0;
      for (int j = 0; j < counts; ++j)
      {
        int len = lengths[j];
        if (len < 0)
          throw std::runtime_error("Negative length in output");

        primes_bytes[j].resize(len);

        if (len > 0)
          std::memcpy(primes_bytes[j].data(), buffer + offset, len);

        offset += len;
      }

      // reset buffers
      free_buffer_aot(out_ptr);
      free_buffer_aot(out_lengths_ptr);

      // 8. to BIGNUM convert
      std::vector<BNPtr> primes(counts);
      for (int j = 0; j < counts; ++j)
        primes[j] = bn_from_bytes(
          primes_bytes[j].data(), (int)primes_bytes[j].size());

      // 9. validation
      for (int j = 0; j < counts; ++j)
      {
        BIGNUM* p = primes[j].get();

        if (BN_cmp(p, min_bn.get()) < 0 || BN_cmp(p, max_bn.get()) > 0)
          throw std::runtime_error("Prime out of range");

        if (!is_mr_prime_bn(p, mrr))
          throw std::runtime_error("Prime failed MR test");
      }

      if (i % std::max(1, rounds / 10) == 0)
        std::cout << ".";
    }

    // 11. Timming
    auto end = std::chrono::steady_clock::now();
    auto ms = std::chrono::duration_cast<std::chrono::milliseconds>(end - start).count();

    std::cout
      << " rounds = " << rounds
      << "; counts = " << (total_counts / rounds)
      << "; bi-byte-length = " << (total_byte_lengths / rounds)
      << "; t = " << ms << "ms"
      << "; td = " << (ms / static_cast<double>(rounds)) << "ms\n";
  }

  void test_rng_crypto_primes_bits_biginteger(int rounds)
  {
    std::cout << "test_rng_crypto_primes_bits_biginteger_aot: ";

    long total_bits = 0, total_counts = 0;
    auto start = std::chrono::steady_clock::now();

    for (int i = 0; i < rounds; ++i)
    {
      // 1. number of primes (10–50)
      cerror_t err;
      int counts = next_crypto_int32_min_max_aot(10, 50, &err);
      assert_error(err);

      total_counts += counts;

      // 2. select Bits (only the first 7 as C#)
      bit_prime_length bits = random_bit_prime_length(7);
      total_bits += static_cast<int>(bits);

      // 3. select MR-rounds
      primality_confidence mrr = random_primality_confidence();

      // 4. call NativeAOT-Function
      uint8_t* out_ptr = nullptr;
      int32_t* out_lengths_ptr = nullptr;

      err = rng_crypto_primes_aot(
        static_cast<int32_t>(mrr), 
        static_cast<int32_t>(bits),
        counts, &out_ptr, &out_lengths_ptr);
      assert_error(err);

      if (!out_ptr || !out_lengths_ptr)
        throw std::runtime_error("Invalid output from rng_crypto_primes_bits_aot");

      uint8_t* buffer = out_ptr; int* lengths = out_lengths_ptr;

      // 5. extract Byte-Arrays 
      std::vector<std::vector<uint8_t>> primes_bytes(counts);

      int offset = 0;
      for (int j = 0; j < counts; ++j)
      {
        int len = lengths[j];
        if (len < 0)
          throw std::runtime_error("Negative length in output");

        primes_bytes[j].resize(len);

        if (len > 0)
          std::memcpy(primes_bytes[j].data(), buffer + offset, len);

        offset += len;
      }

      // reset Buffer
      free_buffer_aot(out_ptr);
      free_buffer_aot(out_lengths_ptr);

      // 6. convert to BIGNUM
      std::vector<BNPtr> primes(counts);
      for (int j = 0; j < counts; ++j)
        primes[j] = bn_from_bytes(
          primes_bytes[j].data(), 
          (int)primes_bytes[j].size());

      // 7. validation
      for (int j = 0; j < counts; ++j)
      {
        BIGNUM* p = primes[j].get();

        // check Bitlength
        int prime_bits = BN_num_bits(p);
        if (prime_bits != static_cast<int>(bits))
          throw std::runtime_error("Prime has wrong bit length");

        // MR-test
        if (!is_mr_prime_bn(p, mrr))
          throw std::runtime_error("Prime failed MR test");
      }
       
      if (i % std::max(1, rounds / 10) == 0)
        std::cout << ".";
    }

    // 9. Timing
    auto end = std::chrono::steady_clock::now();
    auto ms = std::chrono::duration_cast<std::chrono::milliseconds>(end - start).count();

    std::cout
      << " rounds = " << rounds
      << "; counts = " << (total_counts / rounds)
      << "; bi-bits = " << (total_bits / rounds)
      << "; t = " << ms << "ms"
      << "; td = " << (ms / static_cast<double>(rounds)) << "ms\n\n";
  }


  // ************ ************ ************ ************ 
  // ************ ************ ************ ************ 


  primality_confidence random_primality_confidence()
  {
    auto values = to_primality_confidences();

    // 1. call NativeAOT RNG 
    cerror_t err;
    auto rng = next_crypto_int32_max_aot((int)values.size(), &err);
    assert_error(err);

    return values[rng];
  }

  bit_prime_length random_bit_prime_length()
  {
    auto values = to_bit_prime_lengths();

    // 1. call NativeAOT RNG 
    cerror_t err;
    auto rng = next_crypto_int32_max_aot((int)values.size(), &err);
    assert_error(err);

    return values[rng];
  }

  bit_prime_length random_bit_prime_length(int firt_x)
  {
    auto values = to_bit_prime_lengths();

    if (firt_x > values.size())
      throw std::invalid_argument("firt_x > 26");

    // 1. call NativeAOT RNG 
    cerror_t err;
    auto rng = next_crypto_int32_max_aot(firt_x, &err);
    assert_error(err);

    return values[rng];
  }

  std::pair<uint64_t, uint64_t> to_min_max_uint64()
  {
    while (true)
    {
      uint64_t min = next_uint64();
      for (int i = 0; i < 3; ++i)
      {
        uint64_t max = next_uint64();
        if (max > min)
          return { min, max };
      }
    }
  }

  std::pair<BNPtr, BNPtr> to_min_max_bigint(int byte_count)
  {
    if (byte_count <= 0)
      throw std::invalid_argument("byte_count must be > 0");

    while (true)
    {
      // --- 1. create min ---
      BNPtr min_bn = random_bigint(byte_count);

      // --- 2. create max ---
      for (int i = 0; i < 3; ++i)
      {
        BNPtr max_bn = random_bigint(byte_count);

        if (BN_cmp(max_bn.get(), min_bn.get()) > 0)
          return { std::move(min_bn), std::move(max_bn) };
      }
    }
  }

  BNPtr random_bigint(int byte_count)
  {
    if (byte_count <= 0)
      throw std::invalid_argument("byte_count must be > 0");

    // 1. RNG from NativeAOT-Library
    void* out_ptr = nullptr;
    cerror_t err = rng_crypto_bytes_aot(byte_count, &out_ptr);
    assert_error(err);

    if (!out_ptr)
      throw std::runtime_error("rng_crypto_bytes_aot returned null");

    // 2. to bytes
    uint8_t* bytes = static_cast<uint8_t*>(out_ptr);

    // 3. Bytes → BIGNUM
    BNPtr bn = bn_from_bytes(bytes, byte_count);

    // 4. reset buffer
    free_buffer_aot(out_ptr);

    return bn;
  }

  uint64_t next_uint64()
  {
    const int size = 8;
    void* out_ptr = nullptr;

    // 1. call NativeAOT RNG 
    auto err = rng_crypto_bytes_aot(size, &out_ptr);
    assert_error(err);

    if (!out_ptr)
      throw std::runtime_error("rng_crypto_bytes_aot returned null pointer");

    // 2. bytes to uint64_t, equals then bitconverter in c#
    uint64_t value = 0;
    std::memcpy(&value, out_ptr, sizeof(uint64_t));

    // 3. reset the buffer out_ptr
    free_buffer_aot(out_ptr);

    return value;
  }

  bool is_mr_prime_uint64(
    uint64_t candidate,
    primality_confidence confidence)
  {
    if (candidate < 2) return false;
    if (candidate == 2) return true;
    if ((candidate & 1ULL) == 0ULL) return false;

    BNPtr bn;
    if (!BN_set_word(bn.get(), candidate))
      throw std::runtime_error("BN_set_word failed");

    BN_CTX_Ptr ctx;
    int rounds = static_cast<int>(confidence);

    int rc = bn_is_prime(bn.get(), rounds, ctx.get());
    if (rc < 0)
      throw std::runtime_error("BN_is_prime_ex failed");

    return rc == 1;
  }

  bool is_mr_prime_bn(
    const BIGNUM* candidate,
    primality_confidence confidence)
  {
    BN_CTX_Ptr ctx;
    int rounds = static_cast<int>(confidence);

    int rc = bn_is_prime(candidate, rounds, ctx.get());
    if (rc < 0)
      throw std::runtime_error("BN_is_prime_ex failed");

    return rc == 1;
  }

  BNPtr bn_from_bytes(const uint8_t* data, int length)
  {
    if (!data || length <= 0)
      throw std::invalid_argument("bn_from_bytes: invalid input");

    std::vector<uint8_t> be(data, data + length);

    BIGNUM* raw = BN_bin2bn(be.data(), static_cast<int>(be.size()), nullptr);
    if (!raw)
      throw std::runtime_error("BN_bin2bn failed");

    return BNPtr(raw);
  }

  std::vector<uint8_t> bn_to_bytes(const BIGNUM* bn)
  {
    if (!bn)
      throw std::invalid_argument("bn_to_bytes: bn is null");

    int len = BN_num_bytes(bn);
    std::vector<uint8_t> be(len);

    // OpenSSL → Big-Endian
    if (BN_bn2binpad(bn, be.data(), len) != len)
      throw std::runtime_error("BN_bn2binpad failed");

    // WICHTIG: NICHT reverse!
    // AOT / C# erwartet Big-Endian (isBigEndian: true)

    return be;
  }


}
