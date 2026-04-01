Option Strict On
Option Explicit On


Imports System.Text
Imports michele.natale
Imports michele.natale.Pointers
Imports System.Security.Cryptography

Namespace michele.natale.Tests
  Partial Class CryptoPqcMlKemTest
    Public Shared Sub TestPqcMlKemEnDecryptionFile(rounds As Int32)

      Console.Write($"{NameOf(TestPqcMlKemEnDecryptionFile)}Aot: ")

      '1. Alice wants to send Bob a message. Alice generates a random MlKem-KeyPair.
      '2. Alice sends Bob her PublicKey, the MlKem-Param and the Crypto-Algo.
      '3. Bob derives a SharedKey and the Capsulation-Cipher from this.
      '4. Alice receives the Capsulation-Cipher from Bob and can derive the SharedKey
      '   using her PrivateKey. Using a random or existing 'Associated', she encrypts
      '   the message using here AEAD and sends it to Bob.
      '5. So Bob receives the CipherText and the 'Associated' from Alice.
      '   Using his SharedKey, he can now decrypt (AEAD) the CipherText and recover
      '   the original PlainText.

      Dim src = Encoding.UTF8.GetBytes("data"),
        dest = Encoding.UTF8.GetBytes("cipher"),
        srcr = Encoding.UTF8.GetBytes("datar")
      Dim ssrc = Encoding.UTF8.GetString(src)

      Dim sw = Stopwatch.StartNew()
      For i = 0 To rounds - 1

        Dim max = (1 << 21) + 1024
        Dim flength = Random.[Shared].[Next](max)
        SetRngFileData(ssrc, flength)

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

        Dim size = RandomNumberGenerator.GetInt32(10, 64)
        Dim associated = If(Int32.IsEvenInteger(RandomNumberGenerator.GetInt32(0, 1024)),
          Encoding.UTF8.GetBytes("© Michele Natale 2026"), RngBytes(size))

        'Alice encrypts the message using the Capsulation she received from Bob,
        'her PrivateKey, and a pre-selected associated. The MLKemAlgorithm and the
        'CryptionAlgorithm have already been specified.
        err = PqcMlKemEncryptionFileAot(src, src.Length, dest, dest.Length, aliceprivkey.ToBytes(), aliceprivkey.Length, bobcapsulation, bobcapsulation.Length, associated, associated.Length, mlkem_param, crypto_algo)
        AssertError(err)


        'Bob receives the CipherText and the associated from Alice.
        'Bob can now decrypt the CipherText using his SharedKey and
        'obtain the original PlainText.
        err = PqcMlKemDecryptionFileAot(
          dest, dest.Length, srcr, srcr.Length,
          bobsharedkey.ToBytes(), bobsharedkey.Length,
          associated, associated.Length)
        AssertError(err)

        If Not NetServicesUtils.FileEquals(src, srcr) Then
          Throw New Exception()
        End If

        If i Mod rounds / 10 = 0 Then Console.Write(".")
      Next

      sw.[Stop]()

      Dim t = sw.ElapsedMilliseconds
      Console.WriteLine($" rounds = {rounds}, t = {t}ms, td = {t / rounds}ms")
    End Sub


    Public Shared Sub TestPqcMlKemEnDecryptionKpfFile(rounds As Int32)

      Console.Write($"{NameOf(TestPqcMlKemEnDecryptionKpfFile)}Aot: ")

      '1. Alice wants to send Bob a message. Alice uses her KeyPair for this.
      '2. Alice sends Bob her PublicKey, the MlKem-Param and the Crypto-Algo.
      '3. Bob derives a SharedKey and the Capsulation-Cipher from this.
      '4. Alice receives the Capsulation-Cipher from Bob and can derive the SharedKey
      '   using her PrivateKey. Using a random or existing 'Associated', she encrypts
      '   the message using here AEAD and sends it to Bob.
      '5. So Bob receives the CipherText and the 'Associated' from Alice.
      '   Using his SharedKey, he can now decrypt (AEAD) the CipherText and recover
      '   the original PlainText.

      Dim alice_kpfile = Encoding.UTF8.GetBytes("alice_mlkem_keypair.key")

      CreateSaveKeyPair(alice_kpfile)

      Dim src As Byte() = Encoding.UTF8.GetBytes("data"),
        dest As Byte() = Encoding.UTF8.GetBytes("cipher"),
        srcr As Byte() = Encoding.UTF8.GetBytes("datar")
      Dim ssrc = Encoding.UTF8.GetString(src)

      Dim sw = Stopwatch.StartNew()

      For i = 0 To rounds - 1

        Dim max = (1 << 21) + 1024
        Dim flength = Random.[Shared].[Next](max)
        SetRngFileData(ssrc, flength)

        'Alice uses her KeyPair for this.
        Dim alice = LoadKeyPair(alice_kpfile)

        Dim alicepubkey = alice.PubKey
        Dim aliceprivkey = alice.PrivKey
        Dim cryptoalgo = alice.CryptoAlgo
        Dim param = CryptoPqcMlKemUtilsTest.FromMLKemAlgorithm(alice.Param)

        Dim size = RandomNumberGenerator.GetInt32(10, 64)
          Dim associated = If(Int32.IsEvenInteger(RandomNumberGenerator.GetInt32(0, 1024)),
            Encoding.UTF8.GetBytes("© Michele Natale 2026"), RngBytes(size))

        Dim bob_shared_key_ptr As IntPtr = IntPtr.Zero, bob_shared_key_length As Int32 = -1,
          bob_capsulation_ptr As IntPtr = IntPtr.Zero, bob_capsulation_length As Int32 = -1


        'Bob receives Alice's PublicKey, the MLKemAlgorithm,
        'and the CryptionAlgorithm from Alice. 
        'Bob uses it to generate his SharedKey, Capsulation.
        Dim err = ToPqcMlKemCapsulationFromPubKeyAot(
          alicepubkey, alicepubkey.Length,
          param,
          bob_shared_key_ptr, bob_shared_key_length,
          bob_capsulation_ptr, bob_capsulation_length)
        AssertError(err)

        Dim bobsharedkey = New UsIPtr(Of Byte)(ToBytes(bob_shared_key_ptr, bob_shared_key_length))
        FreeBuffer(bob_shared_key_ptr)

        Dim bobcapsulation = ToBytes(bob_capsulation_ptr, bob_capsulation_length)
        FreeBuffer(bob_capsulation_ptr)

        'Alice encrypts the message using the Capsulation she received from Bob,
        'her PrivateKey, and a pre-selected associated. The MLKemAlgorithm and the
        'CryptionAlgorithm have already been specified.
        err = PqcMlKemEncryptionFileAot(
          src, src.Length, dest, dest.Length,
          aliceprivkey.ToBytes(), aliceprivkey.Length,
          bobcapsulation, bobcapsulation.Length,
          associated, associated.Length,
          param, cryptoalgo)
        AssertError(err)


          'Bob receives the CipherText and the associated from Alice.
          'Bob can now decrypt the CipherText using his SharedKey and
          'obtain the original PlainText.
          err = PqcMlKemDecryptionFileAot(
            dest, dest.Length, srcr, srcr.Length, bobsharedkey.ToBytes(), bobsharedkey.Length, associated, associated.Length)
          AssertError(err)

        If Not NetServicesUtils.FileEquals(src, srcr) Then Throw New Exception()

        If i Mod rounds / 10 = 0 Then
          Console.Write(".")
        End If
      Next

      sw.[Stop]()

      Dim t = sw.ElapsedMilliseconds
      Console.WriteLine($" rounds = {rounds}, t = {t}ms, td = {t / rounds}ms")
    End Sub


    Private Shared Sub CreateSaveKeyPair(
      kpfile As Byte(), Optional with_priv_key As Boolean = True)

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

      'var guid = new Guid(guid_bytes);
      'var cryptoalgo = (CryptionAlgorithm)crypto_algo;
      'var param = ToMLKemAlgorithm((MLKemParam)mlkem_param);

      'WICHTIG: Wenn 'with_priv_key = false', dann wird der
      '         PrivateKey nicht abgespeichert.

      'IMPORTANT: If 'with_priv_key = false', the private
      '           key will not be saved.
      err = SavePqcMlKemKeyPairAot(
        kpfile, kpfile.Length,
        privkey.ToBytes(), privkey.Length,
        pubkey, pubkey.Length,
        guid_bytes, guid_bytes.Length,
        mlkem_param, crypto_algo,
        with_priv_key)
      AssertError(err)
    End Sub

    Private Shared Function LoadKeyPair(kpfile As Byte()) As (PrivKey As UsIPtr(Of Byte), PubKey As Byte(), Guid As Guid, Param As MLKemAlgorithm, CryptoAlgo As CryptionAlgorithm)

      Dim priv_key_ptr2 As IntPtr = IntPtr.Zero, priv_key_length2 As Int32 = -1,
        pub_key_ptr2 As IntPtr = IntPtr.Zero, pub_key_length2 As Int32 = -1,
        guid_id_ptr2 As IntPtr = IntPtr.Zero, guid_id_length2 As Int32 = -1,
        mlkem_param2 As Byte = 0, crypto_algo2 As Byte = 0

      Dim err = LoadPqcMlKemKeyPairAot(
        kpfile, kpfile.Length,
        priv_key_ptr2, priv_key_length2,
        pub_key_ptr2, pub_key_length2,
        guid_id_ptr2, guid_id_length2,
        mlkem_param2, crypto_algo2)
      AssertError(err)

      Dim privkey = New UsIPtr(Of Byte)(ToBytes(priv_key_ptr2, priv_key_length2))
      FreeBuffer(priv_key_ptr2)

      Dim pubkey = ToBytes(pub_key_ptr2, pub_key_length2)
      FreeBuffer(pub_key_ptr2)

      Dim guid_bytes = ToBytes(guid_id_ptr2, guid_id_length2)
      FreeBuffer(guid_id_ptr2)

      Dim guid = New Guid(guid_bytes)
      Dim cryptoalgo = CType(crypto_algo2, CryptionAlgorithm)
      Dim param = CryptoPqcMlKemUtilsTest.ToMLKemAlgorithm(CType(mlkem_param2, MLKemParam))

      Return (privkey, pubkey, guid, param, cryptoalgo)
    End Function

  End Class
End Namespace
