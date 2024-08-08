

using System.Windows.Input;

namespace michele.natale.LoginSystems.Views;

using ViewModels;
 

/// <summary>
/// The UserControl for registration.
/// </summary>
internal partial class UcRegistEx : UserControl
{
  /// <summary>
  /// C-Tor
  /// </summary>
  public UcRegistEx()
  {
    this.InitializeComponent();
    this.InitializeBindings();

    //if (File.Exists(DataSettingFile))
    //  this.LlRegistPwForget.Enabled = true;
  }
  private void InitializeBindings()
  {
    this.SuspendLayout();
    this.BsUcRegist.DataSource = typeof(VmUcRegist);

    this.TbPw.DataBindings.Add(new Binding("Text", this.BsUcRegist, "PassWord", true, DataSourceUpdateMode.OnPropertyChanged));
    this.TbEmail.DataBindings.Add(new Binding("Text", this.BsUcRegist, "EMail", true, DataSourceUpdateMode.OnPropertyChanged));
    this.TbRepeatPw.DataBindings.Add(new Binding("Text", this.BsUcRegist, "RPassWord", true, DataSourceUpdateMode.OnPropertyChanged));
    this.TbUserName.DataBindings.Add(new Binding("Text", this.BsUcRegist, "UserName", true, DataSourceUpdateMode.OnPropertyChanged));

    this.LlGetLogin.DataBindings.Add(new Binding("Tag", this.BsUcRegist, "IcLlLogin", true, DataSourceUpdateMode.OnPropertyChanged));
    this.LlPwForget.DataBindings.Add(new Binding("Tag", this.BsUcRegist, "IcLlPwForget", true, DataSourceUpdateMode.OnPropertyChanged));

    this.BtRegist.DataBindings.Add(new Binding("Command", this.BsUcRegist, "IcBtRegist", true, DataSourceUpdateMode.OnPropertyChanged));
    this.BtCancel.DataBindings.Add(new Binding("Command", this.BsUcRegist, "IcBtCancel", true, DataSourceUpdateMode.OnPropertyChanged));
    this.ResumeLayout(true);

  }

  /// <summary>
  /// The click events used in this UserControl.
  /// </summary>
  /// <param name="sender">The sender of the events</param>
  /// <param name="e">The Parameter from events</param>
  private void UcRegist_Click(object sender, LinkLabelLinkClickedEventArgs e)
  {
    switch (sender)
    {
      case var obj when obj == this.LlGetLogin: this.ToLogin(e); return;
      case var obj when obj == this.LlPwForget: this.ToPwForget(e); return;
    }
  }

  /// <summary>
  /// To switch to the login user control.
  /// </summary>
  private void ToLogin(LinkLabelLinkClickedEventArgs e)
  {
    if (e.Button == MouseButtons.Left)
      ((ICommand)this.LlGetLogin?.Tag!).Execute(null);
  }

  /// <summary>
  /// To switch to the ToPwForget user control
  /// </summary>
  private void ToPwForget(LinkLabelLinkClickedEventArgs e)
  {
    if (e.Button == MouseButtons.Left)
      ((ICommand)this.LlPwForget?.Tag!).Execute(null);
  }

  private void OnClose()
  {
    var bs = this.BsUcRegist.DataSource as VmUcRegist;
    bs?.Dispose();
  }
}
