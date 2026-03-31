#pragma once

namespace michele::natale::Tests
{
  std::vector<uint8_t> ref_md5_hash(const std::vector<uint8_t>& data);
  std::vector<uint8_t> ref_md5_hmac(const std::vector<uint8_t>& key, const std::vector<uint8_t>& data);

  std::vector<uint8_t> ref_sha1_hash(const std::vector<uint8_t>& data);
  std::vector<uint8_t> ref_sha1_hmac(const std::vector<uint8_t>& key, const std::vector<uint8_t>& data);

  std::vector<uint8_t> ref_sha_256_hash(const std::vector<uint8_t>& data);
  std::vector<uint8_t> ref_sha_256_hmac(const std::vector<uint8_t>& key, const std::vector<uint8_t>& data);

  std::vector<uint8_t> ref_sha_384_hash(const std::vector<uint8_t>& data);
  std::vector<uint8_t> ref_sha_384_hmac(const std::vector<uint8_t>& key, const std::vector<uint8_t>& data);

  std::vector<uint8_t> ref_sha_512_hash(const std::vector<uint8_t>& data);
  std::vector<uint8_t> ref_sha_512_hmac(const std::vector<uint8_t>& key, const std::vector<uint8_t>& data);


}