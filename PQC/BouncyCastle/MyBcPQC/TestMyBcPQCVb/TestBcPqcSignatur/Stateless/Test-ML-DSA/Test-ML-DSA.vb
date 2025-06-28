'ML-DSA (Dilithium)
'Module-Lattice-Based
'FIPS PUB 204
'https://pq-crystals.org/dilithium/index.shtml
'https://nvlpubs.nist.gov/nistpubs/fips/nist.fips.204.pdf


Imports System.IO
Imports System.Security
Imports michele.natale.BcPqcs
Imports michele.natale.Services
Imports Org.BouncyCastle.Security
Imports System.Security.Cryptography
Imports Org.BouncyCastle.Crypto.Parameters
Imports TestMyBcPQCVb.michele.natale.TestBcPqcs

Namespace michele.natale.BcPqcs

  Public Module TestMLDSA
    Public Sub Start()
      Dim rounds = 10
      Test_ML_DSA_Single_Signature(rounds)
      Test_ML_DSA_Single_Signature_File(rounds)

      Test_ML_DSA_Alice_Bob_Signature(rounds)

      Test_ML_DSA_Multi_Signature(rounds)
      Test_ML_DSA_Multi_Signature_File(rounds)

      Console.WriteLine()
    End Sub

    Private Sub Test_ML_DSA_Single_Signature(rounds As Integer)
      'A new key pair is always created and saved. This takes time. 
      'For the temporary exchange, it would not be necessary to save
      'the keys, as all generated keys are only required for the
      'respective session.

      'Es wird immer wieder ein neuer Schlüsselpaar erstellt und
      'abgespeichert. Das braucht seine Zeit. 

      'Für den temporären Austausch, wäre das Abspeichern der
      'Schlüssel nicht notwending, da alle generierten Schlüssel
      'nur für die jeweilige Sitzung benötigt werden.

      Console.Write($"{NameOf(Test_ML_DSA_Single_Signature)}: ")

      If rounds < 10 Then rounds = 10
      Dim rand = New SecureRandom()
      Dim parameters = BcPqcServices.ToMLDsaParameters()

      Dim sw = Stopwatch.StartNew()
      For i = 0 To rounds - 1

        'Create a random Message
        Dim size = rand.[Next](10, 128)
        Dim message = BcPqcServices.RngCryptoBytes(size)

        'ML-DSA Parameter select. Only the first 3 parameters
        Dim idx = rand.[Next](parameters.Length)
        Dim parameter = parameters(idx)

        'Create and Save a legal ML-DSA-KeyPair
        Dim kpfile = "mldsa_keypair.key"
        Dim kp = MLDSA.ToKeyPair(rand, parameter)

        Using mldsainfo = New MlDsaKeyPairInfo(
          kp.PubKey, kp.PrivKey, parameter)
          mldsainfo.SaveKeyPair(kpfile, True)

          'Load KeyPairs Again
          Using info = MlDsaKeyPairInfo.Load_KeyPair(kpfile)
            If Not mldsainfo.Equals(info) Then Throw New Exception()

            Dim signature = MLDSA.Sign(info, message)
            Dim verify = MLDSA.Verify(info, signature, message)

            If Not verify Then Throw New Exception()
          End Using
        End Using

        If i Mod rounds / 10 = 0 Then Console.Write(".")
      Next

      sw.Stop()

      Dim t = sw.ElapsedMilliseconds
      Console.WriteLine($" rounds = {rounds}, t = {t}ms, td = {t / rounds}ms")
    End Sub

    Private Sub Test_ML_DSA_Single_Signature_File(rounds As Integer)

      'A new key pair is always created and saved. This takes time. 
      'For the temporary exchange, it would not be necessary to save
      'the keys, as all generated keys are only required for the
      'respective session.

      'Es wird immer wieder ein neuer Schlüsselpaar erstellt und
      'abgespeichert. Das braucht seine Zeit. 

      'Für den temporären Austausch, wäre das Abspeichern der
      'Schlüssel nicht notwending, da alle generierten Schlüssel
      'nur für die jeweilige Sitzung benötigt werden.

      Console.Write($"{NameOf(Test_ML_DSA_Single_Signature_File)}: ")

      If rounds < 10 Then rounds = 10

      Dim srcfile = "data"
      Dim rand = New SecureRandom()
      Dim parameters = BcPqcServices.ToMLDsaParameters()

      Dim sw = Stopwatch.StartNew()
      For i = 0 To rounds - 1
        'Create a rng-testfile 
        Dim max = (1 << 21) + 1024 'ca. 2Mb
        Dim size = Random.Shared.[Next](max)
        SetRngFileData(srcfile, size)

        'ML-DSA Parameter select. Only the first 3 parameters
        Dim idx = rand.[Next](parameters.Length)
        Dim parameter = parameters(idx)

        'Create and Save a legal ML-DSA-KeyPair
        Dim kpfile = "mldsa_keypair.key"
        Dim kp = MLDSA.ToKeyPair(rand, parameter)
        Using mldsainfo = New MlDsaKeyPairInfo(
          kp.PubKey, kp.PrivKey, parameter)
          mldsainfo.SaveKeyPair(kpfile, True)

          'Load KeyPairs again
          Using info = MlDsaKeyPairInfo.Load_KeyPair(kpfile)
            If Not mldsainfo.Equals(info) Then Throw New Exception()

            Dim signature = MLDSA.Sign(info, srcfile)
            Dim verify = MLDSA.Verify(info, signature, srcfile)

            If Not verify Then Throw New Exception()
          End Using
        End Using

        If i Mod rounds / 10 = 0 Then Console.Write(".")
      Next
      sw.Stop()

      Dim t = sw.ElapsedMilliseconds
      Console.WriteLine($" rounds = {rounds}, t = {t}ms, td = {t / rounds}ms")
    End Sub

    Private Sub Test_ML_DSA_Alice_Bob_Signature(rounds As Integer)

      Console.Write($"{NameOf(Test_ML_DSA_Alice_Bob_Signature)}: ")

      rounds = If(rounds < 10, 10, 10 * rounds)

      Dim rand = New SecureRandom()
      Dim parameters = BcPqcServices.ToMLDsaParameters()

      Dim sw = Stopwatch.StartNew()
      For i = 0 To rounds - 1

        'Create a random Message
        Dim size = rand.[Next](10, 128)
        Dim message = BcPqcServices.RngCryptoBytes(size)

        'Alice select a ML-DSA Parameter.
        Dim idx = rand.[Next](parameters.Length)
        Dim parameter = parameters(idx)


        'Alice <<>> Bob
        '**************

        'Alice selects an ML-DSA-Parameter and has
        'all keys generated with one instance. 
        Using alice = New AliceMLDSA(parameter)

          'Alice signs the message with her PrivateKey.
          Dim sp = alice.Sign(message)

          'Bob can now use Alice's PubKey to verify the message.
          Dim verify = BobMLDSA.Verify(alice.Info.PubKey, sp.Sign, message, parameter)

          If Not verify Then Throw New Exception()
        End Using

        idx = rand.[Next](parameters.Length)
        parameter = parameters(idx)

        'Bob <<>> Alice
        '**************

        'Bob selects an ML-DSA-Parameter and has
        'all keys generated with one instance. 
        Using bob = New BobMLDSA(parameter)

          'Bob signs the message with his PrivateKey.
          Dim sp = bob.Sign(message)

          'Alice can now use Bob's PubKey to verify the message.
          Dim verify = AliceMLDSA.Verify(bob.Info.PubKey, sp.Sign, message, parameter)

          If Not verify Then Throw New Exception()
        End Using

        If i Mod rounds / 10 = 0 Then Console.Write(".")
      Next

      sw.Stop()

      Dim t = sw.ElapsedMilliseconds
      Console.WriteLine($" rounds = {rounds}, t = {t}ms, td = {t / (2 * rounds)}ms")
    End Sub

    Private Sub Test_ML_DSA_Multi_Signature(rounds As Integer)
      Console.Write($"{NameOf(Test_ML_DSA_Multi_Signature)}: ")

      If rounds < 10 Then rounds = 10

      Dim s = 0 'Sum Signatories.
      Dim rand = New SecureRandom()
      Dim parameters = BcPqcServices.ToMLDsaParameters()

      Dim sw = Stopwatch.StartNew()
      For i = 0 To rounds - 1
        'number of signatories
        Dim cnt = rand.[Next](5, 30)
        s += cnt

        'Create random Message
        Dim size = rand.[Next](10, 128)
        Dim message = BcPqcServices.RngCryptoBytes(size)

        'ML-DSA Parameter select. Only the first 3 parameters
        Dim idx = rand.[Next](parameters.Length)
        Dim parameter = parameters(idx)

        'The order in 'signinfo' does not matter. 
        ''signinfo' can also be saved and reloaded.
        Dim signinfo = SignInfoSamples(cnt, message, parameter)
        Using multiinfo = MLDSAMultiSignVerifyInfo.ToMultiInfo(signinfo)

          'Check multiinfo us null
          If multiinfo Is Nothing Then Throw New NullReferenceException($"{NameOf(multiinfo)} has failed!")

          'privkey and pubkey are in 'multiinfo'
          Dim sign = MLDSAMultiSignVerifyInfo.MultiSign(multiinfo, message)
          Dim verify = MLDSAMultiSignVerifyInfo.MultiVerify(multiinfo, sign, message)

          If Not verify Then Throw New Exception()
        End Using

        If i Mod rounds / 10 = 0 Then Console.Write(".")
      Next

      sw.Stop()

      Dim t = sw.ElapsedMilliseconds
      Console.WriteLine($" rounds = {rounds}, signatories = {s / rounds}, t = {t}ms, td = {t / rounds}ms")
    End Sub

    Private Sub Test_ML_DSA_Multi_Signature_File(rounds As Integer)
      Console.Write($"{NameOf(Test_ML_DSA_Multi_Signature_File)}: ")

      If rounds < 10 Then rounds = 10

      Dim srcfile = "data"
      Dim s = 0 'Sum Signatories.
      Dim rand = New SecureRandom()
      Dim parameters = BcPqcServices.ToMLDsaParameters()

      Dim sw = Stopwatch.StartNew()
      For i = 0 To rounds - 1
        'Create a rng-testfile 
        Dim max = (1 << 21) + 1024 'ca. 2Mb
        Dim size = Random.Shared.[Next](max)
        SetRngFileData(srcfile, size)

        'number of signatories
        Dim cnt = rand.[Next](5, 30)
        s += cnt

        'ML-DSA Parameter select. Only the first 3 parameters
        Dim idx = rand.[Next](parameters.Length)
        Dim parameter = parameters(idx)

        'The order in 'signinfo' does not matter. 
        ''signinfo' can also be saved and reloaded.
        Dim signinfo = SignInfoSamples(cnt, srcfile, parameter)
        Using multiinfo = MLDSAMultiSignVerifyInfo.ToMultiInfo(signinfo)

          'Check multiinfo us null
          If multiinfo Is Nothing Then Throw New NullReferenceException($"{NameOf(multiinfo)} has failed!")

          'privkey and pubkey are in 'multiinfo'
          Dim sign = MLDSAMultiSignVerifyInfo.MultiSign(multiinfo, srcfile)
          Dim verify = MLDSAMultiSignVerifyInfo.MultiVerify(multiinfo, sign, srcfile)

          If Not verify Then Throw New Exception()
        End Using

        If i Mod rounds / 10 = 0 Then Console.Write(".")
      Next

      sw.Stop()

      Dim t = sw.ElapsedMilliseconds
      Console.WriteLine($" rounds = {rounds}, signatories = {s / rounds}, t = {t}ms, td = {t / rounds}ms")
    End Sub
#Region "Utils"

    Private Function SignInfoSamples(size As Integer,
      message As Byte(), parameter As MLDsaParameters) As MLDSASignInfo()
      If size < 2 OrElse message.Length < 10 Then Throw New ArgumentOutOfRangeException(NameOf(size))

      Dim rand = New SecureRandom()
      Dim result = New MLDSASignInfo(size - 1) {}
      For i = 0 To size - 1
        Dim name = $"Name_{i}"
        Dim msg = Convert.ToHexString(message)

        'Create a legal ML-DSA-KeyPair 
        Dim kp = MLDSA.ToKeyPair(rand, parameter)
        Dim info = New MlDsaKeyPairInfo(kp.PubKey, kp.PrivKey, parameter)

        'Create signatur and check verification.
        Dim signature = MLDSA.Sign(info, message)
        If MLDSA.Verify(info, signature, message) Then
          result(i) = New MLDSASignInfo(name, parameter, msg, kp.PubKey, signature)

          Continue For
        End If

        Throw New VerificationException($"{NameOf(SignInfoSamples)} has failed!")
      Next

      Return result
    End Function

    Private Function SignInfoSamples(size As Integer, datapath As String, parameter As MLDsaParameters) As MLDSASignInfo()
      Dim rand = New SecureRandom()
      Dim msghash = ToFileHash(datapath)
      Dim result = New MLDSASignInfo(size - 1) {}

      For i = 0 To size - 1
        Dim name = $"Name_{i}"
        Dim msghex = Convert.ToHexString(msghash)

        'Create a legal ML-DSA-KeyPair 
        Dim kp = MLDSA.ToKeyPair(rand, parameter)
        Dim info = New MlDsaKeyPairInfo(kp.PubKey, kp.PrivKey, parameter)

        'Create signatur and check verification.
        Dim signature = MLDSA.Sign(info, msghash)
        If MLDSA.Verify(info, signature, msghash) Then
          result(i) = New MLDSASignInfo(name, parameter, msghex, kp.PubKey, signature)

          Continue For
        End If

        Throw New VerificationException($"{NameOf(SignInfoSamples)} has failed!")
      Next

      Return result
    End Function

    Private Function ToFileHash(datapath As String) As Byte()
      If Not File.Exists(datapath) Then Throw New FileNotFoundException(datapath)

      Using fsin = New FileStream(datapath, FileMode.Open, FileAccess.Read)
        Return SHA512.HashData(fsin)
      End Using
    End Function

    Private Sub SetRngFileData(filename As String, size As Integer)
      Using fsout = New FileStream(filename, FileMode.Create, FileAccess.Write)

        Dim length = If(size < 1024 * 1024, size, 1024 * 1024)

        While length > 0
          fsout.Write(BcPqcServices.RngCryptoBytes(length))
          size -= length
          length = If(size < 1024 * 1024, size, 1024 * 1024)
        End While
      End Using
    End Sub

    Public Function FileEquals(leftfile As String, rightfile As String) As Boolean
      Using fsleft = New FileStream(leftfile, FileMode.Open, FileAccess.Read)
        Using fsright = New FileStream(rightfile, FileMode.Open, FileAccess.Read)
          Return SHA256.HashData(fsleft).SequenceEqual(SHA256.HashData(fsright))
        End Using
      End Using
    End Function

#End Region
  End Module
End Namespace
