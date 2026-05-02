
#include "pch.h"

#include <chrono>
#include <string>
#include <vector>
#include <iostream>
#include <fstream>
#include <iterator>


#include "bridge.h"
#include "compression_type.h"
#include "compression_level.h"
#include "crypto_utils_test.h"
#include "compress_gzip_brotli_test.h"


namespace michele::natale::Tests
{
  compression_type_t random_compress_type_gb(); 
  compression_level_t random_compress_level_gb();


  static std::string original_string()
  {
    std::ifstream f("data.txt", std::ios::binary);
    return std::string((std::istreambuf_iterator<char>(f)),
      std::istreambuf_iterator<char>());
  }

  static void test_compress_gzip(int rounds)
  {
    std::cout << "test_compress_gzip_aot: ";

    std::string original = original_string();
    std::vector<uint8_t> message(original.begin(), original.end());

    auto start = std::chrono::high_resolution_clock::now();

    for (int i = 0; i < rounds; i++)
    {
      auto compresslevel = (uint8_t)random_compress_level_gb();

      int32_t out_length = 0;
      uint8_t* out_ptr = nullptr;

      cerror_t err = compress_message_gzip_aot(
        message.data(), (int32_t)message.size(),
        compresslevel,
        &out_ptr, &out_length);
      assert_error(err);

      std::vector<uint8_t> compressed(out_ptr, out_ptr + out_length);
      free_buffer_aot(out_ptr);

      err = decompress_message_gzip_aot(
        compressed.data(), (int32_t)compressed.size(),
        &out_ptr, &out_length);
      assert_error(err);

      std::vector<uint8_t> decompressed(out_ptr, out_ptr + out_length);
      free_buffer_aot(out_ptr);

      if (decompressed != message)
        throw std::runtime_error("roundtrip");

      if (i % (rounds / 10) == 0)
        std::cout << ".";
    }

    auto end = std::chrono::high_resolution_clock::now();
    double ms = std::chrono::duration<double, std::milli>(end - start).count();

    std::cout << " rounds = " << rounds
      << "; t = " << ms << "ms; td = " << (ms / rounds) << "ms\n";
  }

  static void test_compress_file_gzip(int rounds)
  {
    std::cout << "test_compress_file_gzip_aot: ";

    const char* src = "data.txt";
    const char* destr = "datar.txt";
    const char* dest = "datacompress";

    std::remove(dest);
    std::remove(destr);

    auto start = std::chrono::high_resolution_clock::now();

    for (int i = 0; i < rounds; i++)
    {
      auto compresslevel = (uint8_t)random_compress_level_gb();

      std::string src_utf8(src);
      std::string dest_utf8(dest);

      cerror_t err = compress_file_gzip_aot(
        (const uint8_t*)src_utf8.data(), (int32_t)src_utf8.size(),
        (const uint8_t*)dest_utf8.data(), (int32_t)dest_utf8.size(),
        compresslevel);
      assert_error(err);

      std::string destr_utf8(destr);

      err = decompress_file_gzip_aot(
        (const uint8_t*)dest_utf8.data(), (int32_t)dest_utf8.size(),
        (const uint8_t*)destr_utf8.data(), (int32_t)destr_utf8.size());
      assert_error(err);

      bool istrue = equal_files_aot(
        (const uint8_t*)src_utf8.data(), (int32_t)src_utf8.size(),
        (const uint8_t*)destr_utf8.data(), (int32_t)destr_utf8.size(),
        &err);
      assert_error(err);

      if (!istrue)
        throw std::runtime_error("roundtrip");

      if (i % (rounds / 10) == 0)
        std::cout << ".";
    }

    auto end = std::chrono::high_resolution_clock::now();
    double ms = std::chrono::duration<double, std::milli>(end - start).count();

    std::cout << " rounds = " << rounds
      << "; t = " << ms << "ms; td = " << (ms / rounds) << "ms\n";
  }

  static void test_compress_file_bs_gzip(int rounds)
  {
    std::cout << "test_compress_file_bs_gzip_aot: ";

    const char* src = "data.txt";
    const char* destr = "datar.txt";
    const char* dest = "datacompress";

    std::remove(dest);
    std::remove(destr);

    auto start = std::chrono::high_resolution_clock::now();

    for (int i = 0; i < rounds; i++)
    {
      auto compresslevel = (uint8_t)random_compress_level_gb();

      std::string src_utf8(src);
      std::string dest_utf8(dest);

      cerror_t err = compress_file_buffer_size_gzip_aot(
        (const uint8_t*)src_utf8.data(), (int32_t)src_utf8.size(),
        (const uint8_t*)dest_utf8.data(), (int32_t)dest_utf8.size(),
        BUFFER_SIZE_DEFAULT, compresslevel);
      assert_error(err);

      std::string destr_utf8(destr);

      err = decompress_file_buffer_size_gzip_aot(
        (const uint8_t*)dest_utf8.data(), (int32_t)dest_utf8.size(),
        (const uint8_t*)destr_utf8.data(), (int32_t)destr_utf8.size(),
        BUFFER_SIZE_DEFAULT);
      assert_error(err);

      bool istrue = equal_files_aot(
        (const uint8_t*)src_utf8.data(), (int32_t)src_utf8.size(),
        (const uint8_t*)src_utf8.data(), (int32_t)src_utf8.size(),
        &err);
      assert_error(err);

      if (!istrue)
        throw std::runtime_error("roundtrip");

      if (i % (rounds / 10) == 0)
        std::cout << ".";
    }

    auto end = std::chrono::high_resolution_clock::now();
    double ms = std::chrono::duration<double, std::milli>(end - start).count();

    std::cout << " rounds = " << rounds
      << "; t = " << ms << "ms; td = " << (ms / rounds) << "ms\n";
  }

  static void test_compress_brotli(int rounds)
  {
    std::cout << "test_compress_brotli_aot: ";

    std::string original = original_string();
    std::vector<uint8_t> message(original.begin(), original.end());

    auto start = std::chrono::high_resolution_clock::now();

    for (int i = 0; i < rounds; i++)
    {
      auto compresslevel = (uint8_t)random_compress_level_gb();

      uint8_t* out_ptr = nullptr;
      int32_t out_length = 0;

      cerror_t err = compress_message_brotli_aot(
        message.data(), (int32_t)message.size(),
        compresslevel,
        &out_ptr, &out_length);
      assert_error(err);

      std::vector<uint8_t> compressed(out_ptr, out_ptr + out_length);
      free_buffer_aot(out_ptr);

      err = decompress_message_brotli_aot(
        compressed.data(), (int32_t)compressed.size(),
        &out_ptr, &out_length);
      assert_error(err);

      std::vector<uint8_t> decompressed(out_ptr, out_ptr + out_length);
      free_buffer_aot(out_ptr);

      if (decompressed != message)
        throw std::runtime_error("roundtrip");

      if (i % (rounds / 10) == 0)
        std::cout << ".";
    }

    auto end = std::chrono::high_resolution_clock::now();
    double ms = std::chrono::duration<double, std::milli>(end - start).count();

    std::cout << " rounds = " << rounds
      << "; t = " << ms << "ms; td = " << (ms / rounds) << "ms\n";
  }

  static void test_compress_file_brotli(int rounds)
  {
    std::cout << "test_compress_file_brotli_aot: ";

    const char* src = "data.txt";
    const char* destr = "datar.txt";
    const char* dest = "datacompress";

    std::remove(dest);
    std::remove(destr);

    auto start = std::chrono::high_resolution_clock::now();

    for (int i = 0; i < rounds; i++)
    {
      auto compresslevel = (uint8_t)random_compress_level_gb();

      std::string src_utf8(src);
      std::string dest_utf8(dest);

      cerror_t err = compress_file_buffer_size_brotli_aot(
        (const uint8_t*)src_utf8.data(), (int32_t)src_utf8.size(),
        (const uint8_t*)dest_utf8.data(), (int32_t)dest_utf8.size(),
        BUFFER_SIZE_DEFAULT, compresslevel);
      assert_error(err);

      std::string destr_utf8(destr);

      err = decompress_file_buffer_size_brotli_aot(
        (const uint8_t*)dest_utf8.data(), (int32_t)dest_utf8.size(),
        (const uint8_t*)destr_utf8.data(), (int32_t)destr_utf8.size(),
        BUFFER_SIZE_DEFAULT);
      assert_error(err);

      bool istrue = equal_files_aot(
        (const uint8_t*)src_utf8.data(), (int32_t)src_utf8.size(),
        (const uint8_t*)src_utf8.data(), (int32_t)src_utf8.size(),
        &err);
      assert_error(err);

      if (!istrue)
        throw std::runtime_error("roundtrip");

      if (i % (rounds / 10) == 0)
        std::cout << ".";
    }

    auto end = std::chrono::high_resolution_clock::now();
    double ms = std::chrono::duration<double, std::milli>(end - start).count();

    std::cout << " rounds = " << rounds
      << "; t = " << ms << "ms; td = " << (ms / rounds) << "ms\n";
  }

  static void test_compress_file_bs_brotli(int rounds)
  {
    std::cout << "test_compress_file_bs_brotli_aot: ";

    const char* src = "data.txt";
    const char* destr = "datar.txt";
    const char* dest = "datacompress";

    std::remove(dest);
    std::remove(destr);

    auto start = std::chrono::high_resolution_clock::now();

    for (int i = 0; i < rounds; i++)
    {
      auto compresslevel = (uint8_t)random_compress_level_gb();

      std::string src_utf8(src);
      std::string dest_utf8(dest);

      cerror_t err = compress_file_brotli_aot(
        (const uint8_t*)src_utf8.data(), (int32_t)src_utf8.size(),
        (const uint8_t*)dest_utf8.data(), (int32_t)dest_utf8.size(),
        compresslevel);
      assert_error(err);

      std::string destr_utf8(destr);

      err = decompress_file_brotli_aot(
        (const uint8_t*)dest_utf8.data(), (int32_t)dest_utf8.size(),
        (const uint8_t*)destr_utf8.data(), (int32_t)destr_utf8.size());
      assert_error(err);

      bool istrue = equal_files_aot(
        (const uint8_t*)src_utf8.data(), (int32_t)src_utf8.size(),
        (const uint8_t*)src_utf8.data(), (int32_t)src_utf8.size(),
        &err);
      assert_error(err);

      if (!istrue)
        throw std::runtime_error("roundtrip");

      if (i % (rounds / 10) == 0)
        std::cout << ".";
    }

    auto end = std::chrono::high_resolution_clock::now();
    double ms = std::chrono::duration<double, std::milli>(end - start).count();

    std::cout << " rounds = " << rounds
      << "; t = " << ms << "ms; td = " << (ms / rounds) << "ms\n\n";
  }

  compression_type_t random_compress_type_gb()
  {
    auto values = to_compression_type();

    // call NativeAOT RNG 
    cerror_t err;
    auto rng = next_crypto_int32_max_aot(
      (int)values.size(), &err);
    assert_error(err);

    return values[rng];
  } 
  
  compression_level_t random_compress_level_gb()
  {
    auto values = to_compression_level();

    // call NativeAOT RNG 
    cerror_t err;
    auto rng = next_crypto_int32_max_aot(
      (int)values.size(), &err);
    assert_error(err);

    return values[rng];
  }


  void start_compress_gzip_brotli(int rounds)
  {
    test_compress_gzip(rounds);
    test_compress_file_gzip(rounds);
    test_compress_file_bs_gzip(rounds);

    test_compress_brotli(rounds);
    test_compress_file_brotli(rounds);
    test_compress_file_bs_brotli(rounds);
  }
}