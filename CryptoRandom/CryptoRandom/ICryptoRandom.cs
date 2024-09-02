

using System.Numerics;

namespace michele.natale.Cryptography.Randoms;

public interface ICryptoRandom
{
  public const string AlphaNumeric = "0123456789";
  public const string AlphaLower = "abcdefghijklmnopqrstuvwxyz";
  public const string AlphaUpper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

  public const string AlphaLowerUpper = AlphaLower + AlphaUpper;
  public const string AlphaLowerNumeric = AlphaLower + AlphaNumeric;
  public const string AlphaUpperNumeric = AlphaUpper + AlphaNumeric;
  public const string AlphaLowerUpperNumeric = AlphaLower + AlphaUpper + AlphaNumeric;

  byte NextCryptoByte(bool no_zeros = true);
  byte[] RngCryptoBytes(int size, bool no_zeros = true);
  void FillCryptoBytes(Span<byte> bytes, bool no_zeros = true);

  T NextCryptoInt<T>()
     where T : INumber<T>, INumberBase<T>, IMinMaxValue<T>;
  T NextCryptoInt<T>(T max)
     where T : INumber<T>, INumberBase<T>;
  T NextCryptoInt<T>(T min, T max)
     where T : INumber<T>, INumberBase<T>;

  T[] RngCryptoInts<T>(int size)
      where T : INumber<T>, INumberBase<T>, IMinMaxValue<T>;
  T[] RngCryptoInts<T>(int size, T max)
      where T : INumber<T>, INumberBase<T>;
  T[] RngCryptoInts<T>(int size, T min, T max)
      where T : INumber<T>, INumberBase<T>;

  void FillCryptoInts<T>(Span<T> ints)
      where T : INumber<T>, INumberBase<T>, IMinMaxValue<T>;
  void FillCryptoInts<T>(Span<T> ints, T max)
      where T : INumber<T>, INumberBase<T>;
  void FillCryptoInts<T>(Span<T> ints, T min, T max)
      where T : INumber<T>, INumberBase<T>;
}
