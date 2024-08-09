
using System.Windows.Input;

namespace michele.natale.LoginSystems;

/// <summary>
/// The class with the context for the ICommand.
/// </summary>
internal class RelayCommand : ICommand
{
  private readonly Action? MExecute = null!;
  private readonly Func<bool> MCanExecute = null!;

  /// <summary>
  /// The event handler for changing the UserControls.
  /// </summary>
  public event EventHandler? CanExecuteChanged = null!;

  /// <summary>
  /// C-Tor
  /// </summary>
  /// <param name="execute"></param>
  public RelayCommand(Action execute)
    : this(execute, null!)
  {
  }

  /// <summary>
  /// C-Tor
  /// </summary>
  /// <param name="execute"></param>
  /// <param name="can_execute"></param>
  public RelayCommand(Action execute, Func<bool> can_execute)
  {
    ArgumentNullException.ThrowIfNull(execute);
    this.MExecute = execute;
    this.MCanExecute = can_execute;
  }
  //bool ICommand.CanExecute(object? parameter)
  //    => MCanExecute is null || MCanExecute.Invoke();

  //void ICommand.Execute(object? parameter)
  //    => this.MExecute!();//.Invoke()

  /// <summary>
  /// CanExecute Methode
  /// </summary>
  /// <param name="parameter">Parameter as object</param>
  /// <returns></returns>
  public bool CanExecute(object? parameter) =>
    this.MCanExecute is null || this.MCanExecute.Invoke();

  /// <summary>
  /// Execute Methode
  /// </summary>
  /// <param name="parameter"></param>
  public void Execute(object? parameter) => this.MExecute!();

  /// <summary>
  /// Raise Event
  /// </summary>
  public void RaiseCanExecuteChanged() =>
    this.CanExecuteChanged?.Invoke(this, EventArgs.Empty);
}



