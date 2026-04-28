


namespace michele.natale;



/// <summary>
/// Provides utility methods for configuring prime generation services.
/// </summary>
/// <remarks>
/// The <see cref="ServiceBigPrimesGenerator"/> class contains helper logic used by 
/// <see cref="BigPrimeGenerator"/> to determine optimal parallelism settings 
/// based on prime bit length. This ensures efficient use of CPU resources 
/// while avoiding unnecessary overhead for very small or very large prime sizes.
/// </remarks>
public class ServiceBigPrimesGenerator
{
  /// <summary>
  /// Determines the recommended degree of parallelism for prime generation 
  /// based on the specified bit length.
  /// </summary>
  /// <param name="bits">
  /// The bit length of the prime numbers to be generated, expressed as a 
  /// <see cref="BitPrimeLength"/> value.
  /// </param>
  /// <returns>
  /// An integer representing the number of parallel tasks that should be used 
  /// for prime generation at the given bit length.
  /// </returns>
  /// <remarks>
  /// <para>
  /// The method adapts parallelism according to prime size:
  /// <list type="bullet">
  ///   <item><description>≤ 128 bits – minimal workload, use 1–2 threads.</description></item>
  ///   <item><description>≤ 2048 bits – moderate workload, use all available cores.</description></item>
  ///   <item><description>≤ 4096 bits – heavy workload, use half the cores to reduce cache thrashing.</description></item>
  ///   <item><description>Default – conservative fallback, half the cores.</description></item>
  /// </list>
  /// </para>
  /// <para>
  /// This adaptive strategy balances throughput and resource contention, 
  /// making prime generation reproducible and audit‑friendly across different environments.
  /// </para>
  /// </remarks>
  public static int GetAdaptiveParallelism(BitPrimeLength bits)
  {
    var cores = Environment.ProcessorCount;

    return bits switch
    {
      // Kleine Bitlängen: kaum Rechenaufwand → nur 1–2 Threads
      <= BitPrimeLength.B128 => Math.Min(2, cores),

      // Mittlere Bitlängen: hier lohnt sich volle Parallelisierung
      <= BitPrimeLength.B2048 => cores,

      // Sehr große Bitlängen: weniger Threads, um Cache/Overhead zu reduzieren
      <= BitPrimeLength.B4096 => Math.Max(2, cores / 2),

      // Default: konservativ
      _ => Math.Max(1, cores / 2)
    };
  }
}