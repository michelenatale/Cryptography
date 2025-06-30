Option Strict On
Option Explicit On



Imports michele.natale.BcPqcs
Imports michele.natale.Services
Imports Org.BouncyCastle.Security
Imports Org.BouncyCastle.Crypto.Signers
Imports Org.BouncyCastle.Crypto.Generators
Imports Org.BouncyCastle.Crypto.Parameters


Namespace michele.natale.TestBcPqcs

  Public Class BobSLHDSA
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

    Private MInfo As SlhDsaKeyPairInfo = Nothing
    Public Property Info() As SlhDsaKeyPairInfo
      Get
        Return Me.MInfo
      End Get
      Private Set
        Me.MInfo = Value
      End Set
    End Property

    Private MParameter As SlhDsaParameters = Nothing
    Public Property Parameter() As SlhDsaParameters
      Get
        Return Me.MParameter
      End Get
      Private Set
        Me.MParameter = Value
      End Set
    End Property


    Private Rand As New SecureRandom

    Public Sub New()
      Me.New(ToRngParameter())
    End Sub

    Public Sub New(parameter As SlhDsaParameters)
      Me.Parameter = parameter

      Dim generator = New SlhDsaKeyGenerationParameters(Me.Rand, parameter)

      Dim keypair_generator = New SlhDsaKeyPairGenerator()
      keypair_generator.Init(generator)

      Dim keypair = keypair_generator.GenerateKeyPair()
      Me.PubKey = DirectCast(keypair.[Public], SlhDsaPublicKeyParameters).GetEncoded()
      Dim privkey = DirectCast(keypair.[Private], SlhDsaPrivateKeyParameters).GetEncoded()
      Me.Info = New SlhDsaKeyPairInfo(Me.PubKey, privkey, parameter)

      Array.Clear(privkey)
      Me.IsDisposed = False
    End Sub

    Public Sub Clear()

      If Me.MIsDisposed Then Return

      Me.Info.Dispose()
      If Me.PubKey IsNot Nothing Then
        Array.Clear(Me.PubKey)
      End If

      Me.Rand = Nothing
      Me.Info = Nothing
      Me.PubKey = Nothing
      Me.Parameter = SlhDsaParameters.slh_dsa_sha2_128s
    End Sub

    Public Function ToPubKey() As SlhDsaPublicKeyParameters
      Return SlhDsaPublicKeyParameters.FromEncoding(Me.Parameter, Me.PubKey)
    End Function

    Public Function Sign(message As Byte()) As (Sign As Byte(), Pubkey As Byte())
      'Sign Message-Data 

      'ML-DSA Parameter 
      Dim parameter = Me.Info.ToParameter()

      Dim signer = New SlhDsaSigner(parameter, True)
      Dim privkey = SlhDsaPrivateKeyParameters.FromEncoding(parameter, Me.Info.ToPrivKey().ToBytes())
      signer.Init(True, privkey)
      signer.BlockUpdate(message, 0, message.Length)

      Return (signer.GenerateSignature(), Me.PubKey)
    End Function

    Public Shared Function Verify(
    pubkeybob As Byte(),
    signature As Byte(),
    message As Byte(),
    Parameter As SlhDsaParameters) As Boolean

      'Verify Signature
      Dim signer = New SlhDsaSigner(Parameter, True)
      Dim PubKey = SlhDsaPublicKeyParameters.FromEncoding(
        Parameter, pubkeybob)
      signer.Init(False, PubKey)
      signer.BlockUpdate(message, 0, message.Length)
      Return signer.VerifySignature(signature)
    End Function

    Private Shared Function ToRngParameter() As SlhDsaParameters
      Dim rand = New SecureRandom()
      Dim parameters = BcPqcServices.ToSLHDsaParameters()
      Dim idx = rand.[Next](parameters.Length)
      Return parameters(idx)
    End Function

    Public Shared Function ToPubKey(
      pubkey As Byte(), parameter As SlhDsaParameters) As SlhDsaPublicKeyParameters
      Return SlhDsaPublicKeyParameters.FromEncoding(parameter, pubkey)
    End Function

    Protected Overridable Sub Dispose(disposing As Boolean)
      If Not Me.IsDisposed Then
        If disposing Then Me.Clear()
        Me.IsDisposed = True
      End If
    End Sub

    Protected Overrides Sub Finalize()
      Me.Dispose(False)
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
      Me.Dispose(True)
      GC.SuppressFinalize(Me)
    End Sub
  End Class
End Namespace
