'ML-DSA (Dilithium)
'Module-Lattice-Based
'FIPS PUB 204
'https://pq-crystals.org/dilithium/index.shtml
'https://nvlpubs.nist.gov/nistpubs/fips/nist.fips.204.pdf

Option Strict On
Option Explicit On

Imports System.IO
Imports System.Security
Imports michele.natale.MsPqcs
Imports michele.natale.Services
Imports System.Security.Cryptography
Imports TestMyMsPQCVb.michele.natale.TestMsPqcs

Namespace michele.natale.MsPqcs
  Public Class TestMLDSA
    Public Shared Async Function Start() As Task
      Dim rounds = 10
      Test_ML_DSA_Single_Signature(rounds)
      Await Test_ML_DSA_Single_Signature_File(rounds)

      Test_ML_DSA_Alice_Bob_Signature(rounds)

      Test_ML_DSA_Multi_Signature(rounds)
      Await Test_ML_DSA_Multi_Signature_File(rounds)

      Console.WriteLine()
    End Function


    Private Shared Sub Test_ML_DSA_Single_Signature(rounds As Integer)
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
      Dim algos = MsPqcServices.ToMLDsaAlgorithm()

      Dim sw = Stopwatch.StartNew()
      For i = 0 To rounds - 1

        'Create a random Message
        Dim size = RandomNumberGenerator.GetInt32(10, 128)
        Dim message = MsPqcServices.RngCryptoBytes(size)

        'ML-DSA Parameter select. Only the first 3 parameters
        Dim idx = RandomNumberGenerator.GetInt32(algos.Length)
        Dim algo = algos(idx)

        'Create and Save a legal ML-DSA-KeyPair
        Dim kpfile = "mldsa_keypair.key"
        Dim kem = MLDsa.GenerateKey(algo)
        Dim privpub = MlDsaEx.ToKeyPair(kem)

        Using mldsainfo = New MlDsaKeyPairInfo(
          privpub.PubKey, privpub.PrivKey.ToBytes(), algo)
          mldsainfo.SaveKeyPair(kpfile, True)


          'Load KeyPairs Again
          Dim info = MlDsaKeyPairInfo.Load_KeyPair(kpfile)
          If Not mldsainfo.Equals(info) Then Throw New Exception()

          Dim signature = MlDsaEx.Sign(info, message)
          Dim verify = MlDsaEx.Verify(info, signature, message)

          If Not verify Then
            Throw New Exception()
          End If

        End Using


        If i Mod rounds / 10 = 0 Then
          Console.Write(".")
        End If
      Next

      sw.[Stop]()

      Dim t = sw.ElapsedMilliseconds
      Console.WriteLine($" rounds = {rounds}, t = {t}ms, td = {t / rounds}ms")
    End Sub

    Private Shared Async Function Test_ML_DSA_Single_Signature_File(rounds As Integer) As Task

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
      Dim algos = MsPqcServices.ToMLDsaAlgorithm()

      Dim sw = Stopwatch.StartNew()
      For i = 0 To rounds - 1
        'Create a rng-testfile 
        Dim max = (1 << 21) + 1024 'ca. 2Mb
        Dim size = Random.[Shared].[Next](1, max)
        SetRngFileData(srcfile, size)

        'ML-DSA Parameter select. Only the first 3 parameters
        Dim idx = RandomNumberGenerator.GetInt32(algos.Length)
        Dim algo = algos(idx)

        'Create and Save a legal ML-DSA-KeyPair
        Dim kpfile = "mldsa_keypair.key"
        Using kem = MLDsa.GenerateKey(algo)
          Dim privpub = MlDsaEx.ToKeyPair(kem)
          Using mldsainfo = New MlDsaKeyPairInfo(
            privpub.PubKey, privpub.PrivKey.ToBytes(), algo)
            mldsainfo.SaveKeyPair(kpfile, True)

            'Load KeyPairs again
            Using info = MlDsaKeyPairInfo.Load_KeyPair(kpfile)
              If Not mldsainfo.Equals(info) Then
                Throw New Exception()
              End If

              Dim signature = Await MlDsaEx.SignSha512Async(info, srcfile)
              Dim verify = Await MlDsaEx.VerifySha512Async(info, signature, srcfile)

              If Not verify Then
                Throw New Exception()
              End If

            End Using
          End Using
        End Using

        If i Mod rounds / 10 = 0 Then
          Console.Write(".")
        End If
      Next
      sw.[Stop]()

      Dim t = sw.ElapsedMilliseconds
      Console.WriteLine($" rounds = {rounds}, t = {t}ms, td = {t / rounds}ms")
    End Function

    Private Shared Sub Test_ML_DSA_Alice_Bob_Signature(rounds As Integer)

      Console.Write($"{NameOf(Test_ML_DSA_Alice_Bob_Signature)}: ")

      rounds = If(rounds < 10, 10, 10 * rounds)

      Dim algos = MsPqcServices.ToMLDsaAlgorithm()

      Dim sw = Stopwatch.StartNew()
      For i = 0 To rounds - 1

        'Create a random Message
        Dim size = RandomNumberGenerator.GetInt32(10, 128)
        Dim message = MsPqcServices.RngCryptoBytes(size)

        'Alice select a ML-DSA Parameter.
        Dim idx = RandomNumberGenerator.GetInt32(algos.Length)
        Dim algo = algos(idx)

        If True Then
          'Alice <<>> Bob
          '**************

          'Alice selects an ML-DSA-Parameter and has
          'all keys generated with one instance. 
          Using alice = New AliceMLDSA(algo)

            'Alice signs the message with her PrivateKey.
            Dim signpubkey = alice.Sign(message)

            'Bob can now use Alice's PubKey to verify the message.
            Dim verify = BobMLDSA.Verify(
              alice.Info.PubKey, signpubkey.Item1, message, algo)

            If Not verify Then
              Throw New Exception()
            End If
          End Using
        End If


        idx = RandomNumberGenerator.GetInt32(algos.Length)
        algo = algos(idx)

        If True Then
          'Bob <<>> Alice
          '**************

          'Bob selects an ML-DSA-Parameter and has
          'all keys generated with one instance. 
          Using bob = New BobMLDSA(algo)

            'Bob signs the message with his PrivateKey.
            Dim signPubkey = bob.Sign(message)

            'Alice can now use Bob's PubKey to verify the message.
            Dim verify = AliceMLDSA.Verify(
              bob.Info.PubKey, signPubkey.Item1, message, algo)

            If Not verify Then
              Throw New Exception()
            End If
          End Using
        End If

        If i Mod rounds / 10 = 0 Then
          Console.Write(".")
        End If
      Next

      sw.[Stop]()

      Dim t = sw.ElapsedMilliseconds
      Console.WriteLine($" rounds = {rounds}, t = {t}ms, td = {t / (2 * rounds)}ms")
    End Sub

    Private Shared Sub Test_ML_DSA_Multi_Signature(rounds As Integer)
      Console.Write($"{NameOf(Test_ML_DSA_Multi_Signature)}: ")

      If rounds < 10 Then rounds = 10

      Dim s = 0 'Sum Signatories. 
      Dim algos = MsPqcServices.ToMLDsaAlgorithm()

      Dim sw = Stopwatch.StartNew()
      For i = 0 To rounds - 1
        'number of signatories
        Dim cnt = RandomNumberGenerator.GetInt32(5, 30)
        s += cnt

        'Create random Message
        Dim size = RandomNumberGenerator.GetInt32(10, 128)
        Dim message = MsPqcServices.RngCryptoBytes(size)

        'ML-DSA Parameter select. Only the first 3 parameters
        Dim idx = RandomNumberGenerator.GetInt32(algos.Length)
        Dim algo = algos(idx)

        'The order in 'signinfo' does not matter. 
        ''signinfo' can also be saved and reloaded.
        Dim signinfo = SignInfoSamples(cnt, message, algo)
        Using multiinfo = New MLDSAMultiSignVerifyInfo(signinfo)

          'var filename = "multiinfo";
          'multiinfo.Save(filename);
          'multiinfo.Load(filename);

          'Check multiinfo us null
          If multiinfo Is Nothing Then
            Throw New NullReferenceException(
              $"{NameOf(multiinfo)} has failed!")
          End If

          'privkey and pubkey are in 'multiinfo'
          Dim sign = multiinfo.MultiSign()
          Dim verify = multiinfo.MultiVerify(sign)

          If Not verify Then
            Throw New Exception()
          End If
        End Using

        If i Mod rounds / 10 = 0 Then
          Console.Write(".")
        End If
      Next

      sw.[Stop]()

      Dim t = sw.ElapsedMilliseconds
      Console.WriteLine($" rounds = {rounds}, signatories = {s / rounds}, t = {t}ms, td = {t / rounds}ms")
    End Sub


    Private Shared Async Function Test_ML_DSA_Multi_Signature_File(rounds As Integer) As Task
      Console.Write($"{NameOf(Test_ML_DSA_Multi_Signature_File)}: ")

      If rounds < 10 Then rounds = 10

      Dim srcfile = "data"
      Dim s = 0 'Sum Signatories.
      Dim algos = MsPqcServices.ToMLDsaAlgorithm()

      Dim sw = Stopwatch.StartNew()
      For i = 0 To rounds - 1
        'Create a rng-testfile 
        Dim max = (1 << 21) + 1024 'ca. 2Mb
        Dim size = Random.[Shared].[Next](max)
        SetRngFileData(srcfile, size)

        'number of signatories
        Dim cnt = RandomNumberGenerator.GetInt32(5, 30)
        s += cnt

        'ML-DSA Parameter select. Only the first 3 parameters
        Dim idx = RandomNumberGenerator.GetInt32(algos.Length)
        Dim algo = algos(idx)

        'The order in 'signinfo' does not matter. 
        ''signinfo' can also be saved and reloaded.
        Dim signinfo = SignInfoSamples(cnt, srcfile, algo)
        Using multiinfo = New MLDSAMultiSignVerifyInfoFile(signinfo, srcfile)

          'var filename = "multiinfo";
          'multiinfo.Save(filename);
          'multiinfo.Load(filename);

          'Check multiinfo us null
          If multiinfo Is Nothing Then
            Throw New NullReferenceException(
              $"{NameOf(multiinfo)} has failed!")
          End If

          'privkey and pubkey are in 'multiinfo'
          Dim sign = Await multiinfo.MultiSign()
          Dim verify = Await multiinfo.MultiVerify(sign)

          If Not verify Then
            Throw New Exception()
          End If
        End Using

        If i Mod rounds / 10 = 0 Then
          Console.Write(".")
        End If
      Next

      sw.[Stop]()

      Dim t = sw.ElapsedMilliseconds
      Console.WriteLine($" rounds = {rounds}, signatories = {s / rounds}, t = {t}ms, td = {t / rounds}ms")
    End Function


    Private Shared Function SignInfoSamples(
      size As Integer, message As Byte(),
      algo As MLDsaAlgorithm) As MLDSASignInfo()

      If size < 2 OrElse message.Length < 10 Then
        Throw New ArgumentOutOfRangeException(NameOf(size))
      End If

      Dim result = New MLDSASignInfo(size - 1) {}
      For i = 0 To size - 1
        Dim name = $"Name_{i}"
        Dim msg = Convert.ToHexString(message)

        'Create a legal ML-DSA-KeyPair 
        Using kem = MLDsa.GenerateKey(algo)
          Dim privpub = MlDsaEx.ToKeyPair(kem)
          Using info = New MlDsaKeyPairInfo(
            privpub.PubKey, privpub.PrivKey.ToBytes(), algo)

            'Create signatur and check verification.
            Dim signature = MlDsaEx.Sign(info, message)
            If MlDsaEx.Verify(info, signature, message) Then
              result(i) = New MLDSASignInfo(
                name, algo, msg, privpub.PubKey, signature)
              Continue For
            End If
          End Using
        End Using

        Throw New VerificationException(
          $"{NameOf(SignInfoSamples)} has failed!")
      Next

      Return result
    End Function

    Private Shared Function SignInfoSamples(
      size As Integer, datapath As String,
      algo As MLDsaAlgorithm) As MLDSASignInfo()

      Dim msghash = ToFileHash(datapath)
      Dim result = New MLDSASignInfo(size - 1) {}

      For i = 0 To size - 1
        Dim name = $"Name_{i}"
        Dim msghex = Convert.ToHexString(msghash)

        'Create a legal ML-DSA-KeyPair 
        Using dsa = MLDsa.GenerateKey(algo)
          Dim privpub = MlDsaEx.ToKeyPair(dsa)
          Using info = New MlDsaKeyPairInfo(
          privpub.PubKey, privpub.PrivKey.ToBytes(), algo)

            'Create signatur and check verification.
            Dim signature = MlDsaEx.Sign(info, msghash)
            If MlDsaEx.Verify(info, signature, msghash) Then
              result(i) = New MLDSASignInfo(
              name, algo, msghex, privpub.PubKey, signature)
              Continue For
            End If
          End Using
        End Using

        Throw New VerificationException(
          $"{NameOf(SignInfoSamples)} has failed!")
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
          fsout.Write(MsPqcServices.RngCryptoBytes(length))
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

  End Class
End Namespace

