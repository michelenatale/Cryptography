
Imports michele.natale.BcPqcs
Imports michele.natale.Services
Imports Org.BouncyCastle.Security
Imports Org.BouncyCastle.Pqc.Crypto.Lms


Namespace michele.natale.TestBcPqcs

  Public Class BobLMS
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

    Private MInfo As LmsKeyPairInfo = Nothing
    Public Property Info() As LmsKeyPairInfo
      Get
        Return Me.MInfo
      End Get
      Private Set
        Me.MInfo = Value
      End Set
    End Property

    Private MParameter As LmsParam = Nothing
    Public Property PArameter() As LmsParam
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

    Public Sub New(parameter As LmsParam)
      Me.PArameter = parameter

      Dim generator = BcPqcServices.ToLmsKeyGenerationParameter(parameter, Me.Rand)

      Dim keypair_generator = New LmsKeyPairGenerator()
      keypair_generator.Init(generator)

      Dim keypair = keypair_generator.GenerateKeyPair()
      Me.PubKey = directcast(keypair.[Public], LmsPublicKeyParameters).GetEncoded()
      Dim privkey = directcast(keypair.[Private], LmsPrivateKeyParameters).GetEncoded()
      Me.Info = New LmsKeyPairInfo(Me.PubKey, privkey, parameter)

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
      Me.PArameter = LmsParam.lms_sha256_h5_w1
    End Sub

    Public Function ToPubKey() As LmsPublicKeyParameters
      Return LmsPublicKeyParameters.GetInstance(Me.PubKey)
    End Function

    Public Function Sign(message As Byte()) As (Sign As Byte(), Pubkey As Byte())
      'Sign Message-Data
      Dim signer = New LmsSigner()
      Dim privkey = LmsPrivateKeyParameters.GetInstance(Me.Info.ToPrivKey().ToBytes())
      signer.Init(True, privkey)

      Return (signer.GenerateSignature(message.ToArray()), Me.PubKey)
    End Function

    Public Shared Function Verify(pubkeybob As Byte(),
      signature As Byte(), message As Byte()) As Boolean
      'Verify Signature
      Dim signer = New LmsSigner()
      Dim pubkey = LmsPublicKeyParameters.GetInstance(pubkeybob.ToArray())
      signer.Init(False, pubkey)
      Return signer.VerifySignature(message.ToArray(), signature.ToArray())
    End Function

    Private Shared Function ToRngParameter() As LmsParam
      Dim rand = New SecureRandom()
      Dim parameters = BcPqcServices.ToLmsParam()
      Dim idx = rand.[Next](parameters.Length)
      Return parameters(idx)
    End Function

    Public Shared Function ToPubKey(pubkey As Byte()) As LmsPublicKeyParameters
      Return LmsPublicKeyParameters.GetInstance(pubkey)
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
