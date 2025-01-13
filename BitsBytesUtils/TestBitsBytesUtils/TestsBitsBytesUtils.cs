

using System.Numerics;
using System.Runtime.CompilerServices;

namespace BitsBytesUtilsTest;


using michele.natale.BitsBytesUtils; 

public class TestsBitsBytesUtils
{


  public static void StartBitsBytesUtilsTests()
  {

    Console.WriteLine($"{nameof(StartBitsBytesUtilsTests)}: ");
    Console.WriteLine($"*************************\n");


    TestExtensions();

    TestBitLength();
    TestLeadingZerosCount();
    TestTwosComplementBigInteger();

    TestBits();
    TestBitStringBytes();

    TestBytes();

    ////Perhaps I will do this at a later date.
    //TestSubBitsSet();
    //TestSubBitsDelete();
    //TestSubBitsExtract();

    Console.WriteLine();
  }
  private static void TestExtensions()
  {
    Console.WriteLine($"{nameof(TestExtensions)}: ");

    var number = 12345678ul;

    var i64 = (long)number;
    var bytes = BitsBytesUtils.ToBytes(i64);
    var ri64 = new ReadOnlySpan<byte>(bytes).ToT<long>();

    var ui64 = number;
    bytes = BitsBytesUtils.ToBytes(ui64);
    var rui64 = new ReadOnlySpan<byte>(bytes).ToT<ulong>();

    var i128 = -(Int128)number;
    bytes = BitsBytesUtils.ToBytes(i128);
    var ri128 = new ReadOnlySpan<byte>(bytes).ToT<Int128>();

    var ui128 = (UInt128)number;
    bytes = BitsBytesUtils.ToBytes(ui128);
    var rui128 = new ReadOnlySpan<byte>(bytes).ToT<UInt128>();

    var bi = -(BigInteger)number;
    var bitstring = bi.ToBitString();
    var rbi = BitsBytesUtils.FromBitStringToBigInteger(bitstring);

    bi = -bi;

    bitstring = bi.ToBitString();
    rbi = BitsBytesUtils.FromBitStringToBigInteger(bitstring);

    bi = 123123132123456789;

    bitstring = bi.ToBitString();
    rbi = BitsBytesUtils.FromBitStringToBigInteger(bitstring);

    Console.WriteLine();
  }

  private static void TestBitLength()
  {
    //If the number is negative, always convert to unsigned.

    Console.WriteLine($"{nameof(TestBitLength)}: ");

    var i8 = 123;
    var bitlength = BitsBytesUtils.BitLength(i8);
    var bitlength2 = i8 == 0 ? 1 : ((long)Math.Log2((byte)i8)) + 1L;

    i8 = -i8;
    //If the number is negative, always convert to unsigned.
    bitlength = BitsBytesUtils.BitLength((byte)i8);
    bitlength2 = i8 == 0 ? 1 : ((long)Math.Log2((byte)i8)) + 1L;

    var i128 = -123456789;
    //If the number is negative, always convert to unsigned.
    bitlength = BitsBytesUtils.BitLength((UInt128)i128);
    bitlength2 = i128 == 0 ? 1 : ((long)UInt128.Log2((UInt128)i128)) + 1L;

    var bi = (BigInteger)i128;
    //With BigInteger it does not matter whether it is positive or negative.
    var caution = BitsBytesUtils.BitLength(bi);
    bitlength = BitsBytesUtils.BitLength(bi, 8 * Unsafe.SizeOf<Int128>()); //Here not
    bitlength2 = bi == 0 ? 1 : ((long)UInt128.Log2((UInt128)i128)) + 1L;

    bi = -bi;
    bitlength = BitsBytesUtils.BitLength(bi);
    bitlength2 = bi == 0 ? 1 : ((long)BigInteger.Log2(bi)) + 1L;
  }

  private static void TestLeadingZerosCount()
  {
    Console.WriteLine($"{nameof(TestLeadingZerosCount)}: ");

    var i64 = -123456789L;
    var lc = BitsBytesUtils.LeadingZerosCount(i64);

    i64 = -i64;
    lc = BitsBytesUtils.LeadingZerosCount(i64);

    var i128 = -(Int128)i64;
    lc = BitsBytesUtils.LeadingZerosCount(i128);

    i128 = -i128;
    lc = BitsBytesUtils.LeadingZerosCount(i128);

    var ui128 = (UInt128)i128;
    lc = BitsBytesUtils.LeadingZerosCount(ui128);

    ui128 = (UInt128)(-i128);
    lc = BitsBytesUtils.LeadingZerosCount(ui128);

    var bi = (BigInteger)i128;
    int sz = Unsafe.SizeOf<Int128>(), tsz = 8 * sz;
    lc = BitsBytesUtils.LeadingZerosCount(bi, tsz);

    bi = -bi;
    sz = Unsafe.SizeOf<Int128>(); tsz = 8 * sz;
    lc = BitsBytesUtils.LeadingZerosCount(bi, tsz);

    Console.WriteLine();
  }

  private static void TestTwosComplementBigInteger()
  {
    Console.WriteLine($"{nameof(TestTwosComplementBigInteger)}: ");

    //Wichtig:
    //Es gibt zwei veschiedene 2's-Komplements-
    //Methoden für den Datentype BigInteger.
    //TwosComplement:
    //  Macht bei negativen Werten das gleiche
    //  wie der Unary minus operators.
    //TwosComplementRange:
    //  Macht bei negativen Werten das gleiche
    //  wie die Konvertierung in ein Unsigned,
    //  wobei hier speziell der nächste PowerTwo-
    //  Range (P2 als Exponent) verwendet wird.

    //Important:
    //There are two different 2's complement methods
    //for the BigInteger data type.
    //TwosComplement:
    // Does the same as the Unary minus operator
    // for negative values.
    //TwosComplementRange:
    // For negative values, does the same as the
    // conversion to an Unsigned, specifically using
    // the next PowerTwo range (P2 as the exponent).

    TestInt16s();
    Console.WriteLine();

    //Internal Methode
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static void TestInt16s()
    {
      for (var i = 0; i < 5; i++)
      {
        var bi = -RandomHelper.RngBigInteger(2);
        if (bi.IsZero) continue;

        var tmp = bi % (BigInteger.One << 16);
        if (tmp == 0 || tmp < short.MinValue)
        {
          i--; continue;
        }

        var sht = (short)tmp;

        //Equals unary minus operators.
        var bytes = bi.ToByteArray();
        var tc = BitsBytesUtils.TwosComplement(bytes, bytes.Length);

        var newbi = new BigInteger(tc);

        if (newbi != -bi) throw new Exception();
        if (newbi != -sht) throw new Exception();
        if (newbi != ~bi + 1) throw new Exception();

        //Equals unsigned Converts.
        var usht = (ushort)sht;
        var ubi = BitsBytesUtils.TwosComplementRange(
          bi, 8 * Unsafe.SizeOf<ushort>()); //must be negativ

        if (usht != ubi) throw new Exception();
      }
    }
  }

  private static void TestBits()
  {
    //If the number is positive, a "false" is prefixed.

    Console.WriteLine($"{nameof(TestBits)}: ");

    var i32 = -123456;
    var bits = BitsBytesUtils.ToBits(i32);
    var bl = BitsBytesUtils.BitLength((uint)i32);

    var ui32 = (uint)i32;
    bits = BitsBytesUtils.ToBits(ui32);// Length plus 1 
    bl = BitsBytesUtils.BitLength(ui32);

    ui32 = (uint)(-i32);
    //Plus 1, as the number is positive
    bits = BitsBytesUtils.ToBits(ui32); // Length plus 1 
    bl = BitsBytesUtils.BitLength(ui32);

    var i128 = (Int128)(-123456789);
    var bitstring = i128.ToString("B").Length;
    bits = BitsBytesUtils.ToBits(i128);
    bl = BitsBytesUtils.BitLength((UInt128)i128);

    var ui128 = (UInt128)i128;
    bits = BitsBytesUtils.ToBits(ui128); // Length plus 1 
    bl = BitsBytesUtils.BitLength((UInt128)i128);

    var bi = (BigInteger)i128;
    bitstring = bi.ToBitString().Length;
    bits = BitsBytesUtils.ToBits(bi);
    bl = BitsBytesUtils.BitLength(bi);

    bi = -bi;
    bitstring = bi.ToBitString().Length;
    bits = BitsBytesUtils.ToBits(bi);// Length plus 1 
    bl = BitsBytesUtils.BitLength(bi);
  }

  private static void TestBitStringBytes()
  {
    //If the number is positive, it will be preceded by a "0".

    Console.WriteLine($"{nameof(TestBitStringBytes)}: ");

    var i8 = -123;
    var bs = i8.ToString("B");
    var bitstring = BitsBytesUtils.ToBitStr(i8);

    var ui8 = (byte)i8;
    bs = ui8.ToString("B");
    bitstring = BitsBytesUtils.ToBitStr(ui8); //Plus "0", as the number is positive

    var i128 = -123456789;
    bs = i128.ToString("B");
    bitstring = BitsBytesUtils.ToBitStr(i128);

    var ui128 = (UInt128)i128;
    bs = ui128.ToString("B");
    bitstring = BitsBytesUtils.ToBitStr(ui128);//Plus "0", as the number is positive

    var bi = (BigInteger)i128;
    bs = bi.ToBitString();
    bitstring = BitsBytesUtils.ToBitStr(bi);

    bi = -bi;
    bs = bi.ToBitString();
    bitstring = BitsBytesUtils.ToBitStr(bi);//Plus "0", as the number is positive

    Console.WriteLine();
  }

  private static void TestBytes()
  {
    Console.WriteLine($"{nameof(TestBytes)}: ");

    var i8s = new sbyte[] { -123, 123, 0, 1 };
    var bbytes = BitsBytesUtils.ToBytes<sbyte>(i8s);

    var ui8s = new byte[] { (256 - 123), 123, 0, 1 };
    bbytes = BitsBytesUtils.ToBytes<byte>(ui8s);

    var i128s = new Int128[] { -123456789, 123456789, 0, 1 };
    bbytes = BitsBytesUtils.ToBytes<Int128>(i128s);

    var nn = (UInt128)i128s.First();
    var ui128s = new UInt128[] { nn, 123456789, 0, 1 };
    bbytes = BitsBytesUtils.ToBytes<UInt128>(ui128s);

    var bis = i128s.Select(x => (BigInteger)x).ToArray();
    bbytes = BitsBytesUtils.ToBytes(bis);

    bis = ui128s.Select(x => (BigInteger)x).ToArray();
    bbytes = BitsBytesUtils.ToBytes(bis);

    Console.WriteLine();
  }


}