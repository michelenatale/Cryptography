Option Strict On
Option Explicit On

Imports System.Text
Imports System.Numerics
Imports michele.natale.Tests

Namespace michele.natale.Tests
  Friend Class ConvertEncodingTest
    Public Shared Sub StartNative(rounds As Int32)
      TestBase64(rounds)
      TestHex(rounds)
      TestBaseX(rounds)
    End Sub

    Private Shared Sub TestBaseX(rounds As Int32)
      TestBaseConverter_2_256(rounds)
      TestBaseConverter_2_256_Stress()
      'TestBaseConverter_2_256_Extrem_Stress() 'solve in ~3/4 min.
    End Sub

    Private Shared Sub TestBase64(rounds As Int32)
      Console.Write($"{NameOf(TestBase64)}Aot: ")

      Dim rand = Random.[Shared]
      Dim sw = Stopwatch.StartNew()
      Dim plain_ptr As IntPtr = Nothing, plain_length As Int32 = Nothing
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

    Private Shared Sub TestHex(rounds As Int32)
      Console.Write($"{NameOf(TestHex)}Aot: ")

      Dim rand = Random.[Shared]
      Dim sw = Stopwatch.StartNew()
      Dim hex_ptr As IntPtr = Nothing, hex_length As Int32 = Nothing
      For i = 0 To rounds - 1
        Dim size = rand.[Next](1, 128)
        Dim bytes = RngBytes(size)

        Dim lower = Int32.IsEvenInteger(rand.[Next]())
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
      Console.WriteLine() : Console.WriteLine()
    End Sub

    Private Shared Sub TestBaseConverter_2_256(rounds As Int32)
      Console.Write($"{NameOf(TestBaseConverter_2_256)}Aot: ")

      Dim rand = Random.[Shared]
      Dim sw = Stopwatch.StartNew()
      Dim st_bases As (startbase As Int32, targetbase As Int32) = Nothing

      For i = 0 To rounds - 1
        st_bases = TestServicesVb.RngBases_2_256()

        Dim rng = TestServices.RngBytes(8, True) 'uint64, int64

        Dim bi = New BigInteger(rng, True, False) 'base 10
        Dim bibytes = TestServicesVb.TrimFirst(bi.ToByteArray(True, False)) 'base 256

        'Notes: For this to work, the byte array of the
        'BigInteger must be entered into the converter
        'as a little-endian.
        Dim bytes = TestServices.Converter_2_256_LE_S(bi, 256, 10)

        Dim bytes2 = bi.ToString().[Select](Function(x) Convert.ToByte(AscW(x) - 48)).ToArray() 'base 10
        If Not bytes.SequenceEqual(bytes2) Then Throw New Exception()

        Dim sbase1 = TestServicesVb.ToBaseX_2_256_LE_S(bi, st_bases.startbase)
        Dim sbase2 = TestServicesVb.ToBaseX_2_256_LE_S(bytes, st_bases.startbase)
        If Not sbase1.SequenceEqual(sbase2) Then Throw New Exception()

        Dim decipher1 = TestServicesVb.FromBaseX_2_256_LE_S(sbase1, st_bases.startbase)
        If Not bytes.SequenceEqual(decipher1) Then Throw New Exception()

        Dim tbase1 = TestServicesVb.Converter_2_256_LE_S(sbase2, st_bases.startbase, st_bases.targetbase)
        Dim tbase2 = TestServicesVb.ToBaseX_2_256_LE_S(bytes, st_bases.targetbase)
        If Not tbase1.SequenceEqual(tbase2) Then Throw New Exception()

        Dim rbytes1 = TestServicesVb.Converter_2_256_LE_S(tbase1, st_bases.targetbase, st_bases.startbase)
        If Not rbytes1.SequenceEqual(sbase1) Then Throw New Exception()

        If i Mod rounds / 10 = 0 Then Console.Write(".")
      Next

      sw.[Stop]()

      Dim t = CDbl(sw.ElapsedMilliseconds)
      Console.Write($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms")
      Console.WriteLine()
    End Sub

    Private Shared Sub TestBaseConverter_2_256_Stress()
      Console.Write($"{NameOf(TestBaseConverter_2_256_Stress)}Aot: ")

      Dim sz = 1024
      Dim rand = Random.[Shared]
      Dim st_bases As (startbase As Int32, targetbase As Int32) = Nothing

      st_bases = TestServices.RngBases_2_256()
      Dim bytes = TestServices.RngBaseXNumber(sz, st_bases.startbase)

      Dim sw = Stopwatch.StartNew()
      Dim basex = TestServicesVb.Converter_2_256_LE_S(bytes, st_bases.startbase, st_bases.targetbase)
      Dim decipher = TestServicesVb.Converter_2_256_LE_S(basex, st_bases.targetbase, st_bases.startbase)

      If Not decipher.SequenceEqual(bytes) Then
        Throw New Exception()
      End If

      sw.[Stop]()
      Console.WriteLine($"startbase = {st_bases.startbase}; targetbase = {st_bases.targetbase}; size = {sz}; t = {sw.ElapsedMilliseconds} ms")
    End Sub

    Private Shared Sub TestBaseConverter_2_256_Extrem_Stress()
      Console.Write($"{NameOf(TestBaseConverter_2_256_Extrem_Stress)}Aot: ")

      Dim sz = 20 * 1024 '20 KB
      Dim rand = Random.[Shared]
      Dim st_bases As (startbase As Int32, targetbase As Int32) = Nothing

      st_bases = TestServices.RngBases_2_256()
      Dim bytes = TestServices.RngBaseXNumber(sz, st_bases.startbase)

      Dim sw = Stopwatch.StartNew()
      Dim basex = TestServices.Converter_2_256_LE_S(bytes, st_bases.startbase, st_bases.targetbase)
      Dim decipher = TestServices.Converter_2_256_LE_S(basex, st_bases.targetbase, st_bases.startbase)

      If Not decipher.SequenceEqual(bytes) Then Throw New Exception()

      sw.[Stop]()
      Console.WriteLine($"startbase = {st_bases.startbase}; targetbase = {st_bases.targetbase}; size = {sz}; t = {sw.ElapsedMilliseconds} ms")
      Console.WriteLine()
    End Sub
  End Class
End Namespace