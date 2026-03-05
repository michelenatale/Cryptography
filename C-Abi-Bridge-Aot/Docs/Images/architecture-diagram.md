# 🧩 Architecture Diagram

This diagram illustrates how the C-Abi-Bridge-Aot module interacts with the NativeAOT library and consuming languages.
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

[← Back to Docs](..\README-DOCS.md)
