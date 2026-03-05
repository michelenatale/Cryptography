Option Strict On
Option Explicit On

Imports System.Text
Imports michele.natale
Imports michele.natale.Pointers

Namespace michele.natale.Tests
  Partial Class CryptoAesTest
    Public Shared Sub TestAesFile(rounds As Int32)
      Console.Write($"{NameOf(TestAesFile)}: ")

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
          RngBytes(NetServices.AES_KEY_SIZE))

          Dim err = AesEncryptFileAot(
          src, src.Length,
          dest, dest.Length,
          key.Ptr, key.Length,
          associated, associated.Length)
          AssertError(err)

          err = AesDecryptFileAot(
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
