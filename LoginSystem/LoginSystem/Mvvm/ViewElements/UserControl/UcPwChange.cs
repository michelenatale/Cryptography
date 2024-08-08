


namespace michele.natale.LoginSystems.Views;


using ViewModels;

/// <summary>
/// The UserControl that is displayed when the password is changed.
/// </summary>
internal partial class UcPwChangeEx : UserControl
{

  /// <summary>
  /// C-Tor
  /// </summary>
  public UcPwChangeEx()
  {
    this.InitializeComponent();
    this.InitializeBindings();
  }

  private void InitializeBindings()
  {
    this.SuspendLayout();
    this.BsUcPwChange.DataSource = typeof(VmUcPwChange);

    this.TbPw.DataBindings.Add(new Binding("Text", this.BsUcPwChange, "PassWord", true, DataSourceUpdateMode.OnPropertyChanged));
    this.TbRPw.DataBindings.Add(new Binding("Text", this.BsUcPwChange, "RPassWord", true, DataSourceUpdateMode.OnPropertyChanged));

    this.BtPwCancel.DataBindings.Add(new Binding("Command", this.BsUcPwChange, "IcBtCancel", true, DataSourceUpdateMode.OnPropertyChanged));
    this.BtPwChange.DataBindings.Add(new Binding("Command", this.BsUcPwChange, "IcBtPwChange", true, DataSourceUpdateMode.OnPropertyChanged));
    this.ResumeLayout(true);
  }

  private void OnClose()
  {
    var bs = this.BsUcPwChange.DataSource as VmUcPwChange;
    bs?.Dispose();
  }
}
