

using System.Runtime.ConstrainedExecution;
using System.Text;

namespace michele.natale.Authors;

public class AuthorsHolder
{

  public static string ToAuthor { get; private set; } = Author();


  private static string Author()
  {
    var result = new StringBuilder();
    result.AppendLine("© EasySignature 2024");
    result.AppendLine("Created by © Michele Natale 2024");
    result.AppendLine("A very quick and simple signature creator.");
    result.AppendLine("Also for multi-signatures.\n ");

    result.AppendLine("Very high standards are set in cryptography for the creation of signatures with verification. ");
    result.AppendLine("© EasySignature 2024 has neither been tested nor verified by me. ");
    result.AppendLine("However, it is freely available to the community. \n");

    result.AppendLine("Für die Erstellung von Signaturen mit der Verifizierung, werden in der Cryptography sehr hohe Ansprüche gestellt. ");
    result.AppendLine("© EasySignature 2024 ist von mir weder getestet noch geprüft worden. ");
    result.AppendLine("Es steht jedoch für die Gemeinschaft frei zur Verfügung.");
    result.AppendLine();
    result.AppendLine();

    return result.ToString();
  }
}
