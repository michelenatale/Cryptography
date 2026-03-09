
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;


namespace michele.natale.Pointers;


/// <summary>
/// New Instance from UsIPtr-Pointer<T> 
/// </summary>
/// <typeparam name="T">Type</typeparam>
/// <remarks>
/// Special thanks to exc-jdbi 
/// <para>
/// <see href="https://www.vb-paradise.de/index.php/User/23082-exc-jdbi/">
/// </see></para>
/// </remarks>
public sealed class UsIPtr<T> : IDisposable where T : struct
{
  /// <summary>
  /// Specifies the length of the array chain.
  /// </summary>
  public int Length { get; private set; } = -1;

  /// <summary>
  /// Specifies the base length of the data type.
  /// </summary>
  public int TypeSize { get; private set; } = -1;

  /// <summary>
  /// <para>
  /// If desired, the buffer is reset with 'zeros' when disposing.
  /// </para>
  /// Recommendation: Strict cryptographic procedures require that any 
  /// memory buffer is reset with 'zeros'.
  /// </summary>
  public bool Set_Zero_Dispose { get; set; } = true;

  /// <summary>
  /// True, if Disposed, otherwise false.
  /// </summary>
  public bool IsDisposed { get; private set; } = false;

  /// <summary>
  /// IntPtr-pointer for the memory buffer.
  /// </summary>
  public nint Ptr { get; private set; } = nint.Zero;

  /// <summary>
  /// Returns the default UsIPtr<T>.
  /// </summary>
  public static UsIPtr<T> Empty
  {
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    get => new() { Ptr = nint.Zero, Length = 0, IsDisposed = true };
  }

  /// <summary>
  /// Returns the complete buffer as span<T>.
  /// </summary>
  public unsafe Span<T> Target
  {
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    get => new(this.Ptr.ToPointer(), this.Length);
  }

  /// <summary>
  /// Indexer for the memory buffer.
  /// </summary>
  /// <param name="index">Index</param>
  /// <returns>Value</returns>
  public unsafe T this[int index]
  {
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    get => new Span<T>(this.Ptr.ToPointer(), this.Length)[index];

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    set => new Span<T>(this.Ptr.ToPointer(), this.Length)[index] = value;
  }

  /// <summary>
  /// C-Tor
  /// </summary>
  /// <exception cref="NotImplementedException"></exception>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public UsIPtr()
  {
    if (!typeof(T).IsPrimitive)
      throw new NotImplementedException(nameof(UsIPtr<T>));

    this.TypeSize = Unsafe.SizeOf<T>();
  }

  /// <summary>
  /// C-Tor
  /// </summary>
  /// <param name="values"></param>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public UsIPtr(ReadOnlySpan<T> values) : this() =>
    this.NewInit(values);

  /// <summary>
  /// New initialization of UsIPtr<T>.
  /// </summary>
  /// <param name="values">Initialization value</param>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public unsafe void NewInit(ReadOnlySpan<T> values)
  {
    ObjectDisposedException.ThrowIf(
      this.IsDisposed, nameof(UsIPtr<T>));

    this.Dispose();
    this.IsDisposed = false;
    this.Length = values.Length;
    this.Ptr = this.ToIntPtr(values);
  }

  /// <summary>
  /// Returns a new copy of the current UsIPtr<T>.
  /// </summary>
  public UsIPtr<T> Copy => new(this.ToValues());

  /// <summary>
  /// Returns a copy of all values as Array of Byte.
  /// </summary>
  /// <returns>Array of Byte</returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public unsafe byte[] ToBytes() => this.ToArray();

  /// <summary>
  /// Returns a copy of all values as Array of Byte.
  /// </summary>
  /// <returns>Array of Byte</returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public unsafe byte[] ToArray() =>
    new Span<byte>(this.Ptr.ToPointer(), this.TypeSize * this.Length).ToArray();

  /// <summary>
  /// Returns a copy of all values as Array of T.
  /// </summary>
  /// <returns>Array of T</returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public unsafe T[] ToValues() =>
    new Span<T>(this.Ptr.ToPointer(), this.Length).ToArray();

  /// <summary>
  /// True, if Empty, otherwise false.
  /// </summary>
  public bool IsEmpty =>
    this.Ptr == nint.Zero || this.Length < 1 || this.IsDisposed == true;

  /// <summary>
  /// <para>FixesTime equality check.</para>
  /// True, if equals, ortherwise false.
  /// </summary>
  /// <param name="obj">Desired Value</param>
  /// <returns>Boolean-Value</returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public unsafe bool Equality(UsIPtr<T> obj)
  {
    if (this.Target.Length != obj.Target.Length)
      return false;

    var result = 0;
    var left_bytes = (byte*)this.Ptr;
    var right_bytes = (byte*)obj.Ptr;
    var length = this.Length * this.TypeSize;

    for (var i = 0; i < length; i++)
      result |= *(left_bytes + i) ^ *(right_bytes + i);

    return result == 0;
  }

  /// <summary>
  /// Returns an IntPtr pointer of a UsIPtr<T> value.
  /// </summary>
  /// <param name="value"></param>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static implicit operator nint(UsIPtr<T> value) =>
    value.Ptr;

  /// <summary>
  /// Dispose UsIPtr<T>
  /// </summary>
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
    this.Ptr = nint.Zero;
  }

  /// <summary>
  /// Allocates memory from the unmanaged memory of the 
  /// process, and returns an IntPtr pointer.
  /// </summary>
  /// <param name="values">
  /// Desired array with the existing values.
  /// </param>
  /// <returns>IntPtr Pointer</returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  private unsafe nint ToIntPtr(ReadOnlySpan<T> values)
  {
    var result = Marshal.AllocHGlobal(this.Length * this.TypeSize);
    var span = new Span<T>(result.ToPointer(), values.Length);
    values.CopyTo(span);
    return result;
  }
}