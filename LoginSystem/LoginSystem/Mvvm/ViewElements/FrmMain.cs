
using LoginSystem.Properties;
using System.ComponentModel;

namespace michele.natale.LoginSystems.Apps;

using Views;
using Models;
using Services;
using ViewModels;
using static Services.AppServices;

public partial class FrmMain : Form
{


  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public object PVmUc { get; set; } = null!;
  public FrmCommands Frm_Cmd = FrmCommands.None;
  private readonly AppServices Services = AppServicesHolder;

  public FrmMain()
  {
    this.InitializeComponent();
    this.InitializeViewModel(out VmMain vm);
    this.InitializeUserControl(vm);

    this.DialogResult = DialogResult.OK;

    this.Icon = Resources.myLogo64;
  }

  protected override void OnLoad(EventArgs e)
  {
    base.OnLoad(e);
    if (this.Frm_Cmd == FrmCommands.UcRegist)
      this.Services.SetUcBinding(this, this.UcRegist, this.PVmUc);
    else if (this.Frm_Cmd == FrmCommands.UcLogin)
      this.Services.SetUcBinding(this, this.UcLogin, this.PVmUc);
  }

  private void InitializeViewModel(out VmMain vm_main)
  {
    vm_main = new VmMain();
    this.BsViewModel.DataSource = vm_main;
    vm_main.FrmCommandHandler += this.FrmMain_Command!;
  }

  private void InitializeUserControl(VmMain vm_main)
  {
    this.Frm_Cmd = FrmCommands.None;
    var sc = AppServicesHolder.ToStartConfiguration();
    if (sc is null) return;
    vm_main.SetUserControl(this.Frm_Cmd = sc.Frm_Cmd);
  }

  private void FrmMain_FormClosed(object sender, FormClosedEventArgs e)
  {
    using var vm_main = this.BsViewModel?.DataSource as VmMain;
    this.DialogResult = DialogResult.Abort;
  }

  private void FrmMain_Command(object sender, FrmMainCommandsEventArgs e)
  {
    var cmd = e.Commands;
    var vm_frm = (VmMain)this.BsViewModel.DataSource;

    this.DialogResult = DialogResult.OK;

    switch (cmd)
    {
      case FrmCommands.UcMain:
      case FrmCommands.UcLogin:
      case FrmCommands.UcRegist:
      case FrmCommands.UcPwChange:
      case FrmCommands.UcPwForget:
        {
          var uc = this.ToAndSetUserControl(cmd);
          this.Services.SetFrmUserControl(this, uc, vm_frm);
        }
        break;

      case FrmCommands.Close:
        {
          this.DialogResult = DialogResult.Abort;
          var uc = this.Services.CurrentUserControl(this);
          uc?.Dispose(); this.Close();
        }
        break;

      case FrmCommands.FrmInfo:
        {
          using var frm = new FrmInfo();
          frm.ShowDialog();
        }
        break;

      default: return;
    }
  }

  private UserControl ToAndSetUserControl(FrmCommands cmd)
  {
    var uc = this.Services.ToNewUserControl(cmd);
    switch (uc.GetType())
    {
      case var obj when obj == typeof(UcMainEx): this.UcMain = (UcMainEx)uc; break;
      case var obj when obj == typeof(UcLoginEx): this.UcLogin = (UcLoginEx)uc; break;
      case var obj when obj == typeof(UcRegistEx): this.UcRegist = (UcRegistEx)uc; break;
      case var obj when obj == typeof(UcPwChangeEx): this.UcPwChange = (UcPwChangeEx)uc; break;
      case var obj when obj == typeof(UcPwForgetEx): this.UcPwForget = (UcPwForgetEx)uc; break;
      default: throw new ArgumentException($"{nameof(cmd)} is failed!");
    }
    this.CleanUpUserControls(uc);
    return uc;
  }

  private void CleanUpUserControls(UserControl uc)
  {
    if (uc != this.UcMain) this.UcMain = null!;
    if (uc != this.UcLogin) this.UcLogin = null!;
    if (uc != this.UcRegist) this.UcRegist = null!;
    if (uc != this.UcPwChange) this.UcPwChange = null!;
    if (uc != this.UcPwForget) this.UcPwForget = null!;
  }
}