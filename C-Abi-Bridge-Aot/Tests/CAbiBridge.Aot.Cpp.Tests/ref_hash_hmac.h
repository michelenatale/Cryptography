#pragma once



#ifndef __REF_HASH_HMAC__H__
#define __REF_HASH_HMAC__H__


namespace michele::natale::Tests
{
  std::vector<uint8_t> ref_md5_hash(const std::vector<uint8_t>& data);
  std::vector<uint8_t> ref_md5_hmac(const std::vector<uint8_t>& key, const std::vector<uint8_t>& data);

  std::vector<uint8_t> ref_sha1_hash(const std::vector<uint8_t>& data);
  std::vector<uint8_t> ref_sha1_hmac(const std::vector<uint8_t>& key, const std::vector<uint8_t>& data);

  //sha
  std::vector<uint8_t> ref_sha_256_hash(const std::vector<uint8_t>& data);
  std::vector<uint8_t> ref_sha_256_hmac(const std::vector<uint8_t>& key, const std::vector<uint8_t>& data);

  std::vector<uint8_t> ref_sha_384_hash(const std::vector<uint8_t>& data);
  std::vector<uint8_t> ref_sha_384_hmac(const std::vector<uint8_t>& key, const std::vector<uint8_t>& data);

  std::vector<uint8_t> ref_sha_512_hash(const std::vector<uint8_t>& data);
  std::vector<uint8_t> ref_sha_512_hmac(const std::vector<uint8_t>& key, const std::vector<uint8_t>& data);

  //sha3
  std::vector<uint8_t> ref_sha3_256_hash(const std::vector<uint8_t>& data);
  std::vector<uint8_t> ref_sha3_256_hmac(const std::vector<uint8_t>& key, const std::vector<uint8_t>& data);

  std::vector<uint8_t> ref_sha3_384_hash(const std::vector<uint8_t>& data);
  std::vector<uint8_t> ref_sha3_384_hmac(const std::vector<uint8_t>& key, const std::vector<uint8_t>& data);

  std::vector<uint8_t> ref_sha3_512_hash(const std::vector<uint8_t>& data);
  std::vector<uint8_t> ref_sha3_512_hmac(const std::vector<uint8_t>& key, const std::vector<uint8_t>& data);

  std::vector<uint8_t> ref_shake_128_hash(const std::vector<uint8_t>& data, size_t outlen);
  std::vector<uint8_t> ref_shake_256_hash(const std::vector<uint8_t>& data, size_t outlen);

}


#endif