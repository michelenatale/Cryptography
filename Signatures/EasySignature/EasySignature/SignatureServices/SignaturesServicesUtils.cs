

using System.Numerics;

namespace michele.natale.Cryptography.Signatures.Services;


partial class SignatureServices
{

  internal static T[] ToCoprimes<T>(
    T[] left, T start, int size)
      where T : INumber<T>
  {
    var buffer = new List<T>();
    var result = new List<T>();
    buffer.Clear(); result.Clear();
    while (result.Count < size)
    {
      buffer.Clear();
      foreach (var item in left)
        if (IsCoprime(item, start))
          buffer.Add(start);
      if (buffer.Count == left.Length)
        result.Add(buffer.First());
      start++;
    }
    return [.. result];
  }

  private static T Gcd<T>(T a, T b)
    where T : INumber<T> =>
      b == T.Zero ? a : Gcd(b, a % b);

  private static bool IsCoprime<T>(T left, T right)
    where T : INumber<T> =>
      Gcd(left, right) == T.One;
}
