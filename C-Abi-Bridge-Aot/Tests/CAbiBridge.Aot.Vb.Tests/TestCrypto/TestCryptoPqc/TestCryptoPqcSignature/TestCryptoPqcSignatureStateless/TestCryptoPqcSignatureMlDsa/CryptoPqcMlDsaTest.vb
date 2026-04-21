Option Strict On
Option Explicit On


Namespace michele.natale.Tests
  Partial Public Module CryptoPqcMlDsaTest

    Public Sub StartNative(rounds As Int32)
      Start_Native(rounds)

      Console.WriteLine()
    End Sub

    Private Sub Start_Native(rounds As Int32)

      TestPqcMlDsaCreateKeyPairs(rounds * 10)
      TestPqcMlDsaCreateKeyPairsParam(rounds * 10)
      TestPqcMlDsaSaveLoadKeyPairs(rounds * 10)

      TestPqcMlDsaSingleSignature(rounds)
      TestPqcMlDsaSingleSignatureKpiSaveLoad(rounds)

      TestPqcMlDsaSingleSignatureFile(rounds)
      TestPqcMlDsaSingleSignatureKpiSaveLoadFile(rounds)

      TestPqcMlDsaMultiSignKpf(rounds) 'kpf = key-pair-file
      TestPqcMlDsaMultiSignFileKpf(rounds) 'kpf = key-pair-file

    End Sub

  End Module
End Namespace