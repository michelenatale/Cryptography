
# 📘 C‑Abi‑Bridge‑Aot — API Reference

This document describes the public C ABI exported by the **C‑Abi‑Bridge‑Aot** NativeAOT library.  
All functions follow a stable, language‑agnostic ABI and can be consumed from C, C++, Rust, Go, Zig, Python, Java, .NET, and any other language that supports C interop.

---

# 1. ABI Conventions

- **Calling convention:** `__cdecl`
- **Export style:** C ABI (`extern "C"`)
- **Name mangling:** Disabled (flat symbol names)
- **Return type:** `int` (status code)
- **Error handling:** Negative values indicate failure
- **Memory:** Caller‑allocated unless explicitly stated otherwise
- **Thread safety:** All functions are thread‑safe unless noted

---

# 2. Error Codes

| Code | Meaning |
|------|---------|
| `0` | Success |
| `-1` | Invalid argument |
| `-2` | Buffer too small |
| `-3` | Null pointer |
| `-4` | Internal error |
| `-5` | Unsupported algorithm |
| `-6` | Cryptographic failure |

---

# 3. Memory Rules

- All buffers must be allocated by the **caller**.
- The library never frees memory allocated by the caller.
- Functions that allocate memory explicitly document this behavior.
- Use `cabi_free_buffer` to free memory allocated by the library.

---

# 4. Random Number Generation

## 4.1 `cabi_crypto_random_bytes`

```c
int cabi_crypto_random_bytes(uint8_t* buffer, int32_t size);
```

**Parameters**

| Name | Type | Description |
| --- | --- | --- |
| ``buffer`` | ``uint8_t*`` | Output buffer |
| ``size`` | ``int32_t`` | Number of bytes to generate |


**Returns**

- `0` on success
- Negative error code on failure

**Example (C)**

```
uint8_t buf[32];
cabi_crypto_random_bytes(buf, 32);
```

--- 

# 5. AES Encryption

## 5.1 AES‑CBC Encrypt

```
int cabi_aes_cbc_encrypt(
    const uint8_t* key,
    int32_t key_len,
    const uint8_t* iv,
    const uint8_t* input,
    int32_t input_len,
    uint8_t* output,
    int32_t output_len
);
```

**Notes**

- `output_len` must be >= `input_len + 16`
- PKCS7 padding is applied automatically

## 5.2 AES‑CBC Decrypt

```
int cabi_aes_cbc_decrypt(
    const uint8_t* key,
    int32_t key_len,
    const uint8_t* iv,
    const uint8_t* input,
    int32_t input_len,
    uint8_t* output,
    int32_t output_len
);
```

---

# 6. AES‑GCM

## 6.1 Encrypt

```
int cabi_aes_gcm_encrypt(
    const uint8_t* key,
    int32_t key_len,
    const uint8_t* iv,
    int32_t iv_len,
    const uint8_t* aad,
    int32_t aad_len,
    const uint8_t* input,
    int32_t input_len,
    uint8_t* output,
    int32_t output_len,
    uint8_t* tag,
    int32_t tag_len
);
```

## 6.2 Decrypt

```
int cabi_aes_gcm_decrypt(
    const uint8_t* key,
    int32_t key_len,
    const uint8_t* iv,
    int32_t iv_len,
    const uint8_t* aad,
    int32_t aad_len,
    const uint8_t* input,
    int32_t input_len,
    const uint8_t* tag,
    int32_t tag_len,
    uint8_t* output,
    int32_t output_len
);
```

---

# 7. ChaCha20‑Poly1305

## 7.1 Encrypt

```
int cabi_chacha20poly1305_encrypt(
    const uint8_t* key,
    const uint8_t* nonce,
    const uint8_t* aad,
    int32_t aad_len,
    const uint8_t* input,
    int32_t input_len,
    uint8_t* output,
    int32_t output_len,
    uint8_t* tag,
    int32_t tag_len
);
```

## 7.2 Decrypt

```
int cabi_chacha20poly1305_decrypt(
    const uint8_t* key,
    const uint8_t* nonce,
    const uint8_t* aad,
    int32_t aad_len,
    const uint8_t* input,
    int32_t input_len,
    const uint8_t* tag,
    int32_t tag_len,
    uint8_t* output,
    int32_t output_len
);
```

---

# 8. Hashing

## 8.1 SHA‑256

```
int cabi_sha256(
    const uint8_t* input,
    int32_t input_len,
    uint8_t* output,
    int32_t output_len
);
```
Output must be 32 bytes.

## 8.2 SHA‑512

```
int cabi_sha512(
    const uint8_t* input,
    int32_t input_len,
    uint8_t* output,
    int32_t output_len
);
```

Output must be 64 bytes.

---

# 9. HMAC

## 9.1 HMAC‑SHA256

```
int cabi_hmac_sha256(
    const uint8_t* key,
    int32_t key_len,
    const uint8_t* input,
    int32_t input_len,
    uint8_t* output,
    int32_t output_len
);
```

---

# 10. Encoding Utilities

## 10.1 Base64 Encode

```
int cabi_base64_encode(
    const uint8_t* input,
    int32_t input_len,
    uint8_t* output,
    int32_t output_len
);
```

## 10.2 Base64 Decode

```
int cabi_base64_decode(
    const uint8_t* input,
    int32_t input_len,
    uint8_t* output,
    int32_t output_len
);
```

--- 

# 11. Memory Management

## 11.1 Free Buffer

```
int cabi_free_buffer(void* buffer);
```

Used only for functions that allocate memory (rare).

---

# 12. File Encryption

## 12.1 AES‑CBC File Encrypt

```
int cabi_aes_cbc_encrypt_file(
    const char* input_path,
    const char* output_path,
    const uint8_t* key,
    int32_t key_len,
    const uint8_t* iv
);
```

---

# 13. Post‑Quantum Cryptography

(Only include if you want — I can expand this section fully.)

---

# 14. Versioning

The API follows semantic versioning:
- Adding functions → minor version
- Changing signatures → major version
- Bug fixes → patch version

---

# 15. Language Interop Notes

C++ uses the .lib from Build/Artifacts
- C# / VB.NET use P/Invoke
- Rust uses extern "C"
- Go uses cgo
- Zig can import the .def directly
- Python uses ctypes or cffi

---

# 16. Example: C++ Usage

```
#include "cabi_exp_imp.h"

uint8_t key[32] = {0};
uint8_t iv[16] = {0};
uint8_t input[32] = {1,2,3};
uint8_t output[64];

cabi_aes_cbc_encrypt(key, 32, iv, input, 32, output, 64);
```

# 17. Example: C# Usage

```
[DllImport("C-Abi-Bridge.Aot.N.dll")]
public static extern int cabi_sha256(byte[] input, int len, byte[] output, int outLen);
```

**End of API Reference**

