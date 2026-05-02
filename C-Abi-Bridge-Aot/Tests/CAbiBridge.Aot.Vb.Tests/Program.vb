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

      Console.WriteLine($"C-Abi-Bridge.Aot.Tests Vb.Net")
      Console.WriteLine()

      CryptoRandomTest.Start(rounds * 1000)

      CryptoAesTest.StartNative(rounds)
      CryptoAesGcmTest.StartNative(rounds)
      CryptoChaCha20Poly1305Test.StartNative(rounds)

      CryptoHashHmacTest.StartNative(rounds * 1000)
      CryptoPqcMlKemTest.StartNative(rounds)
      CryptoPqcMlDsaTest.StartNative(rounds)

      CryptoPrimesTest.StartNative(rounds)

      ConvertEncodingTest.StartNative(rounds * 1000)

      CompressesTest.StartNative(rounds)

    End Sub
  End Module
End Namespace



