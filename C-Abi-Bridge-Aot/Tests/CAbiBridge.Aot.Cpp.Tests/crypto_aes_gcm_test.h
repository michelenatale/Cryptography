#pragma once

#include "cabi_exp_imp.h"
#include "crypto_utils_test.h"



#ifndef __CRYPTO_AES_GCM_TEST_H__
#define __CRYPTO_AES_GCM_TEST_H__

namespace michele::natale::Tests 
{
  void start_aes_gcm_native(int rounds);
    
  void test_aes_gcm_file(int rounds);
  void test_aes_gcm_bytes(int rounds);
  void test_aes_gcm_bytes_stress();
}

#endif