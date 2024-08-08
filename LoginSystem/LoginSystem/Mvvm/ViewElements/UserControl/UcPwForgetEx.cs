

using System.Windows.Input;

namespace michele.natale.LoginSystems.Views;

using ViewModels;

/// <summary>
/// The UserControl that is displayed if the password has been forgotten.
/// </summary>
internal partial class UcPwForgetEx : UcPwForgetExBase //UserControl
{

  /// <summary>
  /// C-Tor
  /// </summary>
  public UcPwForgetEx()
  {
    this.InitializeComponent();
    this.InitializeBindings();

    this.CbDelivery.SelectedIndex = 0;

    //if (!File.Exists(DataSettingFile))
    //  this.LlPwForgetRegist.Enabled = true;
  }

  private void InitializeBindings()
  {
    this.SuspendLayout();
    this.BsUcPwForget.DataSource = typeof(VmUcPwForget);

    this.TbPw.DataBindings.Add(new Binding("Text", this.BsUcPwForget, "PassWord", true, DataSourceUpdateMode.OnPropertyChanged));
    this.TbRPw.DataBindings.Add(new Binding("Text", this.BsUcPwForget, "RPassWord", true, DataSourceUpdateMode.OnPropertyChanged));
    this.TbEmail.DataBindings.Add(new Binding("Text", this.BsUcPwForget, "EMail", true, DataSourceUpdateMode.OnPropertyChanged));

    this.TbPort.DataBindings.Add(new Binding("Text", this.BsUcPwForget, "Port", true, DataSourceUpdateMode.OnPropertyChanged));
    this.TbHost.DataBindings.Add(new Binding("Text", this.BsUcPwForget, "Host", true, DataSourceUpdateMode.OnPropertyChanged));
    this.TbUsername.DataBindings.Add(new Binding("Text", this.BsUcPwForget, "UserName", true, DataSourceUpdateMode.OnPropertyChanged));

    this.CbSsl.DataBindings.Add(new Binding("Checked", this.BsUcPwForget, "IsSsl", true, DataSourceUpdateMode.OnPropertyChanged));
    this.CbDelivery.DataBindings.Add(new Binding("SelectedItem", this.BsUcPwForget, "Delivery", true, DataSourceUpdateMode.OnPropertyChanged));

    this.LlLogin.DataBindings.Add(new Binding("Tag", this.BsUcPwForget, "IcLlLogin", true, DataSourceUpdateMode.OnPropertyChanged));
    this.LlRegist.DataBindings.Add(new Binding("Tag", this.BsUcPwForget, "IcLlRegist", true, DataSourceUpdateMode.OnPropertyChanged));

    this.BtOk.DataBindings.Add(new Binding("Command", this.BsUcPwForget, "IcBtPwForget", true, DataSourceUpdateMode.OnPropertyChanged));
    this.BtCancel.DataBindings.Add(new Binding("Command", this.BsUcPwForget, "IcBtCancel", true, DataSourceUpdateMode.OnPropertyChanged));
    this.ResumeLayout(true);

  }


  /// <summary>
  /// Preparation before the form is set up.
  /// </summary>
  /// <param name="sender"></param>
  /// <param name="e"></param>
  private void UcPwForget_Load(object sender, EventArgs e)
  {

    MessageBox.Show(ToMsgBoxText, "Login-SYSTEM",
      MessageBoxButtons.OK, MessageBoxIcon.Information);

    this.CbSsl.Checked = true;
  }

  /// <summary>
  /// The click events used in this UserControl.
  /// </summary>
  /// <param name="sender">Sender from Event</param>
  /// <param name="e">The parameters of the event.</param>
  private void UcPwForget_Click(object sender, LinkLabelLinkClickedEventArgs e)
  {
    switch (sender)
    {
      case var obj when obj == this.LlRegist: this.ToRegist(e); return;
      case var obj when obj == this.LlLogin: this.ToLogin(e); return;
    }
  }

  /// <summary>
  /// For changing the UserControl.
  /// </summary>
  private void ToRegist(LinkLabelLinkClickedEventArgs e)
  {
    if (e.Button == MouseButtons.Left)
      ((ICommand)this.LlRegist?.Tag!).Execute(null);
  }

  /// <summary>
  /// For changing the UserControl
  /// </summary>
  private void ToLogin(LinkLabelLinkClickedEventArgs e)
  {
    if (e.Button == MouseButtons.Left)
      ((ICommand)this.LlLogin?.Tag!).Execute(null);
  }

  private void OnClose()
  {
    var bs = this.BsUcPwForget.DataSource as VmUcPwForget;
    bs?.Dispose();
  }

}
