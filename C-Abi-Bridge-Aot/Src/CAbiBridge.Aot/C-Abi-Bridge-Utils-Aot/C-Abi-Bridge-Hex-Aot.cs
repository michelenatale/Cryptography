

using System.Text;
using System.Runtime.InteropServices;

namespace michele.natale.CAbiBridge;

using static NetServicesUtils;

partial class UtilsBridge
{
  [UnmanagedCallersOnly(EntryPoint = "to_hex_utf8_aot")]
  public unsafe static CError ToHexUtf8Aot(
    byte* bytes, int bytes_length, byte** output_ptr, int* outputLen,
    [MarshalAs(UnmanagedType.U1)] bool lower)
  {
    try
    {
      byte[] result = [];
      var data = new ReadOnlySpan<byte>(bytes, bytes_length);
      if (lower) result = Encoding.UTF8.GetBytes(Convert.ToHexStringLower(data));
      else result = Encoding.UTF8.GetBytes(Convert.ToHexString(data));

      var buffer = (byte*)NativeMemory.Alloc((nuint)result.Length);
      result.CopyTo(new Span<byte>(buffer, result.Length));
      *output_ptr = buffer; *outputLen = result.Length;

      return new CError { error_code = (int)CErrorCode.Ok };
    }
    catch (Exception ex)
    {
      CheckSetZero((nint*)*output_ptr);
      return CreateError(CErrorCode.UnknownError, ex.Message);
    }
  }

  [UnmanagedCallersOnly(EntryPoint = "from_hex_utf8_aot")]
  public unsafe static CError FromHexUtf8Aot(
    byte* bytes, int bytes_length, byte** output_ptr, int* outputLen)
  {
    try
    {
      var data = new ReadOnlySpan<byte>(bytes, bytes_length);
      var result = Convert.FromHexString(Encoding.UTF8.GetString(data));

      var buffer = (byte*)NativeMemory.Alloc((nuint)result.Length);
      result.CopyTo(new Span<byte>(buffer, result.Length));
      *output_ptr = buffer; *outputLen = result.Length;

      return new CError { error_code = (int)CErrorCode.Ok };
    }
    catch (Exception ex)
    {
      CheckSetZero((nint*)*output_ptr);
      return CreateError(CErrorCode.UnknownError, ex.Message);
    }
  }
}
