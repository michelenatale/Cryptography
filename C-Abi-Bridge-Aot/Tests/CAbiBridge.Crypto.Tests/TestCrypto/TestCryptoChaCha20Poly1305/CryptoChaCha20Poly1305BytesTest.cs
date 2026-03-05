
 


using System.Text; 
using System.Diagnostics;
using System.Security.Cryptography;

namespace michele.natale.Tests;

using Pointers; 
using static CryptoTestUtils;

partial class CryptoChaCha20Poly1305Test
{
  public async static Task TestChaCha20Poly1305BytesAsync(int rounds)
  {
    Console.Write($"{nameof(TestChaCha20Poly1305BytesAsync)}: ");

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
        outputLength: NetServices.CHACHA_POLY_MAX_KEY_SIZE));

      var tag = new byte[NetServices.CHACHA_POLY_TAG_SIZE];
      var nonce = RngBytes(NetServices.CHACHA_POLY_NONCE_SIZE);

      size = rand.Next(
        NetServices.CHACHA_POLY_MIN_PLAIN_SIZE,
        NetServices.CHACHA_POLY_MAX_PLAIN_SIZE);

      var plain = RngBytes(size);

      var associat = Encoding.UTF8.GetBytes("© Michele Natale 2021");
      var cipher = NetServices.EncryptionChaCha20Poly1305(plain, key, associat);
      var decipher = NetServices.DecryptionChaCha20Poly1305(cipher, key, associat);

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



  public async static Task TestChaCha20Poly1305BytesStressAsync()
  {
    Console.Write($"{nameof(TestChaCha20Poly1305BytesStressAsync)}: ");

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
     outputLength: NetServices.AES_GCM_MAX_KEY_SIZE));

    var tag = new byte[NetServices.AES_GCM_TAG_SIZE];
    var nonce = RngBytes(NetServices.AES_GCM_NONCE_SIZE);
    var plain = RngBytes(NetServices.AES_GCM_MAX_PLAIN_SIZE);

    var associat = Encoding.UTF8.GetBytes("© Michele Natale 2021");
    var cipher = NetServices.EncryptionAesGcm(plain, key, associat);
    var decipher = NetServices.DecryptionAesGcm(cipher, key, associat);

    sw.Stop();

    if (!decipher.SequenceEqual(plain))
      throw new Exception();

    Console.Write($"t = {sw.ElapsedMilliseconds}ms");
    Console.WriteLine();
  }
}
