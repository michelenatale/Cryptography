Option Strict On
Option Explicit On

Imports michele.natale
Imports System.Numerics
Imports System.Runtime.InteropServices
Imports System.Security.Cryptography

Namespace michele.natale.Tests
  Partial Friend NotInheritable Class CryptoPrimesTest
    Public Shared Sub StartNative(rounds As Int32)
      TestNextCryptoPrimesMinMaxUInt64(rounds)
      TestNextCryptoPrimesMinMaxBigInteger(rounds)
      TestNextCryptoPrimesBitsBigInteger(rounds)

      TestRngCryptoPrimesMinMaxUInt64(rounds * 10)
      TestRngCryptoPrimesMinMaxBigInteger(rounds)
      TestRngCryptoPrimesBitsBigInteger(rounds)

      Console.WriteLine()
    End Sub

    Private Shared Sub TestNextCryptoPrimesMinMaxUInt64(rounds As Int32)
      Console.Write($"{NameOf(TestNextCryptoPrimesMinMaxUInt64)}Aot: ")

      Dim rand = Random.[Shared]
      Dim sw = Stopwatch.StartNew()
      For i = 0 To rounds - 1
        Dim min_max = ToMinMaxULong()
        Dim mrr = [Enum].GetValues(Of PrimalityConfidence)()

        Dim err As CError
        Dim min = min_max.Min, max = min_max.Max
        Dim miller_rabin_rounds = mrr(rand.[Next](mrr.Length))
        Dim prime = Native.NextCryptoPrimesMinMaxUInt64Aot(
          CInt(miller_rabin_rounds), min, max, err)
        AssertError(err)

        If prime < min OrElse prime > max Then
          Throw New Exception()
        End If

        If Not IsMRPrimeUInt64(prime, miller_rabin_rounds) Then
          Throw New Exception()
        End If

        If i Mod rounds / 10 = 0 Then
          Console.Write(".")
        End If
      Next

      sw.[Stop]()
      Dim t = CDbl(sw.ElapsedMilliseconds)
      Console.WriteLine($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms")
    End Sub

    Private Shared Sub TestNextCryptoPrimesMinMaxBigInteger(rounds As Int32)
      Console.Write($"{NameOf(TestNextCryptoPrimesMinMaxBigInteger)}Aot: ")

      Dim rand = Random.[Shared]
      Dim sw = Stopwatch.StartNew()
      'Dim minMax As (min As var, max As var) = Nothing, out_ptr = Nothing, out_length As Int32 = Nothing
      For i = 0 To rounds - 1
        Dim byte_count = rand.[Next](5, 32)
        Dim min_max = ToMinMaxBigInteger(byte_count)
        Dim mrr = [Enum].GetValues(Of PrimalityConfidence)()

        Dim min = min_max.Min, max = min_max.Max
        Dim min_bytes = min.ToByteArray(True, True)
        Dim max_bytes = max.ToByteArray(True, True)
        Dim miller_rabin_rounds = mrr(rand.[Next](mrr.Length))

        Dim out_ptr As IntPtr, out_length As Int32
        Dim err = Native.NextCryptoPrimesMinMaxAot(
          CInt(miller_rabin_rounds),
          min_bytes, min_bytes.Length,
          max_bytes, max_bytes.Length,
          out_ptr, out_length)
        AssertError(err)

        Dim prime_bytes = ToBytes(out_ptr, out_length)
        Dim prime = New BigInteger(prime_bytes, True, True)

        If prime < min OrElse prime > max Then
          Throw New Exception()
        End If

        If Not IsMRPrime(prime, miller_rabin_rounds) Then
          Throw New Exception()
        End If

        If i Mod rounds / 10 = 0 Then
          Console.Write(".")
        End If
      Next

      sw.[Stop]()
      Dim t = CDbl(sw.ElapsedMilliseconds)
      Console.WriteLine($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms")
    End Sub

    Private Shared Sub TestNextCryptoPrimesBitsBigInteger(rounds As Int32)
      Console.Write($"{NameOf(TestNextCryptoPrimesBitsBigInteger)}Aot: ")

      Dim rand = Random.Shared
      Dim sw = Stopwatch.StartNew()

      For i = 0 To rounds - 1
        Dim bpl = CType([Enum].GetValues(GetType(BitPrimeLength)), BitPrimeLength())
        Dim mrr = CType([Enum].GetValues(GetType(PrimalityConfidence)), PrimalityConfidence())

        Dim bits = bpl(rand.Next(10)) ' only first 10
        Dim miller_rabin_rounds = mrr(rand.Next(mrr.Length))

        Dim out_ptr As IntPtr, out_length As Int32
        Dim err = Native.NextCryptoPrimesAot(
            CInt(miller_rabin_rounds), CInt(bits),
            out_ptr, out_length)
        AssertError(err)

        Dim prime_bytes = ToBytes(out_ptr, out_length)
        Dim prime = New BigInteger(prime_bytes, isUnsigned:=True, isBigEndian:=True)

        If prime.GetBitLength() <> CInt(bits) Then
          Throw New Exception()
        End If

        If Not IsMRPrime(prime, miller_rabin_rounds) Then
          Throw New Exception()
        End If

        If i Mod (rounds \ 10) = 0 Then
          Console.Write("."c)
        End If
      Next

      sw.Stop()
      Dim t = sw.ElapsedMilliseconds
      Console.WriteLine($" rounds = {rounds}; t = {t}ms; td = {t \ rounds}ms")
    End Sub

    Private Shared Sub TestRngCryptoPrimesMinMaxUInt64(rounds As Int32)
      Console.Write($"{NameOf(TestRngCryptoPrimesMinMaxUInt64)}Aot: ")

      Dim rand = Random.Shared
      Dim sw = Stopwatch.StartNew()

      For i = 0 To rounds - 1
        Dim count = rand.Next(10, 101)
        Dim range = ToMinMaxULong()
        Dim min = range.Min, max = range.Max

        Dim mrr = CType([Enum].GetValues(GetType(PrimalityConfidence)), PrimalityConfidence())
        Dim miller_rabin_rounds = mrr(rand.Next(mrr.Length))

        Dim output As IntPtr
        Dim err = Native.RngCryptoPrimesMinMaxUInt64Aot(
            CInt(miller_rabin_rounds), count,
            min, max, output)
        AssertError(err)

        ' output: pointer to array of UInt64
        Dim primes(count - 1) As UInt64
        For j = 0 To count - 1
          Dim bytes(7) As Byte
          Marshal.Copy(IntPtr.Add(output, j * 8), bytes, 0, 8)
          primes(j) = BitConverter.ToUInt64(bytes, 0)
        Next

        For Each prime In primes
          If prime < min OrElse prime > max Then
            Throw New Exception()
          End If

          If Not IsMRPrimeUInt64(prime, miller_rabin_rounds) Then
            Throw New Exception()
          End If
        Next

        If i Mod (rounds \ 10) = 0 Then
          Console.Write("."c)
        End If
      Next

      sw.Stop()
      Dim t = sw.ElapsedMilliseconds
      Console.WriteLine($" rounds = {rounds}; t = {t}ms; td = {t \ rounds}ms")
    End Sub

    Private Shared Sub TestRngCryptoPrimesMinMaxBigInteger(rounds As Int32)
      Console.Write($"{NameOf(TestRngCryptoPrimesMinMaxBigInteger)}Aot: ")

      Dim total_counts As Int64 = 0
      Dim total_byte_lengths As Int32 = 0
      Dim rand = Random.Shared
      Dim sw = Stopwatch.StartNew()

      For i = 0 To rounds - 1
        Dim counts = rand.Next(10, 101)
        total_counts += counts

        Dim byte_count = rand.Next(5, 15)
        total_byte_lengths += byte_count

        Dim range = ToMinMaxBigInteger(byte_count)
        Dim min = range.Min
        Dim max = range.Max

        Dim mrr = CType([Enum].GetValues(GetType(PrimalityConfidence)), PrimalityConfidence())
        Dim miller_rabin_rounds = mrr(rand.Next(mrr.Length))

        Dim min_bytes = min.ToByteArray(isUnsigned:=True, isBigEndian:=True)
        Dim max_bytes = max.ToByteArray(isUnsigned:=True, isBigEndian:=True)

        Dim output As IntPtr
        Dim out_lengths As IntPtr

        Dim err = Native.RngCryptoPrimesMinMaxAot(
            CInt(miller_rabin_rounds), counts,
            min_bytes, min_bytes.Length,
            max_bytes, max_bytes.Length,
            output, out_lengths)
        AssertError(err)

        ' out_lengths: int[counts]
        Dim lengths(counts - 1) As Int32
        Marshal.Copy(out_lengths, lengths, 0, counts)

        ' output: contiguous byte buffer
        Dim primes_bytes(counts - 1)() As Byte
        Dim offset As Int32 = 0

        For j = 0 To counts - 1
          Dim len = lengths(j)

          If len < 0 Then
            Throw New InvalidOperationException($"Negative length at index {j}: {len}")
          End If

          Dim dest(len - 1) As Byte
          primes_bytes(j) = dest

          If len > 0 Then
            Dim srcPtr = IntPtr.Add(output, offset)
            Marshal.Copy(srcPtr, dest, 0, len)
          End If

          offset += len
        Next

        Dim primes(counts - 1) As BigInteger
        For j = 0 To counts - 1
          primes(j) = New BigInteger(primes_bytes(j), isUnsigned:=True, isBigEndian:=True)
        Next

        For Each prime In primes
          If prime < min OrElse prime > max Then
            Throw New Exception()
          End If

          If Not IsMRPrime(prime, miller_rabin_rounds) Then
            Throw New Exception()
          End If
        Next

        If i Mod (rounds \ 10) = 0 Then
          Console.Write("."c)
        End If
      Next

      sw.Stop()
      Dim t = sw.ElapsedMilliseconds
      Console.WriteLine($" rounds = {rounds}; counts = {total_counts \ rounds}; bi-byte-length = {total_byte_lengths \ rounds}; t = {t}ms; td = {t \ rounds}ms")
    End Sub

    Private Shared Sub TestRngCryptoPrimesBitsBigInteger(rounds As Int32)
      Console.Write($"{NameOf(TestRngCryptoPrimesBitsBigInteger)}Aot: ")

      Dim total_bits As Int64 = 0
      Dim total_counts As Int64 = 0
      Dim rand = Random.Shared
      Dim sw = Stopwatch.StartNew()

      For i = 0 To rounds - 1
        Dim counts = rand.Next(10, 50)
        total_counts += counts

        Dim bpl = CType([Enum].GetValues(GetType(BitPrimeLength)), BitPrimeLength())
        Dim mrr = CType([Enum].GetValues(GetType(PrimalityConfidence)), PrimalityConfidence())

        Dim bits = bpl(rand.Next(7)) ' only first 7
        Dim miller_rabin_rounds = mrr(rand.Next(mrr.Length))
        total_bits += CInt(bits)

        Dim out_ptr As IntPtr, out_lengths As IntPtr
        Dim err = Native.RngCryptoPrimesAot(
            CInt(miller_rabin_rounds), CInt(bits), counts,
            out_ptr, out_lengths)
        AssertError(err)

        ' out_lengths: int[counts]
        Dim lengths(counts - 1) As Int32
        Marshal.Copy(out_lengths, lengths, 0, counts)

        ' out_ptr: contiguous byte buffer
        Dim primes_bytes(counts - 1)() As Byte
        Dim offset As Int32 = 0

        For j = 0 To counts - 1
          Dim len = lengths(j)

          If len < 0 Then
            Throw New InvalidOperationException($"Negative length at index {j}: {len}")
          End If

          Dim dest(len - 1) As Byte
          primes_bytes(j) = dest

          If len > 0 Then
            Dim srcPtr = IntPtr.Add(out_ptr, offset)
            Marshal.Copy(srcPtr, dest, 0, len)
          End If

          offset += len
        Next

        Dim primes(counts - 1) As BigInteger
        For j = 0 To counts - 1
          primes(j) = New BigInteger(primes_bytes(j), isUnsigned:=True, isBigEndian:=True)
        Next

        For Each prime In primes
          If prime.GetBitLength() <> CInt(bits) Then
            Throw New Exception()
          End If

          If Not IsMRPrime(prime, miller_rabin_rounds) Then
            Throw New Exception()
          End If
        Next

        If i Mod (rounds \ 10) = 0 Then
          Console.Write("."c)
        End If
      Next

      sw.Stop()
      Dim t = sw.ElapsedMilliseconds
      Console.WriteLine($" rounds = {rounds}; counts = {total_counts \ rounds}; bi-bits = {total_bits \ rounds}; t = {t}ms; td = {t \ rounds}ms")
    End Sub



    Private Shared Function ToMinMaxULong() As (Min As UInt64, Max As UInt64)
      Dim min As UInt64, max As UInt64

      While True
        min = NextUnmanagedULong()
        For i = 0 To 2
          max = NextUnmanagedULong()
          If max > min Then
            Return (min, max)
          End If
        Next
      End While

      Return Nothing
    End Function

    Private Shared Function ToMinMaxBigInteger(
      Optional byte_count As Int32 = 32) As (Min As BigInteger, Max As BigInteger)
      Dim min As BigInteger, max As BigInteger

      While True
        min = NextBigInteger(byte_count)
        For i = 0 To 2
          max = NextBigInteger(byte_count)
          If max > min Then
            Return (min, max)
          End If
        Next
      End While

      Return Nothing
    End Function


    Public Shared Function NextUnmanagedULong() As UInt64
      Dim buffer = New Byte(7) {} ' 8 Bytes für UInt64
      Random.Shared.NextBytes(buffer)
      Return BitConverter.ToUInt64(buffer, 0)
    End Function


    Public Shared Function NextBigInteger(Optional byte_count As Int32 = 32) As BigInteger
      Dim bytes = New Byte(byte_count - 1) {}
      Random.Shared.NextBytes(bytes)

      ' Sign-Bit entfernen → garantiert positiv
      bytes(bytes.Length - 1) = CByte(bytes(bytes.Length - 1) And &H7F)

      Return New BigInteger(bytes)
    End Function


    Private Shared Function IsMRPrimeUInt64(
      candidate As UInt64, confidence As PrimalityConfidence) As Boolean
      If candidate < 2UL Then Return False
      If candidate = 2UL Then Return True
      If (candidate And 1UL) = 0UL Then Return False

      Dim d As UInt64 = candidate - 1UL
      Dim s As Int32 = 0
      While (d And 1UL) = 0UL
        d >>= 1 : s += 1
      End While

      Dim bases As UInt64() =
      {
          2UL, 325UL, 9375UL, 28178UL,
          450775UL, 9780504UL, 1795265022UL
      }

      Dim i = 0
      Dim rounds = CInt(confidence)
      While i < rounds AndAlso i < bases.Length
        Dim a = bases(i) Mod candidate
        If a = 0UL Then
          i += 1
          Continue While
        End If

        Dim x As UInt64 = CULng(BigInteger.ModPow(a, d, candidate))
        If x = 1UL OrElse x = candidate - 1UL Then
          i += 1
          Continue While
        End If

        Dim witness = True
        For r = 1 To s - 1
          x = CULng(BigInteger.ModPow(x, 2, candidate))
          If x = candidate - 1UL Then
            witness = False
            Exit For
          End If
        Next

        If witness Then
          Return False
        End If
        i += 1
      End While

      Return True
    End Function


    Private Shared Function IsMRPrime(
      candidate As BigInteger, confidence As PrimalityConfidence) As Boolean
      If candidate < 2 Then Return False
      If candidate = 2 Then Return True
      If candidate.IsEven Then Return False

      Dim s As Int32 = 0
      Dim d As BigInteger = candidate - 1
      While d.IsEven
        d >>= 1 : s += 1
      End While

      Dim rounds = CInt(confidence)
      Dim buffer As Byte() = New Byte(candidate.GetByteCount() + 1 - 1) {}

      For i = 0 To rounds - 1
        Dim a As BigInteger
        Do
          RandomNumberGenerator.Fill(buffer)
          buffer(buffer.Length - 1) = 0 ' positiv erzwingen
          a = New BigInteger(buffer)
        Loop While a < 2 OrElse a >= candidate - 2

        Dim x As BigInteger = BigInteger.ModPow(a, d, candidate)
        If x = 1 OrElse x = candidate - 1 Then
          Continue For
        End If

        Dim witness = True
        For r = 1 To s - 1
          x = BigInteger.ModPow(x, 2, candidate)
          If x = candidate - 1 Then
            witness = False
            Exit For
          End If
        Next

        If witness Then Return False
      Next

      Return True
    End Function


  End Class
End Namespace