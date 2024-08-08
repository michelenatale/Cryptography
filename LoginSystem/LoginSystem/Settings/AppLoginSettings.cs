

namespace michele.natale.LoginSystems;

using Services;

/// <summary>
/// <para>The secret customer information that © LoginSystem 2024 keeps for you.</para>
/// © LoginSystem 2024 - Created by © Michele Natale 2024.
/// </summary>
public sealed class AppLoginSettings
{
  /// <summary>
  /// Empty Variable.
  /// </summary>
  public static readonly AppLoginSettings Empty = new();

  /// <summary>
  /// The last login process that was made with © LoginSystem 2024.
  /// </summary>
  public long LastTimeStamp { get; set; } = -1;

  /// <summary>
  /// The first login process that was made with © LoginSystem 2024.
  /// </summary>
  public long FirstTimeStamp { get; set; } = -1;

  /// <summary>
  /// Author from © LoginSystem 2024.
  /// </summary>
  public string Author { get; set; } = "© Michele Natale 2024";

  /// <summary>
  /// The Name of © LoginSystem 2024
  /// </summary>
  public string LoginSystemName { get; set; } = "© LoginSystem 2024";


  /// <summary>
  /// Your Username. 
  /// You can enter your Username from this, or 
  /// another system directly here if you need it.
  /// </summary>
  public string UserName { get; set; } = string.Empty;

  /// <summary>
  /// Your MasterPassword. 
  /// Enter your desired Masterpassword here. Once you 
  /// have Logged In, this class, and therefore your 
  /// Masterpassword, will be available to you again.
  /// </summary>
  public byte[] MasterPassword { get; set; } = [];



  // ********** ********** ********** ********** **********
  // ********** ********** ********** ********** **********
  //  If you would like to keep certain information
  //  protected, you can enter it here.

  //  This class is strongly encrypted. To decrypt this
  //  class, you must follow the login procedure of
  //  © LoginSystem 2024.
  // ********** ********** ********** ********** **********
  // ********** ********** ********** ********** **********


  //public byte[] MacAddress { get; set; } = []; 

  //public byte[] PasswordKey1 { get; set; } = [];
  //public byte[] PasswordKey2 { get; set; } = [];

  //Public string YourSystemName { get; set; } = string.empty;

  // ....
  // ....


  public void Reset()
  {
    if (!string.IsNullOrEmpty(this.UserName))
      AppServices.AppServicesHolder.ResetText(this.UserName);

    if (AppServices.AppServicesHolder.IsNullOrEmpty(this.MasterPassword))
      AppServices.AppServicesHolder.ClearPrimitives(this.MasterPassword);

    // ...
  }


  public static bool IsNullOrEmpty(AppLoginSettings app_settings)
  {
    if (app_settings is null) return true;
    return app_settings == Empty;
  }
}
