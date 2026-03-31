#include "pch.h"

#include <iostream>
#include "crypto_utils_test.h"
#include "crypto_chacha_poly_test.h"


namespace michele::natale::Tests
{
  static void start_native_chacha_poly_crypto(int rounds)
  {
    test_chacha_poly_file(rounds);
    test_chacha_poly_bytes(rounds);
    test_chacha_poly_bytes_stress();

    std::cout << "\n";
  }


  void start_chacha_poly_native(int rounds)
  {
    start_native_chacha_poly_crypto(rounds);
  }
}