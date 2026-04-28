#pragma once

#include <iostream>

#ifndef __CRYPTO_PRIMES__H__
#define __CRYPTO_PRIMES__H__

namespace michele::natale::Tests
{
  void test_next_crypto_primes_min_max_uint64(int rounds);
  void test_next_crypto_primes_min_max_biginteger(int rounds);
  void test_next_crypto_primes_bits_biginteger(int rounds);

  void test_rng_crypto_primes_min_max_uint64(int rounds);
  void test_rng_crypto_primes_min_max_biginteger(int rounds);
  void test_rng_crypto_primes_bits_biginteger(int rounds);
}

#endif