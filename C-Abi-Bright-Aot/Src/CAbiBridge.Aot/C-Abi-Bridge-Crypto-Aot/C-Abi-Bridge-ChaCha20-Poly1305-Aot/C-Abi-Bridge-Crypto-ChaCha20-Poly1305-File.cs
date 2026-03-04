
using System.Text;
using System.Security.Cryptography;
using System.Runtime.InteropServices;

namespace michele.natale.CAbiBridge;

using Pointers;
using static NetServices;

partial class CryptoBridge
{


  [UnmanagedCallersOnly(EntryPoint = "chacha20_poly1305_encrypt_file_aot")]
  public  unsafe static CError ChaCha20Poly1305EncryptFileAot(
    byte* src_ptr, int src_length,
    byte* dest_ptr, int dest_length,
    byte* key_ptr, int key_length,
    byte* associated_ptr, int associated_length)
  {
    try
    {
      //UnmanagedCallersOnly blocks the ability to test the method.
      //This is how the ‘ChaCha20Poly1305EncryptFileAot’ method can be tested.
      var span_key = new ReadOnlySpan<byte>(key_ptr, key_length).ToArray();
      using var key = new UsIPtr<byte>(span_key);

      return ChaCha20Poly1305EncryptFileManaged(
        new ReadOnlySpan<byte>(src_ptr, src_length),
        new ReadOnlySpan<byte>(dest_ptr, dest_length),
        key, new ReadOnlySpan<byte>(associated_ptr, associated_length));
    }
    catch (Exception ex)
    {
      return CreateError(CErrorCode.UnknownError, ex.Message);
    }
  }

  public  static CError ChaCha20Poly1305EncryptFileManaged(
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

      EncryptionFileChaCha20Poly1305Async(ssrc, sdest, key, associated.ToArray())
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


  [UnmanagedCallersOnly(EntryPoint = "chacha20_poly1305_decrypt_file_aot")]
  public  unsafe static CError ChaCha20Poly1305DecryptFileAot(
      byte* src_ptr, int src_length,
      byte* dest_ptr, int dest_length,
      byte* key_ptr, int key_length,
      byte* associated_ptr, int associated_length)
  {
    try
    {
      //UnmanagedCallersOnly blocks the ability to test the method.
      //This is how the ‘ChaCha20Poly1305DecryptFileAot’ method can be tested.
      var span_key = new ReadOnlySpan<byte>(key_ptr, key_length).ToArray();
      using var key = new UsIPtr<byte>(span_key);

      return ChaCha20Poly1305DecryptFileManaged(
        new ReadOnlySpan<byte>(src_ptr, src_length),
        new ReadOnlySpan<byte>(dest_ptr, dest_length),
        key, new ReadOnlySpan<byte>(associated_ptr, associated_length));
    }
    catch (Exception ex)
    {
      return CreateError(CErrorCode.UnknownError, ex.Message);
    }
  }

  public  static CError ChaCha20Poly1305DecryptFileManaged(
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

      DecryptionFileChaCha20Poly1305Async(ssrc, sdest, key, associated.ToArray())
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
