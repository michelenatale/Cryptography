//ML-DSA
//Module-Lattice-Based
//FIPS PUB 204 
//https://nvlpubs.nist.gov/nistpubs/fips/nist.fips.204.pdf



using michele.natale.Pointers;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Crypto.Parameters;


namespace michele.natale.BcPqcs;


/// <summary>
/// Interface for the Class MLDSA
/// </summary>
public interface IMLDSA
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
  static abstract byte[] Sign(MLDsaParameters parameter, UsIPtr<byte> privkey, ReadOnlySpan<byte> message);

  /// <summary>
  /// Signs the existing message using the desired parameters.
  /// </summary>
  /// <param name="privkey">Desired PrivateKey as Bytes</param>
  /// <param name="message">Desired Message</param>
  /// <returns>Returns a signature as bytes.</returns>
  static abstract byte[] Sign(MLDsaPrivateKeyParameters privkey, ReadOnlySpan<byte> message);

  /// <summary>
  /// Verify the existing message and Signature using the desired parameters.
  /// </summary>
  /// <param name="info">Desired KeyPair informations.</param>
  /// <param name="signature">Desired signature</param>
  /// <param name="message">Desired Message</param>
  /// <returns>True, verify is ok, ortherwise false.</returns>
  static abstract bool Verify(MlDsaKeyPairInfo info, ReadOnlySpan<byte> signature, ReadOnlySpan<byte> message);

  /// <summary>
  /// Verify the existing message and Signature using the desired parameters.
  /// </summary>
  /// <param name="pubkey">Desired PublicKey</param>
  /// <param name="signature">Desired Signature</param>
  /// <param name="message">Desired Message</param>
  /// <returns>True, verify is ok, ortherwise false.</returns>
  static abstract bool Verify(MLDsaPublicKeyParameters pubkey, ReadOnlySpan<byte> signature, ReadOnlySpan<byte> message);


  /// <summary> 
  /// Signs the existing message from datafile using the desired parameters.
  /// </summary>
  /// <param name="info">Desired KeyPair informations.</param>
  /// <param name="datapath">Desired datafile</param>
  /// <returns>Returns a signature as bytes.</returns>
  static abstract byte[] Sign(MlDsaKeyPairInfo info, string datapath);

  /// <summary>
  /// Signs the existing message from datafile using the desired parameters.
  /// </summary>
  /// <param name="parameter">Desired ML-DSA-Parameter</param>
  /// <param name="privkey">Desired PrivateKey as Bytes</param>
  /// <param name="datapath">Desired datafile.</param>
  /// <returns>Returns a signature as bytes.</returns>
  static abstract byte[] Sign(MLDsaParameters parameter, UsIPtr<byte> privkey, string datapath);

  /// <summary>
  /// Signs the existing message from datafile using the desired parameters.
  /// </summary>
  /// <param name="privkey">Desired PrivateKey</param>
  /// <param name="datapath">Desired datafile</param>
  /// <returns>Returns a signature as bytes.</returns>
  static abstract byte[] Sign(MLDsaPrivateKeyParameters privkey, string datapath);

  /// <summary>
  /// Verify the existing message and signature from datafile using the desired parameters.
  /// </summary>
  /// <param name="info">Desired KeyPair information.</param>
  /// <param name="signature">Desired Signature as bytes</param>
  /// <param name="datapath">Desired datafile</param>
  /// <returns>True, the verify is ok, ortherwise false.</returns>
  static abstract bool Verify(MlDsaKeyPairInfo info, ReadOnlySpan<byte> signature, string datapath);

  /// <summary>
  /// Verify the existing message and signature from datafile using the desired parameters.
  /// </summary>
  /// <param name="parameter">Desired ML-DSA-Parameter</param>
  /// <param name="pubkey">Desired PublicKey</param>
  /// <param name="signature">Desired Signature</param>
  /// <param name="datapath">Desired filedata</param>
  /// <returns>True, the verify is ok, ortherwise false.</returns>
  static abstract bool Verify(MLDsaParameters parameter, ReadOnlySpan<byte> pubkey, ReadOnlySpan<byte> signature, string datapath);

  /// <summary>
  /// Verify the existing message and signature from datafile using the desired parameters.
  /// </summary>
  /// <param name="pubkey">Desired PublicKey</param>
  /// <param name="signature">Desired Signature</param>
  /// <param name="datapath">Desired datafile</param>
  /// <returns>True, the verify is ok, ortherwise false.</returns>
  static abstract bool Verify(MLDsaPublicKeyParameters pubkey, ReadOnlySpan<byte> signature, string datapath);

  /// <summary>
  /// Creates a KeyPair based on the existing data.
  /// </summary>
  /// <param name="rand">Desired SecureRandom-Instance</param>
  /// <param name="parameter">Desired ML-DSA-Parameter</param>
  /// <returns>Returns the Public- and the PrivateKey as bytes.</returns>
  static abstract (byte[] PrivKey, byte[] PubKey) ToKeyPair(SecureRandom rand, MLDsaParameters parameter);

  /// <summary>
  /// Returns the ML-DSA parameter based on an index.
  /// </summary>
  /// <param name="idx">Desired index</param>
  /// <returns></returns>
  static abstract MLDsaParameters ToMLDsaParameter(int idx);

  /// <summary>
  /// Returns the index based on an ML-DSA-PArameter.
  /// </summary>
  /// <param name="parameter">Desired ML-DSA-Parameter</param>
  /// <returns></returns>
  static abstract int ToIndex(MLDsaParameters parameter);
}
