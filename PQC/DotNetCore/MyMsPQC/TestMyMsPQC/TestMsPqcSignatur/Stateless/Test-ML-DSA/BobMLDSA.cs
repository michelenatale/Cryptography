 
using System.Security.Cryptography;

namespace michele.natale.TestMsPqcs;

using MsPqcs; 
using Services;

public class BobMLDSA : IDisposable
{
  public byte[] PubKey { get; private set; } = null!;
  public bool IsDisposed { get; private set; } = true;
  public MlDsaKeyPairInfo Info { get; private set; } = null!;
  public MLDsaAlgorithm Algo { get; private set; } = null!; 


  public BobMLDSA()
    : this(ToRngAlgo())
  {
  }

  public BobMLDSA(MLDsaAlgorithm algo)
  {
    this.Algo = algo;

    using var kem = MLDsa.GenerateKey(algo);

    var (priv, pub) = MlDsaEx.ToKeyPair(kem);
    var keypair = new KeyPairInfo(pub, priv);
    this.PubKey = keypair.PublicKey;

    this.Info = new MlDsaKeyPairInfo(
      this.PubKey, keypair.PrivateKey.ToBytes(), algo);

    this.IsDisposed = false;
  }

  public void Clear()
  {
    if (this.IsDisposed) return;

    this.Info.Dispose();
    if (this.PubKey is not null)
      Array.Clear(this.PubKey);
     
    this.Info = null!;
    this.PubKey = null!;
    this.Algo = null!;

  }

  public (byte[] Sign, byte[] PubKey) Sign(ReadOnlySpan<byte> message)
  {
    var signatur = MlDsaEx.Sign(this.Info, message);

    return (signatur, this.PubKey);
  }

  public static bool Verify(
    ReadOnlySpan<byte> pubkeyalice,
    ReadOnlySpan<byte> signature,
    ReadOnlySpan<byte> message,
    MLDsaAlgorithm algo) =>
      MlDsaEx.Verify(algo, pubkeyalice, signature, message);

  private static MLDsaAlgorithm ToRngAlgo()
  {
    var parameters = MsPqcServices.ToMLDsaAlgorithm();
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

  ~BobMLDSA() => Dispose(false);

  public void Dispose()
  {
    Dispose(true);
    GC.SuppressFinalize(this);
  }
}