

using System.Text;

namespace michele.natale.EcCurveDsaDh;

partial class EcService
{

  public static byte[] ToUserSystemData()
  {
    var sysdata = Environment.CommandLine +
       Environment.CurrentDirectory + Environment.ProcessPath +
       /*Environment.CurrentManagedThreadId +*/ Environment.HasShutdownStarted +
       Environment.Is64BitOperatingSystem + Environment.Is64BitProcess +
       Environment.IsPrivilegedProcess + Environment.MachineName +
       Environment.OSVersion + /*Environment.ProcessId +*/ Environment.ProcessorCount +
       Environment.ProcessPath /*+ Environment.StackTrace */+ Environment.SystemDirectory +
       Environment.SystemPageSize /*+ Environment.TickCount + Environment.TickCount64*/ +
       Environment.UserDomainName + Environment.UserInteractive + Environment.UserName +
       Environment.Version /*+ Environment.WorkingSet*/;
    return Encoding.UTF8.GetBytes(sysdata);
  }
}
