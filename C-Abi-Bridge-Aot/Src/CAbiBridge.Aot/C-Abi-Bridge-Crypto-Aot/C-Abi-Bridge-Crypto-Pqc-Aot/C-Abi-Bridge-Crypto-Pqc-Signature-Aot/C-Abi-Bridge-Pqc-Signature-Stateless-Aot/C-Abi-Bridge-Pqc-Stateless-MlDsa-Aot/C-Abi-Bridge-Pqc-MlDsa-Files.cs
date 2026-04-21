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
  [UnmanagedCallersOnly(EntryPoint = "pqc_mldsa_sign_file_aot")]
  public unsafe static CError PqcMlDsaSignFileAot(
    byte* src_file_ptr, int src_file_length,
    byte* private_key_ptr, int private_key_length,
    byte mldsa_param, byte** sign_ptr, int* sign_length)
  {
    *sign_ptr = null; *sign_length = 0;

    try
    {
      var mldsaalgo = ToMLDsaAlgorithm((MLDsaParam)mldsa_param);
      var srcfile = ToStringUtf8Safe(src_file_ptr, src_file_length);
      var privatekey = ToUsIPtrSafe(private_key_ptr, private_key_length);

      using var cts = new CancellationTokenSource();
      var result = SignFileAsync(srcfile, privatekey, mldsaalgo, cts.Token)
        .GetAwaiter().GetResult();

      //The first value in the buffer or signature is reserved for the MLDSA algorithm.
      var buffer = (byte*)NativeMemory.Alloc((nuint)(result.Length + 1));
      result.CopyTo(new Span<byte>(buffer, result.Length + 1).Slice(1));
      *buffer = mldsa_param; *sign_ptr = buffer; *sign_length = result.Length + 1;
      Array.Clear(result);

      return new CError { error_code = (int)CErrorCode.Ok };
    }
    catch (Exception ex)
    {
      CheckSetZero((nint*)*sign_ptr); *sign_length = 0;
      return CreateError(CErrorCode.UnknownError, ex.Message);
    }
  }

  [UnmanagedCallersOnly(EntryPoint = "pqc_mldsa_verify_file_aot")]
  [return: MarshalAs(UnmanagedType.U1)]
  public unsafe static bool PqcMlDsaVerifyFileAot(
    byte* src_file_ptr, int src_file_length,
    byte* public_key_ptr, int public_key_length,
    byte* signature_ptr, int signature_length,
    CError* cerror)
  {
    *cerror = new CError { error_code = -1, message = IntPtr.Zero };

    try
    {
      var pubkey = ToMemSafe(public_key_ptr, public_key_length);
      var param = ToMLDsaAlgorithm((MLDsaParam)(*signature_ptr));
      var srcfile = ToStringUtf8Safe(src_file_ptr, src_file_length);
      var signature = ToMemSafe(signature_ptr + 1, signature_length - 1);

      using var cts = new CancellationTokenSource();
      var verify = VerifyFileAsync(srcfile, pubkey, signature, param, cts.Token)
        .GetAwaiter().GetResult();

      if (verify)
      {
        *cerror = new CError { error_code = (int)CErrorCode.Ok };
        return verify;
      }

      var exc_str = "File-Verification has failed!";
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
