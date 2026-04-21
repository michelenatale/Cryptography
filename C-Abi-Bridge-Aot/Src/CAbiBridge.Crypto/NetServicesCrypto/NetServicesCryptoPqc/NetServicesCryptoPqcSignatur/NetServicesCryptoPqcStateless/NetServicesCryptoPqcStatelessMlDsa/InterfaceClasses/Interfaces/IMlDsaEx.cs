//ML-DSA
//Module-Lattice-Based
//FIPS PUB 204 
//https://nvlpubs.nist.gov/nistpubs/fips/nist.fips.204.pdf



using System.Security.Cryptography;


namespace michele.natale;


using Pointers;


/// <summary>
/// Interface for the Class MLDSA
/// </summary>
public interface IMlDsaEx
{
  /// <summary>
  /// Signs the existing message using the desired parameters.
  /// </summary>
  /// <param name="message">Desired Message as Bytes</param>
  /// <param name="privkey">Desired PrivateKey as Bytes</param>
  /// <param name="algo">Desired MlDsa-Algorithm</param>
  /// <returns>Returns a signature as bytes.</returns>
  static abstract byte[] Sign(ReadOnlySpan<byte> message, UsIPtr<byte> privkey, MLDsaAlgorithm algo);

  /// <summary>
  /// Signs the existing message using the desired parameters.
  /// </summary>
  /// <param name="message">Desired Message as Bytes</param>
  /// <param name="privkey">Desired PrivateKey as Bytes</param>
  /// <param name="mldsa_param">Desired MlDsa-Algorithm-Param</param>
  /// <returns>Returns a signature as bytes.</returns>
  static abstract byte[] Sign(ReadOnlySpan<byte> message, ReadOnlySpan<byte> privkey, MLDsaParam mldsa_param);

  /// <summary>
  /// Verify the existing message and Signature using the desired parameters.
  /// </summary>
  /// <param name="message">Desired Message</param>
  /// <param name="pubkey">Desired PublicKey</param>
  /// <param name="signature">Desired Signature</param>
  /// <param name="algo">Desired MLDsa-Algorithm</param>
  /// <returns>True, verify is ok, ortherwise false.</returns>
  static abstract bool Verify(ReadOnlySpan<byte> message, ReadOnlySpan<byte> pubkey, ReadOnlySpan<byte> signature, MLDsaAlgorithm algo);

  /// <summary>
  /// Verify the existing message and Signature using the desired parameters.
  /// </summary>
  /// <param name="message">Desired Message</param>
  /// <param name="pubkey">Desired PublicKey</param>
  /// <param name="signature">Desired Signature</param>
  /// <param name="mldsa_param">Desired MlDsa-Algorithm-Param</param>
  /// <returns>True, verify is ok, ortherwise false.</returns>
  static abstract bool Verify(ReadOnlySpan<byte> message, ReadOnlySpan<byte> pubkey, ReadOnlySpan<byte> signature, MLDsaParam mldsa_param);

  /// <summary>
  /// Signs the existing message from datafile (Async) using the desired parameters.
  /// </summary>
  /// <param name="datapath">Desired datafile</param>
  /// <param name="privkey">Desired PrivateKey</param>
  /// <param name="algo">Desired MLDsa-Algorithm</param>
  /// <param name="ct">Desired CancellationToken </param>
  /// <returns>Returns a signature as bytes.</returns>
  static abstract Task<byte[]> SignFileAsync(string datapath, UsIPtr<byte> privkey, MLDsaAlgorithm algo, CancellationToken ct = default);

  /// <summary>
  /// Verify the existing message and signature from datafile using the desired parameters.
  /// </summary>
  /// <param name="datapath">Desired datafile</param>
  /// <param name="pubkey">Desired PublicKey</param>
  /// <param name="signature">Desired Signature</param>
  /// <param name="algo">Desired MLDsa-Algorithm</param>
  /// <param name="ct">Desired CancellationToken </param>
  /// <returns>True, the verify is ok, ortherwise false.</returns>
  static abstract Task<bool> VerifyFileAsync(string datapath, ReadOnlyMemory<byte> pubkey, ReadOnlyMemory<byte> signature, MLDsaAlgorithm algo, CancellationToken ct = default);

  /// <summary>
  /// Creates a KeyPair based on the existing data.
  /// </summary>
  /// <param name="mldsa">Desired MLDsa-Instance</param> 
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
  /// <param name="algo">Desired ML-DSA-Parameter</param>
  /// <returns></returns>
  static abstract int ToIndex(MLDsaAlgorithm algo);
}
