#pragma once

#include "pch.h"

#include <limits> 
#include <iostream>
#include <algorithm>

#ifndef __TEST_SERVICES_CONST_H__
#define __TEST_SERVICES_CONST_H__

namespace michele::natale::test_service
{
  // Zero
  template<typename T>
  constexpr T zero() { return T(0); }

  // MaxValue
  template<typename T>
  constexpr T max_value() { return std::numeric_limits<T>::max(); }

  // Abs (für signed)
  template<typename T>
  T abs_val(T v) {
    if constexpr (std::is_signed<T>::value)
      return v < 0 ? -v : v;
    else
      return v;
  }

  template<typename T>
  constexpr bool is_supported_numeric() {
    return std::is_integral<T>::value;
  }
}

#endif