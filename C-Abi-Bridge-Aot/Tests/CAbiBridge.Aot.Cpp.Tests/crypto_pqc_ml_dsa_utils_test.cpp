#include "pch.h"

#include <iostream>
#include <vector>
#include <string>
#include <cstdint>
#include <cstring>
#include <algorithm>
#include <filesystem>
#include <unordered_map>

#include "bridge.h"
#include "cerror.h"
#include "variables.h"
#include "usi_ptr_t.h"
#include "crypto_utils_test.h"
#include "ml_dsa_signer_info.h"
#include "crypto_pqc_ml_dsa_utils_test.h"
#include "crypto_pqc_ml_dsa_param_test.h"

namespace michele::natale::Tests
{
  key_pair_param_info create_native_ml_dsa_key_pair()
  {
    int pub_key_length = 0;
    int guid_id_length = 0;
    int priv_key_length = 0;
    uint8_t mldsa_param = 0;

    uint8_t* pub_key_ptr = nullptr;
    uint8_t* guid_id_ptr = nullptr;
    uint8_t* priv_key_ptr = nullptr;

    cerror_t err = create_mldsa_key_pair_aot(
      &priv_key_ptr, &priv_key_length,
      &pub_key_ptr, &pub_key_length,
      &guid_id_ptr, &guid_id_length,
      &mldsa_param);
    assert_error(err);

    key_pair_param_info info;

    // Copy + free
    info.priv_key.assign(priv_key_ptr, priv_key_ptr + priv_key_length);
    free_buffer_aot(priv_key_ptr);

    info.pub_key.assign(pub_key_ptr, pub_key_ptr + pub_key_length);
    free_buffer_aot(pub_key_ptr);

    info.guid.assign(guid_id_ptr, guid_id_ptr + guid_id_length);
    free_buffer_aot(guid_id_ptr);

    info.algo = static_cast<ml_dsa_param>(mldsa_param);

    return info;
  }

  key_pair_param_info create_native_ml_dsa_key_pair(ml_dsa_param algo)
  {
    int guid_id_length = 0;
    int pub_key_length = 0;
    int priv_key_length = 0;

    uint8_t* guid_id_ptr = nullptr;
    uint8_t* pub_key_ptr = nullptr;
    uint8_t* priv_key_ptr = nullptr;
    uint8_t algo_byte = static_cast<uint8_t>(algo);

    cerror_t err = create_mldsa_key_pair_param_aot(
      algo_byte,
      &priv_key_ptr, &priv_key_length,
      &pub_key_ptr, &pub_key_length,
      &guid_id_ptr, &guid_id_length);

    assert_error(err);

    key_pair_param_info info;
    info.priv_key.assign(priv_key_ptr, priv_key_ptr + priv_key_length);
    free_buffer_aot(priv_key_ptr);

    info.pub_key.assign(pub_key_ptr, pub_key_ptr + pub_key_length);
    free_buffer_aot(pub_key_ptr);

    info.guid.assign(guid_id_ptr, guid_id_ptr + guid_id_length);
    free_buffer_aot(guid_id_ptr);

    info.algo = algo;

    return info;
  }

  void save_native_ml_dsa_key_pair(
    key_pair_param_info kppi, std::string file_path,
    bool with_priv_key)
  {
    std::vector<uint8_t> vec_empty = {};
    auto file_ptr = reinterpret_cast<const uint8_t*>(file_path.data());
    std::vector<uint8_t> privk = with_priv_key ? kppi.priv_key : vec_empty;

    auto err = save_pqc_mldsa_key_pair_aot(
      file_ptr, (int)file_path.length(),
      privk.data(), (int)privk.size(),
      kppi.pub_key.data(), (int)kppi.pub_key.size(),
      kppi.guid.data(), (int)kppi.guid.size(),
      from_ml_dsa_param(kppi.algo), true);
    assert_error(err);
  }

  void save_native_ml_dsa_key_pair(
    std::vector<uint8_t> guid, std::vector<uint8_t> priv_key,
    std::vector<uint8_t>pub_key, ml_dsa_param param,
    std::string file_path, bool with_priv_key)
  {
    if (guid.empty()) throw std::runtime_error("save_native_ml_dsa_key_pair: guid is empty");
    if (pub_key.empty()) throw std::runtime_error("save_native_ml_dsa_key_pair: pub_key is empty");
    //if (priv_key.empty()) throw std::runtime_error("save_native_ml_dsa_key_pair: priv_key is empty");

    key_pair_param_info info;
    info.guid = guid; info.algo = param;
    info.pub_key = pub_key; info.priv_key = priv_key;

    return save_native_ml_dsa_key_pair(info, file_path, with_priv_key);
  }

  key_pair_param_info load_native_ml_dsa_key_pair(const std::string& filename)
  {

    std::vector<uint8_t> fname(filename.begin(), filename.end());

    uint8_t* priv_ptr = nullptr, * pub_ptr = nullptr, * guid_ptr = nullptr;
    int priv_len = 0, pub_len = 0, guid_len = 0;
    uint8_t algo_byte = 0;

    cerror_t err = load_pqc_mldsa_key_pair_aot(
      fname.data(), (int)fname.size(),
      &priv_ptr, &priv_len,
      &pub_ptr, &pub_len,
      &guid_ptr, &guid_len,
      &algo_byte);

    assert_error(err);

    key_pair_param_info result;

    result.priv_key.assign(priv_ptr, priv_ptr + priv_len);
    free_buffer_aot(priv_ptr);

    result.pub_key.assign(pub_ptr, pub_ptr + pub_len);
    free_buffer_aot(pub_ptr);

    result.guid.assign(guid_ptr, guid_ptr + guid_len);
    free_buffer_aot(guid_ptr);

    result.algo = (ml_dsa_param)algo_byte;

    return result;
  }

  std::vector<key_pair_param_info> load_native_ml_dsa_key_pair_all(const std::string& folder)
  {
    std::vector<key_pair_param_info> result;

    // Dateien im Ordner sammeln
    std::vector<std::string> files;
    for (auto& entry : std::filesystem::directory_iterator(folder))
    {
      if (entry.is_regular_file())
        files.push_back(entry.path().string());
    }

    std::sort(files.begin(), files.end());

    for (auto& fname : files)
    {
      // UTF‑8 Filename → Bytes
      std::vector<uint8_t> filename(fname.begin(), fname.end());

      uint8_t* priv_ptr = nullptr, * pub_ptr = nullptr, * guid_ptr = nullptr;
      int priv_len = 0, pub_len = 0, guid_len = 0;
      uint8_t algo_byte = 0;

      cerror_t err = load_pqc_mldsa_key_pair_aot(
        filename.data(), (int)filename.size(),
        &priv_ptr, &priv_len,
        &pub_ptr, &pub_len,
        &guid_ptr, &guid_len,
        &algo_byte);

      assert_error(err);

      key_pair_param_info kppi; 

      kppi.priv_key.assign(priv_ptr, priv_ptr + priv_len);
      free_buffer_aot(priv_ptr);

      kppi.pub_key.assign(pub_ptr, pub_ptr + pub_len);
      free_buffer_aot(pub_ptr);

      kppi.guid.assign(guid_ptr, guid_ptr + guid_len);
      free_buffer_aot(guid_ptr);

      kppi.algo = (ml_dsa_param)algo_byte;

      result.push_back(std::move(kppi));
    }

    return result;
  }

  std::unordered_map<std::string, std::string>
    create_ml_dsa_key_pair_and_save(const std::vector<std::string>& signernames,
      const std::string& folder)
  {
    // Ordner löschen + neu erstellen
    std::filesystem::remove_all(folder);
    std::filesystem::create_directories(folder);

    std::unordered_map<std::string, std::string> dict;

    for (auto& name : signernames)
    {
      key_pair_param_info kppi = create_native_ml_dsa_key_pair();

      std::string guid_str(kppi.guid.begin(), kppi.guid.end());
      std::string filename = folder + "/mldsa_keypair-" + to_lower(name) + ".key";

      std::vector<uint8_t> fname(filename.begin(), filename.end());

      cerror_t err = save_pqc_mldsa_key_pair_aot(
        fname.data(), (int)fname.size(),
        kppi.priv_key.data(), (int)kppi.priv_key.size(),
        kppi.pub_key.data(), (int)kppi.pub_key.size(),
        kppi.guid.data(), (int)kppi.guid.size(),
        (uint8_t)kppi.algo, true);
      assert_error(err);

      dict[guid_str] = name;
    }

    return dict;
  }

  std::vector<ml_dsa_signer_info> extract_data_and_sort(
    const std::vector<key_pair_param_info>& kppis,
    const std::unordered_map<std::string, std::string>& dict,
    const std::vector<uint8_t>& message,
    const std::string& projectname)
  {
    std::vector<ml_dsa_signer_info> list;

    for (auto& k : kppis)
    {
      std::string guid_str(k.guid.begin(), k.guid.end());
      //std::string signername = dict.at(guid_str);
      list.push_back(to_ml_dsa_signer_info(
        k, message, dict.at(guid_str), projectname));
    }

    std::sort(list.begin(), list.end(),
      [](auto& a, auto& b)
        {
          return a.signature < b.signature;
        });

    return list;
  }

  std::vector<ml_dsa_signer_info> extract_data_and_sort(
    const std::vector<key_pair_param_info>& kppis,
    const std::unordered_map<std::string, std::string>& dict,
    const std::string& srcfile,
    const std::string& projectname)
  {
    std::vector<ml_dsa_signer_info> list;

    for (auto& k : kppis)
    {
      std::string guid_str(k.guid.begin(), k.guid.end());
      //std::string signername = dict.at(guid_str);
      list.push_back(to_ml_dsa_signer_info(
        k, srcfile, dict.at(guid_str), projectname));
    }

    std::sort(list.begin(), list.end(),
      [](auto& a, auto& b)
    {
      return a.signature < b.signature;
    });

    return list;
  }

  ml_dsa_signer_info to_ml_dsa_signer_info(
    const key_pair_param_info& kppi,
    const std::string& filedata,
    const std::string& signername,
    const std::string& projectname)
  {
    // Datei-Name in Bytes (UTF-8)
    std::vector<uint8_t> fname(filedata.begin(), filedata.end());

    // Signatur erzeugen
    uint8_t* sign_ptr = nullptr;
    int sign_len = 0;

    cerror_t err = pqc_mldsa_sign_file_aot(
      fname.data(), (int)fname.size(),
      kppi.priv_key.data(), (int)kppi.priv_key.size(),
      (uint8_t)kppi.algo,
      &sign_ptr, &sign_len);

    assert_error(err);

    ml_dsa_signer_info info;

    info.signature.assign(sign_ptr, sign_ptr + sign_len);
    free_buffer_aot(sign_ptr);

    info.signer_id = kppi.guid;
    info.public_key = kppi.pub_key;
    info.pqc_sign_algo = (uint8_t)1; // ML-DSA
    info.pqc_sign_algo_param = (uint8_t)kppi.algo;
    info.signer_name.assign(signername.begin(), signername.end());
    info.project_name.assign(projectname.begin(), projectname.end());

    return info;
  }

  ml_dsa_signer_info to_ml_dsa_signer_info(
    const key_pair_param_info& kppi,
    std::vector<uint8_t> message,
    const std::string& signername,
    const std::string& projectname)
  {

    // Signatur erzeugen
    int sign_len = 0;
    uint8_t* sign_ptr = nullptr;

    cerror_t err = pqc_mldsa_sign_aot(
      message.data(), (int)message.size(),
      kppi.priv_key.data(), (int)kppi.priv_key.size(),
      (uint8_t)kppi.algo,
      &sign_ptr, &sign_len);

    assert_error(err);

    ml_dsa_signer_info info;

    info.signature.assign(sign_ptr, sign_ptr + sign_len);
    free_buffer_aot(sign_ptr);

    info.signer_id = kppi.guid;
    info.public_key = kppi.pub_key;
    info.pqc_sign_algo = (uint8_t)1; // ML-DSA
    info.pqc_sign_algo_param = (uint8_t)kppi.algo;
    info.signer_name.assign(signername.begin(), signername.end());
    info.project_name.assign(projectname.begin(), projectname.end());

    return info;
  }
}