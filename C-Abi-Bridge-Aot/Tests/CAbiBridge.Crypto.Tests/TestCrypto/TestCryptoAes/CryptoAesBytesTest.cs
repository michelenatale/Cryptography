

using System.Text;
using System.Diagnostics;
using System.Security.Cryptography;

namespace michele.natale.Tests;

using Pointers; 
using static CryptoTestUtils;

partial class CryptoAesTest
{

  public async static Task TestAesBytesAsync(int rounds)
  {
    Console.Write($"{nameof(TestAesBytesAsync)}: ");

    var rand = Random.Shared;
    var sw = Stopwatch.StartNew();
    for (int i = 0; i < rounds; i++)
    {
      var size = rand.Next(
        NetServices.MIN_PW_SIZE, 17);

      var pw = RngBytes(size);

      size = rand.Next(
        NetServices.MIN_SALT_SIZE, 
        NetServices.MAX_SALT_SIZE);

      var salt = RngBytes(size);

      using var key = new UsIPtr<byte>(
        Rfc2898DeriveBytes.Pbkdf2(
        password: pw, salt: salt, 
        iterations: NetServices.MIN_ITERATION,
        hashAlgorithm: HashAlgorithmName.SHA512,
        outputLength: NetServices.AES_KEY_SIZE));

      var tag = new byte[NetServices.AES_TAG_SIZE];
      var nonce = RngBytes(NetServices.AES_IV_SIZE);

      size = rand.Next(
        NetServices.AES_MIN_PLAIN_SIZE, 
        NetServices.AES_MAX_PLAIN_SIZE);

      var plain = RngBytes(size);

      var associat = Encoding.UTF8.GetBytes("© Michele Natale 2021");
      var cipher = NetServices.EncryptionAes(plain, key, associat);
      var decipher = NetServices.DecryptionAes(cipher, key,associat);

      if (!decipher.SequenceEqual(plain))
        throw new Exception();

      if (i % (rounds / 10) == 0)
        Console.Write(".");
    }

    sw.Stop();

    var t = sw.ElapsedMilliseconds;
    Console.Write($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms");
    Console.WriteLine();
  }



  public async static Task TestAesBytesStressAsync()
  {
    Console.Write($"{nameof(TestAesBytesStressAsync)}: ");

    var rand = Random.Shared;
    var sw = Stopwatch.StartNew();

    var size = rand.Next(
      NetServices.MIN_PW_SIZE, 17);

    var pw = RngBytes(size);

    size = rand.Next(
      NetServices.MIN_SALT_SIZE,
      NetServices.MAX_SALT_SIZE);

    var salt = RngBytes(size);

    using var key = new UsIPtr<byte>(
     Rfc2898DeriveBytes.Pbkdf2(
     password: pw, salt: salt,
     iterations: NetServices.MIN_ITERATION,
     hashAlgorithm: HashAlgorithmName.SHA512,
     outputLength: NetServices.AES_KEY_SIZE));

    var plain = RngBytes(1024 * 1024);
    var tag = new byte[NetServices.AES_TAG_SIZE];
    var nonce = RngBytes(NetServices.AES_IV_SIZE);

    var associat = Encoding.UTF8.GetBytes("© Michele Natale 2021");
    var cipher = NetServices.EncryptionAes(plain, key, associat);
    var decipher = NetServices.DecryptionAes(cipher, key, associat);

    sw.Stop();

    if (!decipher.SequenceEqual(plain))
      throw new Exception();

    Console.Write($"t = {sw.ElapsedMilliseconds}ms");
    Console.WriteLine();
  }
}
