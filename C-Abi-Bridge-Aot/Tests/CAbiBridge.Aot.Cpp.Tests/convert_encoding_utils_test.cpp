#include "pch.h"

#include <string>
#include <vector>
#include <iomanip>
#include <sstream>
#include <iostream>

#include "convert_encoding_utils_test.h"

namespace michele::natale::Tests
{
  std::string hex_str_lower(const uint8_t* data, size_t length)
  {
    static const char* hex_table = "0123456789abcdef";

    std::string result;
    result.resize(length * 2);

    for (size_t i = 0; i < length; ++i)
    {
      uint8_t b = data[i];
      result[2 * i] = hex_table[b >> 4];
      result[2 * i + 1] = hex_table[b & 0x0F];
    }

    return result;
  }

  std::string hex_str_upper(const uint8_t* data, size_t length)
  {
    static const char* hex_table = "0123456789ABCDEF";

    std::string result;
    result.resize(length * 2);

    for (size_t i = 0; i < length; ++i)
    {
      uint8_t b = data[i];
      result[2 * i] = hex_table[b >> 4];
      result[2 * i + 1] = hex_table[b & 0x0F];
    }

    return result;
  }


  static const char b64_table[] =
    "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
    "abcdefghijklmnopqrstuvwxyz"
    "0123456789+/";

  std::string encode64(const uint8_t* data, size_t length)
  {
    std::string result;
    result.reserve(((length + 2) / 3) * 4);

    size_t i = 0;
    while (i < length)
    {
      uint32_t v = 0;
      int bytes = 0;

      // pack up to 3 bytes into a 24-bit block
      for (; bytes < 3 && i < length; bytes++, i++)
        v = (v << 8) | data[i];
      v <<= (3 - bytes) * 8; // pad with zeros

      // output 4 chars
      result.push_back(b64_table[(v >> 18) & 0x3F]);
      result.push_back(b64_table[(v >> 12) & 0x3F]);
      result.push_back(bytes > 1 ? b64_table[(v >> 6) & 0x3F] : '=');
      result.push_back(bytes > 2 ? b64_table[(v >> 0) & 0x3F] : '=');
    }

    return result;
  }

}