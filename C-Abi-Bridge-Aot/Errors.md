# 📘 Error Codes & Handling  

This document describes the error codes returned by the **C‑Abi‑Bridge‑Aot** NativeAOT library and provides guidance on how to handle them safely across different languages.

All exported functions return an `int`:

- `0` indicates **success**
- Negative values indicate **failure**

---

# 1. Error Code Table

| Code | Name | Meaning | Typical Cause |
|------|------|---------|----------------|
| `0` | `CABI_OK` | Operation completed successfully | — |
| `-1` | `CABI_ERR_INVALID_ARGUMENT` | One or more arguments are invalid | Null pointer, invalid key length, invalid IV length |
| `-2` | `CABI_ERR_BUFFER_TOO_SMALL` | Output buffer is too small | Caller did not allocate enough space |
| `-3` | `CABI_ERR_NULL_POINTER` | A required pointer was null | Missing buffer, key, IV, or input |
| `-4` | `CABI_ERR_INTERNAL` | Internal error occurred | Unexpected failure inside crypto backend |
| `-5` | `CABI_ERR_UNSUPPORTED` | Unsupported algorithm or parameter | Invalid mode, unsupported key size |
| `-6` | `CABI_ERR_CRYPTO_FAILURE` | Cryptographic operation failed | Authentication failure, invalid tag, decryption error |

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

## 3.1 C / C++

```
int result = cabi_aes_gcm_decrypt(...);

if (result == 0) {
    // success
} else if (result == -6) {
    // authentication failure
} else {
    // general error handling
}
```

## 3.2 C# (P/Invoke)


```
int rc = cabi_sha256(input, input.Length, output, output.Length);

if (rc < 0)
    throw new InvalidOperationException($"Crypto error: {rc}");
```

## 3.3 VB.NET

```
Dim rc = cabi_sha256(input, input.Length, output, output.Length)
If rc <> 0 Then
    Throw New Exception("Crypto error: " & rc)
End If
```

## 3.4 Rust

```
let rc = unsafe { cabi_sha256(input.as_ptr(), len, out.as_mut_ptr(), out_len) };

if rc != 0 {
    panic!("crypto error: {}", rc);
}
```

---

## 4. Common Error Scenarios

## 4.1 Buffer Too Small (`-2`)

Occurs when:
- Output buffer is smaller than required
- Tag buffer is too small
- GCM output buffer does not include space for ciphertext

Fix:
- Allocate a larger buffer
- Check required sizes in API.md

## 4.2 Invalid Argument (-1)

Occurs when:
- Key length is invalid
- IV length is invalid
- AAD length is negative
- Input length is negative

Fix:
- Validate all parameters before calling
- Use correct key sizes (AES‑128/192/256, GCM IV = 12 bytes)

## 4.3 Null Pointer (-3)
Occurs when:
- A required pointer is NULL
- Caller forgot to allocate a buffer

Fix:
- Ensure all pointers are valid
- Ensure buffers are allocated before calling

## 4.4 Crypto Failure (-6)
Occurs when:
- AES‑GCM authentication fails
- ChaCha20‑Poly1305 tag mismatch
- Decryption input is corrupted

Fix:
- Verify key, IV, and tag
- Ensure ciphertext was not modified

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

End of Error Documentation


