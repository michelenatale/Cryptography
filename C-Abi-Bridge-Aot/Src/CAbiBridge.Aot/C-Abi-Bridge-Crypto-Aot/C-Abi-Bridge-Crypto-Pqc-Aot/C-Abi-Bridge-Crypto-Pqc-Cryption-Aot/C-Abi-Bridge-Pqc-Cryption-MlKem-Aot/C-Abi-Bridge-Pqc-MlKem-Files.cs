
using System.Text;
using michele.natale.Pointers;
using System.Runtime.InteropServices;


//Start
//Alice(Keypair)
//    >> Bob(Encapsulation)
//        >> Alice(Encryption)
//            >> Bob(Decryption)
//Finish


namespace michele.natale.CAbiBridge;

using static NetServicesUtils;
using static NetServicesCrypto;

partial class CryptoBridge
{
  [UnmanagedCallersOnly(EntryPoint = "pqc_mlkem_encryption_file_aot")]
  public unsafe static CError PqcMlKemEncryptionFileAot(
    byte* src_file_ptr, int src_file_length,
    byte* dest_file_ptr, int dest_file_length,
    byte* private_key_ptr, int private_key_length,
    byte* capsulation_ptr, int capsulation_length,
    byte* associated_ptr, int associated_length,
    byte mlkem_param, byte crypto_algo)
  {
    try
    {
      var mlkemalgo = ToMLKemAlgorithm(mlkem_param);
      var srcfile = ToStringUtf8Safe(src_file_ptr, src_file_length);
      var associated = ToMemSafe(associated_ptr, associated_length);
      var capsulation = ToMemSafe(capsulation_ptr, capsulation_length);
      var destfile = ToStringUtf8Safe(dest_file_ptr, dest_file_length);
      var privatekey = ToUsIPtrSafe(private_key_ptr, private_key_length);

      var cts = new CancellationTokenSource();
      PqcMlKemEncryptionFileAsync(srcfile, destfile, privatekey,
        capsulation, associated, mlkemalgo,
        (CryptionAlgorithm)crypto_algo, cts.Token)
          .GetAwaiter().GetResult();

      return new CError { error_code = (int)CErrorCode.Ok };
    }
    catch (Exception ex)
    {
      return CreateError(CErrorCode.UnknownError, ex.Message);
    }
  }

  [UnmanagedCallersOnly(EntryPoint = "pqc_mlkem_decryption_file_aot")]
  public unsafe static CError PqcMlKemDecryptionFileAot(
    byte* src_file_ptr, int src_file_length,
    byte* dest_file_ptr, int dest_file_length,
    byte* shared_key_ptr, int shared_key_length,
    byte* associated_ptr, int associated_length)
  {
    try
    {
      var srcfile = ToStringUtf8Safe(src_file_ptr, src_file_length);
      var associated = ToMemSafe(associated_ptr, associated_length);
      var sharedkey = ToUsIPtrSafe(shared_key_ptr, shared_key_length);
      var destfile = ToStringUtf8Safe(dest_file_ptr, dest_file_length);

      var cts = new CancellationTokenSource();
      PqcMlKemDecryptionFileAsync(srcfile, destfile, 
        sharedkey, associated, cts.Token)
        .GetAwaiter().GetResult();

      return new CError { error_code = (int)CErrorCode.Ok };
    }
    catch (Exception ex)
    {
      return CreateError(CErrorCode.UnknownError, ex.Message);
    }
  }
}
