Option Strict On
Option Explicit On

Imports System.IO
Imports System.Text
Imports System.IO.Compression
Imports System.Security.Cryptography
Imports System.Runtime.InteropServices


Namespace michele.natale.Tests

  Partial Class CompressesTest

    Public Shared Sub StartFileCompressPackage(rounds As Int32)
      TestPackNoneFile()
      TestPackNoneBsFile()
      TestPackGZipFile()
      TestPackBrotliFile()

      Dim srcfolder = "sourcefolder"
      PreparationAsync(srcfolder).GetAwaiter().GetResult()
      TestPackNoneArchiv(srcfolder)
      TestPackGZipArchiv(srcfolder)
      TestPackBrotliArchiv(srcfolder)
      Finish(srcfolder)

      Console.WriteLine()

    End Sub

    Private Shared Sub TestPackNoneFile()

      Dim rand = Random.Shared
      Dim sw = Stopwatch.StartNew()
      Dim compresstype = CompressionType.None
      Dim outputfolder = "output", archivepath = "test.fcp"
      Dim packlist As String() = {"data2.txt", "data3.txt", "data2.txt", "data3.txt"}

      Dim pack_ptr = ToPackListPtr(packlist)
      Dim archiv_path_utf8 = Encoding.UTF8.GetBytes(archivepath)
      Dim totalfilesize As Int64 = 0, totalcompresssize As Int64 = 0

      ' 'PackFileBsCLAot' would use the
      ' buffersize and compressionlevel
      Dim err = Native.PackFileAot(
        pack_ptr.FNamesPtr, pack_ptr.FLengthsPtr, pack_ptr.Cnt,
        archiv_path_utf8, archiv_path_utf8.Length,
        CByte(compresstype),
        totalfilesize, totalcompresssize)
      AssertError(err)
      FreePackListPtr(pack_ptr)

      'With HeaderInformation
      'Console.WriteLine($"Total File Size = {totalfilesize} Bytes, Total Compression Size = {totalcompresssize} Bytes and Total Compression Ratio = {totalcompresssize / (double)totalfilesize}\n");

      Dim output_folder_utf8 = Encoding.UTF8.GetBytes(outputfolder)

      ' Extract files, here without a buffer size
      ' 'UnPackFileArchivBsAot' would be set to buffersize
      err = Native.UnPackFileArchivAot(
        archiv_path_utf8, archiv_path_utf8.Length,
        output_folder_utf8, output_folder_utf8.Length)
      AssertError(err)

      If Not FileEqualsSpec(packlist, outputfolder) Then
        Throw New Exception()
      End If

      sw.[Stop]()

      Console.Write($"{NameOf(TestPackNoneFile)}Aot: ")

      Dim t = CDbl(sw.ElapsedMilliseconds)
      Dim sum_size = SumFileSizes(packlist)
      Dim compress_size = New FileInfo(archivepath).Length
      Console.WriteLine($" t = {t}ms; file_count = {sum_size.FCnt}; file_sizes = {sum_size.Size}; compress_size = {compress_size}")
    End Sub

    Private Shared Sub TestPackNoneBsFile()

      Dim rand = Random.Shared
      Dim sw = Stopwatch.StartNew()
      Dim outputfolder = "output", archivepath = "test.fcp"
      Dim packlist As String() = {"data2.txt", "data3.txt", "data2.txt", "data3.txt"}

      Dim cl = [Enum].GetValues(Of CompressionLevel)()
      Dim idx = rand.Next(0, cl.Length)
      Dim compresslevel = cl(idx)

      Dim pack_ptr = ToPackListPtr(packlist)
      Dim compresstype = CompressionType.None
      Dim archiv_path_utf8 = Encoding.UTF8.GetBytes(archivepath)
      Dim totalfilesize As Int64 = 0, totalcompresssize As Int64 = 0

      Dim err = Native.PackFileBsCLAot(
        pack_ptr.FNamesPtr, pack_ptr.FLengthsPtr, pack_ptr.Cnt,
        archiv_path_utf8, archiv_path_utf8.Length,
        CByte(compresstype), BUFFER_SIZE_DEFAULT, CByte(compresslevel),
        totalfilesize, totalcompresssize)
      AssertError(err)
      FreePackListPtr(pack_ptr)


      'With HeaderInformation
      'Console.WriteLine($"Total File Size = {totalfilesize} Bytes, Total Compression Size = {totalcompresssize} Bytes and Total Compression Ratio = {totalcompresssize / (double)totalfilesize}\n");

      Dim output_folder_utf8 = Encoding.UTF8.GetBytes(outputfolder)

      ' UnPack Files
      err = Native.UnPackFileArchivBsAot(
        archiv_path_utf8, archiv_path_utf8.Length,
        output_folder_utf8, output_folder_utf8.Length,
        BUFFER_SIZE_DEFAULT)
      AssertError(err)

      If Not FileEqualsSpec(packlist, outputfolder) Then
        Throw New Exception()
      End If

      sw.[Stop]()

      Console.Write($"{NameOf(TestPackNoneBsFile)}Aot: ")

      Dim t = CDbl(sw.ElapsedMilliseconds)
      Dim sum = SumFileSizes(packlist)
      Dim compress_size = New FileInfo(archivepath).Length
      Console.WriteLine($" t = {t}ms; file_count = {sum.FCnt}; file_sizes = {sum.Size}; compress_size = {compress_size}")
    End Sub


    Private Shared Sub TestPackGZipFile()

      Dim rand = Random.Shared
      Dim sw = Stopwatch.StartNew()
      Dim outputfolder = "output", archivepath = "test.fcp"
      Dim packlist As String() = {"data2.txt", "data3.txt", "data2.txt", "data3.txt"}

      Dim pack_ptr = ToPackListPtr(packlist)
      Dim compresstype = CompressionType.GZip
      Dim archiv_path_utf8 = Encoding.UTF8.GetBytes(archivepath)
      Dim totalfilesize As Int64 = 0, totalcompresssize As Int64 = 0

      ' 'PackFileBsCLAot' would use the buffersize and compressionlevel
      Dim err = Native.PackFileAot(
        pack_ptr.FNamesPtr, pack_ptr.FLengthsPtr, pack_ptr.Cnt,
        archiv_path_utf8, archiv_path_utf8.Length, CByte(compresstype),
        totalfilesize, totalcompresssize)
      AssertError(err)

      'With HeaderInformation
      'Console.WriteLine($"Total File Size = {totalfilesize} Bytes, Total Compression Size = {totalcompresssize} Bytes and Total Compression Ratio = {totalcompresssize / (double)totalfilesize}\n");

      Dim output_folder_utf8 = Encoding.UTF8.GetBytes(outputfolder)

      ' UnPack Files
      ' 'UnPackFileArchivBsAot' would be set to buffersize
      err = Native.UnPackFileArchivAot(
        archiv_path_utf8, archiv_path_utf8.Length,
        output_folder_utf8, output_folder_utf8.Length)
      AssertError(err)

      If Not FileEqualsSpec(packlist, outputfolder) Then
        Throw New Exception()
      End If

      sw.[Stop]()

      Console.Write($"{NameOf(TestPackGZipFile)}Aot: ")

      Dim sum = SumFileSizes(packlist)
      Dim t = CDbl(sw.ElapsedMilliseconds)
      Dim compress_size = New FileInfo(archivepath).Length
      Console.WriteLine($" t = {t}ms; file_count = {sum.FCnt}; file_sizes = {sum.Size}; compress_size = {compress_size}")

    End Sub
    Private Shared Sub TestPackBrotliFile()

      Dim rand = Random.Shared
      Dim sw = Stopwatch.StartNew()
      Dim outputfolder = "output", archivepath = "test.fcp"
      Dim packlist As String() = {"data2.txt", "data3.txt", "data2.txt", "data3.txt"}

      Dim pack_ptr = ToPackListPtr(packlist)
      Dim compresstype = CompressionType.Brotli
      Dim archiv_path_utf8 = Encoding.UTF8.GetBytes(archivepath)
      Dim totalfilesize As Int64 = 0, totalcompresssize As Int64 = 0

      ' 'PackFileBsCLAot' would use the buffersize and compressionlevel
      Dim err = Native.PackFileAot(
        pack_ptr.FNamesPtr, pack_ptr.FLengthsPtr, pack_ptr.Cnt,
        archiv_path_utf8, archiv_path_utf8.Length,
        CByte(compresstype),
        totalfilesize, totalcompresssize)
      AssertError(err)

      'With HeaderInformation
      'Console.WriteLine($"Total File Size = {totalfilesize} Bytes, Total Compression Size = {totalcompresssize} Bytes and Total Compression Ratio = {totalcompresssize / (double)totalfilesize}\n");

      Dim output_folder_utf8 = Encoding.UTF8.GetBytes(outputfolder)

      ' UnPack Files
      ' 'UnPackFileArchivBsAot' would be set to buffersize
      err = Native.UnPackFileArchivAot(
        archiv_path_utf8, archiv_path_utf8.Length,
        output_folder_utf8, output_folder_utf8.Length)
      AssertError(err)

      If Not FileEqualsSpec(packlist, outputfolder) Then
        Throw New Exception()
      End If

      sw.[Stop]()

      Console.Write($"{NameOf(TestPackBrotliFile)}Aot: ")

      Dim t = CDbl(sw.ElapsedMilliseconds)
      Dim sum = SumFileSizes(packlist)
      Dim compress_size = New FileInfo(archivepath).Length
      Console.WriteLine($" t = {t}ms; file_count = {sum.FCnt}; file_sizes = {sum.Size}; compress_size = {compress_size}")
      Console.WriteLine() : Console.WriteLine()
    End Sub

    Private Shared Sub TestPackNoneArchiv(srcfolder As String)

      Dim rand = Random.Shared
      Dim sw = Stopwatch.StartNew()
      Dim outputfolder = "output", archivepath = "test.fcp"

      Dim compresstype = CompressionType.None
      Dim src_folder_utf8 = Encoding.UTF8.GetBytes(srcfolder)
      Dim archive_path_utf8 = Encoding.UTF8.GetBytes(archivepath)
      Dim totalfilesize As Int64 = 0, totalcompresssize As Int64 = 0

      Dim err = Native.PackArchivAot(
        src_folder_utf8, src_folder_utf8.Length,
        archive_path_utf8, archive_path_utf8.Length,
        CByte(compresstype),
        totalfilesize, totalcompresssize)
      AssertError(err)

      'With HeaderInformation
      'Console.WriteLine($"Total File Size = {totalfilesize} Bytes, Total Compression Size = {totalcompresssize} Bytes and Total Compression Ratio = {totalcompresssize / (double)totalfilesize}\n");

      Dim output_folder_utf8 = Encoding.UTF8.GetBytes(outputfolder)

      err = Native.UnPackFileArchivAot(
        archive_path_utf8, archive_path_utf8.Length,
        output_folder_utf8, output_folder_utf8.Length)
      AssertError(err)

      If Not FileEqualsSpec(srcfolder, outputfolder) Then
        Throw New Exception()
      End If

      sw.[Stop]()

      Console.Write($"{NameOf(TestPackNoneArchiv)}Aot: ")

      Dim t = CDbl(sw.ElapsedMilliseconds)
      Dim sum = SumFileSizesFolder(srcfolder)
      Dim compress_size = New FileInfo(archivepath).Length
      Console.WriteLine($" t = {t}ms; file_count = {sum.FCnt}; file_sizes = {sum.Size}; compress_size = {compress_size}")
    End Sub

    Private Shared Sub TestPackGZipArchiv(srcfolder As String)

      Dim rand = Random.Shared
      Dim sw = Stopwatch.StartNew()
      Dim outputfolder = "output", archivepath = "test.fcp"

      Dim compresstype = CompressionType.GZip
      Dim src_folder_utf8 = Encoding.UTF8.GetBytes(srcfolder)
      Dim archive_path_utf8 = Encoding.UTF8.GetBytes(archivepath)
      Dim totalfilesize As Int64 = 0, totalcompresssize As Int64 = 0

      Dim err = Native.PackArchivAot(
        src_folder_utf8, src_folder_utf8.Length,
        archive_path_utf8, archive_path_utf8.Length,
        CByte(compresstype), totalfilesize, totalcompresssize)
      AssertError(err)

      'With HeaderInformation
      'Console.WriteLine($"Total File Size = {totalfilesize} Bytes, Total Compression Size = {totalcompresssize} Bytes and Total Compression Ratio = {totalcompresssize / (double)totalfilesize}\n");

      Dim output_folder_utf8 = Encoding.UTF8.GetBytes(outputfolder)

      err = Native.UnPackFileArchivAot(
        archive_path_utf8, archive_path_utf8.Length,
        output_folder_utf8, output_folder_utf8.Length)
      AssertError(err)

      If Not FileEqualsSpec(srcfolder, outputfolder) Then
        Throw New Exception()
      End If

      sw.[Stop]()

      Console.Write($"{NameOf(TestPackGZipArchiv)}Aot: ")

      Dim t = CDbl(sw.ElapsedMilliseconds)
      Dim sum = SumFileSizesFolder(srcfolder)
      Dim compress_size = New FileInfo(archivepath).Length
      Console.WriteLine($" t = {t}ms; file_count = {sum.FCnt}; file_sizes = {sum.Size}; compress_size = {compress_size}")
    End Sub

    Private Shared Sub TestPackBrotliArchiv(srcfolder As String)

      Dim rand = Random.Shared
      Dim sw = Stopwatch.StartNew()
      Dim outputfolder = "output", archivepath = "test.fcp"

      Dim compresstype = CompressionType.Brotli
      Dim src_folder_utf8 = Encoding.UTF8.GetBytes(srcfolder)
      Dim archive_path_utf8 = Encoding.UTF8.GetBytes(archivepath)
      Dim totalfilesize As Int64 = 0, totalcompresssize As Int64 = 0

      Dim err = Native.PackArchivAot(
        src_folder_utf8, src_folder_utf8.Length,
        archive_path_utf8, archive_path_utf8.Length,
        CByte(compresstype),
        totalfilesize, totalcompresssize)
      AssertError(err)

      'With HeaderInformation
      'Console.WriteLine($"Total File Size = {totalfilesize} Bytes, Total Compression Size = {totalcompresssize} Bytes and Total Compression Ratio = {totalcompresssize / (double)totalfilesize}\n");

      Dim output_folder_utf8 = Encoding.UTF8.GetBytes(outputfolder)

      err = Native.UnPackFileArchivAot(
        archive_path_utf8, archive_path_utf8.Length,
        output_folder_utf8, output_folder_utf8.Length)
      AssertError(err)

      If Not FileEqualsSpec(srcfolder, outputfolder) Then
        Throw New Exception()
      End If

      sw.[Stop]()

      Console.Write($"{NameOf(TestPackBrotliArchiv)}Aot: ")

      Dim t = CDbl(sw.ElapsedMilliseconds)
      Dim sum = SumFileSizesFolder(srcfolder)
      Dim compress_size = New FileInfo(archivepath).Length
      Console.WriteLine($" t = {t}ms; file_count = {sum.FCnt}; file_sizes = {sum.Size}; compress_size = {compress_size}")
      Console.WriteLine()
    End Sub

    Public Shared Function ToPackListPtr(packlist As String()) As (FNamesPtr As IntPtr, FLengthsPtr As IntPtr, Cnt As Int32)
      If packlist Is Nothing OrElse packlist.Length = 0 Then
        Return (IntPtr.Zero, IntPtr.Zero, 0)
      End If

      Dim count As Int32 = packlist.Length
      Dim names_size As UIntPtr = CType(count * IntPtr.Size, UIntPtr)
      Dim names As IntPtr = Marshal.AllocHGlobal(CType(names_size, Int32))

      Dim lengths_size As UIntPtr = CType(count * Marshal.SizeOf(Of Int32)(), UIntPtr)
      Dim lengths As IntPtr = Marshal.AllocHGlobal(CType(lengths_size, Int32))

      For i As Int32 = 0 To count - 1
        Dim utf8 As Byte() = Encoding.UTF8.GetBytes(If(packlist(i), String.Empty))
        Dim str_ptr As IntPtr = Marshal.AllocHGlobal(utf8.Length + 1)
        Marshal.Copy(utf8, 0, str_ptr, utf8.Length)
        Marshal.WriteByte(str_ptr, utf8.Length, 0) ' set last zero

        Marshal.WriteIntPtr(names, i * IntPtr.Size, str_ptr)
        Marshal.WriteInt32(lengths, i * Marshal.SizeOf(Of Int32)(), utf8.Length)
      Next

      Return (names, lengths, count)
    End Function

    Public Shared Sub FreePackListPtr(
      ByRef pack_ptr As (FNamesPtr As IntPtr, FLengthsPtr As IntPtr, Cnt As Int32))
      If pack_ptr.Cnt <= 0 Then
        Return
      End If

      ' --- 1) UTF‑8 Strings freigeben ---
      For i As Int32 = 0 To pack_ptr.Cnt - 1
        Dim strPtr As IntPtr = Marshal.ReadIntPtr(pack_ptr.FNamesPtr, i * IntPtr.Size)
        If strPtr <> IntPtr.Zero Then
          Marshal.FreeHGlobal(strPtr)
        End If
      Next

      ' --- 2) Array der String‑Pointer freigeben ---
      If pack_ptr.FNamesPtr <> IntPtr.Zero Then
        Marshal.FreeHGlobal(pack_ptr.FNamesPtr)
      End If

      ' --- 3) Array der Längen freigeben ---
      If pack_ptr.FLengthsPtr <> IntPtr.Zero Then
        Marshal.FreeHGlobal(pack_ptr.FLengthsPtr)
      End If

      ' --- 4) Struktur zurücksetzen ---
      pack_ptr.FNamesPtr = IntPtr.Zero
      pack_ptr.FLengthsPtr = IntPtr.Zero
      pack_ptr.Cnt = 0
    End Sub

    Private Shared Sub MarshalFree(ParamArray ptrs As IntPtr())

      Dim length = ptrs.Length
      For i = 0 To length - 1
        Marshal.FreeHGlobal(ptrs(i))
        If Not ptrs(i) = IntPtr.Zero Then
          ptrs(i) = IntPtr.Zero
        End If
      Next
    End Sub


    'Public Shared Function ToPackListPtr2(packlist As String()) As (FNamesPtr As IntPtr, FLengthsPtr As IntPtr, Cnt As Int32)
    '  If packlist Is Nothing OrElse packlist.Length = 0 Then
    '    Return (IntPtr.Zero, IntPtr.Zero, 0)
    '  End If

    '  Dim count = packlist.Length

    '  '' Speicher für byte** (UTF8‑Pointer)
    '  'Dim names_size As UIntPtr = CType(count * IntPtr.Size, UIntPtr)
    '  'Dim names As IntPtr = Marshal.AllocHGlobal(CInt(names_size))

    '  '' Speicher für int* (Längen)
    '  'Dim lengths_size As UIntPtr = CType(count * 4, UIntPtr) ' int = 4 Bytes
    '  'Dim lengths As IntPtr = Marshal.AllocHGlobal(CInt(lengths_size))

    '  ' Speicher für byte** (UTF8‑Pointer)
    '  Dim names_size = CType(count * Marshal.SizeOf(Of IntPtr)(), UIntPtr)
    '  Dim names = Marshal.AllocHGlobal(CType(names_size, Int32))

    '  ' Speicher für int* (Längen)
    '  Dim lengths_size = CType(count * Marshal.SizeOf(Of Int32)(), UIntPtr)
    '  Dim lengths = Marshal.AllocHGlobal(CType(lengths_size, Int32))

    '  For i As Int32 = 0 To count - 1
    '    Dim text = If(packlist(i), String.Empty)
    '    Dim utf8 = Encoding.UTF8.GetBytes(text)

    '    ' Speicher für UTF‑8 + Nullterminator
    '    Dim str_ptr = Marshal.AllocHGlobal(utf8.Length + 1)

    '    ' Bytes kopieren
    '    Marshal.Copy(utf8, 0, str_ptr, utf8.Length)

    '    ' Nullterminator setzen
    '    Marshal.WriteByte(str_ptr, utf8.Length, 0)

    '    ' Pointer in names[i] schreiben
    '    Marshal.WriteIntPtr(names, i * IntPtr.Size, str_ptr)

    '    ' Länge in lengths[i] schreiben
    '    Marshal.WriteInt32(lengths, i * 4, utf8.Length)
    '  Next

    '  Return (names, lengths, count)
    'End Function

    Private Shared Function FileEqualsSpec(filelist As String(), outputfolder As String) As Boolean
      'Special Tester
      For Each file In filelist
        If Not FileEquals(file, Path.Combine(outputfolder, file)) Then Return False
      Next
      Return True
    End Function

    Private Shared Function FileEqualsSpec(srcfolder As String, destfolder As String) As Boolean
      Dim left = New DirectoryInfo(srcfolder).GetFiles("*.*", SearchOption.AllDirectories).OrderBy(Function(x) x.FullName).ToArray()

      Dim right = New DirectoryInfo(destfolder).GetFiles("*.*", SearchOption.AllDirectories).OrderBy(Function(x) x.FullName).ToArray()

      If EqualitySpec(left, right, srcfolder) Then
        Dim length = left.Length
        For i = 0 To length - 1
          If Not FileEquals(left(i).FullName, right(i).FullName) Then Return False
        Next
        Return True
      End If

      Return False
    End Function

    Private Shared Function FileEquals(left As String, right As String) As Boolean
      Using fleft = New FileStream(left, FileMode.Open, FileAccess.Read)
        Using fright = New FileStream(right, FileMode.Open, FileAccess.Read)
          Dim sha = SHA512.Create()
          Return sha.ComputeHash(fleft).SequenceEqual(sha.ComputeHash(fright))
        End Using
      End Using
    End Function

    Private Shared Function EqualitySpec(
      left As FileInfo(), right As FileInfo(), srcfolder As String) As Boolean
      If Not left.Length = right.Length Then Return False

      Dim length = left.Length
      For i = 0 To length - 1
        Dim idx1 = left(CInt(i)).FullName.IndexOf(srcfolder, StringComparison.Ordinal)
        Dim idx2 = right(CInt(i)).FullName.IndexOf(srcfolder, StringComparison.Ordinal)
        If Not left(CInt(i)).FullName.Substring(idx1).SequenceEqual(right(CInt(i)).FullName.Substring(idx2)) Then Return False
      Next
      Return True
    End Function

    Private Shared Async Function PreparationAsync(srcfolder As String) As Task
      Console.WriteLine($"A SourceFolder with many files and directories is created.")
      Dim packlist = {"data.txt", "data2.txt", "data3.txt"}
      Await CreateRngFolders(srcfolder, packlist)
    End Function

    Private Shared Async Function CreateRngFolders(basefolder As String, files As String()) As Task
      Dim rand = Random.[Shared]

      If Directory.Exists(basefolder) Then Directory.Delete(basefolder, True)

      Directory.CreateDirectory(basefolder)
      Dim file = files(rand.[Next](files.Length))
      Dim dest = Path.Combine(basefolder, file)
      Await CopyFileAsync(file, dest, overwrite:=True)
      For i = 0 To 2
        Dim subroot = Path.Combine(basefolder, RngFolderName(8))
        Directory.CreateDirectory(subroot)

        Dim current = subroot
        file = files(rand.[Next](files.Length))
        dest = Path.Combine(current, file)
        Await CopyFileAsync(file, dest, overwrite:=True)
        For depth = 0 To 2
          current = Path.Combine(current, RngFolderName(8))
          Directory.CreateDirectory(current)

          Dim c = rand.[Next](files.Length) + 1
          For j = 0 To c - 1
            If rand.NextDouble() < 0.95 Then ' 95% Chance
              file = files(rand.[Next](files.Length))
              dest = Path.Combine(current, file)
              Await CopyFileAsync(file, dest, overwrite:=True)
            End If
          Next
        Next
      Next
      Console.WriteLine()
    End Function

    Private Shared Async Function CopyFileAsync(sourcepath As String, destinationPath As String, Optional overwrite As Boolean = False) As Task
      If Not File.Exists(sourcepath) Then Throw New FileNotFoundException("Source file not found.", sourcepath)

      If File.Exists(destinationPath) AndAlso overwrite Then File.Delete(destinationPath)
      Using fsin = New FileStream(sourcepath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize:=4096, useAsync:=True)
        Using fsout = New FileStream(destinationPath, FileMode.CreateNew, FileAccess.Write, FileShare.None, bufferSize:=4096, useAsync:=True)
          Await fsin.CopyToAsync(fsout)
        End Using
      End Using
    End Function

    Private Shared Function RngFolderName(size As Int32) As String
      Return Guid.NewGuid().ToString("N").Substring(0, size)
    End Function

    Private Shared Sub Finish(srcfolder As String)
      Console.WriteLine($"The source directory is deleted again.")
      If Directory.Exists(srcfolder) Then Directory.Delete(srcfolder, True)
      Console.WriteLine()
    End Sub

    Private Shared Function SumFileSizesFolder(foldername As String) As (Size As Int64, FCnt As Int32)
      Dim files = Directory.GetFiles(foldername, "*.*", SearchOption.AllDirectories)

      Return SumFileSizes(files)
    End Function

    Private Shared Function SumFileSizes(files As String()) As (Size As Int64, FCnt As Int32)
      Dim result = 0L
      For Each fname In files
        Dim fi = New FileInfo(fname)
        If Not fi.Exists Then Throw New FileNotFoundException(NameOf(files))
        result += fi.Length
      Next

      Return (result, files.Length)
    End Function

    Private Shared Function ToIntPtrUtf8(str As String) As (Ptr As IntPtr, Length As Int32)
      Dim utf8 = Encoding.UTF8.GetBytes(str)
      Dim utf8_ptr = Marshal.AllocHGlobal(utf8.Length + 1)

      Marshal.Copy(utf8, 0, utf8_ptr, utf8.Length)
      Marshal.WriteByte(utf8_ptr, utf8.Length, 0)

      Return (utf8_ptr, utf8.Length)
    End Function

    Private Shared Function ToIntPtr(bytes As Byte()) As IntPtr
      Dim bytes_ptr = Marshal.AllocHGlobal(bytes.Length + 1)

      Marshal.Copy(bytes, 0, bytes_ptr, bytes.Length)
      Marshal.WriteByte(bytes_ptr, bytes.Length, 0)

      Return bytes_ptr
    End Function
  End Class
End Namespace