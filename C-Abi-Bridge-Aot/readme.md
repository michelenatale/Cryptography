# C-Abi-Bridge-Aot

C-Abi-Bridge-Aot is a high‑performance, [NativeAOT](https://learn.microsoft.com/en-us/dotnet/core/deploying/native-aot)‑compatible [C ABI interface](https://en.wikipedia.org/wiki/Application_binary_interface) for [cryptographically secure random number generation](https://en.wikipedia.org/wiki/Cryptographically_secure_pseudorandom_number_generator), byte generation, and [modern encryption algorithms](https://en.wikipedia.org/wiki/Cryptography).  

It provides a stable, unmanaged‑friendly API that can be consumed from any language supporting the C ABI ([C](https://en.wikipedia.org/wiki/C_(programming_language)), [C++](https://en.wikipedia.org/wiki/C%2B%2B), [Rust](https://en.wikipedia.org/wiki/Rust_(programming_language)), [Go](https://en.wikipedia.org/wiki/Go_(programming_language)), [Zig](https://en.wikipedia.org/wiki/Zig_(programming_language)), [Python](https://en.wikipedia.org/wiki/Python_(programming_language)), etc.).

This module is part of the larger **Cryptography** repository but is released as a standalone component.

---

## 🛠️ Project Status

This module is currently being prepared for its first public release.  

It will be published as part of the broader **Cryptography** repository and will serve as the foundation for future cross‑language interoperability work.

A [demonstration video](#-video-demonstration) is available in the `Docs` folder and as a direct download below. 

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

### 🔒 Modern Encryption
- AES (CBC)
- AES‑GCM (AEAD)
- [ChaCha20‑Poly1305 (AEAD)](https://en.wikipedia.org/wiki/ChaCha20-Poly1305)
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

## 📦 Memory Management

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

## 🧪 Tests and Stability
The project includes extensive tests:

Range validation

Interop roundtrip tests (Decimal, Double, Single)

Performance tests

AES / AES‑GCM / ChaCha20‑Poly1305 validation

All tests run stable over thousands of iterations.

---

## 📁 Project Structure

```
C-Abi-Bridge-Aot/
 ├─ Docs /
 │  ├─ ...
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
#include "bridge.h"

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

## 📌 Changelog

The full version history is available in  
[CHANGELOG.md](CHANGELOG.md).

---

## 📦 Releases

Releases are published through GitHub and include:

- Compiled NativeAOT binaries
- Header files (bridge.h)
- Documentation
- Changelog

---

## 📬 Maintainer

**Michele Natale**

GitHub: [https://github.com/michelenatale](https://github.com/michelenatale)

---

## 🎬 Video Demonstration

A demonstration video is available in the "Docs" folder and as a link below:

- **c-abi-bridge-aot.mp4**

It shows the test execution, the NativeAOT behavior, and how the C ABI interface is used from different languages.


---

## 📥 Video Mp4 - Downloads

**Publish**: 

➡️ [c-abi-bridge-aot.mp4](Docs/Videos/c-abi-bridge-aot_3_24671.mp4)

---


