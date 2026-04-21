#include "pch.h"

#include <vector>
#include <chrono>
#include <random>
#include <iostream>
#include <stdexcept>
#include <algorithm>

#include "bridge.h"
#include "crypto_utils_test.h"
#include "crypto_random_test.h"


namespace michele::natale::Tests
{
  static void test_rng_crypto_bytes_aot(int rounds)
  {
    std::cout << "test_rng_crypto_bytes_aot: ";

    std::mt19937 rng(std::random_device{}());
    std::uniform_int_distribution<int> dist(128, 512);
    auto start = std::chrono::high_resolution_clock::now();

    for (int i = 0; i < rounds; i++)
    {
      int size = dist(rng);

      void* out_ptr = nullptr;
      auto err = rng_crypto_bytes_aot(size, &out_ptr);
      assert_error(err);

      if (!out_ptr)
        throw std::runtime_error("Null pointer returned");

      std::vector<uint8_t> bytes((uint8_t*)out_ptr, (uint8_t*)out_ptr + size);
      free_buffer_aot(out_ptr);

      if (bytes.size() != size)
        throw std::runtime_error("Size mismatch");

      if (i % (rounds / 10) == 0)
        std::cout << ".";
    }

    auto end = std::chrono::high_resolution_clock::now();
    double ms = std::chrono::duration<double, std::milli>(end - start).count();

    std::cout << " rounds = " << rounds
      << "; t = " << ms << "ms; td = " << (ms / rounds) << "ms\n";


    std::cout << "test_fill_crypto_bytes_aot: ";
    start = std::chrono::high_resolution_clock::now();

    for (int i = 0; i < rounds; i++)
    {
      size_t size = dist(rng);
      std::vector<uint8_t> bytes(size);

      auto err = fill_crypto_bytes_aot(bytes.data(), (int)bytes.size());
      assert_error(err);

      if (bytes.size() != size)
        throw std::runtime_error("Size mismatch");

      if (*std::max_element(bytes.begin(), bytes.end()) == 0)
        throw std::runtime_error("All bytes are zero");

      if (i % (rounds / 10) == 0)
        std::cout << ".";
    }

    end = std::chrono::high_resolution_clock::now();
    ms = std::chrono::duration<double, std::milli>(end - start).count();

    std::cout << " rounds = " << rounds
      << "; t = " << ms << "ms; td = " << (ms / rounds) << "ms\n\n";
  }

  static void test_rng_crypto_bool_aot(int rounds)
  {
    std::cout << "test_rng_crypto_bool_aot: ";

    std::mt19937 rng(std::random_device{}());
    std::uniform_int_distribution<int> dist(128, 512);
    auto start = std::chrono::high_resolution_clock::now();

    for (int i = 0; i < rounds; i++)
    {
      int size = dist(rng);
      void* out_ptr = nullptr;
      auto err = rng_crypto_bool_aot(size, &out_ptr);
      assert_error(err);

      if (!out_ptr)
        throw std::runtime_error("Null pointer returned");

      std::vector<bool> bools((bool*)out_ptr, (bool*)out_ptr + size);
      free_buffer_aot(out_ptr);

      if (bools.size() != size)
        throw std::runtime_error("Size mismatch");

      if (i % (rounds / 10) == 0)
        std::cout << ".";
    }

    auto end = std::chrono::high_resolution_clock::now();
    double ms = std::chrono::duration<double, std::milli>(end - start).count();

    std::cout << " rounds = " << rounds
      << "; t = " << ms << "ms; td = " << (ms / rounds) << "ms\n";


    std::cout << "test_next_crypto_bool_aot: ";
    start = std::chrono::high_resolution_clock::now();

    for (int i = 0; i < rounds; i++)
    {
      cerror_t err;
      bool b = next_crypto_bool_aot(&err);
      assert_error(err);

      if (i % (rounds / 10) == 0)
        std::cout << ".";
    }

    end = std::chrono::high_resolution_clock::now();
    ms = std::chrono::duration<double, std::milli>(end - start).count();

    std::cout << " rounds = " << rounds
      << "; t = " << ms << "ms; td = " << (ms / rounds) << "ms\n\n";
  }

  static void test_next_crypto_int32_aot(int rounds)
  {
    std::cout << "test_next_crypto_int32_aot: ";

    std::mt19937 rng(std::random_device{}());
    std::uniform_int_distribution<int> dist(0, INT32_MAX);
    auto start = std::chrono::high_resolution_clock::now();

    for (int i = 0; i < rounds; i++)
    {
      cerror_t err;
      int result = next_crypto_int32_aot(&err);
      assert_error(err);

      if (result < 0)
        throw std::runtime_error("Negative result");

      if (i % (rounds / 10) == 0)
        std::cout << ".";
    }

    auto end = std::chrono::high_resolution_clock::now();
    double ms = std::chrono::duration<double, std::milli>(end - start).count();

    std::cout << " rounds = " << rounds
      << "; t = " << ms << "ms; td = " << (ms / rounds) << "ms\n";

    std::cout << "test_next_crypto_int32_max_aot: ";
    start = std::chrono::high_resolution_clock::now();

    for (int i = 0; i < rounds; i++)
    {
      cerror_t err;
      int max = dist(rng);

      int result = next_crypto_int32_max_aot(max, &err);
      assert_error(err);

      if (result < 0 || result > max)
        throw std::runtime_error("Out of range");

      if (i % (rounds / 10) == 0)
        std::cout << ".";
    }

    end = std::chrono::high_resolution_clock::now();
    ms = std::chrono::duration<double, std::milli>(end - start).count();

    std::cout << " rounds = " << rounds
      << "; t = " << ms << "ms; td = " << (ms / rounds) << "ms\n";


    std::cout << "test_next_crypto_int32_min_max_aot: ";
    start = std::chrono::high_resolution_clock::now();

    for (int i = 0; i < rounds; i++)
    {
      int min = INT32_MAX / 3;
      int max = min * 2;

      cerror_t err;
      int result = next_crypto_int32_min_max_aot(min, max, &err);
      assert_error(err);

      if (result < min || result > max)
        throw std::runtime_error("Out of range");

      if (i % (rounds / 10) == 0)
        std::cout << ".";
    }

    end = std::chrono::high_resolution_clock::now();
    ms = std::chrono::duration<double, std::milli>(end - start).count();

    std::cout << " rounds = " << rounds
      << "; t = " << ms << "ms; td = " << (ms / rounds) << "ms\n\n";
  }

  static void test_rng_crypto_int32_aot(int rounds)
  {
    std::cout << "test_rng_crypto_int32_aot: ";

    std::mt19937 rng(std::random_device{}());
    std::uniform_int_distribution<int> dist(0, INT32_MAX);
    std::uniform_int_distribution<int> dist_size(128, 512);

    auto start = std::chrono::high_resolution_clock::now();

    for (int i = 0; i < rounds; i++)
    {
      void* out_ptr = nullptr;
      int size = dist_size(rng);
      auto err = rng_crypto_int32_aot(size, &out_ptr);
      assert_error(err);

      if (!out_ptr)
        throw std::runtime_error("Null pointer");

      std::vector<int> result((int*)out_ptr, (int*)out_ptr + size);
      free_buffer_aot(out_ptr);

      if (result.size() != size)
        throw std::runtime_error("Size mismatch");

      if (i % (rounds / 10) == 0)
        std::cout << ".";
    }

    auto end = std::chrono::high_resolution_clock::now();
    double ms = std::chrono::duration<double, std::milli>(end - start).count();

    std::cout << " rounds = " << rounds
      << "; t = " << ms << "ms; td = " << (ms / rounds) << "ms\n";

    std::cout << "test_rng_crypto_int32_max_aot: ";
    start = std::chrono::high_resolution_clock::now();

    for (int i = 0; i < rounds; i++)
    {
      int max = dist(rng);
      int size = dist_size(rng);

      void* out_ptr = nullptr;
      auto err = rng_crypto_int32_max_aot(size, max, &out_ptr);
      assert_error(err);

      std::vector<int> result((int*)out_ptr, (int*)out_ptr + size);
      free_buffer_aot(out_ptr);

      if (result.size() != size)
        throw std::runtime_error("Size mismatch");

      if (*std::min_element(result.begin(), result.end()) < 0 ||
        *std::max_element(result.begin(), result.end()) > max)
        throw std::runtime_error("Out of range");

      if (i % (rounds / 10) == 0)
        std::cout << ".";
    }

    end = std::chrono::high_resolution_clock::now();
    ms = std::chrono::duration<double, std::milli>(end - start).count();

    std::cout << " rounds = " << rounds
      << "; t = " << ms << "ms; td = " << (ms / rounds) << "ms\n";

    std::cout << "test_rng_crypto_int32_min_max_aot: ";
    start = std::chrono::high_resolution_clock::now();

    for (int i = 0; i < rounds; i++)
    {
      int size = dist_size(rng);
      int min = INT32_MAX / 3;
      int max = min * 2;

      void* out_ptr = nullptr;
      auto err = rng_crypto_int32_min_max_aot(size, min, max, &out_ptr);
      assert_error(err);

      std::vector<int> result((int*)out_ptr, (int*)out_ptr + size);
      free_buffer_aot(out_ptr);

      if (result.size() != size)
        throw std::runtime_error("Size mismatch");

      if (*std::min_element(result.begin(), result.end()) < min ||
        *std::max_element(result.begin(), result.end()) > max)
        throw std::runtime_error("Out of range");

      if (i % (rounds / 10) == 0)
        std::cout << ".";
    }

    end = std::chrono::high_resolution_clock::now();
    ms = std::chrono::duration<double, std::milli>(end - start).count();

    std::cout << " rounds = " << rounds
      << "; t = " << ms << "ms; td = " << (ms / rounds) << "ms\n\n";
  }


  static void test_next_crypto_int64_aot(int rounds)
  {

    std::cout << "test_next_crypto_int64_aot: ";

    std::mt19937_64 rng(std::random_device{}());
    std::uniform_int_distribution<int64_t> dist(0, INT64_MAX);
    auto start = std::chrono::high_resolution_clock::now();

    for (int i = 0; i < rounds; i++)
    {
      cerror_t err;
      int64_t result = next_crypto_int64_aot(&err);
      assert_error(err);

      if (result < 0)
        throw std::runtime_error("Negative result");

      if (i % (rounds / 10) == 0)
        std::cout << ".";
    }

    auto end = std::chrono::high_resolution_clock::now();
    double ms = std::chrono::duration<double, std::milli>(end - start).count();

    std::cout << " rounds = " << rounds
      << "; t = " << ms << "ms; td = " << (ms / rounds) << "ms\n";


    std::cout << "test_next_crypto_int64_max_aot: ";
    start = std::chrono::high_resolution_clock::now();

    for (int i = 0; i < rounds; i++)
    {
      cerror_t err;
      int64_t max = dist(rng);
      int64_t result = next_crypto_int64_max_aot(max, &err);
      assert_error(err);

      if (result < 0 || result > max)
        throw std::runtime_error("Out of range");

      if (i % (rounds / 10) == 0)
        std::cout << ".";
    }

    end = std::chrono::high_resolution_clock::now();
    ms = std::chrono::duration<double, std::milli>(end - start).count();

    std::cout << " rounds = " << rounds
      << "; t = " << ms << "ms; td = " << (ms / rounds) << "ms\n";


    std::cout << "test_next_crypto_int64_min_max_aot: ";
    start = std::chrono::high_resolution_clock::now();

    for (int i = 0; i < rounds; i++)
    {
      int64_t min = INT64_MAX / 3;
      int64_t max = min * 2;

      cerror_t err;
      int64_t result = next_crypto_int64_min_max_aot(min, max, &err);
      assert_error(err);

      if (result < min || result > max)
        throw std::runtime_error("Out of range");

      if (i % (rounds / 10) == 0)
        std::cout << ".";
    }

    end = std::chrono::high_resolution_clock::now();
    ms = std::chrono::duration<double, std::milli>(end - start).count();

    std::cout << " rounds = " << rounds
      << "; t = " << ms << "ms; td = " << (ms / rounds) << "ms\n\n";
  }

  static void test_rng_crypto_int64_aot(int rounds)
  {
    std::cout << "test_rng_crypto_int64_aot: ";

    std::mt19937_64 rng(std::random_device{}());
    std::uniform_int_distribution<int64_t> dist(0, INT64_MAX);
    std::uniform_int_distribution<int> dist_size(128, 512);
    auto start = std::chrono::high_resolution_clock::now();

    for (int i = 0; i < rounds; i++)
    {
      int size = dist_size(rng);
      void* out_ptr = nullptr;
      auto err = rng_crypto_int64_aot(size, &out_ptr);
      assert_error(err);

      if (!out_ptr)
        throw std::runtime_error("Null pointer");

      std::vector<int64_t> result((int64_t*)out_ptr, (int64_t*)out_ptr + size);
      free_buffer_aot(out_ptr);

      if (result.size() != size)
        throw std::runtime_error("Size mismatch");

      if (i % (rounds / 10) == 0)
        std::cout << ".";
    }

    auto end = std::chrono::high_resolution_clock::now();
    double ms = std::chrono::duration<double, std::milli>(end - start).count();

    std::cout << " rounds = " << rounds
      << "; t = " << ms << "ms; td = " << (ms / rounds) << "ms\n";

    std::cout << "test_rng_crypto_int64_max_aot: ";
    start = std::chrono::high_resolution_clock::now();

    for (int i = 0; i < rounds; i++)
    {
      int64_t max = dist(rng);
      int size = dist_size(rng);

      void* out_ptr = nullptr;
      auto err = rng_crypto_int64_max_aot(size, max, &out_ptr);
      assert_error(err);

      std::vector<int64_t> result((int64_t*)out_ptr, (int64_t*)out_ptr + size);
      free_buffer_aot(out_ptr);

      if (result.size() != size)
        throw std::runtime_error("Size mismatch");

      if (*std::min_element(result.begin(), result.end()) < 0 ||
        *std::max_element(result.begin(), result.end()) > max)
        throw std::runtime_error("Out of range");

      if (i % (rounds / 10) == 0)
        std::cout << ".";
    }

    end = std::chrono::high_resolution_clock::now();
    ms = std::chrono::duration<double, std::milli>(end - start).count();

    std::cout << " rounds = " << rounds
      << "; t = " << ms << "ms; td = " << (ms / rounds) << "ms\n";

    std::cout << "test_rng_crypto_int64_min_max_aot: ";
    start = std::chrono::high_resolution_clock::now();

    for (int i = 0; i < rounds; i++)
    {
      int size = dist_size(rng);
      int64_t min = INT64_MAX / 3;
      int64_t max = min * 2;

      void* out_ptr = nullptr;
      auto err = rng_crypto_int64_min_max_aot(size, min, max, &out_ptr);
      assert_error(err);

      std::vector<int64_t> result((int64_t*)out_ptr, (int64_t*)out_ptr + size);
      free_buffer_aot(out_ptr);

      if (result.size() != size)
        throw std::runtime_error("Size mismatch");

      if (*std::min_element(result.begin(), result.end()) < min ||
        *std::max_element(result.begin(), result.end()) > max)
        throw std::runtime_error("Out of range");

      if (i % (rounds / 10) == 0)
        std::cout << ".";
    }

    end = std::chrono::high_resolution_clock::now();
    ms = std::chrono::duration<double, std::milli>(end - start).count();

    std::cout << " rounds = " << rounds
      << "; t = " << ms << "ms; td = " << (ms / rounds) << "ms\n\n";
  }


  static void test_next_crypto_double_aot(int rounds)
  {
    std::cout << "test_next_crypto_double_aot: ";
    std::mt19937_64 rng(std::random_device{}());
    std::uniform_real_distribution<double> dist(0.0, 1e300);
    auto start = std::chrono::high_resolution_clock::now();

    for (int i = 0; i < rounds; i++)
    {
      cerror_t err;
      double result = next_crypto_double_aot(&err);
      assert_error(err);

      if (result < 0)
        throw std::runtime_error("Negative result");

      if (i % (rounds / 10) == 0)
        std::cout << ".";
    }

    auto end = std::chrono::high_resolution_clock::now();
    double ms = std::chrono::duration<double, std::milli>(end - start).count();

    std::cout << " rounds = " << rounds
      << "; t = " << ms << "ms; td = " << (ms / rounds) << "ms\n";


    std::cout << "test_next_crypto_double_max_aot: ";
    start = std::chrono::high_resolution_clock::now();

    for (int i = 0; i < rounds; i++)
    {
      cerror_t err;
      double max = dist(rng);
      double result = next_crypto_double_max_aot(max, &err);
      assert_error(err);

      if (result < 0 || result > max)
        throw std::runtime_error("Out of range");

      if (i % (rounds / 10) == 0)
        std::cout << ".";
    }

    end = std::chrono::high_resolution_clock::now();
    ms = std::chrono::duration<double, std::milli>(end - start).count();

    std::cout << " rounds = " << rounds
      << "; t = " << ms << "ms; td = " << (ms / rounds) << "ms\n";


    std::cout << "test_next_crypto_double_min_max_aot: ";
    start = std::chrono::high_resolution_clock::now();

    for (int i = 0; i < rounds; i++)
    {
      double min = DBL_MAX / 3;
      double max = min * 2;

      cerror_t err;
      double result = next_crypto_double_min_max_aot(min, max, &err);
      assert_error(err);

      if (result < min || result > max)
        throw std::runtime_error("Out of range");

      if (i % (rounds / 10) == 0)
        std::cout << ".";
    }

    end = std::chrono::high_resolution_clock::now();
    ms = std::chrono::duration<double, std::milli>(end - start).count();

    std::cout << " rounds = " << rounds
      << "; t = " << ms << "ms; td = " << (ms / rounds) << "ms\n\n";
  }

  static void test_rng_crypto_double_aot(int rounds)
  {
    std::cout << "test_rng_crypto_double_aot: ";

    std::mt19937_64 rng(std::random_device{}());
    std::uniform_int_distribution<int64_t> dist_size(128, 512);
    std::uniform_real_distribution<double> dist(0.0, 1e300);
    auto start = std::chrono::high_resolution_clock::now();

    for (int i = 0; i < rounds; i++)
    {
      void* out_ptr = nullptr;
      int size = (int)dist_size(rng);
      auto err = rng_crypto_double_aot(size, &out_ptr);
      assert_error(err);

      if (!out_ptr)
        throw std::runtime_error("Null pointer");

      std::vector<double> result((double*)out_ptr, (double*)out_ptr + size);
      free_buffer_aot(out_ptr);

      if (result.size() != size)
        throw std::runtime_error("Size mismatch");

      if (i % (rounds / 10) == 0)
        std::cout << ".";
    }
    auto end = std::chrono::high_resolution_clock::now();
    double ms = std::chrono::duration<double, std::milli>(end - start).count();

    std::cout << " rounds = " << rounds
      << "; t = " << ms << "ms; td = " << (ms / rounds) << "ms\n";

    std::cout << "test_rng_crypto_double_max_aot: ";
    start = std::chrono::high_resolution_clock::now();

    for (int i = 0; i < rounds; i++)
    {
      double max = dist(rng);
      int size = (int)dist_size(rng);

      void* out_ptr = nullptr;
      auto err = rng_crypto_double_max_aot(size, max, &out_ptr);
      assert_error(err);

      std::vector<double> result((double*)out_ptr, (double*)out_ptr + size);
      free_buffer_aot(out_ptr);

      if (result.size() != size)
        throw std::runtime_error("Size mismatch");

      if (*std::min_element(result.begin(), result.end()) < 0 ||
        *std::max_element(result.begin(), result.end()) > max)
        throw std::runtime_error("Out of range");

      if (i % (rounds / 10) == 0)
        std::cout << ".";
    }

    end = std::chrono::high_resolution_clock::now();
    ms = std::chrono::duration<double, std::milli>(end - start).count();

    std::cout << " rounds = " << rounds
      << "; t = " << ms << "ms; td = " << (ms / rounds) << "ms\n";

    std::cout << "test_rng_crypto_double_min_max_aot: ";
    start = std::chrono::high_resolution_clock::now();

    for (int i = 0; i < rounds; i++)
    {
      double min = DBL_MAX / 3;
      double max = min * 2;
      int size = (int)dist_size(rng);

      void* out_ptr = nullptr;
      auto err = rng_crypto_double_min_max_aot(size, min, max, &out_ptr);
      assert_error(err);

      std::vector<double> result((double*)out_ptr, (double*)out_ptr + size);
      free_buffer_aot(out_ptr);

      if (result.size() != size)
        throw std::runtime_error("Size mismatch");

      if (*std::min_element(result.begin(), result.end()) < min ||
        *std::max_element(result.begin(), result.end()) > max)
        throw std::runtime_error("Out of range");

      if (i % (rounds / 10) == 0)
        std::cout << ".";
    }

    end = std::chrono::high_resolution_clock::now();
    ms = std::chrono::duration<double, std::milli>(end - start).count();

    std::cout << " rounds = " << rounds
      << "; t = " << ms << "ms; td = " << (ms / rounds) << "ms\n\n";
  }


  static void test_next_crypto_single_aot(int rounds)
  {

    std::cout << "test_next_crypto_single_aot: ";
    std::mt19937 rng(std::random_device{}());
    std::uniform_real_distribution<float> dist(0.0f, 1e30f);
    auto start = std::chrono::high_resolution_clock::now();

    for (int i = 0; i < rounds; i++)
    {
      cerror_t err;
      float result = next_crypto_single_aot(&err);
      assert_error(err);

      if (result < 0)
        throw std::runtime_error("Negative result");

      if (i % (rounds / 10) == 0)
        std::cout << ".";
    }

    auto end = std::chrono::high_resolution_clock::now();
    double ms = std::chrono::duration<double, std::milli>(end - start).count();

    std::cout << " rounds = " << rounds
      << "; t = " << ms << "ms; td = " << (ms / rounds) << "ms\n";


    std::cout << "test_next_crypto_single_max_aot: ";
    start = std::chrono::high_resolution_clock::now();

    for (int i = 0; i < rounds; i++)
    {
      cerror_t err;
      float max = dist(rng);
      float result = next_crypto_single_max_aot(max, &err);
      assert_error(err);

      if (result < 0 || result > max)
        throw std::runtime_error("Out of range");

      if (i % (rounds / 10) == 0)
        std::cout << ".";
    }

    end = std::chrono::high_resolution_clock::now();
    ms = std::chrono::duration<double, std::milli>(end - start).count();

    std::cout << " rounds = " << rounds
      << "; t = " << ms << "ms; td = " << (ms / rounds) << "ms\n";


    std::cout << "test_next_crypto_single_min_max_aot: ";
    start = std::chrono::high_resolution_clock::now();

    for (int i = 0; i < rounds; i++)
    {
      float min = FLT_MAX / 3;
      float max = min * 2;

      cerror_t err;
      float result = next_crypto_single_min_max_aot(min, max, &err);
      assert_error(err);

      if (result < min || result > max)
        throw std::runtime_error("Out of range");

      if (i % (rounds / 10) == 0)
        std::cout << ".";
    }

    end = std::chrono::high_resolution_clock::now();
    ms = std::chrono::duration<double, std::milli>(end - start).count();

    std::cout << " rounds = " << rounds
      << "; t = " << ms << "ms; td = " << (ms / rounds) << "ms\n\n";
  }

  static void test_rng_crypto_single_aot(int rounds)
  {
    std::cout << "test_rng_crypto_single_aot: ";

    std::mt19937 rng(std::random_device{}());
    std::uniform_int_distribution<int> dist_size(128, 512);
    std::uniform_real_distribution<float> dist(0.0, 1e30f);
    auto start = std::chrono::high_resolution_clock::now();

    for (int i = 0; i < rounds; i++)
    {
      void* out_ptr = nullptr;
      int size = dist_size(rng);
      auto err = rng_crypto_single_aot(size, &out_ptr);
      assert_error(err);

      if (!out_ptr)
        throw std::runtime_error("Null pointer");

      std::vector<float> result((float*)out_ptr, (float*)out_ptr + size);
      free_buffer_aot(out_ptr);

      if (result.size() != size)
        throw std::runtime_error("Size mismatch");

      if (i % (rounds / 10) == 0)
        std::cout << ".";
    }
    auto end = std::chrono::high_resolution_clock::now();
    double ms = std::chrono::duration<double, std::milli>(end - start).count();

    std::cout << " rounds = " << rounds
      << "; t = " << ms << "ms; td = " << (ms / rounds) << "ms\n";

    std::cout << "test_rng_crypto_single_max_aot: ";
    start = std::chrono::high_resolution_clock::now();

    for (int i = 0; i < rounds; i++)
    {
      float max = dist(rng);
      int size = dist_size(rng);

      void* out_ptr = nullptr;
      auto err = rng_crypto_single_max_aot(size, max, &out_ptr);
      assert_error(err);

      std::vector<float> result((float*)out_ptr, (float*)out_ptr + size);
      free_buffer_aot(out_ptr);

      if (result.size() != size)
        throw std::runtime_error("Size mismatch");

      if (*std::min_element(result.begin(), result.end()) < 0 ||
        *std::max_element(result.begin(), result.end()) > max)
        throw std::runtime_error("Out of range");

      if (i % (rounds / 10) == 0)
        std::cout << ".";
    }

    end = std::chrono::high_resolution_clock::now();
    ms = std::chrono::duration<double, std::milli>(end - start).count();

    std::cout << " rounds = " << rounds
      << "; t = " << ms << "ms; td = " << (ms / rounds) << "ms\n";

    std::cout << "test_rng_crypto_single_min_max_aot: ";
    start = std::chrono::high_resolution_clock::now();

    for (int i = 0; i < rounds; i++)
    {
      int size = dist_size(rng);
      float min = FLT_MAX / 3;
      float max = min * 2;

      void* out_ptr = nullptr;
      auto err = rng_crypto_single_min_max_aot(size, min, max, &out_ptr);
      assert_error(err);

      std::vector<float> result((float*)out_ptr, (float*)out_ptr + size);
      free_buffer_aot(out_ptr);

      if (result.size() != size)
        throw std::runtime_error("Size mismatch");

      if (*std::min_element(result.begin(), result.end()) < min ||
        *std::max_element(result.begin(), result.end()) > max)
        throw std::runtime_error("Out of range");

      if (i % (rounds / 10) == 0)
        std::cout << ".";
    }

    end = std::chrono::high_resolution_clock::now();
    ms = std::chrono::duration<double, std::milli>(end - start).count();

    std::cout << " rounds = " << rounds
      << "; t = " << ms << "ms; td = " << (ms / rounds) << "ms\n\n";
  }


  static void test_next_crypto_decimal_aot(int rounds)
  {
    //The placeholders are included in bridge.h. Just uncomment them.
    //If anyone wants to maintain them, they can follow the same examples as above.
  }

  static void test_rng_crypto_decimal_aot(int rounds)
  {
    //The placeholders are included in bridge.h. Just uncomment them.
    //If anyone wants to maintain them, they can follow the same examples as above.
  }


  void start_crypto_random_native(int rounds)
  {
    test_rng_crypto_bool_aot(rounds);
    test_rng_crypto_bytes_aot(rounds);

    test_next_crypto_int32_aot(rounds);
    test_rng_crypto_int32_aot(rounds);

    test_next_crypto_int64_aot(rounds);
    test_rng_crypto_int64_aot(rounds);

    test_next_crypto_double_aot(rounds);
    test_rng_crypto_double_aot(rounds);

    test_next_crypto_single_aot(rounds);
    test_rng_crypto_single_aot(rounds);

    test_next_crypto_decimal_aot(rounds);
    test_rng_crypto_decimal_aot(rounds);
  }
}