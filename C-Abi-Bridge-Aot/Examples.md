# 📘 Practical Usage Examples  

This document provides real‑world examples for using the **C‑Abi‑Bridge‑Aot** NativeAOT library from multiple programming languages.

All examples assume:

- The DLL is named `C-Abi-Bridge.Aot.N.dll` (for .NET consumers)
- The C++ import library (`.lib`) is available in `Build/Artifacts`
- The C ABI functions are documented in `API.md`

---

# 1. Random Bytes

## 1.1 C Example

```c
auto err = fill_crypto_bytes_aot(bytes.data(), (int)bytes.size());
assert_error(err);
```

## 1.2 C# Example

```
var size = rand.Next(128, 512);
var err = Native.RngCryptoBytesAot(size, out var out_ptr);
AssertError(err);
```

---

# 2. SHA‑256 Hashing

## 2.1 C Example

```
var err = fill_crypto_bytes_aot(bytes, bytes.Length);
AssertError(err);
```

## 2.2 C# Example

```
var err = fill_crypto_bytes_aAot(bytes, bytes.Length);
AssertError(err);
```

---

# 3. AES‑CBC Encryption

## 3.1 C Example

```
auto err = aes_encrypt_aot(
  plain.data(), plain.size(),
  key.data(), key.size(),
  associat.data(), associat.size(),
  &cipher_ptr, &cipher_len);
assert_error(err);
```

---

# 4. AES‑GCM Encryption

## 4.1 C Example

```
auto err = aes_gcm_encrypt_aot(
  plain.data(), (int)plain.size(),
  key.data(), (int)key.size(),
  associat.data(), (int)associat.size(),
  &cipher_ptr, &cipher_len);
assert_error(err);
```

---

# 5. ChaCha20‑Poly1305 Encryption

## 5.1 C Example

```
auto err = chacha20_poly1305_encrypt_aot(
  plain.data(), (int)plain.size(),
  key.data(), (int)key.size(),
  associat.data(), (int)associat.size(),
  &cipher_ptr, &cipher_len);
assert_error(err);
```

---

# 6. Base64 Encoding

## 6.1 C Example
   
```
auto err = chacha20_poly1305_encrypt_aot(
  plain.data(), (int)plain.size(),
  key.data(), (int)key.size(),
  associat.data(), (int)associat.size(),
  &cipher_ptr, &cipher_len);
assert_error(err);
```

## 6.2 Python Example (ctypes)

```
from ctypes import *

dll = CDLL("C-Abi-Bridge.Aot.dll")

out = (c_ubyte * 128)()
dll.to_base_64_utf8_aot(input_buf, len(input_buf), out, 128)
```

---

# 7. C++ Example (Full Flow)

```
#include "cabi_exp_imp.h"

int main() {
  auto plain = rng_bytes(plain_size);
  auto associat = rng_bytes(associat_size);
  std::vector<uint8_t> key(Services::AES_KEY_SIZE);

  int cipher_len = 0;
  void* cipher_ptr = nullptr;

  auto err = aes_encrypt_aot(
    plain.data(), (int)plain.size(),
    key.data(), (int)key.size(),
    associat.data(), (int)associat.size(),
    &cipher_ptr, &cipher_len);
  assert_error(err);
}
```

---

# 8. Rust Example

```
extern "C" {
    fn sha_256_aot(input: *const u8, len: i32, out: *mut u8, out_len: i32) -> i32;
}

let mut out = [0u8; 32];
unsafe {
    sha_256_aot(data.as_ptr(), data.len() as i32, out.as_mut_ptr(), 32);
}
```

# 9. Go Example (cgo)

```
buf := make([]byte, 32)
C.crypto_random_bytes_aot((*C.uint8_t)(&buf[0]), C.int(len(buf)))
```

# 10. Python Example (ctypes)

```
from ctypes import *

dll = CDLL("C-Abi-Bridge.Aot.dll")

buf = (c_ubyte * 32)()
dll.crypto_random_bytes_aot(buf, 32)
```

# 11. VB.NET Example

```
Dim buf(31) As Byte
Dim rc = crypto_random_bytes_aot(buf, buf.Length)
```
