


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
    get => this.MUserName;
    set
    {
      this.MUserName = value;
      this.OnPropertyChanged(nameof(this.UserName));
    }
  }

  private UsIPtr<byte> MPassWord = UsIPtr<byte>.Empty;
  public string PassWord
  {
    get => Encoding.UTF8.GetString(this.MPassWord.ToArray());
    set
    {
      if (!AppServicesHolder.IsNullOrEmpty(MPassWord))
        this.MPassWord?.Dispose();
      this.MPassWord = new UsIPtr<byte>(Encoding.UTF8.GetBytes(value));
      this.OnPropertyChanged(nameof(this.PassWord));
    }
  }

  private UsIPtr<byte> MRPassWord = UsIPtr<byte>.Empty;
  public string RPassWord
  {
    get => Encoding.UTF8.GetString(this.MRPassWord.ToArray());
    set
    {
      if (!AppServicesHolder.IsNullOrEmpty(MRPassWord))
        this.MRPassWord.Dispose();
      this.MRPassWord = new UsIPtr<byte>(Encoding.UTF8.GetBytes(value));
      this.OnPropertyChanged(nameof(this.RPassWord));
    }
  }

  private string MEMail = string.Empty;
  public string EMail
  {
    get => this.MEMail;
    set
    {
      this.MEMail = value;
      this.OnPropertyChanged(nameof(this.EMail));
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
    if (!this.IsDisposed)
    {
      if (disposing)
      {
        this.Clear();
      }
      this.IsDisposed = true;
    }
  }

  ~VmUcRegist() => this.Dispose(false);

  public override void Dispose()
  {
    this.Dispose(true);
    GC.SuppressFinalize(this);
  }

  public void Clear()
  {
    if (this.IsDisposed)
      return;
    AppServicesHolder.ResetText(this.MEMail);
    this.MEMail = string.Empty;

    AppServicesHolder.ResetText(this.MUserName);
    this.MUserName = string.Empty;

    this.MPassWord.Dispose();
    this.MPassWord = UsIPtr<byte>.Empty;

    this.MRPassWord.Dispose();
    this.MRPassWord = UsIPtr<byte>.Empty;

  }
}
