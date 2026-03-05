


using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace michele.natale.Tests;

using Pointers;
using CAbiBridge;
using static CryptoTestUtils;

partial class CryptoAesGcmTest
{

  #region Native
  public static void TestAesGcmBytes(int rounds)
  {
    Console.Write($"{nameof(TestAesGcmBytes)} ");

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
        outputLength: NetServices.AES_GCM_MAX_KEY_SIZE));

      var tag = new byte[NetServices.AES_GCM_TAG_SIZE];
      var nonce = RngBytes(NetServices.AES_GCM_NONCE_SIZE);

      size = rand.Next(
        NetServices.AES_GCM_MIN_PLAIN_SIZE,
        NetServices.AES_GCM_MAX_PLAIN_SIZE);

      var plain = RngBytes(size);

      var associat = Encoding.UTF8.GetBytes("© Michele Natale 2021");

      var err = Native.AesGcmEncryptAot(
       plain, plain.Length, key.Ptr, key.Length,
       associat, associat.Length,
       out IntPtr cipher_ptr, out int cipher_length);

      AssertError(err);
      var cipher = ToBytes(cipher_ptr, cipher_length);
      Native.FreeBuffer(cipher_ptr);

      err = Native.AesGcmDecryptAot(
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


  public static void TestAesGcmBytesStress()
  {
    Console.Write($"{nameof(TestAesGcmBytesStress)} ");

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

    var err = Native.AesGcmEncryptAot(
     plain, plain.Length, key.Ptr, key.Length,
     associat, associat.Length,
     out IntPtr cipher_ptr, out int cipher_length);

    AssertError(err);
    var cipher = ToBytes(cipher_ptr, cipher_length);
    Native.FreeBuffer(cipher_ptr);

    err = Native.AesGcmDecryptAot(
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
  public static void TestAesGcmBytesManaged(int rounds)
  {
    Console.Write($"{nameof(TestAesGcmBytesManaged)} ");

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
        outputLength: NetServices.AES_GCM_MAX_KEY_SIZE));

      var tag = new byte[NetServices.AES_GCM_TAG_SIZE];
      var nonce = RngBytes(NetServices.AES_GCM_NONCE_SIZE);

      size = rand.Next(
        NetServices.AES_GCM_MIN_PLAIN_SIZE,
        NetServices.AES_GCM_MAX_PLAIN_SIZE);

      var plain = RngBytes(size);

      var associat = Encoding.UTF8.GetBytes("© Michele Natale 2021");

      var err = CryptoBridge.AesGcmEncryptAotManaged(
              plain, key, associat, out var cipher);
      AssertError(err);

      err = CryptoBridge.AesGcmDecryptAotManaged(
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


  public static void TestAesGcmBytesStressManaged()
  {
    Console.Write($"{nameof(TestAesGcmBytesStressManaged)} ");

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

    var err = CryptoBridge.AesGcmEncryptAotManaged(
      plain, key, associat, out var cipher);
    AssertError(err);

    err = CryptoBridge.AesGcmDecryptAotManaged(
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
