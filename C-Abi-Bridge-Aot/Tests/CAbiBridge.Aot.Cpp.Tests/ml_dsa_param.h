#pragma once

#include <iostream>
#include <stdint.h>
#include <array>
#include <string>

namespace michele::natale::Tests
{
  enum class ml_dsa_param : uint8_t
  {
    Ml_Dsa_44 = 0,
    Ml_Dsa_65 = 1,
    Ml_Dsa_87 = 2,

    Invalid = 255,
  };
}