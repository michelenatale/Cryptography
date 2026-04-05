
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

1. Runs `make-def.bat`
   - Extracts exported symbols from the DLL
   - Generates `C-Abi-Bridge.Aot.def`
2. Runs `lib.exe`
   - Generates `C-Abi-Bridge.Aot.lib`
3. Stores both files in:

```
Build/Artifacts/
```

These files are stable **artifacts** used by C++ consumers.

The DLL is **not** stored here — it remains in the Publish folder.

---

## 3.3 Consumer Projects Copy the DLL

Each test project contains a `CopyAotDllAfterBuild` target:

## C# / VB.NET

- Copy DLL → OutDir
- Rename to `C-Abi-Bridge.Aot.N.dll`

## C++

- Copy DLL → OutDir
- Copy LIB → OutDir
- Link against the LIB

This ensures all consumers always use the **latest build**.

---

# 4. Using the Library

## 4.1 C++ (linking against the import library)

Add to your `.vcxproj`:
```
<AdditionalLibraryDirectories>$(SolutionDir)Build\Artifacts</AdditionalLibraryDirectories>
<AdditionalDependencies>C-Abi-Bridge.Aot.lib</AdditionalDependencies>
```

Load the DLL at runtime:
```
#include "cabi_exp_imp.h"

uint8_t buffer[32];
auto result = cabi_crypto_random_bytes(buffer, sizeof(buffer));
```

## 4.2 C# (P/Invoke)

```
[LibraryImport("C-Abi-Bridge.Aot.N.dll")]
public static extern int cabi_crypto_random_bytes(byte[] buffer, int size);
```

## 4.3 VB.NET (P/Invoke)

```
<DllImport("C-Abi-Bridge.Aot.N.dll")>
Public Shared Function cabi_crypto_random_bytes(
    buffer As Byte(),
    size As Integer
) As Integer
End Function
```
---

# 5. Running Tests

## 5.1 .NET Tests

Run all managed tests:

```
dotnet test
```

## 5.2 C++ Tests

Open:
```
Tests/C-Abi-Bridge.Aot.Cpp.Tests
```

Build and run the test executable.

These tests validate:
- ABI stability
- Struct layout
- Memory ownership
- Error handling
- All cryptographic operations

---

## 6. Regenerating Artifacts

Artifacts are regenerated automatically after each publish.

To manually regenerate:
```
dotnet publish -c Release -r win-x64
```

This will:
- Rebuild the DLL
- Regenerate the `.def`
- Regenerate the `.lib`
- Update `Build/Artifacts/`

--- 

## 7. Release Workflow

1. Update version in [`CHANGELOG.md`](https://github.com/michelenatale/Cryptography/blob/main/C-Abi-Bridge-Aot/CHANGELOG.md)
2. Run full publish:
```
dotnet publish -c Release -r win-x64
```
3. Verify:
   - DLL in Publish folder
   - DEF + LIB in Artifacts folder
4. Run all tests
5. Commit and tag release
6. Push to GitHub

---

# 8. Notes

- The DLL is intentionally **not** stored in `Build/Artifacts`
- Only `.def` and `.lib` are versioned artifacts
- The DLL is always copied fresh into each consumer’s `OutDir`
- The build pipeline is fully automated and requires no manual steps

# 9. License

GPL-3.0 License
© Michele Natale 2026

---

