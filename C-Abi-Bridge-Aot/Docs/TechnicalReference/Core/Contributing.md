# ⭐ Contributing to C‑Abi‑Bridge‑Aot

**Status**: ✅   
**Version**: 0.2.0   
**Last Updated**: 2026.04.07

---

Thank you for your interest in contributing to **C‑Abi‑Bridge‑Aot**!  

This project aims to provide a high‑performance, NativeAOT‑compiled, C‑ABI‑compatible cryptography runtime that can be consumed from any language.  

Contributions are welcome and appreciated.

---

## 1. How to Contribute

There are several ways to contribute:

- Reporting bugs  
- Improving documentation  
- Adding new cryptographic features  
- Enhancing the build pipeline  
- Improving cross‑language interop  
- Writing tests (C++, C#, VB.NET)  
- Providing examples for other languages (Rust, Go, Zig, Python, etc.)

Before submitting a pull request, please read the guidelines below.

---

## 2. Development Environment

### Requirements

- .NET 10.0 (NativeAOT)
- Visual Studio 2022 or newer
- C++ build tools (MSVC)
- Windows SDK
- `lib.exe` (Microsoft Library Manager)
- Git

Optional:

- CMake (for external C++ examples)
- Python, Rust, Go, Zig (for interop examples)

---

## 3. Repository Structure

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

## 4. Build Instructions

### 4.1 Build the NativeAOT DLL

```bash
dotnet publish -c Release -r win-x64
```
This generates the NativeAOT single‑file DLL:
```
Src/CAbiBridge.Aot/Build/net10.0/publish/C-Abi-Bridge.Aot.dll
```

### 4.2 Generate .def and .lib files

This happens automatically after publish:
- make-def.bat extracts exported symbols
- lib.exe generates the import library
- Files are stored in Build/Artifacts

### 4.3 Run Tests

#### .NET tests
```
 dotnet test
```

#### C++ tests

Open and run:
```
Tests/C-Abi-Bridge.Aot.Cpp.Tests
```

--- 

## 5. Coding Guidelines

- Use clear, minimal, C‑ABI‑safe types
- Avoid allocations in exported functions
- Prefer fixed‑size buffers over dynamic memory
- Keep the API surface stable
- Document all exported functions
- Ensure ABI compatibility across languages
- Follow .NET NativeAOT restrictions

---

## 6. Pull Requests

Before submitting a PR:
1. Ensure the project builds successfully
2. Run all tests
3. Update documentation if needed
4. Update CHANGELOG.md if applicable
5. Keep commits clean and focused

PRs that break ABI compatibility must be discussed first.

---

## 7. Reporting Issues

When reporting a bug, please include:

- Steps to reproduce
- Expected behavior
- Actual behavior
- Logs or error messages

Environment details (OS, .NET version, compiler version)

---

## 8. License

By participating, you agree that your contributions will be licensed under the license mentioned above.

---

Thank you for helping improve C‑Abi‑Bridge‑Aot!

