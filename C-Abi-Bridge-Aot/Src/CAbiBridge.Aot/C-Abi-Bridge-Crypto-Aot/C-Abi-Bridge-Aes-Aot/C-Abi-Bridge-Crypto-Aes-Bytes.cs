
using System.Security.Cryptography;
using System.Runtime.InteropServices;

namespace michele.natale.CAbiBridge;

using Pointers;
using static NetServices;

partial class CryptoBridge
{

  [UnmanagedCallersOnly(EntryPoint = "aes_encrypt_aot")]
  public unsafe static CError AesEncryptAot(
    byte* bytes_ptr, int bytes_length,
    byte* key_ptr, int key_length,
    byte* associated_ptr, int associated_length,
    byte** cipher_ptr, int* cipher_length)
  {
    try
    {
      //UnmanagedCallersOnly blocks the ability to test the method.
      //This is how the ‘AesEncryptAotManaged’ method can be tested.
      var span_key = new ReadOnlySpan<byte>(key_ptr, key_length).ToArray();
      using var key = new UsIPtr<byte>(span_key);

      var result = AesEncryptAotManaged(
          new ReadOnlySpan<byte>(bytes_ptr, bytes_length),
          key, new ReadOnlySpan<byte>(associated_ptr, associated_length),
          out var cipher);

      var buffer = (byte*)NativeMemory.Alloc((nuint)cipher.Length);
      cipher.CopyTo(new Span<byte>(buffer, cipher.Length));
      *cipher_ptr = buffer; *cipher_length = cipher.Length;

      return result;
    }
    catch (Exception ex)
    {
      return CreateError(CErrorCode.UnknownError, ex.Message);
    }
  }

  public static CError AesEncryptAotManaged(
  ReadOnlySpan<byte> bytes, UsIPtr<byte> key,
  ReadOnlySpan<byte> associated, out byte[] cipher)
  {
    cipher = [];

    try
    {
      if (bytes.IsEmpty || key.IsEmpty)
        return new CError { error_code = (int)CErrorCode.NullPointer };

      if (bytes.Length <= 0 || key.Length <= 0)
        return new CError { error_code = (int)CErrorCode.InvalidLength };

      cipher = EncryptionAes(bytes, key, associated);

      if (cipher is not null)
        return new CError { error_code = (int)CErrorCode.Ok };

      throw new CryptographicException(
        $"The method '{nameof(EncryptionAes)}' returned a null value!!");
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


  [UnmanagedCallersOnly(EntryPoint = "aes_decrypt_aot")]
  public unsafe static CError AesDecryptAot(
   byte* bytes_ptr, int bytes_length,
   byte* key_ptr, int key_length,
   byte* associated_ptr, int associated_length,
   byte** decipher_ptr, int* decipher_length)
  {
    try
    {
      //UnmanagedCallersOnly blocks the ability to test the method.
      //This is how the ‘AesDecryptAotManaged’ method can be tested.
      var span_key = new ReadOnlySpan<byte>(key_ptr, key_length).ToArray();
      using var key = new UsIPtr<byte>(span_key);

      var result = AesDecryptAotManaged(
          new ReadOnlySpan<byte>(bytes_ptr, bytes_length),
          key, new ReadOnlySpan<byte>(associated_ptr, associated_length),
          out var decipher);

      var buffer = (byte*)NativeMemory.Alloc((nuint)decipher.Length);
      decipher.CopyTo(new Span<byte>(buffer, decipher.Length));
      *decipher_ptr = buffer; *decipher_length = decipher.Length;

      return result;
    }
    catch (Exception ex)
    {
      return CreateError(CErrorCode.UnknownError, ex.Message);
    }
  }


  public static CError AesDecryptAotManaged(
    ReadOnlySpan<byte> bytes, UsIPtr<byte> key,
    ReadOnlySpan<byte> associated, out byte[] decipher)
  {
    decipher = [];

    try
    {

      if (bytes.IsEmpty || key.IsEmpty)
        return new CError { error_code = (int)CErrorCode.NullPointer };

      if (bytes.Length <= 0 || key.Length <= 0)
        return new CError { error_code = (int)CErrorCode.InvalidLength };

      decipher = DecryptionAes(bytes, key, associated);

      if (decipher is not null)
        return new CError { error_code = (int)CErrorCode.Ok };

      throw new CryptographicException(
        $"The method '{nameof(DecryptionAes)}' returned a null value!!");
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
