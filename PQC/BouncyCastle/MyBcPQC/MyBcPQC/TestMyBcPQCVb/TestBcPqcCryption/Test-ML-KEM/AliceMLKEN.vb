


Imports System.Text
Imports michele.natale.Pointers
Imports michele.natale.Services
Imports Org.BouncyCastle.Crypto
Imports Org.BouncyCastle.Security
Imports Org.BouncyCastle.Crypto.Kems
Imports Org.BouncyCastle.Crypto.Parameters
Imports Org.BouncyCastle.Crypto.Generators

Namespace michele.natale.TestBcPqcs

  Public Class AliceMLKEM
    Implements IDisposable

    Private MPubKey As Byte() = Array.Empty(Of Byte)
    Public Property PubKey() As Byte()
      Get
        Return Me.MPubKey
      End Get
      Private Set
        Me.MPubKey = Value
      End Set
    End Property

    Private MIsDisposed As Boolean = True
    Public Property IsDisposed() As Boolean
      Get
        Return Me.MIsDisposed
      End Get
      Set
        Me.MIsDisposed = Value
      End Set
    End Property

    Private MCapsulationKey As Byte() = Array.Empty(Of Byte)
    Public Property CapsulationKey() As Byte()
      Get
        Return Me.MCapsulationKey
      End Get
      Private Set
        Me.MCapsulationKey = Value
      End Set
    End Property

    Private MParameter As MLKemParameters = Nothing
    Public Property Parameter() As MLKemParameters
      Get
        Return Me.MParameter
      End Get
      Set
        Me.MParameter = Value
      End Set
    End Property

    Private MAssociated As Byte() =
      Encoding.UTF8.GetBytes("© michele natale 2025")
    Public Property Associated() As Byte()
      Get
        Return Me.MAssociated
      End Get
      Private Set
        Me.MAssociated = Value
      End Set
    End Property


    Private Rand As New SecureRandom
    Private KeyPair As AsymmetricCipherKeyPair = Nothing
    Private SharedKey As UsIPtr(Of Byte) = UsIPtr(Of Byte).Empty

    Public Sub New()
      Me.New(ToRngParameter())
    End Sub

    Public Sub New(parameter As MLKemParameters)
      Me.Parameter = parameter

      Dim generator = New MLKemKeyGenerationParameters(Me.Rand, Me.Parameter)

      Dim keypair_generator = New MLKemKeyPairGenerator()
      keypair_generator.Init(generator)

      Me.KeyPair = keypair_generator.GenerateKeyPair()
      Me.PubKey = DirectCast(Me.KeyPair.[Public], MLKemPublicKeyParameters).GetEncoded()

      Me.IsDisposed = False
    End Sub

    Public Sub Clear()
      If Me.MIsDisposed Then Return

      If Me.MPubKey IsNot Nothing Then
        Array.Clear(Me.PubKey)
      End If

      If Me.MAssociated IsNot Nothing Then
        Array.Clear(Me.MAssociated)
      End If

      If Me.CapsulationKey IsNot Nothing Then
        Array.Clear(Me.CapsulationKey)
      End If

      Me.SharedKey.Dispose()

      Me.Rand = Nothing
      Me.KeyPair = Nothing
      Me.Parameter = Nothing
      Me.PubKey = Array.Empty(Of Byte)
      Me.SharedKey = UsIPtr(Of Byte).Empty
      Me.CapsulationKey = Array.Empty(Of Byte)
      Me.Associated = Encoding.UTF8.GetBytes("© michele natale 2025")
    End Sub

    Public Function ToPubKey() As MLKemPublicKeyParameters
      Return MLKemPublicKeyParameters.FromEncoding(Me.Parameter, Me.PubKey)
    End Function

    Public Function Encryption(
      bytes As Byte(), capsulationkey As Byte(),
      associated As Byte(), cryptoalgo As CryptionAlgorithm) As Byte()
      Dim associat = If(associated.AsSpan().IsEmpty, Me.Associated, associated)
      Dim sharedkey = Me.ToSharedKey(capsulationkey)

      Return BcPqcServices.EncryptionWithCryptionAlgo(bytes, sharedkey, associat, cryptoalgo)
    End Function

    Public Function Decryption(bytes As Byte(), associated As Byte(),
      cryptoalgo As CryptionAlgorithm) As Byte()
      Dim associat = If(associated.AsSpan.IsEmpty, Me.Associated, associated)

      Return BcPqcServices.DecryptionWithCryptionAlgo(bytes, Me.SharedKey, associat, cryptoalgo)
    End Function

    Public Function ToSharedKey(capsulationkey As Byte()) As UsIPtr(Of Byte)
      'SharedKeys are always secret.

      ' Alice decapsulates a new shared secret using Alice's private key
      Dim decapsulator = New MLKemDecapsulator(Me.Parameter)
      decapsulator.Init(Me.KeyPair.[Private])

      Dim sharedkey = New Byte(decapsulator.SecretLength - 1) {}
      decapsulator.Decapsulate(capsulationkey.ToArray(), 0, capsulationkey.Length, sharedkey, 0, sharedkey.Length)

      Return New UsIPtr(Of Byte)(sharedkey)
    End Function

    Public Function GenerateSharedKey(bob_pubkey As Byte()) As Byte()
      'SharedKeys are always secret, whereas the CapsulationKey is always public.
      If Me.SharedKey IsNot Nothing AndAlso Not Me.SharedKey.IsDisposed Then
        Me.ClearSharedKey()
      End If

      ' Alice encapsulates a new shared secret using bob's public key
      Dim encapsulator = New MLKemEncapsulator(Me.Parameter)
      encapsulator.Init(New ParametersWithRandom(
        ToPubKey(bob_pubkey.ToArray(), Me.Parameter), Me.Rand))

      Dim skey = New Byte(encapsulator.SecretLength) {}
      Dim ckey = New Byte(encapsulator.EncapsulationLength) {}
      encapsulator.Encapsulate(ckey, 0, ckey.Length, skey, 0, skey.Length)

      Me.SharedKey = New UsIPtr(Of Byte)(skey)
      Me.CapsulationKey = ckey

      Return ckey.ToArray() 'copy
    End Function

    Private Sub ClearSharedKey()
      If Me.SharedKey IsNot Nothing AndAlso Not Me.SharedKey.IsDisposed Then
        Me.SharedKey.Dispose()
        Me.SharedKey = UsIPtr(Of Byte).Empty

        Array.Clear(Me.CapsulationKey)
        Me.CapsulationKey = Nothing
      End If
    End Sub

    Private Shared Function ToRngParameter() As MLKemParameters
      Dim rand = New SecureRandom()
      Dim parameters = BcPqcServices.ToMLKemParameters()
      Dim idx = rand.[Next](parameters.Length)
      Return parameters(idx)
    End Function

    Public Shared Function ToPubKey(pubkey As Byte(), parameter As MLKemParameters) As MLKemPublicKeyParameters
      Return MLKemPublicKeyParameters.FromEncoding(parameter, pubkey)
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
