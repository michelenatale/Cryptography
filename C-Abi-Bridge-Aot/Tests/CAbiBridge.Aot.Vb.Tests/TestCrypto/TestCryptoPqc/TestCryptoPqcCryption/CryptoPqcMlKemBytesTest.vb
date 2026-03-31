Option Strict On
Option Explicit On


Imports System.Numerics
Imports System.Security.Cryptography
Imports System.Text
Imports michele.natale
Imports michele.natale.Pointers



'Start
'Alice(Keypair)
'    >> Bob(Encapsulation)
'        >> Alice(Encryption)
'            >> Bob(Decryption)
'Finish


Namespace michele.natale.Tests
  Partial Class CryptoPqcMlKemTest
    Public Shared Sub TestPqcMlKemCreateKeyPairs(rounds As Int32)
      Console.Write($"{NameOf(TestPqcMlKemCreateKeyPairs)}Aot: ")

      Dim rand = Random.[Shared]
      Dim sw = Stopwatch.StartNew()
      Dim priv_key_ptr As IntPtr = Nothing, priv_key_length As Int32 = Nothing, pub_key_ptr As IntPtr = Nothing, pub_key_length As Int32 = Nothing, guid_id_ptr As IntPtr = Nothing, guid_id_length As Int32 = Nothing, mlkem_param As Byte = Nothing, crypto_algo As Byte = Nothing
      For i = 0 To rounds - 1
        Dim err = CreateMlKemKeyPairAot(
          priv_key_ptr, priv_key_length,
          pub_key_ptr, pub_key_length,
          guid_id_ptr, guid_id_length,
          mlkem_param, crypto_algo)
        AssertError(err)

        Dim privkey = New UsIPtr(Of Byte)(ToBytes(priv_key_ptr, priv_key_length))
        FreeBuffer(priv_key_ptr)

        Dim pubkey = ToBytes(pub_key_ptr, pub_key_length)
        FreeBuffer(pub_key_ptr)

        Dim guid = New Guid(ToBytes(guid_id_ptr, guid_id_length))
        FreeBuffer(guid_id_ptr)

        Dim cryptoalgo = CType(crypto_algo, CryptionAlgorithm)
        Dim param = CryptoPqcMlKemUtilsTest.ToMLKemAlgorithm(CType(mlkem_param, MLKemParam))

        If i Mod rounds / 10 = 0 Then Console.Write(".")
      Next

      sw.[Stop]()

      Dim t = sw.ElapsedMilliseconds
      Console.Write($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms")
      Console.WriteLine()
    End Sub

    Public Shared Sub TestPqcMlKemCreateKeyPairsParam(rounds As Int32)
      Console.Write($"{NameOf(TestPqcMlKemCreateKeyPairsParam)}Aot: ")

      Dim rand = Random.[Shared]
      Dim sw = Stopwatch.StartNew()

      Dim priv_key_ptr As IntPtr = IntPtr.Zero, priv_key_length As Int32 = -1,
        pub_key_ptr As IntPtr = IntPtr.Zero, pub_key_length As Int32 = -1,
        guid_id_ptr As IntPtr = IntPtr.Zero, guid_id_length As Int32 = -1

      For i = 0 To rounds - 1
        Dim mlkem_param = CryptoPqcMlKemUtilsTest.ToMLKemAlgorithm()

        'ML-KEM Parameter select. Only the first 3 algos
        Dim idx = RandomNumberGenerator.GetInt32(mlkem_param.Length)
        Dim param = CByte(CryptoPqcMlKemUtilsTest.FromMLKemAlgorithm(mlkem_param(idx)))

        'Symmetric CryptionAlgorithm select
        Dim cas = [Enum].GetValues(Of CryptionAlgorithm)()
        idx = RandomNumberGenerator.GetInt32(cas.Length)
        Dim cryptoalgo = CByte(cas(idx))

        Dim err = CreateMlKemKeyPairParamAot(
          param, cryptoalgo,
          priv_key_ptr, priv_key_length,
          pub_key_ptr, pub_key_length,
          guid_id_ptr, guid_id_length)
        AssertError(err)

        Dim privkey = New UsIPtr(Of Byte)(ToBytes(priv_key_ptr, priv_key_length))
        FreeBuffer(priv_key_ptr)

        Dim pubkey = ToBytes(pub_key_ptr, pub_key_length)
        FreeBuffer(pub_key_ptr)

        Dim guid = New Guid(ToBytes(guid_id_ptr, guid_id_length))
        FreeBuffer(guid_id_ptr)

        If i Mod rounds / 10 = 0 Then Console.Write(".")
      Next

      sw.[Stop]()

      Dim t = sw.ElapsedMilliseconds
      Console.Write($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms")
      Console.WriteLine()
    End Sub

    Public Shared Sub TestPqcMlKemSafeLoadKeyPairs(rounds As Int32)
      Console.Write($"{NameOf(TestPqcMlKemSafeLoadKeyPairs)}Aot: ")

      Dim rand = Random.[Shared]
      Dim sw = Stopwatch.StartNew()

      For i = 0 To rounds - 1

        Dim priv_key_ptr As IntPtr = IntPtr.Zero, priv_key_length As Int32 = -1,
        pub_key_ptr As IntPtr = IntPtr.Zero, pub_key_length As Int32 = -1,
        guid_id_ptr As IntPtr = IntPtr.Zero, guid_id_length As Int32 = -1,
        mlkem_param As Byte = 0, crypto_algo As Byte = 0

        Dim err = CreateMlKemKeyPairAot(
          priv_key_ptr, priv_key_length,
          pub_key_ptr, pub_key_length,
          guid_id_ptr, guid_id_length,
          mlkem_param, crypto_algo)
        AssertError(err)

        Dim privkey = New UsIPtr(Of Byte)(ToBytes(priv_key_ptr, priv_key_length))
        FreeBuffer(priv_key_ptr)

        Dim pubkey = ToBytes(pub_key_ptr, pub_key_length)
        FreeBuffer(pub_key_ptr)

        Dim guid_bytes = ToBytes(guid_id_ptr, guid_id_length)
        FreeBuffer(guid_id_ptr)

        Dim guid = New Guid(guid_bytes)
        Dim cryptoalgo = CType(crypto_algo, CryptionAlgorithm)
        Dim param = CryptoPqcMlKemUtilsTest.ToMLKemAlgorithm(CType(mlkem_param, MLKemParam))

        'WICHTIG: Wenn 'with_priv_key = false', dann wird der
        '         PrivateKey nicht abgespeichert.

        'IMPORTANT: If 'with_priv_key = false', the private
        '           key will not be saved.
        Dim with_priv_key = Int32.IsEvenInteger(rand.[Next]())

        Dim kpfile = Encoding.UTF8.GetBytes("mlkem_keypair.key")
        err = SavePqcMlKemKeyPairAot(
          kpfile, kpfile.Length,
          privkey.ToBytes(), privkey.Length,
          pubkey, pubkey.Length,
          guid_bytes, guid_bytes.Length,
          mlkem_param, crypto_algo,
          with_priv_key)
        AssertError(err)

        Dim priv_key_ptr2 As IntPtr = IntPtr.Zero, priv_key_length2 As Int32 = -1,
          pub_key_ptr2 As IntPtr = IntPtr.Zero, pub_key_length2 As Int32 = -1,
          guid_id_ptr2 As IntPtr = IntPtr.Zero, guid_id_length2 As Int32 = -1,
          mlkem_param2 As Byte = 0, crypto_algo2 As Byte = 0

        err = LoadPqcMlKemKeyPairAot(
          kpfile, kpfile.Length,
          priv_key_ptr2, priv_key_length2,
          pub_key_ptr2, pub_key_length2,
          guid_id_ptr2, guid_id_length2,
          mlkem_param2, crypto_algo2)
        AssertError(err)

        Dim privkey2 = New UsIPtr(Of Byte)(ToBytes(priv_key_ptr2, priv_key_length2))
        FreeBuffer(priv_key_ptr2)

        Dim pubkey2 = ToBytes(pub_key_ptr2, pub_key_length2)
        FreeBuffer(pub_key_ptr2)

        Dim guid_bytes2 = ToBytes(guid_id_ptr2, guid_id_length2)
        FreeBuffer(guid_id_ptr2)

        Dim guid2 = New Guid(guid_bytes2)
        Dim cryptoalgo2 = CType(crypto_algo2, CryptionAlgorithm)
        Dim param2 = CryptoPqcMlKemUtilsTest.ToMLKemAlgorithm(CType(mlkem_param2, MLKemParam))

        If with_priv_key AndAlso Not privkey.Equality(privkey2) Then
          Throw New Exception()
        ElseIf Not with_priv_key AndAlso privkey2.Length <> 0 Then
          Throw New Exception()
        End If

        If Not pubkey.SequenceEqual(pubkey2) Then
          Throw New Exception()
        End If

        If Not guid_bytes.SequenceEqual(guid_bytes2) Then
          Throw New Exception()
        End If

        If Not guid = guid2 Then
          Throw New Exception()
        End If

        If Not cryptoalgo = cryptoalgo2 Then
          Throw New Exception()
        End If

        If Not param = param2 Then
          Throw New Exception()
        End If

        If i Mod rounds / 10 = 0 Then
          Console.Write(".")
        End If
      Next

      sw.[Stop]()

      Dim t = sw.ElapsedMilliseconds
      Console.Write($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms")
      Console.WriteLine()
    End Sub

    Public Shared Sub TestCapsulationSharedKeyWithPublicKey(rounds As Int32)
      'Bob bekommt von Alice ihren 'Publickey' sowie den
      ''MlKem Parameter' und lässt sich so seinen 'SharedKey'
      'und den 'Capsulation' generieren.

      'Bob receives Alice's ''Publickey' and
      ''MlKem Parameter' and uses them to generate
      'his 'SharedKey' and the 'Capsulation'.

      Console.Write($"{NameOf(TestCapsulationSharedKeyWithPublicKey)}Aot: ")

      Dim rand = Random.[Shared]
      Dim sw = Stopwatch.StartNew()

      For i = 0 To rounds - 1

        Dim alice_priv_key_ptr As IntPtr = IntPtr.Zero, alice_priv_key_length As Int32 = -1,
          alice_pub_key_ptr As IntPtr = IntPtr.Zero, alice_pub_key_length As Int32 = -1,
          alice_guid_id_ptr As IntPtr = IntPtr.Zero, alice_guid_id_length As Int32 = -1,
          mlkem_param As Byte = 0, crypto_algo As Byte = 0


        'Alice generates a new KeyPair
        Dim err = CreateMlKemKeyPairAot(
          alice_priv_key_ptr, alice_priv_key_length,
          alice_pub_key_ptr, alice_pub_key_length,
          alice_guid_id_ptr, alice_guid_id_length,
          mlkem_param, crypto_algo)
        AssertError(err)

        Dim aliceprivkey = New UsIPtr(Of Byte)(ToBytes(alice_priv_key_ptr, alice_priv_key_length))
        FreeBuffer(alice_priv_key_ptr)

        Dim alicepubkey = ToBytes(alice_pub_key_ptr, alice_pub_key_length)
        FreeBuffer(alice_pub_key_ptr)

        Dim aliceguid = New Guid(ToBytes(alice_guid_id_ptr, alice_guid_id_length))
        FreeBuffer(alice_guid_id_ptr)

        Dim cryptoalgo = CType(crypto_algo, CryptionAlgorithm)
        Dim param = CryptoPqcMlKemUtilsTest.ToMLKemAlgorithm(CType(mlkem_param, MLKemParam))

        Dim bob_shared_key_ptr As IntPtr = IntPtr.Zero, bob_shared_key_length As Int32 = -1,
          bob_capsulation_ptr As IntPtr = IntPtr.Zero, bob_capsulation_length As Int32 = -1

        'Bob uses it to generate his SharedKey and Capsulation.
        err = ToPqcMlKemCapsulationFromPubKeyAot(
          alicepubkey, alice_pub_key_length, mlkem_param,
          bob_shared_key_ptr, bob_shared_key_length,
          bob_capsulation_ptr, bob_capsulation_length)
        AssertError(err)

        Dim bobsharedkey = New UsIPtr(Of Byte)(ToBytes(bob_shared_key_ptr, bob_shared_key_length))
        FreeBuffer(bob_shared_key_ptr)

        Dim bobcapsulation = ToBytes(bob_capsulation_ptr, bob_capsulation_length)
        FreeBuffer(bob_capsulation_ptr)

        If bobsharedkey.IsEmpty Then
          Throw New Exception()
        End If

        If IsNullOrEmpty(bobcapsulation) Then
          Throw New Exception()
        End If

        If i Mod rounds / 10 = 0 Then
          Console.Write(".")
        End If
      Next

      sw.[Stop]()

      Dim t = sw.ElapsedMilliseconds
      Console.Write($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms")
      Console.WriteLine()
    End Sub


    Public Shared Sub TestPqcMlKemSharedKeyFromCapsualtionPrivateKey(rounds As Int32)
      Console.Write($"{NameOf(TestPqcMlKemSharedKeyFromCapsualtionPrivateKey)}Aot: ")

      Dim sw = New Stopwatch()
      Dim rand = Random.[Shared]

      For i = 0 To rounds - 1

        Dim alice_priv_key_ptr As IntPtr = IntPtr.Zero, alice_priv_key_length As Int32 = -1,
          alice_pub_key_ptr As IntPtr = IntPtr.Zero, alice_pub_key_length As Int32 = -1,
          alice_guid_id_ptr As IntPtr = IntPtr.Zero, alice_guid_id_length As Int32 = -1,
          mlkem_param As Byte = 0, crypto_algo As Byte = 0


        'Alice generates a new KeyPair
        Dim err = CreateMlKemKeyPairAot(
          alice_priv_key_ptr, alice_priv_key_length,
          alice_pub_key_ptr, alice_pub_key_length,
          alice_guid_id_ptr, alice_guid_id_length,
          mlkem_param, crypto_algo)
        AssertError(err)

        Dim aliceprivkey = New UsIPtr(Of Byte)(ToBytes(alice_priv_key_ptr, alice_priv_key_length))
        FreeBuffer(alice_priv_key_ptr)

        Dim alicepubkey = ToBytes(alice_pub_key_ptr, alice_pub_key_length)
        FreeBuffer(alice_pub_key_ptr)

        Dim aliceguid = New Guid(ToBytes(alice_guid_id_ptr, alice_guid_id_length))
        FreeBuffer(alice_guid_id_ptr)

        Dim cryptoalgo = CType(crypto_algo, CryptionAlgorithm)
        Dim param = CryptoPqcMlKemUtilsTest.ToMLKemAlgorithm(CType(mlkem_param, MLKemParam))

        Dim bob_shared_key_ptr As IntPtr = IntPtr.Zero, bob_shared_key_length As Int32 = -1,
          bob_capsulation_ptr As IntPtr = IntPtr.Zero, bob_capsulation_length As Int32 = -1,
          alice_shared_key_ptr As IntPtr = IntPtr.Zero, alice_shared_length As Int32 = -1

        'Bob uses it to generate his SharedKey and Capsulation.
        err = ToPqcMlKemCapsulationFromPubKeyAot(
          alicepubkey, alice_pub_key_length, mlkem_param,
          bob_shared_key_ptr, bob_shared_key_length,
          bob_capsulation_ptr, bob_capsulation_length)
        AssertError(err)

        Dim bobsharedkey = New UsIPtr(Of Byte)(ToBytes(bob_shared_key_ptr, bob_shared_key_length))
        FreeBuffer(bob_shared_key_ptr)

        Dim bobcapsulation = ToBytes(bob_capsulation_ptr, bob_capsulation_length)
        FreeBuffer(bob_capsulation_ptr)

        sw.Start()

        'Sofern Alice nur den 'SharedKey' wünscht, zb. für ein eigenes
        'Verkryptungsalgorithmus, so lässt sich das problemlos über
        'den 'PrivateKey' von Alice generieren.

        'If Alice only wants the 'SharedKey' — for example,
        'for her own encryption algorithm — it can be easily
        'generated using Alice's 'PrivateKey'.

        err = ToPqcMlKemSharedKeyFromPrivateKeyAot(
          aliceprivkey.ToBytes(), aliceprivkey.Length,
          bobcapsulation, bobcapsulation.Length,
          mlkem_param,
          alice_shared_key_ptr, alice_shared_length)
        AssertError(err)

        Dim alicesharedkey = New UsIPtr(Of Byte)(ToBytes(alice_shared_key_ptr, alice_shared_length))
        FreeBuffer(alice_shared_key_ptr)

        If Not bobsharedkey.Equality(alicesharedkey) Then
          Throw New Exception()
        End If

        sw.[Stop]()

        If i Mod rounds / 10 = 0 Then Console.Write(".")
      Next

      sw.[Stop]()

      Dim t = sw.ElapsedMilliseconds
      Console.Write($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms")
      Console.WriteLine()
    End Sub

    Public Shared Sub TestPqcMlKemEnDecryptionBytes(rounds As Int32)
      Console.Write($"{NameOf(TestPqcMlKemEnDecryptionBytes)}Aot: ")

      '1. Alice wants to send Bob a message. Alice generates a random MlKem-KeyPair.
      '2. Alice sends Bob her PublicKey, the MlKem-Param and the Crypto-Algo.
      '3. Bob derives a SharedKey and the Capsulation-Cipher from this.
      '4. Alice receives the Capsulation-Cipher from Bob and can derive the SharedKey
      '   using her PrivateKey. Using a random or existing 'Associated', she encrypts
      '   the message using here AEAD and sends it to Bob.
      '5. So Bob receives the CipherText and the 'Associated' from Alice.
      '   Using his SharedKey, he can now decrypt (AEAD) the CipherText and recover
      '   the original PlainText.


      Dim sw = New Stopwatch()
      For i = 0 To rounds - 1

        Dim alice_priv_key_ptr As IntPtr = IntPtr.Zero, alice_priv_key_length As Int32 = -1,
          alice_pub_key_ptr As IntPtr = IntPtr.Zero, alice_pub_key_length As Int32 = -1,
          alice_guid_id_ptr As IntPtr = IntPtr.Zero, alice_guid_id_length As Int32 = -1,
          mlkem_param As Byte = 0, crypto_algo As Byte = 0

        'Alice generates a new KeyPair
        Dim err = CreateMlKemKeyPairAot(alice_priv_key_ptr, alice_priv_key_length, alice_pub_key_ptr, alice_pub_key_length, alice_guid_id_ptr, alice_guid_id_length, mlkem_param, crypto_algo)
        AssertError(err)

        Dim aliceprivkey = New UsIPtr(Of Byte)(ToBytes(alice_priv_key_ptr, alice_priv_key_length))
        FreeBuffer(alice_priv_key_ptr)

        Dim alicepubkey = ToBytes(alice_pub_key_ptr, alice_pub_key_length)
        FreeBuffer(alice_pub_key_ptr)

        Dim aliceguid = New Guid(ToBytes(alice_guid_id_ptr, alice_guid_id_length))
        FreeBuffer(alice_guid_id_ptr)

        Dim cryptoalgo = CType(crypto_algo, CryptionAlgorithm)
        Dim param = CryptoPqcMlKemUtilsTest.ToMLKemAlgorithm(CType(mlkem_param, MLKemParam))

        Dim bob_shared_key_ptr As IntPtr = IntPtr.Zero, bob_shared_key_length As Int32 = -1,
          bob_capsulation_ptr As IntPtr = IntPtr.Zero, bob_capsulation_length As Int32 = -1,
          cipher_ptr As IntPtr = IntPtr.Zero, cipher_length As Int32 = -1,
          decipher_ptr As IntPtr = IntPtr.Zero, decipher_length As Int32 = -1

        'Bob receives Alice's PublicKey, the MLKemAlgorithm,
        'and the CryptionAlgorithm from Alice. 
        'Bob uses it to generate his SharedKey, Capsulation.
        err = ToPqcMlKemCapsulationFromPubKeyAot(
          alicepubkey, alice_pub_key_length, mlkem_param,
          bob_shared_key_ptr, bob_shared_key_length,
          bob_capsulation_ptr, bob_capsulation_length)
        AssertError(err)

        Dim bobsharedkey = New UsIPtr(Of Byte)(ToBytes(bob_shared_key_ptr, bob_shared_key_length))
        FreeBuffer(bob_shared_key_ptr)

        Dim bobcapsulation = ToBytes(bob_capsulation_ptr, bob_capsulation_length)
        FreeBuffer(bob_capsulation_ptr)

        sw.Start()

        Dim size = RandomNumberGenerator.GetInt32(10, 128)
        Dim message = RngBytes(size)

        size = RandomNumberGenerator.GetInt32(10, 64)
        Dim associated = If(Int32.IsEvenInteger(RandomNumberGenerator.GetInt32(0, 1024)),
          Encoding.UTF8.GetBytes("© Michele Natale 2026"), RngBytes(size))

        'Alice encrypts the message using the Capsulation she received from Bob,
        'her PrivateKey, and a pre-selected associated. The MLKemAlgorithm and the
        'CryptionAlgorithm have already been specified.
        err = PqcMlKemEncryptionAot(
          message, message.Length,
          aliceprivkey.ToBytes(), aliceprivkey.Length,
          bobcapsulation, bobcapsulation.Length,
          associated, associated.Length,
          mlkem_param, crypto_algo,
          cipher_ptr, cipher_length)
        AssertError(err)

        Dim cipher = ToBytes(cipher_ptr, cipher_length)
        FreeBuffer(cipher_ptr)

        'Bob receives the CipherText and the associated from Alice.
        'Bob can now decrypt the CipherText using his SharedKey and
        'obtain the original PlainText.
        err = PqcMlKemDecryptionAot(
          cipher, cipher.Length,
          bobsharedkey.ToBytes(), bobsharedkey.Length,
          associated, associated.Length,
          mlkem_param, crypto_algo,
          decipher_ptr, decipher_length)
        AssertError(err)

        Dim decipher = ToBytes(decipher_ptr, decipher_length)
        FreeBuffer(decipher_ptr)

        If Not message.SequenceEqual(decipher) Then
          Throw New Exception()
        End If

        sw.[Stop]()

        If i Mod rounds / 10 = 0 Then Console.Write(".")
      Next

      sw.[Stop]()

      Dim t = sw.ElapsedMilliseconds
      Console.Write($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms")
      Console.WriteLine()

    End Sub

    Public Shared Sub TestPqcMlKemEnDecryptionBytesStress()
      Console.Write($"{NameOf(TestPqcMlKemEnDecryptionBytesStress)}Aot: ")

      '1. Alice wants to send Bob a message. Alice generates a random MlKem-KeyPair.
      '2. Alice sends Bob her PublicKey, the MlKem-Param and the Crypto-Algo.
      '3. Bob derives a SharedKey and the Capsulation-Cipher from this.
      '4. Alice receives the Capsulation-Cipher from Bob and can derive the SharedKey
      '   using her PrivateKey. Using a random or existing 'Associated', she encrypts
      '   the message using here AEAD and sends it to Bob.
      '5. So Bob receives the CipherText and the 'Associated' from Alice.
      '   Using his SharedKey, he can now decrypt (AEAD) the CipherText and recover
      '   the original PlainText.

      Dim alice_priv_key_ptr As IntPtr = IntPtr.Zero, alice_priv_key_length As Int32 = -1,
        alice_pub_key_ptr As IntPtr = IntPtr.Zero, alice_pub_key_length As Int32 = -1,
        alice_guid_id_ptr As IntPtr = IntPtr.Zero, alice_guid_id_length As Int32 = -1,
        mlkem_param As Byte = 0, crypto_algo As Byte = 0

      Dim sw = Stopwatch.StartNew()

      ' Alice generates a new KeyPair
      Dim err = CreateMlKemKeyPairAot(
        alice_priv_key_ptr, alice_priv_key_length,
        alice_pub_key_ptr, alice_pub_key_length,
        alice_guid_id_ptr, alice_guid_id_length,
        mlkem_param, crypto_algo)
      AssertError(err)

      Dim aliceprivkey = ToBytes(alice_priv_key_ptr, alice_priv_key_length)
      FreeBuffer(alice_priv_key_ptr)

      Dim alicepubkey = ToBytes(alice_pub_key_ptr, alice_pub_key_length)
      FreeBuffer(alice_pub_key_ptr)

      Dim aliceguid = New Guid(ToBytes(alice_guid_id_ptr, alice_guid_id_length))
      FreeBuffer(alice_guid_id_ptr)

      Dim cryptoalgo = CType(crypto_algo, CryptionAlgorithm)
      Dim param = CryptoPqcMlKemUtilsTest.ToMLKemAlgorithm(CType(mlkem_param, MLKemParam))

      Dim bob_shared_key_ptr As IntPtr = IntPtr.Zero, bob_shared_key_length As Int32 = -1,
        bob_capsulation_ptr As IntPtr = IntPtr.Zero, bob_capsulation_length As Int32 = -1

      'Bob receives Alice's PublicKey, the MLKemAlgorithm,
      'and the CryptionAlgorithm from Alice. 
      'Bob uses it to generate his SharedKey, Capsulation.
      err = ToPqcMlKemCapsulationFromPubKeyAot(
        alicepubkey, alice_pub_key_length, mlkem_param,
        bob_shared_key_ptr, bob_shared_key_length,
        bob_capsulation_ptr, bob_capsulation_length)
      AssertError(err)

      Dim bobsharedkey = ToBytes(bob_shared_key_ptr, bob_shared_key_length)
      FreeBuffer(bob_shared_key_ptr)

      Dim bobcapsulation = ToBytes(bob_capsulation_ptr, bob_capsulation_length)
      FreeBuffer(bob_capsulation_ptr)

      Dim msize = 1 << 20 + 1024
      Dim message = RngBytes(msize)

      Dim size = RandomNumberGenerator.GetInt32(10, 64)
      Dim associated = If(Int32.IsEvenInteger(RandomNumberGenerator.GetInt32(0, 1024)),
          Encoding.UTF8.GetBytes("© Michele Natale 2026"), RngBytes(size))
      Dim cipher_ptr As IntPtr = Nothing, cipher_length As Int32 = Nothing

      'Alice encrypts the message using the Capsulation she received from Bob,
      'her PrivateKey, and a pre-selected associated. The MLKemAlgorithm and the
      'CryptionAlgorithm have already been specified.
      err = PqcMlKemEncryptionAot(
        message, message.Length,
        aliceprivkey, aliceprivkey.Length,
        bobcapsulation, bobcapsulation.Length,
        associated, associated.Length,
        mlkem_param, crypto_algo,
        cipher_ptr, cipher_length)
      AssertError(err)

      Dim cipher = ToBytes(cipher_ptr, cipher_length)
      FreeBuffer(cipher_ptr)
      Dim decipher_ptr As IntPtr = Nothing, decipher_length As Int32 = Nothing

      'Bob receives the CipherText and the associated from Alice.
      'Bob can now decrypt the CipherText using his SharedKey and
      'obtain the original PlainText.
      err = PqcMlKemDecryptionAot(
        cipher, cipher.Length,
        bobsharedkey, bobsharedkey.Length,
        associated, associated.Length,
        mlkem_param, crypto_algo,
        decipher_ptr, decipher_length)
      AssertError(err)

      Dim decipher = ToBytes(decipher_ptr, decipher_length)
      FreeBuffer(decipher_ptr)

      If Not message.SequenceEqual(decipher) Then
        Throw New Exception()
      End If

      sw.[Stop]()

      Dim t = sw.ElapsedMilliseconds
      Console.Write($" cryptoalgo = {cryptoalgo}; mlkemparam = {param}; size = {msize}; t = {t}ms")
      Console.WriteLine()

    End Sub



    Private Shared Function IsNullOrEmpty(Of T As INumber(Of T))(bytes As T()) As Boolean
      Return bytes Is Nothing OrElse bytes.Length = 0
    End Function

    'Private Shared Function IsNullOrEmpty(Of T As INumber(Of T))(bytes As ReadOnlyMemory(Of T)) As Boolean
    '  Return bytes.IsEmpty OrElse bytes.Length = 0
    'End Function
  End Class
End Namespace
