
#include "pch.h"
#include <cstdlib>

#include "crypto_aes_test.h"



namespace michele::natale::Tests
{
  static void start_native_aes_crypto(int rounds)
  {
    test_aes_file(rounds);
    test_aes_bytes(rounds);
    test_aes_bytes_stress();

    std::cout << "\n";
  }

  void start_aes_native(int rounds)
  {
    start_native_aes_crypto(rounds);
  }
}