




using System.Text;
using System.Windows.Input;

namespace michele.natale.LoginSystems.ViewModels;

using Pointers;

internal sealed class VmUcPwChange : VmBase
{

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

  public ICommand IcBtCancel { get; set; } = null!;
  public ICommand IcBtPwChange { get; set; } = null!;


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

  ~VmUcPwChange() => Dispose(false);

  public override void Dispose()
  {
    Dispose(true);
    GC.SuppressFinalize(this);
  }

  public void Clear()
  {

    MPassWord.Dispose();
    MPassWord = UsIPtr<byte>.Empty;

    MRPassWord.Dispose();
    MRPassWord = UsIPtr<byte>.Empty;

  }
}
