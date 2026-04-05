# 📘 Technical Reference Documentation  

**Status**: ✅   
**Version**: 0.2.0   
**Last Updated**: 2026.04.07

---

> **Documentation Update 1 / 2026.04.07**  
> This document is part of the updated Technical Reference series.  
> The original documentation series (`README-DOCS.md`) remains available for historical context and may still be useful for understanding the evolution of the project.

---

This file provides a structured overview of all documentation available in the `Docs/` directory.  

Use this as the entry point to navigate the project documentation.

```
Docs /
│
├── TechnicalReference /
│   │
│   ├── Core /
│   │   ├── API.md
│   │   ├── Build.md
│   │   └── Contributing.md
│   │
│   ├── RuntimeBehaviorSafety /
│   │   ├── Errors.md
│   │   ├── Memory.md
│   │   └── Security.md
│   │
│   ├── UsageIntegration /
│   │   ├── Examples.md
│   │   ├── FAQ.md 
│   │   └── Interop.md
│   │
│   └── Architecture /
│       ├── Architecture.md
│       ├── ArchitectureDiagram.md
│       └── DesignPrinciples.md
│
└── TechnicalReference.md
```

---

## 1. [Core Documentation](TechnicalReference/Core)

### **[API.md](TechnicalReference/Core/API.md)**  
Complete reference of all exported C ABI functions, parameters, return values, and usage notes.

### **[Build.md](TechnicalReference/Core/Build.md)**  
Explains how to build the NativeAOT DLL, generate `.def` and `.lib` files, and run the build pipeline.

### **[CONTRIBUTING.md](TechnicalReference/Core/Contributing.md)**  
Guidelines for contributors, coding standards, pull request rules, and development workflow.

---

## 2. [Runtime Behavior & Safety](TechnicalReference/RuntimeBehaviorSafety)

### **[Errors.md](TechnicalReference/RuntimeBehaviorSafety/Errors.md)**  
List of all error codes, meanings, and common error scenarios.

### **[Memory.md](TechnicalReference/RuntimeBehaviorSafety/Memory.md)**  
Memory ownership rules, caller‑allocated buffers, zeroing behavior, and cross‑language memory safety.

### **[Security.md](TechnicalReference/RuntimeBehaviorSafety/Security.md)**  
Security policy, responsible disclosure process, cryptographic guarantees, and threat model.

---

## 3. [Usage & Integration](TechnicalReference/UsageIntegration)

### **[Interop.md](TechnicalReference/UsageIntegration/Interop.md)**  
Cross‑language interop guide with examples for C, C++, C#, VB.NET, Rust, Go, Zig, Python, and Java.

### **[Examples.md](TechnicalReference/UsageIntegration/Examples.md)**  
Practical usage examples for AES, GCM, ChaCha20‑Poly1305, hashing, HMAC, Base64, and random bytes.

### **[FAQ.md](TechnicalReference/UsageIntegration/FAQ.md)**  
Frequently asked questions about building, using, and integrating the library.

---

## 4. [Architectural Documentation](TechnicalReference/Architectural)

### **[DesignPrinciples.md](TechnicalReference/Architectural/DesignPrinciples.md)**  
Explains why the project uses NativeAOT, why a C ABI, and why the architecture is layered.

### *(Optional)* **[Architecture.md](TechnicalReference/Architectural/Architecture.md)**  
High‑level overview of the internal structure, layering model, and build pipeline.

### **[ArchitectureDiagram.md](TechnicalReference/Architectural/ArchitectureDiagram.md)**  
General overview of the internal structure of the C-Abi-Bridge-Aot project.

---

## 5. Recommended Reading Order

1. **TechnicalReference.md** (this file)  
2. **DesignPrinciples.md**  
3. **Architecture.md**  
4. **API.md**  
5. **Memory.md**  
6. **Errors.md**  
7. **Interop.md**  
8. **Examples.md**  
9. **Security.md**  
10. **Contributing.md**

---

