# Bouncy Castle

[Bouncy Castle](https://www.bouncycastle.org) Crypto API are a collection of open-source cryptographic programming interfaces (API) for the Java and C# programming languages. They are maintained by the Australian-based Legion of the Bouncy Castle Inc.

The [library](https://www.bouncycastle.org) already contains some PQC algorithms that have recently been standardized by the [NIST](https://csrc.nist.gov/projects/post-quantum-cryptography) (National Institute of Standards and Technology).

The most important encryption algorithm is the [ML-KEM](https://csrc.nist.gov/pubs/fips/203/final) (Kyber). On the signature side, there are stateless algorithms such as [ML-DSA](https://csrc.nist.gov/pubs/fips/204/final) (Dilithium) and [SLH-DSA](https://csrc.nist.gov/pubs/fips/205/final) (SPHINCS+), as well as the stateful signatures [LMS](https://datatracker.ietf.org/doc/html/rfc8554) and [XMSS](https://datatracker.ietf.org/doc/html/rfc8391).

With the exception of XMSS, all are available in the C# library. XMSS is currently only available for [Java](https://www.bouncycastle.org/documentation/specification_interoperability/). But perhaps this will change, or you can simply write this algorithm yourself, because the [RFC 8391](https://datatracker.ietf.org/doc/html/rfc8391) descriptions are very easy to understand.

## Applying the MyBcPQC:
The MyBcPQC offers at the moment 4 options.
 - ML-KEM
 - ML-DSA
 - SLH-DSA
 - LMS

MyBcPQC is designed to be very easy to use. There are two test projects available that show how to use MyBcPQC. One written in [C#](https://github.com/michelenatale/Cryptography/tree/main/PQC/BouncyCastle/MyBcPQC/TestMyBcPQC) and one in [Vb.Net](https://github.com/michelenatale/Cryptography/tree/main/PQC/BouncyCastle/MyBcPQC/TestMyBcPQCVb). 

## ML-KEM 

The ML-KEM Module-Lattice-Based Key-Encapsulation Mechanism (also known as Kyber), standardized under FIPS 203, was developed with the aim of being resistant to cryptanalytic attacks using future powerful quantum computers.

The typical feature of this algorithm is encapsulation, which allows two parties to create a shared secret key over a public channel under certain conditions. It is an asymmetric cryptosystem that uses a variant of the presumably NP-hard lattice problem of learning with errors as the basic trapdoor function.

This extracted key can be used e.g. as a password for a much faster symmetric encryption. See [TestCode](https://github.com/michelenatale/Cryptography/blob/main/PQC/BouncyCastle/MyBcPQC/TestMyBcPQC/TestBcPqcCryption/Test-ML-KEM/Test-ML-KEM.cs)

## ML-DSA

The ML-DSA Module-Lattice-Based Digital Signature Algorithm (also known as Dilithium), standardized under Fips 204, was developed with the aim of providing the recipient of signed data with a digital signature as evidence to show a third party that the signature was actually created by the claimed signatory. Any changes can also be immediately verified. This is even referred to as non-repudiation, as the signatory can prove the signature at a later date at any time.

A typical feature of this algorithm is that ML-DSA is based on the LWE (Learning With Errors) module and the SIS (Short Integer Solution) module, which is typical of lattice-based cryptography and should ensure that it is also secure against future quantum computers. See [TestCode](https://github.com/michelenatale/Cryptography/tree/main/PQC/BouncyCastle/MyBcPQC/TestMyBcPQC/TestBcPqcSignatur/Stateless/Test-ML-DSA)

## SLH-DSA

The SLH-DSA Stateless Hash-Based Digital Signature Algorithm (also known as SPHINCS+), standardized under Fips 205, was developed like ML-DSA with the aim of providing the recipient of signed data with a digital signature as evidence to show a third party that the signature was actually created by the claimed signatory. Any changes can also be immediately verified. This is even referred to as non-repudiation, as the signatory can prove the signature at a later date at any time.

A typical feature of this algorithm is that it is based on a one-time signature scheme called WOTS+ (a modified version of the Winternitz one-time signature scheme), a few-time signature scheme called FORS (Forest of Random Subsets) and Merkle trees, which is why NIST calls it “conservative”, as its security is based solely on the preimage and collision resistance of the underlying hash function. See [TestCode](https://github.com/michelenatale/Cryptography/blob/main/PQC/BouncyCastle/MyBcPQC/TestMyBcPQC/TestBcPqcSignatur/Stateless/Test-SLH-DSA/Test-SLH-DSA.cs)

## LMS

LMS Leighton-Micali Signatures, standardized under RFC 8554, was developed with the goal of being resistant to cryptanalytic attacks using future powerful quantum computers.

LMS is a stateful hash-based signature scheme that has attracted a lot of attention in the cryptographic community due to its robust security features and potential applications in post-quantum cryptography.

A typical feature of this algorithm is that it is stateful, meaning that the signer must maintain a state, usually in the form of a counter or tree structure, in order to generate signatures. This state is used to ensure that each signature is unique and cannot be reused. See [TestCode](https://github.com/michelenatale/Cryptography/blob/main/PQC/BouncyCastle/MyBcPQC/TestMyBcPQC/TestBcPqcSignatur/Stateful/Test-LMS/Test-LMS.cs)

And this is what the console output looks like:
![](https://github.com/michelenatale/Cryptography/blob/main/PQC/BouncyCastle/MyBcPQC/Documentation/test.png)

