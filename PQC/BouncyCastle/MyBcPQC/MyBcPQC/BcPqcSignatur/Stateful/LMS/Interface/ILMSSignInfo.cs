


namespace michele.natale.BcPqcs;

using Services;

public interface ILMSSignInfo
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
  /// Desired LMS-Parameter
  /// </summary> 
  LmsParam Parameter
  {
    get; set;
  }

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