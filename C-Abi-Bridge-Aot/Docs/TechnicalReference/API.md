
# 📘 C‑Abi‑Bridge‑Aot — API Reference

This document describes the public C ABI exported by the **C‑Abi‑Bridge‑Aot** NativeAOT library.  

All functions follow a stable, language‑agnostic ABI and can be consumed from C, C++, Rust, Go, Zig, Python, Java, .NET, and any other language that supports C interop.

---

# 1. ABI Conventions

- **Calling convention:** `__cdecl`
- **Export style:** C ABI (`extern "C"`)
- **Name mangling:** Disabled (flat symbol names)
- **Return type:** `int` or `cerror` (status code)
- **Error handling:** Negative values indicate failure
- **Memory:** Caller‑allocated unless explicitly stated otherwise
- **Thread safety:** All functions are thread‑safe unless noted

---

# 2. Error Codes

| Code | Meaning |
|------|---------|
| `0` | Success |
| `-1` | NullPointer |
| `-2` | InvalidLength |
| `-3` | IoError |
| `-4` | CryptoError |
| `-5` | OutOfRange |
| `-99` | UnknownError |

---

# 3. Memory Rules

- All buffers must be allocated by the **caller**.
- The library never frees memory allocated by the caller.
- Functions that allocate memory explicitly document this behavior.
- Use `cabi_free_buffer` to free memory allocated by the library.

---

# 4. Random Number Generation

## 4.1 `fill_crypto_bytes_aot`

```c
CError fill_crypto_bytes_aot(uint8_t* buffer, int32_t size);
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
uint8_t buffer[32];
fill_crypto_bytes_aot(buffer, 32);
```

--- 

# 5. AES Encryption

## 5.1 AES‑CBC-AEAD Encrypt

```
[UnmanagedCallersOnly(EntryPoint = "aes_encrypt_aot")]
public unsafe static CError AesEncryptAot(
  byte* bytes_ptr, int bytes_length,
  byte* key_ptr, int key_length,
  byte* associated_ptr, int associated_length,
  byte** cipher_ptr, int* cipher_length);
```

**Notes**

- die `output_length` ist immer länger als die `Input_length`

## 5.2 AES‑CBC-AEAD Decrypt

```
[UnmanagedCallersOnly(EntryPoint = "aes_decrypt_aot")]
public unsafe static CError AesDecryptAot(
  byte* bytes_ptr, int bytes_length,
  byte* key_ptr, int key_length,
  byte* associated_ptr, int associated_length,
  byte** decipher_ptr, int* decipher_length);
```

---

# 6. AES‑GCM

## 6.1 Encrypt

```
[UnmanagedCallersOnly(EntryPoint = "aes_gcm_encrypt_aot")]
public unsafe static CError AesGcmEncryptAot(
  byte* bytes_ptr, int bytes_length,
  byte* key_ptr, int key_length,
  byte* associated_ptr, int associated_length,
  byte** cipher_ptr, int* cipher_length);
```

## 6.2 Decrypt

```
[UnmanagedCallersOnly(EntryPoint = "aes_gcm_decrypt_aot")]
public unsafe static CError AesGcmDecryptAot(
 byte* bytes_ptr, int bytes_length,
 byte* key_ptr, int key_length,
 byte* associated_ptr, int associated_length,
 byte** decipher_ptr, int* decipher_length);
```

---

# 7. ChaCha20‑Poly1305

## 7.1 Encrypt

```
[UnmanagedCallersOnly(EntryPoint = "chacha20_poly1305_encrypt_aot")]
public unsafe static CError ChaCha20Poly1305EncryptAot(
  byte* bytes_ptr, int bytes_length,
  byte* key_ptr, int key_length,
  byte* associated_ptr, int associated_length,
  byte** cipher_ptr, int* cipher_length);
```

## 7.2 Decrypt

```
[UnmanagedCallersOnly(EntryPoint = "chacha20_poly1305_decrypt_aot")]
public unsafe static CError ChaCha20Poly1305DecryptAot(
 byte* bytes_ptr, int bytes_length,
 byte* key_ptr, int key_length,
 byte* associated_ptr, int associated_length,
 byte** decipher_ptr, int* decipher_length);
```

---

# 8. Hashing

## 8.1 SHA‑256

```
[UnmanagedCallersOnly(EntryPoint = "sha_256_hash_data_aot")]
public unsafe static CError Sha256HashDataAot(
  byte* bytes_ptr, int length,
  byte** sha_out_ptr, int* sha_out_length);
```
Output must be 32 bytes.

## 8.2 SHA‑512

```
[UnmanagedCallersOnly(EntryPoint = "sha_512_hash_data_aot")]
public unsafe static CError Sha512HashDataAot(
  byte* bytes_ptr, int length,
  byte** sha_out_ptr, int* sha_out_length);
```

Output must be 64 bytes.

---

# 9. HMAC

## 9.1 HMAC‑SHA256

```
[UnmanagedCallersOnly(EntryPoint = "hmac_sha_256_hash_data_aot")]
public unsafe static CError HmacSha256HashDataAot(
  byte* bytes_ptr, int bytes_length,
  byte* key_ptr, int key_length,
  byte** sha_out_ptr, int* sha_out_length);
```

---

# 10. Encoding Utilities

## 10.1 Base64 Encode

```
[UnmanagedCallersOnly(EntryPoint = "to_base_64_utf8_aot")]
public unsafe static CError ToBase64Utf8Aot(
 byte* bytes, int bytes_length, byte** output_ptr, int* outputLen);
```

## 10.2 Base64 Decode

```
[UnmanagedCallersOnly(EntryPoint = "from_base_64_utf8_aot")]
public unsafe static CError FromBase64Utf8Aot(
  byte* bytes, int bytes_length, byte** output_ptr, int* outputLen);
```

--- 

# 11. Memory Management

## 11.1 Free Buffer

```
[UnmanagedCallersOnly(EntryPoint = "free_buffer_clear_aot")]
public static void FreeBuffer(void* buffer, int length);
```

Used only for functions that allocate memory (rare).

---

# 12. File Encryption

## 12.1 AES‑CBC-AEAD File Encrypt

```
[UnmanagedCallersOnly(EntryPoint = "aes_encrypt_file_aot")]
public unsafe static CError AesEncryptFileAot(
  byte* src_ptr, int src_length,
  byte* dest_ptr, int dest_length,
  byte* key_ptr, int key_length,
  byte* associated_ptr, int associated_length);
```

---

# 13. Post‑Quantum Cryptography

```
[UnmanagedCallersOnly(EntryPoint = "create_mlkem_key_pair_aot")]
public unsafe static CError CreateMlKemKeyPairAot(
  byte** priv_key_ptr, int* priv_key_length,
  byte** pub_key_ptr, int* pub_key_length,
  byte** guid_id_ptr, int* guid_id_length,
  byte* mlkem_param, byte* crypto_algo);
```

```
[UnmanagedCallersOnly(EntryPoint = "pqc_mlkem_encryption_aot")]
public unsafe static CError PqcMlKemEncryptionAot(
  byte* message_ptr, int message_length,
  byte* private_key_ptr, int private_key_length,
  byte* capsulation_ptr, int capsulation_length,
  byte* associated_ptr, int associated_length,
  byte mlkem_param, byte crypto_algo,
  byte** cipher_ptr, int* cipher_length);

[UnmanagedCallersOnly(EntryPoint = "pqc_mlkem_decryption_aot")]
public unsafe static CError PqcMlKemDecryptionAot(
  byte* cipher_ptr, int cipher_length,
  byte* shared_key_ptr, int shared_key_length,
  byte* associated_ptr, int associated_length,
  byte mlkem_param, byte crypto_algo,
  byte** decipher_ptr, int* decipher_length);
```

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
[LibraryImport(DllName, EntryPoint = "sha_256_hash_data_aot")]
internal static partial CError Sha256HashDataAot(
  ReadOnlySpan<byte> bytes, int bytes_length,
  out IntPtr output, out int output_length);
```

---


