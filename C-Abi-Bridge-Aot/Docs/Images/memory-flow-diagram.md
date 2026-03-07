# 🧠 Memory-Flow Diagram

This diagram illustrates how  memory is allocated and released in the ABI.
```
+---------------------------+
|   NativeAOT allocates     |
|   buffer (internal heap)  |
+-------------+-------------+
              |
              v
+---------------------------+
|   Pointer returned to     |
|   C / C++ / Rust / etc.   |
+-------------+-------------+
              |
              v
+---------------------------+
|   User processes data     |
+-------------+-------------+
              |
              v
+---------------------------+
|   free_buffer(ptr)        |
|   (NativeAOT frees heap)  |
+---------------------------+
```

---

[← Back to Docs](../README-DOCS.md)
