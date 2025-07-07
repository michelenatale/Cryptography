# PQC Post-Quantum Cryptography

[Post-quantum cryptography](https://en.wikipedia.org/wiki/Post-quantum_cryptography) refers to a branch of [cryptography](https://en.wikipedia.org/wiki/Cryptography) that deals with [cryptographic primitives](https://en.wikipedia.org/wiki/Cryptographic_primitive) which, in contrast to most [asymmetric cryptosystems](https://en.wikipedia.org/wiki/Public-key_cryptography) currently in use, are practically indecipherable even when using [quantum computers](https://en.wikipedia.org/wiki/Quantum_computing).

The term post-quantum cryptography was introduced by [Daniel J. Bernstein](https://en.wikipedia.org/wiki/Daniel_J._Bernstein), who was also involved in organizing the first PQCrypto conference on this topic in 2006.

## Bouncy Castle

[Bouncy Castle](https://www.bouncycastle.org) Crypto API are a collection of open-source cryptographic programming interfaces (API) for the Java and C# programming languages. They are maintained by the Australian-based Legion of the Bouncy Castle Inc.

The [library](https://www.bouncycastle.org) already contains some PQC algorithms that have recently been standardized by the [NIST](https://csrc.nist.gov/projects/post-quantum-cryptography) (National Institute of Standards and Technology).

The most important encryption algorithm is the [ML-KEM](https://csrc.nist.gov/pubs/fips/203/final) (Kyber). On the signature side, there are stateless algorithms such as [ML-DSA](https://csrc.nist.gov/pubs/fips/204/final) (Dilithium) and [SLH-DSA](https://csrc.nist.gov/pubs/fips/205/final) (SPHINCS+), as well as the stateful signatures [LMS](https://datatracker.ietf.org/doc/html/rfc8554) and [XMSS](https://datatracker.ietf.org/doc/html/rfc8391).

With the exception of XMSS, all are available in the C# library. XMSS is currently only available for [Java](https://www.bouncycastle.org/documentation/specification_interoperability/). But perhaps this will change, or you can simply write this algorithm yourself, because the [RFC 8391](https://datatracker.ietf.org/doc/html/rfc8391) descriptions are very easy to understand.

https://github.com/michelenatale/Cryptography/tree/main/PQC/BouncyCastle/MyBcPQC


## DotNet Core

Microsoft is also joining in, and has already made a valuable start with SymCrypt. I am sure that Dotnet will also make these algorithms available in the near future.

See here: 

[Microsoft Security Community Blog](https://techcommunity.microsoft.com/blog/microsoft-security-blog/microsofts-quantum-resistant-cryptography-is-here/4238780)

[Post-Quantum-Kryptografie (PQC)](https://learn.microsoft.com/de-de/dotnet/core/whats-new/dotnet-10/libraries#post-quantum-cryptography-pqc)


https://github.com/michelenatale/Cryptography/tree/main/PQC/DotNetCore
