
#include "pch.h"


#include "fcp_type.h"



std::array<fcp_type_t, 2> to_fcp_type()
{
  return std::array<fcp_type_t, 2>{
    fcp_type_t::File,
      fcp_type_t::Archiv,
  };
}