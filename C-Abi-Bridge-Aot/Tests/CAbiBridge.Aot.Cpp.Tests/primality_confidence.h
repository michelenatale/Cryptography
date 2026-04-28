#pragma once

#include <array>
#include <iostream>


#ifndef __PRIMALITY_CONFIDENCE_AOT_H__
#define __PRIMALITY_CONFIDENCE_AOT_H__

enum class primality_confidence : int32_t
{
  Bottom = 32,
  Normal = 40,
  Authorities = 64,
  Bankings = 72,
  Top = 96,
  Extreme = 128
};


std::array<primality_confidence, 6> to_primality_confidences()
{
  return std::array<primality_confidence, 6>{
    primality_confidence::Bottom,
      primality_confidence::Normal,
      primality_confidence::Authorities,
      primality_confidence::Bankings,
      primality_confidence::Top,
      primality_confidence::Extreme
  };
}

#endif