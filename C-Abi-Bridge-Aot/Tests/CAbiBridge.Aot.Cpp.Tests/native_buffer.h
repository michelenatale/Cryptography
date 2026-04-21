#pragma once

#include <vector> 
#include <iostream>

class native_buffer
{
public:
  native_buffer(const std::vector<uint8_t>& src)
  {
    len_ = (int)src.size();

    if (len_ == 0)
    {
      ptr_ = nullptr;
      return;
    }

    ptr_ = (uint8_t*)malloc(len_);
    if (!ptr_)
      throw std::bad_alloc();

    memcpy(ptr_, src.data(), len_);
  }


  ~native_buffer()
  {
    if (ptr_) free(ptr_);
  }

  uint8_t* ptr() const { return ptr_; }
  int len() const { return len_; }

private:
  uint8_t* ptr_ = nullptr;
  int len_ = 0;
};