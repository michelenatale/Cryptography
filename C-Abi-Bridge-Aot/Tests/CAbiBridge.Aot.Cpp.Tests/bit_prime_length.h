#pragma once

#include <array>
#include <iostream>


#ifndef __BIT_PRIME_LENGTH_AOT_H__
#define __BIT_PRIME_LENGTH_AOT_H__

enum class bit_prime_length : int32_t
{
  B8 = 8,
  B16 = 16,
  B32 = 32,
  B64 = 64,
  B96 = 96,
  B128 = 128,
  B192 = 192,
  B256 = 256,
  B384 = 384,
  B512 = 512,
  B768 = 768,
  B1024 = 1024,
  B1536 = 1536,
  B2048 = 2048,
  B3072 = 3072,
  B4096 = 4096,
  B6144 = 6144,
  B8192 = 8192,
  B12288 = 12288,
  B16384 = 16384,
  B24576 = 24576,
  B32768 = 32768,
  B49152 = 49152,
  B65536 = 65536,
  B98304 = 98304,
  B131072 = 131072
};

std::array<bit_prime_length, 26> to_bit_prime_lengths()
{
  return std::array<bit_prime_length, 26>
  {
    bit_prime_length::B8,
    bit_prime_length::B16,
    bit_prime_length::B32,
    bit_prime_length::B64,
    bit_prime_length::B96,
    bit_prime_length::B128,
    bit_prime_length::B192,
    bit_prime_length::B256,
    bit_prime_length::B384,
    bit_prime_length::B512,
    bit_prime_length::B768,
    bit_prime_length::B1024,
    bit_prime_length::B1536,
    bit_prime_length::B2048,
    bit_prime_length::B3072,
    bit_prime_length::B4096,
    bit_prime_length::B6144,
    bit_prime_length::B8192,
    bit_prime_length::B12288,
    bit_prime_length::B16384,
    bit_prime_length::B24576,
    bit_prime_length::B32768,
    bit_prime_length::B49152,
    bit_prime_length::B65536,
    bit_prime_length::B98304,
    bit_prime_length::B131072,
  };
}

#endif