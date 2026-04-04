
---

# 📝 CHANGELOG.md

```markdown
# Changelog  
All notable changes to this project will be documented in this file.

The format is based on **Keep a Changelog**  
and this project adheres to **Semantic Versioning (SemVer)**.

---

## 1.0.0 — Initial Release

### Added
- NativeAOT-compatible C ABI for:
  - NextCryptoBool / Byte / Int32 / Int64 / Single / Double / Decimal
  - RngCryptoBool / Byte / Int32 / Int64 / Single / Double / Decimal
  - Range variants for all numeric types
- AES (CBC) file and byte APIs
- AES-GCM file and byte APIs
- ChaCha20-Poly1305 file and byte APIs
- Full Decimal interop (GetBits / WriteInt32)
- Explicit unmanaged memory allocation and FreeBuffer
- Extensive tests and performance benchmarks
- Clean project structure for future extensions

---

## [0.1.0] – 2026-04-04  
### Added
- Initial release of **C‑Abi‑Bridge‑Aot**, a NativeAOT‑compiled C‑ABI bridge for cryptographic operations.
- NativeAOT single‑file DLL (`C-Abi-Bridge.Aot.dll`) with stable C ABI exports.
- Automated generation of:
  - `.def` export definition file  
  - `.lib` import library for C++ consumers  
- Build pipeline with:
  - NativeAOT publish step  
  - Automatic DEF + LIB generation  
  - Artifact storage under `Build/Artifacts`  
  - Tooling under `Build/Tools`  
- Complete cryptography feature set:
  - AES (CBC-AEAD, GCM)
  - ChaCha20‑Poly1305
  - SHA‑3 hashing
  - HMAC-SHA‑3
  - Random number generation
  - Encoding utilities (Base64, Hex)
  - File and stream encryption
  - Post‑quantum cryptography (ML‑KEM, ML‑DSA, [SLH‑DSA later])
- Cross‑language support:
  - C++
  - C#
  - VB.NET
  - Any language supporting C ABI
- Test suite:
  - C++ ABI tests
  - C# P/Invoke tests
  - VB.NET P/Invoke tests
  - Managed crypto tests
- Project structure with:
  - `Src/` for implementation
  - `Tests/` for all test projects
  - `Build/` for artifacts and tools
  - `Docs/` for documentation
- Initial documentation and repository layout.

---

## [Unreleased]
### Planned
- Rust, Go, Zig, and Python interop examples.
- Additional PQC algorithms.
- Extended documentation (API reference, diagrams).
- Continuous Integration (CI) pipeline.
- Performance benchmarks and comparison charts.
```

---

