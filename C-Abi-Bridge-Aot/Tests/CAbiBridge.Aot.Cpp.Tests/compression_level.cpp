
#include "pch.h"


#include "compression_level.h"


std::array<compression_level_t, 4> to_compression_level()
{
  return std::array<compression_level_t, 4>{
    compression_level_t::Optimal,
      compression_level_t::Fastest,
      compression_level_t::NoCompression,
      compression_level_t::SmallestSize,
  };
}