﻿
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
      var folder = Path.Combine(ToCurrentFolder, Current_Config_Folder);
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
      var folder = Path.Combine(ToCurrentFolder, Current_Config_Folder);
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
      var folder = Path.Combine(ToCurrentFolder, Current_Config_Folder);
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
      var folder = Path.Combine(ToCurrentFolder, Current_Config_Folder);
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
      var folder = Path.Combine(ToCurrentFolder, Current_Config_Folder);
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
    SaveToFileBytesUtf8(StartConfigFile, SerializeJson(data));
  }

  /// <summary>
  /// Save the Set Start Configuration
  /// </summary>
  /// <param name="fcrt"></param>
  public void SetStartConfiguration(
    FrmCommands fcrt)
  {
    var data = new StartSettings(fcrt);
    SaveToFileBytesUtf8(StartConfigFile, SerializeJson(data));
  }

  /// <summary>
  /// Load Start Configuration
  /// </summary>
  /// <returns></returns>
  public StartSettings ToStartConfiguration()
  {
    if (File.Exists(StartConfigFile))
      return DeserializeJson<StartSettings>(
        LoadFromFileBytesUtf8(StartConfigFile))!;
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
    SaveToFileBytesUtf8(SaltSettingFile, SerializeJson(data.ToArray()));
  }

  /// <summary>
  /// Save  Salt Settings
  /// </summary>
  /// <param name="data"></param>
  public void SetSaltSetting(byte[][] data)
  {
    SaveToFileBytesUtf8(SaltSettingFile, SerializeJson(data));
  }

  /// <summary>
  /// Load salt Settings
  /// </summary>
  /// <returns></returns>
  public byte[][] ToSaltSetting()
  {
    if (File.Exists(SaltSettingFile))
      return DeserializeJson<byte[][]>(
        LoadFromFileBytesUtf8(SaltSettingFile))!;
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
    var data = EncryptionAesGcm(SerializeJson(settings), key.ToArray(), associated);
    SaveToFileBytesUtf8(DataSettingFile, data);
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
    SetDataSetting(settings, k, associated);
  }

  /// <summary>
  /// Load Data Settings
  /// </summary>
  /// <returns></returns>
  public AppLoginSettings ToDataSetting()
  {
    if (File.Exists(DataSettingFile))
      return DeserializeJson<AppLoginSettings>(
        LoadFromFileBytesUtf8(DataSettingFile))!;
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
    return ToDataSetting(k, associated);
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
    if (File.Exists(DataSettingFile))
      return DeserializeJson<AppLoginSettings>(
        DecryptionAesGcm(LoadFromFileBytesUtf8(DataSettingFile), key.ToArray(), associated))!;
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
    SetRTable(size);
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
    SaveToFileBytesUtf8(RTableSettingFile, SerializeJson(data));
  }

  /// <summary>
  /// Load RTable
  /// </summary>
  /// <returns></returns>
  public byte[] ToRTableValue()
  {
    var rt = LoadRTable();
    var size = rt.Length;
    var start = size / 3 - 1;

    var result = new List<byte> { rt[start] };
    while (start > 1)
    {
      start = (start & 1) == 0 ? start / 2 : 3 * start + 1;
      result.Add(rt[start % size]);
    }
    return HashDataAlgo(result.ToArray(), DEFAULT_H_NAME);
  }

  /// <summary>
  /// Load RTable
  /// </summary>
  /// <returns></returns>
  private byte[] LoadRTable()
  {
    if (File.Exists(RTableSettingFile))
      return DeserializeJson<byte[]>(
        LoadFromFileBytesUtf8(RTableSettingFile))!;
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
    var sypw = new_sypw ? CreateLoginSecretData(sdi) : sypassword.ToArray();
    var (unpw, empw) = sdi!.ToHashes(HashAlgorithmName.SHA256);
    sdi.Reset();

    //Die komplette SecureDataInfo wird abgespeichert.
    var a = EncryptionChaCha20Poly1305(scd.ToArray(), unpw, associated);
    var b = EncryptionChaCha20Poly1305(scd.ToArray(), empw, associated);
    var c = EncryptionChaCha20Poly1305(scd.ToArray(), sypw, associated);
    ClearPrimitives(unpw, empw, sypw);
    SaveToFileBytesUtf8(MPwSettingFile, SerializeJson<byte[][]>([a, b, c]));
  }

  /// <summary>
  /// Load Masterpassword
  /// </summary>
  /// <returns></returns>
  public byte[][] ToMPwSettings()
  {
    if (File.Exists(MPwSettingFile))
      return DeserializeJson<byte[][]>(
        LoadFromFileBytesUtf8(MPwSettingFile))!;
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
    return TryToMPwSetting(k, associated, out result);
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
    foreach (var mpw in ToMPwSettings())
    {
      try
      {
        var decipher = DecryptionChaCha20Poly1305(mpw, key.ToValues(), associated);
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
    var cfolder = ToCurrentFolder;
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

    ShowMessageSecretFile(filename);

    return Encoding.UTF8.GetBytes(ls_spw_f);
  }

  /// <summary>
  /// LoginSystemText
  /// </summary>
  public static string LoginSystemText =>
    "© LoginSystem 2024 - Created by © Michele Natale 2024";

  #endregion MPW Config 
}