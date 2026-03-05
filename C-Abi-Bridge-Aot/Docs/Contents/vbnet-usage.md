# 🧩 VB.NET Usage Guide

This guide explains how to call the C‑Abi‑Bridge‑Aot native library from VB.NET.

VB.NET can interoperate with native code just like C#, but there is one important difference:

VB.NET does not currently support LibraryImport, so this guide uses DllImport.

---

## ⚠️ About LibraryImport Support in VB.NET
The modern `LibraryImport` attribute (used in C# for NativeAOT‑optimized P/Invoke) is **not fully supported in VB.NET**.

The VB.NET compiler does not yet integrate with the required source generators.

Because of this limitation:
- `LibraryImport` may fail to compile
- partial methods are not generated
- NativeAOT‑optimized stubs are not emitted

For VB.NET, the correct and stable approach is:

```vbnet
<DllImport("bridge", EntryPoint:="...")>
```
This is fully compatible with NativeAOT and works reliably across all platforms.

We are optimistic that future .NET versions may bring LibraryImport support to VB.NET.
If that happens, this documentation will be updated accordingly.

---

## Importing Native Functions

```vbnet
Imports System.Runtime.InteropServices

Public Module Bridge

    <DllImport(DllName, EntryPoint:="next_crypto_int32_aot")>
    Public Function NextCryptoInt32Aot(ByRef err As CError) As Int32
    End Function

    <DllImport(DllName, EntryPoint:="rng_crypto_int32_aot")>
    Public Function RngCryptoInt32Aot(size As Int32, <Out> ByRef ptr As IntPtr) As CError
    End Function

    <DllImport("bridge", EntryPoint:="free_buffer")>
    Public Sub FreeBuffer(ptr As IntPtr)
    End Sub

End Module
```

---

## Decimal Structure

```vbnet
<StructLayout(LayoutKind.Sequential)>
Public Structure Decimal128
    Public Flags As UInteger
    Public Hi As UInteger
    Public Mid As UInteger
    Public Low As UInteger
End Structure
```

This matches the exact memory layout of .NET decimal used by the C ABI.

---

## Example: Getting a Single Random Int32

```vbnet
Dim err As CError
Dim value = Native.NextCryptoInt32Aot(err)

If err.ErrorCode = 0 Then
    Console.WriteLine($"Value: {value}")
Else Console.WriteLine($"Error: {err.ErrorCode}")
End If
```

--- 

## Example: Getting an Array of Int32

```vbnet
Dim ptr As IntPtr = IntPtr.Zero
Dim err = Bridge.RngCryptoInt32Aot(128, ptr)

If err.ErrorCode = 0 Then
    Dim arr(127) As Integer
    Marshal.Copy(ptr, arr, 0, 128)

    ' Always free the buffer returned by the native library
    Bridge.FreeBuffer(ptr)

    For Each n In arr
        Console.WriteLine(n)
    Next
Else
    Console.WriteLine($"Error: {err.ErrorCode}")
End If
```

--- 

## Memory Ownership

All buffers returned by the native library must be released using:
```vbnet
Bridge.FreeBuffer(ptr)
```

Do not use:
- Marshal.FreeHGlobal
- Marshal.FreeCoTaskMem
- free()
- delete

The memory is allocated inside the NativeAOT runtime and must be freed by the matching allocator.

--- 

