# 📘 Error Codes & Handling  

This document describes the error codes returned by the **C‑Abi‑Bridge‑Aot** NativeAOT library and provides guidance on how to handle them safely across different languages.

All exported functions return an `int` / `cerror`:

- `0` indicates **success**
- Negative values indicate **failure**

---

# 1. Error Code Table

| Code | Name | Meaning | Typical Cause |
| --- | --- | --- | --- |
| ``0`` | ``CABI_OK`` | Operation completed successfully | — |
| ``-1`` | ``CABI_ERR_NULL_POINTER`` | A required pointer was null | Missing buffer, key, IV, or input |
| ``-2`` | ``CABI_ERR_INVALID_LENGTH`` | Length parameter is invalid | Wrong key size, wrong IV size, negative length |
| ``-3`` | ``CABI_ERR_IO_ERROR`` | File or stream I/O failed | File not found, access denied, read/write error |
| ``-4`` | ``CABI_ERR_CRYPTO_ERROR`` | Cryptographic operation failed | Authentication failure, invalid tag, corrupted ciphertext |
| ``-5`` | ``CABI_ERR_OUT_OF_RANGE`` | Value outside allowed range | Numeric range checks, invalid enum values |
| ``-99`` | ``CABI_ERR_UNKNOWN`` | Unknown or unexpected error | Unexpected internal failure |

These codes are stable and part of the public ABI.

---

# 2. General Error Handling Rules

### ✔ Always check the return value  
Never assume success.

```c
int result = fill_crypto_bytes_aot(buf, 32);
if (result != 0) {
    // handle error
}
```

✔ Never ignore negative values
A negative return value always indicates failure.

✔ Do not rely on exceptions
The library does not throw exceptions.
All errors are communicated through return codes.

✔ Validate buffer sizes before calling
Most cryptographic functions require the caller to allocate sufficiently large output buffers.

✔ Validate key and IV sizes
Incorrect key lengths are a common source of -1 errors.

---

# 3. Error Handling by Language

## 3.1 Universal Error Convention for All Languages

`CError` is the **only canonical error code** of the C‑ABI Bridge.  
It is defined as an int value and represents all possible error conditions returned by the NativeAOT export functions.

All languages consuming the C‑ABI **must follow this convention**:

- They must **mirror the `CError` values exactly** (1:1 mapping).
- They must **not introduce custom error codes** or alternative numeric schemes.
- They may provide **language‑specific wrappers** (e.g., Rust `Result<T, CryptoError>`, C# exceptions, Go `error`, Python exceptions),  
  but these wrappers must always be based on the **original `CError` byte value**.
- The underlying `CError` value must remain the **single source of truth** for all error handling across all bindings.

This rule applies to **all languages**, not only Rust, go, etc.  
It is based on the error model originally defined for **C / C# / VB.NET**, and all other languages are expected to follow the same contract to ensure API stability, predictability, and cross‑language consistency.

## 3.2 C / C++

```
auto err = aes_gcm_encrypt_aot(
  plain.data(), (int)plain.size(),
  key.data(), (int)key.size(),
  associat.data(), (int)associat.size(),
  &cipher_ptr, &cipher_len);
assert_error(err);
```

## 3.3 C# (P/Invoke)

```
var err = Native.Sha256HashDataAot(
  bytes, bytes.Length,
  out IntPtr hash_ptr, out int hash_length);
AssertError(err);
```

## 3.4 VB.NET

```
Dim err = Sha256HashDataAot(bytes, bytes.Length, hash_ptr, hash_length)
AssertError(err)
```

## 3.5 Rust

```
#[repr(u8)]
#[derive(Debug, Copy, Clone, PartialEq, Eq)]
pub enum CError {
    Ok = 0,
    // ...
}

extern "C" {
    fn aes_decrypt_aot(
        bytes_ptr: *const u8,
        bytes_length: i32,
        key_ptr: *const u8,
        key_length: i32,
        associated_ptr: *const u8,
        associated_length: i32,
        decipher_ptr: *mut *mut u8,
        decipher_length: *mut i32,
    ) -> u8; // oder CError, wenn via bindgen gespiegelt
}
```
```
fn aes_decrypt(...) -> Result<Vec<u8>, CryptoError> {
    let err = unsafe { aes_decrypt_aot(... ) };
    match err {
        0 => Ok(...),
        code => Err(CryptoError::from(code)),
    }
}
```

---

## 4. Common Error Scenarios

## 4.1 Null Pointer (`-1`)

Occurs when:
- A required pointer is `NULL`
- Caller forgot to allocate a buffer
- A buffer, key, IV, tag, or input pointer is missing

Fix:
- Ensure all pointers are valid
- Ensure buffers are allocated before calling
- Validate that no required argument is `NULL`

---

## 4.2 Invalid Length (`-2`)

Occurs when:
- A length parameter is negative
- Key length is invalid
- IV length is invalid
- Tag length is invalid
- Input or output length is inconsistent

Fix:
- Validate all length parameters before calling
- Use correct key sizes (e.g. AES‑128/192/256)
- Use valid IV/tag sizes for the chosen algorithm
- Check required sizes in `API.md`

---

## 4.3 I/O Error (`-3`)

Occurs when:
- A file cannot be opened
- A file cannot be read or written
- The path is invalid or access is denied

Fix:
- Verify file paths
- Check file permissions
- Ensure the file exists and is accessible

---

## 4.4 Crypto Error (`-4`)

Occurs when:
- AES‑GCM authentication fails
- ChaCha20‑Poly1305 tag verification fails
- Ciphertext or tag has been modified or corrupted

Fix:
- Verify key, IV, AAD, and tag
- Ensure ciphertext was not modified
- Treat this as a **hard failure** and do not use the output

---

## 4.5 Out of Range (`-5`)

Occurs when:
- A numeric value is outside the allowed range
- An enum or mode value is invalid
- A parameter violates documented constraints

Fix:
- Validate all numeric and enum parameters
- Ensure values are within documented ranges

---

## 4.6 Unknown Error (`-99`)

Occurs when:
- An unexpected internal condition is hit
- An error cannot be mapped to a more specific code

Fix:
- Log the error code and context
- Try to reproduce with debug builds
- If reproducible, consider reporting it (see `SECURITY.md` / issue tracker)

---

## 5. Debugging Tips
✔ Enable debug logging in your consumer project
Log buffer sizes, key lengths, and return codes.

✔ Validate all inputs before calling
Most errors come from incorrect buffer sizes.

✔ Use the C++ test suite as reference
The C++ tests demonstrate correct usage patterns.

✔ Check for memory alignment issues
Especially in C, C++, and Rust.

---

## 6. ABI Stability
Error codes are part of the public ABI and will not change without a major version bump.

---


