

using System.Numerics;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace BitsBytesUtilsTest;


public class RandomHelper
{
  public static T RngT<T>()
    where T : INumber<T>
  {
    var rand = Random.Shared;
    var sz = Unsafe.SizeOf<T>();

    var bytes = new byte[sz];
    rand.NextBytes(bytes);

    return Unsafe.ReadUnaligned<T>(
      ref MemoryMarshal.GetReference(bytes.AsSpan()));
  }

  public static T[] RngT<T>(int size)
    where T : INumber<T> =>
      Enumerable.Range(0, size)
        .Select(_ => RngT<T>()).ToArray();


  public static BigInteger RngBigInteger(int size)
  {
    var rand = Random.Shared;
    var bytes = new byte[size];
    rand.NextBytes(bytes);
    var result = new BigInteger(bytes);

    if (result.Sign >= 0)
      return result;

    return -result;
  }

  public static BigInteger[] RngBigInteger(
    int size, int bytesize, bool issignmixes = false)
  {
    var rand = Random.Shared;
    var result = new BigInteger[size];
    for (int i = 0; i < size; i++)
    { 
      result[i] = RngBigInteger(bytesize);
      if(issignmixes && (rand.Next()&1)==1)
        result[i] = -result[i];
    }
    return result;
  }
}