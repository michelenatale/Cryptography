#include "pch.h"

#include <chrono>
#include <iostream>
//#include <random>
#include <vector>
#include <limits> 
#include <cstring>
#include <stdexcept>
#include <algorithm>
#include <type_traits>
#include <openssl/bn.h> 


#include "bridge.h"
#include "test_servises.h"
#include "crypto_utils_test.h"

 

namespace michele::natale::test_service
{
  using namespace michele::natale::Tests;

  std::vector<uint8_t> rng_bytes_s(
    int size, bool non_zeros)
  {
    void* out_ptr = nullptr;
    auto err = rng_crypto_bytes_aot(
      size, &out_ptr);
    assert_error(err);

    std::vector<uint8_t> result(((uint8_t*)out_ptr), ((uint8_t*)out_ptr) + size);
    free_buffer_aot(out_ptr);

    return result;
  }


  std::vector<uint8_t> rng_base_x_number(int size, int basex)
  {
    return rng_ints<uint8_t>(size, 0, static_cast<uint8_t>(basex));
  }
}