//ML-KEM (Kyber)
//Module-Lattice-Based
//FIPS PUB 203
//https://nvlpubs.nist.gov/nistpubs/FIPS/NIST.FIPS.203.ipd.pdf

using Org.BouncyCastle.Security;
using Org.BouncyCastle.Crypto.Parameters;

namespace michele.natale.BcPqcs;

using Pointers;


/// <summary>
/// Interfaces for the Class MLKEM
/// </summary>
public interface IMLKEM
{

  /// <summary>
  /// Encrypts the message data with the ML-KEM algorithm.
  /// </summary>
  /// <param name="bytes">Desired Message in Bytes</param>
  /// <param name="keypairfile">Desired file with the key information.</param>
  /// <param name="associated">Desired Associated</param>
  /// <param name="rand">Desired instance of the Securerandom</param>
  /// <returns>Returns a byte array.</returns>
  static abstract byte[] MlKemEncryption(
    ReadOnlySpan<byte> bytes, string keypairfile,
    ReadOnlySpan<byte> associated, SecureRandom rand);

  /// <summary>
  /// Decrypts the cipher informations with the ML-KEM algorithm.
  /// </summary>
  /// <param name="bytes">Desired Cipher in Bytes</param>
  /// <param name="keypairfile">Desired file with the key information.</param>
  /// <param name="associated">Desired Associated</param>
  /// <returns>Returns the decipher as bytes</returns>
  static abstract byte[] MlKemDecryption(
      ReadOnlySpan<byte> bytes, string keypairfile,
      ReadOnlySpan<byte> associated);

  /// <summary>
  /// Encrypts the message data from file with the ML-KEM algorithm.
  /// </summary>
  /// <param name="src">Desired source file with the plain text</param>
  /// <param name="dest">Desired destination file for the cipher information.</param>
  /// <param name="keypairfile">Desired file with the key information.</param>
  /// <param name="associated">Desired Associated</param>
  /// <param name="rand">Desired instance of the Securerandom</param>
  static abstract void MlKemEncryptionFile(
      string src, string dest, string keypairfile,
      ReadOnlySpan<byte> associated, SecureRandom rand);

  /// <summary> 
  /// Decrypts the cipher informations from file with the ML-KEM algorithm.
  /// </summary>
  /// <param name="src">Desired source file with the Cipher informations.</param>
  /// <param name="dest">Desired destination file for the Decipher information.</param>
  /// <param name="keypairfile">Desired file with the key information.</param>
  /// <param name="associated">Desired Associated</param>
  static abstract void MlKemDecryptionFile(
      string src, string dest, string keypairfile,
      ReadOnlySpan<byte> associated);

  /// <summary>
  /// Creates a KeyPair based on the entered parameters.
  /// </summary>
  /// <param name="rand">Desired instance of the SecureRandom</param>
  /// <param name="parameter">Desired ML.KEM-Parameter</param>
  /// <returns>Returns the Private- and the PublicKey as bytes.</returns>
  static abstract (byte[] PrivKey, byte[] PubKey) ToKeyPair(
      SecureRandom rand, MLKemParameters parameter);

  /// <summary>
  /// Creates a Shared Secred Key based on the entered parameters. (Encapsulation)
  /// </summary>
  /// <param name="pubkey">Desired publickey</param>
  /// <param name="parameter">Desired ML-KEM-Parameter</param>
  /// <param name="rand">Desired instance of the SecureRandom</param>
  /// <param name="capsulationkey">Desired CapsulationKey (Ciphertext from the Encapsulation)</param>
  /// <returns>Returns the Shared Secred Key as bytes in a protected (fix) UsIPtr-instance</returns>
  static abstract UsIPtr<byte> ToSharedKey(
      MLKemPublicKeyParameters pubkey, MLKemParameters parameter,
      SecureRandom rand, out byte[] capsulationkey);

  /// <summary>
  /// Creates a Shared Secred Key based on the entered parameters. (Decapsulation)
  /// </summary>
  /// <param name="privkey">Desired privatekey</param>
  /// <param name="parameter">Desired ML-KEM-Parameter</param>
  /// <param name="capsulationkey">Desired CapsulationKey (Ciphertext from the Encapsulation)</param>
  /// <returns>Returns the Shared Secred Key as bytes in a protected (fix) UsIPtr-instance</returns>
  static abstract UsIPtr<byte> ToSharedKey(
      MLKemPrivateKeyParameters privkey, MLKemParameters parameter,
      ReadOnlySpan<byte> capsulationkey);

  /// <summary>
  /// Returns the ML-KEM parameter based on the index.
  /// </summary>
  /// <param name="idx">Desired index</param>
  /// <returns></returns>
  static abstract MLKemParameters ToMLKemParameter(int idx);

  /// <summary>
  /// Returns the index based on the ML-KEM parameter.
  /// </summary>
  /// <param name="parameter">Desired ML-KEM-Parameter.</param>
  /// <returns>Returns the index as integer.</returns>
  public abstract static int ToIndex(MLKemParameters parameter);
}
