﻿

using System.Security.Cryptography;

namespace michele.natale.Cryptography.Signatures;

using static Services.SignatureServices;

public class SingleSignature
{


  /// <summary>
  /// Seed size.
  /// </summary>
  public const int SEED_SIZE = 64;

  /// <summary>
  /// The minimum key length for the PrivateKey and PublicKey.
  /// </summary>
  public const int MIN_KEY_SIZE = 64;

  /// <summary>
  /// The maximum key length for the PrivateKey and PublicKey.
  /// </summary>
  public const int MAX_KEY_SIZE = 2048;

  /// <summary>
  /// Creates a Signature.
  /// </summary>
  /// <param name="key">Desired Key (PrivateKey)</param>
  /// <param name="message">Desired Message</param>
  /// <returns>Array of byte</returns>
  public static byte[] Sign(
    ReadOnlySpan<byte> key, ReadOnlySpan<byte> message)
  {
    var pk = CreatePublicKey(key);
    var hpk = SHA512.HashData(pk);
    var msg = SHA512.HashData(message);
    var result = HMACSHA512.HashData(pk, msg)
      .Concat(HMACSHA512.HashData(hpk, msg))
      .Select((x, i) => (byte)(x ^ hpk[i % hpk.Length])).ToArray();
    MemoryClear(msg, pk);
    return result;
  }

  /// <summary>
  /// Creates a stronger signature.
  /// </summary>
  /// <param name="key">Desired Key (PrivateKey)</param>
  /// <param name="message">Desired Message</param>
  /// <param name="seed">Desired Seed</param>
  /// <returns>Array of byte</returns>
  public static byte[] Sign(
      ReadOnlySpan<byte> key, ReadOnlySpan<byte> message, 
      ReadOnlySpan<byte> seed)
  {
    var pk = CreatePublicKey(key, seed);
    var hpk = SHA512.HashData(pk);
    var msg = SHA512.HashData(message);
    var result = HMACSHA512.HashData(pk, msg)
      .Concat(HMACSHA512.HashData(hpk, msg))
      .Select((x, i) => (byte)(x ^ hpk[i % hpk.Length])).ToArray();
    MemoryClear(msg, pk);
    return result;
  }

  /// <summary>
  /// Returns the verification.
  /// </summary>
  /// <param name="key">Desired Key</param>
  /// <param name="sign">Desired Sign</param>
  /// <param name="message">Desired Message</param>
  /// <returns>True, if Verify is O.K., otherwise false.</returns>
  public static bool Verify(
    ReadOnlySpan<byte> key, ReadOnlySpan<byte> sign, ReadOnlySpan<byte> message)
  {
    var hpk = SHA512.HashData(key);
    var msg = SHA512.HashData(message);
    var shash = HMACSHA512.HashData(key, msg)
      .Concat(HMACSHA512.HashData(hpk, msg))
      .Select((x, i) => (byte)(x ^ hpk[i % hpk.Length])).ToArray();

    var result = sign.SequenceEqual(shash);
    MemoryClear(hpk, msg, shash);
    return result;
  }

  /// <summary>
  /// Generates a key pair for creating a signature and verifying it.
  /// </summary>
  /// <param name="seed">Desired Seed</param>
  /// <param name="size">Desired Size</param>
  /// <param name="extra_force">Desired Force</param>
  /// <returns></returns>
  public static (byte[] PrivateKey, byte[] PublicKey) CreateKeyPair(
    ReadOnlySpan<byte> seed, int size = 128, bool extra_force = false)
  {
    AssertCreatePair(seed, size);

    byte[] pubkey;
    var privkey = CreatePrivateKey(seed, size);
    if(extra_force)
      pubkey = CreatePublicKey(privkey, seed);
    else pubkey = CreatePublicKey(privkey);

    return (privkey, pubkey);
  }

  private static byte[] CreatePublicKey(
    ReadOnlySpan<byte> key) =>
      CreateKey(key, key.Length);

  private static byte[] CreatePublicKey(
    ReadOnlySpan<byte> key, ReadOnlySpan<byte> seed)
  {
    var length = key.Length;
    var s = SHA512.HashData(seed);
    var k = key.ToArray()
      .Select((x, i) => (byte)(x ^ s[i % s.Length]))
      .ToArray();

    return CreateKey(k, length);
  }

  private static byte[] CreatePrivateKey(
    ReadOnlySpan<byte> seed, int size) =>
      CreateKey(seed, size);

  private static byte[] CreateKey(
    ReadOnlySpan<byte> seed, int size = 128)
  {
    var ksize = (size - 1) / SEED_SIZE + 1;

    var sum = seed.ToArray().Sum(x => x);
    var sum_bytes = BitConverter.GetBytes(sum);
    if (BitConverter.IsLittleEndian) Array.Reverse(sum_bytes);

    var seed_bytes = new byte[seed.Length + 2 * sum_bytes.Length];
    seed.CopyTo(seed_bytes); sum_bytes.CopyTo(seed_bytes, seed.Length);

    var seed_size = ksize * SEED_SIZE;
    var result = new byte[seed_size];
    var hash_seed = SHA512.HashData(seed);
    var offset = seed.Length + sum_bytes.Length;
    for (int i = 0; i < ksize; i++)
    {
      var cnt_bytes = BitConverter.GetBytes(i);
      if (BitConverter.IsLittleEndian) Array.Reverse(sum_bytes);
      cnt_bytes.CopyTo(seed_bytes, offset);

      var h1 = SHA512.HashData(seed_bytes);
      var h2 = HMACSHA512.HashData(seed, h1)
        .Select((x, i) => (byte)(x ^ hash_seed[i % hash_seed.Length])).ToArray();

      h2.CopyTo(result, i * h2.Length);
    }

    MemoryClear(sum_bytes, seed_bytes, hash_seed);

    if (result.Length > size)
      return result.Take(size).ToArray();

    return result;
  }


  private static void AssertCreatePair(
    ReadOnlySpan<byte> seed, int size)
  {
    if (seed.Length != SEED_SIZE)
      throw new ArgumentOutOfRangeException(nameof(seed));

    if (size < MIN_KEY_SIZE || size > MAX_KEY_SIZE)
      throw new ArgumentOutOfRangeException(nameof(size));
  }

}