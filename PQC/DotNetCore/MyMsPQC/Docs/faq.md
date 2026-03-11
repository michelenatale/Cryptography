# 🔍 PQC Crypto FAQ

This document provides additional background information on the cryptographic concepts used in the MyMsPQC project.  

It complements the main README and explains why certain design choices were made.

---

## 🔒 AEAD and Associated Data

### **Why is “associated data” used in the encryption examples?**

The encryption examples in this project follow the **AEAD model** (*Authenticated Encryption with Associated Data*).  

AEAD allows additional metadata (such as headers, nonces, or protocol information) to be authenticated without being encrypted.  
- This ensures that contextual information cannot be tampered with, while still remaining visible.
- The examples intentionally use associated data to reflect realistic and modern protocol designs.

### Why are key pairs stored in the tests, even though they are only temporary?

The tests are designed to demonstrate realistic workflows where key pairs are persisted.

For purely temporary sessions, storing the keys would not be necessary, but it is shown here intentionally to illustrate complete end‑to‑end usage patterns.

---

## ✒️ Digital Signatures

### **Why is there no “associated data” in the digital signature examples?**

Digital signatures (e.g., ML‑DSA) provide *integrity and authenticity* for a message. 

Everything that needs to be protected is simply included in the message that is signed.  

Since signatures do not provide confidentiality and do not separate metadata from payload, the AEAD concept does not apply here.

---

## 🔑 KEM vs. Digital Signatures

### **What is the difference between a KEM and a digital signature scheme?**

A **Key Encapsulation Mechanism (KEM)** is used to establish a shared secret between two parties. It provides confidentiality and is typically used to bootstrap symmetric encryption.

A **digital signature scheme** provides integrity and authenticity for messages. It does not establish shared secrets and does not provide confidentiality.

In short:

- **KEM = secure key exchange**  
- **Signature = secure message authentication**

Both are essential in modern cryptographic protocols, but they solve different problems.

---

## ♻️ Key Reuse

### **Can ML‑KEM keys be reused across multiple sessions?**

Yes. ML‑KEM public keys are designed for reuse across many sessions.  

This is necessary for practical deployment (e.g., servers handling many clients).

However:

- Private keys must remain secret  
- Ephemeral keys may be used for additional forward secrecy  
- Key rotation policies still apply depending on the application

Each encapsulation produces a fresh shared secret, even when the same key pair is reused.

### **Can ML‑DSA signing keys be reused?**

Yes. ML‑DSA signing keys are intended for long‑term use.  

Unlike classical schemes such as ECDSA, ML‑DSA does not rely on fragile random nonces that could leak the private key.

Still, best practice includes:

- Protecting the private key  
- Rotating keys periodically  
- Avoiding reuse across unrelated systems

---

## 📏 Parameter Sets

### **How should I choose between ML‑KEM parameter sets (512, 768, 1024)?**

| Parameter Set   | Security Level | Typical Use Case                          |
|-----------------|----------------|-------------------------------------------|
| **ML‑KEM‑512**  | NIST Level 1   | Lightweight clients, embedded systems     |
| **ML‑KEM‑768**  | NIST Level 3   | General‑purpose applications              |
| **ML‑KEM‑1024** | NIST Level 5   | High‑security, long‑term confidentiality  |

Rule of thumb:

- **512** → smallest and fastest  
- **768** → balanced default  
- **1024** → maximum security  

### **How should I choose between ML‑DSA parameter sets (44, 65, 87)?**

| Parameter Set   | Security Level | Signature Size | Use Case                          |
|-----------------|----------------|----------------|-----------------------------------|
| **ML‑DSA‑44**   | NIST Level 1   | Smallest       | Lightweight applications           |
| **ML‑DSA‑65**   | NIST Level 3   | Medium         | General‑purpose signing            |
| **ML‑DSA‑87**   | NIST Level 5   | Largest        | High‑security, long‑term integrity |

If unsure, **ML‑DSA‑65** is a safe and balanced default.

### Why do KEMs and signatures use different parameter sets?

Because they solve different problems:
- KEMs focus on **key exchange**, ciphertext size, and decapsulation performance
- Signatures focus on **message integrity**, signature size, and verification speed

Each algorithm is optimized for its own security and performance trade‑offs.

---

## 🧮 Hashing Before Signing

### **Why are hashes (e.g., SHA‑512) computed before signing?**

ML‑DSA signs hash values rather than raw data.  

Hashing large files:

- reduces memory usage  
- improves performance  
- follows the standard behavior of modern signature schemes  

This is why the examples compute a hash before signing.

---

## 📚 More Documentation

For additional details, refer to the main project [README](../readme.md) or the official [Microsoft PQC documentation](https://github.com/michelenatale/Cryptography/tree/main/PQC/DotNetCore/MyMsPQC#-reference).

