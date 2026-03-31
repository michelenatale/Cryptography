

using System.Diagnostics;
using System.Numerics;
using System.Security.Cryptography;


//Start
//Alice(Keypair)
//    >> Bob(Encapsulation)
//        >> Alice(Encryption)
//            >> Bob(Decryption)
//Finish


namespace michele.natale.Tests;

using Pointers;
using static CryptoTestUtils;

partial class CryptoPqcMlKemTest
{
  public static void TestPqcMlKemCreateKeyPairs(int rounds)
  {
    Console.Write($"{nameof(TestPqcMlKemCreateKeyPairs)}Aot: ");

    var rand = Random.Shared;
    var sw = Stopwatch.StartNew();
    for (int i = 0; i < rounds; i++)
    {
      var err = Native.CreateMlKemKeyPairAot(
        out IntPtr priv_key_ptr, out int priv_key_length,
        out IntPtr pub_key_ptr, out int pub_key_length,
        out IntPtr guid_id_ptr, out int guid_id_length,
        out byte mlkem_param, out byte crypto_algo);
      AssertError(err);

      var privkey = new UsIPtr<byte>(ToBytes(priv_key_ptr, priv_key_length));
      Native.FreeBuffer(priv_key_ptr);

      var pubkey = ToBytes(pub_key_ptr, pub_key_length);
      Native.FreeBuffer(pub_key_ptr);

      var guid = new Guid(ToBytes(guid_id_ptr, guid_id_length));
      Native.FreeBuffer(guid_id_ptr);

      var cryptoalgo = (CryptionAlgorithm)crypto_algo;
      var param = ToMLKemAlgorithm((MLKemParam)mlkem_param);

      if (i % (rounds / 10) == 0)
        Console.Write(".");
    }

    sw.Stop();

    var t = sw.ElapsedMilliseconds;
    Console.Write($" rounds = {rounds}; t = {t}ms; td = {t / (double)rounds}ms");
    Console.WriteLine();
  }

  public static void TestPqcMlKemCreateKeyPairsParam(int rounds)
  {
    Console.Write($"{nameof(TestPqcMlKemCreateKeyPairsParam)}Aot: ");

    var rand = Random.Shared;
    var sw = Stopwatch.StartNew();
    for (int i = 0; i < rounds; i++)
    {
      var mlkem_param = ToMLKemAlgorithm();

      //ML-KEM Parameter select. Only the first 3 algos
      var idx = RandomNumberGenerator.GetInt32(mlkem_param.Length);
      var param = (byte)FromMLKemAlgorithm(mlkem_param[idx]);

      //Symmetric CryptionAlgorithm select
      var cas = Enum.GetValues<CryptionAlgorithm>();
      idx = RandomNumberGenerator.GetInt32(cas.Length);
      var cryptoalgo = (byte)cas[idx];

      var err = Native.CreateMlKemKeyPairParamAot(
        param, cryptoalgo,
        out IntPtr priv_key_ptr, out int priv_key_length,
        out IntPtr pub_key_ptr, out int pub_key_length,
        out IntPtr guid_id_ptr, out int guid_id_length);
      AssertError(err);

      var privkey = new UsIPtr<byte>(ToBytes(priv_key_ptr, priv_key_length));
      Native.FreeBuffer(priv_key_ptr);

      var pubkey = ToBytes(pub_key_ptr, pub_key_length);
      Native.FreeBuffer(pub_key_ptr);

      var guid = new Guid(ToBytes(guid_id_ptr, guid_id_length));
      Native.FreeBuffer(guid_id_ptr);

      if (i % (rounds / 10) == 0)
        Console.Write(".");
    }

    sw.Stop();

    var t = sw.ElapsedMilliseconds;
    Console.Write($" rounds = {rounds}; t = {t}ms; td = {t / (double)rounds}ms");
    Console.WriteLine();
  }

  public static void TestPqcMlKemSafeLoadKeyPairs(int rounds)
  {
    Console.Write($"{nameof(TestPqcMlKemSafeLoadKeyPairs)}Aot: ");

    var rand = Random.Shared;
    var sw = Stopwatch.StartNew();
    for (int i = 0; i < rounds; i++)
    {
      var err = Native.CreateMlKemKeyPairAot(
        out IntPtr priv_key_ptr, out int priv_key_length,
        out IntPtr pub_key_ptr, out int pub_key_length,
        out IntPtr guid_id_ptr, out int guid_id_length,
        out byte mlkem_param, out byte crypto_algo);
      AssertError(err);

      var privkey = new UsIPtr<byte>(ToBytes(priv_key_ptr, priv_key_length));
      Native.FreeBuffer(priv_key_ptr);

      var pubkey = ToBytes(pub_key_ptr, pub_key_length);
      Native.FreeBuffer(pub_key_ptr);

      var guid_bytes = ToBytes(guid_id_ptr, guid_id_length);
      Native.FreeBuffer(guid_id_ptr);

      var guid = new Guid(guid_bytes);
      var cryptoalgo = (CryptionAlgorithm)crypto_algo;
      var param = ToMLKemAlgorithm((MLKemParam)mlkem_param);

      //WICHTIG: Wenn 'with_priv_key = false', dann wird der
      //         PrivateKey nicht abgespeichert.

      //IMPORTANT: If 'with_priv_key = false', the private
      //           key will not be saved.
      var with_priv_key = int.IsEvenInteger(rand.Next());

      var kpfile = "mlkem_keypair.key"u8;
      err = Native.SavePqcMlKemKeyPairAot(
        kpfile, kpfile.Length,
        privkey.ToBytes(), privkey.Length,
        pubkey, pubkey.Length,
        guid_bytes, guid_bytes.Length,
        mlkem_param, crypto_algo, with_priv_key);
      AssertError(err);

      err = Native.LoadPqcMlKemKeyPairAot(
        kpfile, kpfile.Length,
        out IntPtr priv_key_ptr2, out int priv_key_length2,
        out IntPtr pub_key_ptr2, out int pub_key_length2,
        out IntPtr guid_id_ptr2, out int guid_id_length2,
        out byte mlkem_param2, out byte crypto_algo2);
      AssertError(err);

      var privkey2 = new UsIPtr<byte>(ToBytes(priv_key_ptr2, priv_key_length2));
      Native.FreeBuffer(priv_key_ptr2);

      var pubkey2 = ToBytes(pub_key_ptr2, pub_key_length2);
      Native.FreeBuffer(pub_key_ptr2);

      var guid_bytes2 = ToBytes(guid_id_ptr2, guid_id_length2);
      Native.FreeBuffer(guid_id_ptr2);

      var guid2 = new Guid(guid_bytes2);
      var cryptoalgo2 = (CryptionAlgorithm)crypto_algo2;
      var param2 = ToMLKemAlgorithm((MLKemParam)mlkem_param2);

      if (with_priv_key && !privkey.Equality(privkey2))
        throw new Exception();
      else if (!with_priv_key && privkey2.Length != 0)
        throw new Exception();

      if (!pubkey.SequenceEqual(pubkey2))
        throw new Exception();

      if (!guid_bytes.SequenceEqual(guid_bytes2))
        throw new Exception();

      if (guid != guid2)
        throw new Exception();

      if (cryptoalgo != cryptoalgo2)
        throw new Exception();

      if (param != param2)
        throw new Exception();

      if (i % (rounds / 10) == 0)
        Console.Write(".");
    }

    sw.Stop();

    var t = sw.ElapsedMilliseconds;
    Console.Write($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms");
    Console.WriteLine();
  }

  public static void TestPqcMlKemCapsulationSharedKeyWithPublicKey(int rounds)
  {
    //Bob bekommt von Alice ihren 'Publickey' sowie den
    //'MlKem Parameter' und lässt sich so seinen 'SharedKey'
    //und den 'Capsulation' generieren.

    //Bob receives Alice's ''Publickey' and
    //'MlKem Parameter' and uses them to generate
    //his 'SharedKey' and the 'Capsulation'.

    Console.Write($"{nameof(TestPqcMlKemCapsulationSharedKeyWithPublicKey)}Aot: ");

    var rand = Random.Shared;
    var sw = Stopwatch.StartNew();
    for (int i = 0; i < rounds; i++)
    {

      //Alice generates a new KeyPair
      var err = Native.CreateMlKemKeyPairAot(
        out IntPtr alice_priv_key_ptr, out int alice_priv_key_length,
        out IntPtr alice_pub_key_ptr, out int alice_pub_key_length,
        out IntPtr alice_guid_id_ptr, out int alice_guid_id_length,
        out byte mlkem_param, out byte crypto_algo);
      AssertError(err);

      var aliceprivkey = new UsIPtr<byte>(ToBytes(alice_priv_key_ptr, alice_priv_key_length));
      Native.FreeBuffer(alice_priv_key_ptr);

      var alicepubkey = ToBytes(alice_pub_key_ptr, alice_pub_key_length);
      Native.FreeBuffer(alice_pub_key_ptr);

      var aliceguid = new Guid(ToBytes(alice_guid_id_ptr, alice_guid_id_length));
      Native.FreeBuffer(alice_guid_id_ptr);

      var cryptoalgo = (CryptionAlgorithm)crypto_algo;
      var param = ToMLKemAlgorithm((MLKemParam)mlkem_param);

      //Bob uses it to generate his SharedKey and Capsulation.
      err = Native.ToPqcMlKemCapsulationFromPubKeyAot(
        alicepubkey, alice_pub_key_length, mlkem_param,
        out IntPtr bob_shared_key_ptr, out int bob_shared_key_length,
        out IntPtr bob_capsulation_ptr, out int bob_capsulation_length);
      AssertError(err);

      var bobsharedkey = new UsIPtr<byte>(ToBytes(bob_shared_key_ptr, bob_shared_key_length));
      Native.FreeBuffer(bob_shared_key_ptr);

      var bobcapsulation = ToBytes(bob_capsulation_ptr, bob_capsulation_length);
      Native.FreeBuffer(bob_capsulation_ptr);

      if (bobsharedkey.IsEmpty)
        throw new Exception();

      if (IsNullOrEmpty(bobcapsulation))
        throw new Exception();

      if (i % (rounds / 10) == 0)
        Console.Write(".");
    }

    sw.Stop();

    var t = sw.ElapsedMilliseconds;
    Console.Write($" rounds = {rounds}; t = {t}ms; td = {t / (double)rounds}ms");
    Console.WriteLine();
  }


  public static void TestPqcMlKemSharedKeyFromCapsualtionPrivateKey(int rounds)
  {
    Console.Write($"{nameof(TestPqcMlKemSharedKeyFromCapsualtionPrivateKey)}Aot: ");

    var sw = new Stopwatch();
    var rand = Random.Shared;
    for (int i = 0; i < rounds; i++)
    {
      //Alice generates a new KeyPair
      var err = Native.CreateMlKemKeyPairAot(
        out IntPtr alice_priv_key_ptr, out int alice_priv_key_length,
        out IntPtr alice_pub_key_ptr, out int alice_pub_key_length,
        out IntPtr alice_guid_id_ptr, out int alice_guid_id_length,
        out byte mlkem_param, out byte crypto_algo);
      AssertError(err);

      var aliceprivkey = new UsIPtr<byte>(ToBytes(alice_priv_key_ptr, alice_priv_key_length));
      Native.FreeBuffer(alice_priv_key_ptr);

      var alicepubkey = ToBytes(alice_pub_key_ptr, alice_pub_key_length);
      Native.FreeBuffer(alice_pub_key_ptr);

      var aliceguid = new Guid(ToBytes(alice_guid_id_ptr, alice_guid_id_length));
      Native.FreeBuffer(alice_guid_id_ptr);

      var cryptoalgo = (CryptionAlgorithm)crypto_algo;
      var param = ToMLKemAlgorithm((MLKemParam)mlkem_param);

      //Bob uses it to generate his SharedKey and Capsulation.
      err = Native.ToPqcMlKemCapsulationFromPubKeyAot(
        alicepubkey, alice_pub_key_length, mlkem_param,
        out IntPtr bob_shared_key_ptr, out int bob_shared_key_length,
        out IntPtr bob_capsulation_ptr, out int bob_capsulation_length);
      AssertError(err);

      var bobsharedkey = new UsIPtr<byte>(ToBytes(bob_shared_key_ptr, bob_shared_key_length));
      Native.FreeBuffer(bob_shared_key_ptr);

      var bobcapsulation = ToBytes(bob_capsulation_ptr, bob_capsulation_length);
      Native.FreeBuffer(bob_capsulation_ptr);

      sw.Start();

      //Sofern Alice nur den 'SharedKey' wünscht, zb. für ein eigenes
      //Verkryptungsalgorithmus, so lässt sich das problemlos über
      //den 'PrivateKey' von Alice generieren.

      //If Alice only wants the 'SharedKey' — for example,
      //for her own encryption algorithm — it can be easily
      //generated using Alice's 'PrivateKey'.

      err = Native.ToPqcMlKemSharedKeyFromPrivateKeyAot(
        aliceprivkey.ToBytes(), aliceprivkey.Length,
        bobcapsulation, bobcapsulation.Length,
        mlkem_param,
        out IntPtr alice_shared_key_ptr, out int alice_shared_length);
      AssertError(err);

      var alicesharedkey = new UsIPtr<byte>(ToBytes(alice_shared_key_ptr, alice_shared_length));
      Native.FreeBuffer(alice_shared_key_ptr);

      if (!bobsharedkey.Equality(alicesharedkey))
        throw new Exception();

      sw.Stop();

      if (i % (rounds / 10) == 0)
        Console.Write(".");
    }

    sw.Stop();

    var t = sw.ElapsedMilliseconds;
    Console.Write($" rounds = {rounds}; t = {t}ms; td = {t / (double)rounds}ms");
    Console.WriteLine();
  }

  public static void TestPqcMlKemEnDecryptionBytes(int rounds)
  {
    Console.Write($"{nameof(TestPqcMlKemEnDecryptionBytes)}Aot: ");

    //1. Alice wants to send Bob a message. Alice generates a random MlKem-KeyPair.
    //2. Alice sends Bob her PublicKey, the MlKem-Param and the Crypto-Algo.
    //3. Bob derives a SharedKey and the Capsulation-Cipher from this.
    //4. Alice receives the Capsulation-Cipher from Bob and can derive the SharedKey
    //   using her PrivateKey. Using a random or existing 'Associated', she encrypts
    //   the message using here AEAD and sends it to Bob.
    //5. So Bob receives the CipherText and the 'Associated' from Alice.
    //   Using his SharedKey, he can now decrypt (AEAD) the CipherText and recover
    //   the original PlainText.


    var sw = new Stopwatch();
    for (int i = 0; i < rounds; i++)
    {
      //Alice generates a new KeyPair
      var err = Native.CreateMlKemKeyPairAot(
        out IntPtr alice_priv_key_ptr, out int alice_priv_key_length,
        out IntPtr alice_pub_key_ptr, out int alice_pub_key_length,
        out IntPtr alice_guid_id_ptr, out int alice_guid_id_length,
        out byte mlkem_param, out byte crypto_algo);
      AssertError(err);

      var aliceprivkey = new UsIPtr<byte>(ToBytes(alice_priv_key_ptr, alice_priv_key_length));
      Native.FreeBuffer(alice_priv_key_ptr);

      var alicepubkey = ToBytes(alice_pub_key_ptr, alice_pub_key_length);
      Native.FreeBuffer(alice_pub_key_ptr);

      var aliceguid = new Guid(ToBytes(alice_guid_id_ptr, alice_guid_id_length));
      Native.FreeBuffer(alice_guid_id_ptr);

      var cryptoalgo = (CryptionAlgorithm)crypto_algo;
      var param = ToMLKemAlgorithm((MLKemParam)mlkem_param);

      //Bob receives Alice's PublicKey, the MLKemAlgorithm,
      //and the CryptionAlgorithm from Alice. 
      //Bob uses it to generate his SharedKey, Capsulation.
      err = Native.ToPqcMlKemCapsulationFromPubKeyAot(
        alicepubkey, alice_pub_key_length, mlkem_param,
        out IntPtr bob_shared_key_ptr, out int bob_shared_key_length,
        out IntPtr bob_capsulation_ptr, out int bob_capsulation_length);
      AssertError(err);

      var bobsharedkey = new UsIPtr<byte>(ToBytes(bob_shared_key_ptr, bob_shared_key_length));
      Native.FreeBuffer(bob_shared_key_ptr);

      var bobcapsulation = ToBytes(bob_capsulation_ptr, bob_capsulation_length);
      Native.FreeBuffer(bob_capsulation_ptr);

      sw.Start();

      var size = RandomNumberGenerator.GetInt32(10, 128);
      var message = RngBytes(size);

      size = RandomNumberGenerator.GetInt32(10, 64);
      var associated = int.IsEvenInteger(RandomNumberGenerator.GetInt32(0, 1024))
        ? "© Michele Natale 2026"u8 : RngBytes(size);

      //Alice encrypts the message using the Capsulation she received from Bob,
      //her PrivateKey, and a pre-selected associated. The MLKemAlgorithm and the
      //CryptionAlgorithm have already been specified.
      err = Native.PqcMlKemEncryptionAot(
        message, message.Length,
        aliceprivkey.ToBytes(), aliceprivkey.Length,
        bobcapsulation, bobcapsulation.Length,
        associated, associated.Length,
        mlkem_param, crypto_algo,
        out IntPtr cipher_ptr, out int cipher_length);
      AssertError(err);

      var cipher = ToBytes(cipher_ptr, cipher_length);
      Native.FreeBuffer(cipher_ptr);

      //Bob receives the CipherText and the associated from Alice.
      //Bob can now decrypt the CipherText using his SharedKey and
      //obtain the original PlainText.
      err = Native.PqcMlKemDecryptionAot(
        cipher, cipher.Length,
        bobsharedkey.ToBytes(), bobsharedkey.Length,
        associated, associated.Length,
        mlkem_param, crypto_algo,
        out IntPtr decipher_ptr, out int decipher_length);
      AssertError(err);

      var decipher = ToBytes(decipher_ptr, decipher_length);
      Native.FreeBuffer(decipher_ptr);

      if (!message.SequenceEqual(decipher))
        throw new Exception();

      sw.Stop();

      if (i % (rounds / 10) == 0)
        Console.Write(".");
    }

    sw.Stop();

    var t = sw.ElapsedMilliseconds;
    Console.Write($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms");
    Console.WriteLine();

  }

  public static void TestPqcMlKemEnDecryptionBytesStress()
  {
    Console.Write($"{nameof(TestPqcMlKemEnDecryptionBytesStress)}Aot: ");

    //1. Alice wants to send Bob a message. Alice generates a random MlKem-KeyPair.
    //2. Alice sends Bob her PublicKey, the MlKem-Param and the Crypto-Algo.
    //3. Bob derives a SharedKey and the Capsulation-Cipher from this.
    //4. Alice receives the Capsulation-Cipher from Bob and can derive the SharedKey
    //   using her PrivateKey. Using a random or existing 'Associated', she encrypts
    //   the message using here AEAD and sends it to Bob.
    //5. So Bob receives the CipherText and the 'Associated' from Alice.
    //   Using his SharedKey, he can now decrypt (AEAD) the CipherText and recover
    //   the original PlainText.


    var sw = Stopwatch.StartNew();

    // Alice generates a new KeyPair
    var err = Native.CreateMlKemKeyPairAot(
      out IntPtr alice_priv_key_ptr, out int alice_priv_key_length,
      out IntPtr alice_pub_key_ptr, out int alice_pub_key_length,
      out IntPtr alice_guid_id_ptr, out int alice_guid_id_length,
      out byte mlkem_param, out byte crypto_algo);
    AssertError(err);

    var aliceprivkey = ToBytes(alice_priv_key_ptr, alice_priv_key_length);
    Native.FreeBuffer(alice_priv_key_ptr);

    var alicepubkey = ToBytes(alice_pub_key_ptr, alice_pub_key_length);
    Native.FreeBuffer(alice_pub_key_ptr);

    var aliceguid = new Guid(ToBytes(alice_guid_id_ptr, alice_guid_id_length));
    Native.FreeBuffer(alice_guid_id_ptr);

    var cryptoalgo = (CryptionAlgorithm)crypto_algo;
    var param = ToMLKemAlgorithm((MLKemParam)mlkem_param);

    //Bob receives Alice's PublicKey, the MLKemAlgorithm,
    //and the CryptionAlgorithm from Alice. 
    //Bob uses it to generate his SharedKey, Capsulation.
    err = Native.ToPqcMlKemCapsulationFromPubKeyAot(
      alicepubkey, alice_pub_key_length, mlkem_param,
      out IntPtr bob_shared_key_ptr, out int bob_shared_key_length,
      out IntPtr bob_capsulation_ptr, out int bob_capsulation_length);
    AssertError(err);

    var bobsharedkey = ToBytes(bob_shared_key_ptr, bob_shared_key_length);
    Native.FreeBuffer(bob_shared_key_ptr);

    var bobcapsulation = ToBytes(bob_capsulation_ptr, bob_capsulation_length);
    Native.FreeBuffer(bob_capsulation_ptr);

    var msize = 1 << 20 + 1024;
    var message = RngBytes(msize);

    var size = RandomNumberGenerator.GetInt32(10, 64);
    var associated = int.IsEvenInteger(RandomNumberGenerator.GetInt32(0, 1024))
      ? "© Michele Natale 2026"u8 : RngBytes(size);

    //Alice encrypts the message using the Capsulation she received from Bob,
    //her PrivateKey, and a pre-selected associated. The MLKemAlgorithm and the
    //CryptionAlgorithm have already been specified.
    err = Native.PqcMlKemEncryptionAot(
      message, message.Length,
      aliceprivkey, aliceprivkey.Length,
      bobcapsulation, bobcapsulation.Length,
      associated, associated.Length,
      mlkem_param, crypto_algo,
      out IntPtr cipher_ptr, out int cipher_length);
    AssertError(err);

    var cipher = ToBytes(cipher_ptr, cipher_length);
    Native.FreeBuffer(cipher_ptr);

    //Bob receives the CipherText and the associated from Alice.
    //Bob can now decrypt the CipherText using his SharedKey and
    //obtain the original PlainText.
    err = Native.PqcMlKemDecryptionAot(
      cipher, cipher.Length,
      bobsharedkey, bobsharedkey.Length,
      associated, associated.Length,
      mlkem_param, crypto_algo,
      out IntPtr decipher_ptr, out int decipher_length);
    AssertError(err);

    var decipher = ToBytes(decipher_ptr, decipher_length);
    Native.FreeBuffer(decipher_ptr);

    if (!message.SequenceEqual(decipher))
      throw new Exception();


    sw.Stop();

    var t = sw.ElapsedMilliseconds;
    Console.Write($" cryptoalgo = {cryptoalgo}; mlkemparam = {param}; size = {msize}; t = {t}ms");
    Console.WriteLine();

  }



  private static bool IsNullOrEmpty<T>(ReadOnlySpan<T> bytes)
  where T : INumber<T> =>
    bytes.IsEmpty || bytes.Length == 0;

  private static bool IsNullOrEmpty<T>(ReadOnlyMemory<T> bytes)
  where T : INumber<T> =>
    bytes.IsEmpty || bytes.Length == 0;

}
