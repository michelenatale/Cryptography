#pragma once

#include <span>
#include <iostream>

#include "crypto_utils_test.h"
#include "test_servises_const.h"

#ifndef __TEST_SERVICES_H__
#define __TEST_SERVICES_H__

namespace michele::natale::test_service
{
  std::vector<uint8_t> rng_bytes_s(int size, bool non_zeros);
  std::vector<uint8_t> rng_base_x_number(int size, int basex);


  std::vector<uint8_t> trim(std::span<const uint8_t> bytes);
  std::vector<uint8_t> trim_last(std::span<const uint8_t> bytes);
  std::vector<uint8_t> trim_first(std::span<const uint8_t> bytes);
  std::span<const uint8_t> trim_first_span(std::span<const uint8_t> bytes);
  bool sequence_equal(const std::vector<uint8_t>& a, const std::vector<uint8_t>& b);

  std::pair<int, int> rng_bases_2_256();
  std::vector<uint8_t> to_base_x_2_256_le_s(const BIGNUM* bi, int target_base);
  std::vector<uint8_t> to_base_x_2_256_le_s(const std::string& number, int target_base);
  std::vector<uint8_t> converter_2_256_le_s(const BIGNUM* bi, int start_base, int target_base);
  std::vector<uint8_t> from_base_x_2_256_le_s(const uint8_t* bytes, int length, int from_base_x);
  std::vector<uint8_t> to_base_x_utf8_2_256_le_s(const uint8_t* bytes, int length, int target_base);
  std::vector<uint8_t> to_base_x_2_256_le_s(const uint8_t* number, int number_length, int target_base);
  std::vector<uint8_t> converter_2_256_le_s(const uint8_t* bytes, int length, int start_base, int target_base);


  template<typename T>
  bool IsNullOrEmpty(const std::vector<T>* v)
  {
    return v == nullptr || v->empty();
  }

  template<typename T>
  T rng_int()
  {
    static_assert(is_supported_numeric<T>(), "Unsupported numeric type");

    return rng_int<T>(zero<T>(), max_value<T>());
  }

  template<typename T>
  T rng_int(T min, T max)
  {
    static_assert(is_supported_numeric<T>(), "Unsupported numeric type");

    if (min >= max)
      throw std::invalid_argument("min >= max");

    T d = max - min;

    // Zufallsbytes für einen Wert
    std::vector<uint8_t> bytes(sizeof(T));
    bytes = rng_bytes(sizeof(T), true);

    // Unaligned read wie in C#
    T tmp;
    std::memcpy(&tmp, bytes.data(), sizeof(T));

    tmp = abs_val(tmp);
    tmp %= d;
    tmp += min;

    return tmp;
  }


  template<typename T>
  std::vector<T> rng_ints(int size, T min, T max)
  {
    static_assert(is_supported_numeric<T>(), "Unsupported numeric type");

    if (size == 0)
      return {};

    std::vector<T> result(size);

    // Zufallsbytes für alle Werte
    auto bytes = rng_bytes_s(size * sizeof(T), true);

    T d = max - min;

    for (size_t i = 0; i < size; i++)
    {
      T tmp;
      std::memcpy(&tmp, &bytes[i * sizeof(T)], sizeof(T));

      tmp = abs_val(tmp);
      tmp %= d; tmp += min;

      result[i] = tmp;
    }

    return result;
  }
}

#endif