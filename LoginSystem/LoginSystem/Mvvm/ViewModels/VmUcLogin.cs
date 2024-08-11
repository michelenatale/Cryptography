
using System.Text;
using System.Windows.Input;

namespace michele.natale.LoginSystems.ViewModels;

using Pointers;
using static Services.AppServices;

internal sealed class VmUcLogin : VmBase
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
      if(!AppServicesHolder.IsNullOrEmpty(this.MPassWord))
        this.MPassWord.Dispose();
      this.MPassWord = new UsIPtr<byte>(Encoding.UTF8.GetBytes(value));
      this.OnPropertyChanged(nameof(this.PassWord));
    }
  }

  public ICommand IcBtLogin { get; set; } = null!;
  public ICommand IcBtCancel { get; set; } = null!;
  public ICommand IcLlRegist { get; set; } = null!;
  public ICommand IcLlPwForget { get; set; } = null!;

  #region Disposing

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

  ~VmUcLogin() => this.Dispose(false);

  public override void Dispose()
  {
    this.Dispose(true);
    GC.SuppressFinalize(this);
  }

  public void ClearTextBoxes()
  {
    this.Clear();
  }

  public void Clear()
  {
    if (this.IsDisposed)
      return;

    AppServicesHolder.ResetText(this.MUserName);
    this.UserName = string.Empty;

    this.MPassWord?.Dispose();
    this.MPassWord = UsIPtr<byte>.Empty;

  }
  #endregion Disposing
}
