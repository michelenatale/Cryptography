

using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Crypto.Kems;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;

namespace michele.natale.TestBcPqcs;

using Pointers;
using Services;

public class BobMLKEM:IDisposable
{
  public byte[] PubKey { get; private set; } = null!;
  public bool IsDisposed { get; private set; } = true;
  public byte[] CapsulationKey { get; private set; } = null!;
  public MLKemParameters Parameter { get; private set; } = null!;
  public byte[] Associated { get; private set; } =
    "© michele natale 2025"u8.ToArray();

  private SecureRandom Rand = new();
  private AsymmetricCipherKeyPair KeyPair = null!;
  private UsIPtr<byte> SharedKey = UsIPtr<byte>.Empty;

  public BobMLKEM()
    : this(ToRngParameter())
  {
  }

  public BobMLKEM(MLKemParameters parameter)
  { 
    this.Parameter = parameter;

    var generator = new MLKemKeyGenerationParameters(
      this.Rand, this.Parameter);

    var keypair_generator = new MLKemKeyPairGenerator();
    keypair_generator.Init(generator);

    this.KeyPair = keypair_generator.GenerateKeyPair();
    this.PubKey = ((MLKemPublicKeyParameters)this.KeyPair.Public).GetEncoded();

    this.IsDisposed = false;
  }

  public void Clear()
  {
    if (this.IsDisposed) return;

    Array.Clear(this.PubKey);
    Array.Clear(this.Associated);
    Array.Clear(this.CapsulationKey);

    this.SharedKey.Dispose();

    this.Rand = null!;
    this.PubKey = null!;
    this.KeyPair = null!;
    this.Parameter = null!;
    this.CapsulationKey = null!;
    this.SharedKey = UsIPtr<byte>.Empty;
    this.Associated = "© michele natale 2025"u8.ToArray();
  }

  public MLKemPublicKeyParameters ToPubKey() =>
    MLKemPublicKeyParameters.FromEncoding(this.Parameter, this.PubKey);

  public byte[] Encryption(
    ReadOnlySpan<byte> bytes,
    ReadOnlySpan<byte> capsulationkey,
    ReadOnlySpan<byte> associated,
    CryptionAlgorithm cryptoalgo)
  { 
    var associat = associated.IsEmpty ? this.Associated : associated;
    var sharedkey = this.ToSharedKey(capsulationkey);
     
    return BcPqcServices.EncryptionWithCryptionAlgo(
      bytes, sharedkey, associat, cryptoalgo);
  }

  public byte[] Decryption(
    ReadOnlySpan<byte> bytes,
    ReadOnlySpan<byte> associated,
    CryptionAlgorithm cryptoalgo)
  {
    var associat = associated.IsEmpty ? this.Associated : associated; 

    return BcPqcServices.DecryptionWithCryptionAlgo(
      bytes, this.SharedKey, associat, cryptoalgo);
  }

  public UsIPtr<byte> ToSharedKey(ReadOnlySpan<byte> capsulationkey)
  {
    //SharedKeys are always secret.

    // Bob decapsulates a new shared secret using Bob's private key
    var decapsulator = new MLKemDecapsulator(this.Parameter);
    decapsulator.Init(this.KeyPair.Private);

    var sharedkey = new byte[decapsulator.SecretLength];
    decapsulator.Decapsulate(capsulationkey.ToArray(), 0, capsulationkey.Length, sharedkey, 0, sharedkey.Length);

    return new UsIPtr<byte>(sharedkey);
  }

  public byte[] GenerateSharedKey(ReadOnlySpan<byte> alice_pubkey)
  {
    //SharedKeys are always secret, whereas the CapsulationKey is always public.

    if (this.SharedKey is not null && !this.SharedKey.IsDisposed)
      this.ClearSharedKey();

    // Bob encapsulates a new shared secret using alice's public key
    var encapsulator = new MLKemEncapsulator(this.Parameter);
    encapsulator.Init(new ParametersWithRandom(
      ToPubKey(alice_pubkey.ToArray(), this.Parameter), this.Rand));

    var capsulationkey = new byte[encapsulator.EncapsulationLength];
    var skey = new byte[encapsulator.SecretLength];
    encapsulator.Encapsulate(capsulationkey, 0, capsulationkey.Length, skey, 0, skey.Length);

    this.SharedKey = new UsIPtr<byte>(skey);
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

  private static MLKemParameters ToRngParameter()
  {
    var rand = new SecureRandom();
    var parameters = BcPqcServices.ToMLKemParameters();
    var idx = rand.Next(parameters.Length);
    return parameters[idx];
  }

  public static MLKemPublicKeyParameters ToPubKey(
    byte[] pubkey, MLKemParameters parameter) =>
      MLKemPublicKeyParameters.FromEncoding(parameter, pubkey);


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
