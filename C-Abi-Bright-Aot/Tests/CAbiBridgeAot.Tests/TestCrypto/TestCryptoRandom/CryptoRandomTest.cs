 
using System.Diagnostics; 

namespace michele.natale.Tests;
 
using static CryptoTestUtils;
using static CAbiBridge.CryptoBridge;

partial class CryptoRandomTest
{

  public static void Start(int rounds)
  {
    TestRngCryptoBoolAot(rounds);
    TestRngCryptoBytesAot(rounds);

    TestNextCryptoInt32Aot(rounds);
    TestRngCryptoInt32Aot(rounds);

    TestNextCryptoInt64Aot(rounds);
    TestRngCryptoInt64Aot(rounds);

    TestNextCryptoDoubleAot(rounds);
    TestRngCryptoDoubleAot(rounds);

    TestNextCryptoSingleAot(rounds);
    TestRngCryptoSingleAot(rounds);

    TestNextCryptoDecimalAot(rounds);
    TestRngCryptoDecimalAot(rounds);
  }

  private static void TestRngCryptoBytesAot(int rounds)
  {
    Console.Write($"{nameof(TestRngCryptoBytesAot)}: ");

    var rand = Random.Shared;
    var sw = Stopwatch.StartNew();
    for (var i = 0; i < rounds; i++)
    {
      var size = rand.Next(128, 512);
      var err = Native.RngCryptoBytesAot(size, out var out_ptr);
      AssertError(err);

      var bytes = ToBytes(out_ptr, size);
      Native.FreeBuffer(out_ptr);

      if (bytes is null || bytes.Length != size)
        throw new Exception();

      if (i % (rounds / 10) == 0)
        Console.Write(".");
    }

    sw.Stop();
    var t = (double)sw.ElapsedMilliseconds;
    Console.WriteLine($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms");


    Console.Write($"Test{nameof(FillCryptoBytesAot)}: ");

    sw = Stopwatch.StartNew();
    for (var i = 0; i < rounds; i++)
    {
      var size = rand.Next(128, 512);
      var bytes = new byte[size];
      var err = Native.FillCryptoBytesAot(bytes, bytes.Length);
      AssertError(err);

      if (bytes is null || bytes.Length != size)
        throw new Exception();

      if (bytes.Max(x => x) <= 0)
        throw new Exception();

      if (i % (rounds / 10) == 0)
        Console.Write(".");
    }

    sw.Stop();
    t = sw.ElapsedMilliseconds;
    Console.WriteLine($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms\n");
  }

  private static void TestRngCryptoBoolAot(int rounds)
  {
    Console.Write($"{nameof(TestRngCryptoBoolAot)}: ");

    var rand = Random.Shared;
    var sw = Stopwatch.StartNew();
    for (var i = 0; i < rounds; i++)
    {
      var size = rand.Next(128, 512);
      var err = Native.RngCryptoBoolAot(size, out var out_ptr);
      AssertError(err);

      var bools = ToBools(out_ptr, size);
      Native.FreeBuffer(out_ptr);

      if (bools is null || bools.Length != size)
        throw new Exception();

      if (i % (rounds / 10) == 0)
        Console.Write(".");
    }

    sw.Stop();
    var t = (double)sw.ElapsedMilliseconds;
    Console.WriteLine($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms");


    Console.Write($"Test{nameof(NextCryptoBoolAot)}: ");

    sw = Stopwatch.StartNew();
    for (var i = 0; i < rounds; i++)
    {
      var bol = Native.NextCryptoBoolAot(out var err);
      AssertError(err);

      if (i % (rounds / 10) == 0)
        Console.Write(".");
    }

    sw.Stop();
    t = sw.ElapsedMilliseconds;
    Console.WriteLine($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms\n");
  }

  private static void TestNextCryptoInt32Aot(int rounds)
  {
    Console.Write($"{nameof(TestNextCryptoInt32Aot)}: ");

    var rand = Random.Shared;
    var sw = Stopwatch.StartNew();
    for (var i = 0; i < rounds; i++)
    {
      var result = Native.NextCryptoInt32Aot(out var err);
      AssertError(err);

      if (result < 0)
        throw new Exception();

      if (i % (rounds / 10) == 0)
        Console.Write(".");
    }

    sw.Stop();
    var t = (double)sw.ElapsedMilliseconds;
    Console.WriteLine($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms");

    Console.Write($"Test{nameof(NextCryptoInt32MaxAot)}: ");

    sw = Stopwatch.StartNew();
    for (var i = 0; i < rounds; i++)
    {
      var max = rand.Next();

      //hier wird vielfach ein minuswert geliefert
      var result = Native.NextCryptoInt32MaxAot(max, out var err); 
      AssertError(err);

      if (result < 0 || result > max)
        throw new Exception();

      if (i % (rounds / 10) == 0)
        Console.Write(".");
    }

    sw.Stop();
    t = sw.ElapsedMilliseconds;
    Console.WriteLine($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms");

    Console.Write($"Test{nameof(NextCryptoInt32MinMaxAot)}: ");

    sw = Stopwatch.StartNew();
    for (var i = 0; i < rounds; i++)
    {
      int min = int.MaxValue / 3, max = 2 * min;
      var result = Native.NextCryptoInt32MinMaxAot(min, max, out var err);
      AssertError(err);

      if (result < min || result > max)
        throw new Exception();

      if (i % (rounds / 10) == 0)
        Console.Write(".");
    }

    sw.Stop();
    t = sw.ElapsedMilliseconds;
    Console.WriteLine($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms\n");
  }

  private static void TestRngCryptoInt32Aot(int rounds)
  {
    Console.Write($"{nameof(TestRngCryptoInt32Aot)}: ");

    var rand = Random.Shared;
    var sw = Stopwatch.StartNew();
    for (var i = 0; i < rounds; i++)
    {
      var size = rand.Next(128, 512);
      var err = Native.RngCryptoInt32Aot(size, out var out_ptr);
      AssertError(err);

      var result = ToInts<int>(out_ptr, size);
      Native.FreeBuffer(out_ptr);

      if (result is null || result.Length != size)
        throw new Exception();

      if (i % (rounds / 10) == 0)
        Console.Write(".");
    }

    sw.Stop();
    var t = (double)sw.ElapsedMilliseconds;
    Console.WriteLine($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms");

    Console.Write($"Test{nameof(RngCryptoInt32MaxAot)}: ");

    sw = Stopwatch.StartNew();
    for (var i = 0; i < rounds; i++)
    {
      var max = rand.Next();
      var size = rand.Next(128, 512);
      var err = Native.RngCryptoInt32MaxAot(size, max, out var out_ptr);
      AssertError(err);

      var result = ToInts<int>(out_ptr, size);
      Native.FreeBuffer(out_ptr);

      if (result is null || result.Length != size)
        throw new Exception();

      if (result.Min(x => x) < 0 || result.Max(x => x) > max)
        throw new Exception();

      if (i % (rounds / 10) == 0)
        Console.Write(".");
    }

    sw.Stop();
    t = sw.ElapsedMilliseconds;
    Console.WriteLine($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms");


    Console.Write($"Test{nameof(RngCryptoInt32MinMaxAot)}: ");

    sw = Stopwatch.StartNew();
    for (var i = 0; i < rounds; i++)
    {
      var size = rand.Next(128, 512);
      int min = int.MaxValue / 3, max = min * 2;
      var err = Native.RngCryptoInt32MinMaxAot(size, min, max, out var out_ptr);
      AssertError(err);

      var result = ToInts<int>(out_ptr, size);
      Native.FreeBuffer(out_ptr);

      if (result is null || result.Length != size)
        throw new Exception();

      if (result.Min(x => x) < min || result.Max(x => x) > max)
        throw new Exception();

      if (i % (rounds / 10) == 0)
        Console.Write(".");
    }

    sw.Stop();
    t = sw.ElapsedMilliseconds;
    Console.WriteLine($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms\n");
  }

  private static void TestNextCryptoInt64Aot(int rounds)
  {
    Console.Write($"{nameof(TestNextCryptoInt64Aot)}: ");

    var rand = Random.Shared;
    var sw = Stopwatch.StartNew();
    for (var i = 0; i < rounds; i++)
    {
      var result = Native.NextCryptoInt64Aot(out var err);
      AssertError(err);

      if (result < 0)
        throw new Exception();

      if (i % (rounds / 10) == 0)
        Console.Write(".");
    }

    sw.Stop();
    var t = (double)sw.ElapsedMilliseconds;
    Console.WriteLine($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms");

    Console.Write($"Test{nameof(NextCryptoInt64MaxAot)}: ");

    sw = Stopwatch.StartNew();
    for (var i = 0; i < rounds; i++)
    {
      var max = rand.NextInt64();
      var result = Native.NextCryptoInt64MaxAot(max, out var err);
      AssertError(err);

      if (result < 0 || result > max)
        throw new Exception();

      if (i % (rounds / 10) == 0)
        Console.Write(".");
    }

    sw.Stop();
    t = sw.ElapsedMilliseconds;
    Console.WriteLine($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms");

    Console.Write($"Test{nameof(NextCryptoInt64MinMaxAot)}: ");

    sw = Stopwatch.StartNew();
    for (var i = 0; i < rounds; i++)
    {
      long min = long.MaxValue / 3, max = 2 * min;
      var result = Native.NextCryptoInt64MinMaxAot(min, max, out var err);
      AssertError(err);

      if (result < min || result > max)
        throw new Exception();

      if (i % (rounds / 10) == 0)
        Console.Write(".");
    }

    sw.Stop();
    t = sw.ElapsedMilliseconds;
    Console.WriteLine($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms");
  }

  private static void TestRngCryptoInt64Aot(int rounds)
  {
    Console.Write($"{nameof(TestRngCryptoInt64Aot)}: ");

    var rand = Random.Shared;
    var sw = Stopwatch.StartNew();
    for (var i = 0; i < rounds; i++)
    {
      var size = rand.Next(128, 512);
      var err = Native.RngCryptoInt64Aot(size, out var out_ptr);
      AssertError(err);

      var result = ToInts<long>(out_ptr, size);
      Native.FreeBuffer(out_ptr);

      if (result is null || result.Length != size)
        throw new Exception();

      if (i % (rounds / 10) == 0)
        Console.Write(".");
    }

    sw.Stop();
    var t = (double)sw.ElapsedMilliseconds;
    Console.WriteLine($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms");

    Console.Write($"Test{nameof(RngCryptoInt64MaxAot)}: ");

    sw = Stopwatch.StartNew();
    for (var i = 0; i < rounds; i++)
    {
      var max = rand.Next();
      var size = rand.Next(128, 512);
      var err = Native.RngCryptoInt64MaxAot(size, max, out var out_ptr);
      AssertError(err);

      var result = ToInts<long>(out_ptr, size);
      Native.FreeBuffer(out_ptr);

      if (result is null || result.Length != size)
        throw new Exception();

      if (result.Min(x => x) < 0 || result.Max(x => x) > max)
        throw new Exception();

      if (i % (rounds / 10) == 0)
        Console.Write(".");
    }

    sw.Stop();
    t = sw.ElapsedMilliseconds;
    Console.WriteLine($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms");


    Console.Write($"Test{nameof(RngCryptoInt64MinMaxAot)}: ");

    sw = Stopwatch.StartNew();
    for (var i = 0; i < rounds; i++)
    {
      var size = rand.Next(128, 512);
      long min = long.MaxValue / 3, max = min * 2;
      var err = Native.RngCryptoInt64MinMaxAot(size, min, max, out var out_ptr);
      AssertError(err);

      var result = ToInts<long>(out_ptr, size);
      Native.FreeBuffer(out_ptr);

      if (result is null || result.Length != size)
        throw new Exception();

      if (result.Min(x => x) < min || result.Max(x => x) > max)
        throw new Exception();

      if (i % (rounds / 10) == 0)
        Console.Write(".");
    }

    sw.Stop();
    t = sw.ElapsedMilliseconds;
    Console.WriteLine($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms\n");
  }

  private static void TestNextCryptoDoubleAot(int rounds)
  {
    Console.Write($"{nameof(TestNextCryptoDoubleAot)}: ");

    var rand = Random.Shared;
    var sw = Stopwatch.StartNew();
    for (var i = 0; i < rounds; i++)
    {
      var result = Native.NextCryptoDoubleAot(out var err);
      AssertError(err);

      if (result < 0)
        throw new Exception();

      if (i % (rounds / 10) == 0)
        Console.Write(".");
    }

    sw.Stop();
    var t = (double)sw.ElapsedMilliseconds;
    Console.WriteLine($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms");

    Console.Write($"Test{nameof(NextCryptoDoubleMaxAot)}: ");

    sw = Stopwatch.StartNew();
    for (var i = 0; i < rounds; i++)
    {
      var max = rand.NextInt64();
      var result = Native.NextCryptoDoubleMaxAot(max, out var err);
      AssertError(err);

      if (result < 0 || result > max)
        throw new Exception();

      if (i % (rounds / 10) == 0)
        Console.Write(".");
    }

    sw.Stop();
    t = sw.ElapsedMilliseconds;
    Console.WriteLine($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms");

    Console.Write($"Test{nameof(NextCryptoDoubleMinMaxAot)}: ");

    sw = Stopwatch.StartNew();
    for (var i = 0; i < rounds; i++)
    {
      double min = double.MaxValue / 3, max = 2 * min;
      var result = Native.NextCryptoDoubleMinMaxAot(min, max, out var err);
      AssertError(err);

      if (result < min || result > max)
        throw new Exception();

      if (i % (rounds / 10) == 0)
        Console.Write(".");
    }

    sw.Stop();
    t = sw.ElapsedMilliseconds;
    Console.WriteLine($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms\n");
  }

  private static void TestRngCryptoDoubleAot(int rounds)
  {
    Console.Write($"{nameof(TestRngCryptoDoubleAot)}: ");

    var rand = Random.Shared;
    var sw = Stopwatch.StartNew();
    for (var i = 0; i < rounds; i++)
    {
      var size = rand.Next(128, 512);
      var err = Native.RngCryptoDoubleAot(size, out var out_ptr);
      AssertError(err);

      var result = ToFloats<double>(out_ptr, size);
      Native.FreeBuffer(out_ptr);

      if (result is null || result.Length != size)
        throw new Exception();

      if (i % (rounds / 10) == 0)
        Console.Write(".");
    }

    sw.Stop();
    var t = (double)sw.ElapsedMilliseconds;
    Console.WriteLine($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms");

    Console.Write($"Test{nameof(RngCryptoDoubleMaxAot)}: ");

    sw = Stopwatch.StartNew();
    for (var i = 0; i < rounds; i++)
    {
      var max = rand.NextInt64();
      var size = rand.Next(128, 512);
      var err = Native.RngCryptoDoubleMaxAot(size, max, out var out_ptr);
      AssertError(err);

      var result = ToFloats<double>(out_ptr, size);
      Native.FreeBuffer(out_ptr);

      if (result is null || result.Length != size)
        throw new Exception();

      if (result.Min(x => x) < 0.0 || result.Max(x => x) > max)
        throw new Exception();

      if (i % (rounds / 10) == 0)
        Console.Write(".");
    }

    sw.Stop();
    t = sw.ElapsedMilliseconds;
    Console.WriteLine($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms");


    Console.Write($"Test{nameof(RngCryptoDoubleMinMaxAot)}: ");

    sw = Stopwatch.StartNew();
    for (var i = 0; i < rounds; i++)
    {
      var size = rand.Next(128, 512);
      long min = long.MaxValue / 3, max = min * 2;
      var err = Native.RngCryptoDoubleMinMaxAot(size, min, max, out var out_ptr);
      AssertError(err);

      var result = ToFloats<double>(out_ptr, size);
      Native.FreeBuffer(out_ptr);

      if (result is null || result.Length != size)
        throw new Exception();

      if (result.Min(x => x) < min || result.Max(x => x) > max)
        throw new Exception();

      if (i % (rounds / 10) == 0)
        Console.Write(".");
    }

    sw.Stop();
    t = sw.ElapsedMilliseconds;
    Console.WriteLine($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms\n");
  }

  private static void TestNextCryptoSingleAot(int rounds)
  {
    Console.Write($"{nameof(TestNextCryptoSingleAot)}: ");

    var rand = Random.Shared;
    var sw = Stopwatch.StartNew();
    for (var i = 0; i < rounds; i++)
    {
      var result = Native.NextCryptoSingleAot(out var err);
      AssertError(err);

      if (result < 0)
        throw new Exception();

      if (i % (rounds / 10) == 0)
        Console.Write(".");
    }

    sw.Stop();
    var t = (double)sw.ElapsedMilliseconds;
    Console.WriteLine($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms");

    Console.Write($"Test{nameof(NextCryptoSingleMaxAot)}: ");

    sw = Stopwatch.StartNew();
    for (var i = 0; i < rounds; i++)
    {
      var max = rand.NextInt64();
      var result = Native.NextCryptoSingleMaxAot(max, out var err);
      AssertError(err);

      if (result < 0 || result > max)
        throw new Exception();

      if (i % (rounds / 10) == 0)
        Console.Write(".");
    }

    sw.Stop();
    t = sw.ElapsedMilliseconds;
    Console.WriteLine($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms");

    Console.Write($"Test{nameof(NextCryptoSingleMinMaxAot)}: ");

    sw = Stopwatch.StartNew();
    for (var i = 0; i < rounds; i++)
    {
      long min = long.MaxValue / 3, max = 2 * min;
      var result = Native.NextCryptoSingleMinMaxAot(min, max, out var err);
      AssertError(err);

      if (result < min || result > max)
        throw new Exception();

      if (i % (rounds / 10) == 0)
        Console.Write(".");
    }

    sw.Stop();
    t = sw.ElapsedMilliseconds;
    Console.WriteLine($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms\n");
  }

  private static void TestRngCryptoSingleAot(int rounds)
  {
    Console.Write($"{nameof(TestRngCryptoSingleAot)}: ");

    var rand = Random.Shared;
    var sw = Stopwatch.StartNew();
    for (var i = 0; i < rounds; i++)
    {
      var size = rand.Next(128, 512);
      var err = Native.RngCryptoSingleAot(size, out var out_ptr);
      AssertError(err);

      var result = ToFloats<float>(out_ptr, size);
      Native.FreeBuffer(out_ptr);

      if (result is null || result.Length != size)
        throw new Exception();

      if (i % (rounds / 10) == 0)
        Console.Write(".");
    }

    sw.Stop();
    var t = (double)sw.ElapsedMilliseconds;
    Console.WriteLine($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms");

    Console.Write($"Test{nameof(RngCryptoSingleMaxAot)}: ");

    sw = Stopwatch.StartNew();
    for (var i = 0; i < rounds; i++)
    {
      var max = rand.NextInt64();
      var size = rand.Next(128, 512);
      var err = Native.RngCryptoSingleMaxAot(size, max, out var out_ptr);
      AssertError(err);

      var result = ToFloats<float>(out_ptr, size);
      Native.FreeBuffer(out_ptr);

      if (result is null || result.Length != size)
        throw new Exception();

      if (result.Min(x => x) < 0.0 || result.Max(x => x) > max)
        throw new Exception();

      if (i % (rounds / 10) == 0)
        Console.Write(".");
    }

    sw.Stop();
    t = sw.ElapsedMilliseconds;
    Console.WriteLine($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms");


    Console.Write($"Test{nameof(RngCryptoSingleMinMaxAot)}: ");

    sw = Stopwatch.StartNew();
    for (var i = 0; i < rounds; i++)
    {
      var size = rand.Next(128, 512);
      long min = long.MaxValue / 3, max = min * 2;
      var err = Native.RngCryptoSingleMinMaxAot(size, min, max, out var out_ptr);
      AssertError(err);

      var result = ToFloats<float>(out_ptr, size);
      Native.FreeBuffer(out_ptr);

      if (result is null || result.Length != size)
        throw new Exception();

      if (result.Min(x => x) < min || result.Max(x => x) > max)
        throw new Exception();

      if (i % (rounds / 10) == 0)
        Console.Write(".");
    }

    sw.Stop();
    t = sw.ElapsedMilliseconds;
    Console.WriteLine($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms\n");
  }


  private static void TestNextCryptoDecimalAot(int rounds)
  {
    Console.Write($"{nameof(TestNextCryptoDecimalAot)}: ");

    var rand = Random.Shared;
    var sw = Stopwatch.StartNew();
    for (var i = 0; i < rounds; i++)
    {
      var result = Native.NextCryptoDecimalAot(out var err);
      AssertError(err);

      if (result < 0)
        throw new Exception();

      if (i % (rounds / 10) == 0)
        Console.Write(".");
    }

    sw.Stop();
    var t = (double)sw.ElapsedMilliseconds;
    Console.WriteLine($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms");

    Console.Write($"Test{nameof(NextCryptoDecimalMaxAot)}: ");

    sw = Stopwatch.StartNew();
    for (var i = 0; i < rounds; i++)
    {
      var max = rand.NextInt64();
      var result = Native.NextCryptoDecimalMaxAot(max, out var err);
      AssertError(err);

      if (result < 0m || result > max)
        throw new Exception();

      if (i % (rounds / 10) == 0)
        Console.Write(".");
    }

    sw.Stop();
    t = sw.ElapsedMilliseconds;
    Console.WriteLine($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms");

    Console.Write($"Test{nameof(NextCryptoDecimalMinMaxAot)}: ");

    sw = Stopwatch.StartNew();
    for (var i = 0; i < rounds; i++)
    {
      long min = long.MaxValue / 3, max = 2 * min;
      var result = Native.NextCryptoDecimalMinMaxAot(min, max, out var err);
      AssertError(err);

      if (result < min || result > max)
        throw new Exception();

      if (i % (rounds / 10) == 0)
        Console.Write(".");
    }

    sw.Stop();
    t = sw.ElapsedMilliseconds;
    Console.WriteLine($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms\n");
  }

  private static void TestRngCryptoDecimalAot(int rounds)
  {
    Console.Write($"{nameof(TestRngCryptoDecimalAot)}: ");

    var rand = Random.Shared;
    var sw = Stopwatch.StartNew();
    for (var i = 0; i < rounds; i++)
    {
      var size = rand.Next(128, 512);
      var err = Native.RngCryptoDecimalAot(size, out var out_ptr);
      AssertError(err);

      var result = ToDecimals(out_ptr, size);
      Native.FreeBuffer(out_ptr);

      if (result is null || result.Length != size)
        throw new Exception();

      if (i % (rounds / 10) == 0)
        Console.Write(".");
    }

    sw.Stop();
    var t = (double)sw.ElapsedMilliseconds;
    Console.WriteLine($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms");

    Console.Write($"Test{nameof(RngCryptoDecimalMaxAot)}: ");

    sw = Stopwatch.StartNew();
    for (var i = 0; i < rounds; i++)
    {
      var max = (decimal)rand.NextInt64();
      var size = rand.Next(128, 512);
      var err = Native.RngCryptoDecimalMaxAot(size, max, out var out_ptr);
      AssertError(err);

      var result = ToDecimals(out_ptr, size);
      Native.FreeBuffer(out_ptr);

      if (result is null || result.Length != size)
        throw new Exception();

      if (result.Min(x => x) < 0m || result.Max(x => x) > max)
        throw new Exception();

      if (i % (rounds / 10) == 0)
        Console.Write(".");
    }

    sw.Stop();
    t = sw.ElapsedMilliseconds;
    Console.WriteLine($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms");


    Console.Write($"Test{nameof(RngCryptoDecimalMinMaxAot)}: ");

    sw = Stopwatch.StartNew();
    for (var i = 0; i < rounds; i++)
    {
      var size = rand.Next(128, 512);
      decimal min = long.MaxValue / 3, max = min * 2;
      var err = Native.RngCryptoDecimalMinMaxAot(size, min, max, out var out_ptr);
      AssertError(err);

      var result = ToDecimals(out_ptr, size);
      Native.FreeBuffer(out_ptr);

      if (result is null || result.Length != size)
        throw new Exception();

      if (result.Min(x => x) < min || result.Max(x => x) > max)
        throw new Exception();

      if (i % (rounds / 10) == 0)
        Console.Write(".");
    }

    sw.Stop();
    t = sw.ElapsedMilliseconds;
    Console.WriteLine($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms\n");
  }
}










//private static CError RngCryptoBytesAot(int size, byte** out_ptr, bool no_zeros = true)
//private static CError FillCryptoBytesAot(byte* bytes, int length, bool no_zeros = true)
//private static int NextCryptoInt32Aot(CError* err = null)
//private static int NextCryptoInt32MaxAot(int max, CError* err = null)
//private static int NextCryptoInt32MinMaxAot(int min, int max, CError* err = null)
//private static CError RngCryptoInt32Aot(int size, int** out_ptr)
//private static CError RngCryptoInt32MaxAot(int size, int max, int** out_ptr)
//private static CError RngCryptoInt32MinMaxAot(int size, int min, int max, int** out_ptr)
//private static long NextCryptoInt64Aot(CError* err = null)
//private static long NextCryptoInt64MaxAot(long max, CError* err = null)
//private static long NextCryptoInt64MinMaxAot(long min, long max, CError* err = null)
//private static CError RngCryptoInt64Aot(int size, long** out_ptr)
//private static CError RngCryptoInt64MaxAot(int size, long max, long** out_ptr)
//private static CError RngCryptoInt64MinMaxAot(int size, long min, long max, long** out_ptr)