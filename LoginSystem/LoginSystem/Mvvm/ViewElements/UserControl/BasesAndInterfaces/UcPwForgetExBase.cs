namespace michele.natale.LoginSystems.Views;

internal abstract class UcPwForgetExBase : UserControl
{
  /// <summary>
  /// The DrawItem to display the ComboBox as desired.
  /// </summary>
  /// <param name="sender"></param>
  /// <param name="e"></param>
  protected void CbDelivery_DrawItem(object sender, DrawItemEventArgs e)
  {
    if (sender is ComboBox cbx)
    {
      e.DrawBackground();
      if (e.Index >= 0)
      {
        using var sf = new StringFormat()
        {
          Alignment = StringAlignment.Center,
          LineAlignment = StringAlignment.Center,
        };

        Brush sbrush = new SolidBrush(cbx.ForeColor);
        if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
          sbrush = SystemBrushes.HighlightText;

        e.Graphics.DrawString(
          cbx?.Items?[e.Index]?.ToString(),
          cbx?.Font!, sbrush, e.Bounds, sf);
      }
    }
  }

  /// <summary>
  /// The Message Text.
  /// </summary>
  protected static string ToMsgBoxText =>
    "Use your e-mail account to send yourself a new password.\n" +
    "Enter your personal email account data.\n\n" +
    "Your personal data will not be saved.\n\n" +
    "Please do not forget to change your password immediately after Login.";
}