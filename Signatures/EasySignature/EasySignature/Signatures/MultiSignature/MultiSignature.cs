 

namespace michele.natale.Cryptography.Signatures;


/// <summary>
/// The class for dealing with Multi-Sign and Multi-verify.
/// </summary>
public class MultiSignature
{
  /// <summary>
  /// Seed size.
  /// </summary>
  public const int SEED_SIZE = 64;

  /// <summary>
  /// The minimum key length for the PrivateKey and PublicKey.
  /// </summary>
  public const int MIN_KEY_SIZE = 64;

  /// <summary>
  /// The maximum key length for the PrivateKey and PublicKey.
  /// </summary>
  public const int MAX_KEY_SIZE = 2048;

}
