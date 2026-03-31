#include "pch.h"

#include <iostream>
#include "crypto_aes_gcm_test.h"


namespace michele::natale::Tests
{
  static void start_native_aes_gcm_crypto(int rounds)
  {
    test_aes_gcm_file(rounds);
    test_aes_gcm_bytes(rounds);
    test_aes_gcm_bytes_stress();

    std::cout << "\n";
  }


  void start_aes_gcm_native(int rounds)
  {
    start_native_aes_gcm_crypto(rounds);
  }
}