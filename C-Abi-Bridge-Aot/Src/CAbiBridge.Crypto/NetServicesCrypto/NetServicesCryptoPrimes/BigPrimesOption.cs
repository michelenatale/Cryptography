

namespace michele.natale;

/// <summary>
/// Represents configuration options for generating large prime numbers.
/// </summary>
/// <remarks>
/// This class encapsulates parameters that control prime generation, including 
/// the number of trial divisions, parallelism settings, bit length of the primes, 
/// and the confidence level for probabilistic primality tests.
/// </remarks>
public sealed class BigPrimesOption
{

  /// <summary>
  /// Gets the number of small primes used for trial division before 
  /// probabilistic primality testing begins.
  /// </summary>
  /// <remarks>
  /// A higher value increases the chance of quickly eliminating composite candidates, 
  /// but also adds overhead. The default is 2048.
  /// </remarks>
  public int CheckPrimeCount { get; init; } = 2048;

  /// <summary>
  /// Gets the maximum degree of parallelism allowed during prime generation.
  /// </summary>
  /// <remarks>
  /// This value typically corresponds to the number of logical processors available. 
  /// The default is <see cref="Environment.ProcessorCount"/>.
  /// </remarks>
  public int MaxParallelism { get; init; } = Environment.ProcessorCount;

  /// <summary>
  /// Gets the bit length of the prime numbers to be generated.
  /// </summary>
  /// <remarks>
  /// Determines the size of the prime candidates. Larger bit lengths require 
  /// more computational effort and longer primality testing. 
  /// The default is <see cref="BitPrimeLength.B4096"/>.
  /// </remarks>
  public BitPrimeLength BitLength { get; init; } = BitPrimeLength.B4096;

  /// <summary>
  /// Gets the confidence level for the Miller–Rabin primality test.
  /// </summary>
  /// <remarks>
  /// Controls the number of rounds performed during probabilistic primality testing. 
  /// Higher confidence levels reduce the probability of false positives but increase runtime. 
  /// The default is <see cref="PrimalityConfidence.Normal"/>.
  /// </remarks>
  public PrimalityConfidence MillerRabinRounds { get; init; } = PrimalityConfidence.Normal;

}
