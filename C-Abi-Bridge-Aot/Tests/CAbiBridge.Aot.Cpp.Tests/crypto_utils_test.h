#pragma once 

#include <vector>
#include <string>
#include <future>
#include <iostream>

#include "cerror.h"

#ifndef __CRYPTO_UTILS_TEST_H__
#define __CRYPTO_UTILS_TEST_H__

namespace michele::natale::Tests
{
  bool rng_even(); 
  int rng_int(int min, int max);
  void assert_error(cerror_t err);
  uint8_t* to_malloc_uint8(int size);
  std::string to_lower(std::string str);
  std::vector<uint8_t> rng_bytes(size_t size);
  std::vector<uint8_t> to_bytes(void* ptr, int length);
  void set_rng_file_data(const std::string& filename, int size);
  std::future<void> set_rng_file_data_async(const std::string& filename, int size);

}

#endif