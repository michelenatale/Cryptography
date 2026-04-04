## 📘 C‑Abi‑Bridge‑Aot

*A high‑performance NativeAOT C‑ABI bridge for cryptographic operations*

C‑Abi‑Bridge‑Aot is a **NativeAOT‑compiled, cross‑language, C‑ABI‑compatible** cryptography runtime designed to be consumed from any language that can call C functions — including **C, C++, Rust, Go, Zig, Python, Java, .NET, and more**.

It provides a stable, minimal, and allocation‑safe API surface for:

- AES (ECB, CBC, GCM)
- ChaCha20‑Poly1305
- Hashing (SHA‑2, SHA‑3, BLAKE2)
- HMAC
- Random number generation
- Post‑quantum cryptography (ML‑KEM, ML‑DSA, SLH‑DSA, XMSS, LMS)
- Encoding utilities (Base64, Hex)
- File and stream encryption
- Secure memory utilities

The library is compiled as a **NativeAOT single‑file** DLL, making it extremely fast to load, dependency‑free, and safe to distribute.

---

## 🚀 Features

- NativeAOT single‑file DLL (~3 MB)
- C ABI stable interface
- Cross‑language compatibility
- Automatic .def + .lib generation for C++
- Zero managed dependencies
- High‑performance cryptography
- Memory‑safe utilities
- Full test suite (C++, C#, VB.NET)

---

## 🧩 Architecture Overview

```
C-Abi-Bridge-Aot/
 ├─ Docs/                     # Documentation
 ├─ Build/
 │  ├─ Artifacts/             # Generated .def and .lib files
 │  ├─ Tools/                 # make-def.bat and helper scripts
 ├─ Src/
 │  ├─ CAbiBridge.Aot         # NativeAOT project (DLL)
 │  ├─ CAbiBridge.Crypto      # Crypto implementation
 │  └─ CAbiBridge.Utils       # Utility layer
 ├─ Tests/
 │  ├─ C-Abi-Bridge.Aot.Cpp.Tests
 │  ├─ C-Abi-Bridge.Aot.Vb.Tests
 │  ├─ CAbiBridge.Crypto.Tests
 │  └─ CAbiBridgeAot.Tests
 ├─ README.md
 ├─ CHANGELOG.md
 └─ C-Abi-Bridge.slnx
```

---

## 🔧 Build Pipeline

The build pipeline is fully automated and consists of three stages:

