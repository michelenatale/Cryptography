#include "pch.h"

#include "crypto_pqc_ml_kem_test.h"
#include "crypto_pqc_ml_kem_file_test.h"
#include "crypto_pqc_ml_kem_bytes_test.h"


//Start
//Alice(Keypair)
//    >> Bob(Encapsulation)
//        >> Alice(Encryption)
//            >> Bob(Decryption)
//Finish


namespace michele::natale::Tests
{
  void start_pqc_ml_kem_native(int rounds)
  {
    test_pqc_ml_kem_create_key_pairs(rounds * 10);
    test_pqc_ml_kem_create_key_pairs_param(rounds * 10);
    test_pqc_ml_kem_safe_load_key_pairs(rounds * 10);

    test_pqc_ml_kem_capsulation_shared_key_with_public_key(rounds * 10);
    test_pqc_ml_kem_sharedKey_from_capsualtion_private_key(rounds * 10);

    test_pqc_ml_kem_enc_decryption_bytes(rounds);
    test_pqc_ml_kem_enc_decryption_bytes_stress();     

    test_pqc_ml_kem_enc_decryption_file(rounds);
    test_pqc_ml_kem_enc_decryption_kpf_file(rounds);
  }
}