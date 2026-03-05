

using System.Text;
using System.Diagnostics;
using System.Security.Cryptography;

namespace michele.natale.Tests;

using Pointers;
using CAbiBridge;
using static CryptoTestUtils;

partial class CryptoAesTest
{
  #region Native

  public static void TestAesBytes(int rounds)
  {
    Console.Write($"{nameof(TestAesBytes)}: ");

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

      var err = Native.AesEncryptAot(
        plain, plain.Length, key.Ptr, key.Length,
        associat, associat.Length,
        out IntPtr cipher_ptr, out int cipher_length);
      AssertError(err);

      var cipher = ToBytes(cipher_ptr, cipher_length);
      Native.FreeBuffer(cipher_ptr);

      err = Native.AesDecryptAot(
        cipher, cipher.Length, key.Ptr, key.Length,
        associat, associat.Length,
        out IntPtr decipher_ptr, out int decipher_length);
      AssertError(err);

      var decipher = ToBytes(decipher_ptr, decipher_length);
      Native.FreeBuffer(decipher_ptr);

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


  public static void TestAesBytesStress()
  {
    Console.Write($"{nameof(TestAesBytesStress)}: ");

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

    var err = Native.AesEncryptAot(
            plain, plain.Length, key.Ptr, key.Length,
            associat, associat.Length,
            out IntPtr cipher_ptr, out int cipher_length);

    AssertError(err);
    var cipher = ToBytes(cipher_ptr, cipher_length);
    Native.FreeBuffer(cipher_ptr);

    err = Native.AesDecryptAot(
      cipher, cipher.Length, key.Ptr, key.Length,
      associat, associat.Length,
      out IntPtr decipher_ptr, out int decipher_length);

    AssertError(err);
    var decipher = ToBytes(decipher_ptr, decipher_length);
    Native.FreeBuffer(decipher_ptr);

    sw.Stop();

    if (!decipher.SequenceEqual(plain))
      throw new Exception();

    Console.Write($"t = {sw.ElapsedMilliseconds}ms");
    Console.WriteLine();
  }
  #endregion Native

  #region Managed

  public static void TestAesBytesManaged(int rounds)
  {
    Console.Write($"{nameof(TestAesBytesManaged)}: ");

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

      var err = CryptoBridge.AesEncryptAotManaged(
        plain, key, associat, out var cipher);
      AssertError(err);

      err = CryptoBridge.AesDecryptAotManaged(
        cipher, key, associat, out var decipher);
      AssertError(err);

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


  public static void TestAesBytesStressManaged()
  {
    Console.Write($"{nameof(TestAesBytesStressManaged)}: ");

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

    var err = CryptoBridge.AesEncryptAotManaged(
      plain, key, associat, out var cipher);
    AssertError(err);

    err = CryptoBridge.AesDecryptAotManaged(
      cipher, key, associat, out var decipher);
    AssertError(err);

    sw.Stop();

    if (!decipher.SequenceEqual(plain))
      throw new Exception();

    Console.Write($"t = {sw.ElapsedMilliseconds}ms");
    Console.WriteLine();
  }

  #endregion Managed
}
