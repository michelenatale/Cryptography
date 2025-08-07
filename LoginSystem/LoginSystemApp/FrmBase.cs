
using System.Security.Cryptography;

namespace LoginSystemApp;

using michele.natale.LoginSystems;
using michele.natale.LoginSystems.Apps;
using michele.natale.Pointers;
using System.ComponentModel;

/// <summary>
/// The abstract class intended for the customer for
/// communication between MyFrmApp and loginSystem.
/// </summary>
public class FrmCustomerBase : Form
{
  public const int MASTER_PASSWORD_SIZE = 48;
  public const int MASTER_PASSWORD_MAX_SIZE = 128;

  private readonly RandomNumberGenerator Rand
    = RandomNumberGenerator.Create();

  public WindowsManager Login_System = null!;

  public UsIPtr<byte> MyMasterPassword = UsIPtr<byte>.Empty;

  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public AppLoginSettings App_Login_Setting { get; private set; } = null!;

  /// <summary>
  /// Intended for the C-Tor
  /// </summary>
  protected void FrmAppCustomerCTor()
  {
    var new_master_password = this.NewRngMasterPassword(MASTER_PASSWORD_SIZE);
    this.MyMasterPassword = new UsIPtr<byte>(new_master_password);
  }

  /// <summary>
  /// Vorgesehen für das Schliessen von MyFrmApp 
  /// </summary>
  protected void FrmAppCustomerClose()
  {
    if (!AppLoginSettings.IsNullOrEmpty(this.App_Login_Setting))
      this.App_Login_Setting.Reset();

    this.MyMasterPassword?.Dispose();
  }


  /// <summary>
  /// The method provided for the customer for 
  /// retrieving the AppLoginSettings protocol.
  /// </summary>
  /// <returns></returns>
  protected bool To_App_Customer_Login_Setting()
  {
    if (this.Login_System is not null)
    {
      if (this.Login_System.To_App_Login_Setting(out var result))
      {
        this.App_Login_Setting = result;
        if (result.MasterPassword.Length != 0)
          this.MyMasterPassword = new UsIPtr<byte>(this.App_Login_Setting.MasterPassword);

        return true;
      }
    }
    return false;
  }

  /// <summary>
  /// The method provided for the customer for 
  /// setting the AppLoginSettings protocol.
  /// </summary>
  /// <returns></returns>
  protected bool Set_App_Customer_Login_Setting()
  {
    if (this.Login_System is not null)
    {
      if (!AppLoginSettings.IsNullOrEmpty(this.App_Login_Setting))
      {
        var new_master_password = NewRngMasterPassword(MASTER_PASSWORD_SIZE);
        this.MyMasterPassword = new UsIPtr<byte>(new_master_password);

        this.App_Login_Setting.MasterPassword = this.MyMasterPassword.ToArray();
        if (this.Login_System.Set_App_Login_Setting(this.App_Login_Setting))
          return true;
      }
    }
    return false;
  }

  protected byte[] NewRngMasterPassword(int size)
  {
    var result = new byte[size];
    Rand.GetNonZeroBytes(result);
    return result;
  }

  /// <summary>
  /// For correctly disposing and saving the data in LoginSystem.
  /// </summary>
  protected void FrmCustomerDispose()
  {
    //see also Dispose(bool)
    this.Login_System.FrmClose();
  }
}