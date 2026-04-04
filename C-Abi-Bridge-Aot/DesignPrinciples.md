# 📘 Design Principles  

This document explains the core design principles behind **C‑Abi‑Bridge‑Aot**, including why the project uses NativeAOT, why it exposes a C ABI, and why the internal structure is organized into clear layers.

---

# 1. Philosophy

C‑Abi‑Bridge‑Aot is built on a simple idea:

> **Provide a fast, secure, dependency‑free cryptographic runtime that works in every programming language.**

To achieve this, the project follows a strict set of design principles that ensure:

- Predictable performance  
- Maximum interoperability  
- Minimal attack surface  
- Clear separation of concerns  
- Long‑term maintainability

---

# 2. Why NativeAOT?

NativeAOT was chosen for several reasons:

### ✔ Zero Dependencies  
The compiled DLL contains everything it needs — no .NET runtime, no JIT, no GC.

### ✔ Predictable Performance  
Native code, no warm‑up time, no runtime compilation.

### ✔ Security Benefits  
- No JIT = no JIT‑based attacks  
- No GC = no object relocation  
- No reflection  
- No dynamic code generation  

### ✔ Perfect for Cross‑Language Interop  
NativeAOT produces a **real native DLL**, which behaves exactly like a C library.

### ✔ Small, Self‑Contained Artifacts  
Ideal for embedding into applications, distributing with installers, or shipping with C++ projects.

---

# 3. Why a C ABI?

The C ABI is the **lowest common denominator** across programming languages.

Every major language can call C functions:

- C / C++
- Rust
- Go
- Zig
- Python (ctypes / cffi)
- Java (JNI)
- C# / VB.NET (P/Invoke)
- Swift
- Kotlin Native
- Lua
- Ruby
- Many more

### Benefits:

### ✔ Universal Compatibility  
No wrappers, no bindings, no runtime dependencies.

### ✔ Stable Function Signatures  
Flat, unmangled names ensure ABI stability.

### ✔ Simple Types  
Only primitive types (`uint8_t*`, `int32_t`, etc.)  
→ No structs, no opaque handles, no complex marshalling.

### ✔ Predictable Memory Model  
Caller‑allocated buffers avoid ownership confusion.

---

# 4. Why This Project Structure?

The repository is organized into three layers:

AOT Export Layer (CAbiBridge.Aot)
Crypto Layer (CAbiBridge.Crypto)
Utility Layer (CAbiBridge.Utils)


### ✔ Clear Separation of Concerns  
Each layer has a single responsibility.

### ✔ Testability  
Crypto logic can be tested independently of the C ABI.

### ✔ Maintainability  
Changes in one layer do not affect others.

### ✔ ABI Stability  
The AOT layer is thin and stable — ideal for long‑term compatibility.

---

# 5. Why Caller‑Allocated Buffers?

The library uses a strict memory model:

- Caller allocates all buffers  
- Library writes into them  
- Library never frees caller memory  
- Library rarely allocates memory  

### Benefits:

### ✔ No hidden allocations  
Predictable memory usage.

### ✔ No GC involvement  
Safe for unmanaged languages.

### ✔ No ownership confusion  
Clear rules for all languages.

### ✔ Zero‑copy design  
Maximum performance.

---

# 6. Why No Exceptions?

All functions return an `int` error code.

### Reasons:

### ✔ Cross‑language safety  
Exceptions do not cross ABI boundaries.

### ✔ Predictable control flow  
No hidden stack unwinding.

### ✔ Simplicity  
Every language understands integer error codes.

### ✔ Security  
No exception metadata, no stack traces leaking sensitive data.

---

# 7. Why Automated DEF + LIB Generation?

The build pipeline automatically generates:

- `.def` (export definition file)
- `.lib` (C++ import library)

### Benefits:

### ✔ Guaranteed ABI correctness  
Symbol names always match the DLL.

### ✔ No manual maintenance  
No risk of outdated DEF files.

### ✔ Seamless C++ integration  
MSVC can link against the library without extra configuration.

---

# 8. Why Minimalism?

The API intentionally avoids:

- Complex structs  
- Opaque handles  
- Dynamic memory allocation  
- Exceptions  
- Classes or objects  
- Templates or generics  

### Why?

### ✔ Simplicity  
Easy to understand, easy to use.

### ✔ Portability  
Works everywhere.

### ✔ Security  
Smaller surface area, fewer attack vectors.

### ✔ Stability  
Simple APIs remain stable for years.

---

# 9. Why Strict Validation?

Every exported function validates:

- Pointers  
- Lengths  
- Ranges  
- Null values  
- Algorithm parameters  

### Benefits:

### ✔ Prevents undefined behavior  
Especially important in C and C++.

### ✔ Protects against misuse  
Invalid parameters return clear error codes.

### ✔ Ensures cryptographic correctness  
No silent failures.

---

# 10. Summary of Core Principles

| Principle | Description |
|----------|-------------|
| **NativeAOT** | Fast, secure, dependency‑free native binaries |
| **C ABI** | Universal interoperability |
| **Layered Architecture** | Clear separation of concerns |
| **Caller‑Allocated Buffers** | Predictable memory ownership |
| **No Exceptions** | Safe cross‑language error handling |
| **Minimalism** | Simple, stable, secure API |
| **Automated Artifacts** | Reliable DEF + LIB generation |
| **Strict Validation** | Prevents misuse and undefined behavior |

---

