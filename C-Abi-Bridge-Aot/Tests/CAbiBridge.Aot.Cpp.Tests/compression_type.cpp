
#include "pch.h"


#include "compression_type.h"


std::array<compression_type_t, 3> to_compression_type()
{
  return std::array<compression_type_t, 3>{
    compression_type_t::None,
      compression_type_t::Brotli,
      compression_type_t::GZip,
  };
}