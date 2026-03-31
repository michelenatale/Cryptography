


using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace michele.natale.Tests;

using Pointers;
using static CryptoTestUtils;

partial class CryptoAesGcmTest
{
  public async static Task TestAesGcmBytesAsync(int rounds)
  {
    Console.Write($"{nameof(TestAesGcmBytesAsync)} ");

    var rand = Random.Shared;
    var sw = Stopwatch.StartNew();
    for (int i = 0; i < rounds; i++)
    {
      var size = rand.Next(
        NetServicesCrypto.MIN_PW_SIZE, 17);

      var pw = RngBytes(size);

      size = rand.Next(
        NetServicesCrypto.MIN_SALT_SIZE,
        NetServicesCrypto.MAX_SALT_SIZE);

      var salt = RngBytes(size);

      using var key = new UsIPtr<byte>(
        Rfc2898DeriveBytes.Pbkdf2(
        password: pw, salt: salt,
        iterations: NetServicesCrypto.MIN_ITERATION,
        hashAlgorithm: HashAlgorithmName.SHA512,
        outputLength: NetServicesCrypto.AES_GCM_MAX_KEY_SIZE));

      var tag = new byte[NetServicesCrypto.AES_GCM_TAG_SIZE];
      var nonce = RngBytes(NetServicesCrypto.AES_GCM_NONCE_SIZE);

      size = rand.Next(
        NetServicesCrypto.AES_GCM_MIN_PLAIN_SIZE,
        NetServicesCrypto.AES_GCM_MAX_PLAIN_SIZE);

      var plain = RngBytes(size);

      var associat = Encoding.UTF8.GetBytes("© Michele Natale 2021");
      var cipher = NetServicesCrypto.EncryptionAesGcm(plain, key, associat);
      var decipher = NetServicesCrypto.DecryptionAesGcm(cipher, key, associat);

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



  public async static Task TestAesGcmBytesStressAsync()
  {
    Console.Write($"{nameof(TestAesGcmBytesStressAsync)} ");

    var rand = Random.Shared;
    var sw = Stopwatch.StartNew();

    var size = rand.Next(
      NetServicesCrypto.MIN_PW_SIZE, 17);

    var pw = RngBytes(size);

    size = rand.Next(
      NetServicesCrypto.MIN_SALT_SIZE,
      NetServicesCrypto.MAX_SALT_SIZE);

    var salt = RngBytes(size);

    using var key = new UsIPtr<byte>(
     Rfc2898DeriveBytes.Pbkdf2(
     password: pw, salt: salt,
     iterations: NetServicesCrypto.MIN_ITERATION,
     hashAlgorithm: HashAlgorithmName.SHA512,
     outputLength: NetServicesCrypto.AES_GCM_MAX_KEY_SIZE));

    var tag = new byte[NetServicesCrypto.AES_GCM_TAG_SIZE];
    var nonce = RngBytes(NetServicesCrypto.AES_GCM_NONCE_SIZE);
    var plain = RngBytes(NetServicesCrypto.AES_GCM_MAX_PLAIN_SIZE);

    var associat = Encoding.UTF8.GetBytes("© Michele Natale 2021");
    var cipher = NetServicesCrypto.EncryptionAesGcm(plain, key, associat);
    var decipher = NetServicesCrypto.DecryptionAesGcm(cipher, key, associat);

    sw.Stop();

    if (!decipher.SequenceEqual(plain))
      throw new Exception();

    Console.Write($"t = {sw.ElapsedMilliseconds}ms");
    Console.WriteLine();
  }
}
