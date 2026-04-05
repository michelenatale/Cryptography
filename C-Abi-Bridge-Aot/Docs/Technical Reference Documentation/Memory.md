# 📘 Memory Ownership & Lifetime Rules  
This document describes how memory is allocated, passed, and freed when using the **C‑Abi‑Bridge‑Aot** NativeAOT library.  
Correct memory handling is essential for safe and predictable cross‑language interop.

---

# 1. General Principles

The library follows strict and predictable memory rules:

- **The caller owns all memory** unless explicitly stated otherwise.
- **The library never frees caller‑allocated memory.**
- **The library rarely allocates memory** — only specific functions do.
- **If the library allocates memory, the caller must free it using `cabi_free_buffer`.**
- **All buffers must be valid, non‑null, and large enough.**

These rules ensure:

- No hidden allocations  
- No GC interaction  
- No runtime dependencies  
- Safe interop across C, C++, Rust, Go, Zig, Python, .NET, etc.

---

# 2. Caller‑Allocated Buffers (Most Common Case)

Most functions follow this pattern:

```c
int cabi_function(
    const uint8_t* input,
    int32_t input_len,
    uint8_t* output,
    int32_t output_len
);
```

**Caller responsibilities:**

- Allocate output buffer
- Ensure output_len is large enough
- Ensure all pointers are non‑null
- Ensure lengths are valid

**Library responsibilities:**

- Write into the provided buffer
- Never allocate memory
- Never free memory
- Return an error code if the buffer is too small

**Typical error codes:**

- `-1` → NullPointer
- `-2` → InvalidLength
- `-4` → CryptoError

---

# 3. Library‑Allocated Memory (Rare Case)

Some functions may allocate memory internally (e.g., future PQC keygen APIs).

These functions will explicitly document:
- That they allocate memory
- That the caller must free it
- That the memory is unmanaged
- reeing allocated memory

**Freeing allocated memory**

```
CError free_buffer_aot(void* buffer);
```

**Rules:**

- Only free memory allocated by the library
- Never free caller‑allocated memory using this function
- Passing `NULL` returns `CABI_ERR_NULL_POINTER (-1)`

---

# 4. Zeroing Sensitive Memory

The library attempts to zero sensitive memory when possible:
- Keys
- IVs
- Nonces
- Intermediate buffers
- Authentication tags

However:
- The caller is responsible for zeroing their own buffers
- The library cannot zero memory it does not own
- Zeroing is best‑effort and platform‑dependent

# 5. Alignment & Safety

All exported functions assume:
- Buffers are properly aligned for their platform
- Buffers are contiguous
- Buffers are writable (for output)
- Buffers are readable (for input)

Misaligned or invalid pointers may cause:
- `CABI_ERR_NULL_POINTER (-1)`
- `CABI_ERR_INVALID_LENGTH (-2)`
- Undefined behavior (in unmanaged languages)

---

# 6. Memory Rules by Language

## 6.1 C / C++

Caller must allocate: 
```
uint8_t out[64];
cabi_sha256(input, len, out, 64);
```
Never free stack memory with cabi_free_buffer.

## 6.2 C# / VB.NET (P/Invoke)

Use managed arrays:
```
byte[] output = new byte[32];
cabi_sha256(input, input.Length, output, output.Length);
```
The GC owns the memory — do not free it manually.

## 6.3 Rust

Use slices or vectors:
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
    fn free_memory_aot(ptr: *mut u8) -> i32; // CError (int32)
}

fn assert_error(err: i32) {
    if err != 0 {
        panic!("CError returned: {}", err);
    }
}

fn main() {
    let mut buffer = vec![1u8, 2, 3, 4];

    let err = unsafe { free_memory_aot(buffer.as_mut_ptr()) };
    assert_error(err);

    println!("Memory freed successfully.");
}
```
Rust owns the memory — do not call `cabi_free_buffer`.

## 6.4 Go (cgo)

Use Go slices:
```
package main

/*
#include <stdint.h>

extern int32_t free_memory_aot(uint8_t* ptr);
*/
import "C"
import "fmt"

func assertError(err C.int32_t) {
    if err != 0 {
        panic(fmt.Sprintf("CError returned: %d", int32(err)))
    }
}

func main() {
    buffer := []C.uint8_t{1, 2, 3, 4}

    err := C.free_memory_aot(&buffer[0])
    assertError(err)

    fmt.Println("Memory freed successfully.")
}
```
Go owns the memory.

## 6.5 Python (ctypes)
```
from ctypes import *

dll = CDLL("C-Abi-Bridge.Aot.dll")

# CError = int32
dll.free_memory_aot.restype = c_int
dll.free_memory_aot.argtypes = [POINTER(c_ubyte)]

def assert_error(err: int):
    if err != 0:
        raise Exception(f"CError returned: {err}")

# Example buffer
buf = (c_ubyte * 4)(1, 2, 3, 4)

err = dll.free_memory_aot(buf)
assert_error(err)

print("Memory freed successfully.")
```
Python owns the memory.

---

## 7. Common Mistakes & How to Avoid Them

❌ Passing NULL pointers
→ Returns -1 (NullPointer)

❌ Passing incorrect buffer sizes
→ Returns -2 (InvalidLength)

❌ Freeing caller‑allocated memory with cabi_free_buffer
→ Undefined behavior

❌ Forgetting to free library‑allocated memory
→ Memory leak

❌ Using the wrong key/IV/tag sizes
→ CryptoError or InvalidLength

---

## 8. Summary

| Rule | Responsibility |
| --- | --- |
| Allocate buffers | Caller |
| Free caller‑allocated memory | Caller |
| Free library‑allocated memory | Caller (using ``cabi_free_buffer``) |
| Zero sensitive memory | Both (best effort) |
| Validate lengths | Caller |
| Validate pointers | Caller |
| Never allocate memory implicitly | Library |

**End of Memory Ownership Rules**



