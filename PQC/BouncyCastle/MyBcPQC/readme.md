# Bouncy Castle

[Bouncy Castle](https://www.bouncycastle.org) Crypto API are a collection of open-source cryptographic programming interfaces (API) for the Java and C# programming languages. They are maintained by the Australian-based Legion of the Bouncy Castle Inc.

The [library](https://www.bouncycastle.org) already contains some PQC algorithms that have recently been standardized by the [NIST](https://csrc.nist.gov/projects/post-quantum-cryptography) (National Institute of Standards and Technology).

The most important encryption algorithm is the [ML-KEM](https://csrc.nist.gov/pubs/fips/203/final) (Kyber). On the signature side, there are stateless algorithms such as [ML-DSA](https://csrc.nist.gov/pubs/fips/204/final) (Dilithium) and [SLH-DSA](https://csrc.nist.gov/pubs/fips/205/final) (SPHINCS+), as well as the stateful signatures [LMS](https://datatracker.ietf.org/doc/html/rfc8554) and [XMSS](https://datatracker.ietf.org/doc/html/rfc8391).

With the exception of XMSS, all are available in the C# library. XMSS is currently only available for [Java](https://www.bouncycastle.org/documentation/specification_interoperability/). But perhaps this will change, or you can simply write this algorithm yourself, because the NIST descriptions are very easy to understand.

## ML-KEM 

The ML-KEM Module-Lattice-Based Key-Encapsulation Mechanism standardized under FIPS 203 has been developed by Kyber with the purpose of being resistant to cryptanalytic attacks with future powerful quantum computers.

The typical feature of this algorithm is encapsulation, which allows two parties to create a shared secret key over a public channel under certain conditions. It is an asymmetric cryptosystem that uses a variant of the presumably NP-hard lattice problem of learning with errors as its basic trapdoor function.

This extracted key can be used e.g. as a password for a much faster symmetric encryption.

