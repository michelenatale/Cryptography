

using michele.natale.Authors;
using System.Diagnostics;


namespace michele.natale.EasySignatureTest;

public class Program
{
  public static void Main()
  {
    //Notes:
    //Very high standards are set in cryptography for
    //the creation of signatures with verification. 
    //EasySignature 2024 has neither been tested nor
    //verified by me. 
    //However, it is freely available to the community.

    var sw = Stopwatch.StartNew();

    var author = AuthorsHolder.ToAuthor;
    Console.WriteLine(author);

    for (int i = 0; i < 5; i++)
      UnitTest.Start();

    sw.Stop();

    Console.WriteLine();
    Console.WriteLine($"Total t = {sw.ElapsedMilliseconds}ms");
    Console.WriteLine();
    Console.WriteLine("FINISH");
    Console.ReadLine();
    Console.WriteLine();
  }
}