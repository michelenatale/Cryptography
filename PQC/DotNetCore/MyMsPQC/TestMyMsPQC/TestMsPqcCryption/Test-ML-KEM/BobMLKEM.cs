
using System.Security.Cryptography;


namespace michele.natale.TestMsPqcs;

using michele.natale.MsPqcs;
using Pointers;
using Services;

public class BobMLKEM : IDisposable
{
  public byte[] PubKey { get; private set; } = null!;
  public bool IsDisposed { get; private set; } = true;
  public byte[] CapsulationKey { get; private set; } = null!;
  public MLKemAlgorithm Algorithm { get; private set; } = null!;
  public byte[] Associated
  {
    get; private set;
  } = "© michele natale 2025"u8.ToArray();

  private KeyPairInfo KeyPair = null!;
  private UsIPtr<byte> SharedKey = UsIPtr<byte>.Empty;

  public BobMLKEM()
    : this(ToRngParameter())
  {
  }

  public BobMLKEM(MLKemAlgorithm algo)
  {
    this.Algorithm = algo;

    using var kem = MLKem.GenerateKey(algo);

    var (priv, pub) = MlKemEx.ToKeyPair(kem);
    this.KeyPair = new KeyPairInfo(pub, priv);
    this.PubKey = this.KeyPair.PublicKey;

    this.IsDisposed = false;
  }

  public void Clear()
  {
    if (this.IsDisposed) return;

    if (this.PubKey is not null)
      Array.Clear(this.PubKey);
    if (this.Associated is not null)
      Array.Clear(this.Associated);
    if (this.CapsulationKey is not null)
      Array.Clear(this.CapsulationKey);

    this.SharedKey.Dispose();
     
    this.PubKey = null!;
    this.KeyPair = null!;
    this.Algorithm = null!;
    this.CapsulationKey = null!;
    this.SharedKey = UsIPtr<byte>.Empty;
    this.Associated = "© michele natale 2025"u8.ToArray();
  }

  public byte[] Encryption(
    ReadOnlySpan<byte> bytes,
    ReadOnlySpan<byte> capsulationkey,
    ReadOnlySpan<byte> associated,
    CryptionAlgorithm cryptoalgo)
  {
    var associat = associated.IsEmpty ? this.Associated : associated;
    using var sharedkey = this.ToSharedKey(capsulationkey);

    return MsPqcServices.EncryptionWithCryptionAlgo(
      bytes, sharedkey, associat, cryptoalgo);
  }

  public byte[] Decryption(
    ReadOnlySpan<byte> bytes,
    ReadOnlySpan<byte> associated,
    CryptionAlgorithm cryptoalgo)
  {
    var associat = associated.IsEmpty ? this.Associated : associated;

    return MsPqcServices.DecryptionWithCryptionAlgo(
      bytes, this.SharedKey, associat, cryptoalgo);
  }

  public UsIPtr<byte> ToSharedKey(ReadOnlySpan<byte> capsulationkey)
  {
    //SharedKeys are always secret.

    // Bob decapsulates a new shared secret using Bob's private key
    using var bob = MLKem.ImportDecapsulationKey(this.Algorithm, this.KeyPair.PrivateKey.ToBytes());

    return MlKemEx.ToSharedKey(bob, capsulationkey);
  }

  public byte[] GenerateSharedKey(ReadOnlySpan<byte> alice_pubkey)
  {
    //SharedKeys are always secret, whereas the CapsulationKey is always public.
     
    if (this.SharedKey is not null && !this.SharedKey.IsDisposed)
      this.ClearSharedKey();

    // Bob encapsulates a new shared secret using alice's public key
    using var alice = MLKem.ImportEncapsulationKey(this.Algorithm, alice_pubkey);
    this.SharedKey = MlKemEx.ToSharedKey(alice, out var capsulationkey);
    this.CapsulationKey = capsulationkey;

    return [.. capsulationkey];
  }

  private void ClearSharedKey()
  {
    if (this.SharedKey is not null && !this.SharedKey.IsDisposed)
    {
      this.SharedKey.Dispose();
      Array.Clear(this.CapsulationKey);
      this.CapsulationKey = null!;
    }
  }

  private static MLKemAlgorithm ToRngParameter()
  { 
    var parameters = MsPqcServices.ToMLKemAlgorithm();
    var idx = RandomNumberGenerator.GetInt32(parameters.Length);
    return parameters[idx];
  }


  protected virtual void Dispose(bool disposing)
  {
    if (!this.IsDisposed)
    {
      if (disposing) this.Clear();
      this.IsDisposed = true;
    }
  }

  ~BobMLKEM() =>
    Dispose(false);

  public void Dispose()
  {
    this.Dispose(true);
    GC.SuppressFinalize(this);
  }
}