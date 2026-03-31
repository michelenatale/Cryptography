Option Strict On
Option Explicit On
Imports System.Text

Namespace michele.natale.Tests
  Friend Class ConvertEncodingTest
    Public Shared Sub StartNative(rounds As Integer)
      TestBase64(rounds)
      TestHex(rounds)
    End Sub

    Private Shared Sub TestBase64(rounds As Integer)
      Console.Write($"{NameOf(TestBase64)}Aot: ")

      Dim rand = Random.[Shared]
      Dim sw = Stopwatch.StartNew()
      Dim plain_ptr As IntPtr = Nothing, plain_length As Integer = Nothing
      For i = 0 To rounds - 1
        Dim size = rand.[Next](1, 128)
        Dim bytes = RngBytes(size)

        Dim err = ToBase64Aot(bytes, bytes.Length, plain_ptr, plain_length)
        AssertError(err)

        Dim data = ToBytes(plain_ptr, plain_length)
        FreeBuffer(plain_ptr)

        Dim b64 = Encoding.UTF8.GetBytes(Convert.ToBase64String(bytes))
        If plain_length <> b64.Length Then Throw New Exception()

        If Not b64.SequenceEqual(data) Then Throw New Exception()

        plain_ptr = IntPtr.Zero
        err = FromBase64Aot(data, data.Length, plain_ptr, plain_length)
        AssertError(err)

        data = ToBytes(plain_ptr, plain_length)
        FreeBuffer(plain_ptr)

        Dim b64r = Convert.FromBase64String(Encoding.UTF8.GetString(b64))

        If plain_length <> b64r.Length Then
          Throw New Exception()
        End If

        If Not b64r.SequenceEqual(data) Then
          Throw New Exception()
        End If

        If i Mod rounds / 10 = 0 Then
          Console.Write(".")
        End If
      Next

      sw.[Stop]()

      Dim t = CDbl(sw.ElapsedMilliseconds)
      Console.Write($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms")
      Console.WriteLine()
    End Sub

    Private Shared Sub TestHex(rounds As Integer)
      Console.Write($"{NameOf(TestHex)}Aot: ")

      Dim rand = Random.[Shared]
      Dim sw = Stopwatch.StartNew()
      Dim hex_ptr As IntPtr = Nothing, hex_length As Integer = Nothing
      For i = 0 To rounds - 1
        Dim size = rand.[Next](1, 128)
        Dim bytes = RngBytes(size)

        Dim lower = Integer.IsEvenInteger(rand.[Next]())
        Dim err = ToHexAot(bytes, bytes.Length, hex_ptr, hex_length, lower)
        AssertError(err)

        Dim data = ToBytes(hex_ptr, hex_length)
        FreeBuffer(hex_ptr)

        Dim hex As Byte()
        If lower Then
          hex = Encoding.UTF8.GetBytes(Convert.ToHexStringLower(bytes))
        Else hex = Encoding.UTF8.GetBytes(Convert.ToHexString(bytes))
        End If

        If Not hex_length = hex.Length Then
          Throw New Exception()
        End If
        If Not hex.SequenceEqual(data) Then
          Throw New Exception()
        End If

        err = FromHexAot(hex, hex.Length, hex_ptr, hex_length)
        AssertError(err)

        data = ToBytes(hex_ptr, hex_length)
        FreeBuffer(hex_ptr)

        Dim hexr = Convert.FromHexString(Encoding.UTF8.GetString(hex))
        If hex_length <> hexr.Length Then Throw New Exception()

        If Not hexr.SequenceEqual(data) Then
          Throw New Exception()
        End If

        If i Mod rounds / 10 = 0 Then
          Console.Write(".")
        End If
      Next

      sw.[Stop]()

      Dim t = CDbl(sw.ElapsedMilliseconds)
      Console.Write($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms")
      Console.WriteLine()
    End Sub

  End Class
End Namespace