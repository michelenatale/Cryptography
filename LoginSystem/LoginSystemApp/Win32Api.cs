using System.Runtime.InteropServices;

namespace michele.natale.LoginSystems.Apps;

internal partial class Win32Api
{

  public const int SC_CLOSE = 0xF060;
  public const int WM_SYSCOMMAND = 0x0112;

  [LibraryImport("user32.dll", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
  public static partial IntPtr FindWindow(string lpClassName, string lpWindowName);

  [LibraryImport("user32.dll")]
  public static partial int SendMessage(int hWnd, uint Msg, int wParam, int lParam);
}
