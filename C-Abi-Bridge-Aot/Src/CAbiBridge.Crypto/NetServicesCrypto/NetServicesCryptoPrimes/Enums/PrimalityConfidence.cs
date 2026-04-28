

namespace michele.natale;


//******************************************************
//* BitPrimeLength   * Recommended PrimalityConfidence *
//******************************************************
//* 2048	           * Normal(40)                      *
//* 4096	           * Authorities(64)                 *
//* 8192	           * Bottom(32) oder Bankings(72)    *
//* 16384+	         * Top(96) oder Extreme(128)       *
//******************************************************


/// <summary>
/// Defines confidence levels for probabilistic primality testing using the Miller–Rabin algorithm.
/// </summary>
/// <remarks>
/// <para>
/// The <see cref="PrimalityConfidence"/> enumeration specifies the number of Miller–Rabin rounds 
/// performed during primality testing. Higher values reduce the probability of false positives 
/// (accepting a composite as prime) but increase computational cost.
/// </para>
/// <para>
/// Typical usage:
/// <list type="bullet">
///   <item><description><see cref="Bottom"/> – minimal confidence, fast but less reliable.</description></item>
///   <item><description><see cref="Normal"/> – balanced confidence, suitable for general use.</description></item>
///   <item><description><see cref="Authorities"/> – stronger confidence, recommended for secure applications.</description></item>
///   <item><description><see cref="Bankings"/> – high confidence, suitable for financial or sensitive contexts.</description></item>
///   <item><description><see cref="Top"/> – very high confidence, rarely needed outside specialized cryptography.</description></item>
///   <item><description><see cref="Extreme"/> – maximum confidence, extremely secure but computationally expensive.</description></item>
/// </list>
/// </para>
/// </remarks>
public enum PrimalityConfidence : int
{
  /// <summary>
  /// Minimal confidence level (32 rounds). Fast but less reliable.
  /// </summary>
  Bottom = 32,

  /// <summary>
  /// Balanced confidence level (40 rounds). Suitable for general use.
  /// </summary>
  Normal = 40,

  /// <summary>
  /// Strong confidence level (64 rounds). Recommended for secure applications.
  /// </summary>
  Authorities = 64,

  /// <summary>
  /// High confidence level (72 rounds). Suitable for banking or financial contexts.
  /// </summary>
  Bankings = 72,

  /// <summary>
  /// Very high confidence level (96 rounds). Rarely needed outside specialized cryptography.
  /// </summary>
  Top = 96,

  /// <summary>
  /// Maximum confidence level (128 rounds). Extremely secure but computationally expensive.
  /// </summary>
  Extreme = 128,
}
