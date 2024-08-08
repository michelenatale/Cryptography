

namespace michele.natale.LoginSystems.Views;

using ViewModels;

/// <summary>
/// The UserControl that is displayed when the login process has been successful.
/// </summary>
internal partial class UcMainEx : UserControl
{

  /// <summary>
  /// C-Tor
  /// </summary>
  public UcMainEx()
  {
    this.InitializeComponent();
    this.InitializeBindings();
  }

  private void InitializeBindings()
  {
    this.SuspendLayout();
    this.BsUcMain.DataSource = typeof(VmUcMain);

    this.BtChangePw.DataBindings.Add(new Binding("Command", this.BsUcMain, "IcBtChangePw", true, DataSourceUpdateMode.OnPropertyChanged));
    this.BtInfoLoginSystem.DataBindings.Add(new Binding("Command", this.BsUcMain, "IcBtInfoLoginSystem", true, DataSourceUpdateMode.OnPropertyChanged));
    this.ResumeLayout(true);
  }

  private void OnClose()
  {
    var bs = this.BsUcMain.DataSource as VmUcMain;
    bs?.Dispose();
  }
}
