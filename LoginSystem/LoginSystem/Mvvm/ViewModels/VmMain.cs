namespace michele.natale.LoginSystems.ViewModels;

using Models;
using Pointers;
using System.Text;
using static Services.AppServices;

internal sealed class VmMain : VmBase
{

  #region Events
  public event EventHandler<FrmMainCommandsEventArgs> FrmCommandHandler = null!;
  #endregion Events

  #region Properties
  public VmUcMain VmUcMain { get; set; } = null!;
  public VmUcLogin VmUcLogin { get; set; } = null!;
  public VmUcRegist VmUcRegist { get; set; } = null!;
  public VmUcPwForget VmUcPwForget { get; set; } = null!;
  public VmUcPwChange VmUcPwChange { get; set; } = null!;
  #endregion Properties

  #region C-Tor
  public VmMain() => this.Init();

  public void Init()
  {
    this.VmUcMain = this.VmUcMainInit();
    this.VmUcLogin = this.VmUcLoginInit();
    this.VmUcRegist = this.VmUcRegistInit();
    this.VmUcPwForget = this.VmUcPwForgetInit();
    this.VmUcPwChange = this.VmUcPwChangeInit();
  }
  #endregion C-Tor

  #region Public Methodes
  public void SetUserControl(FrmCommands cmd)
  {
    var arg = new FrmMainCommandsEventArgs
    {
      HasWorked = false,
      Commands = cmd,
    };
    this.RaiseEvent(arg);
  }
  #endregion Public Methodes

  #region UserControls

  #region VmUcMain
  private VmUcMain VmUcMainInit()
  {
    return new VmUcMain
    {
      IcBtChangePw = new RelayCommand(this.BtChangePwUcMain),
      IcBtInfoLoginSystem = new RelayCommand(this.BtInfoLoginSystemUcMain),
    };
  }
  private void BtChangePwUcMain()
  {
    //...

    var cmd = FrmCommands.UcPwChange;
    this.RaiseEvent(cmd);
  }
  private void BtInfoLoginSystemUcMain()
  {
    var cmd = FrmCommands.FrmInfo;
    this.RaiseEvent(cmd);
  }
  #endregion VmUcMain

  #region VmUcLogin
  private VmUcLogin VmUcLoginInit()
  {
    return new VmUcLogin()
    {
      IcBtLogin = new RelayCommand(this.BtLogin),
      IcBtCancel = new RelayCommand(this.BtCancelUcLogin),
      IcLlRegist = new RelayCommand(this.LlRegistUcLogin),
      IcLlPwForget = new RelayCommand(this.LlPwForgetUcLogin),
    };
  }

  private void BtLogin()
  {
    var vm = this.VmUcLogin;
    var uname = vm.UserName.ToLower().Trim();
    using var pw = new UsIPtr<byte>(Encoding.UTF8.GetBytes(vm.PassWord.Trim()));
    if (AppServicesHolder.CheckValuesLogin(uname, pw, out var result))
    {
      //vm.ClearTextBoxes();
      var sdi = new SecureDataInfo(result);
      var serialize = sdi.Serialize();
      sdi.Reset();

      var argr = new RegistDataInfo
      {
        Tag = serialize,
        ASInfo = AppSettingsInfo.CheckRegistData,
      };

      AppServicesHolder.ToRegistDataInfo(argr);

      if (argr.CorrectExecution)
        this.SetUserControl(FrmCommands.UcMain);
      return;
    }
    vm.ClearTextBoxes();
    AppServicesHolder.ShowMessageApp("Not all inputs are correct.");
  }


  private void BtCancelUcLogin()
  {
    var cmd = FrmCommands.Close;
    this.RaiseEvent(cmd);
  }
  private void LlRegistUcLogin()
  {
    var cmd = FrmCommands.UcRegist;
    this.RaiseEvent(cmd);
  }
  private void LlPwForgetUcLogin()
  {
    var cmd = FrmCommands.UcPwForget;
    this.RaiseEvent(cmd);
  }

  #endregion VmUcLogin

  #region VmUcRegist

  private VmUcRegist VmUcRegistInit()
  {
    return new VmUcRegist()
    {
      IcBtRegist = new RelayCommand(this.BtRegist),
      IcBtCancel = new RelayCommand(this.BtCancelUcRegist),
      IcLlLogin = new RelayCommand(this.LlLoginUcRegist),
      IcLlPwForget = new RelayCommand(this.LlPwForgetUcRegist),
    };
  }

  private void BtRegist()
  {
    var vm = this.VmUcRegist;
    var email = vm.EMail.ToLower().Trim();
    var uname = vm.UserName.ToLower().Trim();
    using var pw = new UsIPtr<byte>(Encoding.UTF8.GetBytes(vm.PassWord.Trim()));
    using var pwr = new UsIPtr<byte>(Encoding.UTF8.GetBytes(vm.RPassWord.Trim()));
    if (AppServicesHolder.CheckValues(uname, pw, pwr, vm.EMail))
    {
      var sdi = new SecureDataInfo((uname, pw, email, null)!);
      sdi.CreateNewMPw();
      var serialize = sdi.Serialize();
      sdi.Reset();

      var arg = new RegistDataInfo
      {
        Tag = serialize,
        ASInfo = AppSettingsInfo.SetRegistData,
      };

      AppServicesHolder.ToRegistDataInfo(arg);

      if (arg.CorrectExecution)
        this.LlLoginUcRegist();

      return;
    }
    AppServicesHolder.ShowMessageApp("Not all inputs are correct.");
  }

  private void BtCancelUcRegist()
  {
    var cmd = FrmCommands.Close;
    this.RaiseEvent(cmd);
  }

  private void LlLoginUcRegist()
  {
    var cmd = FrmCommands.UcLogin;
    this.RaiseEvent(cmd);
  }
  private void LlPwForgetUcRegist()
  {
    var cmd = FrmCommands.UcPwForget;
    this.RaiseEvent(cmd);
  }
  #endregion VmUcRegist

  #region VmUcPwForget
  private VmUcPwForget VmUcPwForgetInit()
  {
    return new VmUcPwForget
    {
      IcBtPwForget = new RelayCommand(this.BtPwForget),
      IcLlLogin = new RelayCommand(this.LlLoginUcPwForget),
      IcBtCancel = new RelayCommand(this.BtCancelUcPwForget),
      IcLlRegist = new RelayCommand(this.LlRegistUcPwForget),
    };
  }

  private void BtPwForget()
  {
    var vm = this.VmUcPwForget;
    var port = vm.Port;
    var is_iss = vm.IsSsl;
    var delivery = vm.Delivery;
    var host = vm.Host.ToLower().Trim();
    var email = vm.EMail.ToLower().Trim();
    var uname = vm.UserName.ToLower().Trim();
    using var pw = new UsIPtr<byte>(Encoding.UTF8.GetBytes(vm.PassWord.Trim()));
    using var pwr = new UsIPtr<byte>(Encoding.UTF8.GetBytes(vm.RPassWord.Trim()));
    var data = (uname, email, pw, pwr, port, host, is_iss, delivery);
    if (AppServicesHolder.CheckValuesPwForget(data, out var result))
    {
      result.SetSubject();
      result.GenerateNewGuidForBody();

      using var serialize = EmailMsgInfo.ToPtrSerialize(result);
      result.Reset();

      var arg = new RegistDataInfo
      {
        Tag = serialize,
        ASInfo = AppSettingsInfo.SetNewRegistData,
      };

      AppServicesHolder.ToRegistDataInfo(arg);

      if (arg.CorrectExecution)
        this.SetUserControl(FrmCommands.UcLogin);
    }
  }
  private void LlLoginUcPwForget()
  {
    var cmd = FrmCommands.UcLogin;
    this.RaiseEvent(cmd);
  }
  private void BtCancelUcPwForget()
  {
    var cmd = FrmCommands.Close;
    this.RaiseEvent(cmd);
  }
  private void LlRegistUcPwForget()
  {
    var cmd = FrmCommands.UcRegist;
    this.RaiseEvent(cmd);
  }
  #endregion VmUcPwForget


  #region VmUcPwChange
  private VmUcPwChange VmUcPwChangeInit()
  {
    return new VmUcPwChange
    {
      IcBtPwChange = new RelayCommand(this.BtPwChange),
      IcBtCancel = new RelayCommand(this.BtCancelUcPwChange),
    };
  }

  private void BtPwChange()
  {
    var vm = this.VmUcPwChange;
    using var pw = new UsIPtr<byte>(Encoding.UTF8.GetBytes(vm.PassWord.Trim()));
    using var pwr = new UsIPtr<byte>(Encoding.UTF8.GetBytes(vm.RPassWord.Trim()));
    if (AppServicesHolder.CheckValuesPwChange(pw, pwr))
    {
      var arg = new RegistDataInfo
      {
        Tag = pw,
        ASInfo = AppSettingsInfo.ChangeRegistData,
      };

      AppServicesHolder.ToRegistDataInfo(arg);

      if (arg.CorrectExecution)
        this.SetUserControl(FrmCommands.UcMain);
    }
  }

  private void BtCancelUcPwChange()
  {
    var cmd = FrmCommands.UcMain;
    this.RaiseEvent(cmd);
  }

  #endregion VmUcPwChange

  #endregion UserControls

  #region RaiseEvent

  private void RaiseEvent(FrmCommands cmd)
  {
    var arg = new FrmMainCommandsEventArgs
    {
      Commands = cmd,
      HasWorked = false,
    };
    this.RaiseEvent(arg);
  }

  private void RaiseEvent(FrmMainCommandsEventArgs e)
  {
    FrmCommandHandler.Invoke(this, e);
  }
  #endregion RaiseEvent

  #region Disposing

  public bool IsDisposed
  {
    get; private set;
  }
  internal void Dispose(bool disposing)
  {
    if (!this.IsDisposed)
    {
      if (disposing)
      {
        this.Clear();
      }
      this.IsDisposed = true;
    }
  }

  ~VmMain() => this.Dispose(false);

  public override void Dispose()
  {
    this.Dispose(true);
    GC.SuppressFinalize(this);
  }

  public void Clear()
  {
    if (this.IsDisposed)
      return;

    this.VmUcMain?.Dispose();
    this.VmUcLogin?.Dispose();
    this.VmUcRegist?.Dispose();
    this.VmUcPwForget?.Dispose();
    this.VmUcPwChange?.Dispose();
    AppServicesHolder.DisposeKey();

    this.VmUcMain = null!;
    this.VmUcLogin = null!;
    this.VmUcRegist = null!;
    this.VmUcPwForget = null!;
    this.VmUcPwChange = null!;
  }
  #endregion Disposing
}
