
using System.Numerics;
using System.Threading.Channels;
using System.Security.Cryptography;


namespace michele.natale;


/// <summary>
/// Provides functionality for generating large prime numbers asynchronously.
/// </summary>
/// <remarks>
/// <para>
/// The <see cref="BigPrimeGenerator"/> class implements both sequential and parallel 
/// prime generation routines. It uses a producer/consumer pipeline with adaptive 
/// parallelism to efficiently generate prime candidates and validate them using 
/// probabilistic primality tests such as Miller–Rabin.
/// </para>
/// <para>
/// Public methods include:
/// <list type="bullet">
///   <item>
///     <description><see cref="ToPrimesSerialAsync(BigPrimesOption,int,CancellationToken)"/> – generates primes sequentially.</description>
///   </item>
///   <item>
///     <description><see cref="ToPrimesParallelAsync(BigPrimesOption,int,CancellationToken)"/> – generates primes in parallel.</description>
///   </item>
///   <item>
///     <description><see cref="ToPrimeAsync(BigPrimesOption)"/> – generates a single prime without cancellation support.</description>
///   </item>
///   <item>
///     <description><see cref="ToPrimeAsync(BigPrimesOption,CancellationToken)"/> – generates a single prime with cancellation support.</description>
///   </item>
/// </list>
/// </para>
/// <para>
/// The class is designed to be audit‑friendly and reproducible, with clear separation 
/// of concerns between candidate generation, primality testing, and result collection.
/// </para>
/// </remarks>
public partial class BigPrimeGenerator
{
  private static readonly RandomNumberGenerator Rand =
      RandomNumberGenerator.Create();

  /// <summary>
  /// Generates a specified number of prime numbers in parallel using asynchronous tasks.
  /// </summary>
  /// <param name="options">
  /// Configuration options for prime generation, including bit length, 
  /// parallelism settings, and Miller–Rabin confidence level.
  /// </param>
  /// <param name="count">
  /// The number of prime numbers to generate.
  /// </param>
  /// <param name="ct">
  /// A cancellation token that can be used to cancel the operation before completion.
  /// </param>
  /// <returns>
  /// An asynchronous task that, when awaited, produces a read-only list of <see cref="BigInteger"/> 
  /// values representing the generated prime numbers.
  /// </returns>
  /// <remarks>
  /// <para>
  /// This method launches <paramref name="count"/> parallel tasks, each generating a prime number 
  /// using <see cref="ToPrimeAsync(BigPrimesOption, CancellationToken)"/>. 
  /// The results are collected and returned once all tasks complete.
  /// </para>
  /// <para>
  /// The degree of parallelism is determined by the number of tasks started, 
  /// which may increase CPU load depending on <paramref name="count"/> and the configured options.
  /// </para>
  /// <para>
  /// If <paramref name="ct"/> is triggered, the operation will attempt to cancel ongoing tasks.
  /// </para>
  /// </remarks>
  public async static Task<IReadOnlyList<BigInteger>> ToPrimesParallelAsync(
    BigPrimesOption options, int count, CancellationToken ct)
  {
    var tasks = Enumerable.Range(0, count)
      .Select(_ => ToPrimeAsync(options, ct))
      .ToArray();

    var primes = await Task.WhenAll(tasks);
    return primes;
  }

  public async static Task<IReadOnlyList<BigInteger>> ToPrimesParallelAsync(
    int miller_rabin_rounds, int bit_prime_length, int count, CancellationToken ct)
  {
    var options = new BigPrimesOption
    {
      BitLength = (BitPrimeLength)bit_prime_length,
      MaxParallelism = Environment.ProcessorCount,
      MillerRabinRounds = PrimalityConfidence.Normal
    };

    return await ToPrimesParallelAsync(options, count, ct);
  }

  /// <summary>
  /// Generates a single prime number asynchronously with cancellation support.
  /// </summary>
  /// <param name="options">
  /// Configuration options for prime generation, including bit length, parallelism settings,
  /// and Miller–Rabin confidence level.
  /// </param>
  /// <param name="ct">
  /// A cancellation token that can be used to cancel the operation before completion.
  /// </param>
  /// <returns>
  /// An asynchronous task that, when awaited, produces a <see cref="BigInteger"/> 
  /// representing the generated prime number.
  /// </returns>
  /// <remarks>
  /// <para>
  /// This method uses a producer/consumer pipeline with adaptive parallelism. Multiple producers
  /// generate candidate numbers, while a consumer validates them and returns the first prime found.
  /// </para>
  /// <para>
  /// Once a prime is found, all producers are cancelled to minimize resource usage.
  /// </para>
  /// <para>
  /// If no prime is found before cancellation, an <see cref="InvalidOperationException"/> is thrown.
  /// </para>
  /// </remarks>
  public async static Task<BigInteger> ToPrimeAsync(
    BigPrimesOption options, CancellationToken ct)
  {
    var bits = (int)options.BitLength;
    var maxprime = ToMaxNthPrime(bits);
    var testprimes = SOE(maxprime);

    var channel = Channel.CreateBounded<BigInteger>(
        options.MaxParallelism * 4);

    using var cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
    var parallelism = ServiceBigPrimesGenerator.GetAdaptiveParallelism(options.BitLength);

    // Producer starten
    var producers = Enumerable.Range(0, parallelism)
        .Select(_ => Task.Run(() => ProducerAsync(
            channel.Writer, bits, testprimes,
            options.MillerRabinRounds, cts.Token)))
        .ToArray();

    // Consumer mit Callback
    var prime = await ConsumerAsync(channel.Reader, async p =>
    {
      cts.Cancel(); // alle Producer abbrechen
      await Task.CompletedTask;
    }, cts.Token);

    await Task.WhenAll(producers);

    if (prime > 1)
      return prime;

    throw new InvalidOperationException("No prime found before cancellation.");
  }

  public async static Task<BigInteger> ToPrimeAsync(
    int miller_rabin_rounds, int bit_prime_length, CancellationToken ct)
  {
    var options = new BigPrimesOption
    {
      MaxParallelism = Environment.ProcessorCount,
      BitLength = (BitPrimeLength)bit_prime_length,
      MillerRabinRounds = (PrimalityConfidence)miller_rabin_rounds,
    };

    return await ToPrimeAsync(options, ct);
  }

  public static async Task<IReadOnlyList<BigInteger>> ToPrimesMinMaxParallelAsync(
  BigInteger min, BigInteger max, int count, int miller_rabin_rounds, CancellationToken ct)
  {
    var confidence = (PrimalityConfidence)miller_rabin_rounds;

    var tasks = Enumerable.Range(0, count)
      .Select(_ => ToPrimeMinMaxAsync(min, max, miller_rabin_rounds, ct))
      .ToArray();

    return await Task.WhenAll(tasks);
  }

  private static async Task ProducerMinMaxAsync(
  ChannelWriter<BigInteger> writer, BigInteger min, BigInteger max,
  PrimalityConfidence confidence, CancellationToken ct)
  {
    try
    {
      var maxprime = ToMaxNthPrime((int)BigInteger.Log(max, 2));
      var testprimes = SOE(maxprime);

      while (!ct.IsCancellationRequested)
      {
        var candidate = await GenerateCandidatesAsync(
          min, max, testprimes, confidence, ct);

        if (candidate > 1)
          await writer.WriteAsync(candidate, ct);
      }
    }
    catch (OperationCanceledException) { }
    finally
    {
      writer.TryComplete();
    }
  }

  public static async Task<BigInteger> ToPrimeMinMaxAsync(
    BigInteger min, BigInteger max, int miller_rabin_rounds,
    CancellationToken ct)
  {
    var confidence = (PrimalityConfidence)miller_rabin_rounds;

    var channel = Channel.CreateBounded<BigInteger>(Environment.ProcessorCount * 4);
    using var cts = CancellationTokenSource.CreateLinkedTokenSource(ct);

    int parallelism = Environment.ProcessorCount;

    var producers = Enumerable.Range(0, parallelism)
      .Select(_ => Task.Run(() => ProducerMinMaxAsync(
        channel.Writer, min, max, confidence, cts.Token)))
      .ToArray();

    var prime = await ConsumerAsync(channel.Reader, async p =>
    {
      cts.Cancel();
      await Task.CompletedTask;
    }, cts.Token);

    await Task.WhenAll(producers);

    if (prime > 1)
      return prime;

    throw new InvalidOperationException("No prime found before cancellation.");
  }



  // Consumer Biginteger
  private static async Task<BigInteger> ConsumerAsync(
    ChannelReader<BigInteger> reader, Func<BigInteger,
    Task>? callbackfound, CancellationToken ct)
  {
    await foreach (var candidate in reader.ReadAllAsync(ct))
    {
      ct.ThrowIfCancellationRequested();

      if (candidate > 1)
      {
        if (callbackfound != null)
          await callbackfound(candidate);

        return candidate;
      }
    }

    return -1;
  }

  // Producer v
  private static async Task ProducerAsync(
    ChannelWriter<BigInteger> writer, int bits, int[] testprimes,
    PrimalityConfidence confidence, CancellationToken ct)
  {
    var (min, max) = ToMinMax(bits);

    try
    {
      while (!ct.IsCancellationRequested)
      {
        var candidate = await GenerateCandidatesAsync(
            min, max, testprimes, confidence, ct);

        if (!await IsTriviallyBadAsync(candidate, ct))
          await writer.WriteAsync(candidate, ct);
      }
    }
    catch (OperationCanceledException) { }
    finally
    {
      writer.TryComplete();
    }
  }

  private static async Task<bool> IsTriviallyBadAsync(
    BigInteger candidate, CancellationToken token)
  {
    token.ThrowIfCancellationRequested();
    await Task.Yield();

    int[] primes = [3, 5, 7, 11, 13, 17, 19];
    return primes.Any(p => candidate % p == 0);
  }

  private static int ToMaxNthPrime(int bits)
  {
    //https://de.wikipedia.org/wiki/Primzahlsatz#Zahlenbeispiele
    ArgumentOutOfRangeException.ThrowIfLessThan(bits, 1);
    if (bits <= 6) return new[] { 2, 3, 5, 7, 11, 13 }[bits - 1];
    double dn = bits, ln = Math.Log(dn), lln = Math.Log(ln);
    return (int)Math.Ceiling(dn * (ln + lln + 1.0));
  }

  private static (BigInteger Min, BigInteger Max) ToMinMax(int bits) =>
    (BigInteger.One << (bits - 1), (BigInteger.One << bits) - 1);

  private static async Task<BigInteger> RngBigIntegerAsync(
    BigInteger min, BigInteger max, CancellationToken token)
  {
    while (true)
    {
      token.ThrowIfCancellationRequested();
      await Task.Yield();

      var bytes = (max - min).ToByteArray();
      Rand.GetNonZeroBytes(bytes);

      bytes[^1] = (byte)(bytes.Last() & 0x7F);

      var result = min + new BigInteger(bytes);
      if (result <= max)
        return result;
    }
  }

  private static async Task<BigInteger> GenerateCandidatesAsync(
    BigInteger min, BigInteger max, int[] testPrimes,
    PrimalityConfidence confidence, CancellationToken token)
  {
    while (!token.IsCancellationRequested)
    {
      var candidate = await RngBigIntegerAsync(min, max, token);

      if (candidate.IsEven)
        candidate = candidate > min ? candidate - 1 : candidate + 1;

      if (candidate >= min && candidate <= max)
        if (await CheckCandidateAsync(candidate, testPrimes, token))
          if (await IsMRPrimeAsync(candidate, confidence, token))
            return candidate;
    }

    return -1;
  }

  private static async Task<bool> CheckCandidateAsync(
    BigInteger candidate, int[] testprimes, CancellationToken token)
  {
    if (candidate.IsEven)
      return false;

    for (int i = 1; i < testprimes.Length; i++)
    {
      token.ThrowIfCancellationRequested();
      await Task.Yield();

      if (candidate == testprimes[i])
        return true;

      if (candidate % testprimes[i] == 0)
        return false;
    }

    return true;
  }

  private static async Task<bool> IsMRPrimeAsync(
    BigInteger candidate, PrimalityConfidence confidence,
    CancellationToken token)
  {
    token.ThrowIfCancellationRequested();
    if (candidate < 2) return false;
    if (candidate == 2) return true;
    if (candidate.IsEven) return false;

    var s = 0;
    var d = candidate - 1;
    while (d.IsEven)
    {
      d >>= 1;
      s++;
    }

    var rounds = (int)confidence;
    var buffer = new byte[candidate.GetByteCount() + 1];

    for (int i = 0; i < rounds; i++)
    {
      token.ThrowIfCancellationRequested();

      BigInteger a;
      do
      {
        Rand.GetBytes(buffer, 0, buffer.Length - 1);
        buffer[^1] = 0;
        a = new BigInteger(buffer);
      } while (a < 2 || a >= candidate - 2);

      var x = BigInteger.ModPow(a, d, candidate);
      if (x == 1 || x == candidate - 1) continue;

      var witness = true;
      for (int r = 1; r < s; r++)
      {
        token.ThrowIfCancellationRequested();

        x = BigInteger.ModPow(x, 2, candidate);
        if (x == candidate - 1)
        {
          witness = false;
          break;
        }
      }

      if (witness) return false;
      await Task.Yield();
    }

    return true;
  }


  private static int[] SOE(int max)
  {
    var sqrt = (int)Math.Round(Math.Truncate(Math.Sqrt(max)));
    var seed = Enumerable.Range(2, max - 1)
      .Select(x => (x != 2) && (x & 1) == 0 ? 0 : x).ToArray();
    var primes = Enumerable.Range(2, sqrt)
    .Where(x => (x & 1) == 1)
    .Aggregate(seed, (sieve, i) =>
    {
      if (sieve[i - 2] == 0)
        return sieve;
      int j = i * i;
      while (j <= max)
      {
        sieve[j - 2] = 0;
        j += 2 * i;
      }
      return sieve;
    }).Where(k => !(k == 0));
    return [.. primes];
  }



}
