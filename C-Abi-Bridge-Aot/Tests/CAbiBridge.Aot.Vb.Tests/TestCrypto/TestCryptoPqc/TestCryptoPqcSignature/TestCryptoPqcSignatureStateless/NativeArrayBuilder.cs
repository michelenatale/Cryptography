
'using System.Runtime.InteropServices;

' namespace michele.natale.Tests;



' using var builder = new NativeArrayBuilder();

' var guid_ptr = builder.Add(guids);
' var sign_ptr = builder.Add(signs);
' var pubkey_ptr = builder.Add(pubkeys);
' var signname_ptr = builder.Add(signnames);
' var projectname_ptr = builder.Add(projectnames);

' var signalgo_ptr = builder.Add(signalgo);
' var signparams_ptr = builder.Add(signparams);

' var msg_ptr = builder.Add(message);

' var guid_lengths = builder.GetLengthsPtr();
' var sign_lengths = builder.GetLengthsPtr();
' var pubkey_lengths = builder.GetLengthsPtr();
' var signname_lengths = builder.GetLengthsPtr();
' var projectname_lengths = builder.GetLengthsPtr();
' var signalgo_lengths = builder.GetLengthsPtr();
' var signparams_lengths = builder.GetLengthsPtr();
' var msg_lengths = builder.GetLengthsPtr();


' public sealed class NativeArrayBuilder : IDisposable
' {
'   private readonly List<IntPtr> MPtrs = [];
'   private readonly List<int> MLengths = [];
'   private readonly List<GCHandle> MHandles = [];

'   public IntPtr Add(byte[][] arrays)
'   {
'     // Pin each array and store pointer + length
'     foreach (var arr in arrays)
'     {
'       var handle = GCHandle.Alloc(arr, GCHandleType.Pinned);
'       this.MHandles.Add(handle);

'       this.MPtrs.Add(handle.AddrOfPinnedObject());
'       this.MLengths.Add(arr.Length);
'     }

'     // Allocate unmanaged memory for byte**
'     var size = IntPtr.Size * arrays.Length;
'     var ptr = Marshal.AllocHGlobal(size);

'     // Copy pointers into unmanaged memory
'     for (int i = 0; i < arrays.Length; i++)
'       Marshal.WriteIntPtr(ptr, i * IntPtr.Size, this.MPtrs[i]);

'     return ptr;
'   }

'   public IntPtr Add(byte[] array)
'   {
'     var handle = GCHandle.Alloc(array, GCHandleType.Pinned);
'     this.MHandles.Add(handle);
'     return handle.AddrOfPinnedObject();
'   }

'   public IntPtr GetLengthsPtr()
'   {
'     var size = sizeof(int) * this.MLengths.Count;
'     var len_ptr = Marshal.AllocHGlobal(size);

'     for (int i = 0; i < this.MLengths.Count; i++)
'       Marshal.WriteInt32(len_ptr, i * sizeof(int), this.MLengths[i]);

'     return len_ptr;
'   }

'   public void Dispose()
'   {
'     foreach (var h in this.MHandles)
'       if (h.IsAllocated) h.Free();

'     this.MPtrs?.Clear();
'     this.MHandles?.Clear();
'     this.MLengths?.Clear();
'   }
' }
