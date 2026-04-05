# 📘 Frequently Asked Questions (FAQ)

This document answers common questions about building, using, and integrating the **C‑Abi‑Bridge‑Aot** NativeAOT library.

---

## 1. What is C‑Abi‑Bridge‑Aot?

C‑Abi‑Bridge‑Aot is a **NativeAOT‑compiled**, **C‑ABI‑compatible** cryptography runtime.  
It exposes a stable C interface that can be consumed from any language capable of calling C functions.

---

## 2. Which platforms are supported?

Currently:

- **Windows x64** (NativeAOT)
- MSVC toolchain for C++ consumers

Planned:

- Linux x64
- macOS ARM64 / x64

---

## 3. What does the build pipeline generate?

A full build produces:

- A NativeAOT single‑file DLL  
- A `.def` export definition file  
- A `.lib` import library for C++  

The DLL is used by all languages.  
The `.lib` is used only by C++.

---

## 4. Where are the generated files located?

- DLL:  
  `Src/CAbiBridge.Aot/Build/<tfm>/publish/`

- DEF + LIB:  
  `Build/Artifacts/`

---

## 5. Why is the DLL not stored in `Build/Artifacts`?

Because:

- The DLL is a **volatile build output**
- It changes with every publish
- It is copied directly into each consumer project’s `OutDir`

Artifacts are reserved for **stable, versioned files** (`.def`, `.lib`).

---

## 6. How do I regenerate the `.def` and `.lib` files?

Simply run:

```bash
dotnet publish -c Release -r win-x64
```

MSBuild automatically:
- Runs `make-def.bat`
- Runs `lib.exe`
- Stores results in `Build/Artifacts`

---

## 7. How do I use the library in C++?

Link against the import library:

```
<AdditionalLibraryDirectories>$(SolutionDir)Build\Artifacts</AdditionalLibraryDirectories>
<AdditionalDependencies>C-Abi-Bridge.Aot.lib</AdditionalDependencies>
```

Load the DLL at runtime and call the exported functions.

---

## 8. How do I use the library in C# or VB.NET?

Use P/Invoke:
```
[DllImport("C-Abi-Bridge.Aot.N.dll")]
public static extern int cabi_crypto_random_bytes(byte[] buffer, int size);
```

---

## 9. Are the exported functions thread‑safe?

Yes.
All cryptographic operations are thread‑safe unless explicitly documented otherwise.

---

# 10. Does the library allocate memory?

Most functions do not allocate memory.
The caller must allocate all buffers.

If a function allocates memory, it will be documented and must be freed using:

```
cabi_free_buffer(ptr);
```

---

## 11. What calling convention is used?

All exported functions use:

```
`extern "C"`
`__cdecl`
No name mangling
```

---

## 12. How are errors reported?

All functions return an int:

```
`0` = success
Negative values = error codes
See `Docs/Errors.md` for details.
```

See Docs/Errors.md for details.

---

## 13. Can I use this library from Rust, Go, Zig, or Python?

Yes.
Examples are provided in `Docs/Interop.md`.

---

## 14. Is the library FIPS‑certified?

No.
The library is not FIPS‑certified and is not intended for high‑assurance or regulated environments without independent review.

---

## 15. How stable is the C ABI?

The ABI is stable and versioned.
Breaking changes only occur in major versions.

---

## 16. How do I report a security issue?

Do not open a public GitHub issue.
See `SECURITY.md` for responsible disclosure instructions.

---

## 17. Why NativeAOT instead of a normal .NET runtime?

NativeAOT provides:
- Zero dependencies
- No JIT
- No GC interaction
- Predictable performance
- Small single‑file binaries
- Perfect for cross‑language interop

---

## 18. Can I embed this DLL in another application?

Yes.
The DLL is self‑contained and safe to redistribute.

---
