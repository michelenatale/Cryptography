#pragma once

#include <vector>
#include <iostream>

struct usi_ptr_t
{
  std::vector<uint8_t> buf;

  usi_ptr_t() = default;
  explicit usi_ptr_t(std::vector<uint8_t> b) : buf(std::move(b)) {}

  bool IsEmpty() const { return buf.empty(); }
  int  Length() const { return (int)(buf.size()); }

  const uint8_t* ToBytes() const { return buf.data(); }
  uint8_t* ToBytes() { return buf.data(); }

  bool Equality(const usi_ptr_t& other) const
  {
    return buf.size() == other.buf.size() &&
      std::equal(buf.begin(), buf.end(), other.buf.begin());
  }
};