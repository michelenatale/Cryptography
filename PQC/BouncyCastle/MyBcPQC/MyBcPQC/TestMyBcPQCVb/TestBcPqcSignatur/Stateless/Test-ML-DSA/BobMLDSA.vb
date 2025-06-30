Option Strict On
Option Explicit On



Imports michele.natale.BcPqcs
Imports michele.natale.Services
Imports Org.BouncyCastle.Security
Imports Org.BouncyCastle.Crypto.Signers
Imports Org.BouncyCastle.Crypto.Generators
Imports Org.BouncyCastle.Crypto.Parameters


Namespace michele.natale.TestBcPqcs

  Public Class BobMLDSA
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

    Private MInfo As MlDsaKeyPairInfo = Nothing
    Public Property Info() As MlDsaKeyPairInfo
      Get
        Return Me.MInfo
      End Get
      Private Set
        Me.MInfo = Value
      End Set
    End Property

    Private MParameter As MLDsaParameters = Nothing
    Public Property Parameter() As MLDsaParameters
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

    Public Sub New(parameter As MLDsaParameters)
      Me.Parameter = parameter

      Dim generator = New MLDsaKeyGenerationParameters(Me.Rand, parameter)

      Dim keypair_generator = New MLDsaKeyPairGenerator()
      keypair_generator.Init(generator)

      Dim keypair = keypair_generator.GenerateKeyPair()
      Me.PubKey = DirectCast(keypair.[Public], MLDsaPublicKeyParameters).GetEncoded()
      Dim privkey = DirectCast(keypair.[Private], MLDsaPrivateKeyParameters).GetEncoded()
      Me.Info = New MlDsaKeyPairInfo(Me.PubKey, privkey, parameter)

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
      Me.Parameter = MLDsaParameters.ml_dsa_44
    End Sub

    Public Function ToPubKey() As MLDsaPublicKeyParameters
      Return MLDsaPublicKeyParameters.FromEncoding(Me.Parameter, Me.PubKey)
    End Function

    Public Function Sign(message As Byte()) As (Sign As Byte(), Pubkey As Byte())
      'Sign Message-Data 

      'ML-DSA Parameter 
      Dim parameter = Me.Info.ToParameter()

      Dim signer = New MLDsaSigner(parameter, True)
      Dim privkey = MLDsaPrivateKeyParameters.FromEncoding(parameter, Me.Info.ToPrivKey().ToBytes())
      signer.Init(True, privkey)
      signer.BlockUpdate(message, 0, message.Length)

      Return (signer.GenerateSignature(), Me.PubKey)
    End Function

    Public Shared Function Verify(
    pubkeyalice As Byte(),
    signature As Byte(),
    message As Byte(),
    Parameter As MLDsaParameters) As Boolean

      'Verify Signature
      Dim signer = New MLDsaSigner(Parameter, True)
      Dim PubKey = MLDsaPublicKeyParameters.FromEncoding(
        Parameter, pubkeyalice)
      signer.Init(False, PubKey)
      signer.BlockUpdate(message, 0, message.Length)
      Return signer.VerifySignature(signature)
    End Function

    Private Shared Function ToRngParameter() As MLDsaParameters
      Dim rand = New SecureRandom()
      Dim parameters = BcPqcServices.ToMLDsaParameters()
      Dim idx = rand.[Next](parameters.Length)
      Return parameters(idx)
    End Function

    Public Shared Function ToPubKey(
      pubkey As Byte(), parameter As MLDsaParameters) As MLDsaPublicKeyParameters
      Return MLDsaPublicKeyParameters.FromEncoding(parameter, pubkey)
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
