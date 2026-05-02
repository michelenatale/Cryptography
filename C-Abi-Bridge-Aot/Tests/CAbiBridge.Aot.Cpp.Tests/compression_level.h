#pragma once

#include <array>
#include <cstdint>

enum class compression_level_t : std::uint8_t
{
  Optimal = 0,
  Fastest = 1,
  NoCompression = 2,
  SmallestSize = 3
};


std::array<compression_level_t, 4> to_compression_level();