



using System.Net.Mail;
using System.Text;
using System.Windows.Input;

namespace michele.natale.LoginSystems.ViewModels;

using Pointers;
using static Services.AppServices;

internal sealed class VmUcPwForget : VmBase
{
  private string MEMail = string.Empty;
  public string EMail
  {
    get => MEMail;
    set
    {
      MEMail = value;
      OnPropertyChanged(nameof(EMail));
    }
  }

  private string MUserName = string.Empty;
  public string UserName
  {
    get => MUserName;
    set
    {
      MUserName = value;
      OnPropertyChanged(nameof(UserName));
    }
  }

  private UsIPtr<byte> MPassWord = UsIPtr<byte>.Empty;
  public string PassWord
  {
    get => Encoding.UTF8.GetString(MPassWord.ToArray());
    set
    {
      MPassWord = new UsIPtr<byte>(Encoding.UTF8.GetBytes(value));
      OnPropertyChanged(nameof(PassWord));
    }
  }

  private UsIPtr<byte> MRPassWord = UsIPtr<byte>.Empty;
  public string RPassWord
  {
    get => Encoding.UTF8.GetString(MRPassWord.ToArray());
    set
    {
      MRPassWord = new UsIPtr<byte>(Encoding.UTF8.GetBytes(value));
      OnPropertyChanged(nameof(RPassWord));
    }
  }

  private int MPort = -1;
  public int Port
  {
    get => MPort;
    set
    {
      MPort = value;
      OnPropertyChanged(nameof(RPassWord));
    }
  }

  private string MHost = string.Empty;
  public string Host
  {
    get => MHost;
    set
    {
      MHost = value;
      OnPropertyChanged(nameof(Host));
    }
  }

  private bool MSsl = true;
  public bool IsSsl
  {
    get => MSsl;
    set
    {
      MSsl = value;
      OnPropertyChanged(nameof(IsSsl));
    }
  }

  private SmtpDeliveryMethod MDelivery =
    SmtpDeliveryMethod.Network;
  public SmtpDeliveryMethod Delivery
  {
    get => MDelivery;
    set
    {
      MDelivery = value;
      OnPropertyChanged(nameof(Delivery));
    }
  }

  public ICommand IcLlLogin { get; set; } = null!;
  public ICommand IcBtPwForget { get; set; } = null!;
  public ICommand IcBtCancel { get; set; } = null!;
  public ICommand IcLlRegist { get; set; } = null!;



  public bool IsDisposed
  {
    get; private set;
  }
  internal void Dispose(bool disposing)
  {
    if (!IsDisposed)
    {
      if (disposing)
      {
        Clear();
      }
      IsDisposed = true;
    }
  }

  ~VmUcPwForget() => Dispose(false);

  public override void Dispose()
  {
    Dispose(true);
    GC.SuppressFinalize(this);
  }

  public void Clear()
  {
    if (IsDisposed)
      return;

    MPort = -1;
    MSsl = true;
    MDelivery = SmtpDeliveryMethod.Network;

    AppServicesHolder.ResetText(MHost);
    MHost = string.Empty;

    AppServicesHolder.ResetText(MEMail);
    MEMail = string.Empty;

    AppServicesHolder.ResetText(MUserName);
    MUserName = string.Empty;

    MPassWord.Dispose();
    MPassWord = UsIPtr<byte>.Empty;

    MRPassWord.Dispose();
    MRPassWord = UsIPtr<byte>.Empty;

  }
}
