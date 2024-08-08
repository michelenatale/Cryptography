

using Microsoft.VisualBasic;

namespace michele.natale.LoginSystems.Services;

partial class AppServices
{
  /// <summary>
  /// Shows an InputBox on the screen
  /// </summary>
  /// <param name="message">Message</param>
  /// <param name="title">Title</param>
  /// <returns>Answer as string.</returns>
  /// <exception cref="ArgumentNullException"></exception>
  public string ToInputBox(
    string message, string title = "SYSTEM LoginSystem")
  {
    if (string.IsNullOrEmpty(message))
      throw new ArgumentNullException(nameof(message));
    var result = Interaction.InputBox(message, title);
    return string.IsNullOrEmpty(result.Trim()) ? string.Empty : result.Trim();
  }

  /// <summary>
  /// The Text for the InputBox
  /// </summary>
  public string ToInputBoxText =>
    "Please enter your secret 'LS-SPW-F' password.";
}
