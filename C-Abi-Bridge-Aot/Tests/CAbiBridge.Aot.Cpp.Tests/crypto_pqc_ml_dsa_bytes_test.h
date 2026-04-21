#pragma once

#include <iostream>

#ifndef __CRYPTO_PQC_ML_DSA_BYTES_Test_H__
#define __CRYPTO_PQC_ML_DSA_BYTES_Test_H__


namespace michele::natale::Tests
{
  void test_pqc_ml_dsa_create_key_pairs(int rounds);
  void test_pqc_ml_dsa_single_signature(int rounds);
  void test_pqc_ml_dsa_safe_load_key_pairs(int rounds);

  void test_pqc_ml_dsa_create_key_pairs_param(int rounds);
  void test_pqc_ml_dsa_single_signature_kpi_save_load(int rounds);
}


#endif