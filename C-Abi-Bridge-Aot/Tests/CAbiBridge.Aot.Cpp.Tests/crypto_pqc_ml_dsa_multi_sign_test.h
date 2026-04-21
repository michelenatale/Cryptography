#pragma once

#include <iostream>


#ifndef __CRYPTO_PQC_ML_DSA_MULTI_SIGN_TEST_H__
#define __CRYPTO_PQC_ML_DSA_MULTI_SIGN_TEST_H__

namespace michele::natale::Tests
{
  void test_pqc_ml_dsa_multi_sign_kpf(int rounds);
  void test_pqc_ml_dsa_multi_sign_file_kpf(int rounds);
}

#endif