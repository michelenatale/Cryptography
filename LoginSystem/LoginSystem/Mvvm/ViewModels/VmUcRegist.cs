


using System.Text;
using System.Windows.Input;

namespace michele.natale.LoginSystems.ViewModels;

using Pointers;
using static Services.AppServices;

internal sealed class VmUcRegist : VmBase
{

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

  public ICommand IcBtRegist { get; set; } = null!;
  public ICommand IcBtCancel { get; set; } = null!;
  public ICommand IcLlLogin { get; set; } = null!;
  public ICommand IcLlPwForget { get; set; } = null!;



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

  ~VmUcRegist() => Dispose(false);

  public override void Dispose()
  {
    Dispose(true);
    GC.SuppressFinalize(this);
  }

  public void Clear()
  {
    if (IsDisposed)
      return;
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
