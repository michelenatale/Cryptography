
using Org.BouncyCastle.Pqc.Crypto.Lms;
using Org.BouncyCastle.Security;

namespace michele.natale.TestBcPqcs;

using BcPqcs;
using Services;

public class AliceLMS : IDisposable
{
  public byte[] PubKey { get; private set; } = null!;
  public bool IsDisposed { get; private set; } = true;
  public LmsKeyPairInfo Info { get; private set; } = null!;
  public LmsParam Parameter
  {
    get; private set;
  } = LmsParam.lms_sha256_h5_w1!;

  private SecureRandom Rand = new();


  public AliceLMS()
    : this(ToRngParameter())
  {
  }

  public AliceLMS(LmsParam parameter)
  {
    this.Parameter = parameter;

    var generator = BcPqcServices.ToLmsKeyGenerationParameter(parameter, this.Rand);

    var keypair_generator = new LmsKeyPairGenerator();
    keypair_generator.Init(generator);

    var keypair = keypair_generator.GenerateKeyPair();
    this.PubKey = ((LmsPublicKeyParameters)keypair.Public).GetEncoded();
    var privkey = ((LmsPrivateKeyParameters)keypair.Private).GetEncoded();
    this.Info = new LmsKeyPairInfo(this.PubKey, privkey, parameter);

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
    this.Parameter = LmsParam.lms_sha256_h5_w1;
  }

  public LmsPublicKeyParameters ToPubKey() =>
    LmsPublicKeyParameters.GetInstance(this.PubKey);


  public (byte[] Sign, byte[] PubKey) Sign(ReadOnlySpan<byte> message)
  {
    //Sign Message-Data
    var signer = new LmsSigner();
    var privkey = LmsPrivateKeyParameters
      .GetInstance(this.Info.ToPrivKey().ToBytes());
    signer.Init(true, privkey);

    return (signer.GenerateSignature(message.ToArray()), this.PubKey);
  }

  public static bool Verify(
    ReadOnlySpan<byte> pubkeybob,
    ReadOnlySpan<byte> signature,
    ReadOnlySpan<byte> message)
  {
    //Verify Signature
    var signer = new LmsSigner();
    var pubkey = LmsPublicKeyParameters
      .GetInstance(pubkeybob.ToArray());
    signer.Init(false, pubkey);
    return signer.VerifySignature(message.ToArray(), signature.ToArray());
  }

  private static LmsParam ToRngParameter()
  {
    var rand = new SecureRandom();
    var parameters = BcPqcServices.ToLmsParam();
    var idx = rand.Next(parameters.Length);
    return parameters[idx];
  }

  public static LmsPublicKeyParameters ToPubKey(byte[] pubkey) =>
      LmsPublicKeyParameters.GetInstance(pubkey);

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

  ~AliceLMS() => Dispose(false);

  public void Dispose()
  {
    Dispose(true);
    GC.SuppressFinalize(this);
  }
}