Option Strict On
Option Explicit On


Imports System.IO
Imports System.Text
Imports System.IO.Compression



Namespace michele.natale.Tests

  Partial Class CompressesTest

    Private Shared ReadOnly Property OriginalString As String
      Get
        Return File.ReadAllText("data.txt")
      End Get
    End Property '5.294 MB

    Public Shared Sub StartCompress(rounds As Int32)
      TestCompressGzip(rounds)
      TestCompressFileGzip(rounds)
      TestCompressFileBsGzip(rounds)

      TestCompressBrotli(rounds)
      TestCompressFileBrotli(rounds)
      TestCompressFileBsBrotli(rounds)
    End Sub

    Private Shared Sub TestCompressGzip(rounds As Int32)
      Console.Write($"{NameOf(TestCompressGzip)}Aot: ")

      Dim message = Encoding.UTF8.GetBytes(OriginalString)

      Dim rand = Random.Shared
      Dim sw = Stopwatch.StartNew()

      Dim out_length As Int32 = 0
      Dim out_ptr As IntPtr = Nothing

      For i = 0 To rounds - 1
        Dim compresslevel = [Enum].GetValues(Of CompressionLevel)()
        Dim idx = rand.Next(0, compresslevel.Length)

        Dim err = Native.CompressMessageGZipAot(
          message, message.Length,
          CByte(compresslevel(idx)),
          out_ptr, out_length)
        AssertError(err)

        Dim compress = ToBytes(out_ptr, out_length)
        Native.FreeBuffer(out_ptr)

        err = Native.DecompressMessageGZipAot(compress, compress.Length, out_ptr, out_length)
        AssertError(err)

        Dim decompress = ToBytes(out_ptr, out_length)
        Native.FreeBuffer(out_ptr)

        If Not message.SequenceEqual(decompress) Then
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

    Private Shared Sub TestCompressFileGzip(rounds As Int32)
      Console.Write($"{NameOf(TestCompressFileGzip)}Aot: ")


      Dim src = "data.txt", dest = "datacompress", destr = "datar.txt"
      File.Delete(dest) : File.Delete(destr)

      Dim rand = Random.Shared
      Dim sw = Stopwatch.StartNew()

      For i = 0 To rounds - 1
        Dim compresslevel = [Enum].GetValues(Of CompressionLevel)()
        Dim idx = rand.Next(0, compresslevel.Length)

        Dim src_utf8 = Encoding.UTF8.GetBytes(src)
        Dim dest_utf8 = Encoding.UTF8.GetBytes(dest)

        Dim err = Native.CompressFileGZipAot(
          src_utf8, src_utf8.Length,
          dest_utf8, dest_utf8.Length,
          CByte(compresslevel(idx)))
        AssertError(err)

        Dim destr_utf8 = Encoding.UTF8.GetBytes(destr)

        err = Native.DecompressFileGZipAot(dest_utf8, dest_utf8.Length, destr_utf8, destr_utf8.Length)
        AssertError(err)

        Dim istrue = Native.EqualFilesAot(
          src_utf8, src_utf8.Length,
          destr_utf8, destr_utf8.Length, err)
        AssertError(err)

        If Not istrue Then
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

    Private Shared Sub TestCompressFileBsGzip(rounds As Int32)
      Console.Write($"{NameOf(TestCompressFileBsGzip)}Aot: ")


      Dim src = "data.txt", dest = "datacompress", destr = "datar.txt"
      File.Delete(dest)
      File.Delete(destr)

      Dim rand = Random.Shared
      Dim sw = Stopwatch.StartNew()
      For i = 0 To rounds - 1
        Dim compresslevel = [Enum].GetValues(Of CompressionLevel)()
        Dim idx = rand.Next(0, compresslevel.Length)

        Dim src_utf8 = Encoding.UTF8.GetBytes(src)
        Dim dest_utf8 = Encoding.UTF8.GetBytes(dest)

        Dim err = Native.CompressFileBufferSizeGZipAot(
          src_utf8, src_utf8.Length,
          dest_utf8, dest_utf8.Length,
          BUFFER_SIZE_DEFAULT, CByte(compresslevel(idx)))
        AssertError(err)

        Dim destr_utf8 = Encoding.UTF8.GetBytes(destr)

        err = Native.DecompressFileBufferSizeGZipAot(
          dest_utf8, dest_utf8.Length,
          destr_utf8, destr_utf8.Length,
          BUFFER_SIZE_DEFAULT)
        AssertError(err)

        Dim istrue = Native.EqualFilesAot(
          src_utf8, src_utf8.Length,
          src_utf8, src_utf8.Length,
          err)
        AssertError(err)

        If Not istrue Then
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

    Private Shared Sub TestCompressBrotli(rounds As Int32)
      Console.Write($"{NameOf(TestCompressBrotli)}Aot: ")

      Dim message = Encoding.UTF8.GetBytes(OriginalString)

      Dim rand = Random.Shared
      Dim sw = Stopwatch.StartNew()

      Dim out_length As Int32 = 0
      Dim out_ptr As IntPtr = Nothing

      For i = 0 To rounds - 1
        Dim compresslevel = [Enum].GetValues(Of CompressionLevel)()
        Dim idx = rand.Next(0, compresslevel.Length)

        Dim err = Native.CompressMessageBrotliAot(
          message, message.Length,
          CByte(compresslevel(idx)),
          out_ptr, out_length)
        AssertError(err)

        Dim compress = ToBytes(out_ptr, out_length)
        Native.FreeBuffer(out_ptr)

        err = Native.DecompressMessageBrotliAot(
          compress, compress.Length,
          out_ptr, out_length)
        AssertError(err)

        Dim decompress = ToBytes(out_ptr, out_length)
        Native.FreeBuffer(out_ptr)

        If Not message.SequenceEqual(decompress) Then
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

    Private Shared Sub TestCompressFileBrotli(rounds As Int32)
      Console.Write($"{NameOf(TestCompressFileBrotli)}Aot: ")


      Dim src = "data.txt", dest = "datacompress", destr = "datar.txt"
      File.Delete(dest)
      File.Delete(destr)

      Dim rand = Random.Shared
      Dim sw = Stopwatch.StartNew()
      For i = 0 To rounds - 1
        Dim compresslevel = [Enum].GetValues(Of CompressionLevel)()
        Dim idx = rand.Next(0, compresslevel.Length)

        Dim src_utf8 = Encoding.UTF8.GetBytes(src)
        Dim dest_utf8 = Encoding.UTF8.GetBytes(dest)

        Dim err = Native.CompressFileBufferSizeBrotliAot(
          src_utf8, src_utf8.Length,
          dest_utf8, dest_utf8.Length,
          BUFFER_SIZE_DEFAULT, CByte(compresslevel(idx)))
        AssertError(err)

        Dim destr_utf8 = Encoding.UTF8.GetBytes(destr)

        err = Native.DecompressFileBufferSizeBrotliAot(
          dest_utf8, dest_utf8.Length,
          destr_utf8, destr_utf8.Length,
          BUFFER_SIZE_DEFAULT)
        AssertError(err)

        Dim istrue = Native.EqualFilesAot(
          src_utf8, src_utf8.Length,
          src_utf8, src_utf8.Length, err)
        AssertError(err)

        If Not istrue Then
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

    Private Shared Sub TestCompressFileBsBrotli(rounds As Int32)
      Console.Write($"{NameOf(TestCompressFileBsBrotli)}Aot: ")


      Dim src = "data.txt", dest = "datacompress", destr = "datar.txt"
      File.Delete(dest) : File.Delete(destr)

      Dim rand = Random.Shared
      Dim sw = Stopwatch.StartNew()

      For i = 0 To rounds - 1
        Dim compresslevel = [Enum].GetValues(Of CompressionLevel)()
        Dim idx = rand.Next(0, compresslevel.Length)

        Dim src_utf8 = Encoding.UTF8.GetBytes(src)
        Dim dest_utf8 = Encoding.UTF8.GetBytes(dest)

        Dim err = Native.CompressFileBrotliAot(
          src_utf8, src_utf8.Length,
          dest_utf8, dest_utf8.Length,
          CByte(compresslevel(idx)))
        AssertError(err)

        Dim destr_utf8 = Encoding.UTF8.GetBytes(destr)

        err = Native.DecompressFileBrotliAot(
          dest_utf8, dest_utf8.Length,
          destr_utf8, destr_utf8.Length)
        AssertError(err)

        Dim istrue = Native.EqualFilesAot(
          src_utf8, src_utf8.Length,
          src_utf8, src_utf8.Length, err)
        AssertError(err)

        If Not istrue Then
          Throw New Exception()
        End If

        If i Mod rounds / 10 = 0 Then
          Console.Write(".")
        End If
      Next

      sw.[Stop]()

      Dim t = CDbl(sw.ElapsedMilliseconds)
      Console.WriteLine($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms")
      Console.WriteLine()
    End Sub


  End Class
End Namespace