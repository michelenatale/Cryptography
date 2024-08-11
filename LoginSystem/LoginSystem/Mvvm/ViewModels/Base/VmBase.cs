

using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace michele.natale.LoginSystems.ViewModels;

public abstract class VmBase : INotifyPropertyChanged, IDisposable
{

  public event PropertyChangedEventHandler? PropertyChanged;

  public void OnPropertyChanged([CallerMemberName] string property_name = null!) =>
    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property_name));

  public abstract void Dispose();

}
