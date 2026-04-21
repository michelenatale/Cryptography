

using System.Runtime.InteropServices;

namespace michele.natale.CAbiBridge;

using static NetServicesUtils;


public sealed partial class SerializeBridge
{

  [UnmanagedCallersOnly(EntryPoint = "json_serialize_files_aot")]
  public unsafe static CError JsonSerializeFilesAot(
    byte* src_ptr, int src_length, byte* dest_ptr, int dest_length)
  {
    try
    {
      if (src_ptr is null || src_length <= 0)
        return new CError { error_code = (int)CErrorCode.InvalidLength };

      if (dest_ptr is null || dest_length <= 0)
        return new CError { error_code = (int)CErrorCode.InvalidLength };

      var src = CryptoBridge.ToStringUtf8Safe(src_ptr, src_length);
      var dest = CryptoBridge.ToStringUtf8Safe(dest_ptr, dest_length);
      NetServicesSerialize. SerializeJsonAsync(src, dest).GetAwaiter().GetResult();

      return new CError { error_code = (int)CErrorCode.Ok };
    }
    catch (Exception ex)
    {
      CheckSetZero((nint*)src_ptr);
      CheckSetZero((nint*)dest_ptr);
      return CreateError(CErrorCode.UnknownError, ex.Message);
    }
  }

  [UnmanagedCallersOnly(EntryPoint = "json_deserialize_files_aot")]
  public unsafe static CError JsonDeserializeFilesAot(
    byte* src_ptr, int src_length, byte* dest_ptr, int dest_length)
  {
    try
    {
      if (src_ptr is null || src_length <= 0)
        return new CError { error_code = (int)CErrorCode.InvalidLength };

      if (dest_ptr is null || dest_length <= 0)
        return new CError { error_code = (int)CErrorCode.InvalidLength };

      var src = CryptoBridge.ToStringUtf8Safe(src_ptr, src_length);
      var dest = CryptoBridge.ToStringUtf8Safe(dest_ptr, dest_length);
      NetServicesSerialize.DeserializeJsonAsync(src, dest).GetAwaiter().GetResult();

      return new CError { error_code = (int)CErrorCode.Ok };
    }
    catch (Exception ex)
    {
      CheckSetZero((nint*)src_ptr);
      CheckSetZero((nint*)dest_ptr);
      return CreateError(CErrorCode.UnknownError, ex.Message);
    }
  }
}
