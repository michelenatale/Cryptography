#pragma once

#include <array>
#include <cstdint>

enum class fcp_type_t : std::uint8_t
{
  File = 1,
  Archiv = 2
};

std::array<fcp_type_t, 2> to_fcp_type();