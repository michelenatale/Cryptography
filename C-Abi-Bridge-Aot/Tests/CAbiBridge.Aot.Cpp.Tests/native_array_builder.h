#pragma once

#include <vector>
#include <iostream>


class native_array_builder
{
public:
  native_array_builder(size_t count)
    : count_(count)
  {
    lens_ = (int*)malloc(count * sizeof(int));
    ptrs_ = (uint8_t**)malloc(count * sizeof(uint8_t*));
  }

  ~native_array_builder()
  {
    if (ptrs_)
    {
      for (size_t i = 0; i < count_; ++i)
        free(ptrs_[i]);
      free(ptrs_);
    }
    if (lens_)
      free(lens_);
  }

  void set(size_t index, const std::vector<uint8_t>& data)
  {
    if (data.empty())
    {
      lens_[index] = 0;
      ptrs_[index] = nullptr;
      return;
    }

    uint8_t* p = (uint8_t*)malloc(data.size());
    if (!p)
      throw std::bad_alloc();

    memcpy(p, data.data(), data.size());

    ptrs_[index] = p;
    lens_[index] = (int)data.size();
  }

  int* lens() const { return lens_; }
  size_t count() const { return count_; }
  uint8_t** ptrs() const { return ptrs_; }

private:
  size_t count_;
  int* lens_ = nullptr;
  uint8_t** ptrs_ = nullptr;
};
