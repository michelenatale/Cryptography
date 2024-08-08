

//namespace michele.natale.LoginSystems.Services;

//using Apps;
//using Views;
//using ViewModels;

//partial  class AppServices 
//{

//  /// <summary>
//  /// Define the UserControl Login
//  /// </summary>
//  /// <param name="frm">FrmMain</param>
//  /// <param name="uclogin">UserControl Login</param>
//  /// <returns>ViewModel Login</returns>
//  public static VmUcLogin SetFrmControlLogin(
//    FrmMain frm, UcLoginEx uclogin)
//  {
//    SetUcSettings(uclogin, uclogin.Name[..^2]);
//    frm.SuspendLayout();
//    frm.Controls.Add(uclogin);
//    frm.ResumeLayout(false);
//    var vm_login = new VmUcLogin();
//    uclogin.BsUcLogin.DataSource = vm_login;
//    return vm_login;
//  }

//  /// <summary>
//  /// Define the UserControl Regist
//  /// </summary>
//  /// <param name="frm">FrmMain</param>
//  /// <param name="ucregist">UserControl Regist</param>
//  /// <returns>ViewModel Regist</returns>
//  public static VmUcRegist SetFrmControlRegist(
//    FrmMain frm, UcRegistEx ucregist)
//  {
//    SetUcSettings(ucregist, ucregist.Name[..^2]);
//    frm.SuspendLayout();
//    frm.Controls.Add(ucregist);
//    frm.ResumeLayout(false);
//    var vm_regist = new VmUcRegist();
//    ucregist.BsUcRegist.DataSource = vm_regist;
//    return vm_regist;
//  }

//  /// <summary>
//  /// Define the UserControl PwForget
//  /// </summary>
//  /// <param name="frm">FrmMain</param>
//  /// <param name="ucpwforget">UserControl PwForget</param>
//  /// <returns>Viewmodel PwForget</returns>
//  public static VmUcPwForget SetFrmControlPwForget(
//    FrmMain frm, UcPwForgetEx ucpwforget)
//  {
//    SetUcSettings(ucpwforget, ucpwforget.Name[..^2]);
//    frm.SuspendLayout();
//    frm.Controls.Add(ucpwforget);
//    frm.ResumeLayout(false);
//    var vm_pw_forget = new VmUcPwForget();
//    ucpwforget.BsUcPwForget.DataSource = vm_pw_forget;
//    return vm_pw_forget;
//  }

//  /// <summary>
//  /// Define the UserControl PwChange
//  /// </summary>
//  /// <param name="frm">FrmMain</param>
//  /// <param name="ucpwchange">UserControl PwChange</param>
//  /// <returns>ViewModel PwChange</returns>
//  public static VmUcPwChange SetFrmControlPwChange(
//    FrmMain frm, UcPwChangeEx ucpwchange)
//  {
//    SetUcSettings(ucpwchange, ucpwchange.Name[..^2]);
//    frm.SuspendLayout();
//    frm.Controls.Add(ucpwchange);
//    frm.ResumeLayout(false);
//    var vm_pw_change = new VmUcPwChange();
//    ucpwchange.BsUcPwChange.DataSource = vm_pw_change;
//    return vm_pw_change;
//  }

//  /// <summary>
//  /// Define the UserControl UcMain
//  /// </summary>
//  /// <param name="frm">FrmMain</param>
//  /// <param name="ucmain">UserControl UcMain</param>
//  /// <returns>ViewModel UcMain</returns>
//  public static VmUcMain SetFrmControlCompleted(
//    FrmMain frm, UcMainEx ucmain)
//  {
//    SetUcSettings(ucmain, ucmain.Name[..^2]);
//    frm.SuspendLayout();
//    frm.Controls.Add(ucmain);
//    frm.ResumeLayout(false);
//    var vm_main = new VmUcMain();
//    ucmain.BsUcMain.DataSource = vm_main;
//    return vm_main;
//  }
//}
