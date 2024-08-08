

using System.Security.Cryptography;
using System.Text;


//Here is a list with the provider information
//https://www.patrick-canterino.de/pop3-smtp/



namespace michele.natale.LoginSystems.Services;

using Models;
using Pointers;

partial class AppServices
{
  private UsIPtr<byte> PKey = UsIPtr<byte>.Empty;

  /// <summary>
  /// Specifies what must be done with AppLoginSettings.
  /// </summary>
  /// <param name="rdi">RegistDataInfo</param>
  public void ToRegistDataInfo(RegistDataInfo rdi)
  {
    switch (rdi.ASInfo)
    {
      //No Activities
      case AppSettingsInfo.None: return;

      //Registration
      case AppSettingsInfo.SetRegistData: SetRegistData(rdi); return;

      //Login
      case AppSettingsInfo.CheckRegistData: CheckRegistData(rdi); return;

      //PwForget
      case AppSettingsInfo.SetNewRegistData: SetNewRegistData(rdi); return;

      //PwChange
      case AppSettingsInfo.ChangeRegistData: ChangeRegistData(rdi); return;

      //Get AppLoginSettings
      case AppSettingsInfo.GetAppLoginSetting: ToAppLoginSetting(rdi); return;

      //Set AppLoginSettings
      case AppSettingsInfo.SetAppLoginSetting: SetAppLoginSetting(rdi); return;
    }
  }


  /// <summary>
  /// A registration is made. 
  /// </summary>
  /// <param name="rdi">RegistDataInfo</param>
  private static void SetRegistData(RegistDataInfo rdi)
  {
    rdi.CorrectExecution = true;
    using var data = rdi.Tag as UsIPtr<byte>;
    if (data is not null)
    {
      AppServicesHolder.SetMPwSetting(data, [], [], true);

      //// 2 special tests that must be passed.
      //ChangeSySetting(); //works :-)
      //TryCheckSetMPwSettings(data); //works

      var app_data = new AppLoginSettings
      {
        FirstTimeStamp = DateTimeOffset.UtcNow.Ticks
      };
      var sci = new SecureDataInfo(data);
      var (_, empw) = sci!.ToHashes(HashAlgorithmName.SHA256);
      sci.Reset();

      if (AppServicesHolder.TryToMPwSetting(empw, null, out var mpw))
      {
        using var pw = mpw;
        AppServicesHolder.SetDataSetting(app_data, pw, null);
        return;
      }
    }
    rdi.CorrectExecution = false;
  }

  /// <summary>
  /// A Login is made.
  /// </summary>
  /// <param name="rdi">RegistDataInfo</param>
  private void CheckRegistData(RegistDataInfo rdi)
  {
    rdi.AppSettings = null!;
    rdi.CorrectExecution = true;
    using var data = rdi.Tag as UsIPtr<byte>;
    if (data is not null)
    {
      var sci = new SecureDataInfo(data);
      var (unpw, empw) = sci!.ToHashes(HashAlgorithmName.SHA256);
      sci.Reset();

      var k = IsNullOrEmpty(empw) ? unpw : empw;
      if (TryToMPwSetting(k, null, out var mpw))
      {
        PKey = new UsIPtr<byte>(k);
        var app_data = ToDataSetting(mpw, null);
        app_data.LastTimeStamp = DateTimeOffset.UtcNow.Ticks;

        SetDataSetting(app_data, mpw, null);
        rdi.AppSettings = app_data;
        return;
      }
    }
    rdi.CorrectExecution = false;
  }

  /// <summary>
  /// A new password is assigned
  /// </summary>
  /// <param name="rdi">RegistDataInfo</param>
  /// <param name="associated">Associated for Cryption</param>
  private void SetNewRegistData(
    RegistDataInfo rdi, ReadOnlySpan<byte> associated = default)
  {
    rdi.CorrectExecution = true;
    using var data = rdi.Tag as UsIPtr<byte>;
    if (data is not null)
    {
      var str_sypw = ToInputBox(ToInputBoxText);
      if (!string.IsNullOrEmpty(str_sypw))
      {
        var backupfile = $"{MPwSettingFile}.backup";
        File.Copy(MPwSettingFile, backupfile, true);
        var sypw = Encoding.UTF8.GetBytes(str_sypw);
        var mpws = ToMPwSettings();
        using var mpw_full = new UsIPtr<byte>(
          DecryptionChaCha20Poly1305(mpws[2], sypw, associated));

        var emi = new EmailMsgInfo(data);
        var sdi = new SecureDataInfo(mpw_full);
        if (sdi is not null)
        {
          if (Encoding.UTF8.GetBytes(emi.To).SequenceEqual(sdi.EMail))
          {
            sdi.Password = new UsIPtr<byte>(Encoding.UTF8.GetBytes(new Guid(emi.MGuid).ToString()));
            SetMPwSetting(sdi.Serialize(), default, sypw);
            ClearPrimitives(sypw);

            // TEST NOCH MACHEN
            //ChangeSySetting(); //works :-)
            //TryCheckSetMPwSettings(data);

            var app_data = ToDataSetting(sdi.MPw, null);
            app_data.LastTimeStamp = DateTimeOffset.UtcNow.Ticks;
            SetDataSetting(app_data, sdi.MPw, null);
            if (SendEmailMessage(emi))
            {
              emi.Reset(); sdi.Reset();
              if (File.Exists(backupfile))
                File.Delete(backupfile);
              return;
            }
          }
          emi?.Reset(); sdi?.Reset();
          File.Copy(backupfile, MPwSettingFile, true);
          if (File.Exists(backupfile))
            File.Delete(backupfile);
        }
      }
      rdi.CorrectExecution = false;
    }
  }

  /// <summary>
  /// The existing password is changed.
  /// </summary>
  /// <param name="rdi">RegistDataInfo</param>
  /// <param name="associated">Associated for Cryption</param>
  private void ChangeRegistData(
    RegistDataInfo rdi, ReadOnlySpan<byte> associated = default)
  {
    rdi.CorrectExecution = true;
    using var data = rdi.Tag as UsIPtr<byte>;
    if (data is not null)
    {
      var str_sypw = ToInputBox(ToInputBoxText);
      if (str_sypw is not null)
      {
        var sypw = Encoding.UTF8.GetBytes(str_sypw);
        var mpws = ToMPwSettings();
        using var mpw_full = new UsIPtr<byte>(
          DecryptionChaCha20Poly1305(mpws[2], sypw, associated));

        var sdi = new SecureDataInfo(mpw_full);
        if (sdi is not null)
        {
          sdi.Password = data.Copy;
          SetMPwSetting(sdi.Serialize(), default, sypw);

          var app_data = ToDataSetting(sdi.MPw, null);
          app_data.LastTimeStamp = DateTimeOffset.UtcNow.Ticks;
          SetDataSetting(app_data, sdi.MPw, null);

          rdi.Tag = app_data;
          sdi.Reset();
        }
      }
    }
  }

  /// <summary>
  /// Get to AppLoginSetting
  /// </summary>
  /// <param name="rdi">RegistDataInfo</param>
  private void ToAppLoginSetting(RegistDataInfo rdi)
  {
    rdi.CorrectExecution = true;
    if (PKey is not null && !PKey.IsEmpty)
      if (TryToMPwSetting(PKey!, null, out var mpw))
      {
        rdi.AppSettings = ToDataSetting(mpw, null);
        return;
      }
    rdi.CorrectExecution = false;
  }

  /// <summary>
  /// Set a new AppLoginSetting
  /// </summary>
  /// <param name="rdi">RegistDataInfo</param>
  private void SetAppLoginSetting(RegistDataInfo rdi)
  {
    rdi.CorrectExecution = true;
    if (PKey is not null && !PKey.IsEmpty)
      if (TryToMPwSetting(PKey!, null, out var mpw))
      {
        rdi.AppSettings.LastTimeStamp = DateTimeOffset.UtcNow.Ticks;
        SetDataSetting(rdi.AppSettings, mpw, null);
        rdi.AppSettings = null!;
        return;
      }
    rdi.CorrectExecution = false;
  }


  /// <summary>
  /// MessageText
  /// </summary>
  /// <returns></returns>
  private static string ToMsgText() =>
    MsgText(Guid.NewGuid());

  /// <summary>
  /// Messagetext with Guid
  /// </summary>
  /// <param name="guid">Guid</param>
  /// <returns></returns>
  private static string MsgText(Guid guid)
  {
    var result = new StringBuilder();
    result.AppendLine("A new password has been created.");
    result.AppendLine(guid.ToString());
    result.AppendLine();
    result.AppendLine("Please change your password soon. Thank you.");
    result.AppendLine();
    result.AppendLine("Greetings © LoginSystem 2024");
    return result.ToString();
  }

}
