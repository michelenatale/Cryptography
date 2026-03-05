# 🧩 Using the C ABI from C#

This document explains how to call the C-Abi-Bridge-Aot module from C# using P/Invoke.

## Import Example

```
[DllImport("bridge", EntryPoint = "next_crypto_int32_aot")]
public static extern CError NextCryptoInt32Aot(out int value);
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
[DllImport("bridge")]
public static extern void free_buffer(IntPtr ptr);
```

## Example Usage

```
var err = NextCryptoInt32Aot(out var value);
Console.WriteLine(value);
```
```
var err = NextCryptoInt32Aot(out var value);
Console.WriteLine(value);
```

---

# 🎬 **video/**

[Docs/video/c-abi-bridge-aot.mp4](Docs/c-abi-bridge-aot_3_24671.mp4)

---
