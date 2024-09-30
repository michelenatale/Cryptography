
namespace michele.natale.EasySignatureTest;

internal class RandomHolder
{
  public static byte[] RngBytes(int size)
  {
    var result =  new byte[size];
    Random.Shared.NextBytes(result);
    return result;
  }
}
