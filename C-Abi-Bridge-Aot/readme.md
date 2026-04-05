# 📘 C‑Abi‑Bridge‑Aot

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

# 🚀 Features

- NativeAOT single‑file DLL (~3 MB)
- C ABI stable interface
- Cross‑language compatibility
- Automatic .def + .lib generation for C++
- Zero managed dependencies
- High‑performance cryptography
- Memory‑safe utilities
- Full test suite (C++, C#, VB.NET)

---

# 🧩 Architecture Overview

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

# 🔧 Build Pipeline

The build pipeline is fully automated and consists of three stages:

## 1. NativeAOT Publish (DLL generation)

Run:

```
dotnet publish -c Release -r win-x64
```

This produces:
```
Src/CAbiBridge.Aot/Build/net10.0/publish/C-Abi-Bridge.Aot.dll
```

This DLL is the single‑file NativeAOT binary.

## 2. Automatic DEF + LIB generation

After publishing, MSBuild automatically:

1. Runs make-def.bat  
→ extracts exported symbols from the DLL
→ generates C-Abi-Bridge.Aot.def

2. Runs lib.exe  
→ generates C-Abi-Bridge.Aot.lib

3. Stores both files in:

```
Build/Artifacts/
```

These files are stable, versioned artifacts used by C++.

## 3. Consumer Projects Copy the DLL

Each test project (C#, VB.NET, C++) contains a CopyAotDllAfterBuild target:
- C# and VB.NET copy the DLL into their OutDir and rename it to C-Abi-Bridge.Aot.N.dll
- C++ copies:
  - DLL → OutDir
  - LIB → OutDir

This ensures all consumers always use the latest build.

---

# 🧪 Tests

The repository includes a full test suite:

- C++ tests (linking against the generated .lib)
- VB.NET tests (P/Invoke)
- C# tests (P/Invoke)
- Crypto unit tests (managed)

The C++ tests validate:

- ABI stability
- Struct layout
- Memory ownership
- Error handling

All cryptographic operations

---

# 🔌 Using the Library

## C++

Link against the generated .lib:

```
<AdditionalLibraryDirectories>$(SolutionDir)Build\Artifacts</AdditionalLibraryDirectories>
<AdditionalDependencies>C-Abi-Bridge.Aot.lib</AdditionalDependencies>
```

Load the DLL at runtime:

```
#include "cabi_exp_imp.h"
auto result = cabi_crypto_random_bytes(buffer, size);
```

---

## C# / VB.NET

Use P/Invoke:

```
[DllImport("C-Abi-Bridge.Aot.N.dll")]
public static extern int cabi_crypto_random_bytes(byte[] buffer, int size);
```

--- 

# 📦 Artifacts

The following files are generated and stored in:

```
Build/Artifacts/
```

| File | Purpose |
| --- | --- |
| ``C-Abi-Bridge.Aot.def`` | Export definition file (C ABI) |
| ``C-Abi-Bridge.Aot.lib`` | Import library for C++ |
| *(DLL is not stored here)* | DLL stays in Publish folder |

---

# 🛠 Development Guide

## Rebuild everything:

```
dotnet publish -c Release -r win-x64
```

## Regenerate DEF + LIB:

Automatically done after publish.

## Run C++ tests:

Build and run:

```
Tests/C-Abi-Bridge.Aot.Cpp.Tests
```

## Run .NET tests:

```
dotnet test
```

---

# 📄 License

GPL-3.0 License
© Michele Natale 2026

---

# 🙌 Credits

Developed by Michele Natale  
NativeAOT, C ABI, and cryptography engineering.

---

