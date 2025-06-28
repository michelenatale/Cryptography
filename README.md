# Cryptographie


## CryptoRandom

CryptoRandom is a very fast, easy-to-use and cryptographically as well as thread-safe random number generator for everyday use.

CryptoRandom is based on the [RandomNumberGenerator](https://learn.microsoft.com/en-us/dotnet/api/system.security.cryptography.randomnumbergenerator) provided by Dotnet. The most important data types are available for quick use.

CryptoRandom also offers other possibilities, such as shuffling and handling strings.

CryptoRandom can be easily integrated into any project and fulfills its purpose for the lifetime of the application.

https://github.com/michelenatale/Cryptography/tree/main/CryptoRandom



## Elliptic-Curve

Shows in a simple way how the sender and receiver function of Alice and Bob works in encrypted form.

The [ECDiffieHellman](https://learn.microsoft.com/en-us/dotnet/api/system.security.cryptography.ecdiffiehellman) (ec key exchange) and [ECDSA](https://learn.microsoft.com/en-us/dotnet/api/system.security.cryptography.ecdsa) (ec digital signing) algorithms are used here.

The [Elliptic Curves](https://en.wikipedia.org/wiki/Elliptic_curve) are always selected randomly so that everything in the temporary area is used.

https://github.com/michelenatale/Cryptography/tree/main/Elliptic-Curve

## Signatures 

There are two signature creators. 

The **Schnorr Signature** was published by the German mathematician [Claus Peter Schnorr](https://en.wikipedia.org/wiki/Claus_P._Schnorr) in 1991. It shows how the [Schnorr Signature](https://en.wikipedia.org/wiki/Schnorr_signature) works as a concept. The [Schnorrgroup](https://en.wikipedia.org/wiki/Schnorr_group) and [Elliptic Curve](https://en.wikipedia.org/wiki/Elliptic_curve) are used here.

**EasySignature** is a very fast and simple [signature](https://en.wikipedia.org/wiki/Digital_signature) creator. However, it has not been tested by me. Keys can be created with Length = 64 - 2048.

Multi-signatures can also be created and verified with both signature creators.

https://github.com/michelenatale/Cryptography/tree/main/Signatures

## LoginSystem

LoginSystem is a simple project that shows how to perform a local [login](Login) for your application. It is based on the **Winform-[Mvvm](https://en.wikipedia.org/wiki/Model%E2%80%93view%E2%80%93viewmodel)-Design** Pattern and UserControl. 

In order for © LoginSystem 2024 to work, a registration must first be made. Only then can a login be made. 

https://github.com/michelenatale/Cryptography/tree/main/LoginSystem

## BitsBytesUtils

BitsBytesUtils is a small project ([BitHacks](https://en.wikipedia.org/wiki/Bitwise_operation) full [generic](https://en.wikipedia.org/wiki/Generic_programming)) that can be used to convert all numeric data types into their bytes or bits and convert them back to the original data type. In addition, further information can be queried, such as [BitLength](https://en.wikipedia.org/wiki/Bit-length), [LeadingZero](https://en.wikipedia.org/wiki/Leading_zero), [Two's Complement](https://en.wikipedia.org/wiki/Two%27s_complement), [PowerOfTwo](https://en.wikipedia.org/wiki/Power_of_two) etc. Feel free to try out the possibilities.

https://github.com/michelenatale/Cryptography/tree/main/BitsBytesUtils

## PQC - Post Quantum Cryptography

[Post-quantum cryptography](https://en.wikipedia.org/wiki/Post-quantum_cryptography) refers to a branch of [cryptography](https://en.wikipedia.org/wiki/Cryptography) that deals with [cryptographic primitives](https://en.wikipedia.org/wiki/Cryptographic_primitive) which, in contrast to most [asymmetric cryptosystems](https://en.wikipedia.org/wiki/Public-key_cryptography) currently in use, are practically indecipherable even when using [quantum computers](https://en.wikipedia.org/wiki/Quantum_computing).

The term post-quantum cryptography was introduced by [Daniel J. Bernstein](https://en.wikipedia.org/wiki/Daniel_J._Bernstein), who was also involved in organizing the first PQCrypto conference on this topic in 2006.

https://github.com/michelenatale/Cryptography/tree/main/PQC


