# 📘 Cross‑Language Interop Guide  

This document provides practical examples for consuming the **C‑Abi‑Bridge‑Aot** NativeAOT library from various programming languages.  

All examples use the exported C ABI functions defined in `API.md`.

---

## 1. Overview

The library exposes a stable C ABI and can be consumed from any language that supports:

- `extern "C"`
- `__cdecl` calling convention
- Flat symbol names (no mangling)

Supported languages include:

- C / C++
- C#
- VB.NET
- Rust
- Go
- Zig
- Python (ctypes / cffi)
- Java (JNI)
- Many others

---

## 2. C++ Interop

### 2.1 Linking the Import Library

Add to your `.vcxproj`:

```xml
<AdditionalLibraryDirectories>$(SolutionDir)Build\Artifacts</AdditionalLibraryDirectories>
<AdditionalDependencies>C-Abi-Bridge.Aot.lib</AdditionalDependencies>
```

### 2.2 Example Usage

```cpp
#include "bridge-aot.h"

int main()
{
    uint8_t buffer[32];
    int result = fill_crypto_bytes_aot(buffer, 32);

    if (result == 0)
        printf("Random bytes generated.\n");
}
```

---

## 3. C# Interop (P/Invoke)

### 3.1 Import Declaration

```
[LibraryImport("C-Abi-Bridge.Aot.N.dll")]
public static extern int fill_crypto_bytes_aot(byte[] buffer, int size);
```

### 3.2 Example Usage

```
var buf = new byte[32];
fill_crypto_bytes_aot(buf, buf.Length);
```

---

## 4. VB.NET Interop

```
<DllImport("C-Abi-Bridge.Aot.N.dll")>
Public Shared Function fill_crypto_bytes_aot(
    buffer As Byte(),
    size As Integer
) As Integer
End Function
```

---

## 5. Rust Interop

### 5.1 FFI Declaration

```
#[link(name = "C-Abi-Bridge.Aot")]
extern "C" {
    fn fill_crypto_bytes_aot(buffer: *mut u8, size: i32) -> i32;
}
```

### 5.2 Example Usage

```
let mut buf = [0u8; 32];
unsafe {
    fill_crypto_bytes_aot(buf.as_mut_ptr(), buf.len() as i32);
}
```

---

## 6. Go Interop (cgo)

### 6.1 cgo Header

```
/*
#cgo LDFLAGS: -LC:/path/to/Artifacts -lC-Abi-Bridge.Aot
#include <stdint.h>

int fill_crypto_bytes_aot(uint8_t* buffer, int32_t size);
*/
import "C"
```

### 6.2 Example Usage

```
buf := make([]byte, 32)
C.fill_crypto_bytes_aot((*C.uint8_t)(&buf[0]), C.int(len(buf)))
```

---

## 7. Zig Interop

Zig can import the `.def` file directly:
```zig
const cabi = @cImport({
    @cInclude("bridge-aot.h");
});
```

Usage:
```zig
var buf: [32]u8 = undefined;
_ = cabi.fill_crypto_bytes_aot(&buf, 32);
```

---

## 8. Python Interop (ctypes)

```
from ctypes import *

dll = CDLL("C-Abi-Bridge.Aot.dll")

buf = (c_ubyte * 32)()
dll.fill_crypto_bytes_aot(buf, 32)
```

## 9. Python Interop (cffi)

```
from cffi import FFI
ffi = FFI()

lib = ffi.dlopen("C-Abi-Bridge.Aot.dll")

buf = ffi.new("uint8_t[32]")
lib.fill_crypto_bytes_aot(buf, 32)
```

## 10. Java Interop (JNI)

```
public class Crypto {
    static {
        System.loadLibrary("C-Abi-Bridge.Aot");
    }

    public static native int fill_crypto_bytes_aot(byte[] buffer, int size);
}
```

## 11. Notes

- All examples assume the DLL is in the process working directory.
- C++ requires the .lib file from Build/Artifacts.
- Other languages load the DLL directly.
- All functions return 0 on success and negative values on error.

---

