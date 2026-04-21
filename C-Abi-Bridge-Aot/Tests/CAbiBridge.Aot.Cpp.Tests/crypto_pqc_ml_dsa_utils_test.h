#pragma once

#include <vector>
#include <cstdint>
#include <cstring>
#include <iostream> 

#include "ml_dsa_param.h"
#include "ml_dsa_signer_info.h"


#ifndef __CRYPTO_PQC_ML_DSA_UTILS_TEST_H__
#define __CRYPTO_PQC_ML_DSA_UTILS_TEST_H__

namespace michele::natale::Tests
{
  struct key_pair_param_info
  {
    std::vector<uint8_t> guid;
    std::vector<uint8_t> pub_key;
    std::vector<uint8_t> priv_key;
    ml_dsa_param algo = ml_dsa_param::Invalid;

    key_pair_param_info() = default;

    key_pair_param_info(
      std::vector<uint8_t> guid, std::vector<uint8_t> pubkey,
      std::vector<uint8_t> privkey, ml_dsa_param algo)
      : guid(std::move(guid)), pub_key(std::move(pubkey)),
      algo(algo), priv_key(std::move(privkey))
    {}

    void Reset()
    {
      guid.clear();
      pub_key.clear();
      priv_key.clear();
      algo = static_cast<ml_dsa_param>(255); // Invalid
    }

    // Optional: für Vergleiche
    bool Equals(const key_pair_param_info& other) const
    {
      return algo == other.algo &&
        guid == other.guid &&
        pub_key == other.pub_key &&
        priv_key == other.priv_key;
    }
  };

  key_pair_param_info create_native_ml_dsa_key_pair();
  key_pair_param_info create_native_ml_dsa_key_pair(ml_dsa_param algo);
  key_pair_param_info load_native_ml_dsa_key_pair(const std::string& folder);
  std::vector<key_pair_param_info> load_native_ml_dsa_key_pair_all(const std::string& folder);
  std::unordered_map<std::string, std::string> create_ml_dsa_key_pair_and_save(const std::vector<std::string>& signernames, const std::string& folder);

  void save_native_ml_dsa_key_pair(key_pair_param_info kppi, std::string file_path, bool with_priv_key = true);
  void save_native_ml_dsa_key_pair(std::vector<uint8_t> guid, std::vector<uint8_t> priv_key, std::vector<uint8_t>pub_key, ml_dsa_param param, std::string file_path, bool with_priv_key = true);

  ml_dsa_signer_info to_ml_dsa_signer_info(const key_pair_param_info& kppi, const std::string& filedata, const std::string& signername, const std::string& projectname);
  ml_dsa_signer_info to_ml_dsa_signer_info(const key_pair_param_info& kppi, std::vector<uint8_t> message, const std::string& signername, const std::string& projectname);

  std::vector<ml_dsa_signer_info> extract_data_and_sort(const std::vector<key_pair_param_info>& kppis, const std::unordered_map<std::string, std::string>& dict, const std::string& srcfile, const std::string& projectname);
  std::vector<ml_dsa_signer_info> extract_data_and_sort(const std::vector<key_pair_param_info>& kppis, const std::unordered_map<std::string, std::string>& dict, const std::vector<uint8_t>& message, const std::string& projectname);

}

#endif