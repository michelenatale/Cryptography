
# 📘 Overview

The **C-Abi-Bridge-Aot** module provides a high‑performance, cross‑language interface to .NET NativeAOT cryptographic functions.  
It exposes a stable C ABI that can be consumed from C, C++, Rust, Zig, Go, Python (via ctypes), and any other language capable of calling native functions.

## Goals

- Provide a minimal, stable, language‑agnostic ABI.
- Enable high‑performance cryptographic operations without managed runtime overhead.
- Offer predictable memory ownership rules.
- Support deterministic random number generation and secure RNG.
- Allow easy integration into existing native applications.

## Key Features

- Next* and Rng* APIs for multiple numeric types.
- AES, AES‑GCM, and ChaCha20 encryption.
- Decimal interop compatible with .NET’s internal representation.
- NativeAOT‑friendly memory allocation patterns.
- Cross‑platform support (Windows, Linux, macOS).

For deeper technical details, see the other documents in this folder.
