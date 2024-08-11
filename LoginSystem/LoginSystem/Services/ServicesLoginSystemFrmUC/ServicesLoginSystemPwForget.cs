
using System.Net.Mail;
using System.Text;

namespace michele.natale.LoginSystems.Services;

using Pointers;

partial class AppServices
{

  public bool CheckValuesPwForget(
    (string uname, string email, UsIPtr<byte> pw, UsIPtr<byte> pwr,
     int port, string host, bool is_ssl, SmtpDeliveryMethod delivery) input,
    out EmailMsgInfo result)
  {
    result = EmailMsgInfo.Empty;
    var (uname, email, pw, pwr, port, host, is_ssl, delivery) = input;
    if (pw is null || pw.IsEmpty) return false;
    if (pwr is null || pwr.IsEmpty) return false;
    if (!pw.Equality(pwr)) return false;

    if (string.IsNullOrEmpty(host)) return false;
    if (string.IsNullOrEmpty(uname)) return false;
    if (port < 0 || port > short.MaxValue) return false;

    if (string.IsNullOrEmpty(email)) return false;
    if (!this.IsValidEmail(email)) return false;
    if (this.ShowMessagePwForgetChange(this.ToStringPwForget(input)) == DialogResult.OK)
    {
      result = new EmailMsgInfo
      {
        MGuid = [],
        To = email,
        From = email,
        Port = port,
        Host = host,
        IsSsl = is_ssl,
        Username = uname,
        Body = string.Empty,
        Subject = string.Empty,
        SmtpMethode = delivery,
        Password = pw,
      };
      return true;
    }
    return false;
  }

  private string ToStringPwForget(
  (string uname, string email, UsIPtr<byte> pw, UsIPtr<byte> pwr,
     int port, string host, bool is_ssl, SmtpDeliveryMethod delivery) input)
  {
    var (uname, email, pw, pwr, port, host, is_ssl, delivery) = input;
    var sb = new StringBuilder();
    sb.AppendLine($"ssl: {is_ssl}");
    sb.AppendLine($"port: {port}");
    sb.AppendLine($"delivery: {delivery}");
    sb.AppendLine($"host: {host.ToLower().Trim()}");
    sb.AppendLine($"email: {email.ToLower().Trim()}");
    sb.AppendLine();
    sb.AppendLine($"username: {uname.ToLower().Trim()}");
    sb.AppendLine($"pw: {new string('*', 20)}");
    sb.AppendLine($"pw repeat: {new string('*', 20)}");
    return sb.ToString();
  }

}
