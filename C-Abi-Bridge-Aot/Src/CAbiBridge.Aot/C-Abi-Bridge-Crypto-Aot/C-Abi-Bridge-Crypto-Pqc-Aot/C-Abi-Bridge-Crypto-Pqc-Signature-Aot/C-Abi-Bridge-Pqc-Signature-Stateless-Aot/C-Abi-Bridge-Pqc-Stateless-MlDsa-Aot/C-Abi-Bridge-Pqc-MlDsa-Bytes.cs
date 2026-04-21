//ML-DSA
//Module-Lattice-Based
//FIPS PUB 204 
//https://nvlpubs.nist.gov/nistpubs/fips/nist.fips.204.pdf


using System.Text;
using michele.natale.Pointers;
using System.Runtime.InteropServices;


namespace michele.natale.CAbiBridge;

using static NetServicesCrypto;
using static NetServicesUtils;

partial class CryptoBridge
{

  [UnmanagedCallersOnly(EntryPoint = "create_mldsa_key_pair_aot")]
  public unsafe static CError CreateMlDsaKeyPairAot(
    byte** priv_key_ptr, int* priv_key_length,
    byte** pub_key_ptr, int* pub_key_length,
    byte** guid_id_ptr, int* guid_id_length,
    byte* mldsa_param)
  {
    *priv_key_length = 0; *pub_key_length = 0; *guid_id_length = 0;
    *priv_key_ptr = null; *pub_key_ptr = null; *guid_id_ptr = null;

    try
    {
      using var mldsainfo = CreateMlDsaKeyPair();

      var pub = mldsainfo.PubKey;
      var id = mldsainfo.Id.ToByteArray();
      var priv = mldsainfo.ToPrivKey().ToBytes();
      var mldsaalgo = (byte)FromMLDsaAlgorithm(mldsainfo.ToAlgo());

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

      *mldsa_param = mldsaalgo;

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

  [UnmanagedCallersOnly(EntryPoint = "create_mldsa_key_pair_param_aot")]
  public unsafe static CError CreateMlDsaKeyPairParamAot(
    byte mldsa_param,
    byte** priv_key_ptr, int* priv_key_length,
    byte** pub_key_ptr, int* pub_key_length,
    byte** guid_id_ptr, int* guid_id_length)
  {
    *priv_key_length = 0; *pub_key_length = 0; *guid_id_length = 0;
    *priv_key_ptr = null; *pub_key_ptr = null; *guid_id_ptr = null;

    try
    {
      using var mldsainfo = CreateMlDsaKeyPair(
        (MLDsaParam)mldsa_param);

      var pub = mldsainfo.PubKey;
      var id = mldsainfo.Id.ToByteArray();
      var priv = mldsainfo.ToPrivKey().ToBytes();

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

  [UnmanagedCallersOnly(EntryPoint = "save_pqc_mldsa_key_pair_aot")]
  public unsafe static CError SavePqcMlDsaKeyPairAot(
    byte* src_ptr, int src_length,
    byte* priv_key_ptr, int priv_key_length,
    byte* pub_key_ptr, int pub_key_length,
    byte* guid_id_ptr, int guid_id_length,
    byte mldsa_param, bool save_private_key)
  {
    try
    {
      var keypairfile = Encoding.UTF8.GetString(
        ToSpanSafe(src_ptr, src_length));
      var priv = ToSpanSafe(priv_key_ptr, priv_key_length);
      var pub = ToSpanSafe(pub_key_ptr, pub_key_length);
      var guid = ToSpanSafe(guid_id_ptr, guid_id_length);

      var mldsaalgo = ToMLDsaAlgorithm((MLDsaParam)mldsa_param);

      SaveMlDsaKeyPair(keypairfile, priv, pub,
        guid, mldsaalgo, save_private_key);

      return new CError { error_code = (int)CErrorCode.Ok };
    }
    catch (Exception ex)
    {
      return CreateError(CErrorCode.UnknownError, ex.Message);
    }
  }


  [UnmanagedCallersOnly(EntryPoint = "load_pqc_mldsa_key_pair_aot")]
  public unsafe static CError LoadPqcMlDsaKeyPairAot(
    byte* src_ptr, int src_length,
    byte** priv_key_ptr, int* priv_key_length,
    byte** pub_key_ptr, int* pub_key_length,
    byte** guid_id_ptr, int* guid_id_length,
    byte* mldsa_param)
  {
    *priv_key_length = 0; *pub_key_length = 0; *guid_id_length = 0;
    *priv_key_ptr = null; *pub_key_ptr = null; *guid_id_ptr = null;

    try
    {
      var keypairfile = Encoding.UTF8.GetString(
        ToSpanSafe(src_ptr, src_length));

      using var mldsainfo = LoadMlDsaKeyPair(keypairfile);

      using var priv_ptr = mldsainfo.ToPrivKey();
      var priv = priv_ptr.ToBytes();

      var id = mldsainfo.Id.ToByteArray();
      var pub = mldsainfo.PubKey.ToArray();
      var mldsaalgo = (byte)FromMLDsaAlgorithm(mldsainfo.ToAlgo());

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

      *mldsa_param = mldsaalgo;

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

  [UnmanagedCallersOnly(EntryPoint = "pqc_mldsa_sign_aot")]
  public unsafe static CError PqcMlDsaSignAot(
    byte* message_ptr, int message_length,
    byte* private_key_ptr, int private_key_length,
    byte mldsa_param, byte** sign_ptr, int* sign_length)
  {
    *sign_ptr = null; *sign_length = 0;

    try
    {
      var param = (MLDsaParam)mldsa_param;
      var message = ToSpanSafe(message_ptr, message_length);
      var privkey = new UsIPtr<byte>(ToSpanSafe(private_key_ptr, private_key_length));

      //The first value in the buffer or signature is reserved for the MLDSA algorithm.
      var sign = Sign(message, privkey, ToMLDsaAlgorithm(param));

      var buffer = (byte*)NativeMemory.Alloc((nuint)(sign.Length + 1));
      sign.CopyTo(new Span<byte>(buffer, sign.Length + 1).Slice(1));
      *buffer = mldsa_param; *sign_ptr = buffer; *sign_length = sign.Length + 1;
      Array.Clear(sign);

      return new CError { error_code = (int)CErrorCode.Ok };
    }
    catch (Exception ex)
    {
      CheckSetZero((nint*)*sign_ptr); *sign_length = 0;
      return CreateError(CErrorCode.UnknownError, ex.Message);
    }
  }

  [UnmanagedCallersOnly(EntryPoint = "pqc_mldsa_verify_aot")]
  [return: MarshalAs(UnmanagedType.U1)]
  public unsafe static bool PqcMlDsaVerifyAot(
    byte* message_ptr, int message_length,
    byte* public_key_ptr, int public_key_length,
    byte* signature_ptr, int signature_length,
    CError* cerror)
  {
    *cerror = new CError { error_code = -1, message = IntPtr.Zero };

    try
    {
      var message = ToSpanSafe(message_ptr, message_length);
      var param = ToMLDsaAlgorithm((MLDsaParam)(*signature_ptr));
      var pubkey = ToSpanSafe(public_key_ptr, public_key_length);
      var signature = ToSpanSafe(signature_ptr + 1, signature_length - 1);

      var verify = Verify(message, pubkey, signature, param);
      if (verify)
      {
        *cerror = new CError { error_code = (int)CErrorCode.Ok };
        return verify;
      }

      var exc_str = "Verification has failed!";
      throw new NotImplementedException(
        $"{nameof(PqcMlDsaVerifyAot)}:\nverify = {verify}; {exc_str}");
    }
    catch (Exception ex)
    {
      *cerror = CreateError(CErrorCode.CryptoError, ex.Message);
      return false;
    }
  }

}

