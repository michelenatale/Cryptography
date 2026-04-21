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
err = dll.to_base_64_utf8_aot(
    input_buf,
    len(input_buf),
    output_buf,
    len(output_buf)
)
assert_error(err)
```

---

## 7. C++ Example (Full Flow)

```
#include "bridge_aot.h"

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
extern "C" {
    fn to_base_64_utf8_aot(
        input_ptr: *const u8,
        input_len: i32,
        output_ptr: *mut *mut u8,
        output_len: *mut i32,
    ) -> i32;

    fn free_buffer(ptr: *mut u8);
}

fn assert_error(err: i32) {
    if err != 0 {
        panic!("CError returned: {}", err);
    }
}

fn main() {
    let input: Vec<u8> = (0..64).collect();

    let mut out_ptr: *mut u8 = std::ptr::null_mut();
    let mut out_len: i32 = 0;

    let err = unsafe {
        to_base_64_utf8_aot(
            input.as_ptr(),
            input.len() as i32,
            &mut out_ptr,
            &mut out_len,
        )
    };
    assert_error(err);

    let data = unsafe { std::slice::from_raw_parts(out_ptr, out_len as usize).to_vec() };

    unsafe { free_buffer(out_ptr) };
    out_ptr = std::ptr::null_mut(); // Important: reset pointer

    println!("Output: {:?}", data);

    .....
}
```

---

## 9. Go Example (cgo)

```
/*
#cgo LDFLAGS: -LC-Abi-Bridge-Aot -lCAbiBridgeAot
#include <stdint.h>

extern int32_t to_base_64_utf8_aot(
    const uint8_t* input_ptr,
    int32_t input_len,
    uint8_t** output_ptr,
    int32_t* output_len
);

extern void free_buffer(uint8_t* ptr);
*/
import "C"
import "unsafe"
import "fmt"

func assertError(err C.int32_t) {
    if err != 0 {
        panic(fmt.Sprintf("CError returned: %d", int32(err)))
    }
}

func main() {
    input := make([]C.uint8_t, 64)
    for i := 0; i < 64; i++ {
        input[i] = C.uint8_t(i)
    }

    var outPtr *C.uint8_t
    var outLen C.int32_t

    err := C.to_base_64_utf8_aot(
        &input[0],
        C.int32_t(len(input)),
        &outPtr,
        &outLen,
    )
    assertError(err)

    data := C.GoBytes(unsafe.Pointer(outPtr), C.int(outLen))

    C.free_buffer(outPtr)
    outPtr = nil // Important: reset pointer

    fmt.Println("Output:", data)

    ....
}

```

---

## 10. Python Example (ctypes)

```
from ctypes import *

dll = CDLL("C-Abi-Bridge.Aot.dll")

dll.to_base_64_utf8_aot.restype = c_int
dll.to_base_64_utf8_utf8_aot.argtypes = [
    POINTER(c_ubyte),
    c_int,
    POINTER(c_void_p),   # output_ptr**
    POINTER(c_int)       # output_len*
]

dll.free_buffer.argtypes = [c_void_p]
dll.free_buffer.restype = None

def assert_error(err: int):
    if err != 0:
        raise Exception(f"CError returned: {err}")

# Example usage
input_buf = (c_ubyte * 64)(*range(64))

out_ptr = c_void_p(None)
out_len = c_int(0)

err = dll.to_base_64_utf8_aot(
    input_buf,
    len(input_buf),
    byref(out_ptr),
    byref(out_len)
)
assert_error(err)

data = string_at(out_ptr, out_len.value)

dll.free_buffer(out_ptr)  # Important: free memory
out_ptr = c_void_p(None)  # Important: reset pointer

print(data)

....
```

---

## 11. VB.NET Example

```
Dim plain_ptr As IntPtr = Nothing, plain_length As Int32 = Nothing
Dim err = ToBase64Aot(bytes, bytes.Length, plain_ptr, plain_length)
AssertError(err)

Dim data = ToBytes(plain_ptr, plain_length)
FreeBuffer(plain_ptr) 'Important: Always reset the memory

plain_ptr = IntPtr.Zero
err = FromBase64Aot(data, data.Length, plain_ptr, plain_length)
AssertError(err)
```

---
