
using System.Security.Cryptography; 
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace michele.natale.CAbiBridge;

using static NetServicesUtils;

unsafe partial class CryptoBridge
{

  [UnmanagedCallersOnly(EntryPoint = "free_buffer_aot")]
  public static void FreeBuffer(void* buffer)
  {
    if (buffer is null) return;
    NativeMemory.Free(buffer);
  }

  [UnmanagedCallersOnly(EntryPoint = "free_buffer_clear_aot")]
  public static void FreeBuffer(void* buffer, int length)
  {
    if (buffer is null || length <= 0)
      return;

    Unsafe.InitBlock(buffer, 0, (uint)length);
    NativeMemory.Free(buffer);
    CheckSetZero((nint*)buffer);
  }

  [UnmanagedCallersOnly(EntryPoint = "pbkdf2_aot")]
  public static CError Pbkdf2Aot(
    byte* pw, int pwLen,
    byte* salt, int saltLen,
    int iterations,
    byte* output, int outputLen)
  {
    var key = Rfc2898DeriveBytes.Pbkdf2(
      new ReadOnlySpan<byte>(pw, pwLen),
      new ReadOnlySpan<byte>(salt, saltLen),
      iterations, HashAlgorithmName.SHA512,
      outputLen);

    key.CopyTo(new Span<byte>(output, outputLen));
    return new CError { error_code = (int)CErrorCode.Ok };
  }


}
