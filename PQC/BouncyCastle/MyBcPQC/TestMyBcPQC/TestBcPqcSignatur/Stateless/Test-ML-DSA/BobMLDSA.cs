using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

namespace michele.natale.TestBcPqcs;

using michele.natale.BcPqcs;
using Org.BouncyCastle.Crypto.Signers;
using Services;

public class BobMLDSA : IDisposable
{
  public byte[] PubKey { get; private set; } = null!;
  public bool IsDisposed { get; private set; } = true;
  public MlDsaKeyPairInfo Info { get; private set; } = null!;
  public MLDsaParameters Parameter { get; private set; } = null!;

  private SecureRandom Rand = new();


  public BobMLDSA()
    : this(ToRngParameter())
  {
  }

  public BobMLDSA(MLDsaParameters parameter)
  {
    this.Parameter = parameter;

    var generator = new MLDsaKeyGenerationParameters(
      this.Rand, this.Parameter);

    var keypair_generator = new MLDsaKeyPairGenerator();
    keypair_generator.Init(generator);

    var keypair = keypair_generator.GenerateKeyPair();
    this.PubKey = ((MLDsaPublicKeyParameters)keypair.Public).GetEncoded();
    var privkey = ((MLDsaPrivateKeyParameters)keypair.Private).GetEncoded();
    this.Info = new MlDsaKeyPairInfo(this.PubKey, privkey, parameter);

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

  public MLDsaPublicKeyParameters ToPubKey() =>
    MLDsaPublicKeyParameters.FromEncoding(
      this.Parameter, this.PubKey);


  public (byte[] Sign, byte[] PubKey) Sign(ReadOnlySpan<byte> message)
  {
    var msg = message.ToArray();

    //ML-DSA Parameter 
    var parameter = this.Info.ToParameter();

    //Sign Message-Data
    var signer = new MLDsaSigner(parameter, true);
    var privkey = MLDsaPrivateKeyParameters
      .FromEncoding(parameter, this.Info.ToPrivKey().ToBytes());
    signer.Init(true, privkey);
    signer.BlockUpdate(msg, 0, msg.Length);

    return (signer.GenerateSignature(), this.PubKey);
  }

  public static bool Verify(
    ReadOnlySpan<byte> pubkeyalice,
    ReadOnlySpan<byte> signature,
    ReadOnlySpan<byte> message,
    MLDsaParameters parameter)
  {
    var msg = message.ToArray();

    //Verify Signature
    var signer = new MLDsaSigner(parameter, true);
    var pubkey = MLDsaPublicKeyParameters.FromEncoding(
      parameter, pubkeyalice.ToArray());
    signer.Init(false, pubkey);
    signer.BlockUpdate(msg, 0, msg.Length);
    return signer.VerifySignature(signature.ToArray());
  }

  private static MLDsaParameters ToRngParameter()
  {
    var rand = new SecureRandom();
    var parameters = BcPqcServices.ToMLDsaParameters();
    var idx = rand.Next(parameters.Length);
    return parameters[idx];
  }

  public static MLDsaPublicKeyParameters ToPubKey(
    byte[] pubkey, MLDsaParameters parameter) =>
      MLDsaPublicKeyParameters.FromEncoding(parameter, pubkey);


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

  ~BobMLDSA() => Dispose(false);

  public void Dispose()
  {
    Dispose(true);
    GC.SuppressFinalize(this);
  }
}