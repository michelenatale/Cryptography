# Cryptographie


## BitHacksOperations (Fill Generic)

Shows a few BitHacks that I have collected over and over again, depending on the problem.

I have deliberately put the whole thing together programmatically as a unit test so that the project can be tested.

I will certainly find a few more interesting BitHacks, which I will then add here.

https://github.com/michelenatale/Cryptography/tree/main/BitHacksOperations



## CryptoRandom

CryptoRandom is a very fast, easy-to-use and cryptographically as well as thread-safe random number generator for everyday use.

CryptoRandom is based on the RandomNumberGenerator provided by Dotnet. The most important data types are available for quick use.

CryptoRandom also offers other possibilities, such as shuffling and handling strings.

CryptoRandom can be easily integrated into any project and fulfills its purpose for the lifetime of the application.

https://github.com/michelenatale/Cryptography/tree/main/CryptoRandom



## Elliptic-Curve

Shows in a simple way how the sender and receiver function of Alice and Bob works in encrypted form.

The ECDiffieHellman (ec key exchange) and ECDSA (ec digital signing) algorithms are used here.

The elliptic curves are always selected randomly so that everything in the temporary area is used.

https://github.com/michelenatale/Cryptography/tree/main/Elliptic-Curve

## Signatures 

There are two signature creators. 

The **Schnorr Signature** was published by the German mathematician Claus Peter Schnorr in 1991. It shows how the Schnorr signature works as a concept. The Schnorrgroup and Elliptic Curve are used here.

**EasySignature** is a very fast and simple signature creator. However, it has not been tested by me. Keys can be created with Length = 64 - 2048.

Multi-signatures can also be created and verified with both signature creators.

https://github.com/michelenatale/Cryptography/tree/main/Signatures

## LoginSystem

LoginSystem is a simple project that shows how to perform a local login for your application. It is based on the **Winform-Mvvm-Design** Pattern and UserControl. 

In order for © LoginSystem 2024 to work, a registration must first be made. Only then can a login be made. 

https://github.com/michelenatale/Cryptography/tree/main/LoginSystem
