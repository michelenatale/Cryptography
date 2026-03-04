# C-Abi-Bright-Aot

C-Abi-Bright-Aot is a high‑performance, NativeAOT‑compatible C ABI interface for cryptographically secure random number generation, byte generation, and modern encryption algorithms.  
It provides a stable, unmanaged‑friendly API that can be consumed from any language supporting the C ABI (C, C++, Rust, Go, Zig, Python, etc.).

This module is part of the larger **Cryptography** repository but is released as a standalone component.

---

## ✨ Features

### 🔐 Cryptographically secure random number generation
- `NextCrypto*` — single values (Bool, Byte, Int32, Int64, Single, Double, Decimal)
- `RngCrypto*` — arrays of random values
- Range variants: `NextCryptoX(min, max)` and `RngCryptoX(min, max)`
- Fully deterministic value ranges  
  (e.g., `RngCryptoDecimal(size, max)` guarantees `0 ≤ value < max`)

### ⚙️ NativeAOT C ABI
- Exported via `[UnmanagedCallersOnly]`
- Callable from any C‑ABI compatible language
- No .NET runtime required
- Zero‑GC interop: memory is explicitly allocated and freed

### 🔒 Modern encryption
- AES (CBC)
- AES‑GCM (AEAD)
- ChaCha20‑Poly1305 (AEAD)
- File and byte‑level APIs

### 🚀 Performance
The library has been stress‑tested and shows extremely stable performance:

| API | 10,000 rounds | Avg per round |
|-----|---------------|----------------|
| NextCryptoInt32 | 15 ms | 0.0015 ms |
| RngCryptoInt32 | 376 ms | 0.0376 ms |
| NextCryptoDecimal | 15 ms | 0.0015 ms |
| RngCryptoDecimal | 764 ms | 0.0764 ms |
| AES‑GCM Bytes | 31 ms | 3 ms |
| ChaCha20‑Poly1305 Bytes | 55 ms | 5 ms |

The results are consistent with no outliers.

---

## 📦 Memory management

All Rng‑methods that return arrays allocate unmanaged memory:

```c
CError rng_crypto_int32_aot(int size, int32_t** out_ptr);
```

The caller must:

Free the memory using FreeBuffer(ptr)

Convert the buffer into the desired managed structure (Int32[], Double[], Decimal[], etc.)

Example in C#:
```
var err = Native.RngCryptoInt32Aot(size, out var ptr);
AssertError(err);

var values = ToInt32Array(ptr, size);
Native.FreeBuffer(ptr);
```

---

## 🧪 Tests and stability
The project includes extensive tests:

Range validation

Interop roundtrip tests (Decimal, Double, Single)

Performance tests

AES / AES‑GCM / ChaCha20‑Poly1305 validation

All tests run stable over thousands of iterations.

---

## 📁 Project structure

```
C-Abi-Bright-Aot/
 ├─ Docs /
 │  └─ ConsoleOutputTestRngCryptoBoolAot.txt
 ├─ Src / 
 │  ├─ CAbiBridge.Aot
 │  ├─ CAbiBridge.Crypto
 │  └─ CAbiBridge.Utils
 ├─ Tests /  
 │  └─ C-Abi-Bridge.Aot.Cpp.Tests
 │  └─ C-Abi-Bridge.Aot.Vb.Tests
 │  └─ CAbiBridge.Crypto.Tests
 │  └─ CAbiBridgeAot.Tests
 ├─ README.md 
 ├─ CHANGELOG.md 
 └─ C-Abi-Bridge.slnx
```

---

## 🛠 Example: Calling from C

```
#include "bright.h"

int main() {
    int size = 16;
    int32_t* data = NULL;

    CError err = rng_crypto_int32_aot(size, &data);
    if (err.error_code != 0) return err.error_code;

    for (int i = 0; i < size; i++)
        printf("%d\n", data[i]);

    free_buffer(data);
    return 0;
}
```

---

## 📄 License

This project inherits the license of the parent repository Cryptography.

---

## 🏷 Versioning

Version numbers follow:
```
MAJOR.MINOR.PATCH
```

Examples:

- **`1.0.0`** — first stable release
- **`1.1.0`** — new features, no breaking changes
- **`1.1.1`** — bugfix release

---

## ⭐ Changelog

The full version history is available in  
[CHANGELOG.md](https://github.com/michelenatale/Cryptography/blob/main/C-Abi-Bright-Aot/CHANGELOG.md).

---

## 📦 Releases

Releases are published through GitHub and include:

- Compiled NativeAOT binaries
- Header files (bright.h)
- Documentation
- Changelog

---

## 📬 Maintainer

**Michele Natale**

GitHub: [https://github.com/michelenatale](https://github.com/michelenatale)

---



