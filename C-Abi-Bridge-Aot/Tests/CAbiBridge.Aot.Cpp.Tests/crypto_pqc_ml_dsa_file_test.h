#pragma once

#include "crypto_pqc_ml_dsa_param_test.h"

//P:\Projekte\C-Abi-Bridge-Aot\Tests\CAbiBridge.Aot.Cpp.Tests\crypto_pqc_ml_dsa_file_test.h

#ifndef __CRYPTO_PQC_ML_DSA_BYTES_FILE_H__
#define __CRYPTO_PQC_ML_DSA_BYTES_FILE_H__

namespace michele::natale::Tests
{
  void test_pqc_ml_dsa_single_signature_file(int rounds);
  void test_pqc_ml_dsa_single_signature_kpi_save_load_file(int rounds);
}

#endif