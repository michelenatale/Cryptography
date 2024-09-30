 

namespace michele.natale.Cryptography.Signatures.Services;


partial class SignatureServices
{ 

  /// <summary>
  /// Clears the contents of all arrays.
  /// </summary>
  /// <param name="input"></param>
  public static void MemoryClear(params byte[][] input)
  {
    for (int i = 0; i < input.Length; i++)
      Array.Clear(input[i]);
  }
}
