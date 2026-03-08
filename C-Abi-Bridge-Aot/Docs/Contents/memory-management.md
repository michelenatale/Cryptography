# 🧠 Memory Management

The C-Abi-Bridge-Aot module uses a deterministic memory model designed for NativeAOT.

---

## Allocation Rules

- All buffers returned from the ABI are allocated using a custom allocator.
- Consumers must free these buffers using:

```
void free_buffer(void* ptr);
```

---

## Diagram

**Memory-Flow Diagram:**

[Docs/Images/memory-flow-diagram.md](../Images/memory-flow-diagram.md)

---

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

---

## Buffer Lifetime

1. ABI allocates memory
2. ABI returns pointer
3. Consumer reads data
4. Consumer calls **`free_buffer(ptr)`**

---

## Example

```
int32_t* values = NULL;
CError err = rng_crypto_int32_aot(128, &values);

if (err.error_code == 0) {
    // use values
}

free_buffer(values);
```

---

## Thread Safety

All memory operations are thread‑safe.

---


