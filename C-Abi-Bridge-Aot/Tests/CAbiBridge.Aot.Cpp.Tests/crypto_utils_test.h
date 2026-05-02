#pragma once 

#include <vector>
#include <string>
#include <future>
#include <iostream>
#include <filesystem>

#include "cerror.h"

#ifndef __CRYPTO_UTILS_TEST_H__
#define __CRYPTO_UTILS_TEST_H__

namespace michele::natale::Tests
{
  int rng_int();
  bool rng_even(); 
  int rng_int(int min, int max);
  void assert_error(cerror_t err);
  uint8_t* to_malloc_uint8(int size);
  std::string to_lower(std::string str);
  int64_t file_size_fs(const char* path);
  std::vector<uint8_t> rng_bytes(size_t size);
  std::vector<uint8_t> to_bytes(void* ptr, int length);
  void set_rng_file_data(const std::string& filename, int size);
  std::pair<int64_t, int> sum_file_sizes_folder(const char* foldername);
  std::pair<int64_t, int> sum_file_sizes(const char* files[], int count);
  std::future<void> set_rng_file_data_async(const std::string& filename, int size);

}

#endif