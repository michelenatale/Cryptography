Option Strict On
Option Explicit On

Imports System.Text
Imports michele.natale
Imports michele.natale.Pointers

Namespace michele.natale.Tests
  Partial Class CryptoChaCha20Poly1305Test
    Public Shared Sub TestChaCha20Poly1305File(rounds As Int32)
      Console.Write($"{NameOf(TestChaCha20Poly1305File)}: ")

      Dim src = Encoding.UTF8.GetBytes("data")
      Dim srcr = Encoding.UTF8.GetBytes("datar")
      Dim dest = Encoding.UTF8.GetBytes("cipher")
      Dim ssrc = Encoding.UTF8.GetString(src)

      Dim sw = Stopwatch.StartNew()

      For i = 0 To rounds - 1
        Dim max = (1 << 21) + 1024
        Dim flength = Random.Shared.Next(max)
        SetRngFileData(ssrc, flength)

        Dim associated = RngBytes(Random.Shared.Next(1, 64))
        Using key = New UsIPtr(Of Byte)(
          RngBytes(NetServices.CHACHA_POLY_MAX_KEY_SIZE))

          Dim err = ChaCha20Poly1305EncryptFileAot(
          src, src.Length,
          dest, dest.Length,
          key.Ptr, key.Length,
          associated, associated.Length)
          AssertError(err)

          err = ChaCha20Poly1305DecryptFileAot(
            dest, dest.Length,
            srcr, srcr.Length,
            key.Ptr, key.Length,
            associated, associated.Length)
          AssertError(err)
        End Using

        If Not NetServices.FileEquals(src, srcr) Then
          Throw New Exception()
        End If

        If i Mod rounds / 10 = 0 Then
          Console.Write(".")
        End If

      Next

      sw.[Stop]()
      Dim t = sw.ElapsedMilliseconds
      Console.WriteLine($" rounds = {rounds}, t = {t}ms, td = {t / rounds}ms")
    End Sub
  End Class
End Namespace
