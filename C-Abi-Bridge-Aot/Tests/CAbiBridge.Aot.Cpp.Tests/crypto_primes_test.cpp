
#include "pch.h"
#include <iostream>

#include "crypto_primes.h"
#include "crypto_primes_test.h"


namespace michele::natale::Tests
{

  void start_crypto_primes_native(int rounds)
  {
    test_next_crypto_primes_min_max_uint64( rounds);
    test_next_crypto_primes_min_max_biginteger(rounds);
    test_next_crypto_primes_bits_biginteger( rounds);

    test_rng_crypto_primes_min_max_uint64( rounds);
    test_rng_crypto_primes_min_max_biginteger(rounds);
    test_rng_crypto_primes_bits_biginteger( rounds);
  }
}
