# 🧠 Memory-Flow Diagram

Dieses Diagramm veranschaulicht, wie Speicher im ABI zugewiesen und freigegeben wird.
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
