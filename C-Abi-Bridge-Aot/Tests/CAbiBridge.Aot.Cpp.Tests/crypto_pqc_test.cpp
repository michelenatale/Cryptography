#include "pch.h"


#include "crypto_pqc_test.h"
#include "crypto_pqc_ml_kem_test.h"
#include "crypto_pqc_ml_dsa_test.h"
//#include "crypto_pqc_slh_dsa_test.h"


namespace michele::natale::Tests
{
  void start_pqc_native(int rounds)
  {
    start_pqc_ml_kem_native(rounds);
    start_pqc_ml_dsa_native(rounds);

    //start_pqc_slh_dsa_native(rounds);
  }
}