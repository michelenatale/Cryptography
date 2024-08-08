

using LoginSystemApp;
using System.Diagnostics;


namespace michele.natale.LoginSystems.Apps;


public static class Program
{

  /// <summary>
  ///  The main entry point for the application.
  /// </summary>
  [STAThread]
  public static void Main()
  {

    ApplicationConfiguration.Initialize();

    CheckFrmIsOpen("LoginSystemApp");

    var author = WindowsManager.AuthorInfo;

    WindowsManager.SetStartConfig();

    //Start here © LoginSystem 2024
    using var frm_main = new FrmMain();
    var wm_frm_main = new WindowsManager(frm_main);

    var app = WindowsService.Instance;
    app.AddService<IWindowsManager>(wm_frm_main);
    app.GetService<IWindowsManager>().Show();

    //Start here your Application
    var my_frm_app = new MyFrmApp
    {
      Login_System = wm_frm_main,
    };
    Application.Run(my_frm_app);
  }

  private static void CheckFrmIsOpen(string app_name)
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
          var handle = Win32Api.FindWindow(null!, hprocess.MainWindowTitle);
          if (!handle.Equals(IntPtr.Zero))
            _ = Win32Api.SendMessage((int)handle, Win32Api.WM_SYSCOMMAND, Win32Api.SC_CLOSE, 0);
        }
      }
      else
      {
        break;
      }
    }
  }
}

