
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
    if (!IsDisposed)
    {
      if (disposing)
      {
        Clear();
      }
      IsDisposed = true;
    }
  }

  ~VmUcLogin() => Dispose(false);

  public override void Dispose()
  {
    Dispose(true);
    GC.SuppressFinalize(this);
  }

  public void ClearTextBoxes()
  {
    Clear();
  }

  public void Clear()
  {
    if (IsDisposed)
      return;

    AppServicesHolder.ResetText(MUserName);
    UserName = string.Empty;

    MPassWord.Dispose();
    MPassWord = UsIPtr<byte>.Empty;

  }
  #endregion Disposing
}
