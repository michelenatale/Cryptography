#include "pch.h"
#include <iostream>

#include "crypto_hash_hmac_test.h"

namespace michele::natale::Tests
{

  void start_crypto_hash_hmac_native(int rounds)
  {
    start_crypto_hash_native(rounds);
    start_crypto_hmac_native(rounds);
  }
}