Option Strict On
Option Explicit On


'SLH-DSA (SPHINCS+)
'Stateless Hash-Based
'FIPS PUB 205
'https://sphincs.org/
'https://nvlpubs.nist.gov/nistpubs/fips/nist.fips.205.pdf


Imports System.IO
Imports System.Security
Imports michele.natale.BcPqcs
Imports michele.natale.Services
Imports Org.BouncyCastle.Security
Imports System.Security.Cryptography
Imports Org.BouncyCastle.Crypto.Parameters
Imports TestMyBcPQCVb.michele.natale.TestBcPqcs

Namespace michele.natale.BcPqcs

  Public Module TestSLHDSA

    Public Sub Start()
      Dim rounds = 1
      Test_SLH_DSA_Single_Signature(rounds)
      Test_SLH_DSA_Single_Signature_File(rounds)

      Test_SLH_DSA_Alice_Bob_Signature(rounds)

      Test_SLH_DSA_Multi_Signature(rounds)
      Test_SLH_DSA_Multi_Signature_File(rounds)

      Console.WriteLine()
    End Sub

    Private Sub Test_SLH_DSA_Single_Signature(rounds As Integer)
      'A new key pair is always created and saved. This takes time. 
      'For the temporary exchange, it would not be necessary to save
      'the keys, as all generated keys are only required for the
      'respective session.

      'Es wird immer wieder ein neuer Schlüsselpaar erstellt und
      'abgespeichert. Das braucht seine Zeit. 

      'Für den temporären Austausch, wäre das Abspeichern der
      'Schlüssel nicht notwending, da alle generierten Schlüssel
      'nur für die jeweilige Sitzung benötigt werden.

      Console.Write($"{NameOf(Test_SLH_DSA_Single_Signature)}: ")

      Dim rand = New SecureRandom()
      Dim parameters = BcPqcServices.ToSLHDsaParameters()

      Dim sw = Stopwatch.StartNew()
      For i = 0 To rounds - 1
        'Create a random Message
        Dim size = rand.[Next](10, 128)
        Dim message = BcPqcServices.RngCryptoBytes(size)

        'SLH-DSA Parameter select. Only the first 3 parameters
        Dim idx = rand.[Next](parameters.Length)
        Dim parameter = parameters(idx)

        'Create and Save a legal SLH-DSA-KeyPair
        Dim kpfile = "slhdsa_keypair.key"
        Dim kp = SLHDSA.ToKeyPair(rand, parameter)

        Using slhdsainfo = New SlhDsaKeyPairInfo(kp.PubKey, kp.PrivKey, parameter)
          slhdsainfo.SaveKeyPair(kpfile, True)

          'Load KeyPairs Again
          Using info = SlhDsaKeyPairInfo.Load_KeyPair(kpfile)
            If Not slhdsainfo.Equals(info) Then Throw New Exception()

            Dim signature = SLHDSA.Sign(info, message)
            Dim verify = SLHDSA.Verify(info, signature, message)

            If Not verify Then Throw New Exception()
          End Using
        End Using
        If rounds > 10 Then
          If i Mod rounds / 10 = 0 Then Console.Write(".")
        End If
      Next

      sw.Stop()

      Dim t = sw.ElapsedMilliseconds
      Console.WriteLine($" rounds = {rounds}, t = {t}ms, td = {t / rounds}ms")
    End Sub

    Private Sub Test_SLH_DSA_Single_Signature_File(rounds As Integer)

      'A new key pair is always created and saved. This takes time. 
      'For the temporary exchange, it would not be necessary to save
      'the keys, as all generated keys are only required for the
      'respective session.

      'Es wird immer wieder ein neuer Schlüsselpaar erstellt und
      'abgespeichert. Das braucht seine Zeit. 

      'Für den temporären Austausch, wäre das Abspeichern der
      'Schlüssel nicht notwending, da alle generierten Schlüssel
      'nur für die jeweilige Sitzung benötigt werden.

      Console.Write($"{NameOf(Test_SLH_DSA_Single_Signature_File)}: ")


      Dim srcfile = "data"
      Dim rand = New SecureRandom()
      Dim parameters = BcPqcServices.ToSLHDsaParameters()

      Dim sw = Stopwatch.StartNew()
      For i = 0 To rounds - 1
        'Create a rng-testfile 
        Dim max = (1 << 21) + 1024 'ca. 2Mb
        Dim size = Random.Shared.[Next](max)
        SetRngFileData(srcfile, size)

        'SLH-DSA Parameter select. Only the first 3 parameters
        Dim idx = rand.[Next](parameters.Length)
        Dim parameter = parameters(idx)

        'Create and Save a legal SLH-DSA-KeyPair
        Dim kpfile = "slhdsa_keypair.key"
        Dim kp = SLHDSA.ToKeyPair(rand, parameter)
        Using slhdsainfo = New SlhDsaKeyPairInfo(kp.PubKey, kp.PrivKey, parameter)
          slhdsainfo.SaveKeyPair(kpfile, True)

          'Load KeyPairs again
          Using info = SlhDsaKeyPairInfo.Load_KeyPair(kpfile)
            If Not slhdsainfo.Equals(info) Then Throw New Exception()

            Dim signature = SLHDSA.Sign(info, srcfile)
            Dim verify = SLHDSA.Verify(info, signature, srcfile)

            If Not verify Then Throw New Exception()
          End Using
        End Using

        If rounds > 10 Then
          If i Mod rounds / 10 = 0 Then Console.Write(".")
        End If
      Next
      sw.Stop()

      Dim t = sw.ElapsedMilliseconds
      Console.WriteLine($" rounds = {rounds}, t = {t}ms, td = {t / rounds}ms")
    End Sub

    Private Sub Test_SLH_DSA_Alice_Bob_Signature(rounds As Integer)

      Console.Write($"{NameOf(Test_SLH_DSA_Alice_Bob_Signature)}: ")


      Dim rand = New SecureRandom()
      Dim parameters = BcPqcServices.ToSLHDsaParameters()

      Dim sw = Stopwatch.StartNew()
      For i = 0 To rounds - 1

        'Create a random Message
        Dim size = rand.[Next](10, 128)
        Dim message = BcPqcServices.RngCryptoBytes(size)

        'Alice select a SLH-DSA Parameter.
        Dim idx = rand.[Next](parameters.Length)
        Dim parameter = parameters(idx)

        'Alice <<>> Bob
        '**************

        'Alice selects an SLH-DSA-Parameter and has
        'all keys generated with one instance. 
        Using alice = New AliceSLHDSA(parameter)

          'Alice signs the message with her PrivateKey.
          Dim sp = alice.Sign(message)

          'Bob can now use Alice's PubKey to verify the message.
          Dim verify = BobSLHDSA.Verify(alice.Info.PubKey, sp.Sign, message, parameter)

          If Not verify Then Throw New Exception()
        End Using

        idx = rand.[Next](parameters.Length)
        parameter = parameters(idx)

        'Bob <<>> Alice
        '**************

        'Bob selects an SLH-DSA-Parameter and has
        'all keys generated with one instance. 
        Using bob = New BobSLHDSA(parameter)

          'Bob signs the message with his PrivateKey.
          Dim sp = bob.Sign(message)

          'Alice can now use Bob's PubKey to verify the message.
          Dim verify = AliceSLHDSA.Verify(bob.Info.PubKey, sp.Sign, message, parameter)

          If Not verify Then Throw New Exception()
        End Using

        If rounds > 10 Then
          If i Mod rounds / 10 = 0 Then Console.Write(".")
        End If
      Next

      sw.Stop()

      Dim t = sw.ElapsedMilliseconds
      Console.WriteLine($" rounds = {rounds}, t = {t}ms, td = {t / (2 * rounds)}ms")
    End Sub

    Private Sub Test_SLH_DSA_Multi_Signature(rounds As Integer)
      Console.Write($"{NameOf(Test_SLH_DSA_Multi_Signature)}: ")

      Dim s = 0 'Sum Signatories.
      Dim rand = New SecureRandom()
      Dim parameters = BcPqcServices.ToSLHDsaParameters()

      Dim sw = Stopwatch.StartNew()
      For i = 0 To rounds - 1
        'number of signatories
        Dim cnt = rand.[Next](3, 10)
        s += cnt

        'Create random Message
        Dim size = rand.[Next](10, 128)
        Dim message = BcPqcServices.RngCryptoBytes(size)

        'SLH-DSA Parameter select. Only the first 3 parameters
        Dim idx = rand.[Next](parameters.Length)
        Dim parameter = parameters(idx)

        'The order in 'signinfo' does not matter. 
        ''signinfo' can also be saved and reloaded.
        Dim signinfo = SignInfoSamples(cnt, message, parameter)
        Using multiinfo = New SLHDSAMultiSignVerifyInfo(signinfo)

          'Dim filename = "multiinfo"
          'multiinfo.Save(filename)
          'multiinfo.Load(filename)

          'Check multiinfo us null
          If multiinfo Is Nothing Then Throw New NullReferenceException($"{NameOf(multiinfo)} has failed!")

          'privkey and pubkey are in 'multiinfo'
          Dim sign = multiinfo.MultiSign()
          Dim verify = multiinfo.MultiVerify(sign)

          If Not verify Then Throw New Exception()
        End Using

        If rounds > 10 Then
          If i Mod rounds / 10 = 0 Then Console.Write(".")
        End If
      Next

      sw.Stop()

      Dim t = sw.ElapsedMilliseconds
      Console.WriteLine($" rounds = {rounds}, signatories = {s / rounds}, t = {t}ms, td = {t / rounds}ms")
    End Sub

    Private Sub Test_SLH_DSA_Multi_Signature_File(rounds As Integer)
      Console.Write($"{NameOf(Test_SLH_DSA_Multi_Signature_File)}: ")


      Dim srcfile = "data"
      Dim s = 0 'Sum Signatories.
      Dim rand = New SecureRandom()
      Dim parameters = BcPqcServices.ToSLHDsaParameters()

      Dim sw = Stopwatch.StartNew()
      For i = 0 To rounds - 1
        'Create a rng-testfile 
        Dim max = (1 << 21) + 1024 '
        Dim size = Random.Shared.[Next](max)
        SetRngFileData(srcfile, size)

        'number of signatories
        Dim cnt = rand.[Next](3, 10)
        s += cnt

        'SLH-DSA Parameter select. Only the first 3 parameters
        Dim idx = rand.[Next](parameters.Length)
        Dim parameter = parameters(idx)

        'The order in 'signinfo' does not matter. 
        ''signinfo' can also be saved and reloaded.
        Dim signinfo = SignInfoSamples(cnt, srcfile, parameter)
        Using multiinfo = New SLHDSAMultiSignVerifyInfoFile(signinfo, srcfile)

          'Dim filename = "multiinfo"
          'multiinfo.Save(filename)
          'multiinfo.Load(filename)

          'Check multiinfo us null
          If multiinfo Is Nothing Then Throw New NullReferenceException($"{NameOf(multiinfo)} has failed!")

          'privkey and pubkey are in 'multiinfo'
          Dim sign = multiinfo.MultiSign()
          Dim verify = multiinfo.MultiVerify(sign)

          If Not verify Then Throw New Exception()
        End Using

        If rounds > 10 Then
          If i Mod rounds / 10 = 0 Then Console.Write(".")
        End If
      Next

      sw.Stop()

      Dim t = sw.ElapsedMilliseconds
      Console.WriteLine($" rounds = {rounds}, signatories = {s / rounds}, t = {t}ms, td = {t / rounds}ms")
    End Sub


#Region "Utils"

    Private Function SignInfoSamples(size As Integer, message As Byte(), parameter As SlhDsaParameters) As SLHDSASignInfo()
      If size < 2 OrElse message.Length < 10 Then Throw New ArgumentOutOfRangeException(NameOf(size))

      Dim rand = New SecureRandom()
      Dim result = New SLHDSASignInfo(size - 1) {}
      For i = 0 To size - 1
        Dim name = $"Name_{i}"
        Dim msg = Convert.ToHexString(message)

        'Create a legal slh-DSA-KeyPair 
        Dim kp = SLHDSA.ToKeyPair(rand, parameter)
        Using info = New SlhDsaKeyPairInfo(kp.PubKey, kp.PrivKey, parameter)

          'Create signatur and check verification.
          Dim signature = SLHDSA.Sign(info, message)
          If SLHDSA.Verify(info, signature, message) Then
            result(i) = New SLHDSASignInfo(name, parameter, msg, kp.PubKey, signature)

            Continue For
          End If
        End Using

        Throw New VerificationException($"{NameOf(SignInfoSamples)} has failed!")
      Next

      Return result
    End Function

    Private Function SignInfoSamples(size As Integer, datapath As String, parameter As SlhDsaParameters) As SLHDSASignInfo()
      Dim rand = New SecureRandom()
      Dim msghash = ToFileHash(datapath)
      Dim result = New SLHDSASignInfo(size - 1) {}

      For i = 0 To size - 1
        Dim name = $"Name_{i}"
        Dim msghex = Convert.ToHexString(msghash)

        'Create a legal slh-DSA-KeyPair 
        Dim kp = SLHDSA.ToKeyPair(rand, parameter)
        Using info = New SlhDsaKeyPairInfo(kp.PubKey, kp.PrivKey, parameter)

          'Create signatur and check verification.
          Dim signature = SLHDSA.Sign(info, msghash)
          If SLHDSA.Verify(info, signature, msghash) Then
            result(i) = New SLHDSASignInfo(name, parameter, msghex, kp.PubKey, signature)

            Continue For
          End If
        End Using

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
