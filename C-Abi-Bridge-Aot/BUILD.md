
# 📘 Build Guide  — Developer Guide

This document describes how to build, publish, test, and consume the **C‑Abi‑Bridge‑Aot** NativeAOT library.  
It is intended for developers working on the repository or integrating the library into other projects.

---

# 1. Overview

C‑Abi‑Bridge‑Aot is a **NativeAOT‑compiled**, **C‑ABI‑compatible** cryptography runtime.  
The build pipeline produces:

- A **NativeAOT single‑file DLL**  
- A **.def** export definition file  
- A **.lib** import library for C++ consumers  

The DLL is used by all languages (C#, VB.NET, C++, Rust, Go, Zig, Python, etc.).  
The `.def` and `.lib` files are used only by C++.

---

# 2. Repository Structure

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

# 3. Build Pipeline

The build pipeline consists of three stages:

---

## 3.1 NativeAOT Publish (DLL generation)

Run:

```bash
dotnet publish -c Release -r win-x64
```

This produces:

```
Src/CAbiBridge.Aot/Build/net10.0/publish/C-Abi-Bridge.Aot.dll
```

This DLL is:

- single‑file
- NativeAOT‑compiled
- dependency‑free
- C‑ABI‑compatible

---

## 3.2 Automatic DEF + LIB generation

After publishing, MSBuild automatically:

1. Runs make-def.bat
   - Extracts exported symbols from the DLL

2. Generates C-Abi-Bridge.Aot.def
   - Runs lib.exe

3. Generates C-Abi-Bridge.Aot.lib

Stores both files in:

```
Build/Artifacts/
```

These files are stable artifacts used by C++ consumers.

The DLL is not stored here — it remains in the Publish folder.

---





