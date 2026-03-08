# 🧩 Architecture

This document explains the internal architecture of the C-Abi-Bridge-Aot module and how it interacts with the .NET NativeAOT runtime.

---

## High-Level Structure

```
[ NativeAOT Library ]
↑
│  C ABI (exported functions)
↓
[ C-Abi-Bridge-Aot ]
↑
│  Language bindings
↓
[ C / C++ / Rust / Zig / Go / Python ]
```

---

## Components

### 1. NativeAOT Layer
Implements the actual cryptographic logic in C# and compiles it to a native library.

### 2. C ABI Layer
Defines stable exported functions using:

- Plain C types
- No exceptions
- No managed pointers
- Explicit memory ownership

### 3. Bridge Layer
Provides:

- Type conversions
- Buffer allocation
- Error reporting
- Decimal bit‑level translation

### 4. Consumer Layer
Any language that can call native functions.

---

## Diagram

**Architecture Diagram:**

[Docs/Images/architecture-diagram.md](../Images/architecture-diagram.md)

---

## Error Handling

All functions return:

```
typedef struct CError {
    int32_t error_code;
    int32_t reserved;
} CError;
```
`error_code == 0` means success.

---

## Memory Ownership

All buffers returned by the ABI must be freed using free_buffer.

Consumers must not free memory using free, delete, or custom allocators.

See [memory-management.md](memory-management.md) for details.
```
if (err.error_code == 0) {
    // use values
}

free_buffer(values);
```

---


