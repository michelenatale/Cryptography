# VB.NET Usage Guide

This document shows how to call the C-Abi-Bridge-Aot native library from VB.NET using `LibraryImport`.

## Importing Native Functions

```vbnet
Imports System.Runtime.InteropServices

Public Module Bridge

    <LibraryImport("bridge", EntryPoint:="next_crypto_int32_aot")>
    Public Function NextCryptoInt32Aot(ByRef value As Integer) As CError
    End Function

    <LibraryImport("bridge", EntryPoint:="free_buffer")>
    Public Sub FreeBuffer(ptr As IntPtr)
    End Sub

End Module
```

## Decimal Structure

```
<StructLayout(LayoutKind.Sequential)>
Public Structure Decimal128
    Public Flags As UInteger
    Public Hi As UInteger
    Public Mid As UInteger
    Public Low As UInteger
End Structure
```

## Example: Getting a Single Random Int32

```
Dim value As Integer
Dim err = Bridge.NextCryptoInt32Aot(value)

If err.ErrorCode = 0 Then
    Console.WriteLine($"Value: {value}")
End If
```

## Example: Getting an Array of Int32

```
Dim ptr As IntPtr = IntPtr.Zero
Dim err = Bridge.RngCryptoInt32Aot(128, ptr)

If err.ErrorCode = 0 Then
    Dim arr(127) As Integer
    Marshal.Copy(ptr, arr, 0, 128)
    Bridge.FreeBuffer(ptr)
End If
```
