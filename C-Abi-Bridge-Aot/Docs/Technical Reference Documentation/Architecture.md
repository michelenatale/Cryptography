
# 📘 High‑Level Architecture  

This document provides an overview of the internal architecture of **C‑Abi‑Bridge‑Aot**, including its design goals, layering model, build pipeline, and cross‑language integration strategy.

---

# 1. Design Goals

The architecture of C‑Abi‑Bridge‑Aot is built around the following principles:

- **Universal interoperability**  
  A stable C ABI that works with any language.

- **Native performance**  
  NativeAOT compilation for predictable, dependency‑free execution.

- **Security‑focused design**  
  No GC interaction, no JIT, no dynamic allocations inside crypto operations.

- **Strict layering**  
  Clear separation between AOT layer, crypto layer, and utility layer.

- **Minimalism**  
  Only primitive types, no complex structs, no opaque handles.

- **Deterministic build outputs**  
  Automated generation of `.def` and `.lib` files for C++ consumers.

---

# 2. Layered Architecture Overview

The project is organized into three main layers:
```
┌──────────────────────────────────────────────┐
│                AOT Export Layer               │
│          (CAbiBridge.Aot — NativeAOT)         │
└──────────────────────────────────────────────┘
▲                 ▲
│                 │
┌──────────────────────────────────────────────┐
│                Crypto Layer                   │
│          (CAbiBridge.Crypto — AES, GCM,       │
│           ChaCha20‑Poly1305, Hashing, HMAC)   │
└──────────────────────────────────────────────┘
▲
│
┌──────────────────────────────────────────────┐
│                Utility Layer                  │
│          (CAbiBridge.Utils — Memory,          │
│           Encoding, Validation, Helpers)      │
└──────────────────────────────────────────────┘
```

### 2.1 AOT Export Layer (CAbiBridge.Aot)

Responsibilities:

- Exposes the **C ABI** (`extern "C"`, `__cdecl`)
- Performs argument validation
- Converts between unmanaged and managed types
- Calls into the Crypto Layer
- Ensures no managed allocations escape into native code
- Compiles into a **single‑file NativeAOT DLL**

### 2.2 Crypto Layer (CAbiBridge.Crypto)

Responsibilities:

- Implements cryptographic algorithms:
  - AES‑CBC, AES‑GCM
  - ChaCha20‑Poly1305
  - SHA
  - HMAC
  - Random number generation
- Ensures constant‑time operations where applicable
- Provides safe, allocation‑free APIs for the AOT layer

### 2.3 Utility Layer (CAbiBridge.Utils)

Responsibilities:

- Memory helpers
- Encoding (Base64, Hex)
- Range validation
- Zeroing sensitive memory
- Common helper routines shared across layers

---

# 3. Build Pipeline Architecture

The build pipeline consists of three automated stages:
```
dotnet publish (NativeAOT)
│
▼
NativeAOT DLL (C-Abi-Bridge.Aot.dll)
│
├── make-def.bat → C-Abi-Bridge.Aot.def
└── lib.exe      → C-Abi-Bridge.Aot.lib
│
▼
Build/Artifacts/ (.def, .lib)
```

### 3.1 NativeAOT Publish

- Produces a **single‑file**, **dependency‑free** DLL
- No JIT, no GC interaction, no runtime dependencies

### 3.2 DEF Generation

- Extracts exported symbols from the DLL
- Ensures stable, unmangled C function names

### 3.3 LIB Generation

- Produces a C++ import library
- Enables seamless linking in MSVC projects

---

# 4. Cross‑Language Integration Architecture

The architecture is designed for universal consumption:
```
C ABI → C++ / Rust / Go / Zig / Python / Java / C# / VB.NET
```

### 4.1 C++  
Links against `.lib`, loads DLL at runtime.

### 4.2 C# / VB.NET  
Uses P/Invoke with `DllImport`.

### 4.3 Rust  
Uses `extern "C"` FFI bindings.

### 4.4 Go  
Uses cgo with direct symbol imports.

### 4.5 Zig  
Imports `.def` directly.

### 4.6 Python  
Uses `ctypes` or `cffi`.

---

# 5. Error Handling Architecture

- All functions return an `int`
- `0` = success  
- Negative values = error codes  
- No exceptions, no panics, no thrown errors  
- Fully deterministic and language‑agnostic

See `Docs/Errors.md` for details.

---

# 6. Memory Architecture

- Caller‑allocated buffers for all operations
- No hidden allocations inside crypto functions
- Library‑allocated memory must be freed with `cabi_free_buffer`
- Sensitive memory is zeroed when possible

See `Docs/Memory.md` for details.

---

# 7. Testing Architecture

The test suite validates:

- ABI stability (C++ tests)
- P/Invoke correctness (C#, VB.NET tests)
- Cryptographic correctness (managed tests)
- Memory safety
- Error handling
- Cross‑language behavior

---

# 8. Architectural Guarantees

The architecture guarantees:

- Stable C ABI across versions
- No GC interaction in exported functions
- No JIT compilation
- No dynamic dispatch
- No reflection
- No runtime dependencies
- Predictable performance
- Safe cross‑language interop

---
 
