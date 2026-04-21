#include "pch.h"

#include <iostream>

#include "crypto_pqc_ml_dsa_test.h"
#include "crypto_pqc_ml_dsa_file_test.h"
#include "crypto_pqc_ml_dsa_bytes_test.h"
#include "crypto_pqc_ml_dsa_multi_sign_test.h"


namespace michele::natale::Tests
{
  void start_pqc_ml_dsa_native(int rounds)
  {
    test_pqc_ml_dsa_create_key_pairs(rounds * 10);
    test_pqc_ml_dsa_create_key_pairs_param(rounds * 10);
    test_pqc_ml_dsa_safe_load_key_pairs(rounds * 10); 

    test_pqc_ml_dsa_single_signature(rounds);
    test_pqc_ml_dsa_single_signature_kpi_save_load(rounds);
            
    test_pqc_ml_dsa_single_signature_file(rounds);
    test_pqc_ml_dsa_single_signature_kpi_save_load_file(rounds);
        
    test_pqc_ml_dsa_multi_sign_kpf(rounds);
    test_pqc_ml_dsa_multi_sign_file_kpf(rounds);

  }
}