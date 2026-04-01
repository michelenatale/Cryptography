
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace michele.natale.Tests;

using Pointers;
using static CryptoTestUtils;

partial class CryptoPqcMlKemTest
{
  public static void TestPqcMlKemEnDecryptionFile(int rounds)
  {

    Console.Write($"{nameof(TestPqcMlKemEnDecryptionFile)}Aot: ");

    //1. Alice wants to send Bob a message. Alice generates a random MlKem-KeyPair.
    //2. Alice sends Bob her PublicKey, the MlKem-Param and the Crypto-Algo.
    //3. Bob derives a SharedKey and the Capsulation-Cipher from this.
    //4. Alice receives the Capsulation-Cipher from Bob and can derive the SharedKey
    //   using her PrivateKey. Using a random or existing 'Associated', she encrypts
    //   the message using here AEAD and sends it to Bob.
    //5. So Bob receives the CipherText and the 'Associated' from Alice.
    //   Using his SharedKey, he can now decrypt (AEAD) the CipherText and recover
    //   the original PlainText.

    ReadOnlySpan<byte> src = "data"u8, dest = "cipher"u8, srcr = "datar"u8;
    var ssrc = Encoding.UTF8.GetString(src);

    var sw = Stopwatch.StartNew();
    for (var i = 0; i < rounds; i++)
    {

      var max = (1 << 21) + 1024;
      var flength = Random.Shared.Next(max);
      SetRngFileData(ssrc, flength);

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

      var size = RandomNumberGenerator.GetInt32(10, 64);
      var associated = int.IsEvenInteger(RandomNumberGenerator.GetInt32(0, 1024))
        ? "© Michele Natale 2026"u8 : RngBytes(size);

      //Alice encrypts the message using the Capsulation she received from Bob,
      //her PrivateKey, and a pre-selected associated. The MLKemAlgorithm and the
      //CryptionAlgorithm have already been specified.
      err = Native.PqcMlKemEncryptionFileAot(
        src, src.Length, dest, dest.Length,
        aliceprivkey.ToBytes(), aliceprivkey.Length,
        bobcapsulation, bobcapsulation.Length,
        associated, associated.Length,
        mlkem_param, crypto_algo);
      AssertError(err);


      //Bob receives the CipherText and the associated from Alice.
      //Bob can now decrypt the CipherText using his SharedKey and
      //obtain the original PlainText.
      err = Native.PqcMlKemDecryptionFileAot(
        dest, dest.Length, srcr, srcr.Length,
        bobsharedkey.ToBytes(), bobsharedkey.Length,
        associated, associated.Length);
      AssertError(err);

      if (!NetServicesUtils.FileEquals(src, srcr))
        throw new Exception();

      if (i % (rounds / 10) == 0)
        Console.Write(".");
    }

    sw.Stop();

    var t = sw.ElapsedMilliseconds;
    Console.WriteLine($" rounds = {rounds}, t = {t}ms, td = {t / rounds}ms");
  }


  public static void TestPqcMlKemEnDecryptionKpfFile(int rounds)
  {

    Console.Write($"{nameof(TestPqcMlKemEnDecryptionKpfFile)}Aot: ");

    //1. Alice wants to send Bob a message. Alice uses her KeyPair for this.
    //2. Alice sends Bob her PublicKey, the MlKem-Param and the Crypto-Algo.
    //3. Bob derives a SharedKey and the Capsulation-Cipher from this.
    //4. Alice receives the Capsulation-Cipher from Bob and can derive the SharedKey
    //   using her PrivateKey. Using a random or existing 'Associated', she encrypts
    //   the message using here AEAD and sends it to Bob.
    //5. So Bob receives the CipherText and the 'Associated' from Alice.
    //   Using his SharedKey, he can now decrypt (AEAD) the CipherText and recover
    //   the original PlainText.

    var alice_kpfile = "alice_mlkem_keypair.key"u8;

    CreateSaveKeyPair(alice_kpfile);

    ReadOnlySpan<byte> src = "data"u8, dest = "cipher"u8, srcr = "datar"u8;
    var ssrc = Encoding.UTF8.GetString(src);

    var sw = Stopwatch.StartNew();
    for (var i = 0; i < rounds; i++)
    {

      var max = (1 << 21) + 1024;
      var flength = Random.Shared.Next(max);
      SetRngFileData(ssrc, flength);

      //Alice uses her KeyPair for this.
      var alice = LoadKeyPair(alice_kpfile);
      var (apk, alicepubkey, guid, mlkem_param, cryptoalgo) = alice;

      using var aliceprivkey = apk;
      var param = FromMLKemAlgorithm(mlkem_param);

      var size = RandomNumberGenerator.GetInt32(10, 64);
      var associated = int.IsEvenInteger(RandomNumberGenerator.GetInt32(0, 1024))
        ? "© Michele Natale 2026"u8 : RngBytes(size);

      //Bob receives Alice's PublicKey, the MLKemAlgorithm,
      //and the CryptionAlgorithm from Alice. 
      //Bob uses it to generate his SharedKey, Capsulation.
      var err = Native.ToPqcMlKemCapsulationFromPubKeyAot(
        alicepubkey, alicepubkey.Length, (byte)param,
        out IntPtr bob_shared_key_ptr, out int bob_shared_key_length,
        out IntPtr bob_capsulation_ptr, out int bob_capsulation_length);
      AssertError(err);

      var bobsharedkey = new UsIPtr<byte>(ToBytes(bob_shared_key_ptr, bob_shared_key_length));
      Native.FreeBuffer(bob_shared_key_ptr);

      var bobcapsulation = ToBytes(bob_capsulation_ptr, bob_capsulation_length);
      Native.FreeBuffer(bob_capsulation_ptr);

      //Alice encrypts the message using the Capsulation she received from Bob,
      //her PrivateKey, and a pre-selected associated. The MLKemAlgorithm and the
      //CryptionAlgorithm have already been specified.
      err = Native.PqcMlKemEncryptionFileAot(
        src, src.Length, dest, dest.Length,
        aliceprivkey.ToBytes(), aliceprivkey.Length,
        bobcapsulation, bobcapsulation.Length,
        associated, associated.Length,
        (byte)param, (byte)cryptoalgo);
      AssertError(err);


      //Bob receives the CipherText and the associated from Alice.
      //Bob can now decrypt the CipherText using his SharedKey and
      //obtain the original PlainText.
      err = Native.PqcMlKemDecryptionFileAot(
        dest, dest.Length, srcr, srcr.Length,
        bobsharedkey.ToBytes(), bobsharedkey.Length,
        associated, associated.Length);
      AssertError(err);

      if (!NetServicesUtils.FileEquals(src, srcr))
        throw new Exception();

      if (i % (rounds / 10) == 0)
        Console.Write(".");
    }

    sw.Stop();

    var t = sw.ElapsedMilliseconds;
    Console.WriteLine($" rounds = {rounds}, t = {t}ms, td = {t / rounds}ms");
  }


  private static void CreateSaveKeyPair(
    ReadOnlySpan<byte> kpfile, bool with_priv_key = true)
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

    //var guid = new Guid(guid_bytes);
    //var cryptoalgo = (CryptionAlgorithm)crypto_algo;
    //var param = ToMLKemAlgorithm((MLKemParam)mlkem_param);

    //WICHTIG: Wenn 'with_priv_key = false', dann wird der
    //         PrivateKey nicht abgespeichert.

    //IMPORTANT: If 'with_priv_key = false', the private
    //           key will not be saved.
    err = Native.SavePqcMlKemKeyPairAot(
      kpfile, kpfile.Length,
      privkey.ToBytes(), privkey.Length,
      pubkey, pubkey.Length,
      guid_bytes, guid_bytes.Length,
      mlkem_param, crypto_algo, with_priv_key);
    AssertError(err);
  }

  private static (UsIPtr<byte> PrivKey, byte[] PubKey, Guid Guid, MLKemAlgorithm Param, CryptionAlgorithm CryptoAlgo) LoadKeyPair(ReadOnlySpan<byte> kpfile)
  {
    var err = Native.LoadPqcMlKemKeyPairAot(
        kpfile, kpfile.Length,
        out IntPtr priv_key_ptr2, out int priv_key_length2,
        out IntPtr pub_key_ptr2, out int pub_key_length2,
        out IntPtr guid_id_ptr2, out int guid_id_length2,
        out byte mlkem_param2, out byte crypto_algo2);
    AssertError(err);

    var privkey = new UsIPtr<byte>(ToBytes(priv_key_ptr2, priv_key_length2));
    Native.FreeBuffer(priv_key_ptr2);

    var pubkey = ToBytes(pub_key_ptr2, pub_key_length2);
    Native.FreeBuffer(pub_key_ptr2);

    var guid_bytes = ToBytes(guid_id_ptr2, guid_id_length2);
    Native.FreeBuffer(guid_id_ptr2);

    var guid = new Guid(guid_bytes);
    var cryptoalgo = (CryptionAlgorithm)crypto_algo2;
    var param = ToMLKemAlgorithm((MLKemParam)mlkem_param2);

    return (privkey, pubkey, guid, param, cryptoalgo);
  }
}
