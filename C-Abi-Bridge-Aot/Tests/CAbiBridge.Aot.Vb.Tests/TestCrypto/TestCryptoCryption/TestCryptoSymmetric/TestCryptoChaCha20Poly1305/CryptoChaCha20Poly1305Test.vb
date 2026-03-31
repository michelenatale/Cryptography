Option Strict On
Option Explicit On

Namespace michele.natale.Tests
  Partial Friend NotInheritable Class CryptoChaCha20Poly1305Test
    Public Shared Sub StartNative(rounds As Int32)
      TestChaCha20Poly1305File(rounds)
      TestChaCha20Poly1305Bytes(rounds)
      TestChaCha20Poly1305BytesStress()
      Console.WriteLine()
    End Sub
  End Class
End Namespace

