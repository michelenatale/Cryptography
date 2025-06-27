
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Signers;
using Org.BouncyCastle.Security;

namespace michele.natale.TestBcPqcs;

using BcPqcs;
using Services;

public class BobSLHDSA : IDisposable
{
  public byte[] PubKey { get; private set; } = null!;
  public bool IsDisposed { get; private set; } = true;
  public SlhDsaKeyPairInfo Info { get; private set; } = null!;
  public SlhDsaParameters Parameter { get; private set; } = null!;

  private SecureRandom Rand = new();


  public BobSLHDSA()
    : this(ToRngParameter())
  {
  }

  public BobSLHDSA(SlhDsaParameters parameter)
  {
    this.Parameter = parameter;

    var generator = new SlhDsaKeyGenerationParameters(
      this.Rand, this.Parameter);

    var keypair_generator = new SlhDsaKeyPairGenerator();
    keypair_generator.Init(generator);

    var keypair = keypair_generator.GenerateKeyPair();
    this.PubKey = ((SlhDsaPublicKeyParameters)keypair.Public).GetEncoded();
    var privkey = ((SlhDsaPrivateKeyParameters)keypair.Private).GetEncoded();
    this.Info = new SlhDsaKeyPairInfo(this.PubKey, privkey, parameter);

    Array.Clear(privkey);
    this.IsDisposed = false;
  }

  public void Clear()
  {
    if (this.IsDisposed) return;

    if (this.PubKey is not null)
      Array.Clear(this.PubKey);

    this.Rand = null!;
    this.Info = null!;
    this.PubKey = null!;
    this.Parameter = null!;

  }

  public SlhDsaPublicKeyParameters ToPubKey() =>
    SlhDsaPublicKeyParameters.FromEncoding(
      this.Parameter, this.PubKey);


  public (byte[] Sign, byte[] PubKey) Sign(ReadOnlySpan<byte> message)
  {
    var msg = message.ToArray();

    //SLH-DSA Parameter 
    var parameter = this.Info.ToParameter();

    //Sign Message-Data
    var signer = new SlhDsaSigner(parameter, true);
    var privkey = SlhDsaPrivateKeyParameters
      .FromEncoding(parameter, this.Info.ToPrivKey().ToBytes());
    signer.Init(true, privkey);
    signer.BlockUpdate(msg, 0, msg.Length);

    return (signer.GenerateSignature(), this.PubKey);
  }

  public static bool Verify(
    ReadOnlySpan<byte> pubkeyalice,
    ReadOnlySpan<byte> signature,
    ReadOnlySpan<byte> message,
    SlhDsaParameters parameter)
  {
    var msg = message.ToArray();

    //Verify Signature
    var signer = new SlhDsaSigner(parameter, true);
    var pubkey = SlhDsaPublicKeyParameters.FromEncoding(
      parameter, pubkeyalice.ToArray());
    signer.Init(false, pubkey);
    signer.BlockUpdate(msg, 0, msg.Length);
    return signer.VerifySignature(signature.ToArray());
  }

  private static SlhDsaParameters ToRngParameter()
  {
    var rand = new SecureRandom();
    var parameters = BcPqcServices.ToSLHDsaParameters();
    var idx = rand.Next(parameters.Length);
    return parameters[idx];
  }

  public static SlhDsaPublicKeyParameters ToPubKey(
    byte[] pubkey, SlhDsaParameters parameter) =>
      SlhDsaPublicKeyParameters.FromEncoding(parameter, pubkey);

  protected virtual void Dispose(bool disposing)
  {
    if (!this.IsDisposed)
    {
      if (disposing)
      {
      }
      this.IsDisposed = true;
    }
  }

  ~BobSLHDSA() => Dispose(false);

  public void Dispose()
  {
    Dispose(true);
    GC.SuppressFinalize(this);
  }
}