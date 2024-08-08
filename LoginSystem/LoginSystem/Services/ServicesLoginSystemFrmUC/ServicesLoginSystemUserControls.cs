
namespace michele.natale.LoginSystems.Services;

partial class AppServices
{

  /// <summary>
  /// Set default values for Properties for a UserControl
  /// </summary>
  /// <param name="uc">UserControl</param>
  /// <param name="ucname">Name of UserControl</param>
  public void SetUcSettings(UserControl uc, string ucname)
  {
    uc.BackColor = Color.Transparent;
    uc.Dock = DockStyle.None;
    uc.Enabled = true;
    uc.Font = new Font("Arial", 14F);
    uc.Location = new Point(0, 0);
    uc.Margin = new Padding(5, 4, 5, 4);
    uc.Name = ucname;
    uc.Size = new Size(808, 595);
    uc.TabIndex = 0;
  }
}
