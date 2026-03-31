#pragma once



namespace michele::natale::Tests
{

  void test_pqc_ml_kem_create_key_pairs(int rounds);
  void test_pqc_ml_kem_create_key_pairs_param(int rounds);
  void test_pqc_ml_kem_safe_load_key_pairs(int rounds);
  
  void test_pqc_ml_kem_capsulation_shared_key_with_public_key(int rounds);
  void test_pqc_ml_kem_sharedKey_from_capsualtion_private_key(int rounds);

  void test_pqc_ml_kem_enc_decryption_bytes(int rounds);
  void test_pqc_ml_kem_enc_decryption_bytes_stress();
}