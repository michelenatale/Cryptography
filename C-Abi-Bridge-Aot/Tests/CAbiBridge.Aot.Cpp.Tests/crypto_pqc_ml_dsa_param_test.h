#pragma once

#include <iostream>
#include <vector>
#include <string>

#include "ml_dsa_param.h"

#ifndef __CRYPTO_PQC_ML_DSA_PARAM_FILE_H__
#define __CRYPTO_PQC_ML_DSA_PARAM_FILE_H__

namespace michele::natale::Tests
{
  template<typename T>
  bool is_null_or_empty(const std::vector<T>& v)
  {
    return v.empty();
  }

  inline std::array<ml_dsa_param, 3> to_ml_dsa_params()
  {
    return
    {
      ml_dsa_param::Ml_Dsa_44,
      ml_dsa_param::Ml_Dsa_65,
      ml_dsa_param::Ml_Dsa_87
    };
  }

  inline const ml_dsa_param to_ml_dsa_param(uint8_t num)
  {
    switch (num)
    {
      case  0: return ml_dsa_param::Ml_Dsa_44;
      case  1: return ml_dsa_param::Ml_Dsa_65;
      case  2: return ml_dsa_param::Ml_Dsa_87;
      default: return ml_dsa_param::Invalid;
    }
  }

  inline const uint8_t from_ml_dsa_param(ml_dsa_param param)
  {
    switch (param)
    {
      case ml_dsa_param::Ml_Dsa_44: return 0;
      case ml_dsa_param::Ml_Dsa_65: return 1;
      case ml_dsa_param::Ml_Dsa_87: return 2;
      default: return 255;
    }
  }

  inline const char* to_ml_dsa_param_name(ml_dsa_param param)
  {
    switch (param)
    {
      case ml_dsa_param::Ml_Dsa_44: return "ml-dsa-44";
      case ml_dsa_param::Ml_Dsa_65: return "ml-dsa-65";
      case ml_dsa_param::Ml_Dsa_87: return "ml-dsa-87";
      default: return "Unknown";
    }
  }

  inline ml_dsa_param from_ml_dsa_param_name(const std::string& name)
  {
    if (name == "ml-dsa-44") return ml_dsa_param::Ml_Dsa_44;
    if (name == "ml-dsa-65") return ml_dsa_param::Ml_Dsa_65;
    if (name == "ml-dsa-87") return ml_dsa_param::Ml_Dsa_87;

    return ml_dsa_param::Invalid;
  }

}

#endif