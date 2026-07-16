# 📘 C‑Abi‑Bridge‑Aot

**Status**: ✅   
**Version**: 0.2.1   
**Last Updated**: 2026.07.16

---

## 📍 Who Is This For?

This project is intended for developers who require a stable, portable, and high‑performance C‑ABI layer for cryptography and cross‑language interoperability.  
It is especially useful for:

- .NET developers working with NativeAOT or ahead‑of‑time compilation  
- Teams integrating .NET components into C, C++, Rust, Go, Python, or other native environments  
- Developers building plugin systems, sandboxed runtimes, or language‑agnostic libraries  
- Anyone who needs predictable memory behavior, stable binary interfaces, and secure cryptographic primitives across platforms

---

*A high‑performance NativeAOT C‑ABI bridge for cryptographic operations*

C‑Abi‑Bridge‑Aot is a **NativeAOT‑compiled, cross‑language, C‑ABI‑compatible** cryptography runtime designed to be consumed from any language that can call C functions — including **C, C++, Rust, Go, Zig, Python, Java, .NET, and more**.

It provides a stable, minimal, and allocation‑safe API surface for:

- AES (CBC-AEAD, GCM)
- ChaCha20‑Poly1305
- Hashing e.g. (SHA‑2, SHA‑3, BLAKE2)
- HMAC
- Random number generation
- Post‑quantum cryptography e.g. (ML‑KEM, ML‑DSA, SLH‑DSA, XMSS, LMS)
- Encoding utilities (Base64, Hex)
- File and stream encryption
- Secure memory utilities

The library is compiled as a **NativeAOT single‑file** DLL, making it extremely fast to load, dependency‑free, and safe to distribute.

---

## Getting Started

To explore the project:

1. Clone the repository  
2. Open the solution in Visual Studio or JetBrains Rider  
3. Build the `C-Abi-Bridge-Aot` project using the provided publish profiles  
4. Run the C++, C#, or VB.NET tests to see the bridge in action  
5. Review the documentation under `/Docs/TechnicalReference` for detailed architecture and usage information

This gives you a complete end‑to‑end view of how the C‑ABI bridge is built, published, and consumed across languages.


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

### 1. NativeAOT Publish (DLL generation)

Run:

```
dotnet publish -c Release -r win-x64
```

This produces:
```
Src/CAbiBridge.Aot/Build/net10.0/publish/C-Abi-Bridge.Aot.dll
```

This DLL is the single‑file NativeAOT binary.

### 2. Automatic DEF + LIB generation

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

### 3. Consumer Projects Copy the DLL

Each test project (C#, VB.NET, C++) contains a CopyAotDllAfterBuild target:
- C# and VB.NET copy the DLL into their OutDir and rename it to C-Abi-Bridge.Aot.N.dll
- C++ copies:
  - DLL → OutDir
  - LIB → OutDir

This ensures all consumers always use the latest build.

---

## 🧪 Tests

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

## 🔌 Using the Library

### C++

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

### C# / VB.NET

Use P/Invoke:

```
[DllImport("C-Abi-Bridge.Aot.N.dll")]
public static extern int cabi_crypto_random_bytes(byte[] buffer, int size);
```

--- 

## 📦 Artifacts

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

## 🛠 Development Guide

### Rebuild everything:

```
dotnet publish -c Release -r win-x64
```

### Regenerate DEF + LIB:

Automatically done after publish.

### Run C++ tests:

Build and run:

```
Tests/C-Abi-Bridge.Aot.Cpp.Tests
```

### Run .NET tests:

```
dotnet test
```

---

## ➡️ Documentation

Two documentation series are available:

- [**README-DOCS.md**](Docs/README-DOCS.md) – the original documentation set, still useful for historical context  
- [**TechnicalReference.Update1.md**](Docs/TechnicalReference.Update1.md) – the updated and expanded technical reference (introduced with version 0.2.0)

The updated documentation series provides a more structured and detailed view of the architecture, runtime behavior, memory model, and cross‑language integration.

---

## 🎓 Academic Proposal

The C‑ABI‑Bridge‑AOT project includes a dedicated **Academic Proposal** section that provides the full scientific foundation behind the architecture.  

It is designed for universities, research institutes, government agencies, financial institutions, military organizations, and industry stakeholders who require a deeper understanding of the theoretical, normative, and security‑related aspects of the technology.

The Academic Proposal contains:
- Executive Summary (institution‑ready overview)
- Full proposal in German and English
- Structured research roadmaps (Master & PhD)
- Scientific domains and research questions
- Sector relevance (government, finance, military, public infrastructure)
- Formal modeling and verification directions
- ASCII diagrams and content index

You can find all documents here:

➡️ **[C-Abi-Bridge-Aot/Docs/AcademicProposal](https://github.com/michelenatale/Cryptography/tree/main/C-Abi-Bridge-Aot/Docs/AcademicProposal)**

---

### 📌 Changelog

The full version history is available in  
[CHANGELOG.md](CHANGELOG.md).

---

## 📄 License

GPL-3.0 License

**© C-Abi-Bridge-Aot 2026**   
Created by *© Michele Natale 2026*

This project inherits the license of the parent repository Cryptography.

---

## 🙌 Credits

**Author & Lead Architect**  

*Developed by Michele Natale 2026*  
- NativeAOT, C ABI and Cryptography Engineering   
- Concept, API design, C-ABI architecture, documentation, implementation

**Maintainer**  

Michele Natale 2026  
- Maintenance, further development, bug fixing, release management

GitHub: [https://github.com/michelenatale](https://github.com/michelenatale)

---

## 🤝 Invitation to Fork

I started this project with a clear vision — but good ideas, and especially continuous improvement, often emerge over time.

If you believe that something can be solved or designed in a better way:

**Feel free to fork the repository, build your own version, and mention your fork if you like.**

I appreciate every contribution, every piece of feedback, and every improvement.

---

## 🎬 Video Demonstration - Video Mp4 - Downloads

There are 3 videos available:

➡️ [c-abi-bridge-aot-basic.mp4](Docs/Videos/c-abi-bridge-aot_3_24671.mp4) - The original first video, which is still available for viewing.   
➡️ [c-abi-bridge-aot-cpp.mp4](Docs/Videos/c-abi-bridge-aot-2.mp4) - A new video has been created using C++, and the procedure shown in it is still available for viewing.   
➡️ [c-abi-bridge-aot-new.mp4](Docs/Videos/c-abi-bridge-aot.mp4) - The latest video, introduced with Update 1 (version 0.2.0).    

All demonstration videos are available in the [Docs/Video](https://github.com/michelenatale/Cryptography/tree/main/C-Abi-Bridge-Aot/Docs/Videos) folder and via the links above.

---

## 🔍 That could also be of interest.

[.NET-Native-Interop-Suite](https://github.com/michelenatale/.NET-Native-Interop-Suite)

---
