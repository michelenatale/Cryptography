
using System.Numerics;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Runtime.CompilerServices;

namespace michele.natale.Tests;

using static CryptoTestUtils;

partial class CryptoRandomPrimesTest
{

  public static void StartNative(int rounds)
  {
    TestNextCryptoPrimesMinMaxUInt64(rounds * 10);
    TestNextCryptoPrimesMinMaxBigInteger(rounds);
    TestNextCryptoPrimesBitsBigInteger(rounds);

    TestRngCryptoPrimesMinMaxUInt64(rounds);
    TestRngCryptoPrimesMinMaxBigInteger(rounds);
    TestRngCryptoPrimesBitsBigInteger(rounds);
  }

  private static void TestNextCryptoPrimesMinMaxUInt64(int rounds)
  {
    Console.Write($"{nameof(TestNextCryptoPrimesMinMaxUInt64)}Aot: ");

    var rand = Random.Shared;
    var sw = Stopwatch.StartNew();
    for (var i = 0; i < rounds; i++)
    {
      var (min, max) = ToMinMax<ulong>();
      var mrr = Enum.GetValues<PrimalityConfidence>();

      var miller_rabin_rounds = mrr[rand.Next(mrr.Length)];
      var prime = Native.NextCryptoPrimesMinMaxUInt64Aot(
        (int)miller_rabin_rounds, min, max, out var err);
      AssertError(err);

      if (prime < min || prime > max)
        throw new Exception();

      if (!IsMRPrimeUInt64(prime, miller_rabin_rounds))
        throw new Exception();

      if (i % (rounds / 10) == 0)
        Console.Write(".");
    }

    sw.Stop();
    var t = (double)sw.ElapsedMilliseconds;
    Console.WriteLine($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms");
  }

  private static void TestNextCryptoPrimesMinMaxBigInteger(int rounds)
  {
    Console.Write($"{nameof(TestNextCryptoPrimesMinMaxBigInteger)}Aot: ");

    var rand = Random.Shared;
    var sw = Stopwatch.StartNew();
    for (var i = 0; i < rounds; i++)
    {
      var byte_count = rand.Next(5, 32);
      var (min, max) = ToMinMaxBigInteger(byte_count);
      var mrr = Enum.GetValues<PrimalityConfidence>();

      var min_bytes = min.ToByteArray(true, true);
      var max_bytes = max.ToByteArray(true, true);
      var miller_rabin_rounds = mrr[rand.Next(mrr.Length)];

      var err = Native.NextCryptoPrimesMinMaxAot(
        (int)miller_rabin_rounds,
        min_bytes, min_bytes.Length,
        max_bytes, max_bytes.Length,
        out var out_ptr, out int out_length);
      AssertError(err);

      var prime_bytes = ToBytes(out_ptr, out_length);
      var prime = new BigInteger(prime_bytes, true, true);

      if (prime < min || prime > max)
        throw new Exception();

      if (!IsMRPrime(prime, miller_rabin_rounds))
        throw new Exception();

      if (i % (rounds / 10) == 0)
        Console.Write(".");
    }

    sw.Stop();
    var t = (double)sw.ElapsedMilliseconds;
    Console.WriteLine($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms");
  }

  private static void TestNextCryptoPrimesBitsBigInteger(int rounds)
  {
    Console.Write($"{nameof(TestNextCryptoPrimesBitsBigInteger)}Aot: ");

    var rand = Random.Shared;
    var sw = Stopwatch.StartNew();
    for (var i = 0; i < rounds; i++)
    {
      var bpl = Enum.GetValues<BitPrimeLength>();
      var mrr = Enum.GetValues<PrimalityConfidence>();

      var bits = bpl[rand.Next(10)]; //only first 10
      var miller_rabin_rounds = mrr[rand.Next(mrr.Length)];

      var err = Native.NextCryptoPrimesAot(
        (int)miller_rabin_rounds, (int)bits,
        out var out_ptr, out int out_length);
      AssertError(err);

      var prime_bytes = ToBytes(out_ptr, out_length);
      var prime = new BigInteger(prime_bytes, true, true);

      if (prime.GetBitLength() != (int)bits)
        throw new Exception();

      if (!IsMRPrime(prime, miller_rabin_rounds))
        throw new Exception();

      if (i % (rounds / 10) == 0)
        Console.Write(".");
    }

    sw.Stop();
    var t = sw.ElapsedMilliseconds;
    Console.WriteLine($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms");
  }

  private unsafe static void TestRngCryptoPrimesMinMaxUInt64(int rounds)
  {
    Console.Write($"{nameof(TestRngCryptoPrimesMinMaxUInt64)}Aot: ");

    var rand = Random.Shared;
    var sw = Stopwatch.StartNew();
    for (var i = 0; i < rounds; i++)
    {
      var count = rand.Next(10, 101);
      var (min, max) = ToMinMax<ulong>();
      var mrr = Enum.GetValues<PrimalityConfidence>();

      var miller_rabin_rounds = mrr[rand.Next(mrr.Length)];
      var err = Native.RngCryptoPrimesMinMaxUInt64Aot(
        (int)miller_rabin_rounds, count, min, max, out var output);
      AssertError(err);

      var primes = new Span<ulong>(output.ToPointer(), count);

      foreach (var prime in primes)
      {
        if (prime < min || prime > max)
          throw new Exception();

        if (!IsMRPrimeUInt64(prime, miller_rabin_rounds))
          throw new Exception();
      }

      if (i % (rounds / 10) == 0)
        Console.Write(".");
    }

    sw.Stop();
    var t = sw.ElapsedMilliseconds;
    Console.WriteLine($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms");
  }

  private unsafe static void TestRngCryptoPrimesMinMaxBigInteger(int rounds)
  {
    Console.Write($"{nameof(TestRngCryptoPrimesMinMaxBigInteger)}Aot: ");

    var total_counts = 0L;
    var rand = Random.Shared;
    var total_byte_lengths = 0;
    var sw = Stopwatch.StartNew();

    for (var i = 0; i < rounds; i++)
    {
      var counts = rand.Next(10, 101);
      total_counts += counts;

      var byte_count = rand.Next(5, 15);
      total_byte_lengths += byte_count;

      var (min, max) = ToMinMaxBigInteger(byte_count);
      var mrr = Enum.GetValues<PrimalityConfidence>();

      var min_bytes = min.ToByteArray(true, true);
      var max_bytes = max.ToByteArray(true, true);
      var miller_rabin_rounds = mrr[rand.Next(mrr.Length)];

      var err = Native.RngCryptoPrimesMinMaxAot(
        (int)miller_rabin_rounds, counts,
        min_bytes, min_bytes.Length,
        max_bytes, max_bytes.Length,
        out var output, out var out_lengths);
      AssertError(err);

      // output = byte* buffer
      // out_lengths = int* lengths
      var buffer = (byte*)output;
      var lengths = new Span<int>(out_lengths.ToPointer(), counts);

      // 1. Extract byte arrays from contiguous buffer
      int offset = 0;
      var primes_bytes = new byte[counts][];

      for (int j = 0; j < counts; j++)
      {
        int len = lengths[j];

        if (len < 0)
          throw new InvalidOperationException($"Negative length at index {j}: {len}");

        var dest = new byte[len];
        primes_bytes[j] = dest;

        if (len > 0)
        {
          var srcSpan = new Span<byte>(buffer + offset, len);
          srcSpan.CopyTo(dest);
        }

        offset += len;
      }

      // 2. Convert to BigInteger
      var primes = new BigInteger[counts];
      for (int j = 0; j < counts; j++)
        primes[j] = new BigInteger(primes_bytes[j], isUnsigned: true, isBigEndian: true);

      // 3. Validate
      foreach (var prime in primes)
      {
        if (prime < min || prime > max)
          throw new Exception();

        if (!IsMRPrime(prime, miller_rabin_rounds))
          throw new Exception();
      }

      if (i % (rounds / 10) == 0)
        Console.Write(".");
    }

    sw.Stop();
    var t = sw.ElapsedMilliseconds;
    Console.WriteLine($" rounds = {rounds}; counts = {total_counts / rounds}; bi-byte-length = {total_byte_lengths / rounds}; t = {t}ms; td = {t / rounds}ms");
  }

  private unsafe static void TestRngCryptoPrimesBitsBigInteger(int rounds)
  {
    Console.Write($"{nameof(TestRngCryptoPrimesBitsBigInteger)}Aot: ");

    var total_bits = 0L;
    var total_counts = 0L;
    var rand = Random.Shared;
    var sw = Stopwatch.StartNew();

    for (var i = 0; i < rounds; i++)
    {
      var counts = rand.Next(10, 50); total_counts += counts;
      var bpl = Enum.GetValues<BitPrimeLength>();
      var mrr = Enum.GetValues<PrimalityConfidence>();

      //var bits = bpl[rand.Next(bpl.Length)];
      var bits = bpl[rand.Next(7)]; //only first 7
      var miller_rabin_rounds = mrr[rand.Next(mrr.Length)];
      total_bits += (int)bits;

      var err = Native.RngCryptoPrimesAot(
        (int)miller_rabin_rounds, (int)bits, counts,
        out var out_ptr, out var out_lengths);
      AssertError(err);

      // out_ptr: byte* buffer with all primes concatenated
      // out_lengths: int[counts] with each prime length
      var buffer = (byte*)out_ptr;
      var lengths = new Span<int>(out_lengths.ToPointer(), counts);

      var primes_bytes = new byte[counts][];
      int offset = 0;

      for (int j = 0; j < counts; j++)
      {
        int len = lengths[j];

        if (len < 0)
          throw new InvalidOperationException($"Negative length at index {j}: {len}");

        var dest = new byte[len]; primes_bytes[j] = dest;

        if (len > 0)
        {
          var srcSpan = new Span<byte>(buffer + offset, len);
          srcSpan.CopyTo(dest);
        }

        offset += len;
      }

      var primes = new BigInteger[counts];
      for (int j = 0; j < counts; j++)
        primes[j] = new BigInteger(primes_bytes[j],
            isUnsigned: true, isBigEndian: true);

      foreach (var prime in primes)
      {
        if (prime.GetBitLength() != (int)bits)
          throw new Exception();

        if (!IsMRPrime(prime, miller_rabin_rounds))
          throw new Exception();
      }

      if (i % (rounds / 10) == 0)
        Console.Write(".");
    }

    sw.Stop();
    var t = sw.ElapsedMilliseconds;
    Console.WriteLine($" rounds = {rounds}; counts = {total_counts / rounds}; bi-bits = {total_bits / rounds}; t = {t}ms; td = {t / rounds}ms\n");
  }

  private static (T Min, T Max) ToMinMax<T>()
    where T : INumber<T>
  {
    T min, max;

    while (true)
    {
      min = NextUnmanaged<T>();
      for (var i = 0; i < 3; i++)
        if ((max = NextUnmanaged<T>()) > min)
          return (min, max);
    }
  }

  private static (BigInteger Min, BigInteger Max) ToMinMaxBigInteger(int byte_count = 32)
  {
    BigInteger min, max;

    while (true)
    {
      min = NextBigInteger(byte_count);
      for (var i = 0; i < 3; i++)
      {
        max = NextBigInteger(byte_count);
        if (max > min)
          return (min, max);
      }
    }
  }


  public static T NextUnmanaged<T>() where T : INumber<T>
  {
    var szt = Unsafe.SizeOf<T>();
    Span<byte> buffer = stackalloc byte[szt];
    Random.Shared.NextBytes(buffer);
    return T.Abs(Unsafe.ReadUnaligned<T>(ref buffer[0]));
  }


  public static BigInteger NextBigInteger(int byte_count = 32)
  {
    var bytes = new byte[byte_count];
    Random.Shared.NextBytes(bytes);

    // Sign-Bit entfernen → garantiert positiv
    bytes[^1] &= 0x7F;

    return new BigInteger(bytes);
  }



  private static bool IsMRPrimeUInt64(
    ulong candidate, PrimalityConfidence confidence)
  {
    if (candidate < 2) return false;
    if (candidate == 2) return true;
    if ((candidate & 1) == 0) return false;

    // Write n - 1 as d * 2^s
    ulong d = candidate - 1;
    int s = 0;
    while ((d & 1) == 0)
    {
      d >>= 1;
      s++;
    }

    // Deterministic bases for 64-bit
    ulong[] bases = { 2, 325, 9375, 28178, 450775, 9780504, 1795265022 };
    int rounds = (int)confidence;

    for (int i = 0; i < rounds && i < bases.Length; i++)
    {
      ulong a = bases[i] % candidate;
      if (a == 0) continue;

      //ulong x = ModPow(a, d, candidate);
      ulong x = (ulong)BigInteger.ModPow(a, d, candidate);
      if (x == 1 || x == candidate - 1)
        continue;

      bool witness = true;
      for (int r = 1; r < s; r++)
      {
        //x = ModPow(x, 2, candidate);
        x = (ulong)BigInteger.ModPow(x, d, candidate);
        if (x == candidate - 1)
        {
          witness = false;
          break;
        }
      }

      if (witness)
        return false;
    }

    return true;
  }


  private static bool IsMRPrime(
    BigInteger candidate, PrimalityConfidence confidence)
  {
    if (candidate < 2) return false;
    if (candidate == 2) return true;
    if (candidate.IsEven) return false;

    // Write n - 1 as d * 2^s
    int s = 0;
    BigInteger d = candidate - 1;
    while (d.IsEven)
    {
      d >>= 1;
      s++;
    }

    int rounds = (int)confidence;
    byte[] buffer = new byte[candidate.GetByteCount() + 1];

    for (int i = 0; i < rounds; i++)
    {
      BigInteger a;
      do
      {
        RandomNumberGenerator.Fill(buffer);
        buffer[^1] = 0; // force positive
        a = new BigInteger(buffer);
      }
      while (a < 2 || a >= candidate - 2);

      BigInteger x = BigInteger.ModPow(a, d, candidate);
      if (x == 1 || x == candidate - 1)
        continue;

      bool witness = true;
      for (int r = 1; r < s; r++)
      {
        x = BigInteger.ModPow(x, 2, candidate);
        if (x == candidate - 1)
        {
          witness = false;
          break;
        }
      }

      if (witness)
        return false;
    }

    return true;
  }

}