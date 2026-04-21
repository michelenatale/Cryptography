#pragma once

#include <cstdint>
#include <cstddef>

//see too:
//[..]C-Abi-Bridge-Aot\Src\CAbiBridge.Aot\C-Abi-Bridge-Crypto-Aot\C-Abi-Bridget-Error.cs


#ifndef __CERROR_BRIDGE_AOT_H__
#define __CERROR_BRIDGE_AOT_H__

enum class cerror_code_t : int32_t
{
  Ok = 0,
  NullPointer = -1,
  InvalidLength = -2,
  IoError = -3,
  CryptoError = -4,
  OutOfRange = -5,
  UnknownError = -99
};

struct cerror_t
{
  int32_t     error_code;
  const char* message;
};

#endif