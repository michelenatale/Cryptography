# MyMsPQC – Post‑Quantum Cryptography for .NET

MyMsPQC is a lightweight .NET project demonstrating how to use Microsoft’s Post‑Quantum Cryptography (PQC) implementations. It provides simple, practical examples for integrating quantum‑resistant algorithms into modern .NET applications.

## ✨ Features

Integration of Microsoft PQC algorithms

Examples for:
- Key generation
- Key Encapsulation Mechanism (KEM): encapsulation & decapsulation
  - Digital signatures: signing & verification
  - Clean project structure with a dedicated test project
- Ideal as a learning resource or integration reference for PQC in .NET

## 📁 Project Structure

```
MyMsPQC/
│
├── MyMsPQC/           # Core library with PQC implementations
├── TestMyMsPQC/       # Unit tests and usage examples
└── TestMyMsPQC.slnx   # Solution file
```

## 🧰 Requirements

- .NET 8 or later
- Windows, Linux, or macOS
- Optional: Visual Studio, Rider, or VS Code

## 🚀 Getting Started

Clone the repository:
```
git clone https://github.com/michelenatale/Cryptography.git
cd Cryptography/PQC/DotNetCore/MyMsPQC
```

Build the project:
```
dotnet build
```

Run the tests:
```
dotnet test
```

## 🧪 Usage

The project demonstrates typical PQC workflows such as:
- Generating a quantum‑safe keypair
- Deriving a shared secret using a KEM
- Using the shared secret for symmetric encryption
- Creating and verifying digital signatures

All examples can be found in the TestMyMsPQC project.

## 🎯 Purpose

This project serves as a practical reference for developers who want to:
- Explore PQC algorithms in .NET
- Understand Microsoft’s PQCrypto implementations
- Build quantum‑resistant applications

## 📄 License

This project is part of the broader Cryptography repository and follows its licensing terms.
