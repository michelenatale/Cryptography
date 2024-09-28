
 
using System.Text;
using System.Diagnostics;

namespace michele.natale.SignatureTest;

using Authors;
using Signatures;
using SS = Signatures.SingleSignature;

public class Program
{
  public static void Main()
  {
    //Very high standards are set in cryptography for
    //the creation of signatures with verification. 
    //EasySignature 2024 has neither been tested nor
    //verified by me. 
    //However, it is freely available to the community.


    var sw = Stopwatch.StartNew();

    var author = AuthorsHolder.ToAuthor;
    Console.WriteLine(author);

    TestSingleSignature();
    TestMultiSignature();

    sw.Stop();

    Console.WriteLine();
    Console.WriteLine($"Total t = {sw.ElapsedMilliseconds}ms");
    Console.WriteLine();
    Console.WriteLine("FINISH");
    Console.ReadLine();
    Console.WriteLine();
  }


  private static void TestSingleSignature()
  {
    Console.Write($"{nameof(TestSingleSignature)}:\t");

    var sw = Stopwatch.StartNew();

    var keyseed = RngBytes(SS.SEED_SIZE);
    var (privkey, pubkey) = SS.CreateKeyPair(keyseed, 129);

    var message = RngBytes(32);

    //For Sign: PrivateKey + Message
    var sign = SS.Sign(privkey, message);

    //For Verify: PublicKey + Message + Signature
    var verify = SS.Verify(pubkey, sign, message);

    sw.Stop();

    if (!verify) throw new Exception();

    Console.Write($"result = {verify}; t = {sw.ElapsedMilliseconds}ms\n");
  }


  private static void TestMultiSignature()
  {
    Console.Write($"{nameof(TestMultiSignature)}:\t");

    //number of signatories
    var cnt = 15;

    var txt = "This is my message. ";

    var message = RepeateForMessage(50, txt);

    var sw = Stopwatch.StartNew();

    //An example is put together at random, with keylength = 2048.
    //The order in 'sign_infos' does not matter here.
    var max_key_force = SingleSignature.MAX_KEY_SIZE;
    var sign_infos = MultiSignature.SignInfoSamples(cnt, message, max_key_force);

    //Extract the public, shared customer class 'MultiSignInfo'.
    //Here too, in 'multi_infos' the order does not matter.
    var multi_infos = sign_infos.Select(x => x.ToMultiSignInfo()).ToArray();

    //Calculates the signature and checks the verify.
    //sign_verify_infos.MultiVerify = true !!!
    var sign_verify_infos = new SignVerifyInfo(multi_infos);

    //Once again, one after the other. The signature is calculated.
    var msign = SignVerifyInfo.Multi_Sign(multi_infos);

    //And here the Verify is performed again.
    var mverify = SignVerifyInfo.Multi_Verifiy(multi_infos, msign);

    sw.Stop();

    if (!mverify) throw new Exception();

    Console.Write($"count = {cnt}; result = {mverify}; t = {sw.ElapsedMilliseconds}ms\n\n");

  }

  private static byte[] RngBytes(int size)
  {
    var rand = Random.Shared;
    var result = new byte[size];
    rand.NextBytes(result);
    return result;
  }

  private static string RepeateForMessage(int cnt, string msg)
  {
    var result = new StringBuilder();
    for (int i = 0; i < cnt; i++)
      result.Append(msg);
    return result.ToString();
  }
}
 