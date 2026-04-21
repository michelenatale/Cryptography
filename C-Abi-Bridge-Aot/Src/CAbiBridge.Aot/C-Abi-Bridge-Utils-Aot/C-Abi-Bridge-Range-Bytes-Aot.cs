

using System.Runtime.InteropServices;

namespace michele.natale.CAbiBridge;

using static NetServicesUtils;

partial class UtilsBridge
{
  [UnmanagedCallersOnly(EntryPoint = "range_nums_uint8_aot")]
  public unsafe static CError RangeNumsUInt8Aot(
   int start, int count, byte** output_ptr, int* output_length)
  {
    *output_ptr = null; *output_length = 0;
    try
    {
      var result = Enumerable.Range(start, count).Select(Convert.ToByte).ToArray();

      var buffer = (byte*)NativeMemory.Alloc((nuint)result.Length);
      result.CopyTo(new Span<byte>(buffer, result.Length));
      *output_ptr = buffer; *output_length = result.Length;

      return new CError { error_code = (int)CErrorCode.Ok };
    }
    catch (Exception ex)
    {
      CheckSetZero((nint*)*output_ptr);
      return CreateError(CErrorCode.UnknownError, ex.Message);
    }
  }


  [UnmanagedCallersOnly(EntryPoint = "range_nums_int32_aot")]
  public unsafe static CError RangeNumsInt32Aot(
   int start, int count, int** output_ptr, int* output_length)
  {
    *output_ptr = null; *output_length = 0;
    try
    {
      var result = Enumerable.Range(start, count).ToArray();
      var buffer = (int*)NativeMemory.Alloc((nuint)result.Length);
      result.CopyTo(new Span<int>(buffer, result.Length));
      *output_ptr = buffer; *output_length = result.Length;

      return new CError { error_code = (int)CErrorCode.Ok };
    }
    catch (Exception ex)
    {
      CheckSetZero((nint*)*output_ptr);
      return CreateError(CErrorCode.UnknownError, ex.Message);
    }
  }

}
