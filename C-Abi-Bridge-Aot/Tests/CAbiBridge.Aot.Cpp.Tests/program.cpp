
#include "pch.h"
#include <iostream>

#include "crypto_aes_test.h"
#include "crypto_pqc_test.h"
#include "crypto_primes_test.h"
#include "crypto_random_test.h"
#include "crypto_aes_gcm_test.h"
#include "convert_encoding_test.h"
#include "crypto_hash_hmac_test.h"
#include "crypto_pqc_ml_kem_test.h"
#include "crypto_pqc_ml_dsa_test.h"
#include "crypto_chacha_poly_test.h"

static void tests(int rounds)
{
  std::cout << "C-Abi-Bridge.Aot.Tests C++\n\n";

  using namespace michele::natale::Tests;

  start_crypto_random_native(rounds * 1000);
  start_crypto_hash_hmac_native(rounds * 1000);

  start_aes_native(rounds);
  start_aes_gcm_native(rounds);
  start_chacha_poly_native(rounds);

  start_pqc_native(rounds);

  start_crypto_primes_native(rounds);

  start_convert_encoding_native(rounds * 1000);
}


int main()
{
  int rounds = 10;
  tests(rounds);

  std::cout << "\n\n";
  std::cout << "Finish\n";
}
