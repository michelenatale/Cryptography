

namespace michele.natale;

partial class NetServicesUtils
{



  public unsafe static bool CheckSetZero(IntPtr* ptr)
  {
    if (ptr is not null)
      *ptr = IntPtr.Zero;
    return true;
  }
}
