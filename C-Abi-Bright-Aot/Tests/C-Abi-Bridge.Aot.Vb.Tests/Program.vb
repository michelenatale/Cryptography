Option Strict On
Option Explicit On


Namespace michele.natale.Tests

   Public Module Program
      Public Sub Main()
         Dim rounds = 10
         Tests(rounds)
         Console.WriteLine()
         Console.WriteLine("Finish")
         Console.ReadLine()
      End Sub

    Private Sub Tests(rounds As Int32)

      CryptoRandomTest.Start(rounds * 1000)

      CryptoAesTest.StartNative(rounds)
      CryptoAesGcmTest.StartNative(rounds)
      CryptoChaCha20Poly1305Test.StartNative(rounds)
    End Sub
  End Module
End Namespace



