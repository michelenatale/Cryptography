
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace michele.natale.CAbiBridge;

using static NetServices;

partial class CryptoBridge
{

  [UnmanagedCallersOnly(EntryPoint = "rng_crypto_bytes_aot")]
  public unsafe static CError RngCryptoBytesAot(
    int size, byte** out_ptr, bool no_zeros = true)
  {
    try
    {
      if (size <= 0)
        return new CError { error_code = (int)CErrorCode.InvalidLength };

      var result = RngCryptoBytes(size, no_zeros);
      var buffer = (byte*)NativeMemory.Alloc((nuint)result.Length);
      result.CopyTo(new Span<byte>(buffer, result.Length));
      *out_ptr = buffer;

      if (buffer is not null)
        return new CError { error_code = (int)CErrorCode.Ok };

      throw new CryptographicException(
        $"The method '{nameof(RngCryptoBytes)}' returned a null value!!");
    }
    catch (CryptographicException ex)
    {
      return CreateError(CErrorCode.CryptoError, ex.Message);
    }
    catch (Exception ex)
    {
      return CreateError(CErrorCode.UnknownError, ex.Message);
    }
  }

  [UnmanagedCallersOnly(EntryPoint = "fill_crypto_bytes_aot")]
  public unsafe static CError FillCryptoBytesAot(
    byte* bytes, int length, bool no_zeros = true)
  {
    try
    {
      if (length <= 0 || bytes == null)
        return new CError { error_code = (int)CErrorCode.InvalidLength };

      var result = new Span<byte>(bytes, length);
      FillCryptoBytes(result, no_zeros);

      if (!result.IsEmpty || result.Length > 0)
        return new CError { error_code = (int)CErrorCode.Ok };

      throw new CryptographicException(
        $"The method '{nameof(FillCryptoBytes)}' returned a 'IsEmpty' value!!");
    }
    catch (CryptographicException ex)
    {
      return CreateError(CErrorCode.CryptoError, ex.Message);
    }
    catch (Exception ex)
    {
      return CreateError(CErrorCode.UnknownError, ex.Message);
    }
  }

  [UnmanagedCallersOnly(EntryPoint = "next_crypto_bool_aot")]
  public unsafe static bool NextCryptoBoolAot(CError* err)
  {
    try
    {
      var result = NextCryptoBool();
      *err = new CError { error_code = (int)CErrorCode.Ok };

      return result;
    }
    catch (Exception ex)
    {
      *err = CreateError(CErrorCode.UnknownError, ex.Message);
      return false;
    }
  }

  [UnmanagedCallersOnly(EntryPoint = "rng_crypto_bool_aot")]
  public unsafe static CError RngCryptoBoolAot(int size, bool** out_ptr)
  {
    try
    {
      if (size <= 0)
        return new CError { error_code = (int)CErrorCode.InvalidLength };

      var result = RngCryptoBool(size);
      var buffer = (bool*)NativeMemory.Alloc((nuint)result.Length);
      result.CopyTo(new Span<bool>(buffer, result.Length));
      *out_ptr = buffer;

      return new CError { error_code = (int)CErrorCode.Ok };
    }
    catch (CryptographicException ex)
    {
      return CreateError(CErrorCode.CryptoError, ex.Message);
    }
    catch (Exception ex)
    {
      return CreateError(CErrorCode.UnknownError, ex.Message);
    }
  }

  [UnmanagedCallersOnly(EntryPoint = "next_crypto_int32_aot")]
  public unsafe static int NextCryptoInt32Aot(CError* err)
  {
    try
    {
      var result = NextCryptoInt32();
      *err = new CError { error_code = (int)CErrorCode.Ok };

      return result;
    }
    catch (Exception ex)
    {
      *err = CreateError(CErrorCode.UnknownError, ex.Message);
      return int.MinValue;
    }
  }

  [UnmanagedCallersOnly(EntryPoint = "next_crypto_int32_max_aot")]
  public unsafe static int NextCryptoInt32MaxAot(int max, CError* err)
  {
    try
    {
      var result = NextCryptoInt32(0, max);
      *err = new CError { error_code = (int)CErrorCode.Ok };

      return result;
    }
    catch (Exception ex)
    {
      *err = CreateError(CErrorCode.UnknownError, ex.Message);
      return int.MinValue;
    }
  }

  [UnmanagedCallersOnly(EntryPoint = "next_crypto_int32_min_max_aot")]
  public unsafe static int NextCryptoInt32MinMaxAot(
    int min, int max, CError* err)
  {
    try
    {
      var result = NextCryptoInt32(min, max);
      *err = new CError { error_code = (int)CErrorCode.Ok };

      return result;
    }
    catch (Exception ex)
    {
      *err = CreateError(CErrorCode.UnknownError, ex.Message);
      return int.MinValue;
    }
  }

  [UnmanagedCallersOnly(EntryPoint = "rng_crypto_int32_aot")]
  public unsafe static CError RngCryptoInt32Aot(int size, IntPtr* out_ptr)
  {
    try
    {
      if (size <= 0 && CheckSetZero(out_ptr))
        return new CError { error_code = (int)CErrorCode.InvalidLength };

      var sz = Unsafe.SizeOf<int>();
      var managed = RngCryptoInt32(size, int.MaxValue);
      //if (out_ptr != null) *out_ptr = ToPtr(managed);

      var ptr = Marshal.AllocHGlobal(size * sz);
      Marshal.Copy(managed, 0, ptr, size);
      if (out_ptr != null) *out_ptr = ptr;

      return new CError { error_code = (int)CErrorCode.Ok };
    }
    catch (Exception ex)
    {
      CheckSetZero(out_ptr);
      return CreateError(CErrorCode.UnknownError, ex.Message);
    }
  }

  [UnmanagedCallersOnly(EntryPoint = "rng_crypto_int32_max_aot")]
  public unsafe static CError RngCryptoInt32MaxAot(int size, int max, IntPtr* out_ptr)
  {
    try
    {
      if (size <= 0 && CheckSetZero(out_ptr))
        return new CError { error_code = (int)CErrorCode.InvalidLength };

      var sz = Unsafe.SizeOf<int>();
      var managed = RngCryptoInt32(size, max);
      //if (out_ptr != null) *out_ptr = ToPtr2(managed);

      var ptr = Marshal.AllocHGlobal(size * sz);
      Marshal.Copy(managed, 0, ptr, size);
      if (out_ptr != null) *out_ptr = ptr;

      return new CError { error_code = (int)CErrorCode.Ok };
    }
    catch (Exception ex)
    {
      CheckSetZero(out_ptr);
      return CreateError(CErrorCode.UnknownError, ex.Message);
    }
  }


  [UnmanagedCallersOnly(EntryPoint = "rng_crypto_int32_min_max_aot")]
  public unsafe static CError RngCryptoInt32MinMaxAot(
    int size, int min, int max, IntPtr* out_ptr)
  {
    try
    {
      if (size <= 0 && CheckSetZero(out_ptr))
        return new CError { error_code = (int)CErrorCode.InvalidLength };

      var sz = Unsafe.SizeOf<int>();
      var managed = RngCryptoInt32(size, min, max);
      //if (out_ptr != null) *out_ptr = ToPtr2(managed);

      var ptr = Marshal.AllocHGlobal(size * sz);
      Marshal.Copy(managed, 0, ptr, size);
      if (out_ptr != null) *out_ptr = ptr;

      return new CError { error_code = (int)CErrorCode.Ok };
    }
    catch (Exception ex)
    {
      CheckSetZero(out_ptr);
      return CreateError(CErrorCode.UnknownError, ex.Message);
    }
  }

  [UnmanagedCallersOnly(EntryPoint = "next_crypto_int64_aot")]
  public unsafe static long NextCryptoInt64Aot(CError* err)
  {
    try
    {
      var result = NextCryptoInt64();
      *err = new CError { error_code = (int)CErrorCode.Ok };

      return result;
    }
    catch (Exception ex)
    {
      *err = CreateError(CErrorCode.UnknownError, ex.Message);
      return int.MinValue;
    }
  }

  [UnmanagedCallersOnly(EntryPoint = "next_crypto_int64_max_aot")]
  public unsafe static long NextCryptoInt64MaxAot(
    long max, CError* err)
  {
    try
    {
      var result = NextCryptoInt64(max);
      *err = new CError { error_code = (int)CErrorCode.Ok };

      return result;
    }
    catch (Exception ex)
    {
      *err = CreateError(CErrorCode.UnknownError, ex.Message);
      return int.MinValue;
    }
  }

  [UnmanagedCallersOnly(EntryPoint = "next_crypto_int64_min_max_aot")]
  public unsafe static long NextCryptoInt64MinMaxAot(
    long min, long max, CError* err)
  {
    try
    {
      var result = NextCryptoInt64(min, max);
      *err = new CError { error_code = (int)CErrorCode.Ok };

      return result;
    }
    catch (Exception ex)
    {
      *err = CreateError(CErrorCode.UnknownError, ex.Message);
      return int.MinValue;
    }
  }

  [UnmanagedCallersOnly(EntryPoint = "rng_crypto_int64_aot")]
  public unsafe static CError RngCryptoInt64Aot(
    int size, IntPtr* out_ptr)
  {
    try
    {
      if (size <= 0 && CheckSetZero(out_ptr))
        return new CError { error_code = (int)CErrorCode.InvalidLength };

      var sz = Unsafe.SizeOf<long>();
      var managed = RngCryptoInt64(size, long.MaxValue);
      //if (out_ptr != null) *out_ptr = ToPtr(managed);

      var ptr = Marshal.AllocHGlobal(size * sz);
      Marshal.Copy(managed, 0, ptr, size);
      if (out_ptr != null) *out_ptr = ptr;

      return new CError { error_code = (int)CErrorCode.Ok };
    }
    catch (Exception ex)
    {
      CheckSetZero(out_ptr);
      return CreateError(CErrorCode.UnknownError, ex.Message);
    }
  }

  [UnmanagedCallersOnly(EntryPoint = "rng_crypto_int64_max_aot")]
  public unsafe static CError RngCryptoInt64MaxAot(
    int size, long max, IntPtr* out_ptr)
  {
    try
    {
      if (size <= 0 && CheckSetZero(out_ptr))
        return new CError { error_code = (int)CErrorCode.InvalidLength };

      var sz = Unsafe.SizeOf<long>();
      var managed = RngCryptoInt64(size, max);
      //if (out_ptr != null) *out_ptr = ToPtr(managed);

      var ptr = Marshal.AllocHGlobal(size * sz);
      Marshal.Copy(managed, 0, ptr, size);
      if (out_ptr != null) *out_ptr = ptr;

      return new CError { error_code = (int)CErrorCode.Ok };
    }
    catch (Exception ex)
    {
      CheckSetZero(out_ptr);
      return CreateError(CErrorCode.UnknownError, ex.Message);
    }
  }


  [UnmanagedCallersOnly(EntryPoint = "rng_crypto_int64_min_max_aot")]
  public unsafe static CError RngCryptoInt64MinMaxAot(
    int size, long min, long max, IntPtr* out_ptr)
  {
    try
    {
      if (size <= 0 && CheckSetZero(out_ptr))
        return new CError { error_code = (int)CErrorCode.InvalidLength };

      var sz = Unsafe.SizeOf<long>();
      var managed = RngCryptoInt64(size, min, max);
      //if (out_ptr != null) *out_ptr = ToPtr(managed);

      var ptr = Marshal.AllocHGlobal(size * sz);
      Marshal.Copy(managed, 0, ptr, size);
      if (out_ptr != null) *out_ptr = ptr;

      return new CError { error_code = (int)CErrorCode.Ok };
    }
    catch (Exception ex)
    {
      CheckSetZero(out_ptr);
      return CreateError(CErrorCode.UnknownError, ex.Message);
    }
  }

  [UnmanagedCallersOnly(EntryPoint = "next_crypto_double_aot")]
  public unsafe static double NextCryptoDoubleAot(CError* err)
  {
    try
    {
      var result = NextCryptoDouble();
      *err = new CError { error_code = (int)CErrorCode.Ok };

      return result;
    }
    catch (ArgumentOutOfRangeException ex)
    {
      *err = CreateError(CErrorCode.OutOfRange, ex.Message);
      return double.MinValue;
    }
    catch (Exception ex)
    {
      *err = CreateError(CErrorCode.UnknownError, ex.Message);
      return double.MinValue;
    }
  }

  [UnmanagedCallersOnly(EntryPoint = "next_crypto_double_max_aot")]
  public unsafe static double NextCryptoDoubleMaxAot(
    double max, CError* err)
  {
    try
    {
      var result = NextCryptoDouble(max);
      *err = new CError { error_code = (int)CErrorCode.Ok };

      return result;
    }
    catch (ArgumentOutOfRangeException ex)
    {
      *err = CreateError(CErrorCode.OutOfRange, ex.Message);
      return double.MinValue;
    }
    catch (Exception ex)
    {
      *err = CreateError(CErrorCode.UnknownError, ex.Message);
      return double.MinValue;
    }
  }

  [UnmanagedCallersOnly(EntryPoint = "next_crypto_double_min_max_aot")]
  public unsafe static double NextCryptoDoubleMinMaxAot(
    double min, double max, CError* err)
  {
    try
    {
      var result = NextCryptoDouble(min, max);
      *err = new CError { error_code = (int)CErrorCode.Ok };

      return result;
    }
    catch (ArgumentOutOfRangeException ex)
    {
      *err = CreateError(CErrorCode.OutOfRange, ex.Message);
      return double.MinValue;
    }
    catch (Exception ex)
    {
      *err = CreateError(CErrorCode.UnknownError, ex.Message);
      return double.MinValue;
    }
  }

  [UnmanagedCallersOnly(EntryPoint = "rng_crypto_double_aot")]
  public unsafe static CError RngCryptoDoubleAot(
    int size, IntPtr* out_ptr)
  {
    try
    {
      if (size <= 0 && CheckSetZero(out_ptr))
        return new CError { error_code = (int)CErrorCode.InvalidLength };

      var sz = Unsafe.SizeOf<double>();
      var managed = RngCryptoDouble(size, double.MaxValue);
      //if (out_ptr != null) *out_ptr = ToPtr(managed);

      var ptr = Marshal.AllocHGlobal(size * sz);
      Marshal.Copy(managed, 0, ptr, size);
      if (out_ptr != null) *out_ptr = ptr;

      return new CError { error_code = (int)CErrorCode.Ok };
    }
    catch (Exception ex)
    {
      CheckSetZero(out_ptr);
      return CreateError(CErrorCode.UnknownError, ex.Message);
    }
  }

  [UnmanagedCallersOnly(EntryPoint = "rng_crypto_double_max_aot")]
  public unsafe static CError RngCryptoDoubleMaxAot(
    int size, double max, IntPtr* out_ptr)
  {
    try
    {
      if (size <= 0 && CheckSetZero(out_ptr))
        return new CError { error_code = (int)CErrorCode.InvalidLength };

      var sz = Unsafe.SizeOf<double>();
      var managed = RngCryptoDouble(size, max);
      //if (out_ptr != null) *out_ptr = ToPtr(managed);

      var ptr = Marshal.AllocHGlobal(size * sz);
      Marshal.Copy(managed, 0, ptr, size);
      if (out_ptr != null) *out_ptr = ptr;

      return new CError { error_code = (int)CErrorCode.Ok };
    }
    catch (Exception ex)
    {
      CheckSetZero(out_ptr);
      return CreateError(CErrorCode.UnknownError, ex.Message);
    }
  }


  [UnmanagedCallersOnly(EntryPoint = "rng_crypto_double_min_max_aot")]
  public unsafe static CError RngCryptoDoubleMinMaxAot(
    int size, double min, double max, IntPtr* out_ptr)
  {
    try
    {
      if (size <= 0 && CheckSetZero(out_ptr))
        return new CError { error_code = (int)CErrorCode.InvalidLength };

      var sz = Unsafe.SizeOf<double>();
      var managed = RngCryptoDouble(size, min, max);
      //if (out_ptr != null) *out_ptr = ToPtr(managed);

      var ptr = Marshal.AllocHGlobal(size * sz);
      Marshal.Copy(managed, 0, ptr, size);
      if (out_ptr != null) *out_ptr = ptr;

      return new CError { error_code = (int)CErrorCode.Ok };
    }
    catch (Exception ex)
    {
      CheckSetZero(out_ptr);
      return CreateError(CErrorCode.UnknownError, ex.Message);
    }
  }

  [UnmanagedCallersOnly(EntryPoint = "next_crypto_single_aot")]
  public unsafe static float NextCryptoSingleAot(CError* err)
  {
    try
    {
      var result = NextCryptoSingle();
      *err = new CError { error_code = (int)CErrorCode.Ok };

      return result;
    }
    catch (ArgumentOutOfRangeException ex)
    {
      *err = CreateError(CErrorCode.OutOfRange, ex.Message);
      return float.MinValue;
    }
    catch (Exception ex)
    {
      *err = CreateError(CErrorCode.UnknownError, ex.Message);
      return float.MinValue;
    }
  }

  [UnmanagedCallersOnly(EntryPoint = "next_crypto_single_max_aot")]
  public unsafe static float NextCryptoSingleMaxAot(
    float max, CError* err)
  {
    try
    {
      var result = NextCryptoSingle(max);
      *err = new CError { error_code = (int)CErrorCode.Ok };

      return result;
    }
    catch (ArgumentOutOfRangeException ex)
    {
      *err = CreateError(CErrorCode.OutOfRange, ex.Message);
      return float.MinValue;
    }
    catch (Exception ex)
    {
      *err = CreateError(CErrorCode.UnknownError, ex.Message);
      return float.MinValue;
    }
  }

  [UnmanagedCallersOnly(EntryPoint = "next_crypto_single_min_max_aot")]
  public unsafe static float NextCryptoSingleMinMaxAot(
    float min, float max, CError* err)
  {
    try
    {
      var result = NextCryptoSingle(min, max);
      *err = new CError { error_code = (int)CErrorCode.Ok };

      return result;
    }
    catch (ArgumentOutOfRangeException ex)
    {
      *err = CreateError(CErrorCode.OutOfRange, ex.Message);
      return float.MinValue;
    }
    catch (Exception ex)
    {
      *err = CreateError(CErrorCode.UnknownError, ex.Message);
      return float.MinValue;
    }
  }

  [UnmanagedCallersOnly(EntryPoint = "rng_crypto_single_aot")]
  public unsafe static CError RngCryptoSingleAot(
    int size, IntPtr* out_ptr)
  {
    try
    {
      if (size <= 0 && CheckSetZero(out_ptr))
        return new CError { error_code = (int)CErrorCode.InvalidLength };

      var sz = Unsafe.SizeOf<float>();
      var managed = RngCryptoSingle(size, float.MaxValue);
      //if (out_ptr != null) *out_ptr = ToPtr(managed);

      var ptr = Marshal.AllocHGlobal(size * sz);
      Marshal.Copy(managed, 0, ptr, size);
      if (out_ptr != null) *out_ptr = ptr;

      return new CError { error_code = (int)CErrorCode.Ok };
    }
    catch (Exception ex)
    {
      CheckSetZero(out_ptr);
      return CreateError(CErrorCode.UnknownError, ex.Message);
    }
  }

  [UnmanagedCallersOnly(EntryPoint = "rng_crypto_single_max_aot")]
  public unsafe static CError RngCryptoSingleMaxAot(
    int size, float max, IntPtr* out_ptr)
  {
    try
    {
      if (size <= 0 && CheckSetZero(out_ptr))
        return new CError { error_code = (int)CErrorCode.InvalidLength };

      var sz = Unsafe.SizeOf<float>();
      var managed = RngCryptoSingle(size, max);
      //if (out_ptr != null) *out_ptr = ToPtr(managed);

      var ptr = Marshal.AllocHGlobal(size * sz);
      Marshal.Copy(managed, 0, ptr, size);
      if (out_ptr != null) *out_ptr = ptr;

      return new CError { error_code = (int)CErrorCode.Ok };
    }
    catch (Exception ex)
    {
      CheckSetZero(out_ptr);
      return CreateError(CErrorCode.UnknownError, ex.Message);
    }
  }


  [UnmanagedCallersOnly(EntryPoint = "rng_crypto_single_min_max_aot")]
  public unsafe static CError RngCryptoSingleMinMaxAot(
    int size, float min, float max, IntPtr* out_ptr)
  {
    try
    {
      if (size <= 0 && CheckSetZero(out_ptr))
        return new CError { error_code = (int)CErrorCode.InvalidLength };

      var sz = Unsafe.SizeOf<float>();
      var managed = RngCryptoSingle(size, min, max);
      //if (out_ptr != null) *out_ptr = ToPtr(managed);

      var ptr = Marshal.AllocHGlobal(size * sz);
      Marshal.Copy(managed, 0, ptr, size);
      if (out_ptr != null) *out_ptr = ptr;

      return new CError { error_code = (int)CErrorCode.Ok };
    }
    catch (Exception ex)
    {
      CheckSetZero(out_ptr);
      return CreateError(CErrorCode.UnknownError, ex.Message);
    }
  }

  [UnmanagedCallersOnly(EntryPoint = "next_crypto_decimal_aot")]
  public unsafe static decimal NextCryptoDecimalAot(CError* err)
  {
    try
    {
      var result = NextCryptoDecimal();
      *err = new CError { error_code = (int)CErrorCode.Ok };

      return result;
    }
    catch (ArgumentOutOfRangeException ex)
    {
      *err = CreateError(CErrorCode.OutOfRange, ex.Message);
      return decimal.MinValue;
    }
    catch (Exception ex)
    {
      *err = CreateError(CErrorCode.UnknownError, ex.Message);
      return decimal.MinValue;
    }
  }

  [UnmanagedCallersOnly(EntryPoint = "next_crypto_decimal_max_aot")]
  public unsafe static decimal NextCryptoDecimalMaxAot(
    decimal max, CError* err)
  {
    try
    {
      var result = NextCryptoDecimal(max);
      *err = new CError { error_code = (int)CErrorCode.Ok };

      return result;
    }
    catch (ArgumentOutOfRangeException ex)
    {
      *err = CreateError(CErrorCode.OutOfRange, ex.Message);
      return decimal.MinValue;
    }
    catch (Exception ex)
    {
      *err = CreateError(CErrorCode.UnknownError, ex.Message);
      return decimal.MinValue;
    }
  }

  [UnmanagedCallersOnly(EntryPoint = "next_crypto_decimal_min_max_aot")]
  public unsafe static decimal NextCryptoDecimalMinMaxAot(
    decimal min, decimal max, CError* err)
  {
    try
    {
      var result = NextCryptoDecimal(min, max);
      *err = new CError { error_code = (int)CErrorCode.Ok };

      return result;
    }
    catch (ArgumentOutOfRangeException ex)
    {
      *err = CreateError(CErrorCode.OutOfRange, ex.Message);
      return decimal.MinValue;
    }
    catch (Exception ex)
    {
      *err = CreateError(CErrorCode.UnknownError, ex.Message);
      return decimal.MinValue;
    }
  }

  [UnmanagedCallersOnly(EntryPoint = "rng_crypto_decimal_aot")]
  public unsafe static CError RngCryptoDecimalAot(
    int size, IntPtr* out_ptr)
  {
    try
    {
      if (size <= 0 && CheckSetZero(out_ptr))
        return new CError { error_code = (int)CErrorCode.InvalidLength };

      var sz = Unsafe.SizeOf<decimal>();
      var maxlimit = 9999999999999999999999999999m; // 10^28 - 1
      var managed = RngCryptoDecimal(size, maxlimit);
      if (out_ptr != null) *out_ptr = ToPtr(managed);

      //var tsz = size * sz;
      //var ptr = Marshal.AllocHGlobal(tsz);
      //var span = MemoryMarshal.AsBytes(managed.AsSpan());

      //fixed (byte* src = span)
      //  Buffer.MemoryCopy(src, (void*)ptr, tsz, tsz);

      //if (out_ptr != null) *out_ptr = ptr;

      return new CError { error_code = (int)CErrorCode.Ok };
    }
    catch (Exception ex)
    {
      CheckSetZero(out_ptr);
      return CreateError(CErrorCode.UnknownError, ex.Message);
    }
  }

  [UnmanagedCallersOnly(EntryPoint = "rng_crypto_decimal_max_aot")]
  public unsafe static CError RngCryptoDecimalMaxAot(
    int size, decimal max, IntPtr* out_ptr)
  {
    try
    {
      if (size <= 0 && CheckSetZero(out_ptr))
        return new CError { error_code = (int)CErrorCode.InvalidLength };

      //var sz = Unsafe.SizeOf<decimal>();
      var managed = RngCryptoDecimal(size, max);
      if (out_ptr != null) *out_ptr = ToPtr(managed);

      //var tsz = size * sz;
      //var ptr = Marshal.AllocHGlobal(tsz);
      //var span = MemoryMarshal.AsBytes(managed.AsSpan());

      //fixed (byte* src = span)
      //  Buffer.MemoryCopy(src, (void*)ptr, tsz, tsz);

      //if (out_ptr != null) *out_ptr = ptr;

      return new CError { error_code = (int)CErrorCode.Ok };
    }
    catch (Exception ex)
    {
      CheckSetZero(out_ptr);
      return CreateError(CErrorCode.UnknownError, ex.Message);
    }
  }

  [UnmanagedCallersOnly(EntryPoint = "rng_crypto_decimal_min_max_aot")]
  public unsafe static CError RngCryptoDecimalMinMaxAot(
    int size, decimal min, decimal max, IntPtr* out_ptr)
  {
    try
    {
      if (size <= 0 && CheckSetZero(out_ptr))
        return new CError { error_code = (int)CErrorCode.InvalidLength };

      var sz = Unsafe.SizeOf<decimal>();
      var managed = RngCryptoDecimal(size, min, max);
      if (out_ptr != null) *out_ptr = ToPtr(managed);

      //var tsz = size * sz;
      //var ptr = Marshal.AllocHGlobal(tsz);
      //var span = MemoryMarshal.AsBytes(managed.AsSpan());

      //fixed (byte* src = span)
      //  Buffer.MemoryCopy(src, (void*)ptr, tsz, tsz);

      //if (out_ptr != null) *out_ptr = ptr;

      return new CError { error_code = (int)CErrorCode.Ok };
    }
    catch (Exception ex)
    {
      CheckSetZero(out_ptr);
      return CreateError(CErrorCode.UnknownError, ex.Message);
    }
  }
}




//[UnmanagedCallersOnly(EntryPoint = "rng_crypto_int32_aot")]
//public unsafe static CError RngCryptoInt32Aot(int size, int** out_ptr)
//{
//  try
//  {
//    if (size <= 0)
//      return new CError { error_code = (int)CErrorCode.InvalidLength };

//    var result = RngCryptoInt32(size, int.MaxValue);
//    //var buffer = (int*)NativeMemory.Alloc((nuint)result.Length);
//    var buffer = (int*)NativeMemory.Alloc((nuint)size, sizeof(int));
//    result.CopyTo(new Span<int>(buffer, result.Length));
//    *out_ptr = buffer;

//    return new CError { error_code = (int)CErrorCode.Ok };
//  }
//  catch (CryptographicException ex)
//  {
//    return CreateError(CErrorCode.CryptoError, ex.Message);
//  }
//  catch (Exception ex)
//  {
//    return CreateError(CErrorCode.UnknownError, ex.Message);
//  }
//}

//[UnmanagedCallersOnly(EntryPoint = "rng_crypto_int32_max_aot")]
//public unsafe static CError RngCryptoInt32MaxAot(int size, int max, int** out_ptr)
//{
//  try
//  {
//    if (size <= 0)
//      return new CError { error_code = (int)CErrorCode.InvalidLength };

//    var result = RngCryptoInt32(size, 0, max);
//    var buffer = (int*)NativeMemory.Alloc((nuint)result.Length);
//    result.CopyTo(new Span<int>(buffer, result.Length));
//    *out_ptr = buffer;

//    return new CError { error_code = (int)CErrorCode.Ok };
//  }
//  catch (CryptographicException ex)
//  {
//    return CreateError(CErrorCode.CryptoError, ex.Message);
//  }
//  catch (ArgumentOutOfRangeException ex)
//  {
//    return CreateError(CErrorCode.OutOfRange, ex.Message);
//  }
//  catch (Exception ex)
//  {
//    return CreateError(CErrorCode.UnknownError, ex.Message);
//  }
//}


//[UnmanagedCallersOnly(EntryPoint = "rng_crypto_int32_min_max_aot")]
//public unsafe static CError RngCryptoInt32MinMaxAot(
//  int size, int min, int max, int** out_ptr)
//{
//  try
//  {
//    if (size <= 0)
//      return new CError { error_code = (int)CErrorCode.InvalidLength };

//    var result = RngCryptoInt32(size, min, max);
//    var buffer = (int*)NativeMemory.Alloc((nuint)result.Length);
//    result.CopyTo(new Span<int>(buffer, result.Length));
//    *out_ptr = buffer;

//    return new CError { error_code = (int)CErrorCode.Ok };
//  }
//  catch (CryptographicException ex)
//  {
//    return CreateError(CErrorCode.CryptoError, ex.Message);
//  }
//  catch (ArgumentOutOfRangeException ex)
//  {
//    return CreateError(CErrorCode.OutOfRange, ex.Message);
//  }
//  catch (Exception ex)
//  {
//    return CreateError(CErrorCode.UnknownError, ex.Message);
//  }
//}
