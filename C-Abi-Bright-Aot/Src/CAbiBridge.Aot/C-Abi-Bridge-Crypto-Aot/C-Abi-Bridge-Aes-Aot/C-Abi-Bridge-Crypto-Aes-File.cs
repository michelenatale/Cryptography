
using System.Text;
using System.Security.Cryptography;
using System.Runtime.InteropServices;

namespace michele.natale.CAbiBridge;

using Pointers;
using static NetServices;

partial class CryptoBridge
{
  [UnmanagedCallersOnly(EntryPoint = "aes_encrypt_file_aot")]
  public  unsafe static CError AesEncryptFileAot(
    byte* src_ptr, int src_length,
    byte* dest_ptr, int dest_length,
    byte* key_ptr, int key_length,
    byte* associated_ptr, int associated_length)
  {
    try
    {
      //UnmanagedCallersOnly blocks the ability to test the method.
      //This is how the ‘AesEncryptFileAotManaged’ method can be tested.
      var span_key = new ReadOnlySpan<byte>(key_ptr, key_length).ToArray();
      using var key = new UsIPtr<byte>(span_key);

      return AesEncryptFileManaged(
        new ReadOnlySpan<byte>(src_ptr, src_length),
        new ReadOnlySpan<byte>(dest_ptr, dest_length),
        key, new ReadOnlySpan<byte>(associated_ptr, associated_length));
    }
    catch (Exception ex)
    {
      return CreateError(CErrorCode.UnknownError, ex.Message);
    }
  }

  public  static CError AesEncryptFileManaged(
    ReadOnlySpan<byte> src, ReadOnlySpan<byte> dest,
    UsIPtr<byte> key, ReadOnlySpan<byte> associated)
  {
    try
    {
      if (src.IsEmpty || dest.IsEmpty || key.IsEmpty)
        return new CError { error_code = (int)CErrorCode.NullPointer };

      if (src.Length <= 0 || dest.Length <= 0 || key.Length <= 0)
        return new CError { error_code = (int)CErrorCode.InvalidLength };

      var ssrc = Encoding.UTF8.GetString(src);
      var sdest = Encoding.UTF8.GetString(dest);

      EncryptionFileAesAsync(ssrc, sdest, key, associated.ToArray())
          .GetAwaiter().GetResult();

      return new CError { error_code = (int)CErrorCode.Ok };
    }
    catch (IOException ex)
    {
      return CreateError(CErrorCode.IoError, ex.Message);
    }
    catch (CryptographicException ex)
    {
      return CreateError(CErrorCode.CryptoError, ex.Message);
    }
    catch (Exception ex)
    {
      return CreateError(CErrorCode.UnknownError, ex.Message);
    }
  }



  [UnmanagedCallersOnly(EntryPoint = "aes_decrypt_file_aot")]
  public  unsafe static CError AesDecryptFileAot(
      byte* src_ptr, int src_length,
      byte* dest_ptr, int dest_length,
      byte* key_ptr, int key_length,
      byte* associated_ptr, int associated_length)
  {
    try
    {
      //UnmanagedCallersOnly blocks the ability to test the method.
      //This is how the ‘AesDecryptFileAotManaged’ method can be tested.
      var span_key = new ReadOnlySpan<byte>(key_ptr, key_length).ToArray();
      using var key = new UsIPtr<byte>(span_key);

      return AesDecryptFileManaged(
        new ReadOnlySpan<byte>(src_ptr, src_length),
        new ReadOnlySpan<byte>(dest_ptr, dest_length),
        key, new ReadOnlySpan<byte>(associated_ptr, associated_length));
    }
    catch (Exception ex)
    {
      return CreateError(CErrorCode.UnknownError, ex.Message);
    }
  }

  public  static CError AesDecryptFileManaged(
    ReadOnlySpan<byte> src, ReadOnlySpan<byte> dest,
    UsIPtr<byte> key, ReadOnlySpan<byte> associated)
  {
    try
    {
      if (src.IsEmpty || dest.IsEmpty || key.IsEmpty)
        return new CError { error_code = (int)CErrorCode.NullPointer };

      if (src.Length <= 0 || dest.Length <= 0 || key.Length <= 0)
        return new CError { error_code = (int)CErrorCode.InvalidLength };

      var ssrc = Encoding.UTF8.GetString(src);
      var sdest = Encoding.UTF8.GetString(dest);

      DecryptionFileAesAsync(ssrc, sdest, key, associated.ToArray())
          .GetAwaiter().GetResult();

      return new CError { error_code = (int)CErrorCode.Ok };
    }
    catch (IOException ex)
    {
      return CreateError(CErrorCode.IoError, ex.Message);
    }
    catch (CryptographicException ex)
    {
      return CreateError(CErrorCode.CryptoError, ex.Message);
    }
    catch (Exception ex)
    {
      return CreateError(CErrorCode.UnknownError, ex.Message);
    }
  }
}
