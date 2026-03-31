Option Strict On
Option Explicit On

Namespace michele.natale.Tests
  Partial Public NotInheritable Class CryptoPqcMlKemTest
    Public Shared Sub StartNative(rounds As Int32)
      Start_Native(rounds)
      Console.WriteLine()
    End Sub

    Private Shared Sub Start_Native(rounds As Int32)
      TestPqcMlKemCreateKeyPairs(rounds * 10)
      TestPqcMlKemCreateKeyPairsParam(rounds * 10)
      TestPqcMlKemSafeLoadKeyPairs(rounds * 10)
      TestCapsulationSharedKeyWithPublicKey(rounds * 10)
      TestPqcMlKemSharedKeyFromCapsualtionPrivateKey(rounds * 10)
      TestPqcMlKemEnDecryptionBytes(rounds)
      TestPqcMlKemEnDecryptionBytesStress()
      TestPqcMlKemEnDecryptionFile(rounds)
      TestPqcMlKemEnDecryptionKpfFile(rounds)
    End Sub
  End Class
End Namespace
