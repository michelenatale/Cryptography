

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
/// Defines supported bit lengths for prime number generation.
/// </summary>
/// <remarks>
/// <para>
/// The <see cref="BitPrimeLength"/> enumeration specifies the size of prime candidates 
/// in bits. Larger values correspond to stronger cryptographic security but require 
/// significantly more computational effort during generation and primality testing.
/// </para>
/// <para>
/// Typical usage:
/// <list type="bullet">
///   <item><description>Small values (8–128 bits) – for testing and benchmarking only.</description></item>
///   <item><description>Medium values (256–1024 bits) – suitable for performance experiments.</description></item>
///   <item><description>Large values (2048–8192 bits) – common in cryptographic applications.</description></item>
///   <item><description>Very large values (≥12288 bits) – for specialized or experimental scenarios.</description></item>
/// </list>
/// </para>
/// </remarks>
public enum BitPrimeLength : int
{
  /// <summary>Prime candidates of 8 bits (testing only).</summary>
  B8 = 8,

  /// <summary>Prime candidates of 16 bits (testing only).</summary>
  B16 = 16,

  /// <summary>Prime candidates of 32 bits (testing only).</summary>
  B32 = 32,

  /// <summary>Prime candidates of 64 bits (testing only).</summary>
  B64 = 64,

  /// <summary>Prime candidates of 96 bits (testing only).</summary>
  B96 = 96,

  /// <summary>Prime candidates of 128 bits (testing only).</summary>
  B128 = 128,

  /// <summary>Prime candidates of 192 bits (testing only).</summary>
  B192 = 192,

  /// <summary>Prime candidates of 256 bits.</summary>
  B256 = 256,

  /// <summary>Prime candidates of 384 bits.</summary>
  B384 = 384,

  /// <summary>Prime candidates of 512 bits.</summary>
  B512 = 512,

  /// <summary>Prime candidates of 768 bits.</summary>
  B768 = 768,

  /// <summary>Prime candidates of 1024 bits.</summary>
  B1024 = 1024,

  /// <summary>Prime candidates of 1536 bits.</summary>
  B1536 = 1536,

  /// <summary>Prime candidates of 2048 bits (commonly used in cryptography).</summary>
  B2048 = 2048,

  /// <summary>Prime candidates of 3072 bits.</summary>
  B3072 = 3072,

  /// <summary>Prime candidates of 4096 bits (commonly used in cryptography).</summary>
  B4096 = 4096,

  /// <summary>Prime candidates of 6144 bits.</summary>
  B6144 = 6144,

  /// <summary>Prime candidates of 8192 bits (high security).</summary>
  B8192 = 8192,

  /// <summary>Prime candidates of 12288 bits.</summary>
  B12288 = 12288,

  /// <summary>Prime candidates of 16384 bits.</summary>
  B16384 = 16384,

  /// <summary>Prime candidates of 24576 bits.</summary>
  B24576 = 24576,

  /// <summary>Prime candidates of 32768 bits.</summary>
  B32768 = 32768,

  /// <summary>Prime candidates of 49152 bits.</summary>
  B49152 = 49152,

  /// <summary>Prime candidates of 65536 bits.</summary>
  B65536 = 65536,

  /// <summary>Prime candidates of 98304 bits.</summary>
  B98304 = 98304,

  /// <summary>Prime candidates of 131072 bits.</summary>
  B131072 = 131072,
}
