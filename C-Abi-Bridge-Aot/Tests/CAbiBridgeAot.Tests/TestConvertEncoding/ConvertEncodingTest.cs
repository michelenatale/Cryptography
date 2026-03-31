


using System.Text;
using System.Diagnostics;

namespace michele.natale.Tests;

using static CryptoTestUtils;

internal class ConvertEncodingTest
{
  public static void StartNative(int rounds)
  {
    TestBase64(rounds);
    TestHex(rounds);
  }

  private static void TestBase64(int rounds)
  {
    Console.Write($"{nameof(TestBase64)}Aot: ");

    var rand = Random.Shared;
    var sw = Stopwatch.StartNew();
    for (int i = 0; i < rounds; i++)
    {
      var size = rand.Next(1, 128);
      var bytes = RngBytes(size);

      var err = Native.ToBase64Aot(
        bytes, bytes.Length,
        out IntPtr plain_ptr, out int plain_length);
      AssertError(err);

      var data = ToBytes(plain_ptr, plain_length);
      Native.FreeBuffer(plain_ptr);

      var b64 = Encoding.UTF8.GetBytes(
        Convert.ToBase64String(bytes));
      if (plain_length != b64.Length)
        throw new Exception();

      if (!b64.SequenceEqual(data))
        throw new Exception();

      plain_ptr = IntPtr.Zero;
      err = Native.FromBase64Aot(
        data, data.Length,
        out plain_ptr, out plain_length);
      AssertError(err);

      data = ToBytes(plain_ptr, plain_length);
      Native.FreeBuffer(plain_ptr);

      var b64r = Convert.FromBase64String(
        Encoding.UTF8.GetString(b64));

      if (plain_length != b64r.Length)
        throw new Exception();

      if (!b64r.SequenceEqual(data))
        throw new Exception();

      if (i % (rounds / 10) == 0)
        Console.Write(".");
    }

    sw.Stop();

    var t = (double)sw.ElapsedMilliseconds;
    Console.Write($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms");
    Console.WriteLine();
  }

  private static void TestHex(int rounds)
  {
    Console.Write($"{nameof(TestHex)}Aot: ");

    var rand = Random.Shared;
    var sw = Stopwatch.StartNew();
    for (int i = 0; i < rounds; i++)
    {
      var size = rand.Next(1, 128);
      var bytes = RngBytes(size);

      var lower = int.IsEvenInteger(rand.Next());
      var err = Native.ToHexAot(
        bytes, bytes.Length,
        out IntPtr hex_ptr, out int hex_length,
        lower);
      AssertError(err);

      var data = ToBytes(hex_ptr, hex_length);
      Native.FreeBuffer(hex_ptr);

      byte[] hex;
      if (lower)
        hex = Encoding.UTF8.GetBytes(
          Convert.ToHexStringLower(bytes));
      else hex = Encoding.UTF8.GetBytes(
        Convert.ToHexString(bytes));

      if (hex_length != hex.Length)
        throw new Exception();

      if (!hex.SequenceEqual(data))
        throw new Exception();

      err = Native.FromHexAot(
        hex, hex.Length,
        out hex_ptr, out hex_length);
      AssertError(err);

      data = ToBytes(hex_ptr, hex_length);
      Native.FreeBuffer(hex_ptr);

      var hexr = Convert.FromHexString(
        Encoding.UTF8.GetString(hex));
      if (hex_length != hexr.Length)
        throw new Exception();

      if (!hexr.SequenceEqual(data))
        throw new Exception();

      if (i % (rounds / 10) == 0)
        Console.Write(".");
    }

    sw.Stop();

    var t = (double)sw.ElapsedMilliseconds;
    Console.Write($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms\n");
    Console.WriteLine();
  }
}
