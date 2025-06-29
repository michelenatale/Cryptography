Option Strict On
Option Explicit On


'LMS - Leighton-Micali Hash-Based Signatures
'Hash-Based Signatures
'RFC 8554
'https://www.rfc-editor.org/info/rfc8554


Imports System.IO
Imports System.Security
Imports michele.natale.BcPqcs
Imports michele.natale.Services
Imports Org.BouncyCastle.Security
Imports System.Security.Cryptography
Imports TestMyBcPQCVb.michele.natale.TestBcPqcs


Namespace michele.natale.BcPqcs

  Public Class TestLMS
    'https://openquantumsafe.org/liboqs/algorithms/sig_stfl/lms.html
    Public Shared Sub Start()
      Dim rounds = 10
      Test_LMS_Single_Signature(rounds)
      Test_LMS_Single_Signature_File(rounds)

      Test_LMS_Alice_Bob_Signature(rounds)

      Test_LMS_Multi_Signature(rounds)
      Test_LMS_Multi_Signature_File(rounds)
    End Sub
    Private Shared Sub Test_LMS_Single_Signature(rounds As Integer)
      'A new key pair is always created and saved. This takes time. 
      'For the temporary exchange, it would not be necessary to save
      'the keys, as all generated keys are only required for the
      'respective session.

      'Es wird immer wieder ein neuer Schlüsselpaar erstellt und
      'abgespeichert. Das braucht seine Zeit. 

      'Für den temporären Austausch, wäre das Abspeichern der
      'Schlüssel nicht notwending, da alle generierten Schlüssel
      'nur für die jeweilige Sitzung benötigt werden.

      Console.Write($"{NameOf(Test_LMS_Single_Signature)}: ")

      If rounds < 10 Then rounds = 10
      Dim rand = New SecureRandom()
      Dim parameters = BcPqcServices.ToLmsParam()

      Dim sw = Stopwatch.StartNew()
      For i = 0 To rounds - 1

        'Create a random Message
        Dim size = rand.[Next](10, 128)
        Dim message = BcPqcServices.RngCryptoBytes(size)

        'Lms Parameter select. 
        Dim idx = rand.[Next](parameters.Length)
        Dim parameter = parameters(idx)

        'For performance reasons, only the
        'parameters 'lms_sha256_h5_x'. 
        parameter = CType(CByte(parameter) Mod 4, LmsParam)

        'Create and Save a legal Lms-KeyPair
        Dim kpfile = "lms_keypair.key"
        Dim kp = LMS.ToKeyPair(rand, parameter)

        Using lmsinfo = New LmsKeyPairInfo(kp.PubKey, kp.PrivKey, parameter)
          lmsinfo.SaveKeyPair(kpfile, True)

          'Load KeyPairs Again
          Using info = LmsKeyPairInfo.Load_KeyPair(kpfile)
            If Not lmsinfo.Equals(info) Then Throw New Exception()

            Dim signature = LMS.Sign(info, message)
            Dim verify = LMS.Verify(info, signature, message)

            If Not verify Then Throw New Exception()

            If i Mod rounds / 10 = 0 Then Console.Write(".")
          End Using
        End Using
      Next

      sw.Stop()

      Dim t = sw.ElapsedMilliseconds
      Console.WriteLine($" rounds = {rounds}, t = {t}ms, td = {t / rounds}ms")
    End Sub

    Private Shared Sub Test_LMS_Single_Signature_File(rounds As Integer)

      'A new key pair is always created and saved. This takes time. 
      'For the temporary exchange, it would not be necessary to save
      'the keys, as all generated keys are only required for the
      'respective session.

      'Es wird immer wieder ein neuer Schlüsselpaar erstellt und
      'abgespeichert. Das braucht seine Zeit. 

      'Für den temporären Austausch, wäre das Abspeichern der
      'Schlüssel nicht notwending, da alle generierten Schlüssel
      'nur für die jeweilige Sitzung benötigt werden.

      Console.Write($"{NameOf(Test_LMS_Single_Signature_File)}: ")


      Dim srcfile = "data"
      Dim rand = New SecureRandom()
      Dim parameters = BcPqcServices.ToLmsParam()

      Dim sw = Stopwatch.StartNew()
      For i = 0 To rounds - 1
        'Create a rng-testfile 
        Dim max = (1 << 21) + 1024 'ca. 2Mb
        Dim size = Random.[Shared].[Next](max)
        SetRngFileData(srcfile, size)

        'LMS Parameter select. Only the first 3 parameters
        Dim idx = rand.[Next](parameters.Length)
        Dim parameter = parameters(idx)

        'For performance reasons, only the
        'parameters 'lms_sha256_h5_x'. 
        parameter = CType(Convert.ToByte(parameter) Mod 4, LmsParam)

        'Create and Save a legal LMS-KeyPair
        Dim kpfile = "lms_keypair.key"
        Dim kp = LMS.ToKeyPair(rand, parameter)
        Using lmsinfo = New LmsKeyPairInfo(kp.PubKey, kp.PrivKey, parameter)
          lmsinfo.SaveKeyPair(kpfile, True)

          'Load KeyPairs again
          Using info = LmsKeyPairInfo.Load_KeyPair(kpfile)
            If Not lmsinfo.Equals(info) Then Throw New Exception()

            Dim signature = LMS.Sign(info, srcfile)
            Dim verify = LMS.Verify(info, signature, srcfile)

            If Not verify Then Throw New Exception()

            If i Mod rounds / 10 = 0 Then Console.Write(".")
          End Using
        End Using
      Next
      sw.Stop()

      Dim t = sw.ElapsedMilliseconds
      Console.WriteLine($" rounds = {rounds}, t = {t}ms, td = {t / rounds}ms")
    End Sub

    Private Shared Sub Test_LMS_Alice_Bob_Signature(rounds As Integer)

      Console.Write($"{NameOf(Test_LMS_Alice_Bob_Signature)}: ")

      Dim rand = New SecureRandom()
      Dim parameters = BcPqcServices.ToLmsParam()

      Dim sw = Stopwatch.StartNew()
      For i = 0 To rounds - 1

        'Create a random Message
        Dim size = rand.[Next](10, 128)
        Dim message = BcPqcServices.RngCryptoBytes(size)

        'Alice select a LMS Parameter.
        Dim idx = rand.[Next](parameters.Length)
        Dim parameter = parameters(idx)

        'For performance reasons, only the
        'parameters 'lms_sha256_h5_x'. 
        parameter = CType(CByte(parameter) Mod 4, LmsParam)

        'Alice <<>> Bob
        '**************

        'Alice selects an LMS-Parameter and has
        'all keys generated with one instance. 
        Using alice = New AliceLMS(parameter)

          'Alice signs the message with her PrivateKey.
          Dim sign_pubk = alice.Sign(message)

          'Bob can now use Alice's PubKey to verify the message.
          Dim verify = BobLMS.Verify(alice.Info.PubKey, sign_pubk.Sign, message)

          If Not verify Then Throw New Exception()
        End Using

        idx = rand.[Next](parameters.Length)
        parameter = parameters(idx)

        'For performance reasons, only the
        'parameters 'lms_sha256_h5_x'. 
        parameter = CType(CByte(parameter) Mod 4, LmsParam)

        'Bob <<>> Alice
        '**************

        'Bob selects an LMS-Parameter and has
        'all keys generated with one instance. 
        Using bob = New BobLMS(parameter)

          'Bob signs the message with his PrivateKey.
          Dim sign_pubk = bob.Sign(message)

          'Alice can now use Bob's PubKey to verify the message.
          Dim verify = AliceLMS.Verify(bob.Info.PubKey, sign_pubk.Sign, message)

          If Not verify Then Throw New Exception()
        End Using

        If i Mod rounds / 10 = 0 Then Console.Write(".")
      Next

      sw.Stop()

      Dim t = sw.ElapsedMilliseconds
      Console.WriteLine($" rounds = {rounds}, t = {t}ms, td = {t / (2 * rounds)}ms")
    End Sub

    Private Shared Sub Test_LMS_Multi_Signature(rounds As Integer)
      Console.Write($"{NameOf(Test_LMS_Multi_Signature)}: ")

      Dim s = 0 'Sum Signatories.
      Dim rand = New SecureRandom()
      Dim parameters = BcPqcServices.ToLmsParam()

      Dim sw = Stopwatch.StartNew()
      For i = 0 To rounds - 1
        'number of signatories
        Dim cnt = rand.[Next](3, 10)
        s += cnt

        'Create random Message
        Dim size = rand.[Next](10, 128)
        Dim message = BcPqcServices.RngCryptoBytes(size)

        'LMS Parameter select. Only the first 3 parameters
        Dim idx = rand.[Next](parameters.Length)
        Dim parameter = parameters(idx)

        'For performance reasons, only the
        'parameters 'lms_sha256_h5_x'. 
        parameter = CType(CByte(parameter) Mod 4, LmsParam)

        'The order in 'signinfo' does not matter. 
        ''signinfo' can also be saved and reloaded.
        Dim signinfo = SignInfoSamples(cnt, message, parameter)
        Using multiinfo = New LMSMultiSignVerifyInfo(signinfo)

          'Dim filename = "multiinfo"
          'multiinfo.Save(filename)
          'multiinfo.Load(filename)

          'Check multiinfo us null
          If multiinfo Is Nothing Then Throw New NullReferenceException($"{NameOf(multiinfo)} has failed!")

          'privkey and pubkey are in 'multiinfo'
          Dim sign = multiinfo.MultiSign()
          Dim verify = multiinfo.MultiVerify(sign)

          If Not verify Then Throw New Exception()

          If i Mod rounds / 10 = 0 Then Console.Write(".")
        End Using
      Next

      sw.Stop()

      Dim t = sw.ElapsedMilliseconds
      Console.WriteLine($" rounds = {rounds}, signatories = {s / rounds}, t = {t}ms, td = {t / rounds}ms")
    End Sub

    Private Shared Sub Test_LMS_Multi_Signature_File(rounds As Integer)
      Console.Write($"{NameOf(Test_LMS_Multi_Signature_File)}: ")

      Dim srcfile = "data"
      Dim s = 0 'Sum Signatories.
      Dim rand = New SecureRandom()
      Dim parameters = BcPqcServices.ToLmsParam()

      Dim sw = Stopwatch.StartNew()
      For i = 0 To rounds - 1
        'Create a rng-testfile 
        Dim max = (1 << 21) + 1024 '
        Dim size = Random.[Shared].[Next](max)
        SetRngFileData(srcfile, size)

        'number of signatories
        Dim cnt = rand.[Next](3, 10)
        s += cnt

        'LMS Parameter select. Only the first 3 parameters
        Dim idx = rand.[Next](parameters.Length)
        Dim parameter = parameters(idx)

        'For performance reasons, only the
        'parameters 'lms_sha256_h5_x'.
        parameter = CType(CByte(parameter) Mod 4, LmsParam)

        'The order in 'signinfo' does not matter. 
        ''signinfo' can also be saved and reloaded.
        Dim signinfo = SignInfoSamples(cnt, srcfile, parameter)
        Using multiinfo = New LMSMultiSignVerifyInfoFile(signinfo, srcfile)

          'Dim filename = "multiinfo"
          'multiinfo.Save(filename)
          'multiinfo.Load(filename)

          'Check multiinfo us null
          If multiinfo Is Nothing Then Throw New NullReferenceException($"{NameOf(multiinfo)} has failed!")

          'privkey and pubkey are in 'multiinfo'
          Dim sign = multiinfo.MultiSign()
          Dim verify = multiinfo.MultiVerify(sign)

          If Not verify Then Throw New Exception()

          If i Mod rounds / 10 = 0 Then Console.Write(".")
        End Using
      Next

      sw.Stop()

      Dim t = sw.ElapsedMilliseconds
      Console.WriteLine($" rounds = {rounds}, signatories = {s / rounds}, t = {t}ms, td = {t / rounds}ms")
    End Sub

#Region "Utils"

    Private Shared Function SignInfoSamples(size As Integer, message As Byte(), parameter As LmsParam) As LMSSignInfo()
      If size < 2 OrElse message.Length < 10 Then Throw New ArgumentOutOfRangeException(NameOf(size))

      Dim rand = New SecureRandom()
      Dim result = New LMSSignInfo(size - 1) {}
      Dim msg = Convert.ToHexString(message)
      For i = 0 To size - 1
        Dim name = $"Name_{i}"

        'Create a legal LMS-KeyPair 
        Dim kp = LMS.ToKeyPair(rand, parameter)
        Dim info = New LmsKeyPairInfo(kp.PubKey, kp.PrivKey, parameter)

        'Create signatur and check verification.
        Dim signature = LMS.Sign(info, message)
        If LMS.Verify(info, signature, message) Then
          result(i) = New LMSSignInfo(name, parameter, msg, kp.PubKey, signature)

          Continue For
        End If

        Throw New VerificationException($"{NameOf(SignInfoSamples)} has failed!")
      Next

      Return result
    End Function

    Private Shared Function SignInfoSamples(size As Integer, datapath As String, parameter As LmsParam) As LMSSignInfo()
      Dim rand = New SecureRandom()
      Dim msghash = ToFileHash(datapath)
      Dim result = New LMSSignInfo(size - 1) {}

      For i = 0 To size - 1
        Dim name = $"Name_{i}"
        Dim msghex = Convert.ToHexString(msghash)

        'Create a legal LMS-KeyPair 
        Dim kp = LMS.ToKeyPair(rand, parameter)
        Dim info = New LmsKeyPairInfo(kp.PubKey, kp.PrivKey, parameter)

        'Create signatur and check verification.
        Dim signature = LMS.Sign(info, msghash)
        If LMS.Verify(info, signature, msghash) Then
          result(i) = New LMSSignInfo(name, parameter, msghex, kp.PubKey, signature)

          Continue For
        End If

        Throw New VerificationException($"{NameOf(SignInfoSamples)} has failed!")
      Next

      Return result
    End Function

    Private Shared Function ToFileHash(datapath As String) As Byte()
      If Not File.Exists(datapath) Then Throw New FileNotFoundException(datapath)

      Using fsin = New FileStream(datapath, FileMode.Open, FileAccess.Read)
        Return SHA512.HashData(fsin)
      End Using
    End Function

    Private Shared Sub SetRngFileData(filename As String, size As Integer)
      Using fsout = New FileStream(filename, FileMode.Create, FileAccess.Write)

        Dim length = If(size < 1024 * 1024, size, 1024 * 1024)

        While length > 0
          fsout.Write(BcPqcServices.RngCryptoBytes(length))
          size -= length
          length = If(size < 1024 * 1024, size, 1024 * 1024)
        End While
      End Using
    End Sub

    Public Shared Function FileEquals(leftfile As String, rightfile As String) As Boolean
      Using fsleft = New FileStream(leftfile, FileMode.Open, FileAccess.Read)
        Using fsright = New FileStream(rightfile, FileMode.Open, FileAccess.Read)
          Return SHA256.HashData(fsleft).SequenceEqual(SHA256.HashData(fsright))
        End Using
      End Using
    End Function

#End Region

  End Class
End Namespace
