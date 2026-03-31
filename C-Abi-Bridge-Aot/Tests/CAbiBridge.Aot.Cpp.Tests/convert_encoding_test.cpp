#include "pch.h"

#include "convert_encoding_test.h"


namespace michele::natale::Tests
{
  void start_convert_encoding_native(int rounds)
  {
    start_convert_encoding_hex_native(rounds);
    start_convert_encoding_base64_native(rounds);

  }
}