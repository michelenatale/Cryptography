
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace michele.natale.Pointers;


/// <summary>
/// New Instance from UsIPtr-Pointer<T> 
/// </summary>
/// <typeparam name="T">Type</typeparam>
/// <remarks>
/// Special thanks to exc-jdbi 
/// <para><see href="https://www.vb-paradise.de/index.php/User/23082-exc-jdbi/"></see></para>
/// </remarks>
public sealed class UsIPtr<T> : IDisposable where T : struct
{
  public IntPtr Ptr
  {
    get; private set;
  }
  public int TypeSize
  {
    get; private set;
  }
  public int Length
  {
    get; private set;
  }
  public bool Set_Zero_Dispose { get; set; } = true;
  public bool IsDisposed { get; private set; } = false;

  public static UsIPtr<T> Empty
  {
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    get => new() { Ptr = IntPtr.Zero, Length = 0, IsDisposed = true };
  }

  public unsafe Span<T> Target
  {
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    get => new(this.Ptr.ToPointer(), this.Length);
  }

  public unsafe T this[int index]
  {
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    get => new Span<T>(this.Ptr.ToPointer(), this.Length)[index];

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    set => new Span<T>(this.Ptr.ToPointer(), this.Length)[index] = value;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public UsIPtr()
  {
    if (!typeof(T).IsPrimitive)
      throw new NotImplementedException(nameof(UsIPtr<T>));

    this.TypeSize = Unsafe.SizeOf<T>();
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public UsIPtr(T[] values) : this() =>
    this.NewInit(values);

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public unsafe void NewInit(T[] values)
  {
    ObjectDisposedException.ThrowIf(
      this.IsDisposed, nameof(UsIPtr<T>));

    this.Dispose();
    this.IsDisposed = false;
    this.Length = values.Length;
    this.Ptr = this.ToIntPtr(values);
  }

  public UsIPtr<T> Copy => new(this.ToValues());

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public unsafe byte[] ToArray() =>
    new Span<byte>(this.Ptr.ToPointer(), this.TypeSize * this.Length).ToArray();

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public unsafe T[] ToValues() =>
    new Span<T>(this.Ptr.ToPointer(), this.Length).ToArray();

  public bool IsEmpty =>
    this.Ptr == IntPtr.Zero || this.Length < 1 || this.IsDisposed == true;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool Equality(UsIPtr<T> obj) =>
    this.Target.SequenceEqual(obj.Target);

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static implicit operator IntPtr(UsIPtr<T> value) =>
    value.Ptr;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public unsafe void Dispose()
  {
    if (this.IsDisposed) return;

    this.IsDisposed = true;

    if (this.Set_Zero_Dispose)
    {
      var bytes = (byte*)this.Ptr;
      var length = this.Length * this.TypeSize;

      for (var i = 0; i < length; i++)
        *(bytes + i) = 0;
    }
    Marshal.FreeHGlobal(this.Ptr);
    this.Ptr = IntPtr.Zero;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  private unsafe IntPtr ToIntPtr(T[] values)
  {
    var result = Marshal.AllocHGlobal(this.Length * this.TypeSize);
    var span = new Span<T>(result.ToPointer(), values.Length);
    values.AsSpan().CopyTo(span);
    return result;
  }
}


