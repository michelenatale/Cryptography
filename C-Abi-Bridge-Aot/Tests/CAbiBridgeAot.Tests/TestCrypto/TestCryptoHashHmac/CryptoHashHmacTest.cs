


using System.Diagnostics;
using System.Security.Cryptography;

namespace michele.natale.Tests;

using static CryptoTestUtils;

internal class CryptoHashHmacTest
{
  public static void StartNative(int rounds)
  {
    TestMd5(rounds);
    TestSha1(rounds);

    TestSha256(rounds);
    TestSha384(rounds);
    TestSha512(rounds);

    TestSha3256(rounds);
    TestSha3384(rounds);
    TestSha3512(rounds);

    TestShake128(rounds);
    TestShake256(rounds);

    TestMd5Hmac(rounds);
    TestSha1Hmac(rounds);

    TestSha256Hmac(rounds);
    TestSha384Hmac(rounds);
    TestSha512Hmac(rounds);

    TestSha3256Hmac(rounds);
    TestSha3384Hmac(rounds);
    TestSha3512Hmac(rounds);

    Console.WriteLine();
  }

  private static void TestSha1(int rounds)
  {
    Console.Write($"{nameof(TestSha1)}Aot: ");

    var rand = Random.Shared;
    var sw = Stopwatch.StartNew();
    for (int i = 0; i < rounds; i++)
    {
      var size = rand.Next(1, 128);
      var bytes = RngBytes(size);

      var err = Native.Sha1HashDataAot(
        bytes, bytes.Length,
        out IntPtr hash_ptr, out int hash_length);
      AssertError(err);

      var data = ToBytes(hash_ptr, hash_length);
      Native.FreeBuffer(hash_ptr);

      var hash = SHA1.HashData(bytes);
      if (hash_length != hash.Length)
        throw new Exception();

      if (!hash.SequenceEqual(data))
        throw new Exception();

      if (i % (rounds / 10) == 0)
        Console.Write(".");
    }

    sw.Stop();

    var t = (double)sw.ElapsedMilliseconds;
    Console.WriteLine($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms");
    Console.WriteLine();
  }

  private static void TestMd5(int rounds)
  {
    Console.Write($"{nameof(TestMd5)}Aot: ");

    var rand = Random.Shared;
    var sw = Stopwatch.StartNew();
    for (int i = 0; i < rounds; i++)
    {
      var size = rand.Next(1, 128);
      var bytes = RngBytes(size);

      var err = Native.Md5HashDataAot(
        bytes, bytes.Length,
        out IntPtr hash_ptr, out int hash_length);
      AssertError(err);

      var data = ToBytes(hash_ptr, hash_length);
      Native.FreeBuffer(hash_ptr);

      var hash = MD5.HashData(bytes);
      if (hash_length != hash.Length)
        throw new Exception();

      if (!hash.SequenceEqual(data))
        throw new Exception();

      if (i % (rounds / 10) == 0)
        Console.Write(".");
    }

    sw.Stop();

    var t = (double)sw.ElapsedMilliseconds;
    Console.Write($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms");
    Console.WriteLine();
  }

  private static void TestSha256(int rounds)
  {
    Console.Write($"{nameof(TestSha256)}Aot: ");

    var rand = Random.Shared;
    var sw = Stopwatch.StartNew();
    for (int i = 0; i < rounds; i++)
    {
      var size = rand.Next(1, 128);
      var bytes = RngBytes(size);

      var err = Native.Sha256HashDataAot(
        bytes, bytes.Length,
        out IntPtr hash_ptr, out int hash_length);
      AssertError(err);

      var data = ToBytes(hash_ptr, hash_length);
      Native.FreeBuffer(hash_ptr);

      var hash = SHA256.HashData(bytes);
      if (hash_length != hash.Length)
        throw new Exception();

      if (!hash.SequenceEqual(data))
        throw new Exception();

      if (i % (rounds / 10) == 0)
        Console.Write(".");
    }

    sw.Stop();

    var t = (double)sw.ElapsedMilliseconds;
    Console.Write($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms");
    Console.WriteLine();
  }

  private static void TestSha384(int rounds)
  {
    Console.Write($"{nameof(TestSha384)}Aot: ");

    var rand = Random.Shared;
    var sw = Stopwatch.StartNew();
    for (int i = 0; i < rounds; i++)
    {
      var size = rand.Next(1, 128);
      var bytes = RngBytes(size);

      var err = Native.Sha384HashDataAot(
        bytes, bytes.Length,
        out IntPtr hash_ptr, out int hash_length);
      AssertError(err);

      var data = ToBytes(hash_ptr, hash_length);
      Native.FreeBuffer(hash_ptr);

      var hash = SHA384.HashData(bytes);
      if (hash_length != hash.Length)
        throw new Exception();

      if (!hash.SequenceEqual(data))
        throw new Exception();

      if (i % (rounds / 10) == 0)
        Console.Write(".");
    }

    sw.Stop();

    var t = (double)sw.ElapsedMilliseconds;
    Console.Write($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms");
    Console.WriteLine();
  }

  private static void TestSha512(int rounds)
  {
    Console.Write($"{nameof(TestSha512)}Aot: ");

    var rand = Random.Shared;
    var sw = Stopwatch.StartNew();
    for (int i = 0; i < rounds; i++)
    {
      var size = rand.Next(1, 128);
      var bytes = RngBytes(size);

      var err = Native.Sha512HashDataAot(
        bytes, bytes.Length,
        out IntPtr hash_ptr, out int hash_length);
      AssertError(err);

      var data = ToBytes(hash_ptr, hash_length);
      Native.FreeBuffer(hash_ptr);

      var hash = SHA512.HashData(bytes);
      if (hash_length != hash.Length)
        throw new Exception();

      if (!hash.SequenceEqual(data))
        throw new Exception();

      if (i % (rounds / 10) == 0)
        Console.Write(".");
    }

    sw.Stop();

    var t = (double)sw.ElapsedMilliseconds;
    Console.Write($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms\n");
    Console.WriteLine();
  }

  private static void TestSha3256(int rounds)
  {
    Console.Write($"{nameof(TestSha3256)}Aot: ");

    var rand = Random.Shared;
    var sw = Stopwatch.StartNew();
    for (int i = 0; i < rounds; i++)
    {
      var size = rand.Next(1, 128);
      var bytes = RngBytes(size);

      var err = Native.Sha3256HashDataAot(
        bytes, bytes.Length,
        out IntPtr hash_ptr, out int hash_length);
      AssertError(err);

      var data = ToBytes(hash_ptr, hash_length);
      Native.FreeBuffer(hash_ptr);

      var hash = SHA3_256.HashData(bytes);
      if (hash_length != hash.Length)
        throw new Exception();

      if (!hash.SequenceEqual(data))
        throw new Exception();

      if (i % (rounds / 10) == 0)
        Console.Write(".");
    }

    sw.Stop();

    var t = (double)sw.ElapsedMilliseconds;
    Console.Write($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms");
    Console.WriteLine();
  }

  private static void TestSha3384(int rounds)
  {
    Console.Write($"{nameof(TestSha3384)}Aot: ");

    var rand = Random.Shared;
    var sw = Stopwatch.StartNew();
    for (int i = 0; i < rounds; i++)
    {
      var size = rand.Next(1, 128);
      var bytes = RngBytes(size);

      var err = Native.Sha3384HashDataAot(
        bytes, bytes.Length,
        out IntPtr hash_ptr, out int hash_length);
      AssertError(err);

      var data = ToBytes(hash_ptr, hash_length);
      Native.FreeBuffer(hash_ptr);

      var hash = SHA3_384.HashData(bytes);
      if (hash_length != hash.Length)
        throw new Exception();

      if (!hash.SequenceEqual(data))
        throw new Exception();

      if (i % (rounds / 10) == 0)
        Console.Write(".");
    }

    sw.Stop();

    var t = (double)sw.ElapsedMilliseconds;
    Console.Write($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms");
    Console.WriteLine();
  }

  private static void TestSha3512(int rounds)
  {
    Console.Write($"{nameof(TestSha3512)}Aot: ");

    var rand = Random.Shared;
    var sw = Stopwatch.StartNew();
    for (int i = 0; i < rounds; i++)
    {
      var size = rand.Next(1, 128);
      var bytes = RngBytes(size);

      var err = Native.Sha3512HashDataAot(
        bytes, bytes.Length,
        out IntPtr hash_ptr, out int hash_length);
      AssertError(err);

      var data = ToBytes(hash_ptr, hash_length);
      Native.FreeBuffer(hash_ptr);

      var hash = SHA3_512.HashData(bytes);
      if (hash_length != hash.Length)
        throw new Exception();

      if (!hash.SequenceEqual(data))
        throw new Exception();

      if (i % (rounds / 10) == 0)
        Console.Write(".");
    }

    sw.Stop();

    var t = (double)sw.ElapsedMilliseconds;
    Console.Write($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms\n");
    Console.WriteLine();
  }

  private static void TestShake128(int rounds)
  {
    Console.Write($"{nameof(TestShake128)}Aot: ");
     
    var rand = Random.Shared;
    var hash_length = rand.Next(32, 128);
    var sw = Stopwatch.StartNew();
    for (int i = 0; i < rounds; i++)
    {
      var size = rand.Next(1, 128);
      var bytes = RngBytes(size);
            
      var err = Native.Shake128HashDataAot(
        bytes, bytes.Length, hash_length,
        out IntPtr hash_ptr);
      AssertError(err);

      var data = ToBytes(hash_ptr, hash_length);
      Native.FreeBuffer(hash_ptr);

      var hash = Shake128.HashData(bytes, hash_length);
      if (hash_length != hash.Length)
        throw new Exception();

      if (!hash.SequenceEqual(data))
        throw new Exception();

      if (i % (rounds / 10) == 0)
        Console.Write(".");
    }

    sw.Stop();

    var t = (double)sw.ElapsedMilliseconds;
    Console.Write($" rounds = {rounds}; length = {hash_length}; t = {t}ms; td = {t / rounds}ms");
    Console.WriteLine();
  }

  private static void TestShake256(int rounds)
  {
    Console.Write($"{nameof(TestShake256)}Aot: ");

    var rand = Random.Shared;
    var hash_length = rand.Next(32,128);

    var sw = Stopwatch.StartNew();
    for (int i = 0; i < rounds; i++)
    {
      var size = rand.Next(1, 128);
      var bytes = RngBytes(size);

      var err = Native.Shake256HashDataAot(
        bytes, bytes.Length, hash_length,
        out IntPtr hash_ptr);
      AssertError(err);

      var data = ToBytes(hash_ptr, hash_length);
      Native.FreeBuffer(hash_ptr);

      var hash = Shake256.HashData(bytes, hash_length);
      if (hash_length != hash.Length)
        throw new Exception();

      if (!hash.SequenceEqual(data))
        throw new Exception();

      if (i % (rounds / 10) == 0)
        Console.Write(".");
    }

    sw.Stop();

    var t = (double)sw.ElapsedMilliseconds;
    Console.Write($" rounds = {rounds}; length = {hash_length}; t = {t}ms; td = {t / rounds}ms\n");
    Console.WriteLine();
  }

  private static void TestSha1Hmac(int rounds)
  {
    Console.Write($"{nameof(TestSha1Hmac)}Aot: ");

    var rand = Random.Shared;
    var sw = Stopwatch.StartNew();
    for (int i = 0; i < rounds; i++)
    {
      var size = rand.Next(1, 128);
      var bytes = RngBytes(size);

      size = rand.Next(1, 128);
      var key = RngBytes(size);

      var err = Native.HmacSha1HashDataAot(
        bytes, bytes.Length, key, key.Length,
        out IntPtr hash_ptr, out int hash_length);
      AssertError(err);

      var data = ToBytes(hash_ptr, hash_length);
      Native.FreeBuffer(hash_ptr);

      var hash = HMACSHA1.HashData(key, bytes);
      if (hash_length != hash.Length)
        throw new Exception();

      if (!hash.SequenceEqual(data))
        throw new Exception();

      if (i % (rounds / 10) == 0)
        Console.Write(".");
    }

    sw.Stop();

    var t = (double)sw.ElapsedMilliseconds;
    Console.WriteLine($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms");
    Console.WriteLine();
  }

  private static void TestMd5Hmac(int rounds)
  {
    Console.Write($"{nameof(TestMd5Hmac)}Aot: ");

    var rand = Random.Shared;
    var sw = Stopwatch.StartNew();
    for (int i = 0; i < rounds; i++)
    {
      var size = rand.Next(1, 128);
      var bytes = RngBytes(size);

      size = rand.Next(1, 128);
      var key = RngBytes(size);

      var err = Native.HmacMd5HashDataAot(
        bytes, bytes.Length, key, key.Length,
        out IntPtr hash_ptr, out int hash_length);
      AssertError(err);

      var data = ToBytes(hash_ptr, hash_length);
      Native.FreeBuffer(hash_ptr);

      var hash = HMACMD5.HashData(key, bytes);
      if (hash_length != hash.Length)
        throw new Exception();

      if (!hash.SequenceEqual(data))
        throw new Exception();

      if (i % (rounds / 10) == 0)
        Console.Write(".");
    }

    sw.Stop();

    var t = (double)sw.ElapsedMilliseconds;
    Console.Write($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms");
    Console.WriteLine();
  }

  private static void TestSha256Hmac(int rounds)
  {
    Console.Write($"{nameof(TestSha256Hmac)}Aot: ");

    var rand = Random.Shared;
    var sw = Stopwatch.StartNew();
    for (int i = 0; i < rounds; i++)
    {
      var size = rand.Next(1, 128);
      var bytes = RngBytes(size);

      size = rand.Next(1, 128);
      var key = RngBytes(size);

      var err = Native.HmacSha256HashDataAot(
        bytes, bytes.Length, key, key.Length,
        out IntPtr hash_ptr, out int hash_length);
      AssertError(err);

      var data = ToBytes(hash_ptr, hash_length);
      Native.FreeBuffer(hash_ptr);

      var hash = HMACSHA256.HashData(key, bytes);
      if (hash_length != hash.Length)
        throw new Exception();

      if (!hash.SequenceEqual(data))
        throw new Exception();

      if (i % (rounds / 10) == 0)
        Console.Write(".");
    }

    sw.Stop();

    var t = (double)sw.ElapsedMilliseconds;
    Console.Write($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms");
    Console.WriteLine();
  }

  private static void TestSha384Hmac(int rounds)
  {
    Console.Write($"{nameof(TestSha384Hmac)}Aot: ");

    var rand = Random.Shared;
    var sw = Stopwatch.StartNew();
    for (int i = 0; i < rounds; i++)
    {
      var size = rand.Next(1, 128);
      var bytes = RngBytes(size);

      size = rand.Next(1, 128);
      var key = RngBytes(size);

      var err = Native.HmacSha384HashDataAot(
        bytes, bytes.Length, key, key.Length,
        out IntPtr hash_ptr, out int hash_length);
      AssertError(err);

      var data = ToBytes(hash_ptr, hash_length);
      Native.FreeBuffer(hash_ptr);

      var hash = HMACSHA384.HashData(key, bytes);
      if (hash_length != hash.Length)
        throw new Exception();

      if (!hash.SequenceEqual(data))
        throw new Exception();

      if (i % (rounds / 10) == 0)
        Console.Write(".");
    }

    sw.Stop();

    var t = (double)sw.ElapsedMilliseconds;
    Console.Write($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms");
    Console.WriteLine();
  }

  private static void TestSha512Hmac(int rounds)
  {
    Console.Write($"{nameof(TestSha512Hmac)}Aot: ");

    var rand = Random.Shared;
    var sw = Stopwatch.StartNew();
    for (int i = 0; i < rounds; i++)
    {
      var size = rand.Next(1, 128);
      var bytes = RngBytes(size);

      size = rand.Next(1, 128);
      var key = RngBytes(size);

      var err = Native.HmacSha512HashDataAot(
        bytes, bytes.Length, key, key.Length,
        out IntPtr hash_ptr, out int hash_length);
      AssertError(err);

      var data = ToBytes(hash_ptr, hash_length);
      Native.FreeBuffer(hash_ptr);

      var hash = HMACSHA512.HashData(key, bytes);
      if (hash_length != hash.Length)
        throw new Exception();

      if (!hash.SequenceEqual(data))
        throw new Exception();

      if (i % (rounds / 10) == 0)
        Console.Write(".");
    }

    sw.Stop();

    var t = (double)sw.ElapsedMilliseconds;
    Console.WriteLine($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms");
    Console.WriteLine();
  }

  private static void TestSha3256Hmac(int rounds)
  {
    Console.Write($"{nameof(TestSha3256Hmac)}Aot: ");

    var rand = Random.Shared;
    var sw = Stopwatch.StartNew();
    for (int i = 0; i < rounds; i++)
    {
      var size = rand.Next(1, 128);
      var bytes = RngBytes(size);

      size = rand.Next(1, 128);
      var key = RngBytes(size);

      var err = Native.HmacSha3256HashDataAot(
        bytes, bytes.Length, key, key.Length,
        out IntPtr hash_ptr, out int hash_length);
      AssertError(err);

      var data = ToBytes(hash_ptr, hash_length);
      Native.FreeBuffer(hash_ptr);

      var hash = HMACSHA3_256.HashData(key, bytes);
      if (hash_length != hash.Length)
        throw new Exception();

      if (!hash.SequenceEqual(data))
        throw new Exception();

      if (i % (rounds / 10) == 0)
        Console.Write(".");
    }

    sw.Stop();

    var t = (double)sw.ElapsedMilliseconds;
    Console.Write($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms");
    Console.WriteLine();
  }

  private static void TestSha3384Hmac(int rounds)
  {
    Console.Write($"{nameof(TestSha3384Hmac)}Aot: ");

    var rand = Random.Shared;
    var sw = Stopwatch.StartNew();
    for (int i = 0; i < rounds; i++)
    {
      var size = rand.Next(1, 128);
      var bytes = RngBytes(size);

      size = rand.Next(1, 128);
      var key = RngBytes(size);

      var err = Native.HmacSha3384HashDataAot(
        bytes, bytes.Length, key, key.Length,
        out IntPtr hash_ptr, out int hash_length);
      AssertError(err);

      var data = ToBytes(hash_ptr, hash_length);
      Native.FreeBuffer(hash_ptr);

      var hash = HMACSHA3_384.HashData(key, bytes);
      if (hash_length != hash.Length)
        throw new Exception();

      if (!hash.SequenceEqual(data))
        throw new Exception();

      if (i % (rounds / 10) == 0)
        Console.Write(".");
    }

    sw.Stop();

    var t = (double)sw.ElapsedMilliseconds;
    Console.Write($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms");
    Console.WriteLine();
  }

  private static void TestSha3512Hmac(int rounds)
  {
    Console.Write($"{nameof(TestSha3512Hmac)}Aot: ");

    var rand = Random.Shared;
    var sw = Stopwatch.StartNew();
    for (int i = 0; i < rounds; i++)
    {
      var size = rand.Next(1, 128);
      var bytes = RngBytes(size);

      size = rand.Next(1, 128);
      var key = RngBytes(size);

      var err = Native.HmacSha3512HashDataAot(
        bytes, bytes.Length, key, key.Length,
        out IntPtr hash_ptr, out int hash_length);
      AssertError(err);

      var data = ToBytes(hash_ptr, hash_length);
      Native.FreeBuffer(hash_ptr);

      var hash = HMACSHA3_512.HashData(key, bytes);
      if (hash_length != hash.Length)
        throw new Exception();

      if (!hash.SequenceEqual(data))
        throw new Exception();

      if (i % (rounds / 10) == 0)
        Console.Write(".");
    }

    sw.Stop();

    var t = (double)sw.ElapsedMilliseconds;
    Console.Write($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms");
    Console.WriteLine();
  }
}
