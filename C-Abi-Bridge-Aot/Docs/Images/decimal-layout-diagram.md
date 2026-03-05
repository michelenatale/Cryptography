# 🔢 Decimal Layout Diagram

This diagram illustrates the internal structure of a .NET Decimal.
```
+-----------------------------------------------------------+
|                     Decimal128 (128-bit)                  |
+------------------+------------------+----------------------+
|     flags        |       hi         |         mid          |
|  (scale, sign)   |   32 bits        |       32 bits        |
+------------------+------------------+----------------------+
|                        low (32 bits)                       |
+-----------------------------------------------------------+

flags layout:
- bits 0–15: unused
- bits 16–23: scale (0–28)
- bit 31: sign (1 = negative)

```

---

[← Back to Docs](../README-DOCS.md)
