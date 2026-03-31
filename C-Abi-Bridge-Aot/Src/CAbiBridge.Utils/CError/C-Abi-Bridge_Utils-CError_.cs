

using System.Runtime.InteropServices;


namespace michele.natale;

partial class NetServicesUtils
{


  public static CError CreateError(CErrorCode code, string msg)
  {
    var ptr = Marshal.StringToHGlobalAnsi(msg);
    return new CError { error_code = (int)code, message = ptr };
  }
}
