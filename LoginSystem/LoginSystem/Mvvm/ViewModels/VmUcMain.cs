

using System.Windows.Input;

namespace michele.natale.LoginSystems.ViewModels;


internal sealed class VmUcMain : VmBase
{
  public ICommand IcBtChangePw { get; set; } = null!;
  public ICommand IcBtInfoLoginSystem { get; set; } = null!;

  public override void Dispose()
  {
  }
}
