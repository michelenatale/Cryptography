
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Pqc.Crypto.Lms;

namespace michele.natale.BcPqcs;

using Pointers;
using Services;

/// <summary>
/// interface for the class LMS.
/// </summary>
public interface ILMS
{

  /// <summary>
  /// Signs the existing message using the desired parameters.
  /// </summary>
  /// <param name="info">Desired KeyPair informations</param>
  /// <param name="message">Desired Message</param>
  /// <returns>Returns a signature as bytes.</returns>
  static abstract byte[] Sign(LmsKeyPairInfo info, ReadOnlySpan<byte> message);

  /// <summary>
  /// Signs the existing message using the desired parameters.
  /// </summary>
  /// <param name="privkey">Desired PrivateKey as Bytes</param>
  /// <param name="message">Desired Message</param>
  /// <returns>Returns a signature as bytes.</returns>
  static abstract byte[] Sign(UsIPtr<byte> privkey, ReadOnlySpan<byte> message);

  /// <summary>
  /// Signs the existing message using the desired parameters.
  /// </summary>
  /// <param name="privkey">Desired PrivateKey as Bytes</param>
  /// <param name="message">Desired Message</param>
  /// <returns>Returns a signature as bytes.</returns>
  static abstract byte[] Sign(LmsPrivateKeyParameters privkey, ReadOnlySpan<byte> message);

  /// <summary>
  /// Verify the existing message and Signature using the desired parameters.
  /// </summary>
  /// <param name="info">Desired KeyPair informations.</param>
  /// <param name="signature">Desired signature</param>
  /// <param name="message">Desired Message</param>
  /// <returns>True, verify is ok, ortherwise false.</returns>
  static abstract bool Verify(LmsKeyPairInfo info, ReadOnlySpan<byte> signature, ReadOnlySpan<byte> message);

  /// <summary>
  /// Verify the existing message and Signature using the desired parameters.
  /// </summary>
  /// <param name="pubkey">Desired PublicKey</param>
  /// <param name="signature">Desired Signature</param>
  /// <param name="message">Desired Message</param>
  /// <returns>True, verify is ok, ortherwise false.</returns>
  static abstract bool Verify(ReadOnlySpan<byte> pubkey, ReadOnlySpan<byte> signature, ReadOnlySpan<byte> message);

  /// <summary>
  /// Verify the existing message and Signature using the desired parameters.
  /// </summary>
  /// <param name="pubkey">Desired PublicKey</param>
  /// <param name="signature">Desired Signature</param>
  /// <param name="message">Desired Message</param>
  /// <returns>True, verify is ok, ortherwise false.</returns>
  static abstract bool Verify(LmsPublicKeyParameters pubkey, ReadOnlySpan<byte> signature, ReadOnlySpan<byte> message);



  /// <summary> 
  /// Signs the existing message from datafile using the desired parameters.
  /// </summary>
  /// <param name="info">Desired KeyPair informations.</param>
  /// <param name="datapath">Desired datafile</param>
  /// <returns>Returns a signature as bytes.</returns>
  static abstract byte[] Sign(LmsKeyPairInfo info, string datapath);

  /// <summary>
  /// Signs the existing message from datafile using the desired parameters.
  /// </summary>
  /// <param name="parameter">Desired LMS-Parameter</param>
  /// <param name="privkey">Desired PrivateKey as Bytes</param>
  /// <param name="datapath">Desired datafile.</param>
  /// <returns>Returns a signature as bytes.</returns>
  static abstract byte[] Sign(LmsParam parameter, UsIPtr<byte> privkey, string datapath);

  /// <summary>
  /// Signs the existing message from datafile using the desired parameters.
  /// </summary>
  /// <param name="privkey">Desired PrivateKey</param>
  /// <param name="datapath">Desired datafile</param>
  /// <returns>Returns a signature as bytes.</returns>
  static abstract byte[] Sign(LmsPrivateKeyParameters privkey, string datapath);


  /// <summary>
  /// Verify the existing message and signature from datafile using the desired parameters.
  /// </summary>
  /// <param name="info">Desired KeyPair information.</param>
  /// <param name="signature">Desired Signature as bytes</param>
  /// <param name="datapath">Desired datafile</param>
  /// <returns>True, the verify is ok, ortherwise false.</returns>
  static abstract bool Verify(LmsKeyPairInfo info, ReadOnlySpan<byte> signature, string datapath);

  /// <summary>
  /// Verify the existing message and signature from datafile using the desired parameters.
  /// </summary>
  /// <param name="parameter">Desired LMS-Parameter</param>
  /// <param name="pubkey">Desired PublicKey</param>
  /// <param name="signature">Desired Signature</param>
  /// <param name="datapath">Desired filedata</param>
  /// <returns>True, the verify is ok, ortherwise false.</returns>
  static abstract bool Verify(LmsParam parameter, ReadOnlySpan<byte> pubkey, ReadOnlySpan<byte> signature, string datapath);

  /// <summary>
  /// Verify the existing message and signature from datafile using the desired parameters.
  /// </summary>
  /// <param name="pubkey">Desired PublicKey</param>
  /// <param name="signature">Desired Signature</param>
  /// <param name="datapath">Desired datafile</param>
  /// <returns>True, the verify is ok, ortherwise false.</returns>
  static abstract bool Verify(LmsPublicKeyParameters pubkey, ReadOnlySpan<byte> signature, string datapath);

  /// <summary>
  /// Creates a KeyPair based on the existing data.
  /// </summary>
  /// <param name="rand">Desired SecureRandom-Instance</param>
  /// <param name="parameter">Desired LMS-Parameter</param>
  /// <returns>Returns Private- and the PublicKey as bytes.</returns>
  static abstract (byte[] PrivKey, byte[] PubKey) ToKeyPair(SecureRandom rand, LmsParam parameter);



}