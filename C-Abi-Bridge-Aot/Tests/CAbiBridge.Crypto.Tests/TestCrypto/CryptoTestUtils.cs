


namespace michele.natale.Tests;

internal class CryptoTestUtils
{
  internal async static Task SetRngFileDataAsync(string filename, int size)
  {
    await using var fsout = new FileStream(
      filename, FileMode.Create, FileAccess.Write);

    var length = size < 1024 * 1024 ? size : 1024 * 1024;

    while (length > 0)
    {
      fsout.Write(RngBytes(length));
      size -= length; length = size < 1024 * 1024 ? size : 1024 * 1024;
    }
  }

  internal static byte[] RngBytes(int size)
  {
    //in practice, using a crypto-random 
    var rand = Random.Shared;
    var result = new byte[size];
    rand.NextBytes(result);
    if (result[0] == 0) result[0]++;
    return result;
  }
}
