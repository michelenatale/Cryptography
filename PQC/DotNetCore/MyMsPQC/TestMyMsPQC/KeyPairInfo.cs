


namespace michele.natale.TestMsPqcs;


using Pointers;


public class KeyPairInfo : IDisposable
{
  public byte[] PublicKey { get; private set; } = [];
  public bool IsDisposed { get; private set; } = false;
  public UsIPtr<byte> PrivateKey { get; private set; } = UsIPtr<byte>.Empty;


  public KeyPairInfo(ReadOnlySpan<byte> pubkey, ReadOnlySpan<byte> privkey)
  {
    this.PublicKey = pubkey.ToArray();
    this.PrivateKey = new UsIPtr<byte>(privkey);
  }

  public KeyPairInfo(ReadOnlySpan<byte> pubkey, UsIPtr<byte> privkey)
  {
    this.PrivateKey = privkey.Copy;
    this.PublicKey = pubkey.ToArray();
  }

  public void Clear()
  {
    this.PrivateKey.Dispose();
    Array.Clear(this.PublicKey);

    this.PublicKey = [];
    this.PrivateKey = UsIPtr<byte>.Empty;
  }

  protected virtual void Dispose(bool disposing)
  {
    if (!this.IsDisposed)
    {
      if (disposing)
        this.Clear();
      this.IsDisposed = true;
    }
  }

  ~KeyPairInfo() =>
   Dispose(disposing: false);


  public void Dispose()
  {
    Dispose(disposing: true);
    GC.SuppressFinalize(this);
  }
}