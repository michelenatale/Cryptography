

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

    //Declare it directly after initializing
    //the ApplicationConfiguration.
    AppChecks.CheckFrmIsOpen("LoginSystemApp");

    var author = WindowsManager.AuthorInfo;

    //Sets or checks the superordinate
    //configurations of LoginSystem.
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
}

