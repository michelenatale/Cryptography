

using Org.BouncyCastle.Crypto.Parameters;

namespace michele.natale.BcPqcs;


/// <summary>
/// Interface for the class SLHDSASignInfo
/// </summary>
public interface ISLHDSASignInfo
{
  /// <summary>
  /// ID as recognition of the data for the signature.
  /// </summary>
  Guid ID
  {
    get; set;
  }

  /// <summary>
  /// Name of the creator.
  /// </summary>
  string Name
  {
    get; set;
  }

  /// <summary>
  /// The calculated signature.
  /// </summary>
  string Sign
  {
    get; set;
  }

  /// <summary>
  /// The message used for the signature.
  /// </summary>
  string Message
  {
    get; set;
  }

  /// <summary>
  /// The generated PublicKey.
  /// </summary>
  string PublicKey
  {
    get; set;
  }

  /// <summary>
  /// Desired SLH-DSA-Parameter
  /// </summary>
  SLHDsaParam Parameter
  {
    get; set;
  }

  /// <summary>
  /// Returns the desired SLH-DSA-Parameter.
  /// </summary>
  /// <returns></returns>
  SlhDsaParameters ToParameter();

  /// <summary>
  /// Saves the current data of the SLHDSASignInfo class to the hardware.
  /// </summary>
  /// <param name="filename">Desired filename</param>
  void Save(string filename);

  /// <summary>
  /// Loads the data from the hardware into the SLHDSASignInfo class.
  /// </summary>
  /// <param name="filename">Desired filename</param>
  void Load(string filename);

  /// <summary>
  /// Deletes all data of the current SLHDSASignInfo class.
  /// </summary>
  void Clear();

}