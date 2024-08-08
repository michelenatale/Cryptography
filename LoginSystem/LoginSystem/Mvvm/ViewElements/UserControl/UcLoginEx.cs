


using System.Windows.Input;

namespace michele.natale.LoginSystems.Views;

using ViewModels;

/// <summary>
/// The UserControl for logging in.
/// </summary>
internal partial class UcLoginEx : UserControl
{

  /// <summary>
  /// C-Tor
  /// </summary>
  public UcLoginEx()
  {
    this.InitializeComponent();
    this.InitializeBindings();

    //if (!File.Exists(DataSettingFile))
    //  this.LlRegist.Enabled = true;

  }

  private void InitializeBindings()
  {
    this.SuspendLayout();
    this.BsUcLogin.DataSource = typeof(VmUcLogin);
    this.TbPw.DataBindings.Add(new Binding("Text", this.BsUcLogin, "PassWord", true, DataSourceUpdateMode.OnPropertyChanged));
    this.TbUserName.DataBindings.Add(new Binding("Text", this.BsUcLogin, "UserName", true, DataSourceUpdateMode.OnPropertyChanged));

    this.LlRegist.DataBindings.Add(new Binding("Tag", this.BsUcLogin, "IcLlRegist", true, DataSourceUpdateMode.OnPropertyChanged));
    this.LlPwForget.DataBindings.Add(new Binding("Tag", this.BsUcLogin, "IcLlPwForget", true, DataSourceUpdateMode.OnPropertyChanged));

    this.BtLogin.DataBindings.Add(new Binding("Command", this.BsUcLogin, "IcBtLogin", true, DataSourceUpdateMode.OnPropertyChanged));
    this.BtCancel.DataBindings.Add(new Binding("Command", this.BsUcLogin, "IcBtCancel", true, DataSourceUpdateMode.OnPropertyChanged));
    this.ResumeLayout(true);
  }

  /// <summary>
  /// All click events are made available to you by the UserControl.
  /// </summary>
  /// <param name="sender">Der Sender des Events.</param>
  /// <param name="e">The event parameters</param>
  private void UcLogin_Click(object sender, LinkLabelLinkClickedEventArgs e)
  {
    switch (sender)
    {
      case var obj when obj == this.LlRegist: this.ToRegist(e); return;
      case var obj when obj == this.LlPwForget: this.ToPwForget(e); return;
    }
  }

  /// <summary>
  /// Causes the UserControl required for registration to be loaded.
  /// </summary>
  private void ToRegist(LinkLabelLinkClickedEventArgs e)
  {
    if (e.Button == MouseButtons.Left)
      ((ICommand)this.LlRegist?.Tag!).Execute(null);
  }

  /// <summary>
  /// Causes the UserControl required for the PwForget to be loaded.
  /// </summary>
  private void ToPwForget(LinkLabelLinkClickedEventArgs e)
  {
    if (e.Button == MouseButtons.Left)
      ((ICommand)this.LlPwForget?.Tag!).Execute(null);
  }

  private void OnClose()
  {
    var bs = this.BsUcLogin.DataSource as VmUcLogin;
    bs?.Dispose();
  }
}
