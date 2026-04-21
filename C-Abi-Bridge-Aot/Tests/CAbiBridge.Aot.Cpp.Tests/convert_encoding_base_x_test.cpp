#include "pch.h"

#include <chrono>
#include <iostream>
#include <random>
#include <cstdint>
#include <vector>
#include <cstring>
#include <stdexcept>
#include <algorithm>
#include <openssl/bn.h>


#include "bridge.h"
#include "test_servises.h"
#include "convert_encoding_test_start.h"

namespace michele::natale::Tests
{
  static uint64_t bytes_to_uint64(const uint8_t* b)
  {
    uint64_t value;
    std::memcpy(&value, b, sizeof(value));
    return value;
  }

  static std::vector<uint8_t> string_to_bytes(const std::string& s)
  {
    std::vector<uint8_t> out;
    out.reserve(s.size());

    for (unsigned char c : s)
      out.push_back(static_cast<uint8_t>(c - '0'));

    return out;
  }


  static void test_base_converter_2_256(int rounds)
  { 
    std::cout << "test_base_converter_2_256_aot: ";

    using namespace michele::natale::test_service;

    auto start = std::chrono::high_resolution_clock::now();

    for (int i = 0; i < rounds; i++)
    {
      auto bases = rng_bases_2_256();
      int startbase = bases.first;
      int targetbase = bases.second;

      // 8 Random Bytes
      auto rng = rng_bytes_s(8, true);
      auto ul = bytes_to_uint64(rng.data()); //check it

      // BIGNUM aus Bytes
      BIGNUM* bi = BN_new();
      BN_lebin2bn(rng.data(), (int)rng.size(), bi);

      // BigInteger → LE (mit fester Länge)
      std::vector<uint8_t> bi_le(rng.size());
      BN_bn2binpad(bi, bi_le.data(), (int)rng.size());
      std::reverse(bi_le.begin(), bi_le.end());

      if (!sequence_equal(rng, bi_le))
        throw std::runtime_error(
          "test_base_converter_2_256_aot: Mismatch in 'sequence_equal(rng, bi_le)'");

      // Converter_2_256_LE_S(bi)
      auto bytes = converter_2_256_le_s(bi, 256, 10);
      if (!sequence_equal(bytes, string_to_bytes(std::to_string(ul))))
        throw std::runtime_error(
          "test_base_converter_2_256_aot: Mismatch in 'sequence_equal(bytes, ul)'");

      // bi.ToString() → base10 → bytes
      char* dec = BN_bn2dec(bi);
      std::vector<uint8_t> bytes2;
      for (char* p = dec; *p; p++)
        bytes2.push_back(static_cast<uint8_t>(*p - '0'));
      OPENSSL_free(dec);

      if (!sequence_equal(bytes, bytes2))
        throw std::runtime_error(
          "test_base_converter_2_256_aot: Mismatch in 'sequence_equal(bytes, bytes2)'");

      // ToBaseX_2_256_LE_S(bi)
      auto sbase1 = to_base_x_2_256_le_s(bi, startbase);
      auto sbase2 = to_base_x_2_256_le_s(bytes.data(), (int)bytes.size(), startbase);

      if (!sequence_equal(sbase1, sbase2))
        throw std::runtime_error("test_base_converter_2_256_aot: Mismatch in 'sequence_equal(sbase1, sbase2)'");

      // Roundtrip
      auto decipher1 = from_base_x_2_256_le_s(sbase1.data(), (int)sbase1.size(), startbase);
      if (!sequence_equal(bytes, decipher1))
        throw std::runtime_error(
          "test_base_converter_2_256_aot: Mismatch in 'sequence_equal(bytes, decipher1)'");

      // Converter_2_256_LE_S(sbase2)
      auto tbase1 = converter_2_256_le_s(sbase2.data(), (int)sbase2.size(), startbase, targetbase);
      auto tbase2 = to_base_x_2_256_le_s(bytes.data(), (int)bytes.size(), targetbase);

      if (!sequence_equal(tbase1, tbase2))
        throw std::runtime_error(
          "test_base_converter_2_256_aot: Mismatch in 'sequence_equal(tbase1, tbase2)'");

      // Rückkonvertierung
      auto rbytes1 = converter_2_256_le_s(tbase1.data(), (int)tbase1.size(), targetbase, startbase);
      if (!sequence_equal(rbytes1, sbase1))
        throw std::runtime_error(
          "test_base_converter_2_256_aot: Mismatch in 'sequence_equal(rbytes1, sbase1)'");

      if (i % (rounds / 10) == 0)
        std::cout << ".";

      BN_free(bi);
    }

    auto end = std::chrono::high_resolution_clock::now();
    double ms = std::chrono::duration<double, std::milli>(end - start).count();

    std::cout << " rounds = " << rounds
      << "; t = " << ms << "ms; td = " << (ms / rounds) << "ms\n";
  }

  static void test_base_converter_2_256_stress()
  {
    std::cout << "test_base_converter_2_256_stress_aot: ";

    using namespace michele::natale::test_service;

    const size_t sz = 1024;

    // Zufällige Start- und Zielbasis
    auto bases = rng_bases_2_256();
    int startbase = bases.first;
    int targetbase = bases.second;

    // Zufällige Zahl in startbase erzeugen
    auto bytes = rng_base_x_number(sz, startbase);

    // Zeitmessung starten
    auto start = std::chrono::high_resolution_clock::now();

    // Konvertieren: startbase → targetbase
    auto basex = converter_2_256_le_s(bytes.data(), (int)bytes.size(), startbase, targetbase);

    // Rückkonvertieren: targetbase → startbase
    auto decipher = converter_2_256_le_s(basex.data(), (int)basex.size(), targetbase, startbase);

    // Roundtrip prüfen
    if (!sequence_equal(decipher, bytes))
      throw std::runtime_error("Roundtrip mismatch in TestBaseConverter_2_256_Stress");

    // Zeitmessung stoppen
    auto end = std::chrono::high_resolution_clock::now();
    double ms = std::chrono::duration<double, std::milli>(end - start).count();

    // Ausgabe
    std::cout << "startbase = " << startbase
      << "; targetbase = " << targetbase
      << "; size = " << sz
      << "; t = " << ms << " ms\n";
  }

  static void test_base_converter_2_256_extrem_stress()
  {
    std::cout << "test_base_converter_2_256_extrem_stress_aot: ";

    using namespace michele::natale::test_service;

    const size_t sz = 20 * 1024; // 20 KB

    // Zufällige Start- und Zielbasis
    auto bases = rng_bases_2_256();
    int startbase = bases.first;
    int targetbase = bases.second;

    // Zufällige Zahl in startbase erzeugen
    auto bytes = rng_base_x_number(sz, startbase);

    // Zeitmessung starten
    auto start = std::chrono::high_resolution_clock::now();

    // Konvertieren: startbase → targetbase
    auto basex = converter_2_256_le_s(
      bytes.data(), (int)bytes.size(), startbase, targetbase);

    // Rückkonvertieren: targetbase → startbase
    auto decipher = converter_2_256_le_s(
      basex.data(), (int)basex.size(), targetbase, startbase);

    // Roundtrip prüfen
    if (!sequence_equal(decipher, bytes))
      throw std::runtime_error(
        "Roundtrip mismatch in TestBaseConverter_2_256_Extrem_Stress");

    // Zeitmessung stoppen
    auto end = std::chrono::high_resolution_clock::now();
    double ms = std::chrono::duration<double, std::milli>(end - start).count();

    // Ausgabe
    std::cout << "startbase = " << startbase
      << "; targetbase = " << targetbase
      << "; size = " << sz
      << "; t = " << ms << " ms\n\n";
  }

  void start_convert_encoding_base_x_native(int rounds)
  {
    try
    {
      test_base_converter_2_256(rounds);
      test_base_converter_2_256_stress();
      //test_base_converter_2_256_extrem_stress(); //solve in ~3/4 min
    }
    catch (const std::exception& ex)
    {
      std::cerr << "EXCEPTION: " << ex.what() << std::endl;
      throw;
    }
    catch (...)
    {
      std::cerr << "UNKNOWN EXCEPTION" << std::endl;
      throw;
    }
  }
}