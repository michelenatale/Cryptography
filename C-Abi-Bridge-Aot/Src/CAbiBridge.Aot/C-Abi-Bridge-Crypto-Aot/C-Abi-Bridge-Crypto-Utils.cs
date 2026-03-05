
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace michele.natale.CAbiBridge;

public unsafe static partial class CryptoBridge
{
  [UnmanagedCallersOnly(EntryPoint = "free_buffer_aot")]
  public static void FreeBuffer(void* buffer)
  {
    if (buffer is null) return;
    NativeMemory.Free(buffer);
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
    int size = values.Length * 16;
    IntPtr ptr = Marshal.AllocHGlobal(size);

    for (int i = 0; i < values.Length; i++)
    {
      int offset = i * 16;
      int[] bits = decimal.GetBits(values[i]);

      Marshal.WriteInt32(ptr, offset + 0, bits[0]);  // lo
      Marshal.WriteInt32(ptr, offset + 4, bits[1]);  // mid
      Marshal.WriteInt32(ptr, offset + 8, bits[2]);  // hi
      Marshal.WriteInt32(ptr, offset + 12, bits[3]); // flags
    }

    return ptr;
  }


  private static CError CreateError(CErrorCode code, string msg)
  {
    var ptr = Marshal.StringToHGlobalAnsi(msg);
    return new CError { error_code = (int)code, message = ptr };
  }


  private static bool CheckSetZero(IntPtr* ptr)
  {
    if (ptr is not null)
      *ptr = IntPtr.Zero;
    return true;
  }

}
