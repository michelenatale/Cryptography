
using System.Text;
using System.Net.Mail;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace michele.natale.LoginSystems;

using Pointers;
using System;
using static Services.AppServices;

/// <summary>
/// Contains all information to send an email
/// </summary>
internal sealed class EmailMsgInfo
{
  //Here is a list with the provider information
  //https://www.patrick-canterino.de/pop3-smtp/

  public int Port { get; set; } = -1;
  public bool IsSsl { get; set; } = true;
  public string To { get; set; } = string.Empty;
  public string From { get; set; } = string.Empty;
  public string Body { get; set; } = string.Empty;
  public string Host { get; set; } = string.Empty;
  public string Subject { get; set; } = string.Empty;
  public string Username { get; set; } = string.Empty;
  public byte[] MGuid { get; set; } = [];
  public UsIPtr<byte> Password { get; set; } = UsIPtr<byte>.Empty;
  public SmtpDeliveryMethod SmtpMethode { get; set; } = SmtpDeliveryMethod.Network;

  [JsonIgnore]
  public static readonly EmailMsgInfo Empty = new();

  [JsonIgnore]
  public bool IsDisposed { get; private set; } = false;

  public EmailMsgInfo()
  {

  }

  public EmailMsgInfo(UsIPtr<byte> data)
    : this(ToPtrDeserialize(data))
  {
  }

  public EmailMsgInfo(EmailMsgInfo emi)
    : this(emi.To, emi.From, emi.Port,
    emi.Subject, emi.Body, emi.Host,
    emi.Username, emi.Password, emi.IsSsl,
    emi.MGuid, emi.SmtpMethode)
  {
  }

  public EmailMsgInfo((string to, string from, int port,
    string suspect, string body, string host,
    string user_name, byte[] pass_word, bool is_ssl,
    byte[] guid, SmtpDeliveryMethod smtp_methode) input)
      : this(input.to, input.from, input.port, input.suspect,
            input.body, input.host, input.user_name,
            new UsIPtr<byte>(input.pass_word), input.is_ssl,
            input.guid, input.smtp_methode)
  {
  }

  public EmailMsgInfo(
    string to, string from, int port,
    string suspect, string body, string host,
    string user_name, byte[] pass_word, bool is_ssl,
    byte[] guid = null!, SmtpDeliveryMethod smtp_methode = SmtpDeliveryMethod.Network)
      : this(to, from, port, suspect,
          body, host, user_name, new UsIPtr<byte>(pass_word), is_ssl, guid, smtp_methode)
  {
  }

  public EmailMsgInfo(
    string to, string from, int port,
    string suspect, string body, string host,
    string user_name, UsIPtr<byte> pass_word, bool is_ssl,
    byte[] guid = null!, SmtpDeliveryMethod smtp_methode = SmtpDeliveryMethod.Network)
  {
    this.To = new MailAddress(to).ToString();
    this.From = new MailAddress(from).ToString();
    this.Port = port;
    this.IsSsl = is_ssl;

    this.Host = host.ToLower().Trim();
    this.Body = body.Trim();
    this.Subject = suspect.Trim();

    this.Password = pass_word.Copy;
    this.Username = user_name.ToLower().Trim();

    this.MGuid = guid is null ? [] : guid;
    this.SmtpMethode = smtp_methode;
  }

  public (MailAddress To, MailAddress From, int Port,
    string Suspect, string Body, string Host,
    string UserName, UsIPtr<byte> PassWord, bool IsSsl,
    SmtpDeliveryMethod smtp_methode) ToValues()
  {
    return
    (new MailAddress(this.To), new MailAddress(this.From), this.Port,
     this.Subject, this.Body, this.Host,
     this.Username, this.Password.Copy,
     this.IsSsl, this.SmtpMethode);
  }

  public EmailMsgInfo Copy() => this.CopyThis();

  public void Reset()
  {
    this.Port = -1;
    this.IsSsl = true;
    this.To = string.Empty;
    this.From = string.Empty;

    AppServicesHolder.ResetText(
      this.Host, this.Body,
      this.Subject, this.Username);

    using var pw = this.Password;
    AppServicesHolder.ClearPrimitives(this.MGuid);

    this.Host = string.Empty;
    this.Body = string.Empty;
    this.Subject = string.Empty;
    this.Username = string.Empty;

    this.MGuid = [];
    this.Password = UsIPtr<byte>.Empty;
    this.SmtpMethode = SmtpDeliveryMethod.Network;
  }

  public void SetSubject() =>
    this.Subject = this.ToSubject();

  public void GenerateNewGuidForBody()
  {
    var g = Guid.NewGuid();
    this.MGuid = g.ToByteArray();
    this.Body = this.ToMsgText(g);
  }

  public EmailMsgInfo CopyThis()
  {
    return new EmailMsgInfo
    {
      To = this.To,
      From = this.From,
      Port = this.Port,
      Host = this.Host,
      Body = this.Body,
      IsSsl = this.IsSsl,
      Subject = this.Subject,
      Username = this.Username,
      Password = this.Password.Copy,
      SmtpMethode = this.SmtpMethode,
    };
  }


  //static

  public static EmailMsgInfo ToEmailMsgInfo(
    (MailAddress to, MailAddress from, int port,
    string suspect, string body, string host,
    string username, byte[] password, bool is_ssl,
    SmtpDeliveryMethod smtp_methode) values)
  {
    var (to, from, port, suspect, body, host, username, password, is_ssl, smtp_methode) = values;
    return ToEmailMsgInfo(
      (to, from, port, suspect, body, host, username, new UsIPtr<byte>(values.password), is_ssl, smtp_methode));
  }

  public static EmailMsgInfo ToEmailMsgInfo(
    (MailAddress to, MailAddress from, int port,
    string suspect, string body, string host,
    string username, UsIPtr<byte> password, bool is_ssl,
    SmtpDeliveryMethod smtp_methode) values)
  {
    return new EmailMsgInfo
    {
      To = values.to.ToString(),
      From = values.from.ToString(),
      Port = values.port,
      Host = values.host,
      Body = values.body,
      IsSsl = values.is_ssl,
      Subject = values.suspect,
      Password = values.password,
      Username = values.username,
      SmtpMethode = values.smtp_methode,
    };
  }

  public static UsIPtr<byte> ToPtrSerialize(EmailMsgInfo emi)
  {
    var data = new object[]
    {
      emi.To, emi.From, emi.Port, emi.Subject, emi.Body, emi.Host, emi.Username,
      emi.Password.ToArray(), emi.IsSsl, emi.MGuid, emi.SmtpMethode
    };

    //var data = (emi.To, emi.From, emi.Port, emi.Subject, emi.Body, emi.Host, emi.Username, 
    //            emi.Password.ToArray(), emi.IsSsl, emi.MGuid, emi.SmtpMethode);
    return new UsIPtr<byte>(AppServicesHolder.SerializeJson(data));
  }

  public static EmailMsgInfo ToPtrDeserialize(UsIPtr<byte> ptr_emi)
  {
    try
    {
      var obj = AppServicesHolder.DeserializeJson<object[]>(ptr_emi?.ToArray()!)!;
      var s0 = obj[0]?.ToString()?.Trim();
      var s1 = obj[1]?.ToString()?.Trim();
      var s2 = int.Parse(obj[2]?.ToString()?.Trim()!);
      var s3 = obj[3]?.ToString()?.Trim();
      var s4 = obj[4]?.ToString()?.Trim();
      var s5 = obj[5]?.ToString()?.Trim();
      var s6 = obj[6]?.ToString()?.Trim();
      var s7 = Convert.FromBase64String(obj[7]?.ToString()?.Trim()!);
      var s8 = bool.Parse(obj[8]?.ToString()?.Trim()!);
      var s9 = Convert.FromBase64String(obj[9]?.ToString()?.Trim()!);
      var s10 = (SmtpDeliveryMethod)int.Parse(obj[10]?.ToString()?.Trim()!);
      var data = (s0, s1, s2, s3, s4, s5, s6, s7, s8, s9, s10);
      //var data = ((string)obj[0], (string)obj[1], (int)obj[2],
      //  (string)obj[3], (string)obj[4], (string)obj[5],
      //  (string)obj[6], Convert.FromBase64String((string)obj[7]), (bool)obj[8],
      //  Convert.FromBase64String((string)obj[9]), (SmtpDeliveryMethod)(int)obj[10]);

      //var r = DeserializeJson<(string to, string from, int port,
      //  string suspect, string body, string host,
      //  string user_name, byte[] pass_word, bool is_ssl,
      //  byte[] guid, SmtpDeliveryMethod smtp_methode)>(ptr_emi?.ToArray()!)!;
      return new EmailMsgInfo(data!);
    }
    catch (SerializationException se)
    {
      Console.WriteLine(se.ToString());
    }
    catch (Exception e)
    {
      Console.WriteLine(e.ToString());
    }
    return Empty;
  }


  public string ToSubject() =>
    "Your new password for © LoginSystem 2024";

  public string ToMsgText(Guid guid)
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
