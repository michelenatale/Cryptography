#pragma once

#include "cabi_exp_imp.h"
#include "crypto_utils_test.h"


#ifndef __CRYPTO_CHACHA_POLY_TEST_H__
#define __CRYPTO_CHACHA_POLY_TEST_H__

namespace michele::natale::Tests
{
  void start_chacha_poly_native(int rounds);

  void test_chacha_poly_file(int rounds);
  void test_chacha_poly_bytes(int rounds);
  void test_chacha_poly_bytes_stress();
}

#endif