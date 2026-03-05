# 🔢 Decimal Interop

The .NET `decimal` type is a 128‑bit structure consisting of:

- 96‑bit integer value
- 1‑bit sign
- 7‑bit scale (0–28)

## Layout

[ 32 bits flags ][ 32 bits hi ][ 32 bits mid ][ 32 bits low ]

## Flags

- Bits 0–15: unused
- Bits 16–23: scale
- Bit 31: sign

## Conversion

The ABI exposes decimals as:

```
typedef struct Decimal128 {
    uint32_t flags;
    uint32_t hi;
    uint32_t mid;
    uint32_t low;
} Decimal128;
```

## Usage Example

```
Decimal128* arr = NULL;
CError err = rng_crypto_decimal_aot(64, &arr);

if (err.error_code == 0) {
    // interpret arr[i]
}

free_buffer(arr);
```

## Scale and Range

- Scale is always between 0 and 28.
- Values are always non‑negative unless explicitly documented.

