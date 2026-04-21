
 
using System.Text;
using System.Numerics;
using System.Diagnostics;

namespace michele.natale.Tests;

using static CryptoTestUtils;
using static TestServices;

internal class ConvertEncodingTest
{
  public static void StartNative(int rounds)
  {
    TestBase64(rounds);
    TestHex(rounds);
    TestBaseX(rounds);
  }

  private static void TestBaseX(int rounds)
  {
    TestBaseConverter_2_256(rounds);
    TestBaseConverter_2_256_Stress();
    //TestBaseConverter_2_256_Extrem_Stress(); //solve in ~3/4 min.

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

  private static void TestBaseConverter_2_256(int rounds)
  {
    Console.Write($"{nameof(TestBaseConverter_2_256)}Aot: ");

    var rand = Random.Shared;
    var sw = Stopwatch.StartNew();
    for (int i = 0; i < rounds; i++)
    {
      var (startbase, targetbase) = RngBases_2_256();

      var rng = RngBytes(8, true); //uint64, int64

      var bi = new BigInteger(rng, true, false); //base 10
      var bibytes = TrimFirst(bi.ToByteArray(true, false)); //base 256

      //Notes: For this to work, the byte array of the
      //BigInteger must be entered into the converter
      //as a little-endian.
      var bytes = Converter_2_256_LE_S(bi, 256, 10);

      var bytes2 = bi.ToString().Select(x => (byte)(x - 48)).ToArray(); //base 10
      if (!bytes.SequenceEqual(bytes2)) throw new Exception();

      var sbase1 = ToBaseX_2_256_LE_S(bi, startbase);
      var sbase2 = ToBaseX_2_256_LE_S(bytes, startbase);
      if (!sbase1.SequenceEqual(sbase2)) throw new Exception();

      var decipher1 = FromBaseX_2_256_LE_S(sbase1, startbase);
      if (!bytes.SequenceEqual(decipher1)) throw new Exception();

      var tbase1 = Converter_2_256_LE_S(sbase2, startbase, targetbase);
      var tbase2 = ToBaseX_2_256_LE_S(bytes, targetbase);
      if (!tbase1.SequenceEqual(tbase2)) throw new Exception();

      var rbytes1 = Converter_2_256_LE_S(tbase1, targetbase, startbase);
      if (!rbytes1.SequenceEqual(sbase1)) throw new Exception();

      if (i % (rounds / 10) == 0)
        Console.Write(".");
    }

    sw.Stop();

    var t = (double)sw.ElapsedMilliseconds;
    Console.Write($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms");
    Console.WriteLine();
  }

  private static void TestBaseConverter_2_256_Stress()
  {
    Console.Write($"{nameof(TestBaseConverter_2_256_Stress)}Aot: ");

    var sz = 1024; 
    var rand = Random.Shared;

    var (startbase, targetbase) = RngBases_2_256();
    var bytes = RngBaseXNumber(sz, startbase);

    var sw = Stopwatch.StartNew();
    var basex = Converter_2_256_LE_S(bytes, startbase, targetbase);
    var decipher = Converter_2_256_LE_S(basex, targetbase, startbase);

    if (!decipher.SequenceEqual(bytes))
      throw new Exception();

    sw.Stop();
    Console.WriteLine($"startbase = {startbase}; targetbase = {targetbase}; size = {sz}; t = {sw.ElapsedMilliseconds} ms");
  }

  private static void TestBaseConverter_2_256_Extrem_Stress()
  {
    Console.Write($"{nameof(TestBaseConverter_2_256_Extrem_Stress)}Aot: ");

    var sz = 20 * 1024; //20 KB
    var rand = Random.Shared;

    var (startbase, targetbase) = RngBases_2_256();
    var bytes = RngBaseXNumber(sz, startbase);

    var sw = Stopwatch.StartNew();
    var basex = Converter_2_256_LE_S(bytes, startbase, targetbase);
    var decipher = Converter_2_256_LE_S(basex, targetbase, startbase);

    if (!decipher.SequenceEqual(bytes))
      throw new Exception();

    sw.Stop();
    Console.WriteLine($"startbase = {startbase}; targetbase = {targetbase}; size = {sz}; t = {sw.ElapsedMilliseconds} ms");
    Console.WriteLine();
  }
}
