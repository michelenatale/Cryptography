#include "pch.h"

#include <vector>
#include <string>
#include <future>
#include <fstream>
#include <iostream>
#include <algorithm>

#include "bridge.h"
#include "cerror.h"
#include "crypto_utils_test.h"


namespace michele::natale::Tests
{
  uint8_t* to_malloc_uint8(int size)
  {
    size += 1;
    uint8_t* result = (uint8_t*)malloc(size);
    if (!result)
    {
      free(result);
      throw std::bad_alloc();
    }
    memset(result, 0, size);

    return result;
  }

  std::future<void> set_rng_file_data_async(const std::string& filename, int size)
  {
    return std::async(std::launch::async, [=]() {
      set_rng_file_data(filename, size);
    });
  }

  void set_rng_file_data(const std::string& filename, int size)
  {
    std::ofstream fsout(filename, std::ios::binary | std::ios::trunc);

    int length = size < 1024 * 1024 ? size : 1024 * 1024;

    while (length > 0)
    {
      auto data = rng_bytes(length);
      fsout.write(reinterpret_cast<const char*>(data.data()), data.size());

      size -= length;
      length = size < 1024 * 1024 ? size : 1024 * 1024;
    }
  }

  // Hilfsfunktion: kopiert aus void* + length in vector<uint8_t>
  std::vector<uint8_t> to_bytes(void* ptr, int length)
  {
    uint8_t* p = static_cast<uint8_t*>(ptr);
    return std::vector<uint8_t>(p, p + length);
  }

  // Hilfsfunktion: Fehler prüfen
  void assert_error(cerror_t err)
  {
    if (err.error_code != (int)cerror_code_t::Ok)
    {
      std::cerr << "Crypto error\n";
      std::abort();
    }
  }

  // ************ ************ ************ ************ 
  // ************ ************ ************ ************ 

  std::string to_lower(std::string str)
  {
    std::transform(str.begin(), str.end(), str.begin(),
      [](unsigned char c) { return std::tolower(c); });
    return str;
  }

  // ************ ************ ************ ************ 
  // ************ ************ ************ ************ 

  std::vector<uint8_t> rng_bytes(size_t size)
  {
    auto sz = (int)size;
    void* out_ptr = nullptr;

    auto err = rng_crypto_bytes_aot(sz, &out_ptr);
    assert_error(err);

    auto bytes = to_bytes(out_ptr, sz);
    free_buffer_aot(out_ptr);

    return bytes;
  }

  int rng_int()
  {
    cerror_t err{};
    int result = next_crypto_int32_aot(&err);
    assert_error(err);

    return result;
  }

  int rng_int(int min, int max)
  {
    cerror_t err{};
    int result = next_crypto_int32_min_max_aot(min, max, &err);
    assert_error(err);

    return result;
  }

  bool rng_even()
  {
    cerror_t err{};
    int result = next_crypto_int32_aot(&err);
    assert_error(err);

    return (result & 1) == 0;
  }

}