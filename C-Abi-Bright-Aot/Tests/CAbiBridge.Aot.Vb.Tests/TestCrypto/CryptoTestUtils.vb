Option Strict On
Option Explicit On

Imports System.IO
Imports michele.natale.CAbiBridge
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Numerics
Imports System.Runtime.CompilerServices

Namespace michele.natale.Tests
  Friend Module CryptoTestUtils

    Friend Sub SetRngFileData(filename As String, size As Int32)
      Using fsout = New FileStream(filename, FileMode.Create, FileAccess.Write)
        Dim length = If(size < 1024 * 1024, size, 1024 * 1024)

        While length > 0
          fsout.Write(RngBytes(length))
          size -= length
          length = If(size < 1024 * 1024, size, 1024 * 1024)
        End While
      End Using
    End Sub

    Friend Async Function SetRngFileDataAsync(filename As String, size As Int32) As Task
      Dim bufferSize = 1024 * 1024

      Dim options = New FileStreamOptions With
      {
       .Mode = FileMode.Create, .Access = FileAccess.Write,
       .Share = FileShare.None, .BufferSize = bufferSize,
       .Options = FileOptions.Asynchronous
      }

      Using fsout As New FileStream(filename, options)

        While size > 0
          Dim chunk = Math.Min(size, bufferSize)
          Dim data = RngBytes(chunk)

          Await fsout.WriteAsync(data.AsMemory(0, chunk))

          size -= chunk
        End While

      End Using
    End Function


    Friend Function RngBytes(size As Int32) As Byte()
      Dim rand = Random.Shared
      Dim result = New Byte(size - 1) {}
      rand.NextBytes(result)
      If result(0) = 0 Then result(0) += CByte(1)
      Return result
    End Function

    Friend Sub AssertError(err As CError)
      If err.error_code <> 0 Then
        Dim msg = Marshal.PtrToStringAnsi(err.message)
        Throw New ArgumentException(msg)
      End If
    End Sub

    Friend Function ToBytes(bytes As IntPtr, length As Int32) As Byte()
      Dim result = New Byte(length - 1) {}
      Marshal.Copy(bytes, result, 0, length)
      Return result
    End Function


    Friend Function ToInts(Of T As INumber(Of T))(ptr As IntPtr, length As Int32) As T()
      'For uint8 to uint64
      Dim szt = Unsafe.SizeOf(Of T)()
      Dim bytes = ToBytes(ptr, length * szt)

      Dim result = New T(length - 1) {}
      Buffer.BlockCopy(bytes, 0, result, 0, length * szt)

      Return result
    End Function

    Public Function ToBools(ptr As IntPtr, length As Int32) As Boolean()
      Dim result = New Boolean(length - 1) {}
      For i = 0 To length - 1
        result(i) = Marshal.ReadByte(ptr, i) <> 0
      Next

      Return result
    End Function

    Friend Function ToFloats(Of T As IFloatingPoint(Of T))(ptr As IntPtr, length As Int32) As T()
      'For double and float
      Dim tsz As Int32 = Unsafe.SizeOf(Of T)() * length

      Dim bytes = New Byte(tsz - 1) {}
      Marshal.Copy(ptr, bytes, 0, tsz)

      Dim result = New T(length - 1) {}
      Buffer.BlockCopy(bytes, 0, result, 0, tsz)

      Return result
    End Function

    Friend Function ToDecimals(ptr As IntPtr, length As Int32) As Decimal()
      Dim result = New Decimal(length - 1) {}

      For i = 0 To length - 1
        Dim offset = i * 16

        Dim lo = Marshal.ReadInt32(ptr, offset + 0)
        Dim mid = Marshal.ReadInt32(ptr, offset + 4)
        Dim hi = Marshal.ReadInt32(ptr, offset + 8)
        Dim flags = Marshal.ReadInt32(ptr, offset + 12)

        result(i) = New Decimal(lo, mid, hi, (flags And &H80000000) <> 0, CByte(flags >> 16 And &H7F))
      Next

      Return result
    End Function


    'Friend Function ToCharsUtf8(ptr As IntPtr, length As Integer) As Char()
    '  Dim bytes = New Byte(length - 1) {}
    '  Marshal.Copy(ptr, bytes, 0, length)

    '  Return Encoding.UTF8.GetChars(bytes)
    'End Function

    'Friend Function ToCharsAscii(ptr As IntPtr, length As Integer) As Char()
    '  Dim result = New Char(length - 1) {}
    '  For i = 0 To length - 1
    '    result(i) = CChar(Marshal.ReadByte(ptr, i))
    '  Next

    '  Return result
    'End Function

    'Friend Function ToCharsUtf16(ptr As IntPtr, length As Integer) As Char()
    '  Dim result = New Char(length - 1) {}
    '  For i = 0 To length - 1
    '    result(i) = CChar(Marshal.ReadInt16(ptr, i * 2))
    '  Next

    '  Return result
    'End Function

  End Module
End Namespace
