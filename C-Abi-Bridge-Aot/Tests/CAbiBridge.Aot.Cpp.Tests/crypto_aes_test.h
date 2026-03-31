#pragma once

#include "cabi_exp_imp.h"
#include "crypto_utils_test.h"


namespace michele::natale::Tests
{
  void start_aes_native(int rounds);

  void test_aes_file(int rounds);
  void test_aes_bytes(int rounds);
  void test_aes_bytes_stress();
}