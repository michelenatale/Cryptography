#pragma once

#include <array>
#include <cstdint>

enum class compression_type_t : std::uint8_t
{
  None = 0,
  Brotli = 1,
  GZip = 2
};


std::array<compression_type_t, 3> to_compression_type();