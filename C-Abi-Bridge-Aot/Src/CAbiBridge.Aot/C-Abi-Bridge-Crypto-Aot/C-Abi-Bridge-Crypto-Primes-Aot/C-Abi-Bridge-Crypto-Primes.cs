
using System.Numerics; 
using System.Runtime.InteropServices; 

namespace michele.natale.CAbiBridge;

using static NetServicesUtils; 

partial class CryptoBridge
{
  [UnmanagedCallersOnly(EntryPoint = "next_crypto_primes_min_max_uint64_aot")]
  public unsafe static ulong NextCryptoPrimesMinMaxUInt64Aot(
   int miller_rabin_rounds, ulong min, ulong max, CError* err)
  {
    *err = default;

    try
    {
      var result = BigPrimeGenerator.ToPrimeUInt64Async(
        min, max, miller_rabin_rounds, CancellationToken.None)
         .GetAwaiter().GetResult();

      *err = new CError { error_code = (int)CErrorCode.Ok };

      return result;
    }
    catch (Exception ex)
    {
      *err = CreateError(CErrorCode.UnknownError, ex.Message);
      return 0;
    }
  }

  [UnmanagedCallersOnly(EntryPoint = "next_crypto_primes_min_max_aot")]
  public unsafe static CError NextCryptoPrimesMinMaxAot(
    int miller_rabin_rounds,
    byte* min_ptr, int min_length,
    byte* max_ptr, int max_length,
    IntPtr* out_ptr, int* out_length)
  {
    *out_ptr = default; *out_length = 0;

    try
    {
      var min = new BigInteger(ToSpanSafe(min_ptr, min_length), true, true);
      var max = new BigInteger(ToSpanSafe(max_ptr, max_length), true, true);

      var prime = BigPrimeGenerator.ToPrimeMinMaxAsync(
        min, max, miller_rabin_rounds, CancellationToken.None)
        .GetAwaiter().GetResult();

      var bytes = prime.ToByteArray(true, true);
      var buffer = (byte*)NativeMemory.Alloc((nuint)bytes.Length);
      bytes.CopyTo(new Span<byte>(buffer, bytes.Length));
      *out_ptr = (IntPtr)buffer; *out_length = bytes.Length;
      Array.Clear(bytes);

      return new CError { error_code = (int)CErrorCode.Ok };
    }
    catch (Exception ex)
    {
      CheckSetZero(out_ptr); *out_length = 0;
      return CreateError(CErrorCode.UnknownError, ex.Message);
    }
  }

  [UnmanagedCallersOnly(EntryPoint = "next_crypto_primes_aot")]
  public unsafe static CError NextCryptoPrimesAot(
    int miller_rabin_rounds, int bit_prime_length,
    IntPtr* out_ptr, int* out_length)
  {
    *out_ptr = IntPtr.Zero; *out_length = 0;

    try
    {
      var prime = BigPrimeGenerator.ToPrimeAsync(
        miller_rabin_rounds, bit_prime_length, CancellationToken.None)
         .GetAwaiter().GetResult();

      var bytes = prime.ToByteArray(true, true);
      var buffer = (byte*)NativeMemory.Alloc((nuint)bytes.Length);
      bytes.CopyTo(new Span<byte>(buffer, bytes.Length));
      *out_ptr = (IntPtr)buffer; *out_length = bytes.Length;
      Array.Clear(bytes);

      return new CError { error_code = (int)CErrorCode.Ok };
    }
    catch (Exception ex)
    {
      CheckSetZero(out_ptr); *out_length = 0;
      return CreateError(CErrorCode.UnknownError, ex.Message);
    }
  }

  [UnmanagedCallersOnly(EntryPoint = "rng_crypto_primes_min_max_uint64_aot")]
  public unsafe static CError RngCryptoPrimesMinMaxUInt64Aot(
    int miller_rabin_rounds, int counts,
    ulong min, ulong max, IntPtr* out_ptr)
  {
    *out_ptr = IntPtr.Zero;

    try
    {
      // 1. Generate prime numbers
      var primes = BigPrimeGenerator.ToPrimesUInt64ParallelAsync(
          min, max, counts, miller_rabin_rounds, CancellationToken.None)
          .GetAwaiter().GetResult();

      // 2. Allocate unmanaged memory for all primes
      //    counts * sizeof(ulong)
      var total_bytes = (nuint)(counts * sizeof(ulong));
      var buffer = (ulong*)NativeMemory.Alloc(total_bytes);

      // 3. Copy primes into unmanaged memory
      for (var i = 0; i < counts; i++)
        buffer[i] = primes[i];

      // 4. Return pointer to unmanaged block
      *out_ptr = (IntPtr)buffer;

      return new CError { error_code = (int)CErrorCode.Ok };
    }
    catch (Exception ex)
    {
      *out_ptr = IntPtr.Zero;
      return CreateError(CErrorCode.UnknownError, ex.Message);
    }
  }

  [UnmanagedCallersOnly(EntryPoint = "rng_crypto_primes_min_max_aot")]
  public unsafe static CError RngCryptoPrimesMinMaxAot(
    int miller_rabin_rounds, int counts,
    byte* min_ptr, int min_length,
    byte* max_ptr, int max_length,
    IntPtr* out_ptr,                    // pointer to all bytes
    IntPtr* out_lengths_ptr)            // pointer to int[counts]
  {
    *out_ptr = IntPtr.Zero; *out_lengths_ptr = IntPtr.Zero;

    try
    {
      // 1. Convert min/max to BigInteger
      var min = new BigInteger(ToSpanSafe(min_ptr, min_length), true, true);
      var max = new BigInteger(ToSpanSafe(max_ptr, max_length), true, true);
      if (min >= max) throw new ArgumentException($"{nameof(min_ptr)}!", nameof(min_ptr));

      // 2. Generate multiple primes
      var primes = BigPrimeGenerator.ToPrimesMinMaxParallelAsync(
        min, max, counts, miller_rabin_rounds, CancellationToken.None)
        .GetAwaiter().GetResult();

      // 3. Convert each prime to bytes
      var total_bytes = 0;
      var lengths = new int[counts];
      var byte_arrays = new byte[counts][];

      for (int i = 0; i < counts; i++)
      {
        var bytes = primes[i].ToByteArray(isUnsigned: true, isBigEndian: true);
        byte_arrays[i] = bytes; lengths[i] = bytes.Length; total_bytes += bytes.Length;
      }

      // 4. Allocate unmanaged memory for all bytes
      byte* buffer = (byte*)NativeMemory.Alloc((nuint)total_bytes);

      // 5. Allocate unmanaged memory for lengths
      int* lengths_ptr = (int*)NativeMemory.Alloc((nuint)(counts * sizeof(int)));

      // 6. Copy lengths
      for (int i = 0; i < counts; i++)
        lengths_ptr[i] = lengths[i];

      // 7. Copy all byte arrays into one big buffer
      for (int i = 0, offset = 0; i < counts; i++)
      {
        var bytes = byte_arrays[i];
        bytes.CopyTo(new Span<byte>(buffer + offset, bytes.Length));
        offset += bytes.Length;
        Array.Clear(bytes);
      }

      // 8. Return pointers
      *out_ptr = (IntPtr)buffer; *out_lengths_ptr = (IntPtr)lengths_ptr;

      return new CError { error_code = (int)CErrorCode.Ok };
    }
    catch (Exception ex)
    {
      CheckSetZero((nint*)*out_ptr);
      CheckSetZero((nint*)*out_lengths_ptr);
      return CreateError(CErrorCode.UnknownError, ex.Message);
    }
  }

  [UnmanagedCallersOnly(EntryPoint = "rng_crypto_primes_aot")]
  public unsafe static CError RngCryptoPrimesAot(
    int miller_rabin_rounds, int bit_prime_length, int counts,
     IntPtr* out_ptr, IntPtr* out_lengths_ptr)
  {
    *out_ptr = IntPtr.Zero; *out_lengths_ptr = IntPtr.Zero;

    try
    {
      // 1. Generate prime numbers
      var primes = BigPrimeGenerator.ToPrimesParallelAsync(
          miller_rabin_rounds, bit_prime_length, counts, CancellationToken.None)
          .GetAwaiter().GetResult();

      // 2. Prepare all byte arrays
      int total_bytes = 0;
      var lengths = new int[counts];
      var byte_arrays = new byte[counts][];

      for (int i = 0; i < counts; i++)
      {
        var bytes = primes[i].ToByteArray(isUnsigned: true, isBigEndian: true);
        byte_arrays[i] = bytes; lengths[i] = bytes.Length; total_bytes += bytes.Length;
      }

      // 3. Allocate unmanaged memory for all bytes
      var buffer = (byte*)NativeMemory.Alloc((nuint)total_bytes);

      // 4. Allocate unmanaged memory for the lengths
      var lengths_ptr = (int*)NativeMemory.Alloc((nuint)(counts * sizeof(int)));

      // 5.Copy the lengths
      for (int i = 0; i < counts; i++)
        lengths_ptr[i] = lengths[i];

      // 6. Copy the byte arrays one after another
      for (int i = 0, offset = 0; i < counts; i++)
      {
        var bytes = byte_arrays[i];
        bytes.CopyTo(new Span<byte>(buffer + offset, bytes.Length));
        offset += bytes.Length;
        Array.Clear(bytes);
      }

      // 7. Return a pointer
      *out_ptr = (IntPtr)buffer; *out_lengths_ptr = (IntPtr)lengths_ptr;

      return new CError { error_code = (int)CErrorCode.Ok };
    }
    catch (Exception ex)
    {
      CheckSetZero((nint*)*out_ptr);
      CheckSetZero((nint*)*out_lengths_ptr);
      return CreateError(CErrorCode.UnknownError, ex.Message);
    }
  }
}

