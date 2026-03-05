# 🧠 Memory-Flow Diagram

This diagram illustrates how  memory is allocated and released in the ABI.
```
+------------------------+
|     NativeAOT DLL      |
|  (.NET compiled native)|
+-----------+------------+
            |
            |  C ABI (exports)
            v
+------------------------+
|   C-Abi-Bridge-Aot     |
|  (bridge layer, C code)|
+-----------+------------+
            |
            |  language bindings
            v
+------------------------+
| C / C++ / Rust / Zig   |
| Go / Python / others   |
+------------------------+
```

---

[← Back to Docs](README.md)
