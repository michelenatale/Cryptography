Option Strict On
Option Explicit On


Imports LoginSystemAppVb.LoginSystemApp
Imports michele.natale.LoginSystems.Apps


Namespace michele.natale.LoginSystems.Apps
  Public Class Program

    <STAThread>
    Public Shared Sub Main()
      Initialize()

      'Declare it directly after initializing
      'the ApplicationConfiguration.
      AppChecks.CheckFrmIsOpen("LoginSystemAppVb")

      Dim author = WindowsManager.AuthorInfo

      'Sets or checks the superordinate
      'configurations of LoginSystem.
      WindowsManager.SetStartConfig()


      'Start here © LoginSystem 2024
      Using frm_main = New FrmMain()
        Dim wm_frm_main = New WindowsManager(frm_main)

        Dim app = WindowsService.Instance
        app.AddService(Of IWindowsManager)(wm_frm_main)
        app.GetService(Of IWindowsManager)().Show()

        Dim my_frm_app = New MyFrmApp With
        {
          .Login_System = wm_frm_main
        }

        'Start here your Application
        Application.Run(my_frm_app)
      End Using
    End Sub
  End Class


  Friend Module ApplicationConfiguration
    Friend Sub Initialize()
      Application.EnableVisualStyles()
      Application.SetCompatibleTextRenderingDefault(False)
      Application.SetHighDpiMode(HighDpiMode.SystemAware)
    End Sub
  End Module

End Namespace
