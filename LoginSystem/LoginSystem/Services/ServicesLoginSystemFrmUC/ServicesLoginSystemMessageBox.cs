
using System.Text;

namespace michele.natale.LoginSystems.Services;

partial class AppServices
{

  public DialogResult ShowMessageApp(
    string msg, string title = "SYSTEM LoginSystem",
    MessageBoxButtons mbb = MessageBoxButtons.OK,
    MessageBoxIcon mbi = MessageBoxIcon.Information)
  {
    //Only English
    return MessageBox.Show(
        msg, title, mbb, mbi);
  }

  /// <summary>
  /// Show a Message on the screen
  /// </summary>
  /// <param name="uname">Username</param>
  /// <param name="email">Email</param>
  /// <param name="mbb">MessageBoxButtons</param>
  /// <param name="mbi">MessageBoxIcon</param>
  /// <returns>DialogResult</returns>
  public DialogResult ShowMessageLogin(
    string uname,
    string email,
    MessageBoxButtons mbb = MessageBoxButtons.OK,
    MessageBoxIcon mbi = MessageBoxIcon.Question)
  {
    //Only English
    string txt = string.Empty;
    if (string.IsNullOrEmpty(email))
      txt = $"username = {uname}{Environment.NewLine}";
    if (string.IsNullOrEmpty(uname))
      txt += $"email = {email}{Environment.NewLine}";
    txt += $"password = {new string('*', 20)}{Environment.NewLine}";
    return MessageBox.Show(
        txt, "SYSTEM LoginSystem",
        mbb, mbi);
  }

  /// <summary>
  /// Show a Message on the screen
  /// </summary>
  /// <param name="uname">Username</param>
  /// <param name="email">Email</param>
  /// <param name="mbb">MessageBoxButtons</param>
  /// <param name="mbi">MessageBoxIcon</param>
  /// <returns>DialogResult</returns>
  public DialogResult ShowMessageRegist(
    string uname,
    string email,
    MessageBoxButtons mbb = MessageBoxButtons.OK,
    MessageBoxIcon mbi = MessageBoxIcon.Question)
  {
    //Only English
    var txt = $"username = {uname}{Environment.NewLine}";
    txt += $"password = {new string('*', 20)}{Environment.NewLine}";
    txt += $"email = {email}{Environment.NewLine}";
    return MessageBox.Show(
        txt, "SYSTEM LoginSystem",
        mbb, mbi);
  }


  /// <summary>
  /// Show a Message on the screen
  /// </summary>
  /// <param name="txt">Message</param>
  /// <param name="mbb">MessageBoxButtons</param>
  /// <param name="mbi">MessageBoxIcon</param>
  /// <returns>DialogResult</returns>
  public DialogResult ShowMessagePwForgetChange(
    string txt,
    MessageBoxButtons mbb = MessageBoxButtons.OK,
    MessageBoxIcon mbi = MessageBoxIcon.Question)
  {
    return MessageBox.Show(
        txt, "SYSTEM LoginSystem",
        mbb, mbi);
  }


  /// <summary>
  /// Show a Message on the screen
  /// </summary>
  /// <param name="filename">Path of file</param>
  /// <param name="mbb">MessageBoxButtons</param>
  /// <param name="mbi">MessageBoxIcon</param>
  /// <returns>DialogResult</returns>
  private DialogResult ShowMessageSecretFile(
    string filename,
    MessageBoxButtons mbb = MessageBoxButtons.OK,
    MessageBoxIcon mbi = MessageBoxIcon.Information)
  {
    var txt = new StringBuilder();
    txt.AppendLine($"Your secret LoginSystem file is:");
    txt.AppendLine($"{filename}");
    txt.AppendLine();
    txt.AppendLine("Please keep the file in a safe place. ");
    txt.AppendLine("Keep the file secret!");
    txt.AppendLine();

    return MessageBox.Show(
        txt.ToString(),
        "SYSTEM LoginSystem",
        mbb, mbi);
  }


}
