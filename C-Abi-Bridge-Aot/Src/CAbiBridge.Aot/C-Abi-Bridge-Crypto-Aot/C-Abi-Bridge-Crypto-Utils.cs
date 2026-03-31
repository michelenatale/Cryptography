
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices; 

namespace michele.natale.CAbiBridge;

unsafe partial class CryptoBridge
{
 

  public static ReadOnlySpan<byte> ToSpanSafe(byte* ptr, int length)
  {
    if (ptr == null || length <= 0)
      return ReadOnlySpan<byte>.Empty;

    byte[] buffer = new byte[length];
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
