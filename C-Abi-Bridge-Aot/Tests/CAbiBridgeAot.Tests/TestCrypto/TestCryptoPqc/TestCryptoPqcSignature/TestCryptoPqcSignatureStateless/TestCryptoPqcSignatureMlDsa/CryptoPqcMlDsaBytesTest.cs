

using System.Numerics;


//Start
//Alice(Keypair)
//    >> Bob(Verify PublicKey)
//        >> Alice(Sign)
//            >> Bob(Verify)
//Finish


namespace michele.natale.Tests;

partial class CryptoPqcMlDsaTest
{
  //KeyGen
  //Sign / Verify 


  private static bool IsNullOrEmpty<T>(ReadOnlySpan<T> bytes)
  where T : INumber<T> =>
    bytes.IsEmpty || bytes.Length == 0;

  private static bool IsNullOrEmpty<T>(ReadOnlyMemory<T> bytes)
  where T : INumber<T> =>
    bytes.IsEmpty || bytes.Length == 0;

}
