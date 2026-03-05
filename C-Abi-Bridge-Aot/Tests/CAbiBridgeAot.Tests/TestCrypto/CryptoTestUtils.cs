


using System.Text;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace michele.natale.Tests;

using CAbiBridge;

internal class CryptoTestUtils
{
  internal static void SetRngFileData(string filename, int size)
  {
    using var fsout = new FileStream(
      filename, FileMode.Create, FileAccess.Write);

    var length = size < 1024 * 1024 ? size : 1024 * 1024;

    while (length > 0)
    {
      fsout.Write(RngBytes(length));
      size -= length; length = size < 1024 * 1024 ? size : 1024 * 1024;
    }
  }

  internal async static Task SetRngFileDataAsync(
    string filename, int size)
  {
    await using var fsout = new FileStream(
      filename, FileMode.Create, FileAccess.Write);

    var length = size < 1024 * 1024 ? size : 1024 * 1024;

    while (length > 0)
    {
      fsout.Write(RngBytes(length));
      size -= length; length = size < 1024 * 1024 ? size : 1024 * 1024;
    }
  }

  internal static byte[] RngBytes(int size)
  {
    //in practice, using a crypto-random 
    var rand = Random.Shared;
    var result = new byte[size];
    rand.NextBytes(result);
    if (result[0] == 0) result[0]++;
    return result;
  }

  internal static void AssertError(CError err)
  {
    if (err.error_code != 0)
    {
      var msg = Marshal.PtrToStringAnsi(err.message);
      throw new ArgumentException(msg);
    }
  }

  internal static byte[] ToBytes(
    IntPtr bytes, int length)
  {
    var result = new byte[length];
    Marshal.Copy(bytes, result, 0, length);
    return result;
  }

  internal static T[] ToInts<T>(
    IntPtr ptr, int length)
    where T : INumber<T>
  {
    //For uint8 to uint64
    var szt = Unsafe.SizeOf<T>();
    var bytes = ToBytes(ptr, length * szt);

    var result = new T[length];
    Buffer.BlockCopy(bytes, 0, result, 0, length * szt);

    return result;
  }

  public static bool[] ToBools(
    IntPtr ptr, int length)
  {
    var result = new bool[length];
    for (int i = 0; i < length; i++)
      result[i] = Marshal.ReadByte(ptr, i) != 0;

    return result;
  }

  internal static T[] ToFloats<T>(
    IntPtr ptr, int length)
    where T : IFloatingPoint<T>
  {
    //For double and float
    int tsz = Unsafe.SizeOf<T>() * length;

    var bytes = new byte[tsz];
    Marshal.Copy(ptr, bytes, 0, tsz);

    var result = new T[length];
    Buffer.BlockCopy(bytes, 0, result, 0, tsz);

    return result;
  }

  internal static decimal[] ToDecimals(IntPtr ptr, int length)
  {
    var result = new decimal[length];

    for (int i = 0; i < length; i++)
    {
      int offset = i * 16;

      int lo = Marshal.ReadInt32(ptr, offset + 0);
      int mid = Marshal.ReadInt32(ptr, offset + 4);
      int hi = Marshal.ReadInt32(ptr, offset + 8);
      int flags = Marshal.ReadInt32(ptr, offset + 12);

      result[i] = new decimal(
        lo, mid, hi, (flags & unchecked((int)0x80000000)) != 0,
          (byte)((flags >> 16) & 0x7F));
    }

    return result;
  }


  internal static char[] ToCharsUtf8(IntPtr ptr, int length)
  {
    var bytes = new byte[length];
    Marshal.Copy(ptr, bytes, 0, length);

    return Encoding.UTF8.GetChars(bytes);
  }

  internal static char[] ToCharsAscii(IntPtr ptr, int length)
  {
    var result = new char[length];
    for (int i = 0; i < length; i++)
      result[i] = (char)Marshal.ReadByte(ptr, i);

    return result;
  }

  internal static char[] ToCharsUtf16(IntPtr ptr, int length)
  {
    var result = new char[length];
    for (int i = 0; i < length; i++)
      result[i] = (char)Marshal.ReadInt16(ptr, i * 2);

    return result;
  }
}
