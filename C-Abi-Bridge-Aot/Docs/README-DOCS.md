# 📘 C-Abi-Bridge-Aot Documentation

This folder contains the extended technical documentation for the **C-Abi-Bridge-Aot** module.  
While the main `README.md` provides a high-level overview, this section goes deeper into architecture, memory rules, interop details, and usage examples across multiple languages.

---

## 📚 Contents

### 🔷 Overview
- [Overview](Contents/overview.md)  
  High-level explanation of the module, goals, features, and supported platforms.

### 🧩 Architecture
- [Architecture](Contents/architecture.md)  
  Internal structure, layers, error handling, and how the C ABI interacts with NativeAOT.

### 🧠 Memory Management
- [Memory Management](Contents/memory-management.md)  
  Allocation rules, buffer lifetime, and why `free_buffer` is required.

### 🔢 Decimal Interop
- [Decimal Interop](Contents/decimal-interop.md)  
  Bit layout, flags, scale, and how .NET `decimal` is represented in the C ABI.

### 💻 Usage Guides
- [Using the C ABI from C](Contents/c-usage.md)  
  Examples for calling the ABI from plain C.

- [Using the C ABI from C#](Contents/csharp-usage.md)  
  P/Invoke examples, struct layouts, and memory handling.

### 🧪 Testing
- [Testing](Contents/testing.md)  
  Overview of the test suites (C#, VB.NET, C++), including the CMake-based C++ tests.

---

## 🎬 Video Demonstration

A demonstration video showing the module in action is available in:

[Docs/c-abi-bridge-aot.mp4](Videos/c-abi-bridge-aot_3_24671.mp4)

---

## 🖼️ Images & Diagrams

Supporting diagrams used across the documentation:

[Docs/images/architecture.png]()
[Docs/images/memory-flow.png]()
[Docs/images/decimal-layout.png]()

---

## 📌 Notes

- All documentation files are designed to be platform‑agnostic.
- The Docs folder will grow as new features and APIs are added.
- Contributions and improvements are welcome.

---

## 🔗 Back to Main Project

Return to the main module:

➡️ [C-Abi-Bridge-Aot](../)

