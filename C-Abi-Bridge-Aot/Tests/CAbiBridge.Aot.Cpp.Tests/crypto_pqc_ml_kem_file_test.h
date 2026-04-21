#pragma once


#ifndef __CRYPTO_PQC_ML_KEM_FILE_TEST_H__
#define __CRYPTO_PQC_ML_KEM_FILE_TEST_H__

namespace michele::natale::Tests
{
  void test_pqc_ml_kem_enc_decryption_file(int rounds);
  void test_pqc_ml_kem_enc_decryption_kpf_file(int rounds);
}


#endif