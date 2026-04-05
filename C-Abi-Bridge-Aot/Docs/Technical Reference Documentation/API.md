
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
[LibraryImport(DllName, EntryPoint = "aes_encrypt_aot")]
internal static partial CError AesEncryptAot(
  ReadOnlySpan<byte> bytes, int bytes_length,
  IntPtr key, int key_length,
  ReadOnlySpan<byte> associated, int associated_length,
  out IntPtr output, out int output_length);
```

**Notes**

- die `output_length` ist immer länger als die `Input_length`

## 5.2 AES‑CBC-AEAD Decrypt

```
[LibraryImport(DllName, EntryPoint = "aes_decrypt_aot")]
internal static partial CError AesDecryptAot(
  ReadOnlySpan<byte> bytes, int bytes_length,
  IntPtr key, int key_length,
  ReadOnlySpan<byte> associated, int associated_length,
  out IntPtr output, out int output_length);
```

---

# 6. AES‑GCM

## 6.1 Encrypt

```
[LibraryImport(DllName, EntryPoint = "aes_gcm_encrypt_aot")]
internal static partial CError AesGcmEncryptAot(
  ReadOnlySpan<byte> bytes, int bytes_length,
  IntPtr key, int key_length,
  ReadOnlySpan<byte> associated, int associated_length,
  out IntPtr output, out int output_length);
```

## 6.2 Decrypt

```
[LibraryImport(DllName, EntryPoint = "aes_gcm_decrypt_aot")]
internal static partial CError AesGcmDecryptAot(
  ReadOnlySpan<byte> bytes, int bytes_length,
  IntPtr key, int key_length,
  ReadOnlySpan<byte> associated, int associated_length,
  out IntPtr output, out int output_length);
```

---

# 7. ChaCha20‑Poly1305

## 7.1 Encrypt

```
[LibraryImport(DllName, EntryPoint = "chacha20_poly1305_encrypt_aot")]
internal static partial CError ChaCha20Poly1305EncryptAot(
  ReadOnlySpan<byte> bytes, int bytes_length,
  IntPtr key, int key_length,
  ReadOnlySpan<byte> associated, int associated_length,
  out IntPtr output, out int output_length);

```

## 7.2 Decrypt

```
[LibraryImport(DllName, EntryPoint = "chacha20_poly1305_decrypt_aot")]
internal static partial CError ChaCha20Poly1305DecryptAot(
  ReadOnlySpan<byte> bytes, int bytes_length,
  IntPtr key, int key_length,
  ReadOnlySpan<byte> associated, int associated_length,
  out IntPtr output, out int output_length);
```

---

# 8. Hashing

## 8.1 SHA‑256

```
[LibraryImport(DllName, EntryPoint = "sha_256_hash_data_aot")]
internal static partial CError Sha256HashDataAot(
  ReadOnlySpan<byte> bytes, int bytes_length,
  out IntPtr output, out int output_length);

```
Output must be 32 bytes.

## 8.2 SHA‑512

```
[LibraryImport(DllName, EntryPoint = "sha_512_hash_data_aot")]
public static partial CError Sha512HashDataAot(
  ReadOnlySpan<byte> bytes, int bytes_length,
  out IntPtr output, out int output_length);
```

Output must be 64 bytes.

---

# 9. HMAC

## 9.1 HMAC‑SHA256

```
[LibraryImport(DllName, EntryPoint = "hmac_sha_256_hash_data_aot")]
public static partial CError HmacSha256HashDataAot(
  ReadOnlySpan<byte> bytes, int bytes_length,
  ReadOnlySpan<byte> key, int key_length,
  out IntPtr output, out int output_length);
```

---

# 10. Encoding Utilities

## 10.1 Base64 Encode

```
[LibraryImport(DllName, EntryPoint = "to_base_64_utf8_aot")]
internal static partial CError ToBase64Aot(
  ReadOnlySpan<byte> bytes, int bytes_length,
  out IntPtr output, out int output_length);
```

## 10.2 Base64 Decode

```
[LibraryImport(DllName, EntryPoint = "from_base_64_utf8_aot")]
public static partial CError FromBase64Aot(
  ReadOnlySpan<byte> bytes, int bytes_length,
  out IntPtr output, out int output_length);
```

--- 

# 11. Memory Management

## 11.1 Free Buffer

```
[LibraryImport(DllName, EntryPoint = "free_buffer_aot")]
internal static partial void FreeBuffer(IntPtr ptr);
```

Used only for functions that allocate memory (rare).

---

# 12. File Encryption

## 12.1 AES‑CBC File Encrypt

```
[LibraryImport(DllName, EntryPoint = "aes_encrypt_file_aot")]
public static partial CError AesEncryptFileAot(
 ReadOnlySpan<byte> src, int src_length,
 ReadOnlySpan<byte> dest, int dest_length,
 IntPtr key, int key_length,
 ReadOnlySpan<byte> associated, int associated_length);
```

---

# 13. Post‑Quantum Cryptography

```
[LibraryImport(DllName, EntryPoint = "create_mlkem_key_pair_aot")]
internal static partial CError CreateMlKemKeyPairAot(
  out IntPtr priv_key_ptr, out int priv_key_length,
  out IntPtr pub_key_ptr, out int pub_key_length,
  out IntPtr guid_id_ptr, out int guid_id_length,
  out byte mlkem_param, out byte crypto_algo);
```

```
[LibraryImport(DllName, EntryPoint = "pqc_mlkem_encryption_aot")]
internal static partial CError PqcMlKemEncryptionAot(
  ReadOnlySpan<byte> message, int message_length,
  ReadOnlySpan<byte> private_key, int private_key_length,
  ReadOnlySpan<byte> capsulation, int capsulation_length,
  ReadOnlySpan<byte> associated, int associated_length,
  byte mlkem_param, byte crypto_algo,
  out IntPtr cipher, out int cipher_length);

[LibraryImport(DllName, EntryPoint = "pqc_mlkem_decryption_aot")]
internal static partial CError PqcMlKemDecryptionAot(
  ReadOnlySpan<byte> cipher, int cipher_length,
  ReadOnlySpan<byte> shared_key, int shared_key_length,
  ReadOnlySpan<byte> associated, int associated_length,
  byte mlkem_param, byte crypto_algo,
  out IntPtr decipher_ptr, out int decipher_length);
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

**End of API Reference**

