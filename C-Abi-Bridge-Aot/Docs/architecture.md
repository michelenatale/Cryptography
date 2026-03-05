# 🧩 Architecture

This document explains the internal architecture of the C-Abi-Bridge-Aot module and how it interacts with the .NET NativeAOT runtime.

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

## Error Handling

All functions return:

```
typedef struct CError {
    int32_t error_code;
    int32_t reserved;
} CError;
```
error_code == 0 means success.

## Memory Ownership

All buffers returned by the ABI must be freed using free_buffer.

Consumers must not free memory using free, delete, or custom allocators.

See memory-management.md for details.

---

# 🧠 **memory-management.md**

```markdown
# Memory Management

The C-Abi-Bridge-Aot module uses a deterministic memory model designed for NativeAOT.

## Allocation Rules

- All buffers returned from the ABI are allocated using a custom allocator.
- Consumers must free these buffers using:

```
void free_buffer(void* ptr);
```

## Why a Custom Free Function?

NativeAOT does not guarantee compatibility with:

- free()
- delete
- C++ allocators
- Rust allocators
- Zig allocators

Using the wrong free function can cause:

- heap corruption
- crashes
- undefined behavior

## Buffer Lifetime

1. ABI allocates memory
2. ABI returns pointer
3. Consumer reads data
4. Consumer calls free_buffer(ptr)

## Example

```
int32_t* values = NULL;
CError err = rng_crypto_int32_aot(128, &values);

if (err.error_code == 0) {
    // use values
}

free_buffer(values);
```
