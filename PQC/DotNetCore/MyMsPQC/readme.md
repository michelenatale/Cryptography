# MyMsPQC – Post‑Quantum Cryptography for .NET

MyMsPQC is a lightweight .NET project demonstrating how to use [Microsoft’s Post‑Quantum Cryptography (PQC)](https://devblogs.microsoft.com/dotnet/post-quantum-cryptography-in-dotnet/) implementations. It provides simple, practical examples for integrating quantum‑resistant algorithms into modern .NET applications.

___

## ✨ Features

Integration of Microsoft PQC algorithms

Examples for:
- Key generation
- Key Encapsulation Mechanism (KEM): encapsulation & decapsulation
- Digital signatures: signing & verification
- Clean project structure with a dedicated test project
- Ideal as a learning resource or integration reference for PQC in .NET

---

## 📁 Project Structure

```
MyMsPQC/
│
├── MyMsPQC /           # Core library with PQC implementations
├── TestMyMsPQC /       # Unit tests and usage examples
├── TestMyMsPQCVb /     # Unit tests and usage examples in Vb.Net
└── TestMyMsPQC.slnx    # Solution file
```

---

## 🧰 Requirements

- .NET 8 or later
- Windows, Linux, or macOS
- Optional: Visual Studio, Rider, or VS Code

---

## 🔐 Post‑Quantum Algorithms Overview

This project demonstrates Microsoft’s implementations of the first NIST‑standardized post‑quantum algorithms. These algorithms were selected after a multi‑year global competition and are now published as official FIPS standards.

Below is an overview of each algorithm family, their origins, performance characteristics, and current availability in .NET.

...

### 📘 ML‑KEM (formerly CRYSTALS‑Kyber) — FIPS 203

Type: Key Encapsulation Mechanism (KEM)
Use cases: Key exchange, hybrid TLS, encrypted messaging
Status: Standardized by NIST in 2024 as FIPS 203

#### 🔎 Background

ML‑KEM originates from CRYSTALS‑Kyber, a lattice‑based KEM built on the hardness of module‑LWE. It was selected for its strong security proofs, excellent performance, and resistance to known quantum attacks.

#### ⚙️ Performance Characteristics

- Very fast key generation and encapsulation
- Small ciphertext and key sizes
- Efficient on constrained devices
- Well‑suited for high‑volume protocols like TLS 1.3

ML‑KEM is widely considered the default PQC KEM for the coming decades.

#### 🔮 Future Outlook
ML‑KEM is expected to become the global standard for PQ key exchange.
NIST recommends maintaining cryptographic agility to adapt to future developments.

...

### 📙 ML‑DSA (formerly CRYSTALS‑Dilithium) — FIPS 204

Type: Digital Signature Algorithm
Use cases: Code signing, certificates, authentication
Status: Standardized by NIST in 2024 as FIPS 204

#### 🔎 Background

ML‑DSA is derived from CRYSTALS‑Dilithium, a lattice‑based signature scheme using module‑LWE and module‑SIS assumptions. It offers a strong balance of security, performance, and signature size.

#### ⚙️ Performance Characteristics

- Very fast signature verification
- Practical signature sizes
- Suitable for certificate chains and high‑volume verification workloads

#### 🔮 Future Outlook

ML‑DSA is expected to replace ECDSA/Ed25519 in many ecosystems.
It is already being integrated into TLS, SSH, and major operating systems.

...

### 📗 SLH‑DSA (formerly SPHINCS+) — FIPS 205

Type: Stateless hash‑based signature scheme
Use cases: Long‑term archival signatures, high‑assurance systems
Status: Standardized by NIST in 2024 as FIPS 205

#### 🔎 Background

SLH‑DSA is based on SPHINCS+, a stateless hash‑based signature scheme.
Its security relies solely on the strength of hash functions, making it extremely conservative and robust.

#### ⚠️ Availability in .NET

At the moment, SLH‑DSA is not yet available in Microsoft’s PQC .NET libraries.
Therefore, this project does not include SLH‑DSA examples.

#### ⚙️ Performance Characteristics
- Very large signatures (tens of kilobytes)
- Slower signing operations
- Extremely strong and conservative security assumptions
- Ideal for long‑term digital archives

#### 🔮 Future Outlook

SLH‑DSA will remain a niche but essential algorithm for high‑assurance environments.

...

### 🧩 Algorithm Comparison

| Algorithm | Origin | FIPS Standard | Type | Strengths | Weaknesses |
|----------|--------|---------------|------|-----------|------------|
| **ML‑KEM** | CRYSTALS‑Kyber | FIPS 203 | KEM | Fast, small keys, ideal for TLS | Lattice‑based assumptions |
| **ML‑DSA** | CRYSTALS‑Dilithium | FIPS 204 | Signature | Fast verification, practical sizes | Larger than classical signatures |
| **SLH‑DSA** | SPHINCS+ | FIPS 205 | Signature | Extremely conservative, hash‑based | Very large signatures, slower |

---

## 🚀 Getting Started

*Clone the repository:*
```
git clone https://github.com/michelenatale/Cryptography.git
cd Cryptography/PQC/DotNetCore/MyMsPQC
```

*Build the project:*
```
dotnet build
```

*Run the tests:*
```
dotnet test
```

---

## 🧪 Usage

The project demonstrates typical PQC workflows such as:
- Generating a quantum‑safe keypair
- Deriving a shared secret using a KEM
- Using the shared secret for symmetric encryption
- Creating and verifying digital signatures

All examples can be found in the TestMyMsPQC project.

---

## 🎯 Purpose

This project serves as a practical reference for developers who want to:
- Explore PQC algorithms in .NET
- Understand Microsoft’s PQCrypto implementations
- Build quantum‑resistant applications

---

## 🔍 Crypto FAQ

#### Why is “associated data” used in the encryption examples?

The encryption examples in this project follow the **AEAD model** (Authenticated Encryption with Associated Data).
AEAD allows additional metadata (e.g., headers, nonces, protocol information) to be authenticated without being encrypted.
This strengthens the security of modern protocols and reflects realistic usage scenarios where context information must be integrity‑protected but not hidden.

#### Why is there no “associated data” in the digital signature examples?

Digital signatures (e.g., ML‑DSA) provide integrity and authenticity for a message.
Everything that needs to be protected is simply included in the message that is signed.
Since signatures do not provide confidentiality and do not separate metadata from payload, the AEAD concept does not apply here.

#### Why are key pairs stored in the tests, even though they are only temporary?

The tests are designed to demonstrate realistic workflows where key pairs are persisted.
For purely temporary sessions, storing the keys would not be necessary, but it is shown here intentionally to illustrate complete end‑to‑end usage patterns.

#### Why are hashes (e.g., SHA‑512) computed before signing?

ML‑DSA signs hash values rather than raw data.
Hashing large files reduces memory usage, improves performance, and follows the standard behavior of modern digital signature schemes.

#### What is the difference between a KEM and a digital signature scheme?

A **Key Encapsulation Mechanism (KEM)** is used to establish a shared secret between two parties.
It provides confidentiality and is typically used to bootstrap symmetric encryption.

A **digital signature scheme** provides integrity and authenticity for messages.
It does not establish shared secrets and does not provide confidentiality.

In short:
- **KEM = secure key exchange**
- **Signature = secure message authentication**

Both are essential in modern cryptographic protocols, but they solve different problems.

#### Can ML‑KEM keys be reused across multiple sessions?

Yes, ML‑KEM public keys can be reused safely across many sessions.
This is an intentional design property of KEMs and is required for practical deployment (e.g., servers handling many clients).

However:
- Private keys must remain secret
- Ephemeral keys can be used for additional forward secrecy
- Key rotation policies still apply depending on the application

The shared secrets produced by ML‑KEM are always fresh and unique, even when the same key pair is reused.

#### Can ML‑DSA signing keys be reused?
Yes, ML‑DSA signing keys are designed for long‑term use.
Unlike classical schemes such as ECDSA or Ed25519, ML‑DSA does not rely on random nonces that could leak the private key if misused.

Still, best practice is:
- Protect the private key
- Rotate keys periodically
- Avoid embedding the same key in multiple unrelated systems

#### How should I choose between ML‑KEM parameter sets (512, 768, 1024)?
The parameter sets represent different security levels:

| Parameter Set   | Security Level | Typical Use Case                     |
|-----------------|----------------|--------------------------------------|
| **ML‑KEM‑512**  | NIST Level 1   | Lightweight clients, embedded systems |
| **ML‑KEM‑768**  | NIST Level 3   | General‑purpose applications          |
| **ML‑KEM‑1024** | NIST Level 5   | High‑security, long‑term confidentiality |

A simple rule of thumb:
- **512** → fast and small
- **768** → balanced
- **1024** → maximum security

Your project supports all three, so developers can choose based on their needs.

#### How should I choose between ML‑DSA parameter sets (44, 65, 87)?

These correspond to different signature sizes and security levels:

| Parameter Set   | Security Level | Signature Size | Use Case                          |
|-----------------|----------------|----------------|-----------------------------------|
| **ML‑DSA‑44**   | NIST Level 1   | Smallest       | Lightweight applications           |
| **ML‑DSA‑65**   | NIST Level 3   | Medium         | General‑purpose signing            |
| **ML‑DSA‑87**   | NIST Level 5   | Largest        | High‑security, long‑term integrity |

If unsure, **ML‑DSA‑65** is the recommended default.

#### Why do KEMs and signatures use different parameter sets?

Because they solve different problems:
- KEMs focus on **key exchange**, ciphertext size, and decapsulation performance
- Signatures focus on **message integrity**, signature size, and verification speed

Each algorithm is optimized for its own security and performance trade‑offs.

---

## 📄 License

This project is part of the broader Cryptography repository and follows its licensing terms.

---

## 🧭 Reference

[Microsoft Security Community Blog](https://techcommunity.microsoft.com/blog/microsoft-security-blog/microsofts-quantum-resistant-cryptography-is-here/4238780)

[PQC - A New Age of Digital Security](https://techcommunity.microsoft.com/blog/microsoft-security-blog/post-quantum-cryptography-comes-to-windows-insiders-and-linux/4413803)

[Post-Quantum-Kryptografie (PQC)](https://learn.microsoft.com/de-de/dotnet/core/whats-new/dotnet-10/libraries#post-quantum-cryptography-pqc)

---

## ▶️ Console Output

![](Docs/ConsoleOutput.png)

---
