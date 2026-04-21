
using System.Text;
using System.Numerics;
using System.Runtime.InteropServices;

namespace michele.natale.CAbiBridge;

using static NetServicesUtils;

partial class UtilsBridge
{

  [UnmanagedCallersOnly(EntryPoint = "converter_2_256_le_aot")]
  public unsafe static CError Converter_2_256_LE(
    byte* base_x_le, int base_x_length,
    int start_base, int target_base,
    byte** output_ptr, int* output_length)
  {
    *output_ptr = null; *output_length = 0;
    AssertConverter_2_256(start_base, target_base);

    try
    {
      var bytes = CryptoBridge.ToSpanSafe(base_x_le, base_x_length);
      var base10 = start_base == 10 ? bytes : FromBase_X(bytes, start_base);
      if (target_base == 10)
      {
        var tmp = (byte*)NativeMemory.Alloc((nuint)base10.Length);
        base10.CopyTo(new Span<byte>(tmp, base10.Length));
        *output_ptr = tmp; *output_length = base10.Length;
        return new CError { error_code = (int)CErrorCode.Ok };
      }

      var result = ToBase_X(base10, target_base);
      var buffer = (byte*)NativeMemory.Alloc((nuint)result.Length);
      result.CopyTo(new Span<byte>(buffer, result.Length));
      *output_ptr = buffer; *output_length = result.Length;

      return new CError { error_code = (int)CErrorCode.Ok };
    }
    catch (Exception ex)
    {
      CheckSetZero((nint*)*output_ptr);
      return CreateError(CErrorCode.UnknownError, ex.Message);
    }
  }

  [UnmanagedCallersOnly(EntryPoint = "to_base_x_2_256_le_aot")]
  public unsafe static CError ToBaseX_2_256_LE(
    byte* bytes_base10_le, int bytes_length, int target_base,
    byte** output_ptr, int* output_length)
  {
    *output_ptr = null; *output_length = 0;
    Assert_2_256(target_base, nameof(target_base));

    try
    {
      var base10 = CryptoBridge.ToSpanSafe(bytes_base10_le, bytes_length);
      if (target_base == 10)
      {
        var tmp = (byte*)NativeMemory.Alloc((nuint)base10.Length);
        base10.CopyTo(new Span<byte>(tmp, base10.Length));
        *output_ptr = tmp; *output_length = base10.Length;
        return new CError { error_code = (int)CErrorCode.Ok };
      }

      var result = Converter(base10, 10, target_base);
      var buffer = (byte*)NativeMemory.Alloc((nuint)result.Length);
      result.CopyTo(new Span<byte>(buffer, result.Length));
      *output_ptr = buffer; *output_length = result.Length;

      return new CError { error_code = (int)CErrorCode.Ok };
    }
    catch (Exception ex)
    {
      CheckSetZero((nint*)*output_ptr);
      return CreateError(CErrorCode.UnknownError, ex.Message);
    }
  }

  [UnmanagedCallersOnly(EntryPoint = "to_base_x_utf8_2_256_le_aot")]
  public unsafe static CError ToBaseXUtf8_2_256_LE(
    byte* bytes_base10_utf8_le, int bytes_utf8_length, int target_base,
    byte** output_ptr, int* output_length)
  {
    //Die UTF8-String-Variante, für z.b. stringbytes, biginteger, etc.

    *output_ptr = null; *output_length = 0;
    Assert_2_256(target_base, nameof(target_base));

    try
    {
      var span = CryptoBridge.ToSpanSafe(bytes_base10_utf8_le, bytes_utf8_length);
      var str_base_10 = Encoding.UTF8.GetString(span);

      if (!IsNumeric(str_base_10))
        throw new ArgumentOutOfRangeException(nameof(bytes_base10_utf8_le),
          $"{nameof(str_base_10)} not all digits are numbers");

      var base10 = str_base_10.Select(x => (byte)(x - 48)).ToArray();
      if (target_base == 10)
      {
        var tmp = (byte*)NativeMemory.Alloc((nuint)base10.Length);
        base10.CopyTo(new Span<byte>(tmp, base10.Length));
        *output_ptr = tmp; *output_length = base10.Length;
        return new CError { error_code = (int)CErrorCode.Ok };
      }

      var result = Converter(base10, 10, target_base);
      var buffer = (byte*)NativeMemory.Alloc((nuint)result.Length);
      result.CopyTo(new Span<byte>(buffer, result.Length));
      *output_ptr = buffer; *output_length = result.Length;

      return new CError { error_code = (int)CErrorCode.Ok };
    }
    catch (Exception ex)
    {
      CheckSetZero((nint*)*output_ptr);
      return CreateError(CErrorCode.UnknownError, ex.Message);
    }
  }

  [UnmanagedCallersOnly(EntryPoint = "from_base_x_2_256_le_aot")]
  public unsafe static CError FromBaseX_2_256_LE(
    byte* bytes_basex_le, int bytes_length, int from_base_x,
    byte** output_ptr, int* output_length)
  {
    *output_ptr = null; *output_length = 0;
    Assert_2_256(from_base_x, nameof(from_base_x));

    try
    {
      var bytes = CryptoBridge.ToSpanSafe(bytes_basex_le, bytes_length);
      if (from_base_x == 10)
      {
        var tmp = (byte*)NativeMemory.Alloc((nuint)bytes.Length);
        bytes.CopyTo(new Span<byte>(tmp, bytes.Length));
        *output_ptr = tmp; *output_length = bytes.Length;
        return new CError { error_code = (int)CErrorCode.Ok };
      }

      var bi = BigInteger.Zero;
      var length = bytes.Length;
      for (var i = 0; i < length; i++)
        bi += bytes[^(1 + i)] * BigInteger.Pow(from_base_x, i);

      byte[] result = [.. bi.ToString().Select(x => (byte)(x - 48))];
      var buffer = (byte*)NativeMemory.Alloc((nuint)result.Length);
      result.CopyTo(new Span<byte>(buffer, result.Length));
      *output_ptr = buffer; *output_length = result.Length;

      return new CError { error_code = (int)CErrorCode.Ok };
    }
    catch (Exception ex)
    {
      CheckSetZero((nint*)*output_ptr);
      return CreateError(CErrorCode.UnknownError, ex.Message);
    }
  }

  private static void AssertConverter_2_256(
    int start_base, int target_base)
  {
    Assert_2_256(start_base, nameof(start_base));
    Assert_2_256(target_base, nameof(target_base));
  }

  private static void Assert_2_256(int basex, string name)
  {
    if (basex < 2 || basex > 256)
      throw new ArgumentOutOfRangeException(name,
        $"{name} must be '{name} < 2 || {name} > 256'");
  }

  private static bool IsNumeric(string number) =>
    number.All(char.IsDigit);

}
