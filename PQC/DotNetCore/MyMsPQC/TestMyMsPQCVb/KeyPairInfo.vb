Option Strict On
Option Explicit On


Imports michele.natale.Pointers

Namespace michele.natale.TestMsPqcs
  Public Class KeyPairInfo
    Implements IDisposable

    Public Property PublicKey As Byte() = Nothing
    Public Property IsDisposed As Boolean = False
    Public Property PrivateKey As UsIPtr(Of Byte) = UsIPtr(Of Byte).Empty

    Public Sub New(pubkey As Byte(), privkey As Byte())
      Me.PublicKey = pubkey.ToArray()
      Me.PrivateKey = New UsIPtr(Of Byte)(privkey)
    End Sub

    Public Sub New(pubkey As Byte(), privkey As UsIPtr(Of Byte))
      Me.PrivateKey = privkey.Copy
      Me.PublicKey = pubkey.ToArray()
    End Sub

    Public Sub Clear()
      Me.PrivateKey.Dispose()
      Array.Clear(Me.PublicKey)

      Me.PublicKey = Nothing
      Me.PrivateKey = UsIPtr(Of Byte).Empty
    End Sub

    Protected Overridable Sub Dispose(disposing As Boolean)
      If Not Me.IsDisposed Then
        If disposing Then Me.Clear()
        Me.IsDisposed = True
      End If
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
