#pragma once

#include <iostream>
#include <openssl/bn.h>     // BIGNUM, BN_new, BN_free, BN_CTX, BN_CTX_new, BN_CTX_free, BN_is_prime_ex, BN_set_word, BN_cmp, BN_num_bits
#include <openssl/rand.h>   // RAND_bytes


#ifndef __BN_CTX_X__AOT_H__
#define __BN_CTX_X__AOT_H__

struct BNPtr {
  BIGNUM* ptr;

  BNPtr() : ptr(BN_new()) {
    if (!ptr)
      throw std::runtime_error("BN_new failed");
  }

  explicit BNPtr(BIGNUM* p) : ptr(p) {}

  ~BNPtr() {
    if (ptr)
      BN_free(ptr);
  }

  BIGNUM* get() const { return ptr; }
  BIGNUM** addr() { return &ptr; }

  BNPtr(const BNPtr&) = delete;
  BNPtr& operator=(const BNPtr&) = delete;

  BNPtr(BNPtr&& other) noexcept : ptr(other.ptr) {
    other.ptr = nullptr;
  }

  BNPtr& operator=(BNPtr&& other) noexcept {
    if (this != &other) {
      if (ptr)
        BN_free(ptr);
      ptr = other.ptr;
      other.ptr = nullptr;
    }
    return *this;
  }
};


struct BN_CTX_Ptr {
  BN_CTX* ctx;

  BN_CTX_Ptr() : ctx(BN_CTX_new()) {
    if (!ctx)
      throw std::runtime_error("BN_CTX_new failed");
  }

  ~BN_CTX_Ptr() {
    if (ctx)
      BN_CTX_free(ctx);
  }

  BN_CTX* get() const { return ctx; }

  BN_CTX_Ptr(const BN_CTX_Ptr&) = delete;
  BN_CTX_Ptr& operator=(const BN_CTX_Ptr&) = delete;

  BN_CTX_Ptr(BN_CTX_Ptr&& other) noexcept : ctx(other.ctx) {
    other.ctx = nullptr;
  }

  BN_CTX_Ptr& operator=(BN_CTX_Ptr&& other) noexcept {
    if (this != &other) {
      if (ctx)
        BN_CTX_free(ctx);
      ctx = other.ctx;
      other.ctx = nullptr;
    }
    return *this;
  }
};

#endif