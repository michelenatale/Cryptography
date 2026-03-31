
using System.Text;
using System.Numerics;
using System.Runtime.InteropServices;

namespace michele.natale.CAbiBridge;

using Pointers;
using static NetServicesUtils;
using static NetServicesCrypto;

partial class CryptoBridge
{

  [UnmanagedCallersOnly(EntryPoint = "create_mlkem_key_pair_aot")]
  public unsafe static CError CreateMlKemKeyPairAot(
    byte** priv_key_ptr, int* priv_key_length,
    byte** pub_key_ptr, int* pub_key_length,
    byte** guid_id_ptr, int* guid_id_length,
    byte* mlkem_param, byte* crypto_algo)
  {
    *priv_key_length = 0; *pub_key_length = 0; *guid_id_length = 0;
    *priv_key_ptr = null; *pub_key_ptr = null; *guid_id_ptr = null;

    try
    {
      using var mlkeminfo = CreateMlKemKeyPair();

      var pub = mlkeminfo.PubKey;
      var id = mlkeminfo.Id.ToByteArray();
      var cryptalgo = (byte)mlkeminfo.CryptAlgo;
      var priv = mlkeminfo.ToPrivKey().ToBytes();
      var mlkemalgo = (byte)FromMLKemAlgorithm(mlkeminfo.ToAlgo());

      var buffer = (byte*)NativeMemory.Alloc((nuint)pub.Length);
      pub.CopyTo(new Span<byte>(buffer, pub.Length));
      *pub_key_ptr = buffer; *pub_key_length = pub.Length;
      Array.Clear(pub);

      buffer = (byte*)NativeMemory.Alloc((nuint)priv.Length);
      priv.CopyTo(new Span<byte>(buffer, priv.Length));
      *priv_key_ptr = buffer; *priv_key_length = priv.Length;
      Array.Clear(priv);

      buffer = (byte*)NativeMemory.Alloc((nuint)id.Length);
      id.CopyTo(new Span<byte>(buffer, id.Length));
      *guid_id_ptr = buffer; *guid_id_length = id.Length;
      Array.Clear(id);

      *mlkem_param = mlkemalgo; *crypto_algo = cryptalgo;

      return new CError { error_code = (int)CErrorCode.Ok };
    }
    catch (Exception ex)
    {
      CheckSetZero((nint*)*guid_id_ptr);
      CheckSetZero((nint*)*pub_key_ptr); CheckSetZero((nint*)*priv_key_ptr);
      *priv_key_length = 0; *pub_key_length = 0; *guid_id_length = 0;

      return CreateError(CErrorCode.UnknownError, ex.Message);
    }
  }

  [UnmanagedCallersOnly(EntryPoint = "create_mlkem_key_pair_param_aot")]
  public unsafe static CError CreateMlKemKeyPairParamAot(
    byte mlkem_param, byte crypto_algo,
    byte** priv_key_ptr, int* priv_key_length,
    byte** pub_key_ptr, int* pub_key_length,
    byte** guid_id_ptr, int* guid_id_length)
  {
    *priv_key_length = 0; *pub_key_length = 0; *guid_id_length = 0;
    *priv_key_ptr = null; *pub_key_ptr = null; *guid_id_ptr = null;

    try
    {
      using var mlkeminfo = CreateMlKemKeyPair(
        (MLKemParam)mlkem_param, (CryptionAlgorithm)crypto_algo);

      var pub = mlkeminfo.PubKey;
      var id = mlkeminfo.Id.ToByteArray();
      var priv = mlkeminfo.ToPrivKey().ToBytes();

      var buffer = (byte*)NativeMemory.Alloc((nuint)pub.Length);
      pub.CopyTo(new Span<byte>(buffer, pub.Length));
      *pub_key_ptr = buffer; *pub_key_length = pub.Length;
      Array.Clear(pub);

      buffer = (byte*)NativeMemory.Alloc((nuint)priv.Length);
      priv.CopyTo(new Span<byte>(buffer, priv.Length));
      *priv_key_ptr = buffer; *priv_key_length = priv.Length;
      Array.Clear(priv);

      buffer = (byte*)NativeMemory.Alloc((nuint)id.Length);
      id.CopyTo(new Span<byte>(buffer, id.Length));
      *guid_id_ptr = buffer; *guid_id_length = id.Length;
      Array.Clear(id);

      return new CError { error_code = (int)CErrorCode.Ok };
    }
    catch (Exception ex)
    {
      CheckSetZero((nint*)*guid_id_ptr);
      CheckSetZero((nint*)*pub_key_ptr); CheckSetZero((nint*)*priv_key_ptr);
      *priv_key_length = 0; *pub_key_length = 0; *guid_id_length = 0;

      return CreateError(CErrorCode.UnknownError, ex.Message);
    }
  }

  [UnmanagedCallersOnly(EntryPoint = "save_pqc_mlkem_key_pair_aot")]
  public unsafe static CError SavePqcMlKemKeyPairAot(
    byte* src_ptr, int src_length,
    byte* priv_key_ptr, int priv_key_length,
    byte* pub_key_ptr, int pub_key_length,
    byte* guid_id_ptr, int guid_id_length,
    byte mlkem_param, byte crypto_algo,
    bool save_private_key)
  {
    try
    {
      var keypairfile = Encoding.UTF8.GetString(
        ToSpanSafe(src_ptr, src_length));
      var priv = ToSpanSafe(priv_key_ptr, priv_key_length);
      var pub = ToSpanSafe(pub_key_ptr, pub_key_length);
      var guid = ToSpanSafe(guid_id_ptr, guid_id_length);

      var mlkemalgo = ToMLKemAlgorithm(mlkem_param);
      var cryptoalgo = (CryptionAlgorithm)crypto_algo;

      SaveMlKemKeyPair(keypairfile, priv, pub,
        guid, mlkemalgo, cryptoalgo, save_private_key);

      return new CError { error_code = (int)CErrorCode.Ok };
    }
    catch (Exception ex)
    {
      return CreateError(CErrorCode.UnknownError, ex.Message);
    }
  }


  [UnmanagedCallersOnly(EntryPoint = "load_pqc_mlkem_key_pair_aot")]
  public unsafe static CError LoadPqcMlKemKeyPairAot(
    byte* src_ptr, int src_length,
    byte** priv_key_ptr, int* priv_key_length,
    byte** pub_key_ptr, int* pub_key_length,
    byte** guid_id_ptr, int* guid_id_length,
    byte* mlkem_param, byte* crypto_algo)
  {
    *priv_key_length = 0; *pub_key_length = 0; *guid_id_length = 0;
    *priv_key_ptr = null; *pub_key_ptr = null; *guid_id_ptr = null;

    try
    {
      var keypairfile = Encoding.UTF8.GetString(
        ToSpanSafe(src_ptr, src_length));

      using var mlkeminfo = LoadMlKemKeyPair(keypairfile);

      using var priv_ptr = mlkeminfo.ToPrivKey();
      var priv = priv_ptr.ToBytes();

      var id = mlkeminfo.Id.ToByteArray();
      var pub = mlkeminfo.PubKey.ToArray();
      var cryptalgo = (byte)mlkeminfo.CryptAlgo;
      var mlkemalgo = (byte)FromMLKemAlgorithm(mlkeminfo.ToAlgo());

      var buffer = (byte*)NativeMemory.Alloc((nuint)pub.Length);
      pub.CopyTo(new Span<byte>(buffer, pub.Length));
      *pub_key_ptr = buffer; *pub_key_length = pub.Length;
      Array.Clear(pub);

      buffer = (byte*)NativeMemory.Alloc((nuint)priv.Length);
      priv.CopyTo(new Span<byte>(buffer, priv.Length));
      *priv_key_ptr = buffer; *priv_key_length = priv.Length;
      Array.Clear(priv);

      buffer = (byte*)NativeMemory.Alloc((nuint)id.Length);
      id.CopyTo(new Span<byte>(buffer, id.Length));
      *guid_id_ptr = buffer; *guid_id_length = id.Length;
      Array.Clear(id);

      *mlkem_param = mlkemalgo; *crypto_algo = cryptalgo;

      return new CError { error_code = (int)CErrorCode.Ok };
    }
    catch (Exception ex)
    {
      CheckSetZero((nint*)*pub_key_ptr);
      CheckSetZero((nint*)*priv_key_ptr); CheckSetZero((nint*)*guid_id_ptr);
      *priv_key_length = 0; *pub_key_length = 0; *guid_id_length = 0;
      return CreateError(CErrorCode.UnknownError, ex.Message);
    }
  }

  [UnmanagedCallersOnly(EntryPoint = "to_pqc_mlkem_capsulation_from_pub_key_aot")]
  public unsafe static CError ToPqcMlKemCapsulationFromPubKeyAot(
    byte* alice_public_key_ptr, int alice_public_key_length,
    byte mlkem_param,
    byte** shared_key_ptr, int* shared_key_length,
    byte** capsulation_ptr, int* capsulation_length)
  {
    //Bob bekommt von Alice ihren 'Publickey' sowie den
    //'MlKem Parameter' und lässt sich so seinen 'SharedKey'
    //und den 'Capsulation' generieren.

    //Bob receives Alice's ''Publickey' and
    //'MlKem Parameter' and uses them to generate
    //his 'SharedKey' and the 'Capsulation'.

    *shared_key_ptr = null; *capsulation_ptr = null;
    *shared_key_length = 0; *capsulation_length = 0;

    try
    {
      var param = ToMLKemAlgorithm(mlkem_param);
      var alice_pub_key = ToSpanSafe(
        alice_public_key_ptr, alice_public_key_length);
      var (sharedkey, capsulation) =
        ToPqcMlKemCapsulationFromPubKey(alice_pub_key, param);

      using var shared = sharedkey;
      var shared_key = shared.ToBytes();

      var buffer = (byte*)NativeMemory.Alloc((nuint)shared_key.Length);
      shared_key.CopyTo(new Span<byte>(buffer, shared_key.Length));
      *shared_key_ptr = buffer; *shared_key_length = shared_key.Length;
      Array.Clear(shared_key);

      buffer = (byte*)NativeMemory.Alloc((nuint)capsulation.Length);
      capsulation.CopyTo(new Span<byte>(buffer, capsulation.Length));
      *capsulation_ptr = buffer; *capsulation_length = capsulation.Length;
      Array.Clear(capsulation);

      return new CError { error_code = (int)CErrorCode.Ok };
    }
    catch (Exception ex)
    {
      *shared_key_length = 0; *capsulation_length = 0;
      CheckSetZero((nint*)*shared_key_ptr); CheckSetZero((nint*)*capsulation_ptr);
      return CreateError(CErrorCode.UnknownError, ex.Message);
    }
  }


  [UnmanagedCallersOnly(EntryPoint = "to_pqc_mlkem_shared_key_from_private_key_aot")]
  public unsafe static CError ToPqcMlKemSharedKeyFromPrivateKeyAot(
    byte* alice_private_key_ptr, int alice_private_key_length,
    byte* bob_capsulation_ptr, int bob_capsulation_length,
    byte mlkem_param, byte** shared_ptr, int* shared_length)
  {
    //Sofern Alice nur den 'SharedKey' wünscht, zb. für ein eigenes
    //Verkryptungsalgorithmus, so lässt sich das problemlos über
    //den 'PrivateKey' von Alice generieren.

    //If Alice only wants the 'SharedKey' — for example,
    //for her own encryption algorithm — it can be easily
    //generated using Alice's 'PrivateKey'.

    *shared_ptr = null; *shared_length = 0;

    try
    {
      var param = ToMLKemAlgorithm(mlkem_param);
      var alice_privkey = ToSpanSafe(alice_private_key_ptr, alice_private_key_length);
      var capsulation = ToSpanSafe(bob_capsulation_ptr, bob_capsulation_length);

      using var sharedkey = ToPqcMlKemSharedKeyFromPrivateKey(
        capsulation, alice_privkey, param);

      var shared = sharedkey.ToBytes();
      var buffer = (byte*)NativeMemory.Alloc((nuint)shared.Length);
      shared.CopyTo(new Span<byte>(buffer, shared.Length));
      *shared_ptr = buffer; *shared_length = shared.Length;
      Array.Clear(shared);

      return new CError { error_code = (int)CErrorCode.Ok };
    }
    catch (Exception ex)
    {
      *shared_length = 0;
      CheckSetZero((nint*)*shared_ptr);
      return CreateError(CErrorCode.UnknownError, ex.Message);
    }
  }


  [UnmanagedCallersOnly(EntryPoint = "pqc_mlkem_encryption_aot")]
  public unsafe static CError PqcMlKemEncryptionAot(
    byte* message_ptr, int message_length,
    byte* private_key_ptr, int private_key_length,
    byte* capsulation_ptr, int capsulation_length,
    byte* associated_ptr, int associated_length,
    byte mlkem_param, byte crypto_algo,
    byte** cipher_ptr, int* cipher_length)
  {
    //Nachdem Alice von Bob den Capsulation bekommen hat,
    //kann Sie mit ihrem PrivateKey eine Pqc-MLKEM-Verkryptung
    //machen. Die 'associated' darf als einziger Parameter 'null' ein.

    //After Alice receives the Capsulation from Bob, she can
    //use her PrivateKey to perform a Pqc-MLKEM encryption.
    //The 'associated' parameter may be set to 'null'.

    *cipher_ptr = null; *cipher_length = 0;

    try
    {
      var param = ToMLKemAlgorithm(mlkem_param);
      var message = ToSpanSafe(message_ptr, message_length);
      var associated = ToSpanSafe(associated_ptr, associated_length);
      var capsulation = ToSpanSafe(capsulation_ptr, capsulation_length);
      var privkey = new UsIPtr<byte>(ToSpanSafe(private_key_ptr, private_key_length));

      var cipher = PqcMlKemEncryption(
        message, capsulation, privkey, associated,
        param, (CryptionAlgorithm)crypto_algo);

      var buffer = (byte*)NativeMemory.Alloc((nuint)cipher.Length);
      cipher.CopyTo(new Span<byte>(buffer, cipher.Length));
      *cipher_ptr = buffer; *cipher_length = cipher.Length;
      Array.Clear(cipher);

      return new CError { error_code = (int)CErrorCode.Ok };
    }
    catch (Exception ex)
    {
      *cipher_length = 0;
      CheckSetZero((nint*)*cipher_ptr);
      return CreateError(CErrorCode.UnknownError, ex.Message);
    }
  }


  [UnmanagedCallersOnly(EntryPoint = "pqc_mlkem_decryption_aot")]
  public unsafe static CError PqcMlKemDecryptionAot(
    byte* cipher_ptr, int cipher_length,
    byte* shared_key_ptr, int shared_key_length,
    byte* associated_ptr, int associated_length,
    byte mlkem_param, byte crypto_algo,
    byte** decipher_ptr, int* decipher_length)
  {
    //Bob kann nun den Ciphertext problemlos mit dem vorher
    //extrahierten 'sharedkey' entkrypten, und den ursprünglichen
    //plaintext von Alice lesen. Die 'associated' darf als
    //einziger Parameter 'null' ein.

    //Bob can now easily decrypt the ciphertext using the
    //previously extracted 'sharedkey' and read the original
    //plaintext from Alice. The 'associated' parameter may
    //be set to 'null'.


    *decipher_ptr = null; *decipher_length = 0;

    try
    {
      var param = ToMLKemAlgorithm(mlkem_param);
      var cipher = ToSpanSafe(cipher_ptr, cipher_length);
      var associated = ToSpanSafe(associated_ptr, associated_length);
      var sharedkey = new UsIPtr<byte>(ToSpanSafe(shared_key_ptr, shared_key_length));

      var decipher = PqcMlKemDecryption(
        cipher, sharedkey, associated, param, (CryptionAlgorithm)crypto_algo);

      var buffer = (byte*)NativeMemory.Alloc((nuint)decipher.Length);
      decipher.CopyTo(new Span<byte>(buffer, decipher.Length));
      *decipher_ptr = buffer; *decipher_length = decipher.Length;
      Array.Clear(decipher);

      return new CError { error_code = (int)CErrorCode.Ok };
    }
    catch (Exception ex)
    {
      *decipher_length = 0;
      CheckSetZero((nint*)*decipher_ptr);
      return CreateError(CErrorCode.UnknownError, ex.Message);
    }
  }

  private static bool IsNullOrEmpty<T>(ReadOnlySpan<T> bytes)
    where T : INumber<T> =>
      bytes.IsEmpty || bytes.Length == 0;

  private static bool IsNullOrEmpty<T>(ReadOnlyMemory<T> bytes)
  where T : INumber<T> =>
    bytes.IsEmpty || bytes.Length == 0;

  //private unsafe static ReadOnlySpan<byte> ToReadOnlySpan(
  //  byte* bytes_ptr, int bytes_length) =>
  //    new(bytes_ptr, bytes_length);

  //private unsafe static byte[] ToReadOnlySpanManaged(
  //  byte* bytes_ptr, int bytes_length) =>
  //    new ReadOnlySpan<byte>(bytes_ptr, bytes_length).ToArray();

}
