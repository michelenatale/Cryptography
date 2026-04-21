


using System.Text;
using System.Numerics;

namespace michele.natale.Tests;

using static CryptoTestUtils;

partial class TestServices
{

  public static byte[] Converter_2_256_LE_S(
    BigInteger bi, int start_base, int target_base) =>
      Converter_2_256_LE_S(
        bi.ToByteArray().Reverse().ToArray(),
        start_base, target_base);

  public static byte[] Converter_2_256_LE_S(
    ReadOnlySpan<byte> bytes, int start_base, int target_base)
  {
    var err = Native.Converter_2_256_LE_Aot(
     bytes, bytes.Length, start_base, target_base,
     out var out_ptr, out var out_length); //base 10
    AssertError(err);

    var result = ToBytes(out_ptr, out_length);
    Native.FreeBuffer(out_ptr);

    return result;
  }

  public static byte[] ToBaseX_2_256_LE_S(
    BigInteger bi, int target_base) =>
    ToBaseX_2_256_LE_S(bi.ToString(), target_base); 

  public static byte[] ToBaseX_2_256_LE_S(
    string number, int target_base) =>
      ToBaseXUtf8_2_256_LE_S(Encoding.UTF8.GetBytes(number),
        target_base);

  public static byte[] ToBaseX_2_256_LE_S(
    ReadOnlySpan<byte> bytes, int target_base)
  {
    var err = Native.ToBaseX_2_256_LE_Aot(
     bytes, bytes.Length, target_base,
     out var out_ptr, out var out_length); //base 10
    AssertError(err);

    var result = ToBytes(out_ptr, out_length);
    Native.FreeBuffer(out_ptr);

    return result;
  }

  public static byte[] ToBaseXUtf8_2_256_LE_S(
    ReadOnlySpan<byte> bytes, int target_base)
  {
    var err = Native.ToBaseXUtf8_2_256_LE_Aot(
     bytes, bytes.Length, target_base,
     out var out_ptr, out var out_length); //base 10
    AssertError(err);

    var result = ToBytes(out_ptr, out_length);
    Native.FreeBuffer(out_ptr);

    return result;
  }

  public static byte[] FromBaseX_2_256_LE_S(
    ReadOnlySpan<byte> bytes, int from_base_x)
  {
    var err = Native.FromBaseX_2_256_LE_Aot(
     bytes, bytes.Length, from_base_x,
     out var out_ptr, out var out_length); //base 10
    AssertError(err);

    var result = ToBytes(out_ptr, out_length);
    Native.FreeBuffer(out_ptr);

    return result;
  }

  public static (int StartBase, int TargetBase) RngBases_2_256()
  {
    int targetbase;
    var rand = Random.Shared;
    var startbase = rand.Next(2, 256);

    while (true)
    {
      targetbase = rand.Next(2, 256);
      if (startbase != targetbase) break;
    }

    return (startbase, targetbase);
  }
}
