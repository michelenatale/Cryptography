Option Strict On
Option Explicit On

Imports System.Numerics
Imports System.Runtime.CompilerServices

Namespace michele.natale.Tests
  Partial Class TestServicesVb

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Shared Function IsNullOrEmpty(
      Of T As {INumber(Of T), INumberBase(Of T)})(ints As T()) As Boolean
      If ints Is Nothing Then Return True
      Return ints.Length = 0
    End Function

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Shared Function TrimLast(bytes As Byte()) As Byte()
      Dim idx As Int32 = 0

      While bytes(bytes.Length - 1 - idx) = 0
        idx += 1
      End While

      Dim length As Int32 = bytes.Length
      Return bytes.AsSpan(0, length - idx).ToArray()
    End Function

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Shared Function TrimFirst(bytes As Byte()) As Byte()
      Dim idx As Int32 = 0

      While bytes(idx) = 0
        idx += 1
      End While

      Return bytes.AsSpan(idx).ToArray()
    End Function

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Shared Function Trim(bytes As Byte()) As Byte()
      Return TrimLast(TrimFirst(bytes))
    End Function
  End Class
End Namespace