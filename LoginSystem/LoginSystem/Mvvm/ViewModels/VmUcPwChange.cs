




using System.Text;
using System.Windows.Input;

namespace michele.natale.LoginSystems.ViewModels;

using Pointers;
using static Services.AppServices;

internal sealed class VmUcPwChange : VmBase
{

  private UsIPtr<byte> MPassWord = UsIPtr<byte>.Empty;
  public string PassWord
  {
    get => Encoding.UTF8.GetString(this.MPassWord.ToArray());
    set
    {
      if (!AppServicesHolder.IsNullOrEmpty(this.MPassWord))
        this.MPassWord.Dispose();
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
      if (!AppServicesHolder.IsNullOrEmpty(this.MRPassWord))
        this.MRPassWord?.Dispose();
      this.MRPassWord = new UsIPtr<byte>(Encoding.UTF8.GetBytes(value));
      this.OnPropertyChanged(nameof(this.RPassWord));
    }
  }

  public ICommand IcBtCancel { get; set; } = null!;
  public ICommand IcBtPwChange { get; set; } = null!;


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

  ~VmUcPwChange() => this.Dispose(false);

  public override void Dispose()
  {
    this.Dispose(true);
    GC.SuppressFinalize(this);
  }

  public void Clear()
  {
    this.MPassWord?.Dispose();
    this.MPassWord = UsIPtr<byte>.Empty;

    this.MRPassWord?.Dispose();
    this.MRPassWord = UsIPtr<byte>.Empty;
  }
}
