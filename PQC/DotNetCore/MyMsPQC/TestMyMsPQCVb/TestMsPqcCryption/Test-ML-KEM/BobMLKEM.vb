Option Strict On
Option Explicit On



Imports System.Text
Imports michele.natale.MsPqcs
Imports michele.natale.Pointers
Imports michele.natale.Services
Imports System.Security.Cryptography

Namespace michele.natale.TestMsPqcs

  Public Class BobMLKEM
    Implements IDisposable

    Private MPubKey As Byte()
    Private MCapsulationKey As Byte()
    Private MAlgorithm As MLKemAlgorithm
    Private MIsDisposed As Boolean = False
    Private MAssociated As Byte() =
      Encoding.UTF8.GetBytes("© michele natale 2025")

    Public Property PubKey As Byte()
      Get
        Return Me.MPubKey
      End Get
      Private Set
        Me.MPubKey = Value
      End Set
    End Property
    Public Property IsDisposed As Boolean
      Get
        Return Me.MIsDisposed
      End Get
      Private Set
        Me.MIsDisposed = Value
      End Set
    End Property
    Public Property CapsulationKey As Byte()
      Get
        Return Me.MCapsulationKey
      End Get
      Private Set
        Me.MCapsulationKey = Value
      End Set
    End Property
    Public Property Algorithm As MLKemAlgorithm
      Get
        Return Me.MAlgorithm
      End Get
      Private Set
        Me.MAlgorithm = Value
      End Set
    End Property
    Public Property Associated As Byte()
      Get
        Return Me.MAssociated
      End Get
      Private Set
        Me.MAssociated = Value
      End Set
    End Property

    Private KeyPair As KeyPairInfo
    Private SharedKey As UsIPtr(Of Byte) = UsIPtr(Of Byte).Empty

    Public Sub New()
      Me.New(ToRngParameter())
    End Sub

    Public Sub New(algo As MLKemAlgorithm)
      Me.Algorithm = algo

      Using kem = MLKem.GenerateKey(algo)
        Dim privpub = MlKemEx.ToKeyPair(kem)
        Me.KeyPair = New KeyPairInfo(privpub.PubKey, privpub.PrivKey)
        Me.PubKey = Me.KeyPair.PublicKey
      End Using

      Me.IsDisposed = False
    End Sub

    Public Sub Clear()
      If Me.IsDisposed Then Return


      If Me.PubKey IsNot Nothing Then
        Array.Clear(Me.PubKey)
      End If
      If Me.Associated IsNot Nothing Then
        Array.Clear(Me.Associated)
      End If
      If Me.CapsulationKey IsNot Nothing Then
        Array.Clear(Me.CapsulationKey)
      End If

      Me.KeyPair.Dispose()
      Me.SharedKey.Dispose()

      Me.PubKey = Nothing
      Me.KeyPair = Nothing
      Me.Algorithm = Nothing
      Me.CapsulationKey = Nothing
      Me.SharedKey = UsIPtr(Of Byte).Empty
      Me.Associated = Encoding.UTF8.GetBytes("© michele natale 2025")
    End Sub

    Public Function Encryption(bytes As Byte(), capsulationkey As Byte(), associated As Byte(), cryptoalgo As CryptionAlgorithm) As Byte()
      Dim associat = If(IsNullOrEmpty(associated), Me.Associated, associated)
      Using sharedkey = Me.ToSharedKey(capsulationkey)
        Return MsPqcServices.EncryptionWithCryptionAlgo(bytes, sharedkey, associat, cryptoalgo)
      End Using
    End Function

    Public Function Decryption(bytes As Byte(), associated As Byte(), cryptoalgo As CryptionAlgorithm) As Byte()
      Dim associat = If(IsNullOrEmpty(associated), Me.Associated, associated)
      Return MsPqcServices.DecryptionWithCryptionAlgo(bytes, Me.SharedKey, associat, cryptoalgo)
    End Function

    Public Function ToSharedKey(capsulationkey As Byte()) As UsIPtr(Of Byte)
      ' SharedKeys are always secret.

      ' Bob decapsulates a new shared secret using Bob's private key
      Using bob = MLKem.ImportDecapsulationKey(Me.Algorithm, Me.KeyPair.PrivateKey.ToBytes())
        Return MlKemEx.ToSharedKey(bob, capsulationkey)
      End Using
    End Function

    Public Function GenerateSharedKey(alice_pubkey As Byte()) As Byte()
      ' SharedKeys are always secret, whereas the CapsulationKey is always public.

      If Me.SharedKey IsNot Nothing AndAlso Not Me.SharedKey.IsDisposed Then
        Me.ClearSharedKey()
      End If

      ' Bob encapsulates a new shared secret using alice's public key
      Using alice = MLKem.ImportEncapsulationKey(Me.Algorithm, alice_pubkey)
        Me.SharedKey = MlKemEx.ToSharedKey(alice, Nothing)

        Dim capsulation_key As Byte() = Nothing
        Me.SharedKey = MlKemEx.ToSharedKey(alice, capsulation_key)
        Me.CapsulationKey = capsulation_key
      End Using

      Return Me.CapsulationKey
    End Function

    Private Sub ClearSharedKey()
      If Me.SharedKey IsNot Nothing AndAlso Not Me.SharedKey.IsDisposed Then
        Me.SharedKey.Dispose()
      End If
      If Me.CapsulationKey IsNot Nothing Then
        Array.Clear(Me.CapsulationKey)
      End If

      Me.CapsulationKey = Nothing
      Me.SharedKey = UsIPtr(Of Byte).Empty
    End Sub

    Private Shared Function IsNullOrEmpty(bytes As Byte()) As Boolean
      If bytes Is Nothing Then Return True
      Return bytes.Length = 0
    End Function

    Private Shared Function ToRngParameter() As MLKemAlgorithm
      Dim parameters = MsPqcServices.ToMLKemAlgorithm()
      Dim idx = RandomNumberGenerator.GetInt32(parameters.Length)
      Return parameters(idx)
    End Function

    Protected Overridable Sub Dispose(disposing As Boolean)
      If Not Me.IsDisposed Then
        If disposing Then Me.Clear()
        Me.IsDisposed = True
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