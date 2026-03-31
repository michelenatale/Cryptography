#include "pch.h"

#include <vector>
#include <iostream>
//#include <string>
//#include <chrono>
//#include <random>
//#include <stdexcept>
//#include <cstring>
#include <openssl/evp.h>
#include <openssl/hmac.h>

#include "ref_hash_hmac.h"

namespace michele::natale::Tests
{

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
}
