Option Strict On
Option Explicit On


Imports System.Text
Imports System.Numerics


Namespace michele.natale.Tests
  Partial Class TestServicesVb

    Public Shared Function Converter_2_256_LE(
    bi As BigInteger, start_base As Int32, target_base As Int32) As Byte()
      Return Converter_2_256_LE_S(bi.ToByteArray().Reverse().ToArray(), start_base, target_base)
    End Function

    Public Shared Function Converter_2_256_LE_S(
    bytes As Byte(), start_base As Int32, target_base As Int32) As Byte()

      Dim out_ptr As IntPtr = IntPtr.Zero, out_length As Int32 = 0
      Dim err = Native.Converter_2_256_LE_Aot(
      bytes, bytes.Length, start_base, target_base, out_ptr, out_length) 'base 10
      AssertError(err)

      Dim result = ToBytes(out_ptr, out_length)
      Native.FreeBuffer(out_ptr)

      Return result
    End Function

    Public Shared Function ToBaseX_2_256_LE_S(bi As BigInteger, target_base As Int32) As Byte()
      Return ToBaseX_2_256_LE_S(bi.ToString(), target_base)
    End Function

    Public Shared Function ToBaseX_2_256_LE_S(number As String, target_base As Int32) As Byte()
      Return ToBaseXUtf8_2_256_LE_S(Encoding.UTF8.GetBytes(number), target_base)
    End Function

    Public Shared Function ToBaseX_2_256_LE_S(bytes As Byte(), target_base As Int32) As Byte()
      Dim out_ptr As IntPtr = IntPtr.Zero, out_length As Int32 = Nothing
      Dim err = Native.ToBaseX_2_256_LE_Aot(
      bytes, bytes.Length, target_base, out_ptr, out_length) 'base 10
      AssertError(err)

      Dim result = ToBytes(out_ptr, out_length)
      Native.FreeBuffer(out_ptr)

      Return result
    End Function

    Public Shared Function ToBaseXUtf8_2_256_LE_S(bytes As Byte(), target_base As Int32) As Byte()
      Dim out_ptr As IntPtr = IntPtr.Zero, out_length As Int32 = Nothing
      Dim err = Native.ToBaseXUtf8_2_256_LE_Aot(
      bytes, bytes.Length, target_base, out_ptr, out_length) 'base 10
      AssertError(err)

      Dim result = ToBytes(out_ptr, out_length)
      Native.FreeBuffer(out_ptr)

      Return result
    End Function

    Public Shared Function FromBaseX_2_256_LE_S(bytes As Byte(), from_base_x As Int32) As Byte()
      Dim out_ptr As IntPtr = IntPtr.Zero, out_length As Int32 = Nothing
      Dim err = Native.FromBaseX_2_256_LE_Aot(
      bytes, bytes.Length, from_base_x, out_ptr, out_length) 'base 10
      AssertError(err)

      Dim result = ToBytes(out_ptr, out_length)
      Native.FreeBuffer(out_ptr)

      Return result
    End Function

    Public Shared Function RngBases_2_256() As (StartBase As Int32, TargetBase As Int32)
      Dim targetbase As Int32
      Dim rand = Random.Shared
      Dim startbase = rand.Next(2, 256)

      While True
        targetbase = rand.Next(2, 256)
        If startbase <> targetbase Then Exit While
      End While

      Return (startbase, targetbase)
    End Function

  End Class
End Namespace