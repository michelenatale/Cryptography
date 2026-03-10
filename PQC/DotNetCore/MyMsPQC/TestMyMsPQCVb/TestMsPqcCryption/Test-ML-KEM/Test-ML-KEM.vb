Option Strict On
Option Explicit On



Imports System.IO
Imports System.Text
Imports michele.natale.MsPqcs
Imports michele.natale.Services
Imports System.Security.Cryptography


Namespace michele.natale.TestMsPqcs

  Friend Class TestMLKEM

    Public Shared Sub Start()
      Dim rounds = 10
      Test_ML_KEM_SharedKey(10 * rounds)
      Test_ML_KEM_Single_Cryption(rounds)
      Test_ML_KEM_Single_Cryption_File(rounds)

      Test_ML_KEM_Alice_Bob_Cryption(rounds)

      Console.WriteLine()
    End Sub

#Region "ML-KEM Test-Methodes"
    Private Shared Sub Test_ML_KEM_SharedKey(rounds As Int32)
      'A new key pair is always created and saved. This takes time. 
      'For the temporary exchange, it would not be necessary to save
      'the keys, as all generated keys are only required for the
      'respective session.

      'Es wird immer wieder ein neuer Schlüsselpaar erstellt und
      'abgespeichert. Das braucht seine Zeit. 
      'Für den temporären Austausch, wäre das Abspeichern der
      'Schlüssel nicht notwending, da alle generierten Schlüssel
      'nur für die jeweilige Sitzung benötigt werden.

      Console.Write($"{NameOf(Test_ML_KEM_SharedKey)}: ")

      If rounds < 10 Then rounds = 10
      Dim algos = MsPqcServices.ToMLKemAlgorithm()

      Dim sw = Stopwatch.StartNew()
      For i As Int32 = 0 To rounds - 1
        'ML-KEM Parameter select. Only the first 3 algos
        Dim idx = RandomNumberGenerator.GetInt32(algos.Length)
        Dim algo = algos(idx)

        'Symmetric CryptionAlgorithm select
        Dim cas = [Enum].GetValues(Of CryptionAlgorithm)()
        idx = RandomNumberGenerator.GetInt32(cas.Length)
        Dim cryptoalgo = cas(idx)

        'Create and Save a legal ML-KEM-KeyPair
        Dim kpfile = "mlkem_keypair.key"
        Using kem = MLKem.GenerateKey(algo)
          Dim privpub = MlKemEx.ToKeyPair(kem)

          Using mlkeminfo = New MlKemKeyPairInfo(
            privpub.PubKey, privpub.PrivKey.ToBytes(), algo, cryptoalgo)
            mlkeminfo.SaveKeyPair(kpfile, True)

            'Load KeyPairs again
            Using info = MlKemKeyPairInfo.Load_KeyPair(kpfile)
              If Not mlkeminfo.Equals(info) Then
                Throw New Exception()
              End If

              'ML-KEM Encapsulation
              cryptoalgo = info.CryptAlgo
              algo = info.ToAlgo()

              'Encapsulation
              Dim pubkey = info.PubKey
              Using kem1 = MLKem.ImportEncapsulationKey(algo, pubkey)
                Dim capsulation_key As Byte() = Nothing
                Using sharedkey1 = MlKemEx.ToSharedKey(kem1, capsulation_key)

                  'Decapsulation 
                  Dim span = capsulation_key.AsSpan()
                  Dim privkey = info.ToPrivKey().ToBytes()
                  Using kem2 = MLKem.ImportDecapsulationKey(algo, privkey)
                    Using sharedkey2 = MlKemEx.ToSharedKey(kem2, span)
                      'Check equality.
                      If Not sharedkey1.Equality(sharedkey2) Then
                        Throw New Exception()
                      End If
                    End Using
                  End Using
                End Using
              End Using
            End Using
          End Using
        End Using

        If i Mod (rounds \ 10) = 0 Then
          Console.Write(".")
        End If
      Next
      sw.Stop()

      Dim t = sw.ElapsedMilliseconds
      Console.WriteLine($" rounds = {rounds}, t = {t}ms, td = {t / (2 * rounds)}ms")
    End Sub

    Private Shared Sub Test_ML_KEM_Single_Cryption(rounds As Int32)
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
      Dim algos = MsPqcServices.ToMLKemAlgorithm()

      Dim sw = Stopwatch.StartNew()
      For i = 0 To rounds - 1
        'Create a RngMessage
        Dim size = RandomNumberGenerator.GetInt32(10, 128)
        Dim message = MsPqcServices.RngCryptoBytes(size)

        'Create a associated 
        'size = rand.Next(8, 128);
        'var associated = RngBytes(rand, size);
        Dim associated = Encoding.UTF8.GetBytes("© michele natale 2025")

        'ML-KEM Parameter select
        Dim idx = RandomNumberGenerator.GetInt32(algos.Length)
        Dim algo = algos(idx)

        'Symmetric CryptionAlgorithm select
        Dim cas = [Enum].GetValues(Of CryptionAlgorithm)()
        idx = RandomNumberGenerator.GetInt32(cas.Length)
        Dim cryptoalgo = cas(idx)
        sym_algo = cryptoalgo.ToString()

        'Create and Save a legal ML-KEM-KeyPair
        Dim kpfile = "mlkem_keypair.key"
        Using kem = MLKem.GenerateKey(algo)
          Dim privpub = MlKemEx.ToKeyPair(kem)
          Using info = New MlKemKeyPairInfo(privpub.PubKey, privpub.PrivKey.ToBytes(), algo, cryptoalgo)
            info.SaveKeyPair(kpfile, True)

            'ML-KEM- and Symmetric Cryption
            Dim cipher = MlKemEx.MlKemEncryption(message, kpfile, associated)
            Dim decipher = MlKemEx.MlKemDecryption(cipher, kpfile, associated)

            'Check equality.
            If Not message.SequenceEqual(decipher) Then
              Throw New Exception()
            End If
          End Using
        End Using

        If i Mod rounds / 10 = 0 Then
          Console.Write(".")
        End If
      Next
      sw.[Stop]()

      Dim t = sw.ElapsedMilliseconds
      Console.WriteLine($" rounds = {rounds}, lastalgo = {sym_algo}, t = {t}ms, td = {t / (2 * rounds)}ms")
    End Sub

    Private Shared Sub Test_ML_KEM_Single_Cryption_File(rounds As Int32)
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
      Dim algos = MsPqcServices.ToMLKemAlgorithm()
      Dim srcfile = "data", destfile = "cipher", srcrfile = "datar"

      Dim sw = Stopwatch.StartNew()
      For i = 0 To rounds - 1
        'Create a rng-testfile 
        Dim max = (1 << 21) + 1024
        Dim size = Random.[Shared].[Next](max)
        SetRngFileData(srcfile, size)

        'Create a associated 
        size = RandomNumberGenerator.GetInt32(8, 128)
        Dim associated = MsPqcServices.RngCryptoBytes(size)
        'var associated = "© michele natale 2025"u8;

        'ML-KEM Parameter select
        Dim idx = RandomNumberGenerator.GetInt32(algos.Length)
        Dim algo = algos(idx)

        'Symmetric CryptionAlgorithm select
        Dim cas = [Enum].GetValues(Of CryptionAlgorithm)()
        idx = RandomNumberGenerator.GetInt32(cas.Length)
        Dim cryptoalgo = cas(idx)
        sym_algo = cryptoalgo.ToString()

        'Create and Save a legal ML-KEM-KeyPair
        Dim kpfile = "mlkem_keypair.key"
        Using kem = MLKem.GenerateKey(algo)
          Dim privpub = MlKemEx.ToKeyPair(kem)
          Using info = New MlKemKeyPairInfo(privpub.PubKey, privpub.PrivKey.ToBytes(), algo, cryptoalgo)
            info.SaveKeyPair(kpfile, True)

            'ML-KEM- and Symmetric Cryption
            MlKemEx.MlKemEncryptionFile(srcfile, destfile, kpfile, associated)
            MlKemEx.MlKemDecryptionFile(destfile, srcrfile, kpfile, associated)

            'Check both files for equality.
            If Not FileEquals(srcrfile, srcrfile) Then
              Throw New Exception()
            End If
          End Using
        End Using

        If i Mod rounds / 10 = 0 Then
          Console.Write(".")
        End If
      Next
      sw.[Stop]()

      Dim t = sw.ElapsedMilliseconds
      Console.WriteLine($" rounds = {rounds}, lastalgo = {sym_algo}, t = {t}ms, td = {t / (2 * rounds)}ms")
    End Sub


    Private Shared Sub Test_ML_KEM_Alice_Bob_Cryption(rounds As Int32)
      '*****************************************************
      'The KeyPair is not stored on any hardware here.
      'Everything remains in temporary exchange, which
      'is why it is so fast.
      '*****************************************************

      Console.Write($"{NameOf(Test_ML_KEM_Alice_Bob_Cryption)}: ")

      If rounds < 10 Then rounds = 10
      'rounds = rounds < 10 ? 10 : 10 * rounds;

      Dim sym_algo = String.Empty
      Dim cas = [Enum].GetValues(Of CryptionAlgorithm)()
      Dim algos = MsPqcServices.ToMLKemAlgorithm()

      Dim sw = Stopwatch.StartNew()
      For i = 0 To rounds - 1
        'Create a RngMessage
        Dim size = RandomNumberGenerator.GetInt32(10, 128)
        Dim message = MsPqcServices.RngCryptoBytes(size)

        'Create a associated 
        'size = rand.Next(8, 128);
        'var associated = RngBytes(rand, size);
        Dim associated = Encoding.UTF8.GetBytes("© michele natale 2025")

        'ML-KEM Parameter select
        Dim idx = RandomNumberGenerator.GetInt32(algos.Length)
        Dim algo = algos(idx)

        'Symmetric CryptionAlgorithm select      
        idx = RandomNumberGenerator.GetInt32(cas.Length)
        Dim cryptoalgo = cas(idx)
        sym_algo = cryptoalgo.ToString()

        'Alice needs the MLKEMParameter for the PublicKey.
        Using alice = New AliceMLKEM(algo)

          'Bob holt sich den PublicKey und den MLKEMParameter von Alice,
          'wählt einen CryptoAlgo, und lässt sich einen Sharedkey wie 
          'auch einen CapsulationKey generieren (Encapsulation), ...

          'Bob gets the PublicKey and the MLKEMParameter from Alice,
          'selects a CryptoAlgo, and generates a Sharedkey as well
          'as a CapsulationKey (Encapsulation), ...
          Using bob = New BobMLKEM(algo)
            Dim capsulationkey = bob.GenerateSharedKey(alice.PubKey)

            ' ... womit Alice den Sharedkey aus der CapsulationKey holt. 
            ' Alice braucht dazu jedoch Ihren PrivateKey. (Decapsulation)
            ' Nun kann Alice eine Verschlüsselung durchführen.

            ' ... which Alice uses to get the Sharedkey from the CapsulationKey.
            ' However, Alice needs your PrivateKey to do this. (Decapsulation)
            ' Alice can now perform encryption.
            Dim cipher = alice.Encryption(message, capsulationkey, associated, cryptoalgo)

            'Bob besitzt nun alle relevanten Infos, um eine Entschlüsselung durchzuführen.

            'Bob now has all the relevant information to carry out decryption.
            Dim decipher = bob.Decryption(cipher, associated, cryptoalgo)

            'Check for equality.
            If Not decipher.SequenceEqual(message) Then
              Throw New Exception()
            End If

          End Using
        End Using

        If i Mod rounds / 10 = 0 Then
          Console.Write(".")
        End If
      Next

      sw.[Stop]()

      Dim t = sw.ElapsedMilliseconds
      Console.WriteLine($" rounds = {rounds}, lastalgo = {sym_algo}, t = {t}ms, td = {t / (2 * rounds)}ms")
    End Sub

#End Region

#Region "Utils"
    Private Shared Sub SetRngFileData(filename As String, size As Int32)
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
#End Region
  End Class

End Namespace