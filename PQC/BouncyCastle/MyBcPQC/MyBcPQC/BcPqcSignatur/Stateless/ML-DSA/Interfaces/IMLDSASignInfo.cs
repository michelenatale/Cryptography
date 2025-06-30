

using Org.BouncyCastle.Crypto.Parameters;

namespace michele.natale.BcPqcs;


/// <summary>
/// Interface for the class MLDSASignInfo
/// </summary>
public interface IMLDSASignInfo
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
  /// Desired ML-DSA-Parameter
  /// </summary>
  MLDsaParam Parameter
  {
    get; set;
  }

  /// <summary>
  /// Return the ML-DSA-Parameter.
  /// </summary>
  /// <returns>Return the ML-DSA-Parameter.</returns>
  MLDsaParameters ToParameter();

  /// <summary>
  /// Saves all information in a file on the hardware.
  /// </summary>
  /// <param name="filename">Desired filename</param>
  void Save(string filename);

  /// <summary>
  /// Ladet all information from a file on the hardware.
  /// </summary>
  /// <param name="filename">Desired filename</param>
  void Load(string filename);

  /// <summary>
  /// Resets all information.
  /// </summary>
  void Clear();

 
}