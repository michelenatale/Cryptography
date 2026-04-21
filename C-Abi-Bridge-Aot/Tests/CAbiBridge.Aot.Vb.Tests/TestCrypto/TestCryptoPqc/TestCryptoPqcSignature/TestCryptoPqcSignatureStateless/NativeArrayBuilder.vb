Option Strict On
Option Explicit On

Imports System.Runtime.InteropServices
Imports System.Runtime.CompilerServices

Namespace michele.natale.Tests
  Public NotInheritable Class NativeArrayBuilder
    Implements IDisposable

    Private IsDisposed As Boolean = False
    Private MPtrs As List(Of IntPtr) = Array.Empty(Of IntPtr).ToList
    Private MLengths As List(Of Int32) = Array.Empty(Of Int32).ToList
    Private MHandles As List(Of GCHandle) = Array.Empty(Of GCHandle).ToList

    Public Function Add(arrays As Byte()()) As IntPtr
      For Each arr In arrays
        Dim handle = GCHandle.Alloc(arr, GCHandleType.Pinned)
        Me.MHandles.Add(handle)
        Me.MPtrs.Add(handle.AddrOfPinnedObject())
        Me.MLengths.Add(arr.Length)
      Next

      Dim size = IntPtr.Size * arrays.Length
      Dim ptr = Marshal.AllocHGlobal(size)

      For i As Int32 = 0 To arrays.Length - 1
        Marshal.WriteIntPtr(ptr, i * IntPtr.Size, Me.MPtrs(i))
      Next

      Return ptr
    End Function

    Public Function Add(array As Byte()) As IntPtr
      Dim handle = GCHandle.Alloc(array, GCHandleType.Pinned)
      Me.MHandles.Add(handle)
      Return handle.AddrOfPinnedObject()
    End Function

    Public Function GetLengthsPtr() As IntPtr
      Dim tsz = Unsafe.SizeOf(Of Int32)()
      Dim size = tsz * Me.MLengths.Count
      Dim len_ptr = Marshal.AllocHGlobal(size)

      For i As Int32 = 0 To Me.MLengths.Count - 1
        Marshal.WriteInt32(len_ptr, i * tsz, Me.MLengths(i))
      Next

      Return len_ptr
    End Function

    Public Sub Clear()
      If Me.IsDisposed Then Return

      For Each h In Me.MHandles
        If h.IsAllocated Then h.Free()
      Next

      Me.MPtrs?.Clear()
      Me.MHandles?.Clear()
      Me.MLengths?.Clear()

      Me.MPtrs = Array.Empty(Of IntPtr).ToList
      Me.MHandles = Array.Empty(Of GCHandle).ToList
      Me.MLengths = Array.Empty(Of Int32).ToList
    End Sub

    Private Sub Dispose(disposing As Boolean)
      If Not Me.IsDisposed Then
        If disposing Then
          Me.Clear()
        End If
      End If

      Me.IsDisposed = True
    End Sub

    Protected Overrides Sub Finalize()
      Me.Dispose(disposing:=False)
      MyBase.Finalize()
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
      Me.Dispose(disposing:=True)
      GC.SuppressFinalize(Me)
    End Sub
  End Class
End Namespace
