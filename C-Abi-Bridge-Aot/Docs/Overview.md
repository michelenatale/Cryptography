# 📘 Documentation Overview  
This file provides a structured overview of all documentation available in the `Docs/` directory.  

Use this as the entry point to navigate the project documentation.

---

# 1. Core Documentation

### **API.md**  
Complete reference of all exported C ABI functions, parameters, return values, and usage notes.

### **BUILD.md**  
Explains how to build the NativeAOT DLL, generate `.def` and `.lib` files, and run the build pipeline.

### **CHANGELOG.md**  
Version history, including added features, fixes, and breaking changes.

### **CONTRIBUTING.md**  
Guidelines for contributors, coding standards, pull request rules, and development workflow.

---

# 2. Runtime Behavior & Safety

### **Errors.md**  
List of all error codes, meanings, and common error scenarios.

### **Memory.md**  
Memory ownership rules, caller‑allocated buffers, zeroing behavior, and cross‑language memory safety.

### **SECURITY.md**  
Security policy, responsible disclosure process, cryptographic guarantees, and threat model.

---

# 3. Usage & Integration

### **Interop.md**  
Cross‑language interop guide with examples for C, C++, C#, VB.NET, Rust, Go, Zig, Python, and Java.

### **Examples.md**  
Practical usage examples for AES, GCM, ChaCha20‑Poly1305, hashing, HMAC, Base64, and random bytes.

### **FAQ.md**  
Frequently asked questions about building, using, and integrating the library.

---

# 4. Architectural Documentation

### **DesignPrinciples.md**  
Explains why the project uses NativeAOT, why a C ABI, and why the architecture is layered.

### *(Optional)* **Architecture.md**  
High‑level overview of the internal structure, layering model, and build pipeline.

---

# 5. Recommended Reading Order

1. **Overview.md** (this file)  
2. **DesignPrinciples.md**  
3. **Architecture.md**  
4. **API.md**  
5. **Memory.md**  
6. **Errors.md**  
7. **Interop.md**  
8. **Examples.md**  
9. **SECURITY.md**  
10. **CONTRIBUTING.md**

---

