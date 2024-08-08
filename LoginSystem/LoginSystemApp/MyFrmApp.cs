


namespace LoginSystemApp;

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


public partial class MyFrmApp : FrmBase
{

  public MyFrmApp()
  {
    this.InitializeComponent();
    this.BtToLI.Enabled = true;
    this.BtSetLI.Enabled = false;

    this.FrmAppCTor();

    this.Icon = Resources.MyLogo64;
  }

  private void MyFrmApp_FormClosed(
    object sender, FormClosedEventArgs e)
  {
    this.FrmAppClose();
  }

  private void BtToLI_Click(object sender, EventArgs e)
  {
    this.TbMPw.Text = "[ ... ]";
    if (this.To_App_Login_Setting())
    {
      this.BtSetLI.Enabled = true;
      this.TbMPw.Text = Convert.ToHexString(this.MyMasterPassword.ToArray());
    }
  }

  private void BtSetLI_Click(object sender, EventArgs e)
  {
    this.TbMPw.Text = "[ ... ]";
    if (this.Set_App_Login_Setting())
      this.TbMPw.Text = Convert.ToHexString(this.MyMasterPassword.ToArray());
  }

}

