#include "pch.h"

#include <span>
#include <iostream> 
#include <vector>
#include <cstring>
#include <stdexcept>
#include <algorithm>
#include <openssl/bn.h> 


#include "bridge.h"
#include "test_servises.h"
#include "crypto_utils_test.h"




namespace michele::natale::test_service
{
  // Vergleich wie SequenceEqual
  bool sequence_equal(const std::vector<uint8_t>& a,
    const std::vector<uint8_t>& b)
  {
    return a.size() == b.size() &&
      std::equal(a.begin(), a.end(), b.begin());
  }

  std::vector<uint8_t> trim_last(std::span<const uint8_t> bytes)
  {
    if (bytes.empty())
      return {};

    size_t idx = 0;

    while (idx < bytes.size() && bytes[bytes.size() - 1 - idx] == 0)
      idx++;

    size_t new_len = bytes.size() - idx;

    return std::vector<uint8_t>(bytes.begin(), bytes.begin() + new_len);
  }

  std::vector<uint8_t> trim_first(std::span<const uint8_t> bytes)
  {
    if (bytes.empty())
      return {};

    size_t idx = 0;

    while (idx < bytes.size() && bytes[idx] == 0)
      idx++;

    return std::vector<uint8_t>(bytes.begin() + idx, bytes.end());
  }

  std::vector<uint8_t> trim(std::span<const uint8_t> bytes)
  {
    auto first = trim_first(bytes);
    return trim_last(first);
  }

  std::span<const uint8_t> trim_first_span(std::span<const uint8_t> bytes)
  {
    size_t idx = 0;
    while (idx < bytes.size() && bytes[idx] == 0)
      idx++;
    return bytes.subspan(idx);
  }

} 