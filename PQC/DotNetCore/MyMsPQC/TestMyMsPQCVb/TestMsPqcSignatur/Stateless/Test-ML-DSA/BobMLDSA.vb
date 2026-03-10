Option Strict On
Option Explicit On



Imports michele.natale.MsPqcs
Imports michele.natale.Services
Imports System.Security.Cryptography

Namespace michele.natale.TestMsPqcs
  Public Class BobMLDSA
    Implements IDisposable

    Private MPubKey As Byte()
    Private MIsDisposed As Boolean
    Private MInfo As MlDsaKeyPairInfo
    Private MAlgorithm As MLDsaAlgorithm

    Public Property IsDisposed As Boolean
      Get
        Return Me.MIsDisposed
      End Get
      Private Set
        Me.MIsDisposed = Value
      End Set
    End Property
    Public Property PubKey As Byte()
      Get
        Return Me.MPubKey
      End Get
      Private Set
        Me.MPubKey = Value
      End Set
    End Property
    Public Property Info As MlDsaKeyPairInfo
      Get
        Return Me.MInfo
      End Get
      Private Set
        Me.MInfo = Value
      End Set
    End Property
    Public Property Algorithm As MLDsaAlgorithm
      Get
        Return Me.MAlgorithm
      End Get
      Private Set
        Me.MAlgorithm = Value
      End Set
    End Property

    Public Sub New()
      Me.New(ToRngAlgo())
    End Sub

    Public Sub New(algo As MLDsaAlgorithm)
      Me.Algorithm = algo

      Dim kem = MLDsa.GenerateKey(algo)
      Dim privpub = MlDsaEx.ToKeyPair(kem)
      Using keypair = New KeyPairInfo(
        privpub.PubKey, privpub.PrivKey)
        Me.PubKey = keypair.PublicKey
        Me.Info = New MlDsaKeyPairInfo(
          Me.PubKey, keypair.PrivateKey.ToBytes(), algo)
      End Using
      Me.IsDisposed = False
    End Sub

    Public Sub Clear()
      If Me.IsDisposed Then Return

      Me.Info.Dispose()

      If Me.PubKey IsNot Nothing Then
        Array.Clear(Me.PubKey)
      End If

      Me.Info = Nothing
      Me.PubKey = Nothing
      Me.Algorithm = Nothing
    End Sub


    Public Function Sign(message As Byte()) As (Byte(), Byte())
      Dim signatur = MlDsaEx.Sign(Me.Info, message)
      Return (signatur, Me.PubKey)
    End Function

    Public Shared Function Verify(
      pubkeyalice As Byte(), signature As Byte(),
      message As Byte(), algo As MLDsaAlgorithm) As Boolean
      Return MlDsaEx.Verify(algo, pubkeyalice, signature, message)
    End Function

    Private Shared Function ToRngAlgo() As MLDsaAlgorithm
      Dim algo = MsPqcServices.ToMLDsaAlgorithm()
      Dim idx = RandomNumberGenerator.GetInt32(algo.Length)
      Return algo(idx)
    End Function

    Protected Overridable Sub Dispose(disposing As Boolean)
      If Not Me.MIsDisposed Then
        If disposing Then Me.Clear()
        Me.MIsDisposed = True
      End If
    End Sub

    Protected Overrides Sub Finalize()
      Me.Dispose(False)
      MyBase.Finalize()
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
      Me.Dispose(True)
      GC.SuppressFinalize(Me)
    End Sub
  End Class
End Namespace