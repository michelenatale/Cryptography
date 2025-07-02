Option Strict On
Option Explicit On

'ML-KEM (Kyber)
'Module-Lattice-Based
'FIPS PUB 203
'https://pq-crystals.org/kyber/index.shtml
'https://nvlpubs.nist.gov/nistpubs/FIPS/NIST.FIPS.203.ipd.pdf


Imports System.IO
Imports System.Text
Imports michele.natale.BcPqcs
Imports michele.natale.Services
Imports Org.BouncyCastle.Security
Imports System.Security.Cryptography
Imports Org.BouncyCastle.Crypto.Parameters

Namespace michele.natale.TestBcPqcs

  Public Module TestMLKEM
    Public Sub Start()
      Dim rounds = 10
      Test_ML_KEM_Key(10 * rounds)
      Test_ML_KEM_Single_Cryption(rounds)
      Test_ML_KEM_Single_Cryption_File(rounds)

      Test_ML_KEM_Alice_Bob_Cryption(rounds)

      Console.WriteLine()
    End Sub

#Region "ML-KEM Test-Methodes"
    Private Sub Test_ML_KEM_Key(rounds As Integer)
      'A new key pair is always created and saved. This takes time. 
      'For the temporary exchange, it would not be necessary to save
      'the keys, as all generated keys are only required for the
      'respective session.

      'Es wird immer wieder ein neuer Schlüsselpaar erstellt und
      'abgespeichert. Das braucht seine Zeit. 
      'Für den temporären Austausch, wäre das Abspeichern der
      'Schlüssel nicht notwending, da alle generierten Schlüssel
      'nur für die jeweilige Sitzung benötigt werden.

      Console.Write($"{NameOf(Test_ML_KEM_Key)}: ")

      If rounds < 10 Then rounds = 10
      Dim rand = New SecureRandom()
      Dim parameters = BcPqcServices.ToMLKemParameters()

      Dim sw = Stopwatch.StartNew()
      For i = 0 To rounds - 1
        'ML-KEM Parameter select. Only the first 3 parameters
        Dim idx = rand.[Next](parameters.Length)
        Dim parameter = parameters(idx)

        'Symmetric CryptionAlgorithm select
        Dim cas = [Enum].GetValues(Of CryptionAlgorithm)()
        idx = rand.[Next](cas.Length)
        Dim cryptoalgo = cas(idx)

        'Create and Save a legal ML-KEM-KeyPair
        Dim kpfile = "mlkem_keypair.key"
        Dim kp = MLKEM.ToKeyPair(rand, parameter)

        Using mlkeminfo = New MlKemKeyPairInfo(kp.PubKey, kp.PrivKey, parameter, cryptoalgo)
          mlkeminfo.SaveKeyPair(kpfile, True)

          'Load KeyPairs again
          Using info = MlKemKeyPairInfo.Load_KeyPair(kpfile)
            If Not mlkeminfo.Equals(info) Then Throw New Exception()

            'ML-KEM Encapsulation
            cryptoalgo = info.CryptAlgo
            parameter = info.ToParameter()

            'Encapsulation
            Dim capsulationkey As Byte() = Nothing
            Dim pubkey = MLKemPublicKeyParameters.FromEncoding(parameter, info.PubKey)
            Using key1 = MLKEM.ToSharedKey(pubkey, parameter, rand, capsulationkey)
              'Decapsulation 
              Dim privkey = MLKemPrivateKeyParameters.FromEncoding(parameter, info.ToPrivKey().ToBytes())
              Using key2 = MLKEM.ToSharedKey(privkey, parameter, capsulationkey)
                'Check equality.
                If Not key1.Equality(key2) Then Throw New Exception()
              End Using
            End Using
          End Using
        End Using

        If i Mod rounds / 10 = 0 Then Console.Write(".")
      Next
      sw.Stop()

      Dim t = sw.ElapsedMilliseconds
      Console.WriteLine($" rounds = {rounds}, t = {t}ms, td = {t / (2 * rounds)}ms")
    End Sub

    Private Sub Test_ML_KEM_Single_Cryption(rounds As Integer)
      'A new key pair is always created and saved. This takes time. 
      'For the temporary exchange, it would not be necessary to save
      'the keys, as all generated keys are only required for the
      'respective session.

      'Es wird immer wieder ein neuer Schlüsselpaar erstellt und
      'abgespeichert. Das braucht seine Zeit. 
      'Für den temporären Austausch, wäre das Abspeichern der
      'Schlüssel nicht notwending, da alle generierten Schlüssel
      'nur für die jeweilige Sitzung benötigt werden.

      Console.Write($"{NameOf(Test_ML_KEM_Single_Cryption)}: ")

      If rounds < 10 Then rounds = 10

      Dim sym_algo = String.Empty
      Dim rand = New SecureRandom()
      Dim parameters = BcPqcServices.ToMLKemParameters()

      Dim sw = Stopwatch.StartNew()
      For i = 0 To rounds - 1
        'Create a RngMessage
        Dim size = rand.[Next](10, 128)
        Dim message = BcPqcServices.RngCryptoBytes(size)

        'Create a associated 
        'size = rand.Next(8, 128);
        'var associated = RngBytes(rand, size);
        Dim associated = Encoding.UTF8.GetBytes("© michele natale 2025")

        'ML-KEM Parameter select
        Dim idx = rand.[Next](parameters.Length)
        Dim parameter = parameters(idx)

        'Symmetric CryptionAlgorithm select
        Dim cas = [Enum].GetValues(Of CryptionAlgorithm)()
        idx = rand.[Next](cas.Length)
        Dim cryptoalgo = cas(idx)
        sym_algo = cryptoalgo.ToString()

        'Create and Save a legal ML-KEM-KeyPair
        Dim kpfile = "mlkem_keypair.key"
        Dim kp = MLKEM.ToKeyPair(rand, parameter)
        Using info = New MlKemKeyPairInfo(kp.PubKey, kp.PrivKey, parameter, cryptoalgo)
          info.SaveKeyPair(kpfile, True)
        End Using

        'ML-KEM- and Symmetric Cryption
        Dim cipher = MLKEM.MlKemEncryption(message, kpfile, associated, rand)
        Dim decipher = MLKEM.MlKemDecryption(cipher, kpfile, associated)

        'Check equality.
        If Not message.SequenceEqual(decipher) Then Throw New Exception()

        If i Mod rounds / 10 = 0 Then Console.Write(".")
      Next
      sw.Stop()

      Dim t = sw.ElapsedMilliseconds
      Console.WriteLine($" rounds = {rounds}, lastalgo = {sym_algo}, t = {t}ms, td = {t / (2 * rounds)}ms")
    End Sub

    Private Sub Test_ML_KEM_Single_Cryption_File(rounds As Integer)
      'A new key pair is always created and saved. This takes time. 
      'For the temporary exchange, it would not be necessary to save
      'the keys, as all generated keys are only required for the
      'respective session.

      'Es wird immer wieder ein neuer Schlüsselpaar erstellt und
      'abgespeichert. Das braucht seine Zeit. 
      'Für den temporären Austausch, wäre das Abspeichern der
      'Schlüssel nicht notwending, da alle generierten Schlüssel
      'nur für die jeweilige Sitzung benötigt werden.

      Console.Write($"{NameOf(Test_ML_KEM_Single_Cryption_File)}: ")

      If rounds < 10 Then rounds = 10

      Dim sym_algo = String.Empty
      Dim rand = New SecureRandom()
      Dim parameters = BcPqcServices.ToMLKemParameters()
      Dim srcfile = "data", destfile = "cipher", srcrfile = "datar"

      Dim sw = Stopwatch.StartNew()
      For i = 0 To rounds - 1
        'Create a rng-testfile 
        Dim max = (1 << 21) + 1024
        Dim size = Random.Shared.[Next](max)
        SetRngFileData(srcfile, size)

        'Create a associated 
        size = rand.[Next](8, 128)
        Dim associated = BcPqcServices.RngCryptoBytes(size)
        'var associated = "© michele natale 2025"u8;

        'ML-KEM Parameter select
        Dim idx = rand.[Next](parameters.Length)
        Dim parameter = parameters(idx)

        'Symmetric CryptionAlgorithm select
        Dim cas = [Enum].GetValues(Of CryptionAlgorithm)()
        idx = rand.[Next](cas.Length)
        Dim cryptoalgo = cas(idx)
        sym_algo = cryptoalgo.ToString()

        'Create and Save a legal ML-KEM-KeyPair
        Dim kpfile = "mlkem_keypair.key"
        Dim kp = MLKEM.ToKeyPair(rand, parameter)
        Using info = New MlKemKeyPairInfo(kp.PubKey, kp.PrivKey, parameter, cryptoalgo)
          info.SaveKeyPair(kpfile, True)
        End Using

        'ML-KEM- and Symmetric Cryption
        MLKEM.MlKemEncryptionFile(srcfile, destfile, kpfile, associated, rand)
        MLKEM.MlKemDecryptionFile(destfile, srcrfile, kpfile, associated)

        'Check both files for equality.
        If Not FileEquals(srcrfile, srcrfile) Then Throw New Exception()

        If i Mod rounds / 10 = 0 Then Console.Write(".")
      Next
      sw.Stop()

      Dim t = sw.ElapsedMilliseconds
      Console.WriteLine($" rounds = {rounds}, lastalgo = {sym_algo}, t = {t}ms, td = {t / (2 * rounds)}ms")
    End Sub

    Private Sub Test_ML_KEM_Alice_Bob_Cryption(rounds As Integer)
      '*****************************************************
      'The KeyPair is not stored on any hardware here.
      'Everything remains in temporary exchange, which
      'is why it is so fast.
      '*****************************************************

      Console.Write($"{NameOf(Test_ML_KEM_Alice_Bob_Cryption)}: ")

      If rounds < 10 Then rounds = 10
      'rounds = rounds < 10 ? 10 : 10 * rounds;

      Dim sym_algo = String.Empty
      Dim rand = New SecureRandom()
      Dim cas = [Enum].GetValues(Of CryptionAlgorithm)()
      Dim parameters = BcPqcServices.ToMLKemParameters()

      Dim sw = Stopwatch.StartNew()
      For i = 0 To rounds - 1
        'Create a RngMessage
        Dim size = rand.[Next](10, 128)
        Dim message = BcPqcServices.RngCryptoBytes(size)

        'Create a associated 
        'size = rand.Next(8, 128);
        'var associated = RngBytes(rand, size);
        Dim associated = Encoding.UTF8.GetBytes(
          "© michele natale 2025")

        'ML-KEM Parameter select
        Dim idx = rand.[Next](parameters.Length)
        Dim parameter = parameters(idx)

        'Symmetric CryptionAlgorithm select      
        idx = rand.[Next](cas.Length)
        Dim cryptoalgo = cas(idx)
        sym_algo = cryptoalgo.ToString()

        'Alice needs the MLKEMParameter for the PublicKey.
        Using alice = New AliceMLKEM(parameter)

          'Bob holt sich den PublicKey und den MLKEMParameter von Alice,
          'wählt einen CryptoAlgo, und lässt sich einen key wie 
          'auch einen CapsulationKey generieren (Encapsulation), ...

          'Bob gets the PublicKey and the MLKEMParameter from Alice,
          'selects a CryptoAlgo, and generates a key as well
          'as a CapsulationKey (Encapsulation), ...
          Using bob = New BobMLKEM(parameter)
            Dim capsulationkey = bob.GenerateSharedKey(alice.PubKey)

            ' ... womit Alice den key aus der CapsulationKey holt. 
            ' Alice braucht dazu jedoch Ihren PrivateKey. (Decapsulation)
            ' Nun kann Alice eine Verschlüsselung durchführen.

            ' ... which Alice uses to get the key from the CapsulationKey.
            ' However, Alice needs your PrivateKey to do this. (Decapsulation)
            ' Alice can now perform encryption.
            Dim cipher = alice.Encryption(message, capsulationkey, associated, cryptoalgo)

            'Bob besitzt nun alle relevanten Infos, um eine Entschlüsselung durchzuführen.

            'Bob now has all the relevant information to carry out decryption.
            Dim decipher = bob.Decryption(cipher, associated, cryptoalgo)

            'Check for equality.
            If Not decipher.SequenceEqual(message) Then Throw New Exception()
          End Using
        End Using

        If i Mod rounds / 10 = 0 Then Console.Write(".")
      Next
      sw.Stop()

      Dim t = sw.ElapsedMilliseconds
      Console.WriteLine($" rounds = {rounds}, lastalgo = {sym_algo}, t = {t}ms, td = {t / (2 * rounds)}ms")
    End Sub

#End Region

#Region "Utils"

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
