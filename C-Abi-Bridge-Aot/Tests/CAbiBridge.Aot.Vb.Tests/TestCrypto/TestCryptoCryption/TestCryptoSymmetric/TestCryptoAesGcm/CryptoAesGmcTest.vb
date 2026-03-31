Option Strict On
Option Explicit On

Namespace michele.natale.Tests
  Partial Friend NotInheritable Class CryptoAesGcmTest
    Public Shared Sub StartNative(rounds As Int32)
      TestAesGcmFile(rounds)
      TestAesGcmBytes(rounds)
      TestAesGcmBytesStress()

      Console.WriteLine()
    End Sub
  End Class
End Namespace

