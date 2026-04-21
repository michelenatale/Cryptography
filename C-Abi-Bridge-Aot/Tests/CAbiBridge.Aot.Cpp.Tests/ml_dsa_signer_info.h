#pragma once

#include <stdint.h>
#include <stddef.h>
#include <vector>

namespace michele::natale::Tests
{
  struct ml_dsa_signer_info
  {
    std::vector< uint8_t> signer_id;

    std::vector< uint8_t> signature;

    uint8_t pqc_sign_algo = 1; //mldsa

    uint8_t pqc_sign_algo_param = 255; //Invalid

    std::vector< uint8_t> public_key;

    std::vector< uint8_t> signer_name;

    std::vector< uint8_t> project_name;
  };

}