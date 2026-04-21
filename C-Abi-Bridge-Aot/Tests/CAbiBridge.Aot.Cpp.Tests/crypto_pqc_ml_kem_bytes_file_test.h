#pragma once

#ifndef __CRYPTO_PQC_ML_KEM_BYTES_FILE_H__
#define __CRYPTO_PQC_ML_KEM_BYTES_FILE_H__

namespace michele::natale::Tests
{

  template<typename T>
  bool is_null_or_empty(const std::vector<T>& v)
  {
    return v.empty();
  }

  enum class MLKemParam : uint8_t 
  { 
    Ml_Kem_512 = 0,
    Ml_Kem_768,
    Ml_Kem_1024,
  };

  enum class CryptionAlgorithm : uint8_t 
  { 
    AES = 0,
    AES_GCM,
    CHACHA20_POLY1305,
  };

  static MLKemParam to_ml_kem_algorithm(MLKemParam p) { return p; }
  static MLKemParam to_ml_kem_algorithm() { return MLKemParam::Ml_Kem_512; }
  static MLKemParam from_ml_kem_algorithm(MLKemParam p) { return p; }
}


#endif