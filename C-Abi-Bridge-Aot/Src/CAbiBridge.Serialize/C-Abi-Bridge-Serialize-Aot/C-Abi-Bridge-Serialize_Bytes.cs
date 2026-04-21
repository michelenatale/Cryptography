

using System.Runtime.InteropServices;

namespace michele.natale.CAbiBridge;

using static NetServicesUtils;


public sealed partial class SerializeBridge
{
  [UnmanagedCallersOnly(EntryPoint = "json_serialize_bytes_aot")]
  public unsafe static CError JsonSerializeBytesAot(
    byte* bytes_ptr, int bytes_length,
    byte** cipher_ptr, int* cipher_length)
  {
    *cipher_ptr = null; *cipher_length = 0;

    try
    {
      if (bytes_ptr is null || bytes_length <= 0)
        return new CError { error_code = (int)CErrorCode.InvalidLength };

      var romem = CryptoBridge.ToMemSafe(bytes_ptr, bytes_length);
      var result = NetServicesSerialize.SerializeJson(romem);

      var buffer = (byte*)NativeMemory.Alloc((nuint)result.Length);
      result.CopyTo(new Span<byte>(buffer, result.Length));
      *cipher_ptr = buffer; *cipher_length = result.Length;
      Array.Clear(result);

      return new CError { error_code = (int)CErrorCode.Ok };
    }
    catch (Exception ex)
    {
      CheckSetZero((nint*)*cipher_ptr);
      return CreateError(CErrorCode.UnknownError, ex.Message);
    }
  }
  [UnmanagedCallersOnly(EntryPoint = "json_deserialize_bytes_aot")]
  public unsafe static CError JsonDeserializeBytesAot(
    byte* bytes_ptr, int bytes_length,
    byte** decipher_ptr, int* decipher_length)
  {
    *decipher_ptr = null; *decipher_length = 0;

    try
    {
      if (bytes_ptr is null || bytes_length <= 0)
        return new CError { error_code = (int)CErrorCode.InvalidLength };

      var rospan = CryptoBridge.ToSpanSafe(bytes_ptr, bytes_length);
      var result = NetServicesSerialize.DeserializeJson(rospan);

      var buffer = (byte*)NativeMemory.Alloc((nuint)result.Length);
      result.CopyTo(new Span<byte>(buffer, result.Length));
      *decipher_ptr = buffer; *decipher_length = result.Length;
      Array.Clear(result);

      return new CError { error_code = (int)CErrorCode.Ok };
    }
    catch (Exception ex)
    {
      CheckSetZero((nint*)*decipher_ptr);
      return CreateError(CErrorCode.UnknownError, ex.Message);
    }
  }
}
