# 📘 Security Policy  

This document describes how security is handled in the **C‑Abi‑Bridge‑Aot** project, including vulnerability reporting, cryptographic guarantees, and secure usage guidelines.

---

# 1. Supported Versions

Only the latest release of **C‑Abi‑Bridge‑Aot** receives security updates.

| Version | Supported |
|--------|-----------|
| Latest | ✔ Yes |
| Older releases | ✖ No |

Security fixes are applied only to the most recent version to ensure consistency and prevent fragmentation.

---

# 2. Reporting a Vulnerability

If you discover a security vulnerability, **do not open a public GitHub issue**.

Instead, please report it privately:

- Email: **michele.natale.net@proton.me**  
- Subject line: `C-Abi-Bridge-Aot Security Report`

Please include:

- A detailed description of the issue  
- Steps to reproduce  
- Potential impact  
- Suggested fixes (optional)  
- Your environment (OS, compiler, .NET version)  

Please note that the interaction between NativeAOT, platform compatibility, and low‑level runtime behavior is still an evolving area within .NET. In some cases, it may be appropriate to contact Microsoft directly so that valuable insights and platform‑level issues can be captured, analyzed, and incorporated into the ongoing improvement of the ecosystem.

---

# 3. Cryptographic Guarantees

The library provides cryptographic primitives through a **NativeAOT‑compiled C ABI**.  
It guarantees:

- No managed allocations inside cryptographic operations  
- No garbage‑collector interaction  
- No dynamic dispatch  
- No reflection  
- No JIT compilation  
- No runtime dependencies  
- Constant‑time implementations where applicable  
- Zeroing of sensitive buffers when possible  

However:

- **This library does not claim FIPS certification**  
- **This library is not a replacement for audited cryptographic libraries**  
- **Use in high‑assurance environments requires independent review**

---

# 4. Secure Usage Guidelines

To ensure safe usage:

### ✔ Always validate buffer sizes  
Most functions return `-2` if the output buffer is too small.

### ✔ Always check return codes  
Never assume success.

### ✔ Do not reuse IVs or nonces  
Especially for AES‑GCM and ChaCha20‑Poly1305.

### ✔ Use strong random keys  
Use `crypto_random_bytes_aot` to generate keys.

### ✔ Zero sensitive memory  
Use secure zeroing functions where appropriate.

### ✔ Avoid exposing raw keys in logs or exceptions  
Never print keys or intermediate values.

---

# 5. Memory Safety

The library follows strict memory rules:

- All buffers must be allocated by the **caller**
- The library never frees caller‑allocated memory
- Functions that allocate memory explicitly document this behavior
- Use `free_buffer_aot` to free library‑allocated memory
- Sensitive buffers are cleared when possible

The NativeAOT runtime ensures:

- No GC relocations  
- No object moves  
- No managed pointers escaping into native code  

---

# 6. ABI Stability

The C ABI is designed to be:

- Stable  
- Backwards compatible  
- Language‑agnostic  

Breaking changes will only occur in **major versions**.

---

# 7. Cryptographic Algorithms

The library includes:

- AES (CBC-AEAD, GCM)
- ChaCha20‑Poly1305
- SHA‑3
- HMAC
- Random number generation
- Encoding utilities
- Post‑quantum algorithms (ML‑KEM, ML‑DSA, [SLH‑DSA later])

All algorithms are implemented using well‑established primitives and follow best practices.

---

# 8. Threat Model

The library assumes:

### In‑scope:
- Local attackers attempting to read memory  
- Side‑channel‑resistant operations (best effort)  
- Safe cross‑language interop  
- Memory safety in unmanaged environments  

### Out‑of‑scope:
- Hardware attacks  
- Kernel‑level compromise  
- Physical access attacks  
- Side‑channel resistance on all platforms (not guaranteed)  
- Misuse of cryptographic primitives  

---

# 9. Responsible Disclosure

We follow a **90‑day responsible disclosure policy**:

1. Vulnerability reported privately  
2. Fix developed and tested  
3. Patch released  
4. Public disclosure after patch availability  

Contributors must not disclose vulnerabilities before a fix is published.

---

# 10. Disclaimer

This project is provided under the MIT License.  
No warranty is given regarding:

- Fitness for a particular purpose  
- Suitability for high‑assurance environments  
- Compliance with regulatory standards  

Use at your own risk.

---

# Thank You

Security researchers and contributors help keep this project safe and reliable.  
We appreciate your efforts and responsible disclosure.

---

