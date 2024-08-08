

using System.Text;

namespace michele.natale.LoginSystems;

/// <summary>
/// Information about the author of © LoginSystem 2024
/// </summary>
internal class AuthorInfos
{
  /// <summary>
  /// Information about the author of © LoginSystem 2024
  /// </summary>
  /// <returns>Author and Name of LoginSystem</returns>
  internal static string Author_Info()
  {
    var sb = new StringBuilder();
    sb.AppendLine("© LoginSystem 2024");
    sb.AppendLine("Created by © Michele Natale 2024");
    sb.AppendLine();
    sb.AppendLine();
    Console.WriteLine(sb.ToString());
    return sb.ToString();
  }
}
