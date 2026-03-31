Option Strict On
Option Explicit On

Imports System.Security.Cryptography

Namespace michele.natale.Tests
  Partial Friend NotInheritable Class CryptoHashHmacTest

    Public Shared Sub StartNative(rounds As Int32)

      TestMd5(rounds)
      TestSha1(rounds)
      TestSha256(rounds)
      TestSha384(rounds)
      TestSha512(rounds)

      TestMd5Hmac(rounds)
      TestSha1Hmac(rounds)
      TestSha256Hmac(rounds)
      TestSha384Hmac(rounds)
      TestSha512Hmac(rounds)

      Console.WriteLine()
    End Sub

    Private Shared Sub TestSha1(rounds As Int32)
      Console.Write($"{NameOf(TestSha1)}Aot: ")
      Dim rand = Random.[Shared]
      Dim sw = Stopwatch.StartNew()
      Dim hash_ptr As IntPtr = Nothing, hash_length As Int32 = Nothing

      For i As Int32 = 0 To rounds - 1
        Dim size = rand.[Next](1, 128)
        Dim bytes = RngBytes(size)
        Dim err = Sha1HashDataAot(
           bytes, bytes.Length, hash_ptr, hash_length)
        AssertError(err)
        Dim data = ToBytes(hash_ptr, hash_length)
        FreeBuffer(hash_ptr)
        Dim hash = SHA1.HashData(bytes)
        If hash_length <> hash.Length Then Throw New Exception()
        If Not hash.SequenceEqual(data) Then Throw New Exception()
        If i Mod (rounds / 10) = 0 Then Console.Write(".")
      Next

      sw.[Stop]()
      Dim t = CDbl(sw.ElapsedMilliseconds)
      Console.Write($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms")
      Console.WriteLine()
    End Sub

    Private Shared Sub TestMd5(rounds As Int32)
      Console.Write($"{NameOf(TestMd5)}Aot: ")

      Dim rand = Random.[Shared]
      Dim sw = Stopwatch.StartNew()
      Dim hash_ptr As IntPtr = Nothing, hash_length As Int32 = Nothing
      For i = 0 To rounds - 1
        Dim size = rand.[Next](1, 128)
        Dim bytes = RngBytes(size)

        Dim err = Md5HashDataAot(
           bytes, bytes.Length, hash_ptr, hash_length)
        AssertError(err)

        Dim data = ToBytes(hash_ptr, hash_length)
        FreeBuffer(hash_ptr)

        Dim hash = MD5.HashData(bytes)
        If hash_length <> hash.Length Then Throw New Exception()

        If Not hash.SequenceEqual(data) Then Throw New Exception()

        If i Mod rounds / 10 = 0 Then Console.Write(".")
      Next

      sw.[Stop]()

      Dim t = CDbl(sw.ElapsedMilliseconds)
      Console.Write($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms")
      Console.WriteLine()
    End Sub

    Private Shared Sub TestSha256(rounds As Int32)
      Console.Write($"{NameOf(TestSha256)}Aot: ")

      Dim rand = Random.[Shared]
      Dim sw = Stopwatch.StartNew()
      Dim hash_ptr As IntPtr = Nothing, hash_length As Int32 = Nothing
      For i = 0 To rounds - 1
        Dim size = rand.[Next](1, 128)
        Dim bytes = RngBytes(size)

        Dim err = Sha256HashDataAot(bytes, bytes.Length, hash_ptr, hash_length)
        AssertError(err)

        Dim data = ToBytes(hash_ptr, hash_length)
        FreeBuffer(hash_ptr)

        Dim hash = SHA256.HashData(bytes)
        If hash_length <> hash.Length Then Throw New Exception()

        If Not hash.SequenceEqual(data) Then Throw New Exception()

        If i Mod rounds / 10 = 0 Then Console.Write(".")
      Next

      sw.[Stop]()

      Dim t = CDbl(sw.ElapsedMilliseconds)
      Console.Write($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms")
      Console.WriteLine()
    End Sub

    Private Shared Sub TestSha384(rounds As Int32)
      Console.Write($"{NameOf(TestSha384)}Aot: ")

      Dim rand = Random.[Shared]
      Dim sw = Stopwatch.StartNew()
      Dim hash_ptr As IntPtr = Nothing, hash_length As Int32 = Nothing
      For i = 0 To rounds - 1
        Dim size = rand.[Next](1, 128)
        Dim bytes = RngBytes(size)

        Dim err = Sha384HashDataAot(bytes, bytes.Length, hash_ptr, hash_length)
        AssertError(err)

        Dim data = ToBytes(hash_ptr, hash_length)
        FreeBuffer(hash_ptr)

        Dim hash = SHA384.HashData(bytes)
        If hash_length <> hash.Length Then Throw New Exception()

        If Not hash.SequenceEqual(data) Then Throw New Exception()

        If i Mod rounds / 10 = 0 Then Console.Write(".")
      Next

      sw.[Stop]()

      Dim t = CDbl(sw.ElapsedMilliseconds)
      Console.Write($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms")
      Console.WriteLine()
    End Sub

    Private Shared Sub TestSha512(rounds As Int32)
      Console.Write($"{NameOf(TestSha512)}Aot: ")

      Dim rand = Random.[Shared]
      Dim sw = Stopwatch.StartNew()
      Dim hash_ptr As IntPtr = Nothing, hash_length As Int32 = Nothing
      For i = 0 To rounds - 1
        Dim size = rand.[Next](1, 128)
        Dim bytes = RngBytes(size)

        Dim err = Sha512HashDataAot(bytes, bytes.Length, hash_ptr, hash_length)
        AssertError(err)

        Dim data = ToBytes(hash_ptr, hash_length)
        FreeBuffer(hash_ptr)

        Dim hash = SHA512.HashData(bytes)
        If hash_length <> hash.Length Then Throw New Exception()

        If Not hash.SequenceEqual(data) Then Throw New Exception()

        If i Mod rounds / 10 = 0 Then Console.Write(".")
      Next

      sw.[Stop]()

      Dim t = CDbl(sw.ElapsedMilliseconds)
      Console.Write($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms")
      Console.WriteLine()
    End Sub

    Private Shared Sub TestSha1Hmac(rounds As Int32)
      Console.Write($"{NameOf(TestSha1Hmac)}Aot: ")

      Dim rand = Random.[Shared]
      Dim sw = Stopwatch.StartNew()
      Dim hash_ptr As IntPtr = Nothing, hash_length As Int32 = Nothing
      For i = 0 To rounds - 1
        Dim size = rand.[Next](1, 128)
        Dim bytes = RngBytes(size)

        size = rand.[Next](1, 128)
        Dim key = RngBytes(size)

        Dim err = HmacSha1HashDataAot(
           bytes, bytes.Length, key, key.Length, hash_ptr, hash_length)
        AssertError(err)

        Dim data = ToBytes(hash_ptr, hash_length)
        FreeBuffer(hash_ptr)

        Dim hash = HMACSHA1.HashData(key, bytes)
        If hash_length <> hash.Length Then Throw New Exception()

        If Not hash.SequenceEqual(data) Then Throw New Exception()

        If i Mod rounds / 10 = 0 Then Console.Write(".")
      Next

      sw.[Stop]()

      Dim t = CDbl(sw.ElapsedMilliseconds)
      Console.Write($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms")
      Console.WriteLine()
    End Sub

    Private Shared Sub TestMd5Hmac(rounds As Int32)
      Console.Write($"{NameOf(TestMd5Hmac)}Aot: ")

      Dim rand = Random.[Shared]
      Dim sw = Stopwatch.StartNew()
      Dim hash_ptr As IntPtr = Nothing, hash_length As Int32 = Nothing
      For i = 0 To rounds - 1
        Dim size = rand.[Next](1, 128)
        Dim bytes = RngBytes(size)

        size = rand.[Next](1, 128)
        Dim key = RngBytes(size)

        Dim err = HmacMd5HashDataAot(
           bytes, bytes.Length, key, key.Length, hash_ptr, hash_length)
        AssertError(err)

        Dim data = ToBytes(hash_ptr, hash_length)
        FreeBuffer(hash_ptr)

        Dim hash = HMACMD5.HashData(key, bytes)
        If hash_length <> hash.Length Then Throw New Exception()

        If Not hash.SequenceEqual(data) Then Throw New Exception()

        If i Mod rounds / 10 = 0 Then Console.Write(".")
      Next

      sw.[Stop]()

      Dim t = CDbl(sw.ElapsedMilliseconds)
      Console.Write($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms")
      Console.WriteLine()
    End Sub

    Private Shared Sub TestSha256Hmac(rounds As Int32)
      Console.Write($"{NameOf(TestSha256Hmac)}Aot: ")

      Dim rand = Random.[Shared]
      Dim sw = Stopwatch.StartNew()
      Dim hash_ptr As IntPtr = Nothing, hash_length As Int32 = Nothing
      For i = 0 To rounds - 1
        Dim size = rand.[Next](1, 128)
        Dim bytes = RngBytes(size)

        size = rand.[Next](1, 128)
        Dim key = RngBytes(size)

        Dim err = HmacSha256HashDataAot(
           bytes, bytes.Length, key, key.Length, hash_ptr, hash_length)
        AssertError(err)

        Dim data = ToBytes(hash_ptr, hash_length)
        FreeBuffer(hash_ptr)

        Dim hash = HMACSHA256.HashData(key, bytes)
        If hash_length <> hash.Length Then Throw New Exception()

        If Not hash.SequenceEqual(data) Then Throw New Exception()

        If i Mod rounds / 10 = 0 Then Console.Write(".")
      Next

      sw.[Stop]()

      Dim t = CDbl(sw.ElapsedMilliseconds)
      Console.Write($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms")
      Console.WriteLine()
    End Sub

    Private Shared Sub TestSha384Hmac(rounds As Int32)
      Console.Write($"{NameOf(TestSha384Hmac)}Aot: ")

      Dim rand = Random.[Shared]
      Dim sw = Stopwatch.StartNew()
      Dim hash_ptr As IntPtr = Nothing, hash_length As Int32 = Nothing
      For i = 0 To rounds - 1
        Dim size = rand.[Next](1, 128)
        Dim bytes = RngBytes(size)

        size = rand.[Next](1, 128)
        Dim key = RngBytes(size)

        Dim err = HmacSha384HashDataAot(
           bytes, bytes.Length, key, key.Length, hash_ptr, hash_length)
        AssertError(err)

        Dim data = ToBytes(hash_ptr, hash_length)
        FreeBuffer(hash_ptr)

        Dim hash = HMACSHA384.HashData(key, bytes)
        If hash_length <> hash.Length Then Throw New Exception()

        If Not hash.SequenceEqual(data) Then Throw New Exception()

        If i Mod rounds / 10 = 0 Then Console.Write(".")
      Next

      sw.[Stop]()

      Dim t = CDbl(sw.ElapsedMilliseconds)
      Console.Write($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms")
      Console.WriteLine()
    End Sub

    Private Shared Sub TestSha512Hmac(rounds As Int32)
      Console.Write($"{NameOf(TestSha512Hmac)}Aot: ")

      Dim rand = Random.[Shared]
      Dim sw = Stopwatch.StartNew()
      Dim hash_ptr As IntPtr = Nothing, hash_length As Int32 = Nothing
      For i = 0 To rounds - 1
        Dim size = rand.[Next](1, 128)
        Dim bytes = RngBytes(size)

        size = rand.[Next](1, 128)
        Dim key = RngBytes(size)

        Dim err = HmacSha512HashDataAot(
           bytes, bytes.Length, key, key.Length, hash_ptr, hash_length)
        AssertError(err)

        Dim data = ToBytes(hash_ptr, hash_length)
        FreeBuffer(hash_ptr)

        Dim hash = HMACSHA512.HashData(key, bytes)
        If hash_length <> hash.Length Then Throw New Exception()

        If Not hash.SequenceEqual(data) Then Throw New Exception()

        If i Mod rounds / 10 = 0 Then Console.Write(".")
      Next

      sw.[Stop]()

      Dim t = CDbl(sw.ElapsedMilliseconds)
      Console.Write($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms")
      Console.WriteLine()
    End Sub

  End Class
End Namespace
