Option Strict On
Option Explicit On



Imports michele.natale.CAbiBridge

Namespace michele.natale.Tests

  Partial Class CryptoRandomTest

    Public Shared Sub Start(rounds As Int32)
      TestRngCryptoBoolAot(rounds)
      TestRngCryptoBytesAot(rounds)

      TestNextCryptoInt32Aot(rounds)
      TestRngCryptoInt32Aot(rounds)

      TestNextCryptoInt64Aot(rounds)
      TestRngCryptoInt64Aot(rounds)

      TestNextCryptoDoubleAot(rounds)
      TestRngCryptoDoubleAot(rounds)

      TestNextCryptoSingleAot(rounds)
      TestRngCryptoSingleAot(rounds)

      TestNextCryptoDecimalAot(rounds)
      TestRngCryptoDecimalAot(rounds)
    End Sub

    Private Shared Sub TestRngCryptoBytesAot(rounds As Int32)
      Console.Write($"{NameOf(TestRngCryptoBytesAot)}: ")

      Dim rand = Random.[Shared]
      Dim sw = Stopwatch.StartNew()
      Dim out_ptr As IntPtr = Nothing
      For i = 0 To rounds - 1

        Dim size = rand.[Next](128, 512)
        Dim err = Native.RngCryptoBytesAot(size, out_ptr)
        AssertError(err)

        Dim bytes = ToBytes(out_ptr, size)
        Native.FreeBuffer(out_ptr)

        If bytes Is Nothing OrElse Not bytes.Length = size Then
          Throw New Exception()
        End If

        If i Mod rounds / 10 = 0 Then
          Console.Write(".")
        End If
      Next

      sw.Stop()
      Dim t = sw.ElapsedMilliseconds
      Console.WriteLine($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms")


      Console.Write($"Test{NameOf(FillCryptoBytesAot)}: ")

      sw = Stopwatch.StartNew()
      For i = 0 To rounds - 1
        Dim size = rand.[Next](128, 512)
        Dim bytes = New Byte(size - 1) {}
        Dim err = Native.FillCryptoBytesAot(bytes, bytes.Length)
        AssertError(err)

        If bytes Is Nothing OrElse bytes.Length <> size Then Throw New Exception()

        If bytes.Max(Function(x) x) <= 0 Then Throw New Exception()

        If i Mod rounds / 10 = 0 Then Console.Write(".")
      Next

      sw.Stop()
      t = sw.ElapsedMilliseconds
      Console.WriteLine($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms")
      Console.WriteLine()
    End Sub

    Private Shared Sub TestRngCryptoBoolAot(rounds As Int32)
      Console.Write($"{NameOf(TestRngCryptoBoolAot)}: ")

      Dim err As CError
      Dim rand = Random.[Shared]
      Dim sw = Stopwatch.StartNew()
      Dim out_ptr As IntPtr = Nothing
      For i = 0 To rounds - 1
        Dim size = rand.[Next](128, 512)
        err = Native.RngCryptoBoolAot(size, out_ptr)
        AssertError(err)

        Dim bools = ToBools(out_ptr, size)
        Native.FreeBuffer(out_ptr)

        If bools Is Nothing OrElse Not bools.Length = size Then
          Throw New Exception()
        End If

        If i Mod rounds / 10 = 0 Then
          Console.Write(".")
        End If

      Next

      sw.Stop()
      Dim t = sw.ElapsedMilliseconds
      Console.WriteLine($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms")

      Console.Write($"Test{NameOf(NextCryptoBoolAot)}: ")

      sw = Stopwatch.StartNew()
      err = Nothing
      For i = 0 To rounds - 1
        Dim bol = Native.NextCryptoBoolAot(err)
        AssertError(err)

        If i Mod rounds / 10 = 0 Then Console.Write(".")
      Next

      sw.Stop()
      t = sw.ElapsedMilliseconds
      Console.WriteLine($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms")
      Console.WriteLine()
    End Sub

    Private Shared Sub TestNextCryptoInt32Aot(rounds As Int32)
      Console.Write($"{NameOf(TestNextCryptoInt32Aot)}: ")

      Dim err As CError
      Dim rand = Random.[Shared]
      Dim sw = Stopwatch.StartNew()
      For i = 0 To rounds - 1
        Dim result = Native.NextCryptoInt32Aot(err)
        AssertError(err)

        If result < 0 Then
          Throw New Exception()
        End If

        If i Mod rounds / 10 = 0 Then
          Console.Write(".")
        End If
      Next

      sw.Stop()
      Dim t = sw.ElapsedMilliseconds
      Console.WriteLine($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms")

      Console.Write($"Test{NameOf(NextCryptoInt32MaxAot)}: ")

      sw = Stopwatch.StartNew()

      'hier wird vielfach ein minuswert geliefert
      err = Nothing
      For i = 0 To rounds - 1
        Dim max = rand.[Next]()
        Dim result = Native.NextCryptoInt32MaxAot(max, err)
        AssertError(err)

        If result < 0 OrElse result > max Then Throw New Exception()

        If i Mod rounds / 10 = 0 Then Console.Write(".")
      Next

      sw.Stop()
      t = sw.ElapsedMilliseconds
      Console.WriteLine($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms")

      Console.Write($"Test{NameOf(NextCryptoInt32MinMaxAot)}: ")

      sw = Stopwatch.StartNew()
      err = Nothing
      For i = 0 To rounds - 1
        Dim min As Int32 = CInt(Int32.MaxValue / 3), max = 2 * min
        Dim result = Native.NextCryptoInt32MinMaxAot(min, max, err)
        AssertError(err)

        If result < min OrElse result > max Then Throw New Exception()

        If i Mod rounds / 10 = 0 Then Console.Write(".")
      Next

      sw.Stop()
      t = sw.ElapsedMilliseconds
      Console.WriteLine($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms")
      Console.WriteLine()
    End Sub

    Private Shared Sub TestRngCryptoInt32Aot(rounds As Int32)
      Console.Write($"{NameOf(TestRngCryptoInt32Aot)}: ")

      Dim rand = Random.[Shared]
      Dim sw = Stopwatch.StartNew()
      Dim out_ptr As IntPtr = Nothing
      For i = 0 To rounds - 1
        Dim size = rand.[Next](128, 512)
        Dim err = Native.RngCryptoInt32Aot(size, out_ptr)
        AssertError(err)

        Dim result = ToInts(Of Int32)(out_ptr, size)
        Native.FreeBuffer(out_ptr)

        If result Is Nothing OrElse Not result.Length = size Then
          Throw New Exception()
        End If

        If i Mod rounds / 10 = 0 Then
          Console.Write(".")
        End If

      Next

      sw.Stop()
      Dim t = sw.ElapsedMilliseconds
      Console.WriteLine($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms")

      Console.Write($"Test{NameOf(RngCryptoInt32MaxAot)}: ")

      sw = Stopwatch.StartNew()
      out_ptr = Nothing
      For i = 0 To rounds - 1
        Dim max = rand.[Next]()
        Dim size = rand.[Next](128, 512)
        Dim err = Native.RngCryptoInt32MaxAot(size, max, out_ptr)
        AssertError(err)

        Dim result = ToInts(Of Int32)(out_ptr, size)
        Native.FreeBuffer(out_ptr)

        If result Is Nothing OrElse Not result.Length = size Then
          Throw New Exception()
        End If

        If result.Min(Function(x) x) < 0 OrElse result.Max(Function(x) x) > max Then
          Throw New Exception()
        End If

        If i Mod rounds / 10 = 0 Then
          Console.Write(".")
        End If
      Next

      sw.Stop()
      t = sw.ElapsedMilliseconds
      Console.WriteLine($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms")


      Console.Write($"Test{NameOf(RngCryptoInt32MinMaxAot)}: ")

      sw = Stopwatch.StartNew()
      out_ptr = Nothing
      For i = 0 To rounds - 1
        Dim size = rand.[Next](128, 512)
        Dim min As Int32 = CInt(Int32.MaxValue / 3), max = min * 2
        Dim err = Native.RngCryptoInt32MinMaxAot(size, min, max, out_ptr)
        AssertError(err)

        Dim result = ToInts(Of Int32)(out_ptr, size)
        Native.FreeBuffer(out_ptr)

        If result Is Nothing OrElse Not result.Length = size Then
          Throw New Exception()
        End If

        If result.Min(Function(x) x) < min OrElse result.Max(Function(x) x) > max Then
          Throw New Exception()
        End If

        If i Mod rounds / 10 = 0 Then
          Console.Write(".")
        End If
      Next

      sw.Stop()
      t = sw.ElapsedMilliseconds
      Console.WriteLine($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms")
      Console.WriteLine()
    End Sub

    Private Shared Sub TestNextCryptoInt64Aot(rounds As Int32)
      Console.Write($"{NameOf(TestNextCryptoInt64Aot)}: ")

      Dim rand = Random.[Shared]
      Dim sw = Stopwatch.StartNew()
      Dim err As CError = Nothing
      For i = 0 To rounds - 1
        Dim result = Native.NextCryptoInt64Aot(err)
        AssertError(err)

        If result < 0 Then
          Throw New Exception()
        End If

        If i Mod rounds / 10 = 0 Then
          Console.Write(".")
        End If
      Next

      sw.Stop()
      Dim t = sw.ElapsedMilliseconds
      Console.WriteLine($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms")

      Console.Write($"Test{NameOf(NextCryptoInt64MaxAot)}: ")

      sw = Stopwatch.StartNew()
      err = Nothing
      For i = 0 To rounds - 1
        Dim max = rand.NextInt64()
        Dim result = Native.NextCryptoInt64MaxAot(max, err)
        AssertError(err)

        If result < 0 OrElse result > max Then Throw New Exception()

        If i Mod rounds / 10 = 0 Then Console.Write(".")
      Next

      sw.Stop()
      t = sw.ElapsedMilliseconds
      Console.WriteLine($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms")

      Console.Write($"Test{NameOf(NextCryptoInt64MinMaxAot)}: ")

      sw = Stopwatch.StartNew()
      err = Nothing
      For i = 0 To rounds - 1
        Dim min As Int64 = CLng(Int64.MaxValue / 3), max = 2 * min
        Dim result = Native.NextCryptoInt64MinMaxAot(min, max, err)
        AssertError(err)

        If result < min OrElse result > max Then
          Throw New Exception()
        End If

        If i Mod rounds / 10 = 0 Then
          Console.Write(".")
        End If
      Next

      sw.Stop()
      t = sw.ElapsedMilliseconds
      Console.WriteLine($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms")
      Console.WriteLine()
    End Sub

    Private Shared Sub TestRngCryptoInt64Aot(rounds As Int32)
      Console.Write($"{NameOf(TestRngCryptoInt64Aot)}: ")

      Dim rand = Random.[Shared]
      Dim sw = Stopwatch.StartNew()
      Dim out_ptr As IntPtr = Nothing
      For i = 0 To rounds - 1
        Dim size = rand.[Next](128, 512)
        Dim err = Native.RngCryptoInt64Aot(size, out_ptr)
        AssertError(err)

        Dim result = ToInts(Of Int64)(out_ptr, size)
        Native.FreeBuffer(out_ptr)

        If result Is Nothing OrElse Not result.Length = size Then
          Throw New Exception()
        End If

        If i Mod rounds / 10 = 0 Then
          Console.Write(".")
        End If
      Next

      sw.Stop()
      Dim t = sw.ElapsedMilliseconds
      Console.WriteLine($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms")

      Console.Write($"Test{NameOf(RngCryptoInt64MaxAot)}: ")

      sw = Stopwatch.StartNew()
      out_ptr = Nothing
      For i = 0 To rounds - 1
        Dim max = rand.[Next]()
        Dim size = rand.[Next](128, 512)
        Dim err = Native.RngCryptoInt64MaxAot(size, max, out_ptr)
        AssertError(err)

        Dim result = ToInts(Of Int64)(out_ptr, size)
        Native.FreeBuffer(out_ptr)

        If result Is Nothing OrElse Not result.Length = size Then Throw New Exception()

        If result.Min(Function(x) x) < 0 OrElse result.Max(Function(x) x) > max Then
          Throw New Exception()
        End If

        If i Mod rounds / 10 = 0 Then
          Console.Write(".")
        End If
      Next

      sw.Stop()
      t = sw.ElapsedMilliseconds
      Console.WriteLine($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms")


      Console.Write($"Test{NameOf(RngCryptoInt64MinMaxAot)}: ")

      sw = Stopwatch.StartNew()
      out_ptr = Nothing
      For i = 0 To rounds - 1
        Dim size = rand.[Next](128, 512)
        Dim min As Int64 = CLng(Int64.MaxValue / 3), max = min * 2
        Dim err = Native.RngCryptoInt64MinMaxAot(size, min, max, out_ptr)
        AssertError(err)

        Dim result = ToInts(Of Int64)(out_ptr, size)
        Native.FreeBuffer(out_ptr)

        If result Is Nothing OrElse Not result.Length = size Then
          Throw New Exception()
        End If

        If result.Min(Function(x) x) < min OrElse result.Max(Function(x) x) > max Then
          Throw New Exception()
        End If

        If i Mod rounds / 10 = 0 Then
          Console.Write(".")
        End If
      Next

      sw.Stop()
      t = sw.ElapsedMilliseconds
      Console.WriteLine($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms")
      Console.WriteLine()
    End Sub

    Private Shared Sub TestNextCryptoDoubleAot(rounds As Int32)
      Console.Write($"{NameOf(TestNextCryptoDoubleAot)}: ")

      Dim rand = Random.[Shared]
      Dim sw = Stopwatch.StartNew()
      Dim err As CError = Nothing

      For i = 0 To rounds - 1
        Dim result = Native.NextCryptoDoubleAot(err)
        AssertError(err)

        If result < 0 Then
          Throw New Exception()
        End If

        If i Mod rounds / 10 = 0 Then
          Console.Write(".")
        End If
      Next

      sw.Stop()
      Dim t = sw.ElapsedMilliseconds
      Console.WriteLine($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms")

      Console.Write($"Test{NameOf(NextCryptoDoubleMaxAot)}: ")

      sw = Stopwatch.StartNew()
      err = Nothing
      For i = 0 To rounds - 1
        Dim max = rand.NextInt64()
        Dim result = Native.NextCryptoDoubleMaxAot(max, err)
        AssertError(err)

        If result < 0 OrElse result > max Then
          Throw New Exception()
        End If

        If i Mod rounds / 10 = 0 Then
          Console.Write(".")
        End If
      Next

      sw.Stop()
      t = sw.ElapsedMilliseconds
      Console.WriteLine($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms")

      Console.Write($"Test{NameOf(NextCryptoDoubleMinMaxAot)}: ")

      sw = Stopwatch.StartNew()
      err = Nothing

      For i = 0 To rounds - 1
        Dim min = Double.MaxValue / 3, max = 2 * min
        Dim result = Native.NextCryptoDoubleMinMaxAot(min, max, err)
        AssertError(err)

        If result < min OrElse result > max Then
          Throw New Exception()
        End If

        If i Mod rounds / 10 = 0 Then
          Console.Write(".")
        End If
      Next

      sw.Stop()
      t = sw.ElapsedMilliseconds
      Console.WriteLine($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms")
      Console.WriteLine()
    End Sub

    Private Shared Sub TestRngCryptoDoubleAot(rounds As Int32)
      Console.Write($"{NameOf(TestRngCryptoDoubleAot)}: ")

      Dim rand = Random.[Shared]
      Dim sw = Stopwatch.StartNew()
      Dim out_ptr As IntPtr = Nothing
      For i = 0 To rounds - 1
        Dim size = rand.[Next](128, 512)
        Dim err = Native.RngCryptoDoubleAot(size, out_ptr)
        AssertError(err)

        Dim result = ToFloats(Of Double)(out_ptr, size)
        Native.FreeBuffer(out_ptr)

        If result Is Nothing OrElse Not result.Length = size Then
          Throw New Exception()
        End If

        If i Mod rounds / 10 = 0 Then
          Console.Write(".")
        End If
      Next

      sw.Stop()
      Dim t = sw.ElapsedMilliseconds
      Console.WriteLine($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms")

      Console.Write($"Test{NameOf(RngCryptoDoubleMaxAot)}: ")

      sw = Stopwatch.StartNew()
      out_ptr = Nothing
      For i = 0 To rounds - 1
        Dim max = rand.NextInt64()
        Dim size = rand.[Next](128, 512)
        Dim err = Native.RngCryptoDoubleMaxAot(size, max, out_ptr)
        AssertError(err)

        Dim result = ToFloats(Of Double)(out_ptr, size)
        Native.FreeBuffer(out_ptr)

        If result Is Nothing OrElse Not result.Length = size Then
          Throw New Exception()
        End If

        If result.Min(Function(x) x) < 0.0 OrElse result.Max(Function(x) x) > max Then
          Throw New Exception()
        End If

        If i Mod rounds / 10 = 0 Then
          Console.Write(".")
        End If
      Next

      sw.Stop()
      t = sw.ElapsedMilliseconds
      Console.WriteLine($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms")


      Console.Write($"Test{NameOf(RngCryptoDoubleMinMaxAot)}: ")

      sw = Stopwatch.StartNew()
      out_ptr = Nothing
      For i = 0 To rounds - 1
        Dim size = rand.[Next](128, 512)
        Dim min As Int64 = CLng(Int64.MaxValue / 3), max = min * 2
        Dim err = Native.RngCryptoDoubleMinMaxAot(size, min, max, out_ptr)
        AssertError(err)

        Dim result = ToFloats(Of Double)(out_ptr, size)
        Native.FreeBuffer(out_ptr)

        If result Is Nothing OrElse Not result.Length = size Then
          Throw New Exception()
        End If

        If result.Min(Function(x) x) < min OrElse result.Max(Function(x) x) > max Then
          Throw New Exception()
        End If

        If i Mod rounds / 10 = 0 Then
          Console.Write(".")
        End If
      Next

      sw.Stop()
      t = sw.ElapsedMilliseconds
      Console.WriteLine($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms")
      Console.WriteLine()
    End Sub

    Private Shared Sub TestNextCryptoSingleAot(rounds As Int32)
      Console.Write($"{NameOf(TestNextCryptoSingleAot)}: ")

      Dim rand = Random.[Shared]
      Dim sw = Stopwatch.StartNew()
      Dim err As CError = Nothing
      For i = 0 To rounds - 1
        Dim result = Native.NextCryptoSingleAot(err)
        AssertError(err)

        If result < 0 Then
          Throw New Exception()
        End If

        If i Mod rounds / 10 = 0 Then
          Console.Write(".")
        End If
      Next

      sw.Stop()
      Dim t = sw.ElapsedMilliseconds
      Console.WriteLine($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms")

      Console.Write($"Test{NameOf(NextCryptoSingleMaxAot)}: ")

      sw = Stopwatch.StartNew()
      err = Nothing
      For i = 0 To rounds - 1
        Dim max = rand.NextInt64()
        Dim result = Native.NextCryptoSingleMaxAot(max, err)
        AssertError(err)

        If result < 0 OrElse result > max Then
          Throw New Exception()
        End If

        If i Mod rounds / 10 = 0 Then
          Console.Write(".")
        End If
      Next

      sw.Stop()
      t = sw.ElapsedMilliseconds
      Console.WriteLine($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms")

      Console.Write($"Test{NameOf(NextCryptoSingleMinMaxAot)}: ")

      sw = Stopwatch.StartNew()
      err = Nothing
      For i = 0 To rounds - 1
        Dim min As Int64 = CLng(Int64.MaxValue / 3), max = 2 * min
        Dim result = Native.NextCryptoSingleMinMaxAot(min, max, err)
        AssertError(err)

        If result < min OrElse result > max Then Throw New Exception()

        If i Mod rounds / 10 = 0 Then Console.Write(".")
      Next

      sw.Stop()
      t = sw.ElapsedMilliseconds
      Console.WriteLine($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms")
      Console.WriteLine()
    End Sub

    Private Shared Sub TestRngCryptoSingleAot(rounds As Int32)
      Console.Write($"{NameOf(TestRngCryptoSingleAot)}: ")

      Dim rand = Random.[Shared]
      Dim sw = Stopwatch.StartNew()
      Dim out_ptr As IntPtr = Nothing
      For i = 0 To rounds - 1
        Dim size = rand.[Next](128, 512)
        Dim err = Native.RngCryptoSingleAot(size, out_ptr)
        AssertError(err)

        Dim result = ToFloats(Of Single)(out_ptr, size)
        Native.FreeBuffer(out_ptr)

        If result Is Nothing OrElse Not result.Length = size Then
          Throw New Exception()
        End If

        If i Mod rounds / 10 = 0 Then
          Console.Write(".")
        End If
      Next

      sw.Stop()
      Dim t = sw.ElapsedMilliseconds
      Console.WriteLine($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms")

      Console.Write($"Test{NameOf(RngCryptoSingleMaxAot)}: ")

      sw = Stopwatch.StartNew()
      out_ptr = Nothing
      For i = 0 To rounds - 1
        Dim max = rand.NextInt64()
        Dim size = rand.[Next](128, 512)
        Dim err = Native.RngCryptoSingleMaxAot(size, max, out_ptr)
        AssertError(err)

        Dim result = ToFloats(Of Single)(out_ptr, size)
        Native.FreeBuffer(out_ptr)

        If result Is Nothing OrElse Not result.Length = size Then
          Throw New Exception()
        End If

        If result.Min(Function(x) x) < 0.0 OrElse result.Max(Function(x) x) > max Then
          Throw New Exception()
        End If

        If i Mod rounds / 10 = 0 Then
          Console.Write(".")
        End If
      Next

      sw.Stop()
      t = sw.ElapsedMilliseconds
      Console.WriteLine($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms")


      Console.Write($"Test{NameOf(RngCryptoSingleMinMaxAot)}: ")

      sw = Stopwatch.StartNew()
      out_ptr = Nothing
      For i = 0 To rounds - 1
        Dim size = rand.[Next](128, 512)
        Dim min As Int64 = CLng(Int64.MaxValue / 3), max = min * 2
        Dim err = Native.RngCryptoSingleMinMaxAot(size, min, max, out_ptr)
        AssertError(err)

        Dim result = ToFloats(Of Single)(out_ptr, size)
        Native.FreeBuffer(out_ptr)

        If result Is Nothing OrElse Not result.Length = size Then Throw New Exception()

        If result.Min(Function(x) x) < min OrElse result.Max(Function(x) x) > max Then
          Throw New Exception()
        End If

        If i Mod rounds / 10 = 0 Then
          Console.Write(".")
        End If
      Next

      sw.Stop()
      t = sw.ElapsedMilliseconds
      Console.WriteLine($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms")
      Console.WriteLine()
    End Sub


    Private Shared Sub TestNextCryptoDecimalAot(rounds As Int32)
      Console.Write($"{NameOf(TestNextCryptoDecimalAot)}: ")

      Dim rand = Random.[Shared]
      Dim sw = Stopwatch.StartNew()
      Dim err As CError = Nothing
      For i = 0 To rounds - 1
        Dim result = Native.NextCryptoDecimalAot(err)
        AssertError(err)

        If result < 0 Then
          Throw New Exception()
        End If

        If i Mod rounds / 10 = 0 Then
          Console.Write(".")
        End If
      Next

      sw.Stop()
      Dim t = sw.ElapsedMilliseconds
      Console.WriteLine($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms")

      Console.Write($"Test{NameOf(NextCryptoDecimalMaxAot)}: ")

      sw = Stopwatch.StartNew()
      err = Nothing
      For i = 0 To rounds - 1
        Dim max = rand.NextInt64()
        Dim result = Native.NextCryptoDecimalMaxAot(max, err)
        AssertError(err)

        If result < 0D OrElse result > max Then
          Throw New Exception()
        End If

        If i Mod rounds / 10 = 0 Then
          Console.Write(".")
        End If
      Next

      sw.Stop()
      t = sw.ElapsedMilliseconds
      Console.WriteLine($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms")

      Console.Write($"Test{NameOf(NextCryptoDecimalMinMaxAot)}: ")

      sw = Stopwatch.StartNew()
      err = Nothing
      For i = 0 To rounds - 1
        Dim min As Int64 = CLng(Int64.MaxValue / 3), max = 2 * min
        Dim result = Native.NextCryptoDecimalMinMaxAot(min, max, err)
        AssertError(err)

        If result < min OrElse result > max Then
          Throw New Exception()
        End If

        If i Mod rounds / 10 = 0 Then
          Console.Write(".")
        End If
      Next

      sw.Stop()
      t = sw.ElapsedMilliseconds
      Console.WriteLine($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms")
      Console.WriteLine()
    End Sub

    Private Shared Sub TestRngCryptoDecimalAot(rounds As Int32)
      Console.Write($"{NameOf(TestRngCryptoDecimalAot)}: ")

      Dim rand = Random.[Shared]
      Dim sw = Stopwatch.StartNew()
      Dim out_ptr As IntPtr = Nothing
      For i = 0 To rounds - 1
        Dim size = rand.[Next](128, 512)
        Dim err = Native.RngCryptoDecimalAot(size, out_ptr)
        AssertError(err)

        Dim result = ToDecimals(out_ptr, size)
        Native.FreeBuffer(out_ptr)

        If result Is Nothing OrElse Not result.Length = size Then
          Throw New Exception()
        End If

        If i Mod rounds / 10 = 0 Then
          Console.Write(".")
        End If
      Next

      sw.Stop()
      Dim t = sw.ElapsedMilliseconds
      Console.WriteLine($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms")

      Console.Write($"Test{NameOf(RngCryptoDecimalMaxAot)}: ")

      sw = Stopwatch.StartNew()
      out_ptr = Nothing
      For i = 0 To rounds - 1
        Dim max = CDec(rand.NextInt64())
        Dim size = rand.[Next](128, 512)
        Dim err = Native.RngCryptoDecimalMaxAot(size, max, out_ptr)
        AssertError(err)

        Dim result = ToDecimals(out_ptr, size)
        Native.FreeBuffer(out_ptr)

        If result Is Nothing OrElse Not result.Length = size Then
          Throw New Exception()
        End If

        If result.Min(Function(x) x) < 0D OrElse result.Max(Function(x) x) > max Then
          Throw New Exception()
        End If

        If i Mod rounds / 10 = 0 Then
          Console.Write(".")
        End If
      Next

      sw.Stop()
      t = sw.ElapsedMilliseconds
      Console.WriteLine($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms")


      Console.Write($"Test{NameOf(RngCryptoDecimalMinMaxAot)}: ")

      sw = Stopwatch.StartNew()
      out_ptr = Nothing
      For i = 0 To rounds - 1
        Dim size = rand.[Next](128, 512)
        Dim min As Decimal = CDec(Int64.MaxValue / 3), max = min * 2
        Dim err = Native.RngCryptoDecimalMinMaxAot(size, min, max, out_ptr)
        AssertError(err)

        Dim result = ToDecimals(out_ptr, size)
        Native.FreeBuffer(out_ptr)

        If result Is Nothing OrElse Not result.Length = size Then
          Throw New Exception()
        End If

        If result.Min(Function(x) x) < min OrElse result.Max(Function(x) x) > max Then
          Throw New Exception()
        End If

        If i Mod rounds / 10 = 0 Then
          Console.Write(".")
        End If
      Next

      sw.Stop()
      t = sw.ElapsedMilliseconds
      Console.WriteLine($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms")
      Console.WriteLine()
    End Sub
  End Class
End Namespace





