using System.Runtime.InteropServices;

namespace michele.natale.LoginSystems.Apps;

internal partial class Win_32_Api
{

  public const int SC_CLOSE = 0xF060;
  public const int WM_SYSCOMMAND = 0x0112;

  //LibraryImport not works correctly
  //[LibraryImport("user32.dll", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
  [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true, CharSet = CharSet.Auto)]
  public extern static IntPtr FindWindow(string lpClassName, string lpWindowName);

  [DllImport("user32.dll", EntryPoint = "SendMessage", SetLastError = true, CharSet = CharSet.Auto)]
  public extern static  int SendMessage(int hWnd, uint Msg, int wParam, int lParam);
}
