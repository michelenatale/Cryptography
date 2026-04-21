//ML-DSA
//Module-Lattice-Based
//FIPS PUB 204 
//https://nvlpubs.nist.gov/nistpubs/fips/nist.fips.204.pdf

using System.Runtime.InteropServices;

namespace michele.natale.CAbiBridge;

using static NetServicesUtils;
using static NetServicesCrypto;

partial class CryptoBridge
{

  [UnmanagedCallersOnly(EntryPoint = "pqc_mldsa_multi_sign_aot")]
  public unsafe static CError PqcMlDsaMultiSignAot(
    byte* message_ptr, int message_length,

    byte** guid_ptr, int* guid_length, int guid_count,
    byte** sign_ptr, int* sign_length, int sign_count,
    byte** public_key_ptr, int* public_key_length, int public_key_count,
    byte** signer_name_ptr, int* signer_name_length, int signer_name_count,
    byte** project_name_ptr, int* project_name_length, int project_name_count,

    byte* sign_algo_ptr, int sign_algo_count,
    byte* mldsa_param_ptr, int mldsa_param_count,

    byte** multi_sign_ptr, int* multi_sign_length,
    byte** multi_private_key_ptr, int* multi_private_key_length,
    byte** multi_public_key_ptr, int* multi_public_key_length)
  {
    *multi_sign_ptr = null; *multi_sign_length = 0;
    *multi_public_key_ptr = null; *multi_public_key_length = 0;
    *multi_private_key_ptr = null; *multi_private_key_length = 0;

    try
    {
      var message = ToBytesSafe(message_ptr, message_length);

      var guids = ToBytesSafe(guid_ptr, guid_length, guid_count);
      var signs = ToBytesSafe(sign_ptr, sign_length, sign_count);
      var publickeys = ToBytesSafe(public_key_ptr, public_key_length, public_key_count);
      var signersnames = ToBytesSafe(signer_name_ptr, signer_name_length, signer_name_count);
      var projectnames = ToBytesSafe(project_name_ptr, project_name_length, project_name_count);

      var sign_algo = ToBytesSafe(sign_algo_ptr, sign_algo_count);
      var mldsa_param = ToBytesSafe(mldsa_param_ptr, mldsa_param_count);

      var mult_sign_data = MultiSignatur(message, guids, signs, publickeys,
        signersnames, projectnames, sign_algo, mldsa_param);

      var (signatur, pk, pubkey, param) = mult_sign_data;
      using var priv_key = pk;

      //The 'multi-mldsa-param' is included first in the signature.
      var buffer = (byte*)NativeMemory.Alloc((nuint)(signatur.Length + 1));
      signatur.CopyTo(new Span<byte>(buffer, signatur.Length + 1).Slice(1));
      *buffer = (byte)FromMLDsaAlgorithm(param); // 'multi-mldsa-param'
      *multi_sign_ptr = buffer; *multi_sign_length = signatur.Length + 1;
      Array.Clear(signatur);

      var privkey = priv_key.Target;
      buffer = (byte*)NativeMemory.Alloc((nuint)privkey.Length);
      privkey.CopyTo(new Span<byte>(buffer, privkey.Length));
      *multi_private_key_ptr = buffer; *multi_private_key_length = privkey.Length;

      buffer = (byte*)NativeMemory.Alloc((nuint)pubkey.Length);
      pubkey.CopyTo(new Span<byte>(buffer, pubkey.Length));
      *multi_public_key_ptr = buffer; *multi_public_key_length = pubkey.Length;
      Array.Clear(pubkey);

      return new CError { error_code = (int)CErrorCode.Ok };
    }
    catch (Exception ex)
    {
      CheckSetZero((nint*)*multi_sign_ptr); *multi_sign_length = 0;
      CheckSetZero((nint*)*multi_public_key_ptr); *multi_public_key_length = 0;
      CheckSetZero((nint*)*multi_private_key_ptr); *multi_private_key_length = 0;
      return CreateError(CErrorCode.UnknownError, ex.Message);
    }
  }

  [UnmanagedCallersOnly(EntryPoint = "pqc_mldsa_multi_sign_file_aot")]
  public unsafe static CError PqcMlDsaMultiSignFileAot(
    byte* src_file_ptr, int src_file_length,

    byte** guid_ptr, int* guid_length, int guid_count,
    byte** sign_ptr, int* sign_length, int sign_count,
    byte** public_key_ptr, int* public_key_length, int public_key_count,
    byte** signer_name_ptr, int* signer_name_length, int signer_name_count,
    byte** project_name_ptr, int* project_name_length, int project_name_count,

    byte* sign_algo_ptr, int sign_algo_count,
    byte* mldsa_param_ptr, int mldsa_param_count,

    byte** multi_sign_ptr, int* multi_sign_length,
    byte** multi_private_key_ptr, int* multi_private_key_length,
    byte** multi_public_key_ptr, int* multi_public_key_length)
  {
    *multi_sign_ptr = null; *multi_sign_length = 0;
    *multi_public_key_ptr = null; *multi_public_key_length = 0;
    *multi_private_key_ptr = null; *multi_private_key_length = 0;

    try
    {
      var filename = ToStringUtf8Safe(src_file_ptr, src_file_length);

      var guids = ToBytesSafe(guid_ptr, guid_length, guid_count);
      var signs = ToBytesSafe(sign_ptr, sign_length, sign_count);
      var publickeys = ToBytesSafe(public_key_ptr, public_key_length, public_key_count);
      var signersnames = ToBytesSafe(signer_name_ptr, signer_name_length, signer_name_count);
      var projectnames = ToBytesSafe(project_name_ptr, project_name_length, project_name_count);

      var sign_algo = ToBytesSafe(sign_algo_ptr, sign_algo_count);
      var mldsa_param = ToBytesSafe(mldsa_param_ptr, mldsa_param_count);

      var mult_sign_data = MultiSignaturFileAsync(filename, guids, signs, publickeys,
        signersnames, projectnames, sign_algo, mldsa_param)
        .GetAwaiter().GetResult();

      var (signatur, pk, pubkey, param) = mult_sign_data;
      using var priv_key = pk;

      //The 'multi-mldsa-param' is included first in the signature.
      var buffer = (byte*)NativeMemory.Alloc((nuint)(signatur.Length + 1));
      signatur.CopyTo(new Span<byte>(buffer, signatur.Length + 1).Slice(1));
      *buffer = (byte)FromMLDsaAlgorithm(param); // 'multi-mldsa-param'
      *multi_sign_ptr = buffer; *multi_sign_length = signatur.Length + 1;
      Array.Clear(signatur);

      var privkey = priv_key.Target;
      buffer = (byte*)NativeMemory.Alloc((nuint)privkey.Length);
      privkey.CopyTo(new Span<byte>(buffer, privkey.Length));
      *multi_private_key_ptr = buffer; *multi_private_key_length = privkey.Length;

      buffer = (byte*)NativeMemory.Alloc((nuint)pubkey.Length);
      pubkey.CopyTo(new Span<byte>(buffer, pubkey.Length));
      *multi_public_key_ptr = buffer; *multi_public_key_length = pubkey.Length;
      Array.Clear(pubkey);

      return new CError { error_code = (int)CErrorCode.Ok };
    }
    catch (Exception ex)
    {
      CheckSetZero((nint*)*multi_sign_ptr); *multi_sign_length = 0;
      CheckSetZero((nint*)*multi_public_key_ptr); *multi_public_key_length = 0;
      CheckSetZero((nint*)*multi_private_key_ptr); *multi_private_key_length = 0;
      return CreateError(CErrorCode.UnknownError, ex.Message);
    }
  }



}
