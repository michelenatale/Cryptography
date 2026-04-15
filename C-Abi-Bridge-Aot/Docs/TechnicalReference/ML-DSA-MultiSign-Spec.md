# MultiSign Specification for PQC Signature Schemes  

**Version:** 0.1‑draft  
**Status:** Experimental  
**Scope:** ML‑DSA, SLH‑DSA, LMS, XMSS, and other PQC signature schemes

---

## 1. Overview

This document defines a **generic Multi‑Signature Meta‑Protocol** for post‑quantum signature schemes.  
It allows multiple independent signers to sign the **same message**, bundle their signatures into a deterministic structure, and optionally produce a **meta‑signature** that cryptographically finalizes the entire multi‑sign state.

The protocol does **not** modify any PQC signature scheme.  
It is a pure meta‑layer that works with:

- ML‑DSA (Dilithium)
- SLH‑DSA (SPHINCS+)
- LMS
- XMSS
- and any other deterministic PQC signature scheme.

---

## 2. Goals

- **Interoperability:** Language‑agnostic, ABI‑friendly, deterministic binary format.
- **Reproducibility:** Same input → same handshake hash → same meta‑signature.
- **Auditability:** Fully storable and reconstructable at any later time.
- **Security:** Based on the underlying PQC schemes + domain‑separated hashing.

---

## 3. Terminology

| Term | Meaning |
|------|---------|
| **Message** | The original message signed by all signers. |
| **Signer** | Entity with its own PQC keypair. |
| **SignerInfo** | Public key, signature, parameters, and identifier of a signer. |
| **MultiSignInfo** | Collection of all SignerInfo entries + the message. |
| **Handshake Hash** | Deterministic hash over MultiSignInfo. |
| **Meta‑Signature** | Optional signature over the handshake hash using a keypair derived from it. |

---

## 4. Data Structures

### 4.1 SignerInfo

Each signer is represented as:

SignerInfo:
signer_id        : 16 bytes (GUID or fixed identifier)
scheme_id        : 1 byte  (0x01 = ML‑DSA, 0x02 = SLH‑DSA, 0x03 = LMS, 0x04 = XMSS, ...)
param_id         : 1 byte  (scheme‑specific parameter set)
reserved         : 2 bytes (alignment / future use)

public_key_len   : 4 bytes (big endian)
public_key       : public_key_len bytes

signature_len    : 4 bytes (big endian)
signature        : signature_len bytes


### 4.2 MultiSignInfo

MultiSignInfo:
message_len      : 4 bytes (big endian)
message          : message_len bytes

signer_count     : 4 bytes (big endian)
signers          : signer_count × SignerInfo

---

## 5. ASCII Layout

### 5.1 MultiSignInfo

```
+-------------------------------+
| message_len (4)              |
+-------------------------------+
| message (message_len)        |
+-------------------------------+
| signer_count (4)             |
+-------------------------------+
| SignerInfo[0]                |
+-------------------------------+
| SignerInfo[1]                |
+-------------------------------+
| ...                          |
+-------------------------------+
| SignerInfo[N-1]              |
+-------------------------------+
```

### 5.2 SignerInfo

```
+-------------------------------+
| signer_id (16)               |
+-------------------------------+
| scheme_id (1)                |
+-------------------------------+
| param_id (1)                 |
+-------------------------------+
| reserved (2)                 |
+-------------------------------+
| public_key_len (4)           |
+-------------------------------+
| public_key (...)             |
+-------------------------------+
| signature_len (4)            |
+-------------------------------+
| signature (...)              |
+-------------------------------+
```

---

## 6. Canonical Serialization & Handshake Hash

### 6.1 Domain Separation

A fixed domain tag prevents cross‑protocol collisions:

DOMAIN = "PQC-MULTISIGN-HANDSHAKE-V1"

### 6.2 Signer Ordering

To ensure determinism:

Signers MUST be sorted lexicographically by signer_id.

### 6.3 Hash Input Format

```
input =
DOMAIN
|| message_len (4)
|| message
|| signer_count (4)
|| for each signer in sorted(signers):
signer_id (16)
scheme_id (1)
param_id (1)
reserved (2)
public_key_len (4)
public_key
signature_len (4)
signature
```

### 6.4 Hash Function

Recommended:

- SHA3‑256 (32 bytes), or  
- SHAKE256 (32 bytes output)

handshake_hash = H(input)

---

## 7. Meta‑Signature (Optional)

The handshake hash can be used to derive a deterministic PQC keypair:

meta_seed = handshake_hash
(meta_sk, meta_pk) = ImportPrivateSeed(param_id_meta, meta_seed)


The meta‑signature finalizes the multi‑sign state:

meta_signature = Sign(meta_sk, handshake_hash)

### Verification

A verifier:

1. Reconstructs MultiSignInfo.
2. Verifies each signer’s signature over the message.
3. Recomputes handshake_hash.
4. Derives (meta_sk, meta_pk) from handshake_hash.
5. Verifies:

Verify(meta_pk, handshake_hash, meta_signature)


If all steps succeed, the MultiSign structure is valid and complete.

---

## 8. Process Flow (ASCII Diagrams)

### 8.1 Signing Phase

```
Signer A        Signer B        ...        Signer N
|               |                         |
| Sign(msg)     | Sign(msg)               | Sign(msg)
|--> sig_A      |--> sig_B                |--> sig_N
|               |                         |
+---------------+----------- ... ----------+
|
v
MultiSignInfo Builder
|
sort signers by signer_id
|
serialize MultiSignInfo
|
handshake_hash = H(...)
|
meta_seed = handshake_hash
meta_keypair = ImportPrivateSeed(...)
|
meta_signature = Sign(meta_sk, handshake_hash)
|
```

store:
- MultiSignInfo
- meta_pk
- meta_signature

### 8.2 Verification Phase

```
Verifier
|
| load MultiSignInfo + meta_signature
v
verify all individual signatures
|
v
recompute handshake_hash
|
v
derive meta_pk from handshake_hash
|
v
verify meta_signature
|
v
MultiSign structure is valid
```

---

## 9. Security Considerations

- **Underlying PQC security preserved:**  
  Each signer uses a standard PQC signature scheme without modification.

- **Deterministic & canonical:**  
  Sorting + length‑prefixing ensures identical results across languages.

- **Domain separation:**  
  Prevents collisions with other hashing contexts.

- **Meta‑signature optional but recommended:**  
  It provides a compact cryptographic commitment to the entire multi‑sign state.

---

## 10. Extensibility

- New PQC schemes can be added via new `scheme_id` values.
- `reserved` bytes allow future flags or metadata.
- Additional fields (timestamps, policies, roles) can be added in a future version with versioned serialization.

---

## 11. Status

This specification is experimental and intended for research, prototyping, and interoperability testing.  
It is not an official standard, but it is designed to be stable, deterministic, and easy to implement across languages and platforms.

