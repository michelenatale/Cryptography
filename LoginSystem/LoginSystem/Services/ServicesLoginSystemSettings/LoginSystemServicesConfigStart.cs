
using System.Security.Cryptography;
using System.Text;

namespace michele.natale.LoginSystems.Services;

using Models;
using Pointers;
using Settings;

partial class AppServices
{

  #region Variables

  /// <summary>
  /// Current_Folder = "LoginSystems"
  /// </summary>
  public const string Current_Folder = "LoginSystems";

  /// <summary>
  /// Current_Config_Folder = "LoginSystems/Config"
  /// </summary>
  public const string Current_Config_Folder = "LoginSystems/Config";

  /// <summary>
  /// Current_Frm_MPw_Setting_File = "mpwsettings"
  /// </summary>
  public const string Current_Frm_MPw_Setting_File = "mpwsettings";

  /// <summary>
  /// Current_Frm_Salt_Setting_File = "saltsettings"
  /// </summary>
  public const string Current_Frm_Salt_Setting_File = "saltsettings";

  /// <summary>
  /// Current_Frm_Data_Setting_File = "datasettings"
  /// </summary>
  public const string Current_Frm_Data_Setting_File = "datasettings";

  /// <summary>
  /// Current_Frm_Start_Setting_File = "startsettings"
  /// </summary>
  public const string Current_Frm_Start_Setting_File = "startsettings";

  /// <summary>
  /// Current_Frm_RTable_Setting_File = "rtablesettings"
  /// </summary>
  public const string Current_Frm_RTable_Setting_File = "rtablesettings";

  #endregion Variables

  #region Config File Adress

  /// <summary>
  /// Return the path of the Start Config File
  /// </summary>
  public string StartConfigFile
  {
    get
    {
      var folder = Path.Combine(this.ToCurrentFolder, Current_Config_Folder);
      var file = Path.Combine(folder, Current_Frm_Start_Setting_File);
      CreateFolder(folder);
      return file;
    }
  }

  /// <summary>
  /// Return the path of Salt Settinh File
  /// </summary>
  public string SaltSettingFile
  {
    get
    {
      var folder = Path.Combine(this.ToCurrentFolder, Current_Config_Folder);
      var file = Path.Combine(folder, Current_Frm_Salt_Setting_File);
      CreateFolder(folder);
      return file;
    }
  }

  /// <summary>
  /// Return the path of DataSetting File
  /// </summary>
  public string DataSettingFile
  {
    get
    {
      var folder = Path.Combine(this.ToCurrentFolder, Current_Config_Folder);
      var file = Path.Combine(folder, Current_Frm_Data_Setting_File);
      CreateFolder(folder);
      return file;
    }
  }

  /// <summary>
  /// Return the pat of RTable Setting File
  /// </summary>
  public string RTableSettingFile
  {
    get
    {
      var folder = Path.Combine(this.ToCurrentFolder, Current_Config_Folder);
      var file = Path.Combine(folder, Current_Frm_RTable_Setting_File);
      CreateFolder(folder);
      return file;
    }
  }

  /// <summary>
  /// Return the path of the Masterpasswords File.
  /// </summary>
  public string MPwSettingFile
  {
    get
    {
      var folder = Path.Combine(this.ToCurrentFolder, Current_Config_Folder);
      var file = Path.Combine(folder, Current_Frm_MPw_Setting_File);
      CreateFolder(folder);
      return file;
    }
  }
  #endregion Config File Adress

  #region Start Config

  /// <summary>
  /// Save the Set Default Start Configuration 
  /// </summary>
  public void SetStartDefaultConfiguration()
  {
    var data = new StartSettings();
    this.SaveToFileBytesUtf8(this.StartConfigFile, this.SerializeJson(data));
  }

  /// <summary>
  /// Save the Set Start Configuration
  /// </summary>
  /// <param name="fcrt"></param>
  public void SetStartConfiguration(
    FrmCommands fcrt)
  {
    var data = new StartSettings(fcrt);
    this.SaveToFileBytesUtf8(this.StartConfigFile, this.SerializeJson(data));
  }

  /// <summary>
  /// Load Start Configuration
  /// </summary>
  /// <returns></returns>
  public StartSettings ToStartConfiguration()
  {
    if (File.Exists(this.StartConfigFile))
      return this.DeserializeJson<StartSettings>(
        this.LoadFromFileBytesUtf8(this.StartConfigFile))!;
    return default!;
  }
  #endregion Start Config

  #region Salt Config

  /// <summary>
  /// Save Salt Settigs 
  /// </summary>
  public void SetSaltSetting()
  {
    var data = new byte[][]
    {
       Guid.NewGuid().ToByteArray(),
       Guid.NewGuid().ToByteArray(),
    }.ToList();
    this.SaveToFileBytesUtf8(this.SaltSettingFile, this.SerializeJson(data.ToArray()));
  }

  /// <summary>
  /// Save  Salt Settings
  /// </summary>
  /// <param name="data"></param>
  public void SetSaltSetting(byte[][] data)
  {
    this.SaveToFileBytesUtf8(this.SaltSettingFile, this.SerializeJson(data));
  }

  /// <summary>
  /// Load salt Settings
  /// </summary>
  /// <returns></returns>
  public byte[][] ToSaltSetting()
  {
    if (File.Exists(this.SaltSettingFile))
      return this.DeserializeJson<byte[][]>(
        this.LoadFromFileBytesUtf8(this.SaltSettingFile))!;
    return default!;
  }
  #endregion Salt Config

  #region Data Config

  /// <summary>
  /// Save Set Data Settings 
  /// </summary>
  /// <param name="settings"></param>
  /// <param name="key"></param>
  /// <param name="associated"></param>
  public void SetDataSetting(
    AppLoginSettings settings, UsIPtr<byte> key, ReadOnlySpan<byte> associated)
  {
    var data = this.EncryptionAesGcm(this.SerializeJson(settings), key.ToArray(), associated);
    this.SaveToFileBytesUtf8(this.DataSettingFile, data);
  }

  /// <summary>
  /// Save Set Data Settings 
  /// </summary>
  /// <param name="settings"></param>
  /// <param name="key"></param>
  /// <param name="associated"></param>
  public void SetDataSetting(
    AppLoginSettings settings, ReadOnlySpan<byte> key, ReadOnlySpan<byte> associated)
  {
    using var k = new UsIPtr<byte>(key.ToArray());
    this.SetDataSetting(settings, k, associated);
  }

  /// <summary>
  /// Load Data Settings
  /// </summary>
  /// <returns></returns>
  public AppLoginSettings ToDataSetting()
  {
    if (File.Exists(this.DataSettingFile))
      return this.DeserializeJson<AppLoginSettings>(
        this.LoadFromFileBytesUtf8(this.DataSettingFile))!;
    return default!;
  }

  /// <summary>
  /// Load Data Settings
  /// </summary>
  /// <param name="key"></param>
  /// <param name="associated"></param>
  /// <returns></returns>
  public AppLoginSettings ToDataSetting(
    ReadOnlySpan<byte> key, ReadOnlySpan<byte> associated)
  {
    using var k = new UsIPtr<byte>(key.ToArray());
    return this.ToDataSetting(k, associated);
  }

  /// <summary>
  /// Load Data Setting
  /// </summary>
  /// <param name="key"></param>
  /// <param name="associated"></param>
  /// <returns></returns>
  public AppLoginSettings ToDataSetting(
    UsIPtr<byte> key, ReadOnlySpan<byte> associated)
  {
    if (File.Exists(this.DataSettingFile))
      return this.DeserializeJson<AppLoginSettings>(
        this.DecryptionAesGcm(this.LoadFromFileBytesUtf8(this.DataSettingFile), key.ToArray(), associated))!;
    return default!;
  }

  #endregion Data Config

  #region RTable Config

  /// <summary>
  /// Save new RTable
  /// </summary>
  public void SetRTable()
  {
    var rand = RandomHolder.Instance;
    var size = rand.NextInt32(1200, 2500);
    this.SetRTable(size);
  }

  /// <summary>
  /// Save new RTable
  /// </summary>
  /// <param name="size"></param>
  public void SetRTable(int size)
  {
    var data = new byte[size];
    var rand = RandomHolder.Instance;
    rand.FillBytes(data);
    this.SaveToFileBytesUtf8(this.RTableSettingFile, this.SerializeJson(data));
  }

  /// <summary>
  /// Load RTable
  /// </summary>
  /// <returns></returns>
  public byte[] ToRTableValue()
  {
    var rt = this.LoadRTable();
    var size = rt.Length;
    var start = size / 3 - 1;

    var result = new List<byte> { rt[start] };
    while (start > 1)
    {
      start = (start & 1) == 0 ? start / 2 : 3 * start + 1;
      result.Add(rt[start % size]);
    }
    return this.HashDataAlgo(result.ToArray(), DEFAULT_H_NAME);
  }

  /// <summary>
  /// Load RTable
  /// </summary>
  /// <returns></returns>
  private byte[] LoadRTable()
  {
    if (File.Exists(this.RTableSettingFile))
      return this.DeserializeJson<byte[]>(
        this.LoadFromFileBytesUtf8(this.RTableSettingFile))!;
    return default!;
  }
  #endregion RTable  Config

  #region MPw Config

  /// <summary>
  /// Set a new Masterpassword
  /// </summary>
  /// <param name="scd">SecureDataInfo</param>
  /// <param name="associated">Associated</param>
  /// <param name="sypassword">Systempassword</param>
  /// <param name="new_sypw">New Systempassword</param>
  public void SetMPwSetting(
    UsIPtr<byte> scd, ReadOnlySpan<byte> associated,
    ReadOnlySpan<byte> sypassword, bool new_sypw = false)
  {
    var sdi = new SecureDataInfo(scd);
    var sypw = new_sypw ? this.CreateLoginSecretData(sdi) : sypassword.ToArray();
    var (unpw, empw) = sdi!.ToHashes(HashAlgorithmName.SHA256);
    sdi.Reset();

    //Die komplette SecureDataInfo wird abgespeichert.
    var a = this.EncryptionChaCha20Poly1305(scd.ToArray(), unpw, associated);
    var b = this.EncryptionChaCha20Poly1305(scd.ToArray(), empw, associated);
    var c = this.EncryptionChaCha20Poly1305(scd.ToArray(), sypw, associated);
    this.ClearPrimitives(unpw, empw, sypw);
    this.SaveToFileBytesUtf8(this.MPwSettingFile, this.SerializeJson<byte[][]>([a, b, c]));
  }

  /// <summary>
  /// Load Masterpassword
  /// </summary>
  /// <returns></returns>
  public byte[][] ToMPwSettings()
  {
    if (File.Exists(this.MPwSettingFile))
      return this.DeserializeJson<byte[][]>(
        this.LoadFromFileBytesUtf8(this.MPwSettingFile))!;
    return default!;
  }

  /// <summary>
  /// Check Masterpassword Settings
  /// </summary>
  /// <param name="key"></param>
  /// <param name="associated"></param>
  /// <param name="result"></param>
  /// <returns></returns>
  public bool TryToMPwSetting(
    ReadOnlySpan<byte> key, ReadOnlySpan<byte> associated, out UsIPtr<byte> result)
  {
    using var k = new UsIPtr<byte>(key.ToArray());
    return this.TryToMPwSetting(k, associated, out result);
  }

  /// <summary>
  /// Check Masterpassword Settings
  /// </summary>
  /// <param name="key"></param>
  /// <param name="associated"></param>
  /// <param name="result"></param>
  /// <returns></returns>
  public bool TryToMPwSetting(
    UsIPtr<byte> key, ReadOnlySpan<byte> associated, out UsIPtr<byte> result)
  {
    result = UsIPtr<byte>.Empty;
    foreach (var mpw in this.ToMPwSettings())
    {
      try
      {
        var decipher = this.DecryptionChaCha20Poly1305(mpw, key.ToValues(), associated);
        if (decipher is not null)
        {
          using var ptr = new UsIPtr<byte>(decipher);
          var sci = new SecureDataInfo(ptr);
          result = sci.MPw.Copy;
          sci.Reset();
          return true;
        }
      }
      catch (Exception)
      {
      }
    }
    return false;
  }

  /// <summary>
  /// Create Login Secret Data
  /// </summary>
  /// <param name="data"></param>
  /// <returns></returns>
  private byte[] CreateLoginSecretData(SecureDataInfo data)
  {
    var cfolder = this.ToCurrentFolder;
    var email = Encoding.UTF8.GetString(data.EMail);
    var password = Encoding.UTF8.GetString(data.Password.ToArray());
    var username = Encoding.UTF8.GetString(data.Username);
    var filename = Path.Combine(cfolder, "LoginSystemSecretData.txt");
    var ls_spw_f = RandomHolder.Instance.RngAlphaString(32);

    var sb = new StringBuilder();
    sb.AppendLine(LoginSystemText);
    sb.AppendLine(); sb.AppendLine();
    sb.AppendLine("Please keep this file in a safe place. Always keep this information secret!");
    sb.AppendLine(); sb.AppendLine();
    sb.AppendLine($"Username:\t{username}");
    sb.AppendLine($"Password:\t{password}");
    sb.AppendLine($"EMail:\t\t{email}");
    sb.AppendLine();
    sb.AppendLine($"LS-SPW-F:\t{ls_spw_f}");
    sb.AppendLine(); sb.AppendLine();
    File.WriteAllText(filename, sb.ToString());
    sb.Clear();

    this.ShowMessageSecretFile(filename);

    return Encoding.UTF8.GetBytes(ls_spw_f);
  }

  /// <summary>
  /// LoginSystemText
  /// </summary>
  public static string LoginSystemText =>
    "© LoginSystem 2024 - Created by © Michele Natale 2024";

  #endregion MPW Config 
}
