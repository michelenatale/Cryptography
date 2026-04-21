#include "pch.h"

#include <vector>
#include <iostream>
#include <openssl/evp.h>
#include <openssl/hmac.h>

#include "ref_hash_hmac.h"

//WICHTIG: OpenSSL ist nur zu testzwecken hier eingespannt.
//IMPORTANT: OpenSSL is included here for testing purposes only.

namespace michele::natale::Tests
{
  // ************ ************ ************ ************ ************ 

  //md5
  std::vector<uint8_t> ref_md5_hash(const std::vector<uint8_t>& data)
  {
    std::vector<uint8_t> out(EVP_MD_size(EVP_md5()));
    unsigned int len = 0;
    EVP_Digest(data.data(), data.size(), out.data(), &len, EVP_md5(), nullptr);
    return out;
  }

  std::vector<uint8_t> ref_md5_hmac(
    const std::vector<uint8_t>& key,
    const std::vector<uint8_t>& data)
  {
    std::vector<uint8_t> out(EVP_MD_size(EVP_md5()));
    unsigned int len = 0;
    HMAC(EVP_md5(), key.data(), (int)key.size(),
      data.data(), data.size(),
      out.data(), &len);
    return out;
  }

  //sha1
  std::vector<uint8_t> ref_sha1_hash(const std::vector<uint8_t>& data)
  {
    std::vector<uint8_t> out(EVP_MD_size(EVP_sha1()));
    unsigned int len = 0;
    EVP_Digest(data.data(), data.size(), out.data(), &len, EVP_sha1(), nullptr);
    return out;
  }

  std::vector<uint8_t> ref_sha1_hmac(
    const std::vector<uint8_t>& key,
    const std::vector<uint8_t>& data)
  {
    std::vector<uint8_t> out(EVP_MD_size(EVP_sha1()));
    unsigned int len = 0;
    HMAC(EVP_sha1(), key.data(), (int)key.size(),
      data.data(), data.size(),
      out.data(), &len);
    return out;
  }

  // ************ ************ ************ ************ ************ 

  //sha256
  std::vector<uint8_t> ref_sha_256_hash(const std::vector<uint8_t>& data)
  {
    std::vector<uint8_t> out(EVP_MD_size(EVP_sha256()));
    unsigned int len = 0;
    EVP_Digest(data.data(), data.size(), out.data(), &len, EVP_sha256(), nullptr);
    return out;
  }

  std::vector<uint8_t> ref_sha_256_hmac(
    const std::vector<uint8_t>& key,
    const std::vector<uint8_t>& data)
  {
    std::vector<uint8_t> out(EVP_MD_size(EVP_sha256()));
    unsigned int len = 0;
    HMAC(EVP_sha256(), key.data(), (int)key.size(),
      data.data(), data.size(),
      out.data(), &len);
    return out;
  }


  //sha384
  std::vector<uint8_t> ref_sha_384_hash(const std::vector<uint8_t>& data)
  {
    std::vector<uint8_t> out(EVP_MD_size(EVP_sha384()));
    unsigned int len = 0;
    EVP_Digest(data.data(), data.size(), out.data(), &len, EVP_sha384(), nullptr);
    return out;
  }

  std::vector<uint8_t> ref_sha_384_hmac(
    const std::vector<uint8_t>& key,
    const std::vector<uint8_t>& data)
  {
    std::vector<uint8_t> out(EVP_MD_size(EVP_sha384()));
    unsigned int len = 0;
    HMAC(EVP_sha384(), key.data(), (int)key.size(),
      data.data(), data.size(),
      out.data(), &len);
    return out;
  }


  //sha512
  std::vector<uint8_t> ref_sha_512_hash(const std::vector<uint8_t>& data)
  {
    std::vector<uint8_t> out(EVP_MD_size(EVP_sha512()));
    unsigned int len = 0;
    EVP_Digest(data.data(), data.size(), out.data(), &len, EVP_sha512(), nullptr);
    return out;
  }

  std::vector<uint8_t> ref_sha_512_hmac(
    const std::vector<uint8_t>& key,
    const std::vector<uint8_t>& data)
  {
    std::vector<uint8_t> out(EVP_MD_size(EVP_sha512()));
    unsigned int len = 0;
    HMAC(EVP_sha512(), key.data(), (int)key.size(),
      data.data(), data.size(),
      out.data(), &len);
    return out;
  }

  // ************ ************ ************ ************ ************ 

    //sha3_256
  std::vector<uint8_t> ref_sha3_256_hash(const std::vector<uint8_t>& data)
  {
    std::vector<uint8_t> out(EVP_MD_size(EVP_sha3_256()));
    unsigned int len = 0;
    EVP_Digest(data.data(), data.size(), out.data(), &len, EVP_sha3_256(), nullptr);
    return out;
  }

  std::vector<uint8_t> ref_sha3_256_hmac(
    const std::vector<uint8_t>& key,
    const std::vector<uint8_t>& data)
  {
    std::vector<uint8_t> out(EVP_MD_size(EVP_sha3_256()));
    unsigned int len = 0;
    HMAC(EVP_sha3_256(), key.data(), (int)key.size(),
      data.data(), data.size(),
      out.data(), &len);
    return out;
  }


  //sha3_384
  std::vector<uint8_t> ref_sha3_384_hash(const std::vector<uint8_t>& data)
  {
    std::vector<uint8_t> out(EVP_MD_size(EVP_sha3_384()));
    unsigned int len = 0;
    EVP_Digest(data.data(), data.size(), out.data(), &len, EVP_sha3_384(), nullptr);
    return out;
  }

  std::vector<uint8_t> ref_sha3_384_hmac(
    const std::vector<uint8_t>& key,
    const std::vector<uint8_t>& data)
  {
    std::vector<uint8_t> out(EVP_MD_size(EVP_sha3_384()));
    unsigned int len = 0;
    HMAC(EVP_sha3_384(), key.data(), (int)key.size(),
      data.data(), data.size(),
      out.data(), &len);
    return out;
  }


  //sha3_512
  std::vector<uint8_t> ref_sha3_512_hash(const std::vector<uint8_t>& data)
  {
    std::vector<uint8_t> out(EVP_MD_size(EVP_sha3_512()));
    unsigned int len = 0;
    EVP_Digest(data.data(), data.size(), out.data(), &len, EVP_sha3_512(), nullptr);
    return out;
  }

  std::vector<uint8_t> ref_sha3_512_hmac(
    const std::vector<uint8_t>& key,
    const std::vector<uint8_t>& data)
  {
    unsigned int len = 0;
    std::vector<uint8_t> out(EVP_MD_size(EVP_sha3_512()));
    HMAC(EVP_sha3_512(), key.data(), (int)key.size(),
      data.data(), data.size(),
      out.data(), &len);
    return out;
  }

  // ************ ************ ************ ************ ************ 

  //shake_128
  std::vector<uint8_t> ref_shake_128_hash(const std::vector<uint8_t>& data, size_t outlen)
  {
    EVP_MD_CTX* ctx = EVP_MD_CTX_new();
    std::vector<uint8_t> out(outlen);

    EVP_DigestInit_ex(ctx, EVP_shake128(), nullptr);
    EVP_DigestUpdate(ctx, data.data(), data.size());
    EVP_DigestFinalXOF(ctx, out.data(), outlen);

    EVP_MD_CTX_free(ctx);
    return out;
  }


  //shake_256
  std::vector<uint8_t> ref_shake_256_hash(const std::vector<uint8_t>& data, size_t outlen)
  {
    EVP_MD_CTX* ctx = EVP_MD_CTX_new();
    std::vector<uint8_t> out(outlen);

    EVP_DigestInit_ex(ctx, EVP_shake256(), nullptr);
    EVP_DigestUpdate(ctx, data.data(), data.size());
    EVP_DigestFinalXOF(ctx, out.data(), outlen);

    EVP_MD_CTX_free(ctx);
    return out;
  }


  // ************ ************ ************ ************ ************ 
}
