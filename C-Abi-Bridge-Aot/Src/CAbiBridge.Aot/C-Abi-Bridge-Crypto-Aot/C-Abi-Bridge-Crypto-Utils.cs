
using System.Text;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace michele.natale.CAbiBridge;

using Pointers;

unsafe partial class CryptoBridge
{
  public static ReadOnlySpan<byte[]> ToSpanSafe(byte** ptr, int* length, int count) =>
    ToBytesSafe(ptr, length, count);

  public static ReadOnlySpan<byte> ToSpanSafe(byte* ptr, int length)
  {
    if (ptr == null || length <= 0)
      return [];

    byte[] buffer = new byte[length];
    Buffer.MemoryCopy(ptr, Unsafe.AsPointer(ref buffer[0]), length, length);
    return buffer;
  }

  public static ReadOnlyMemory<byte> ToMemSafe(byte* ptr, int length)
  {
    if (ptr == null || length <= 0)
      return Array.Empty<byte>();

    var buffer = new byte[length];
    Buffer.MemoryCopy(ptr, Unsafe.AsPointer(ref buffer[0]), length, length);

    return buffer;
  }

  public static string[] ToStringUtf8Safe(byte** ptr, int* length, int count)
  {
    if (ptr == null || length == null || count <= 0)
      return [];

    var result = new string[count];
    for (var i = 0; i < count; i++)
      result[i] = ToStringUtf8Safe(*(ptr + i), *(length + i));

    return result;
  }

  public static string ToStringUtf8Safe(byte* ptr, int length)
  {
    if (ptr == null || length <= 0)
      return string.Empty;

    var buffer = new byte[length];
    Buffer.MemoryCopy(ptr, Unsafe.AsPointer(ref buffer[0]), length, length);

    return Encoding.UTF8.GetString(buffer);
  }

  public static UsIPtr<byte> ToUsIPtrSafe(byte* ptr, int length)
  {
    if (ptr == null || length <= 0)
      return UsIPtr<byte>.Empty;

    var buffer = new byte[length];
    Buffer.MemoryCopy(ptr, Unsafe.AsPointer(ref buffer[0]), length, length);

    return new UsIPtr<byte>(buffer);
  }


  public static byte[][] ToBytesSafe(byte** ptr, int* length, int count)
  {
    if (ptr == null || length == null || count <= 0)
      return [];

    var result = new byte[count][];
    for (var i = 0; i < count; i++)
      result[i] = ToBytesSafe(*(ptr + i), *(length + i));

    return result;
  }

  public static byte[] ToBytesSafe(byte* ptr, int length)
  {
    if (ptr == null || length <= 0)
      return [];

    var buffer = new byte[length];
    Buffer.MemoryCopy(ptr, Unsafe.AsPointer(ref buffer[0]), length, length);

    return buffer;
  }

  public static IntPtr ToPtr2<T>(T[] managed)
    where T : unmanaged, INumber<T>
  {
    var length = managed.Length;
    var sz = Unsafe.SizeOf<T>();
    var bytes = new byte[sz * length];
    Buffer.BlockCopy(managed, 0, bytes, 0, bytes.Length);

    var result = IntPtr.Zero;
    Marshal.Copy(bytes, 0, result, bytes.Length);

    return result;
  }

  internal static IntPtr ToPtr(decimal[] values)
  {
    var size = values.Length * 16;
    IntPtr ptr = Marshal.AllocHGlobal(size);

    for (int i = 0; i < values.Length; i++)
    {
      var offset = i * 16;
      var bits = decimal.GetBits(values[i]);

      Marshal.WriteInt32(ptr, offset + 0, bits[0]);  // lo
      Marshal.WriteInt32(ptr, offset + 4, bits[1]);  // mid
      Marshal.WriteInt32(ptr, offset + 8, bits[2]);  // hi
      Marshal.WriteInt32(ptr, offset + 12, bits[3]); // flags
    }

    return ptr;
  }

}
