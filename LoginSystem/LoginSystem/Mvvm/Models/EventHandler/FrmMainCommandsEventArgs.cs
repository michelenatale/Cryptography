



namespace michele.natale.LoginSystems.Models;

public class FrmMainCommandsEventArgs : EventArgs
{
  public bool HasWorked { get; set; } = false;
  public FrmCommands Commands { get; set; } = FrmCommands.None;
}
