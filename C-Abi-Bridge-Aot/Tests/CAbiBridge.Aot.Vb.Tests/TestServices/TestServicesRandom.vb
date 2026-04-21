'Option Strict On
'Option Explicit On


'Imports System.Numerics
'Imports System.Runtime.CompilerServices

'Namespace michele.natale.Tests
'  Partial Class TestServices

'    <MethodImpl(MethodImplOptions.AggressiveInlining)>
'    Public Shared Function RngBaseXNumber(size As Integer, basex As Integer) As Byte()
'      Return RngInts(Of Byte)(size, 0, CByte(basex))
'    End Function

'    <MethodImpl(MethodImplOptions.AggressiveInlining)>
'    Public Shared Function RngBytes(
'      size As Integer, Optional non_zeros As Boolean = True) As Byte()
'      Dim rand = Random.Shared
'      Dim result = New Byte(size - 1) {}

'      If Not non_zeros Then
'        rand.NextBytes(result)
'        Return result
'      End If

'      For i = 0 To size - 1
'        result(i) = CByte(rand.Next(1, 256))
'      Next

'      Return result
'    End Function

'    <MethodImpl(MethodImplOptions.AggressiveInlining)>
'    Public Shared Function RngInt(Of T As {INumber(Of T), INumberBase(Of T), IMinMaxValue(Of T)})() As T
'      Dim result = New T(0) {}
'      Dim zero = result.First()
'      RngInts(result, zero, DirectCast(GetMaxValue(GetType(T)), T))
'      Return result.First()
'    End Function

'    <MethodImpl(MethodImplOptions.AggressiveInlining)>
'    Public Shared Function RngInt2(Of T As {INumber(Of T), INumberBase(Of T), IMinMaxValue(Of T)})() As T
'      Dim result = New T(0) {}
'      Dim zero = result.First()
'      RngInts(result, zero, DirectCast(GetMaxValue(GetType(T)), T))
'      Return result.First()
'    End Function


'    <MethodImpl(MethodImplOptions.AggressiveInlining)>
'    Public Shared Function RngInt(Of T As {INumber(Of T), INumberBase(Of T)})(min As T, max As T) As T
'      Dim result = New T(0) {}
'      RngInts(result, min, max)
'      Return result.First()
'    End Function

'    <MethodImpl(MethodImplOptions.AggressiveInlining)>
'    Public Shared Function RngInts(Of T As {INumber(Of T), INumberBase(Of T)})(size As Integer, min As T, max As T) As T()
'      If size = 0 Then Return Array.Empty(Of T)()
'      Dim result = New T(size - 1) {}
'      RngInts(result, min, max)
'      Return result
'    End Function

'    <MethodImpl(MethodImplOptions.AggressiveInlining)>
'    Private Shared Sub RngInts(Of T As {INumber(Of T), INumberBase(Of T)})(ints As T(), min As T, max As T)
'      If IsNullOrEmpty(ints) Then Return
'      ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(min, max)

'      Dim d = max - min
'      Dim length = ints.Length
'      Dim type_bits = Unsafe.SizeOf(Of T)()
'      Dim bytes = RngBytes(type_bits * length, True)
'      'DataTypes Int128 and UInt128 are not yet recognized as primitives. 
'      If GetType(T).IsPrimitive OrElse GetType(T) Is GetType(Int128) OrElse GetType(T) Is GetType(UInt128) Then
'        For i = 0 To length - 1
'          Dim tmp = Unsafe.ReadUnaligned(Of T)(bytes(i * type_bits))
'          ints(i) = T.Abs(tmp)
'          ints(i) = ints(i) Mod d
'          ints(i) += min
'        Next
'      End If
'    End Sub



'    Public Shared Function GetMaxValue(t As Type) As Object
'      Select Case t
'        Case GetType(Byte) : Return Byte.MaxValue
'        Case GetType(SByte) : Return SByte.MaxValue

'        Case GetType(Short) : Return Short.MaxValue
'        Case GetType(UShort) : Return UShort.MaxValue

'        Case GetType(Integer) : Return Integer.MaxValue
'        Case GetType(UInteger) : Return UInteger.MaxValue

'        Case GetType(Long) : Return Long.MaxValue
'        Case GetType(ULong) : Return ULong.MaxValue

'        Case GetType(Int128) : Return Int128.MaxValue
'        Case GetType(UInt128) : Return UInt128.MaxValue

'        Case GetType(Single) : Return Single.MaxValue
'        Case GetType(Double) : Return Double.MaxValue
'        Case GetType(Decimal) : Return Decimal.MaxValue
'        Case Else
'          Throw New NotSupportedException($"Type {t.FullName} not supported.")
'      End Select
'    End Function

'  End Class
'End Namespace