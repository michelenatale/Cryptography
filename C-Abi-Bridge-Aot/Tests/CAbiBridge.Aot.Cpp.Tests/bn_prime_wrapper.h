#pragma once

#include <stdexcept>
#include <openssl/bn.h>     
#include <openssl/rand.h>   


#ifndef __BN_PRIME_WRAPPER_AOT_H__
#define __BN_PRIME_WRAPPER_AOT_H__

// Hilfsfunktion: (base^exp mod mod)
inline BN_CTX* ensure_ctx(BN_CTX* ctx) {
  return ctx ? ctx : BN_CTX_new();
}

inline bool bn_modexp(BIGNUM* r, const BIGNUM* base, const BIGNUM* exp, const BIGNUM* mod, BN_CTX* ctx)
{
  return BN_mod_exp(r, base, exp, mod, ctx) == 1;
}

// Miller–Rabin primality test (ohne BN_is_prime_ex)
inline bool bn_is_probable_prime_mr(const BIGNUM* n, int rounds, BN_CTX* ctx)
{
  BN_CTX* local_ctx = ensure_ctx(ctx);
  BN_CTX_start(local_ctx);

  BIGNUM* n_minus_1 = BN_CTX_get(local_ctx);
  BIGNUM* d = BN_CTX_get(local_ctx);
  BIGNUM* a = BN_CTX_get(local_ctx);
  BIGNUM* x = BN_CTX_get(local_ctx);
  BIGNUM* tmp = BN_CTX_get(local_ctx);

  if (!tmp)
    throw std::runtime_error("BN_CTX_get failed");

  // n <= 3 → direkt behandeln
  if (BN_cmp(n, BN_value_one()) <= 0) return false;
  if (BN_cmp(n, BN_value_one()) == 0) return false;
  if (BN_cmp(n, BN_value_one()) == 1 && BN_cmp(n, BN_value_one()) <= 3) return true;

  // n - 1
  BN_copy(n_minus_1, n);
  BN_sub_word(n_minus_1, 1);

  // Schreibe n-1 = d * 2^s
  BN_copy(d, n_minus_1);
  int s = 0;
  while (!BN_is_bit_set(d, 0)) {
    BN_rshift1(d, d);
    s++;
  }

  // Runden
  for (int i = 0; i < rounds; i++)
  {
    // a = random in [2, n-2]
    BN_rand_range(a, n_minus_1);
    if (BN_cmp(a, BN_value_one()) <= 0)
      BN_add_word(a, 2);

    // x = a^d mod n
    if (!bn_modexp(x, a, d, n, local_ctx))
      return false;

    if (BN_cmp(x, BN_value_one()) == 0 || BN_cmp(x, n_minus_1) == 0)
      continue;

    bool composite = true;

    for (int r = 1; r < s; r++)
    {
      // x = x^2 mod n
      BN_mod_mul(x, x, x, n, local_ctx);

      if (BN_cmp(x, n_minus_1) == 0) {
        composite = false;
        break;
      }
    }

    if (composite) {
      BN_CTX_end(local_ctx);
      if (!ctx) BN_CTX_free(local_ctx);
      return false;
    }
  }

  BN_CTX_end(local_ctx);
  if (!ctx) BN_CTX_free(local_ctx);
  return true;
}

inline bool bn_is_prime(const BIGNUM* bn, int rounds, BN_CTX* ctx)
{
  return bn_is_probable_prime_mr(bn, rounds, ctx);
}

#endif