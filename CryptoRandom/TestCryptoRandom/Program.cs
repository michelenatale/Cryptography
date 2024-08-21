

using System.Diagnostics; 
using michele.natale.Cryptography.Randoms;

namespace CryptoRandomTest;

public class Program
{
  private readonly static object TLock = new();
  private static CryptoRandom Rand { get; } = CryptoRandom.Instance;

  public static void Main()
  {
    var rounds = 100;
    var sw = Stopwatch.StartNew();
    for (int i = 0; i < rounds; i++)
    {
      TestCryptoRandom();
      TestThreadSave();
      if ((i % (rounds / 10)) == 0)
        Console.Write(".");
    }
    Console.WriteLine();
    Console.WriteLine($"rounds = {rounds}; t = {sw.ElapsedMilliseconds}ms");

    Console.ReadLine();
  }

  private static void TestCryptoRandom()
  {
    TestBoolean();

    TestByte();

    TestDouble();

    TestInteger();
    TestUInteger();

    TestLong();
    TestULong();

    TestI128();
    TestUI128();

    TestString();
  }

  private static void TestBoolean()
  {
    var bool1 = Rand.NextCryptoBool();

    var buffer = new bool[1000];
    Rand.FillCryptoBools(buffer);

    buffer = Rand.RngCryptoBools(1000);

    buffer = Enumerable.Range(0, buffer.Length)
      .Select(x => (x & 1) == 0).ToArray();
    Rand.Shuffle(buffer);
  }

  private static void TestDouble()
  {
    var double0 = Rand.NextCryptoDouble();
    var double1 = Rand.NextCryptoDouble();
    var double2 = Rand.NextCryptoDouble(-316542.0, 316542.0);
    var double3 = Rand.NextCryptoDouble(-316542.0, -310000.0);
    for (var i = 0; i < 10; i++)
    {
      var dbl = Rand.NextCryptoDouble(-316542.0, -310000.0);
      if (dbl < -316542.0 || dbl >= -310000.0)
        throw new Exception();
    }

    var buffer = new double[1000];
    Rand.FillCryptoDoubles(buffer);
    Rand.FillCryptoDoubles(buffer, -316542.0, 316542.0);
    var min = buffer.Min();
    var max = buffer.Max();
    if (min < -316542.0 || max >= 316542.0)
      throw new Exception();

    buffer = Rand.RngCryptoDouble(1000, -316542.0, 316542.0);
    min = buffer.Min();
    max = buffer.Max();
    if (min < -316542.0 || max >= 316542.0)
      throw new Exception();

    buffer = Enumerable.Range(0, buffer.Length)
      .Select(x => (double)x).ToArray();
    Rand.Shuffle(buffer);
  }

  private static void TestByte()
  {
    var byte0 = Rand.NextCryptoByte(5, 15);
    var byte1 = Rand.RngCryptoBytes(10);

    var buffer = new byte[1000];
    Rand.FillCryptoBytes(buffer);

    Array.Clear(buffer);
    Rand.FillCryptoBytes(buffer, 5, 10);
    var min = buffer.Min();
    var max = buffer.Max();
    if (min < 5 || max >= 10)
      throw new Exception();

    buffer = Rand.RngCryptoBytes(1000, 5, 10);
    min = buffer.Min();
    max = buffer.Max();
    if (min < 5 || max >= 10)
      throw new Exception();

    buffer = Enumerable.Range(0, buffer.Length)
      .Select(x => (byte)x).ToArray();
    Rand.Shuffle(buffer);
  }

  private static void TestInteger()
  {
    var integer0 = Rand.NextCryptoInt<int>();
    var integer1 = Rand.NextCryptoInt(316542);
    var integer2 = Rand.NextCryptoInt(-316542, 316542);
    if (integer2 < -316542 || integer2 >= 316542)
      throw new Exception();

    var buffer = new int[1000];
    Rand.FillCryptoInts<int>(buffer);
    Rand.FillCryptoInts(buffer, 316542);
    Rand.FillCryptoInts(buffer, -316542, 316542);
    var min = buffer.Min();
    var max = buffer.Max();
    if (min < -316542 || max >= 316542)
      throw new Exception();

    buffer = Rand.RngCryptoInts(1000, -316542, 316542);
    min = buffer.Min();
    max = buffer.Max();
    if (min < -316542 || max >= 316542)
      throw new Exception();

    buffer = Enumerable.Range(0, buffer.Length).ToArray();
    Rand.Shuffle(buffer);
  }

  private static void TestUInteger()
  {
    var uinteger0 = Rand.NextCryptoInt<uint>();
    var uinteger1 = Rand.NextCryptoInt<uint>(316542);
    var uinteger2 = Rand.NextCryptoInt<uint>(316542, 2 * 316542);
    if (uinteger2 < 316542 || uinteger2 >= 2 * 316542)
      throw new Exception();

    var buffer = new uint[1000];
    Rand.FillCryptoInts<uint>(buffer);
    Rand.FillCryptoInts<uint>(buffer, 316542);
    Rand.FillCryptoInts<uint>(buffer, 316542, 2 * 316542);
    var min = buffer.Min();
    var max = buffer.Max();
    if (min < 316542 || max >= 2 * 316542)
      throw new Exception();

    buffer = Rand.RngCryptoInts<uint>(1000, 316542, 2 * 316542);
    min = buffer.Min();
    max = buffer.Max();
    if (min < 316542 || max >= 2 * 316542)
      throw new Exception();

    buffer = Enumerable.Range(0, buffer.Length)
      .Select(x => (uint)x).ToArray();
    Rand.Shuffle(buffer);
  }

  private static void TestLong()
  {
    var long0 = Rand.NextCryptoInt<long>();
    var long1 = Rand.NextCryptoInt<long>(316542);
    var long2 = Rand.NextCryptoInt<long>(-316542, 316542);
    if (long2 < -316542 || long2 >= 316542)
      throw new Exception();

    var buffer = new long[1000];
    Rand.FillCryptoInts<long>(buffer);
    Rand.FillCryptoInts<long>(buffer, 316542);
    Rand.FillCryptoInts<long>(buffer, -316542, 316542);
    var min = buffer.Min();
    var max = buffer.Max();
    if (min < -316542 || max >= 316542)
      throw new Exception();

    buffer = Rand.RngCryptoInts<long>(1000, -316542, 316542);
    min = buffer.Min();
    max = buffer.Max();
    if (min < -316542 || max >= 316542)
      throw new Exception();

    buffer = Enumerable.Range(0, buffer.Length)
      .Select(x => (long)x).ToArray();
    Rand.Shuffle(buffer);
  }

  private static void TestULong()
  {
    var ulong0 = Rand.NextCryptoInt<ulong>();
    var ulong1 = Rand.NextCryptoInt<ulong>(316542);
    var ulong2 = Rand.NextCryptoInt<ulong>(316542, 316542 * 2);
    if (ulong2 < 316542 || ulong2 >= 2 * 316542)
      throw new Exception();

    var buffer = new ulong[1000];
    Rand.FillCryptoInts<ulong>(buffer);
    Rand.FillCryptoInts<ulong>(buffer, 316542);
    Rand.FillCryptoInts<ulong>(buffer, 316542, 316542 * 2);
    var min = buffer.Min();
    var max = buffer.Max();
    if (min < 316542 || max >= 2 * 316542)
      throw new Exception();

    buffer = Rand.RngCryptoInts<ulong>(1000, 316542, 2 * 316542);
    min = buffer.Min();
    max = buffer.Max();
    if (min < 316542 || max >= 2 * 316542)
      throw new Exception();

    buffer = Enumerable.Range(0, buffer.Length)
      .Select(x => (ulong)x).ToArray();
    Rand.Shuffle(buffer);

  }

  private static void TestI128()
  {
    var int128_0 = Rand.NextCryptoInt<Int128>();
    var int128_1 = Rand.NextCryptoInt<Int128>(316542);
    var int128_2 = Rand.NextCryptoInt<Int128>(-316542, 316542 * 2);
    if (int128_2 < -316542 || int128_2 >= 2 * 316542)
      throw new Exception();

    var buffer = new Int128[1000];
    Rand.FillCryptoInts<Int128>(buffer);
    Rand.FillCryptoInts<Int128>(buffer, 316542);
    Rand.FillCryptoInts<Int128>(buffer, -316542, 316542 * 2);
    var min = buffer.Min();
    var max = buffer.Max();
    if (min < -316542 || max >= 2 * 316542)
      throw new Exception();

    buffer = Rand.RngCryptoInts<Int128>(1000, -316542, 316542);
    min = buffer.Min();
    max = buffer.Max();
    if (min < -316542 || max >= 316542)
      throw new Exception();

    buffer = Enumerable.Range(0, buffer.Length)
      .Select(x => (Int128)x).ToArray();
    Rand.Shuffle(buffer);
  }

  private static void TestUI128()
  {
    var uint128_0 = Rand.NextCryptoInt<UInt128>();
    var uint128_1 = Rand.NextCryptoInt<UInt128>(316542);
    var uint128_2 = Rand.NextCryptoInt<UInt128>(316542, 316542 * 2);
    if (uint128_2 < 316542 || uint128_2 >= 2 * 316542)
      throw new Exception();

    var buffer = new UInt128[1000];
    Rand.FillCryptoInts<UInt128>(buffer);
    Rand.FillCryptoInts<UInt128>(buffer, 316542);
    Rand.FillCryptoInts<UInt128>(buffer, 316542, 316542 * 2);
    var min = buffer.Min();
    var max = buffer.Max();
    if (min < 316542 || max >= 2 * 316542)
      throw new Exception();

    buffer = Rand.RngCryptoInts<UInt128>(1000, 316542, 2 * 316542);
    min = buffer.Min();
    max = buffer.Max();
    if (min < 316542 || max >= 2 * 316542)
      throw new Exception();

    buffer = Enumerable.Range(0, buffer.Length)
      .Select(x => (UInt128)x).ToArray();
    Rand.Shuffle(buffer);
  }

  private static void TestString()
  {
    var string1 = Rand.NextString(3, "YourBeechBarSalad");
    var string2 = Rand.NextString(10, ICryptoRandom.AlphaLowerUpperNumeric);

    var alum = ICryptoRandom.AlphaLowerUpperNumeric;
    Rand.Shuffle(ref alum);
  }


  private static void TestThreadSave()
  {
    TestParallelFor();
    TestTaskFactor();
    TestMultiThread();
  }

  private static void TestParallelFor()
  {
    var cnt = 10;
    var crand = CryptoRandom.Shared;
    var parallelOptions = new ParallelOptions
    {
      MaxDegreeOfParallelism = Environment.ProcessorCount
    };

    var rarray = Enumerable.Range(0, cnt).Select(x => new int[10]).ToArray();

    Parallel.For(0, cnt, parallelOptions,
    (i) =>
    {
      lock (TLock)
        crand.FillCryptoInts<int>(rarray[i]);
    });
    if (rarray[crand.NextCryptoInt(rarray.Length)][crand.NextCryptoInt(rarray.First().Length)] == 0)
      throw new Exception();
  }

  private static void TestTaskFactor()
  {

    var cnt = 10;
    var crand = CryptoRandom.Shared;
    var cts = new CancellationTokenSource();
    var rarray1 = Enumerable.Range(0, cnt).Select(x => new int[10]).ToArray();
    var rarray2 = Enumerable.Range(0, cnt).Select(x => new int[10]).ToArray();
    var rarray3 = Enumerable.Range(0, cnt).Select(x => new int[10]).ToArray();

    var tasks = new[]
    {
      Task.Factory.StartNew(() => FWorker(rarray1), cts.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default),
      Task.Factory.StartNew(() => FWorker(rarray2), cts.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default),
      Task.Factory.StartNew(() => FWorker(rarray3), cts.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default),
    };

     Task.WaitAll(tasks);
    if (rarray1[crand.NextCryptoInt(rarray1.Length)][crand.NextCryptoInt( rarray1.First().Length)] == 0)
      throw new Exception();
    if (rarray2[crand.NextCryptoInt(rarray2.Length)][crand.NextCryptoInt(rarray2.First().Length)] == 0)
      throw new Exception();
    if (rarray3[crand.NextCryptoInt(rarray3.Length)][crand.NextCryptoInt(rarray3.First().Length)] == 0)
      throw new Exception();
  }

  private static void TestMultiThread()
  {
    var cnt = 10;
    var crand = CryptoRandom.Shared;
    var rarray1 = Enumerable.Range(0, cnt).Select(x => new int[10]).ToArray();
    var rarray2 = Enumerable.Range(0, cnt).Select(x => new int[10]).ToArray();
    var rarray3 = Enumerable.Range(0, cnt).Select(x => new int[10]).ToArray();
    Thread t1 = new Thread(TWorker!);
    Thread t2 = new Thread(TWorker!);
    Thread t3 = new Thread(TWorker!);
    t1.Start(rarray1);
    t2.Start(rarray2);
    t3.Start(rarray3);

    t1.Join();
    t2.Join();
    t3.Join();

    if (rarray1[crand.NextCryptoInt(rarray1.Length)][crand.NextCryptoInt(rarray1.First().Length)] == 0)
      throw new Exception();
    if (rarray2[crand.NextCryptoInt(rarray2.Length)][crand.NextCryptoInt(rarray2.First().Length)] == 0)
      throw new Exception();
    if (rarray3[crand.NextCryptoInt(rarray3.Length)][crand.NextCryptoInt(rarray3.First().Length)] == 0)
      throw new Exception();
  }


  private static void FWorker(int[][] rarray)
  {
    var crand = CryptoRandom.Shared;
    for (var i = 0; i < rarray.Length; i++)
      lock (TLock)
        crand.FillCryptoInts<int>(rarray[i]);
  }

  private static void TWorker(object obj)
  {
    var rarray = (int[][])obj;
    var crand = CryptoRandom.Shared;
    for (var i = 0; i < rarray.Length; i++)
      lock (TLock)
        crand.FillCryptoInts<int>(rarray[i]);
  }
}