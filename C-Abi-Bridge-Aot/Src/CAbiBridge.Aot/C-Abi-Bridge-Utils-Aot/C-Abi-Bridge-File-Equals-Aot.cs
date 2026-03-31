

using System.Text;
using System.Runtime.InteropServices; 

namespace michele.natale.CAbiBridge;

using static NetServicesUtils;


partial class UtilsBridge
{
  [UnmanagedCallersOnly(EntryPoint = "equal_files_aot")]
  [return: MarshalAs(UnmanagedType.U1)]
  public unsafe static bool EqualFilesAot(
    byte* file_path_left_ptr, int file_path_left_length,
    byte* file_path_right_ptr, int file_path_right_length,
    CError* cerror)
  {
    try
    {
      var left = Encoding.UTF8.GetString(new ReadOnlySpan<byte>(file_path_left_ptr, file_path_left_length));
      var right = Encoding.UTF8.GetString(new ReadOnlySpan<byte>(file_path_right_ptr, file_path_right_length));

      var result = EqualFiles(left,right);
      if (!result) 
        throw new Exception($"FileEquality has failed!");

      *cerror = new CError { error_code = (int)CErrorCode.Ok };
      return result;
    }
    catch (Exception ex)
    {
      *cerror = CreateError(CErrorCode.UnknownError, ex.Message);
      return false;
    }
  }
}
