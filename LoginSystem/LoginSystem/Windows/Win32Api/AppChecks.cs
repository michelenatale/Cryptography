
using System.Diagnostics;

namespace michele.natale.LoginSystems.Apps;

public class AppChecks
{
  public static void CheckFrmIsOpen(string app_name)
  {
    while (true)
    {
      var count = Process.GetProcessesByName(AppDomain.CurrentDomain.FriendlyName).Length;
      if (count > 1)
      {
        var processes = Process.GetProcesses()
          .Where(p => p.ProcessName.ToLower().Trim().Contains(app_name.ToLower().Trim()));

        foreach (var hprocess in processes)
        {
          var handle = Win_32_Api.FindWindow(null!, hprocess.MainWindowTitle);
          if (!handle.Equals(IntPtr.Zero))
            _ = Win_32_Api.SendMessage((int)handle, Win_32_Api.WM_SYSCOMMAND, Win_32_Api.SC_CLOSE, 0);
        }
      }
      else
      {
        break;
      }
    }
  }
}
