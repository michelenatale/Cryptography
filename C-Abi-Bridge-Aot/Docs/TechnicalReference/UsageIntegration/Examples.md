# 📘 Practical Usage Examples  

**Status**: ✅   
**Version**: 0.2.0   
**Last Updated**: 2026.04.07

---

This document provides real‑world examples for using the **C‑Abi‑Bridge‑Aot** NativeAOT library from multiple programming languages.

All examples assume:

- The DLL is named `C-Abi-Bridge.Aot.N.dll` (for .NET consumers)
- The C++ import library (`.lib`) is available in `Build/Artifacts`
- The C ABI functions are documented in `API.md`

---

## 1. Random Bytes

### 1.1 C Example

```c
auto err = fill_crypto_bytes_aot(bytes.data(), (int)bytes.size());
assert_error(err);
```

### 1.2 C# Example

```
var size = rand.Next(128, 512);
var err = Native.RngCryptoBytesAot(size, out var out_ptr);
AssertError(err);
```

---

## 2. SHA‑256 Hashing

### 2.1 C Example

```
var err = fill_crypto_bytes_aot(bytes, bytes.Length);
AssertError(err);
```

### 2.2 C# Example

```
var err = fill_crypto_bytes_aAot(bytes, bytes.Length);
AssertError(err);
```

---

## 3. AES‑CBC Encryption

### 3.1 C Example

```
auto err = aes_encrypt_aot(
  plain.data(), plain.size(),
  key.data(), key.size(),
  associat.data(), associat.size(),
  &cipher_ptr, &cipher_len);
assert_error(err);
```

---

## 4. AES‑GCM Encryption

### 4.1 C Example

```
auto err = aes_gcm_encrypt_aot(
  plain.data(), (int)plain.size(),
  key.data(), (int)key.size(),
  associat.data(), (int)associat.size(),
  &cipher_ptr, &cipher_len);
assert_error(err);
```

---

## 5. ChaCha20‑Poly1305 Encryption

### 5.1 C Example

```
auto err = chacha20_poly1305_encrypt_aot(
  plain.data(), (int)plain.size(),
  key.data(), (int)key.size(),
  associat.data(), (int)associat.size(),
  &cipher_ptr, &cipher_len);
assert_error(err);
```

---

## 6. Base64 Encoding

### 6.1 C Example
   
```
auto err = chacha20_poly1305_encrypt_aot(
  plain.data(), (int)plain.size(),
  key.data(), (int)key.size(),
  associat.data(), (int)associat.size(),
  &cipher_ptr, &cipher_len);
assert_error(err);
```

### 6.2 Python Example (ctypes)

```
from ctypes import *

dll = CDLL("C-Abi-Bridge.Aot.dll")

### Define return type (CError = int32)
dll.to_base_64_utf8_aot.restype = c_int

### Define argument types
dll.to_base_64_utf8_aot.argtypes = [
    POINTER(c_ubyte),  # input buffer
    c_int,             # input length
    POINTER(c_ubyte),  # output buffer
    c_int              # output length
]

### Example usage
input_buf = (c_ubyte * 64)(*range(64))
out_buf = (c_ubyte * 128)()

err = dll.to_base_64_utf8_aot(input_buf, len(input_buf), out_buf, 128)

if err != 0:
    raise Exception(f"CError returned: {err}")

print(bytes(out_buf))
```

---

## 7. C++ Example (Full Flow)

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

## 8. Rust Example

```
#[repr(i32)]
#[derive(Debug, Copy, Clone, PartialEq, Eq)]
pub enum CError {
    Ok = 0,
    ArgumentError = -1,
    CryptoError = -2,
    // ...
}

extern "C" {
    fn to_base_64_utf8_aot(
        input_ptr: *const u8,
        input_len: i32,
        output_ptr: *mut u8,
        output_len: i32,
    ) -> i32; // CError as signed int32
}

fn main() {
    let input: Vec<u8> = (0..64).collect();
    let mut output = vec![0u8; 128];

    let err = unsafe {
        to_base_64_utf8_aot(
            input.as_ptr(),
            input.len() as i32,
            output.as_mut_ptr(),
            output.len() as i32,
        )
    };

    if err != 0 {
        panic!("CError returned: {}", err);
    }

    println!("Output: {:?}", output);
}
```

---

## 9. Go Example (cgo)

```
package main

/*
#cgo LDFLAGS: -LC-Abi-Bridge-Aot -lCAbiBridgeAot
#include <stdint.h>

extern int32_t to_base_64_utf8_aot(
    const uint8_t* input_ptr,
    int32_t input_len,
    uint8_t* output_ptr,
    int32_t output_len
);
*/
import "C"
import (
    "fmt"
)

func main() {
    input := make([]C.uint8_t, 64)
    output := make([]C.uint8_t, 128)

    for i := 0; i < 64; i++ {
        input[i] = C.uint8_t(i)
    }

    err := C.to_base_64_utf8_aot(
        &input[0],
        C.int32_t(len(input)),
        &output[0],
        C.int32_t(len(output)),
    )

    if err != 0 {
        panic(fmt.Sprintf("CError returned: %d", int32(err)))
    }

    fmt.Println("Output:", output)
}
```

---

## 10. Python Example (ctypes)

```
from ctypes import *

dll = CDLL("C-Abi-Bridge.Aot.dll")

# CError = int32
dll.to_base_64_utf8_aot.restype = c_int

dll.to_base_64_utf8_aot.argtypes = [
    POINTER(c_ubyte),  # input buffer
    c_int,             # input length
    POINTER(c_ubyte),  # output buffer
    c_int              # output length
]

input_buf = (c_ubyte * 64)(*range(64))
output_buf = (c_ubyte * 128)()

err = dll.to_base_64_utf8_aot(input_buf, len(input_buf), output_buf, len(output_buf))

if err != 0:
    raise Exception(f"CError returned: {err}")

print(bytes(output_buf))
```

---

## 11. VB.NET Example

```
Dim buf(31) As Byte
Dim rc = crypto_random_bytes_aot(buf, buf.Length)
```

---
