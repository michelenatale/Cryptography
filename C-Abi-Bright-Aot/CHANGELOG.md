
---

# 📝 CHANGELOG.md (English)

```markdown
# Changelog

## 1.0.0 — Initial Release

### Added
- NativeAOT-compatible C ABI for:
  - NextCryptoBool / Byte / Int32 / Int64 / Single / Double / Decimal
  - RngCryptoBool / Byte / Int32 / Int64 / Single / Double / Decimal
  - Range variants for all numeric types
- AES (CBC) file and byte APIs
- AES-GCM file and byte APIs
- ChaCha20-Poly1305 file and byte APIs
- Full Decimal interop (GetBits / WriteInt32)
- Explicit unmanaged memory allocation and FreeBuffer
- Extensive tests and performance benchmarks
- Clean project structure for future extensions
