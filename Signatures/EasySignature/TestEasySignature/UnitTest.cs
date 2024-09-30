


using System.Diagnostics;

namespace michele.natale.EasySignatureTest;

using static RandomHolder;
using Cryptography.Signatures;
using SS = Cryptography.Signatures.SingleSignature;
using System.Text;

public class UnitTest
{
  public static void Start()
  {

    TestSingleSignature();
    TestSingleSignatureForce();

    TestMultiSignature();
    TestMultiSignatureForce();
  }

  private static void TestSingleSignature()
  {
    Console.Write($"{nameof(TestSingleSignature)}:\t\t");

    var sw = Stopwatch.StartNew();

    var keyseed = RngBytes(SS.SEED_SIZE);
    var (privkey, pubkey) = SS.CreateKeyPair(keyseed, 129);

    //See too, SingleSignature.MIN_MESSAGE_SIZE = 10
    var message = RngBytes(32);

    //For Sign: PrivateKey + Message
    var sign = SS.Sign(privkey, message);

    //For Verify: PublicKey + Message + Signature
    var verify = SS.Verify(pubkey, sign, message);

    sw.Stop();

    if (!verify) throw new Exception();

    Console.Write($"result = {verify}; t = {sw.ElapsedMilliseconds}ms\n");
  }

  private static void TestSingleSignatureForce()
  {
    Console.Write($"{nameof(TestSingleSignatureForce)}:\t");

    var sw = Stopwatch.StartNew();

    var keyseed = RngBytes(SS.SEED_SIZE);

    var (privkey, pubkey) = SS.CreateKeyPair(keyseed, 2048, true);

    var message = RngBytes(32);

    //For Sign: PrivateKey + Message + keyseed (force)
    var sign = SS.Sign(privkey, message, keyseed);

    //For Verify: PublicKey + Message + Signature
    var verify = SS.Verify(pubkey, sign, message);

    sw.Stop();

    if (!verify) throw new Exception();

    Console.Write($"result = {verify}; t = {sw.ElapsedMilliseconds}ms\n");
  }


  private static void TestMultiSignature()
  {
    Console.Write($"{nameof(TestMultiSignature)}:\t\t");

    //number of signatories
    var cnt = 15;

    //See too, MultiSignature.MIN_MESSAGE_SIZE
    var message = RngBytes(180);

    var sw = Stopwatch.StartNew();

    //An example is put together at random, with keylength = 2048.
    //The order in 'sign_infos' does not matter here.
    var max_key_force = MultiSignature.MAX_KEY_SIZE;
    var sign_infos = SignInfoSamples(cnt, message, max_key_force);

    //Extract the public, shared customer class 'MultiSignInfo'.
    //Here too, in 'multi_infos' the order does not matter.
    var multi_infos = sign_infos.Select(x => x.ToMultiSignInfo()).ToArray();

    //Calculates the signature and checks the verify.
    //sign_verify_infos.MultiVerify = true !!!
    //An enhanced signature (extra_force) calculation would also be possible here.
    var sign_verify_infos = new SignVerifyInfo(multi_infos);

    if (!sign_verify_infos.MultiVerify) throw new Exception();

    Console.Write($"count = {cnt}; result = {sign_verify_infos.MultiVerify}; t = {sw.ElapsedMilliseconds}ms\n");

  }

  private static void TestMultiSignatureForce()
  {
    Console.Write($"{nameof(TestMultiSignatureForce)}:\t");

    //number of signatories
    var cnt = 15;

    //See too, MultiSignature.MIN_MESSAGE_SIZE
    var message = RngBytes(180);

    var sw = Stopwatch.StartNew();

    //An example is put together at random, with keylength = 2048.
    //The order in 'sign_infos' does not matter here.
    var max_key_force = MultiSignature.MAX_KEY_SIZE;
    var sign_infos = SignInfoSamples(cnt, message, max_key_force, true);

    //Extract the public, shared customer class 'MultiSignInfo'.
    //Here too, in 'multi_infos' the order does not matter.
    var multi_infos = sign_infos.Select(x => x.ToMultiSignInfo()).ToArray();

    //In this way, including ExtraForce = true.
    //Once again, one after the other. The signature is calculated.
    var msign_force = SignVerifyInfo.Multi_Sign(multi_infos, true);

    //Or in this way, including ExtraForce = true.
    //And here the Verify is performed again.
    var mverify_force = SignVerifyInfo.Multi_Verifiy(multi_infos, msign_force, true);

    sw.Stop();

    if (!mverify_force) throw new Exception();

    Console.Write($"count = {cnt}; result = {mverify_force}; t = {sw.ElapsedMilliseconds}ms\n\n");

  }


  /// <summary>
  /// Returns the random sample data for the calculation of a signature. 
  /// </summary>
  /// <param name="size">Desired Size</param>
  /// <param name="message">Desired Message</param>
  /// <param name="keysize">Desired KeySize</param>
  /// <param name="extra_force">Desired Signature Force</param>
  /// <returns>SignInfo</returns>
  private static SignInfo[] SignInfoSamples(
    int size, string message, int keysize = 128, bool extra_force = false) =>
    SignInfoSamples(size, Encoding.UTF8.GetBytes(message), keysize, extra_force);

  /// <summary>
  /// Returns the random sample data for the calculation of a signature. 
  /// </summary>
  /// <param name="size">Desired Size</param>
  /// <param name="message">Desired Message</param>
  /// <param name="keysize">Desired KeySize</param>
  /// <param name="extra_force">Desired Signature Force</param>
  /// <returns>SignInfo</returns>
  private static SignInfo[] SignInfoSamples(
    int size, byte[] message, int keysize = 128, bool extra_force = false)
  {
    if (size < 2 || message.Length < MultiSignature.MIN_MESSAGE_SIZE)
      throw new ArgumentOutOfRangeException(nameof(size));

    if (keysize < MultiSignature.MIN_MESSAGE_SIZE || keysize > MultiSignature.MAX_KEY_SIZE)
      throw new ArgumentOutOfRangeException(nameof(keysize));

    var result = new SignInfo[size];
    for (int i = 0; i < size; i++)
    {
      var name = $"Name_{i}";
      var msg = Convert.ToHexString(message);
      var seed = RngBytes(SingleSignature.SEED_SIZE);
      var (privk, pubk) = SingleSignature.CreateKeyPair(seed, keysize, extra_force);

      byte[] sign;
      if (extra_force)
        sign = SingleSignature.Sign(privk, Encoding.UTF8.GetBytes(msg), seed);
      else sign = SingleSignature.Sign(privk, Encoding.UTF8.GetBytes(msg));
      result[i] = new SignInfo(name, seed, msg, pubk, sign, extra_force);
    }
    return result;
  }
}
