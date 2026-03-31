#pragma once


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

  // Platzhalter – in deinem Code sind das echte Mapping-Funktionen.
  static MLKemParam to_ml_kem_algorithm(MLKemParam p) { return p; }
  static MLKemParam to_ml_kem_algorithm() { return MLKemParam::Ml_Kem_512; }
  static MLKemParam from_ml_kem_algorithm(MLKemParam p) { return p; }

  // RandomNumberGenerator.GetInt32(min,max)
  inline int rand_int(int min_inclusive, int max_exclusive)
  {
    static thread_local std::mt19937 rng{ std::random_device{}() };
    std::uniform_int_distribution<int> dist(min_inclusive, max_exclusive - 1);
    return dist(rng);
  }

  // int.IsEvenInteger(rand.Next())
  static bool rand_even()
  {
    static thread_local std::mt19937 rng{ std::random_device{}() };
    return (rng() & 1) == 0;
  }
}