
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;


namespace michele.natale.LoginSystems.Services;

using Pointers;

partial class AppServices
{
  /// <summary>
  /// Check is Email valid
  /// </summary>
  /// <param name="email">Email</param>
  /// <returns></returns>
  public bool IsValidEmail(string email)
  {
    var result = email.Trim();
    try
    {
      var addr = new MailAddress(email);
      return addr.Address.SequenceEqual(result);
    }
    catch
    {
      return false;
    }
  }

  /// <summary>
  /// Check is Email Address valid (over regex)
  /// </summary>
  /// <param name="input">Email</param>
  /// <returns></returns>
  public bool IsValidEmailRegex(string input)
  {
    // returns true if the input is a valid email
    return Regex.IsMatch(input, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
  }

  /// <summary>
  /// Send Email 
  /// </summary>
  /// <param name="emi">EmailMsgInfo</param>
  /// <returns></returns>
  public bool SendEmailMessage(EmailMsgInfo emi)
  {
    return SendEmailMessage(
      emi.To, emi.From, emi.Port, emi.Subject,
      emi.Body, emi.Host, emi.Username, emi.Password,
      emi.IsSsl, emi.SmtpMethode);
  }

  /// <summary>
  /// Send Email
  /// </summary>
  /// <param name="sto">To</param>
  /// <param name="sfrom">From</param>
  /// <param name="port">Port</param>
  /// <param name="subject">Subject</param>
  /// <param name="body">Body</param>
  /// <param name="host">Host</param>
  /// <param name="user_name">Username</param>
  /// <param name="pass_word">Password</param>
  /// <param name="is_ssl">SSL</param>
  /// <param name="smtp_methode">Methode SMTP</param>
  /// <returns></returns>
  public bool SendEmailMessage(
    string sto, string sfrom, int port,
    string subject, string body, string host,
    string user_name, byte[] pass_word, bool is_ssl,
    SmtpDeliveryMethod smtp_methode = SmtpDeliveryMethod.Network)
  {
    return SendEmailMessage(
       new MailAddress(sto), new MailAddress(sfrom), port, subject,
       body, host, user_name, pass_word, is_ssl, smtp_methode);
  }

  /// <summary>
  /// Send Email
  /// </summary>
  /// <param name="sto">To</param>
  /// <param name="sfrom">From</param>
  /// <param name="port">Port</param>
  /// <param name="subject">Subject</param>
  /// <param name="body">Body</param>
  /// <param name="host">host</param>
  /// <param name="user_name">Username</param>
  /// <param name="pass_word">Password</param>
  /// <param name="is_ssl">SSL</param>
  /// <param name="smtp_methode">Methode of SMTP</param>
  /// <returns></returns>
  public bool SendEmailMessage(
    string sto, string sfrom, int port,
    string subject, string body, string host,
    string user_name, UsIPtr<byte> pass_word, bool is_ssl,
    SmtpDeliveryMethod smtp_methode = SmtpDeliveryMethod.Network)
  {
    return SendEmailMessage(
      new MailAddress(sto), new MailAddress(sfrom), port, subject,
      body, host, user_name, pass_word.ToArray(), is_ssl, smtp_methode);
  }

  /// <summary>
  /// Send EMail
  /// </summary>
  /// <param name="mato">To</param>
  /// <param name="mafrom">From</param>
  /// <param name="port">Port</param>
  /// <param name="subject">Subject</param>
  /// <param name="body">body</param>
  /// <param name="host">Host</param>
  /// <param name="user_name">Username</param>
  /// <param name="pass_word">Password</param>
  /// <param name="is_ssl">SSL</param>
  /// <param name="smtp_methode">Methode of SMTP</param>
  /// <returns></returns>
  public bool SendEmailMessage(
    MailAddress mato, MailAddress mafrom, int port,
    string subject, string body, string host,
    string user_name, byte[] pass_word, bool is_ssl,
    SmtpDeliveryMethod smtp_methode = SmtpDeliveryMethod.Network)
  {
    //https://manuals.jam-software.de/smartserialmail/DE/faq_mail_provider.html 

    MailAddress to = mato;
    MailAddress from = mafrom;

    using var email = new MailMessage(from.Address, to.Address)
    {
      Body = body,
      Subject = subject
    };

    using var smtp = new SmtpClient
    {
      Port = port,
      Host = host,
      UseDefaultCredentials = false,
      DeliveryMethod = smtp_methode,
      EnableSsl = is_ssl,
    };

    smtp.Credentials = new NetworkCredential(
      from.Address, Encoding.UTF8.GetString(pass_word));


    try
    {
      smtp.Send(email);
      return true;
    }
    catch (SmtpException ex)
    {
      Console.WriteLine(ex.ToString());
      return false;
    }
  }
}
