
using System.Numerics;
using System.Threading.Channels;
using System.Security.Cryptography;


namespace michele.natale;


partial class BigPrimeGenerator
{



  //Producer UInt64
  private static async Task ProducerUInt64Async(
  ChannelWriter<ulong> writer, ulong min, ulong max,
  PrimalityConfidence confidence, CancellationToken ct)
  {
    try
    {
      while (!ct.IsCancellationRequested)
      {
        var candidate = await RngUInt64Async(min, max, confidence, ct);

        if ((candidate & 1) == 0)
        {
          if (candidate < max)
            candidate++;
          else if (candidate > min)
            candidate--;
        }

        if (candidate < min || candidate > max)
          continue;

        if (await IsMRPrimeUInt64Async(candidate, confidence, ct))
          await writer.WriteAsync(candidate, ct);
      }
    }
    catch (OperationCanceledException) { }
    finally
    {
      writer.TryComplete();
    }
  }



  //Consumer UInt64
  private static async Task<T> ConsumerAsync<T>(
    ChannelReader<T> reader, Func<T, Task>? callbackfound,
    CancellationToken ct)
  {
    await foreach (var candidate in reader.ReadAllAsync(ct))
    {
      ct.ThrowIfCancellationRequested();

      if (callbackfound != null)
        await callbackfound(candidate);

      return candidate;
    }

    return default!;
  }

  private static async Task<ulong> RngUInt64Async(
  ulong min, ulong max, PrimalityConfidence confidence,
  CancellationToken ct)
  {
    ct.ThrowIfCancellationRequested();
    await Task.Yield(); // einmalig

    ArgumentOutOfRangeException.ThrowIfGreaterThan(min, max);

    ulong range = max - min;
    Span<byte> buffer = stackalloc byte[8];

    while (true)
    {
      ct.ThrowIfCancellationRequested();
      
      buffer.Clear();
      Rand.GetBytes(buffer);

      var val = BitConverter.ToUInt64(buffer);

      if (range == ulong.MaxValue)
        return val;

      if (val <= range)
        return min + val;
    }
  }

  public static async Task<ulong> ToPrimeUInt64Async(
    ulong min, ulong max, int miller_rabin_rounds, CancellationToken ct)
  {
    var confidence = (PrimalityConfidence)miller_rabin_rounds;

    var channel = Channel.CreateBounded<ulong>(Environment.ProcessorCount * 4);
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(ct);

    int parallelism = Environment.ProcessorCount;

    var producers = Enumerable.Range(0, parallelism)
      .Select(_ => Task.Run(() => ProducerUInt64Async(
        channel.Writer, min, max, confidence, cts.Token)))
      .ToArray();

    var prime = await ConsumerAsync(channel.Reader, async p =>
    {
      cts.Cancel();
      await Task.CompletedTask;
    }, cts.Token);

    await Task.WhenAll(producers);

    if (prime != 0)
      return prime;

    throw new InvalidOperationException("No prime found before cancellation.");
  }

  public static async Task<IReadOnlyList<ulong>> ToPrimesUInt64ParallelAsync(
    ulong min, ulong max, int count, int miller_rabin_rounds, CancellationToken ct)
  {
    var confidence = (PrimalityConfidence)miller_rabin_rounds;

    var tasks = Enumerable.Range(0, count)
      .Select(_ => ToPrimeUInt64Async(min, max, miller_rabin_rounds, ct))
      .ToArray();

    var primes = await Task.WhenAll(tasks);
    return primes;
  }


  private static async Task<bool> IsMRPrimeUInt64Async(
    ulong candidate, PrimalityConfidence confidence, CancellationToken ct)
  {
    ct.ThrowIfCancellationRequested();
    await Task.Yield();

    if (candidate < 2) return false;
    if (candidate == 2) return true;
    if ((candidate & 1) == 0) return false;

    ulong d = candidate - 1;
    int s = 0;
    while ((d & 1) == 0)
    {
      d >>= 1; s++;
    }

    // deterministic bases for 64-bit
    ulong[] bases = { 2, 325, 9375, 28178, 450775, 9780504, 1795265022 };
    int rounds = (int)confidence;

    for (int i = 0; i < rounds && i < bases.Length; i++)
    {
      ct.ThrowIfCancellationRequested();
      await Task.Yield();

      ulong a = bases[i] % candidate;
      if (a == 0) continue;

      //ulong x = ModPow(a, d, candidate);
      var x = (ulong)BigInteger.ModPow(a, d, candidate);
      if (x == 1 || x == candidate - 1)
        continue;

      bool witness = true;
      for (int r = 1; r < s; r++)
      {
        ct.ThrowIfCancellationRequested();
        await Task.Yield();

        //x = ModPow(x, 2, candidate);
        x = (ulong)BigInteger.ModPow(a, d, candidate);
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

  //private static ulong ModPow(ulong b, ulong e, ulong m)
  //{
  //  //BigInteger.ModPow(b, e, m);
  //  b %= m;
  //  ulong result = 1;

  //  while (e > 0)
  //  {
  //    if ((e & 1) == 1)
  //      result = (ulong)(((BigInteger)result * b) % m);

  //    e >>= 1;
  //    if (e > 0)
  //      b = (ulong)(((BigInteger)b * b) % m);
  //  }

  //  return result;
  //}
}
