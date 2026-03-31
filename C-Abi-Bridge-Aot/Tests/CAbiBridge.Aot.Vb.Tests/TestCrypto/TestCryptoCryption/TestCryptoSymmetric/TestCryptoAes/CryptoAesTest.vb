Option Strict On
Option Explicit On

Namespace michele.natale.Tests
  Partial Friend NotInheritable Class CryptoAesTest
    Public Shared Sub StartNative(rounds As Int32)
      TestAesFile(rounds)
      TestAesBytes(rounds)
      TestAesBytesStress()
      Console.WriteLine()
    End Sub
  End Class
End Namespace

