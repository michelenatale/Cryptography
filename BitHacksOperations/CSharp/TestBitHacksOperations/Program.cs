

using System.Diagnostics;
using System.Runtime.CompilerServices;


//https://catonmat.net/low-level-bit-hacks


namespace michele.natale.BitHacksOperationsTest;


using BitHacksOperations;

public class Program
{
  public static void Main()
  {

    var sw = Stopwatch.StartNew();

    Test();

    sw.Stop();
    Console.WriteLine(sw.ElapsedMilliseconds);

    Console.WriteLine();
    Console.WriteLine("FINISH");
    Console.WriteLine();
    Console.ReadLine();
  }

  private static void Test()
  {

    //Thanks to https://catonmat.net/low-level-bit-hacks
    TestIsBinary();       //Bit-Hack #0.  Bitdarstellung prüfen.
    TestIsOdd();          //Bit-Hack #1.  Prüfe, ob die Ganzzahl gerade ist.
    TestIsEven();         //Bit-Hack #1.  Prüfe, ob die Ganzzahl ungerade ist.
    TestIsPosXSet();      //Bit-Hack #2.  Testen, ob das n-te Bit gesetzt ist.
    TestPosXSet();        //Bit-Hack #3.  Set das n-te Bit.
    TestPosXReset();      //Bit-Hack #4.  Reset das n-te Bit.
    TestPosXToggle();     //Bit-Hack #5.  Schaltet das n-te Bit um.
    TestTurnOffR1();      //Bit-Hack #6.  Schaltet das ganz rechte 1-Bit aus.
    TestIsolateR1();      //Bit-Hack #7.  Isolieren das ganz rechte 1-Bit.
    TestPropagateR1();    //Bit-Hack #8.  Propagiere das äußerste rechte 1-Bit nach rechts.
    TestIsolateR0();      //Bit-Hack #9.  Isolieren das 0-Bit ganz rechts.
    TestTurnOnR0();       //Bit-Hack #10. Schalte das 0-Bit ganz rechts ein.


    //Many other interesting examples. (Full Generic)
    TestToPosX();
    TestIsPower2();
    TestIncrement();
    TestToBinaryString();
    TestCountBitSet();
    TestToParity();
    TestBitSwap();
    TestReversBits();
    TestModuloPower2();
    TestNextPermutationWR();
    TestLeastMostSignificantSetBit(); //  Same Bit-Hack #7, but with lsb msb
    TestClearsLsbMsbToPosX();
    TestDivide2Mult2();
    TestToLowerToUpper();
    TestLowerUpperNumericAlpha();
    TestIndexFirstLastSetBit(); //        Same Bit-Hack #7, but with index
    TestNegate();
    TestModuloRange();
    TestSign();
    TestAbs();
    TestHasZeroBytes();
    TestRotateLeftRightBitwise();
    TestIsUnsignedType();
    TestIsOppositeSign();
    TestMinMax();
    TestEquality();
    TestSwap();
    TestShuffle();
  }

  private static void TestIsBinary()
  {
    //Bit-Hack #0.  Bitdarstellung prüfen.

    var rand = Random.Shared;

    for (var i = 0; i < 1_000; i++)
    {
      var n = rand.Next();
      var s1 = Convert.ToString(n, 2);
      var s2 = BitHacksOPs.ToBinary(n);
      if (!s1.SequenceEqual(s2))
        throw new Exception();
      if (!BitHacksOPs.IsBinary(s2))
        throw new Exception();
    }
  }

  private static void TestIsOdd()
  {
    //Bit-Hack #1.  Prüfe, ob die Ganzzahl ungerade ist.

    var rand = Random.Shared;

    for (var i = 0; i < 1_000; i++)
    {
      var n = rand.Next();
      if ((n & 1) != 1) n++;
      if (!BitHacksOPs.IsOdd(n))
        throw new Exception();
    }
  }

  private static void TestIsEven()
  {
    //Bit-Hack #1.  Prüfe, ob die Ganzzahl gerade ist.

    var rand = Random.Shared;

    for (var i = 0; i < 1_000; i++)
    {
      var n = rand.Next();
      if ((n & 1) == 1) n++;
      if (!BitHacksOPs.IsEven(n))
        throw new Exception();
    }
  }

  private static void TestIsPosXSet()
  {
    //Bit-Hack #2.  Testen, ob das n-te Bit gesetzt ist.

    var rand = Random.Shared;

    for (var i = 0; i < 1_000; i++)
    {
      var number = rand.Next();
      var idx = rand.Next(0, 31);
      if (!BitHacksOPs.IsPosXSet(number, idx))
        number = BitHacksOPs.SetPosX(number, idx);
      if (!BitHacksOPs.IsPosXSet(number, idx))
        throw new Exception();
    }
  }

  private static void TestPosXSet()
  {
    //Bit-Hack #3.  Set das n-te Bit.

    var rand = Random.Shared;

    for (var i = 0; i < 1_000; i++)
    {
      var number = rand.Next();
      var idx = rand.Next(0, 31);
      if (!BitHacksOPs.IsPosXSet(number, idx))
        number = BitHacksOPs.SetPosX(number, idx);
      if (!BitHacksOPs.IsPosXSet(number, idx))
        throw new Exception();
    }
  }

  private static void TestPosXReset()
  {
    //Bit-Hack #4.  Reset das n-te Bit.

    var rand = Random.Shared;

    for (var i = 0; i < 1_000; i++)
    {
      var number = rand.Next();
      var idx = rand.Next(0, 31);
      if (BitHacksOPs.IsPosXSet(number, idx))
        number = BitHacksOPs.UnSetPosX(number, idx);
      if (BitHacksOPs.IsPosXSet(number, idx))
        throw new Exception();
    }
  }

  private static void TestPosXToggle()
  {
    //Bit-Hack #5.  Schalten Sie das n-te Bit um.

    var rand = Random.Shared;

    for (var i = 0; i < 1_000; i++)
    {
      var number = rand.Next();
      var idx = rand.Next(0, 31);
      var tf = BitHacksOPs.IsPosXSet(number, idx);
      number = BitHacksOPs.TogglePosX(number, idx);
      if (BitHacksOPs.IsPosXSet(number, idx) == tf)
        throw new Exception();
    }
  }

  private static void TestTurnOffR1()
  {
    //Bit-Hack #6.  Schaltet das ganz rechte 1-Bit aus.

    var rand = Random.Shared;

    for (var i = 0; i < 1_000; i++)
    {
      int idx = 0;
      var number = rand.Next();
      for (var j = 0; j < 32; j++)
        if (BitHacksOPs.IsPosXSet(number, j))
        {
          idx = j; break;
        }

      if (!BitHacksOPs.IsPosXSet(number, idx))
        throw new Exception();

      number = BitHacksOPs.TurnOffR1(number);
      if (BitHacksOPs.IsPosXSet(number, idx))
        throw new Exception();
    }
  }

  private static void TestIsolateR1()
  {
    //Bit-Hack #7.  Isolieren Sie das ganz rechte 1-Bit.

    //Gibt den Positionswert des 1-Bit ganz rechts zurück (1 << idx) 
    //was einem power 2 entspricht.

    //Returns the position value of the rightmost 1-bit (1 << idx),
    //which corresponds to a power 2.

    var rand = Random.Shared;

    for (var i = 0; i < 1_000; i++)
    {
      int idx = 0;
      var number = rand.Next();
      for (var j = 0; j < 32; j++)
        if (BitHacksOPs.IsPosXSet(number, j))
        {
          idx = j; break;
        }

      if (!BitHacksOPs.IsPosXSet(number, idx))
        throw new Exception();

      var bitstr = BitHacksOPs.ToBinary(number);
      number = BitHacksOPs.IsolateR1(number);

      if (!int.IsPow2(number))
        throw new Exception();
      if (number != 1 << idx)
        throw new Exception();
    }
  }

  private static void TestPropagateR1()
  {
    //Bit-Hack #8.  Propagiere das äußerste rechte 1-Bit nach rechts.

    var rand = Random.Shared;

    for (var i = 0; i < 1_000; i++)
    {
      int idx = 0;
      var number = rand.Next();
      for (var j = 0; j < 32; j++)
        if (BitHacksOPs.IsPosXSet(number, j))
        {
          idx = j; break;
        }

      if (!BitHacksOPs.IsPosXSet(number, idx))
        throw new Exception();

      number = BitHacksOPs.PropagateR1(number);

      for (var j = 0; j < idx + 1; j++)
        if (!BitHacksOPs.IsPosXSet(number, j))
          throw new Exception();
    }
  }

  private static void TestIsolateR0()
  {
    //Bit-Hack #9.  Isolieren Sie das 0-Bit ganz rechts.

    var rand = Random.Shared;

    for (var i = 0; i < 1_000; i++)
    {
      int idx = 0;
      var number = rand.Next();
      for (var j = 0; j < 32; j++)
        if (!BitHacksOPs.IsPosXSet(number, j))
        {
          idx = j; break;
        }

      if (BitHacksOPs.IsPosXSet(number, idx))
        throw new Exception();

      number = BitHacksOPs.IsolateR0(number);

      if (!int.IsPow2(number))
        throw new Exception();
      if (number != 1 << idx)
        throw new Exception();
    }
  }

  private static void TestTurnOnR0()
  {
    //Bit-Hack #10. Schalte das 0-Bit ganz rechts ein.

    var rand = Random.Shared;

    for (var i = 0; i < 1_000; i++)
    {
      int idx = 0;
      var number = rand.Next();
      for (var j = 0; j < 32; j++)
        if (!BitHacksOPs.IsPosXSet(number, j))
        {
          idx = j; break;
        }

      if (BitHacksOPs.IsPosXSet(number, idx))
        throw new Exception();

      number = BitHacksOPs.TurnOnR0(number);
      if (!BitHacksOPs.IsPosXSet(number, idx))
        throw new Exception();
    }
  }

  private static void TestToPosX()
  {
    var rand = Random.Shared;

    var typesize = Unsafe.SizeOf<int>();
    var rangesize = 8 * typesize;

    var number = rand.Next();
    var bitstr = BitHacksOPs.ToBinary(number);
    var lsb = BitHacksOPs.ToPosX(number, 0);
    var msb = BitHacksOPs.ToPosX(number, rangesize - 1);

    for (var i = 0; i < 1_000; i++)
    {
      number = rand.Next();
      var idx = rand.Next(0, rangesize);
      bitstr = BitHacksOPs.ToBinary(number);
      var length = bitstr.Length;
      var bitvalue = BitHacksOPs.ToPosX(number, idx);
      if (idx < length && bitvalue != int.Parse(bitstr[length - 1 - idx].ToString()))
        throw new Exception();
    }
  }

  private static void TestIsPower2()
  {
    var list = new List<int>();
    for (int i = 0; i < 70_000; i++)
      if (BitHacksOPs.IsPower2(i))
        list.Add(i);
  }

  private static void TestIncrement()
  {
    ulong iterul = 0;
    for (int i = 0; i < 10; i++)
      iterul = BitHacksOPs.ITP(iterul);

    iterul = 0;
    for (int i = 0; i < 10; i++)
      iterul = BitHacksOPs.ITP(iterul, 2ul);

    uint iterui = 0;
    for (int i = 0; i < 10; i++)
      iterui = BitHacksOPs.ITM(iterui);

    iterul = 0;
    for (int i = 0; i < 10; i++)
      iterui = BitHacksOPs.ITM(iterui, 5u);

    //Bitwise Increment
    iterul = ulong.MaxValue - 500;
    for (int i = 0; i < 1000; i++)
    {
      iterul = BitHacksOPs.Increment(iterul);
      if (i > 500 && iterul != (ulong)(i % 500))
        throw new Exception();
    }

    //Bitwise Increment (+1)
    iterul = ulong.MaxValue - 500;
    for (int i = 0; i < 1000; i++)
    {
      iterul = BitHacksOPs.Increment(iterul, 500ul);
      if (i > 500 && iterul != (ulong)((i - 1) % 500))
        throw new Exception();
    }
  }

  private static void TestToBinaryString()
  {
    var rand = Random.Shared;

    for (var i = 0; i < 1_000; i++)
    {
      var n = rand.Next();
      var s1 = Convert.ToString(n, 2);
      var s2 = BitHacksOPs.ToBinary(n);
      if (!s1.SequenceEqual(s2))
        throw new Exception();
    }
  }

  private static void TestCountBitSet()
  {
    var rand = Random.Shared;

    for (var i = 0; i < 1_000; i++)
    {
      var n = rand.Next();
      var cnt = BitHacksOPs.CountBitsSet(n);
      var bitstr = BitHacksOPs.ToBinary(n);
      if (cnt != bitstr.Count(x => x == '1'))
        throw new Exception();
    }
  }

  private static void TestToParity()
  {
    var rand = Random.Shared;

    for (var i = 0; i < 1_000; i++)
    {
      var n = rand.Next();
      var is_parity_odd = BitHacksOPs.ToParity(n); //parity ungerade?
      var bitstr_parity = (BitHacksOPs.ToBinary(n).Count(x => x == '1') & 1) == 1;
      if (is_parity_odd != bitstr_parity)
        throw new Exception();
    }

    for (var i = 0; i < 1_000; i++)
    {
      var n = (uint)rand.Next();
      var is_parity_odd = BitHacksOPs.ToParity(n); //parity ungerade?
      var bitstr_parity = (BitHacksOPs.ToBinary(n).Count(x => x == '1') & 1) == 1;
      if (is_parity_odd != bitstr_parity)
        throw new Exception();
    }
  }

  private static void TestBitSwap()
  {
    var rand = Random.Shared;

    for (var i = 0; i < 1_000; i++)
    {
      var n = rand.Next();

      int from = 1, to = 5, length = 3;
      var bitstr1 = BitHacksOPs.ToBinary(n);
      var swap = BitHacksOPs.SwapBits(n, from, to, length); //Von rechts gesehen
      var bitstr2 = BitHacksOPs.ToBinary(swap);

      var sub10 = bitstr1.Substring(bitstr1.Length - from - length, length);
      var sub20 = bitstr2.Substring(bitstr2.Length - from - length, length);

      var sub11 = bitstr1.Substring(bitstr1.Length - to - length, length);
      var sub21 = bitstr2.Substring(bitstr2.Length - to - length, length);

      if (!sub10.SequenceEqual(sub21)) throw new Exception();
      if (!sub11.SequenceEqual(sub20)) throw new Exception();

      swap = BitHacksOPs.SwapBits(swap, from, to, length); //Von rechts gesehen
      if (n != swap) throw new Exception();
    }

    for (var i = 0; i < 1_000; i++)
    {
      var n = (ulong)rand.NextInt64() + (ulong)rand.NextInt64();

      int from = 1, to = 5, length = 3;
      var bitstr1 = BitHacksOPs.ToBinary(n);
      var swap = BitHacksOPs.SwapBits(n, from, to, length); //Von rechts gesehen
      var bitstr2 = BitHacksOPs.ToBinary(swap);

      var sub10 = bitstr1.Substring(bitstr1.Length - from - length, length);
      var sub20 = bitstr2.Substring(bitstr2.Length - from - length, length);

      var sub11 = bitstr1.Substring(bitstr1.Length - to - length, length);
      var sub21 = bitstr2.Substring(bitstr2.Length - to - length, length);

      if (!sub10.SequenceEqual(sub21)) throw new Exception();
      if (!sub11.SequenceEqual(sub20)) throw new Exception();

      swap = BitHacksOPs.SwapBits(swap, from, to, length); //Von rechts gesehen
      if (n != swap) throw new Exception();
    }


    for (var i = 0; i < 1_000; i++)
    {
      var n = (uint)rand.Next() + (uint)rand.Next();

      int from = 1, to = 5, length = 3;
      var bitstr1 = BitHacksOPs.ToBinary(n);
      var swap = BitHacksOPs.SwapBits(n, from, to, length); //Von rechts gesehen
      var bitstr2 = BitHacksOPs.ToBinary(swap);

      var sub10 = bitstr1.Substring(bitstr1.Length - from - length, length);
      var sub20 = bitstr2.Substring(bitstr2.Length - from - length, length);

      var sub11 = bitstr1.Substring(bitstr1.Length - to - length, length);
      var sub21 = bitstr2.Substring(bitstr2.Length - to - length, length);

      if (!sub10.SequenceEqual(sub21)) throw new Exception();
      if (!sub11.SequenceEqual(sub20)) throw new Exception();

      swap = BitHacksOPs.SwapBits(swap, from, to, length); //Von rechts gesehen
      if (n != swap) throw new Exception();
    }
  }

  private static void TestReversBits()
  {
    var rand = Random.Shared;

    for (var i = 0; i < 1_000; i++)
    {
      var n = rand.Next();
      var start = rand.Next(0, 10);
      var length = rand.Next(2, (int)Math.Log2(n) - start);

      var bitstr1 = BitHacksOPs.ToBinary(n);
      var revers = BitHacksOPs.ReversBits(n, start, length); //Von rechts gesehen
      var bitstr2 = BitHacksOPs.ToBinary(revers);

      var bitstr3 = bitstr1.ToCharArray();
      Array.Reverse(bitstr3, bitstr3.Length - start - length, length);

      if (!bitstr2.SequenceEqual(bitstr3))
        throw new Exception();

      revers = BitHacksOPs.ReversBits(revers, start, length);
      bitstr2 = BitHacksOPs.ToBinary(revers);

      if (!bitstr2.SequenceEqual(bitstr1))
        throw new Exception();

      if (n != revers)
        throw new Exception();
    }
  }

  private static void TestModuloPower2()
  {
    var rand = Random.Shared;

    for (var i = 0; i < 1_000; i++)
    {
      var n = rand.Next();
      var s = rand.Next(1, 31);
      var m = 1 << s; //Power2

      //m must be a power 2, ortherwise Exception is thrown
      var modulo = BitHacksOPs.ModuloPower2(n, m);

      if (modulo != n % m)
        throw new Exception();
    }
  }

  private static void TestNextPermutationWR()
  {

    //https://www.vb-paradise.de/index.php/Thread/95752-Permutationen/?postID=1171669#post1171669

    var cnt = 100;
    var rand = Random.Shared;

    for (var i = 0; i < cnt; i++)
    {
      int n = 0;
      while (n == 0 || n == int.MaxValue)
        n = rand.Next();

      var perm = BitHacksOPs.NextPermutationWR(n);
      if (perm == n) throw new Exception();
    }

    for (var i = 0; i < cnt; i++)
    {
      uint n = 0;
      while (n == 0 || n == uint.MaxValue)
        n = (uint)rand.Next() + (uint)rand.Next();

      var perm = BitHacksOPs.NextPermutationWR(n);
      if (perm == n) throw new Exception();
    }

    for (var i = 0; i < cnt; i++)
    {
      long n = 0;
      while (n == 0 || n == long.MaxValue)
        n = rand.NextInt64();

      var perm = BitHacksOPs.NextPermutationWR(n);
      if (perm == n) throw new Exception();
    }

    for (var i = 0; i < cnt; i++)
    {
      ulong n = 0;
      while (n == 0 || n == ulong.MaxValue)
        n = (ulong)rand.NextInt64() + (ulong)rand.NextInt64();

      var perm = BitHacksOPs.NextPermutationWR(n);
      if (perm == n) throw new Exception();
    }
  }

  private static void TestLeastMostSignificantSetBit()
  {

    //The position value is calculated e.g. 100 = 4
    //Es wird der Positions-Wert ausgerechnet z.b. 100 = 4

    var cnt = 100;
    var rand = Random.Shared;

    for (var i = 0; i < cnt; i++)
    {
      var n = rand.Next();
      //var str = Convert.ToString(n, 2); 
      var irm = 1 << BitHacksOPs.ToIndexRightmostSetBit(n);
      var lsb = BitHacksOPs.ToLeastSignificantSetBit(n);
      var msb = BitHacksOPs.ToMostSignificantSetBit(n);
      var ilm = 1 << BitHacksOPs.ToIndexLeftmostSetBit(n);

      if (irm != lsb) throw new Exception();
      if (ilm != msb) throw new Exception();
    }

    for (var i = 0; i < cnt; i++)
    {
      var n = (uint)rand.Next() + (uint)rand.Next();
      //var str = Convert.ToString(n, 2); 
      var irm = 1u << BitHacksOPs.ToIndexRightmostSetBit(n);
      var lsb = BitHacksOPs.ToLeastSignificantSetBit(n);
      var msb = BitHacksOPs.ToMostSignificantSetBit(n);
      var ilm = 1u << BitHacksOPs.ToIndexLeftmostSetBit(n);

      if (irm != lsb) throw new Exception();
      if (ilm != msb) throw new Exception();
    }

    for (var i = 0; i < cnt; i++)
    {
      var n = rand.NextInt64();
      //var str = Convert.ToString(n, 2); 
      var irm = 1L << BitHacksOPs.ToIndexRightmostSetBit(n);
      var lsb = BitHacksOPs.ToLeastSignificantSetBit(n);
      var msb = BitHacksOPs.ToMostSignificantSetBit(n);
      var ilm = 1L << BitHacksOPs.ToIndexLeftmostSetBit(n);

      if (irm != lsb) throw new Exception();
      if (ilm != msb) throw new Exception();
    }

    for (var i = 0; i < cnt; i++)
    {
      var n = (ulong)rand.NextInt64() + (ulong)rand.NextInt64();
      //var str = n.ToString("B", CultureInfo.InvariantCulture);
      var irm = 1ul << BitHacksOPs.ToIndexRightmostSetBit(n);
      var lsb = BitHacksOPs.ToLeastSignificantSetBit(n);
      var msb = BitHacksOPs.ToMostSignificantSetBit(n);
      var ilm = 1ul << BitHacksOPs.ToIndexLeftmostSetBit(n);

      if (irm != lsb) throw new Exception();
      if (ilm != msb) throw new Exception();
    }
  }

  public static void TestClearsLsbMsbToPosX()
  {
    var cnt = 1_000;
    var rand = Random.Shared;

    for (var i = 0; i < cnt; i++)
    {
      var n = rand.Next();
      var str = Convert.ToString(n, 2);
      var length = (int)Math.Log2(n) + 1;

      var i1 = rand.Next(0, length);
      var n1a = BitHacksOPs.ClearsLsbToPosX(n, i1);
      var l1 = n1a == 0 ? 1 : (int)Math.Log2(n1a) + 1;
      var s1a = Convert.ToString(n1a, 2);
      var s1b = str[..(str.Length - 1 - i1)] + new string('0', i1 + 1);
      var n1b = Convert.ToInt32(s1b, 2);

      if (n1a != n1b) throw new Exception();

      var i2 = rand.Next(0, length);
      var n2a = BitHacksOPs.ClearsMsbToPosX(n, i2);
      var l2 = n2a == 0 ? 1 : (int)Math.Log2(n2a) + 1;
      var s2a = Convert.ToString(n2a, 2);
      var s2b = str.Substring(str.Length - i2, i2);
      s2b = string.IsNullOrEmpty(s2b) ? "0" : s2b;
      var n2b = Convert.ToInt32(s2b, 2);

      if (n2a != n2b) throw new Exception();
    }



    for (var i = 0; i < cnt; i++)
    {
      var n = rand.NextInt64();
      var str = Convert.ToString(n, 2);
      var length = (int)Math.Log2(n) + 1;

      var i1 = rand.Next(0, length);
      var n1a = BitHacksOPs.ClearsLsbToPosX(n, i1);
      var l1 = n1a == 0 ? 1 : (int)Math.Log2(n1a) + 1;
      var s1a = Convert.ToString(n1a, 2);
      var s1b = str[..(str.Length - 1 - i1)] + new string('0', i1 + 1);
      var n1b = Convert.ToInt64(s1b, 2);

      if (n1a != n1b) throw new Exception();

      var i2 = rand.Next(0, length);
      var n2a = BitHacksOPs.ClearsMsbToPosX(n, i2);
      var l2 = n2a == 0 ? 1 : (int)Math.Log2(n2a) + 1;
      var s2a = Convert.ToString(n2a, 2);
      var s2b = str.Substring(str.Length - i2, i2);
      s2b = string.IsNullOrEmpty(s2b) ? "0" : s2b;
      var n2b = Convert.ToInt64(s2b, 2);

      if (n2a != n2b) throw new Exception();
    }
  }

  public static void TestDivide2Mult2()
  {
    var cnt = 1_000;
    var rand = Random.Shared;

    for (var i = 0; i < cnt; i++)
    {
      var n = rand.Next();

      var nm2 = BitHacksOPs.Mult2(n);
      var nd2 = BitHacksOPs.Divide2(n);

      if (nm2 != n * 2) throw new Exception();
      if (nd2 != n / 2) throw new Exception();

    }
  }

  public static void TestLowerUpperNumericAlpha()
  {
    var lower_alpha = BitHacksOPs.ToLowerAlpha();
    var upper_alpha = BitHacksOPs.ToUpperAlpha();
    var numeric_alpha = BitHacksOPs.ToNumericAlpha();
    var lower_upper_alpha = BitHacksOPs.ToLowerUpperAlpha();
    var lower_numeric_alpha = BitHacksOPs.ToLowerNumericAlpha();
    var upper_numeric_alpha = BitHacksOPs.ToUpperNumericAlpha();
    var lower_upper_numeric_alpha = BitHacksOPs.ToLowerUpperNumericAlpha();

    var idx_a = BitHacksOPs.IndexOfAlphabeth('a');
    var idx_A = BitHacksOPs.IndexOfAlphabeth('A');
    var idx_0 = BitHacksOPs.IndexOfAlphabeth('0');
    var idx_z = BitHacksOPs.IndexOfAlphabeth('z');
    var idx_Z = BitHacksOPs.IndexOfAlphabeth('Z');
    var idx_9 = BitHacksOPs.IndexOfAlphabeth('9');
  }

  public static void TestToLowerToUpper()
  {
    var cnt = 1_000;
    var rand = Random.Shared;

    string chin = "中文", kore = "한국어";
    var abc = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

    for (var i = 0; i < cnt; i++)
    {
      var n = rand.GetItems<char>(abc, 1).First();
      var lower = BitHacksOPs.ToLower(n);
      var upper = BitHacksOPs.ToUpper(n);

      if (lower != char.ToLower(n)) throw new Exception();
      if (upper != char.ToUpper(n)) throw new Exception();
    }

    for (var i = 0; i < cnt; i++)
    {
      var length = rand.Next(1, 10);

      var n = rand.GetItems<char>(abc, length);
      var lower = BitHacksOPs.ToLower(n);
      var upper = BitHacksOPs.ToUpper(n);

      var str = new string(n);

      if (!lower.SequenceEqual(str.ToLower())) throw new Exception();
      if (!upper.SequenceEqual(str.ToUpper())) throw new Exception();
    }

    var abcd = (chin + kore + abc + chin + kore).ToCharArray();
    rand.Shuffle(abcd);

    for (var i = 0; i < cnt; i++)
    {
      var length = rand.Next(1, 10);

      var n = rand.GetItems(abcd, length);
      var lower = BitHacksOPs.ToLower(n);
      var upper = BitHacksOPs.ToUpper(n);

      var str = new string(n);

      if (!lower.SequenceEqual(str.ToLower())) throw new Exception();
      if (!upper.SequenceEqual(str.ToUpper())) throw new Exception();
    }
  }

  private static void TestIndexFirstLastSetBit()
  {

    //The index (0-base) is calculated here.
    //Der Index (0-Base) wird hier ausgerechnet.

    var cnt = 1_000;
    var rand = Random.Shared;

    for (var i = 0; i < cnt; i++)
    {
      var n = rand.Next();
      //var str = Convert.ToString(n, 2); 
      var last = BitHacksOPs.IndexLastSetBit(n);
      var first = BitHacksOPs.IndexFirstSetBit(n);

      if (!BitHacksOPs.IsPosXSet(n, first)) throw new Exception();
      if (!BitHacksOPs.IsPosXSet(n, last)) throw new Exception();
    }
  }

  private static void TestNegate()
  {
    var cnt = 1_000;
    var rand = Random.Shared;

    for (var i = 0; i < cnt; i++)
    {
      var ni32 = rand.Next();
      var n1 = BitHacksOPs.Negate(ni32);
      var stri32 = Convert.ToString(ni32, 2);
      var stri32n = Convert.ToString(n1, 2);

      var ni64 = rand.NextInt64();
      var n2 = BitHacksOPs.Negate(ni64);
      var stri64 = Convert.ToString(ni64, 2);
      var stri64n = Convert.ToString(n2, 2);

      var nui32 = (uint)rand.NextInt64() + (uint)rand.NextInt64();
      var n3 = BitHacksOPs.Negate(nui32);
      var strui32 = Convert.ToString(nui32, 2);
      var strui32n = Convert.ToString(n3, 2);

      var nui64 = (ulong)rand.NextInt64() + (ulong)rand.NextInt64();
      var n4 = BitHacksOPs.Negate(nui64);
      var strui64 = Convert.ToString((long)nui64, 2);
      var strui64n = Convert.ToString((long)n4, 2);

      if (ni32 == -ni32) throw new Exception();
      if (nui32 == -nui32) throw new Exception();
      if (ni64 == -ni64) throw new Exception();
      if (nui64 == -(Int128)nui64) throw new Exception();
    }
  }

  private static void TestModuloRange()
  {
    var cnt = 1_000;
    var rand = Random.Shared;

    for (var i = 0; i < cnt; i++)
    {
      var ni32 = rand.Next();
      var n1 = BitHacksOPs.ModuloRange(ni32);
      var m1 = ni32 % 32;

      var ni64 = rand.NextInt64();
      var n2 = BitHacksOPs.ModuloRange(ni64);
      var m2 = ni64 % 64;

      var nui32 = (uint)rand.NextInt64() + (uint)rand.NextInt64();
      var n3 = BitHacksOPs.ModuloRange(nui32);
      var m3 = nui32 % 32;

      var nui64 = (ulong)rand.NextInt64() + (ulong)rand.NextInt64();
      var n4 = BitHacksOPs.ModuloRange(nui64);
      var m4 = nui64 % 64;

      if (n1 != m1) throw new Exception();
      if (n2 != m2) throw new Exception();
      if (n3 != m3) throw new Exception();
      if (n4 != m4) throw new Exception();
    }
  }

  private static void TestSign()
  {
    var cnt = 1_000;
    var rand = Random.Shared;

    for (var i = 0; i < cnt; i++)
    {
      var ni8 = (sbyte)rand.Next();
      var n1 = BitHacksOPs.Sign(ni8);
      var m1 = sbyte.Sign(ni8);

      var ni16 = (short)rand.Next();
      var n2 = BitHacksOPs.Sign(ni16);
      var m2 = short.Sign(ni16);

      var ni32 = rand.Next();
      if (int.IsOddInteger(rand.Next()))
        ni32 = -ni32;
      var n3 = BitHacksOPs.Sign(ni32);
      var m3 = int.Sign(ni32);

      var ni64 = rand.NextInt64();
      if (int.IsOddInteger(rand.Next()))
        ni64 = -ni64;
      var n4 = BitHacksOPs.Sign(ni64);
      var m4 = long.Sign(ni64);

      if (n1 != m1) throw new Exception();
      if (n2 != m2) throw new Exception();
      if (n3 != m3) throw new Exception();
      if (n4 != m4) throw new Exception();
    }
  }

  private static void TestAbs()
  {
    var cnt = 1_000;
    var rand = Random.Shared;

    for (var i = 0; i < cnt; i++)
    {
      var ni8 = (sbyte)rand.Next();
      var n1 = BitHacksOPs.Abs(ni8);
      var m1 = ni8 == sbyte.MinValue ? ni8 : sbyte.Abs(ni8);

      var ni16 = (short)rand.Next();
      var n2 = BitHacksOPs.Abs(ni16);
      var m2 = ni16 == short.MinValue ? ni16 : short.Abs(ni16);

      var ni32 = rand.Next();
      if (int.IsOddInteger(rand.Next()))
        ni32 = -ni32;
      var n3 = BitHacksOPs.Abs(ni32);
      var m3 = int.Abs(ni32);

      var ni64 = rand.NextInt64();
      if (int.IsOddInteger(rand.Next()))
        ni64 = -ni64;
      var n4 = BitHacksOPs.Abs(ni64);
      var m4 = long.Abs(ni64);

      if (n1 != m1) throw new Exception();
      if (n2 != m2) throw new Exception();
      if (n3 != m3) throw new Exception();
      if (n4 != m4) throw new Exception();
    }
  }

  private static void TestHasZeroBytes()
  {
    var cnt = 1_000;
    var v = "00000000";
    var rand = Random.Shared;

    for (var i = 0; i < cnt; i++)
    {
      bool m1 = false;
      var ni8 = (sbyte)rand.Next();
      var strni8 = Convert.ToString(ni8, 2).PadLeft(8, '0').ToCharArray();
      var n1 = BitHacksOPs.HasZeroByte(ni8);
      Array.Reverse(strni8);
      for (int j = 0; j < strni8.Length; j += 8)
      {
        var buffer = new char[8];
        Array.Copy(strni8, j, buffer, 0, 8);
        if (v.SequenceEqual(buffer)) { m1 = true; break; }
      }
      if (n1 != m1) throw new Exception();

      var m2 = false;
      var ni16 = (short)rand.Next();
      var strni16 = Convert.ToString(ni16, 2).PadLeft(16, '0').ToCharArray();
      var n2 = BitHacksOPs.HasZeroByte(ni16);
      Array.Reverse(strni16);
      for (int j = 0; j < strni16.Length; j += 8)
      {
        var buffer = new char[8];
        Array.Copy(strni16, j, buffer, 0, 8);
        if (v.SequenceEqual(buffer)) { m2 = true; break; }
      }
      if (n2 != m2) throw new Exception();

      var m3 = false;
      var ni32 = rand.Next();
      if (int.IsOddInteger(rand.Next()))
        ni32 = -ni32;
      var strni32 = Convert.ToString(ni32, 2).PadLeft(32, '0').ToCharArray();
      var n3 = BitHacksOPs.HasZeroByte(ni32);
      Array.Reverse(strni32);
      for (int j = 0; j < strni32.Length; j += 8)
      {
        var buffer = new char[8];
        Array.Copy(strni32, j, buffer, 0, 8);
        if (v.SequenceEqual(buffer)) { m3 = true; break; }
      }
      if (n3 != m3) throw new Exception();

      var m4 = false;
      var ni64 = rand.NextInt64();
      if (int.IsOddInteger(rand.Next()))
        ni64 = -ni64;
      var strni64 = Convert.ToString(ni64, 2).PadLeft(64, '0').ToCharArray();
      var n4 = BitHacksOPs.HasZeroByte(ni64);
      Array.Reverse(strni64);
      for (int j = 0; j < strni64.Length; j += 8)
      {
        var buffer = new char[8];
        Array.Copy(strni64, j, buffer, 0, 8);
        if (v.SequenceEqual(buffer)) { m4 = true; break; }
      }
      if (n4 != m4) throw new Exception();

      var m5 = false;
      var nui8 = (byte)rand.Next();
      var strnui8 = Convert.ToString(nui8, 2).PadLeft(8, '0').ToCharArray();
      var n5 = BitHacksOPs.HasZeroByte(nui8);
      Array.Reverse(strnui8);
      for (int j = 0; j < strnui8.Length; j += 8)
      {
        var buffer = new char[8];
        Array.Copy(strnui8, j, buffer, 0, 8);
        if (v.SequenceEqual(buffer)) { m5 = true; break; }
      }
      if (n5 != m5) throw new Exception();

      var m6 = false;
      var nui16 = (ushort)rand.Next();
      var strnui16 = Convert.ToString(nui16, 2).PadLeft(16, '0').ToCharArray();
      var n6 = BitHacksOPs.HasZeroByte(nui16);
      Array.Reverse(strnui16);
      for (int j = 0; j < strnui16.Length; j += 8)
      {
        var buffer = new char[8];
        Array.Copy(strnui16, j, buffer, 0, 8);
        if (v.SequenceEqual(buffer)) { m6 = true; break; }
      }
      if (n6 != m6) throw new Exception();

      var m7 = false;
      var nui32 = (uint)rand.Next() + (uint)rand.Next();
      var strnui32 = Convert.ToString(nui32, 2).PadLeft(32, '0').ToCharArray();
      var n7 = BitHacksOPs.HasZeroByte(nui32);
      Array.Reverse(strnui32);
      for (int j = 0; j < strnui32.Length; j += 8)
      {
        var buffer = new char[8];
        Array.Copy(strnui32, j, buffer, 0, 8);
        if (v.SequenceEqual(buffer)) { m7 = true; break; }
      }
      if (n7 != m7) throw new Exception();

      var m8 = false;
      var nui64 = (ulong)rand.NextInt64() + (ulong)rand.NextInt64();
      var strnui64 = Convert.ToString((long)nui64, 2).PadLeft(64, '0').ToCharArray();
      var n8 = BitHacksOPs.HasZeroByte(nui64);
      Array.Reverse(strnui64);
      for (int j = 0; j < strnui64.Length; j += 8)
      {
        var buffer = new char[8];
        Array.Copy(strnui64, j, buffer, 0, 8);
        if (v.SequenceEqual(buffer)) { m8 = true; break; }
      }
      if (n8 != m8) throw new Exception();
    }
  }

  private static void TestRotateLeftRightBitwise()
  {
    var cnt = 1_000;
    var rand = Random.Shared;

    for (var i = 0; i < cnt; i++)
    {
      var ni8 = (sbyte)rand.Next();
      //var org_bits = Convert.ToString(ni8, 2);

      var shift = rand.Next(1, 8);
      var n1 = BitHacksOPs.RotateLeft(ni8, shift);
      //var str_bits = Convert.ToString(n1, 2);

      shift = rand.Next(1, 8);
      var n2 = BitHacksOPs.RotateRight(n1, shift);
      //str_bits = Convert.ToString(n2, 2);

      var ni16 = (short)rand.Next();

      shift = rand.Next(1, 16);
      var n3 = BitHacksOPs.RotateLeft(ni16, shift);

      shift = rand.Next(1, 16);
      var n4 = BitHacksOPs.RotateRight(ni16, shift);


      var ni32 = rand.Next();
      if (int.IsOddInteger(ni32)) ni32 = -ni32;

      shift = rand.Next(1, 32);
      var n5 = BitHacksOPs.RotateLeft(ni32, shift);

      shift = rand.Next(1, 32);
      var n6 = BitHacksOPs.RotateRight(ni32, shift);


      var ni64 = rand.NextInt64();
      if (long.IsOddInteger(ni64)) ni64 = -ni64;

      shift = rand.Next(1, 64);
      var n7 = BitHacksOPs.RotateLeft(ni64, shift);

      shift = rand.Next(1, 64);
      var n8 = BitHacksOPs.RotateRight(ni64, shift);


      var nui32 = (uint)rand.Next() + (uint)rand.Next();

      shift = rand.Next(1, 32);
      var n9 = BitHacksOPs.RotateLeft(nui32, shift);

      shift = rand.Next(1, 32);
      var n10 = BitHacksOPs.RotateRight(nui32, shift);


      var nui64 = rand.NextInt64();

      shift = rand.Next(1, 64);
      var n11 = BitHacksOPs.RotateLeft(nui64, shift);

      shift = rand.Next(1, 64);
      var n12 = BitHacksOPs.RotateRight(nui64, shift);

    }
  }

  public static void TestIsUnsignedType()
  {
    var cnt = 1_000;
    byte ui8 = 0;
    sbyte i8 = 0;
    short i16 = 0;
    ushort ui16 = 0;
    int i32 = 0;
    uint ui32 = 0;
    long i64 = 0;
    ulong ui64 = 0;
    nint ni = 0;
    nuint nui = 0;
    Int128 i128 = 0;
    UInt128 ui128 = 0;
    for (var i = 0; i < cnt; i++)
    {
      var rui8 = BitHacksOPs.IsUnsignedType(ui8);
      var ri8 = BitHacksOPs.IsUnsignedType(i8);
      var ri16 = BitHacksOPs.IsUnsignedType(i16);
      var rui16 = BitHacksOPs.IsUnsignedType(ui16);
      var ri32 = BitHacksOPs.IsUnsignedType(i32);
      var rui32 = BitHacksOPs.IsUnsignedType(ui32);
      var ri64 = BitHacksOPs.IsUnsignedType(i64);
      var rui64 = BitHacksOPs.IsUnsignedType(ui64);
      var rni = BitHacksOPs.IsUnsignedType(ni);
      var rnui = BitHacksOPs.IsUnsignedType(nui);
      var ri128 = BitHacksOPs.IsUnsignedType(i128);
      var rui128 = BitHacksOPs.IsUnsignedType(ui128);
    }
  }

  public static void TestIsOppositeSign()
  {
    var cnt = 1_000;
    var rand = Random.Shared;

    for (var i = 0; i < cnt; i++)
    {
      var li8 = (sbyte)rand.Next();
      var ri8 = (sbyte)rand.Next();

      var is_usign_type = BitHacksOPs.IsUnsignedType(li8);
      var resi8 = BitHacksOPs.IsOppositeSign(li8, ri8);

      var lui8 = (byte)rand.Next();
      var rui8 = (byte)rand.Next();

      is_usign_type = BitHacksOPs.IsUnsignedType(lui8);
      var resui8 = BitHacksOPs.IsOppositeSign(lui8, rui8);
    }
  }

  public static void TestMinMax()
  {
    var cnt = 1_000 / 2;
    var rand = Random.Shared;

    for (var i = 0; i < cnt; i++)
    {
      var l = -rand.Next(0, 100);
      var r = -rand.Next(100, 1000);

      var min_int = BitHacksOPs.Min(l, r);
      var max_int = BitHacksOPs.Max(l, r);

      if (min_int != r) throw new Exception();
      if (max_int != l) throw new Exception();


      l = -rand.Next(0, 100);
      r = rand.Next(100, 1000);

      min_int = BitHacksOPs.Min(l, r);
      max_int = BitHacksOPs.Max(l, r);

      if (min_int != l) throw new Exception();
      if (max_int != r) throw new Exception();


      l = rand.Next(0, 100);
      r = rand.Next(100, 1000);

      min_int = BitHacksOPs.Min(l, r);
      max_int = BitHacksOPs.Max(l, r);

      if (min_int != int.Min(l, r)) throw new Exception();
      if (max_int != int.Max(l, r)) throw new Exception();
    }

    for (var i = 0; i < cnt; i++)
    {
      var l = (uint)rand.Next(0, 100);
      var r = (uint)rand.Next(100, 1000);

      var min_int = BitHacksOPs.Min(l, r);
      var max_int = BitHacksOPs.Max(l, r);
      if (min_int != l) throw new Exception();
      if (max_int != r) throw new Exception();


      l = (uint)rand.Next(0, 1000);
      r = (uint)rand.Next(0, 1000);

      min_int = BitHacksOPs.Min(l, r);
      max_int = BitHacksOPs.Max(l, r);

      if (min_int != uint.Min(l, r)) throw new Exception();
      if (max_int != uint.Max(l, r)) throw new Exception();
    }
  }

  public static void TestEquality()
  {
    var cnt = 100;
    var rand = Random.Shared;

    for (var i = 0; i < cnt; i++)
    {
      var l = rand.Next(0, 1024);
      var r = rand.Next(0, 1024);
      var truefalse = BitHacksOPs.Equals(l, r);

      l = r;
      truefalse = BitHacksOPs.Equals(l, r);

      var a = new byte[1024];
      var b = new byte[1024];
      rand.NextBytes(a); rand.NextBytes(b);
      truefalse = BitHacksOPs.FixedTimeEquals<byte>(a, b);

      var ai = new int[1024 / 4];
      var bi = new int[1024 / 4];
      Buffer.BlockCopy(a, 0, ai, 0, a.Length);
      Buffer.BlockCopy(b, 0, bi, 0, b.Length);
      truefalse = BitHacksOPs.FixedTimeEquals<int>(ai, bi);

      ai = bi.ToArray();
      truefalse = BitHacksOPs.FixedTimeEquals<int>(ai, bi);
    }
  }

  public static void TestSwap()
  {
    var cnt = 1_000;
    var rand = Random.Shared;

    for (var i = 0; i < cnt; i++)
    {
      var l = rand.Next(0, 1024);
      var r = rand.Next(0, 1024);
      BitHacksOPs.Swap(ref l, ref r);

      l = r;
      BitHacksOPs.Swap(ref l, ref r);

      var lul = (ulong)rand.NextInt64(0, 1024);
      var rul = (ulong)rand.NextInt64(0, 1024);
      BitHacksOPs.Swap(ref lul, ref rul);

      lul = rul;
      BitHacksOPs.Swap(ref lul, ref rul);
    }
  }

  public static void TestShuffle()
  {
    var cnt = 100;
    var rand = Random.Shared;

    for (var i = 0; i < cnt; i++)
    {
      var n = rand.Next(0, 1024);

      //number must have 0's as well as 1's.
      if (n == 0 || n == -1 || n == int.MaxValue || n == int.MinValue)
        continue;

      var nset = BitHacksOPs.CountBitsSet(n);

      //Entropie
      var en = rand.Next(0, 1024);

      //Sample without entropie
      var shl1 = BitHacksOPs.Shuffle(n);

      //number must have 0's as well as 1's. 
      if (shl1 == 0 || shl1 == -1 || shl1 == int.MaxValue || shl1 == int.MinValue)
        continue;

      var nset_shl1 = BitHacksOPs.CountBitsSet(shl1);


      //Sample with entropie
      var shl2 = BitHacksOPs.Shuffle(n, en);

      //number must have 0's as well as 1's. 
      if (shl2 == 0 || shl2 == -1 || shl2 == int.MaxValue || shl2 == int.MinValue)
        continue;

      var nset_shl2 = BitHacksOPs.CountBitsSet(shl2);

      //Checks
      if (nset != nset_shl1) throw new Exception();
      if (nset != nset_shl2) throw new Exception();

    }

    for (var i = 0; i < cnt; i++)
    {
      var n = (ulong)rand.Next(0, 1024);

      //number must have 0's as well as 1's. 
      if (n == ulong.MaxValue || n == ulong.MinValue)
        continue;

      var nset = BitHacksOPs.CountBitsSet(n);

      //Entropie
      var en = (ulong)rand.Next(0, 1024);

      //Sample without entropie
      var shl1 = BitHacksOPs.Shuffle(n);

      //number must have 0's as well as 1's. 
      if (shl1 == ulong.MaxValue || shl1 == ulong.MinValue)
        continue;

      var nset_shl1 = BitHacksOPs.CountBitsSet(shl1);


      //Sample with entropie
      var shl2 = BitHacksOPs.Shuffle(n, en);

      //number must have 0's as well as 1's. 
      if (shl2 == ulong.MaxValue || shl2 == ulong.MinValue)
        continue;

      var nset_shl2 = BitHacksOPs.CountBitsSet(shl2);

      //Checks
      if (nset != nset_shl1) throw new Exception();
      if (nset != nset_shl2) throw new Exception();

    }
  }
}