# 🧩 Using the C ABI from C#

This document explains how to call the C-Abi-Bridge-Aot module from C# using P/Invoke.

## Import Example

```
[LibraryImport("bridge", EntryPoint = "next_crypto_int32_aot")]
public static partial CError NextCryptoInt32Aot(out int value);
```

## Decimal Interop

```[StructLayout(LayoutKind.Sequential)]
public struct Decimal128 {
    public uint Flags;
    public uint Hi;
    public uint Mid;
    public uint Low;
}
```

## Freeing Memory

```
[LibraryImport("bridge", EntryPoint = "free_buffer")]
public static partial void FreeBuffer(IntPtr ptr);
```

## Example Usage

```
var err = NextCryptoInt32Aot(out var value);
Console.WriteLine(value);
```
```
var err = Native.RngCryptoInt32Aot(size, out var ptr);
AssertError(err);

var values = ToInt32Array(ptr, size);
Native.FreeBuffer(ptr);
```

---

# 🎬 **video/**

[Docs/video/c-abi-bridge-aot.mp4](c-abi-bridge-aot_3_24671.mp4)

---
