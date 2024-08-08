
using System.Text;

namespace michele.natale.LoginSystems.Services;

using Pointers;

partial class AppServices
{

  public bool CheckValuesPwChange(UsIPtr<byte> pw, UsIPtr<byte> pwr)
  {
    if (pw is null || pw.IsEmpty) return false;
    if (pwr is null || pwr.IsEmpty) return false;
    if (!pw.Equality(pwr)) return false;
    return ShowMessagePwForgetChange(ToStringPwChange(pw)) == DialogResult.OK;
  }

  private string ToStringPwChange(UsIPtr<byte> pw)
  {
    var sb = new StringBuilder();
    sb.AppendLine($"pw: {Encoding.UTF8.GetString(pw.ToArray())}");
    return sb.ToString();
  }

}
