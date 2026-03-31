#pragma once

#include "cabi_exp_imp.h"
#include "crypto_utils_test.h"

namespace michele::natale::Tests
{
  void start_convert_encoding_native(int rounds);

  void start_convert_encoding_hex_native(int rounds);
  void start_convert_encoding_base64_native(int rounds);

}