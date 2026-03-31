#pragma once


namespace michele::natale::Cpp::Services
{
  //General
  const int MIN_PW_SIZE = 10;
  const int MAX_PW_SIZE = 256;

  const int MIN_SALT_SIZE = 16;
  const int MAX_SALT_SIZE = 128;

  const int MIN_ITERATION = 1234;
  const int MAX_ITERATION = 1 << 16;


  //AES
  const int AES_IV_SIZE = 16;
  const int AES_TAG_SIZE = 16;
  const int AES_KEY_SIZE = 32;
  const int AES_MIN_PLAIN_SIZE = 8;
  const int AES_MAX_PLAIN_SIZE = 1024 * 1024;


  //AES GCM
  const int AES_GCM_TAG_SIZE = 16;
  const int AES_GCM_NONCE_SIZE = 12;
  const int AES_GCM_MIN_KEY_SIZE = 16;
  const int AES_GCM_MID_KEY_SIZE = 24;
  const int AES_GCM_MAX_KEY_SIZE = 32;
  const int AES_GCM_MIN_PLAIN_SIZE = 8;
  const int AES_GCM_MAX_PLAIN_SIZE = 1024 * 1024;


  //ChaCha20Poly1305
  const int CHACHA_POLY_TAG_SIZE = 16;
  const int CHACHA_POLY_NONCE_SIZE = 12;
  const int CHACHA_POLY_MIN_KEY_SIZE = 16;
  const int CHACHA_POLY_MID_KEY_SIZE = 24;
  const int CHACHA_POLY_MAX_KEY_SIZE = 32;
  const int CHACHA_POLY_MIN_PLAIN_SIZE = 8;
  const int CHACHA_POLY_MAX_PLAIN_SIZE = 1024 * 1024;

  //Pqc ML-KEM
  const int PQC_ML_KEM_MIN_PLAIN_SIZE = 8;
  const int PQC_ML_KEM_MAX_PLAIN_SIZE = 1024 * 1024;
}