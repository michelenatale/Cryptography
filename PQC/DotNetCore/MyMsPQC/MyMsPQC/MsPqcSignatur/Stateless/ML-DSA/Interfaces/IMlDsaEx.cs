//ML-DSA
//Module-Lattice-Based
//FIPS PUB 204 
//https://nvlpubs.nist.gov/nistpubs/fips/nist.fips.204.pdf





namespace michele.natale.MsPqcs;


using Pointers;
using System.Security.Cryptography;


/// <summary>
/// Interface for the Class MLDSA
/// </summary>
public interface IMlDsaEx
{

  /// <summary>
  /// Signs the existing message using the desired parameters.
  /// </summary>
  /// <param name="info">Desired KeyPair informations</param>
  /// <param name="message">Desired Message</param>
  /// <returns>Returns a signature as bytes.</returns>
  static abstract byte[] Sign(MlDsaKeyPairInfo info, ReadOnlySpan<byte> message);

  /// <summary>
  /// Signs the existing message using the desired parameters.
  /// </summary>
  /// <param name="parameter">Desired ML-DSA-Parameter</param>
  /// <param name="privkey">Desired PrivateKey as Bytes</param>
  /// <param name="message">Desired Message</param>
  /// <returns>Returns a signature as bytes.</returns>
  static abstract byte[] Sign(MLDsa mldsa, ReadOnlySpan<byte> message);

  /// <summary>
  /// Signs the existing message using the desired parameters.
  /// </summary>
  /// <param name="privkey">Desired PrivateKey as Bytes</param>
  /// <param name="message">Desired Message</param>
  /// <returns>Returns a signature as bytes.</returns>
  static abstract byte[] Sign(MLDsaAlgorithm algo, UsIPtr<byte> privkey, ReadOnlySpan<byte> message);

  /// <summary>
  /// Verify the existing message and Signature using the desired parameters.
  /// </summary>
  /// <param name="info">Desired KeyPair informations.</param>
  /// <param name="signature">Desired signature</param>
  /// <param name="message">Desired Message</param>
  /// <returns>True, verify is ok, ortherwise false.</returns>
  static abstract bool Verify(MLDsa mldsa, ReadOnlySpan<byte> signature, ReadOnlySpan<byte> message);

  /// <summary>
  /// Verify the existing message and Signature using the desired parameters.
  /// </summary>
  /// <param name="pubkey">Desired PublicKey</param>
  /// <param name="signature">Desired Signature</param>
  /// <param name="message">Desired Message</param>
  /// <returns>True, verify is ok, ortherwise false.</returns>
  static abstract bool Verify(MLDsaAlgorithm algo, ReadOnlySpan<byte> pubkey, ReadOnlySpan<byte> signature, ReadOnlySpan<byte> message);


  /// <summary> 
  /// Signs the existing message from datafile using the desired parameters.
  /// </summary>
  /// <param name="info">Desired KeyPair informations.</param>
  /// <param name="datapath">Desired datafile</param>
  /// <returns>Returns a signature as bytes.</returns>
  static abstract Task<byte[]> SignSha512Async(MlDsaKeyPairInfo info, string datapath);

  /// <summary>
  /// Signs the existing message from datafile using the desired parameters.
  /// </summary>
  /// <param name="parameter">Desired ML-DSA-Parameter</param>
  /// <param name="privkey">Desired PrivateKey as Bytes</param>
  /// <param name="datapath">Desired datafile.</param>
  /// <returns>Returns a signature as bytes.</returns>
  static abstract Task<byte[]> SignSha512Async(MLDsa mldsa, UsIPtr<byte> privkey, string datapath);

  /// <summary>
  /// Signs the existing message from datafile using the desired parameters.
  /// </summary>
  /// <param name="privkey">Desired PrivateKey</param>
  /// <param name="datapath">Desired datafile</param>
  /// <returns>Returns a signature as bytes.</returns>
  static abstract Task<byte[]> SignSha512Async(MLDsaAlgorithm algo, UsIPtr<byte> privkey, string datapath);

  /// <summary>
  /// Verify the existing message and signature from datafile using the desired parameters.
  /// </summary>
  /// <param name="info">Desired KeyPair information.</param>
  /// <param name="signature">Desired Signature as bytes</param>
  /// <param name="datapath">Desired datafile</param>
  /// <returns>True, the verify is ok, ortherwise false.</returns>
  static abstract Task<bool> VerifySha512Async(MlDsaKeyPairInfo info, ReadOnlyMemory<byte> signature, string datapath);

  /// <summary>
  /// Verify the existing message and signature from datafile using the desired parameters.
  /// </summary>
  /// <param name="parameter">Desired ML-DSA-Parameter</param>
  /// <param name="pubkey">Desired PublicKey</param>
  /// <param name="signature">Desired Signature</param>
  /// <param name="datapath">Desired filedata</param>
  /// <returns>True, the verify is ok, ortherwise false.</returns>
  static abstract Task<bool> VerifySha512Async(MLDsa mldsa, ReadOnlyMemory<byte> signature, string datapath);

  /// <summary>
  /// Verify the existing message and signature from datafile using the desired parameters.
  /// </summary>
  /// <param name="pubkey">Desired PublicKey</param>
  /// <param name="signature">Desired Signature</param>
  /// <param name="datapath">Desired datafile</param>
  /// <returns>True, the verify is ok, ortherwise false.</returns>
  static abstract Task<bool> VerifySha512Async(MLDsaAlgorithm algo, ReadOnlyMemory<byte> pubkey, ReadOnlyMemory<byte> signature, string datapath);

  /// <summary>
  /// Creates a KeyPair based on the existing data.
  /// </summary>
  /// <param name="rand">Desired SecureRandom-Instance</param>
  /// <param name="parameter">Desired ML-DSA-Parameter</param>
  /// <returns>Returns the Public- and the PrivateKey as bytes.</returns>
  static abstract (UsIPtr<byte> PrivKey, byte[] PubKey) ToKeyPair(MLDsa mldsa);

  /// <summary>
  /// Returns the ML-DSA parameter based on an index.
  /// </summary>
  /// <param name="idx">Desired index</param>
  /// <returns></returns>
  static abstract MLDsaAlgorithm ToMLDsaParameter(int idx);

  /// <summary>
  /// Returns the index based on an ML-DSA-PArameter.
  /// </summary>
  /// <param name="parameter">Desired ML-DSA-Parameter</param>
  /// <returns></returns>
  static abstract int ToIndex(MLDsaAlgorithm parameter);
}
