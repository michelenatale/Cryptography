

using System.Security.Cryptography;

namespace LoginSystemApp;

using michele.natale.LoginSystems;
using michele.natale.LoginSystems.Apps;
using michele.natale.Pointers;
using Properties;

//The AppLoginSettings class is your customer class.
//In this class, you can store the relevant information
//that ultimately makes up the login process.

//If only a Masterpassword is important to you as a
//login, then this can be built into the AppLoginSettings
//class. If it is other information, then set up the class
//according to your needs.

//The trick of the login procedure is very simple. If you are
//logged in, the AppLoginSettings class is also available
//(e.g. for your application). If you are not logged in,
//this class is not available.

//You can change the information you store in the AppLoginSettings
//class at any time and save it again.

//On the other hand, you can also close LoginSystem completely
//once you have the information in AppLoginSettings.


public partial class MyFrmApp : Form
{
  public const int MASTER_PASSWORD_SIZE = 48;
  public const int MASTER_PASSWORD_MAX_SIZE = 128;

  private readonly RandomNumberGenerator Rand
    = RandomNumberGenerator.Create();

  public WindowsManager Login_System = null!;
  public AppLoginSettings App_Login_Setting { get; private set; } = null!;

  public UsIPtr<byte> MyMasterPassword = UsIPtr<byte>.Empty;

  public MyFrmApp()
  {
    this.InitializeComponent();
    this.BtToLI.Enabled = true;
    this.BtSetLI.Enabled = false;

    var new_master_password = NewRngMasterPassword(MASTER_PASSWORD_SIZE);
    this.MyMasterPassword = new UsIPtr<byte>(new_master_password);

    this.Icon = Resources.MyLogo64;
  }

  private void MyFrmApp_FormClosed(
    object sender, FormClosedEventArgs e)
  {
    if (!AppLoginSettings.IsNullOrEmpty(this.App_Login_Setting))
      this.App_Login_Setting.Reset();

    this.MyMasterPassword?.Dispose();
  }

  private void BtToLI_Click(object sender, EventArgs e)
  {
    this.TbMPw.Text = "[ ... ]";
    if (this.Login_System is not null)
    {
      if (this.Login_System.To_App_Login_Setting(out var result))
      {
        this.App_Login_Setting = result;
        if (result.MasterPassword.Length != 0)
          this.MyMasterPassword = new UsIPtr<byte>(this.App_Login_Setting.MasterPassword);

        this.BtSetLI.Enabled = true;
        this.TbMPw.Text = Convert.ToHexString(this.MyMasterPassword.ToArray());
      }
    }
  }

  private void BtSetLI_Click(object sender, EventArgs e)
  {
    this.TbMPw.Text = "[ ... ]";
    if (this.Login_System is not null)
    {
      if (!AppLoginSettings.IsNullOrEmpty(this.App_Login_Setting))
      {
        var new_master_password = NewRngMasterPassword(MASTER_PASSWORD_SIZE);
        this.MyMasterPassword = new UsIPtr<byte>(new_master_password);

        this.App_Login_Setting.MasterPassword = this.MyMasterPassword.ToArray();
        if (this.Login_System.Set_App_Login_Setting(this.App_Login_Setting))
          this.TbMPw.Text = Convert.ToHexString(this.MyMasterPassword.ToArray());
      }
    }
  }

  private byte[] NewRngMasterPassword(int size)
  {
    var result = new byte[size];
    Rand.GetNonZeroBytes(result);
    return result;
  }

}
