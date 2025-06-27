

namespace michele.natale.BcPqcs;


using Pointers;
using Services;


public interface ILmsKeyPairInfo
{

  /// <summary>
  /// Specifies the Id as GUID
  /// </summary>
  Guid Id
  {
    get; set;
  }

  /// <summary>
  /// Specifies the PublicKey as Bytes
  /// </summary>
  byte[] PubKey
  {
    get; set;
  }

  /// <summary>
  /// Return the LMS-Parameter.
  /// </summary>
  /// <returns>Return the LMS-Parameter.</returns>
  LmsParam Parameter
  {
    get; set;
  }

  /// <summary>
  /// Returns the current PrivateKey as bytes in a protected (fix) UsIPtr-instance.
  /// </summary>
  /// <returns></returns>
  UsIPtr<byte> ToPrivKey();

  /// <summary>
  /// Clear all data.
  /// </summary>
  void Clear();

  /// <summary>
  /// Saves the KeyPair to the hardware.
  /// </summary>
  /// <param name="filename">Desired filename</param>
  /// <param name="with_privkey">True if the PrivateKey is also to be saved, otherwise False.</param>
  void SaveKeyPair(string filename, bool with_privkey);

  /// <summary>
  /// Loads the KeyPair from the hardware.
  /// </summary>
  /// <param name="filename">Desired filename.</param>
  void LoadKeyPair(string filename);

  /// <summary>
  /// Checks whether both LMSKeyPairInfo-Instances are the same.
  /// </summary>
  /// <param name="info">Desired LMSKeyPairInfo</param>
  /// <returns>True, if equals, ortherwise false.</returns>
  bool Equals(LmsKeyPairInfo info);

  /// <summary>
  /// Loads the KeyPair from the hardware.
  /// </summary>
  /// <param name="filename">Desired filename.</param>
  /// <returns>Returns the KeyPair in a LMSKeyPairInfo-Instance.</returns>
  static abstract LmsKeyPairInfo Load_KeyPair(string filename);

}