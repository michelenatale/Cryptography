
using System.Text;
using System.Numerics;
using System.Diagnostics;
using System.Security.Cryptography;


namespace michele.natale.Tests;

using Pointers;
using static CryptoTestUtils;


partial class CryptoPqcMlDsaTest 
{
  public static void TestPqcMlDsaCreateKeyPairs(int rounds)
  {
    Console.Write($"{nameof(TestPqcMlDsaCreateKeyPairs)}Aot: ");

    var rand = Random.Shared;
    var sw = Stopwatch.StartNew();
    for (int i = 0; i < rounds; i++)
    {
      var err = Native.CreateMlDsaKeyPairAot(
        out IntPtr priv_key_ptr, out int priv_key_length,
        out IntPtr pub_key_ptr, out int pub_key_length,
        out IntPtr guid_id_ptr, out int guid_id_length,
        out byte mldsa_param);
      AssertError(err);

      var privkey = new UsIPtr<byte>(ToBytes(
        priv_key_ptr, priv_key_length));
      Native.FreeBuffer(priv_key_ptr);

      var pubkey = ToBytes(pub_key_ptr, pub_key_length);
      Native.FreeBuffer(pub_key_ptr);

      var guid = new Guid(ToBytes(guid_id_ptr, guid_id_length));
      Native.FreeBuffer(guid_id_ptr);

      var param = NetServicesCrypto.
        ToMLDsaAlgorithm((MLDsaParam)mldsa_param);

      if (i % (rounds / 10) == 0)
        Console.Write(".");
    }

    sw.Stop();

    var t = sw.ElapsedMilliseconds;
    Console.WriteLine($" rounds = {rounds}; t = {t}ms; td = {t / (double)rounds}ms");
  }

  public static void TestPqcMlDsaCreateKeyPairsParam(int rounds)
  {
    Console.Write($"{nameof(TestPqcMlDsaCreateKeyPairsParam)}Aot: ");

    var rand = Random.Shared;
    var sw = Stopwatch.StartNew();
    for (int i = 0; i < rounds; i++)
    {
      var mlkem_param = NetServicesCrypto.ToMLDsaAlgorithm();

      //ML-KEM Parameter select. Only the first 3 algos
      var idx = rand.Next(mlkem_param.Length);
      var param = (byte)NetServicesCrypto.FromMLDsaAlgorithm(mlkem_param[idx]);

      var err = Native.CreateMlDsaKeyPairParamAot(
        param,
        out IntPtr priv_key_ptr, out int priv_key_length,
        out IntPtr pub_key_ptr, out int pub_key_length,
        out IntPtr guid_id_ptr, out int guid_id_length);
      AssertError(err);

      var privkey = new UsIPtr<byte>(
        ToBytes(priv_key_ptr, priv_key_length));
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
    Console.WriteLine($" rounds = {rounds}; t = {t}ms; td = {t / (double)rounds}ms");
  }

  public static void TestPqcMlDsaSaveLoadKeyPairs(int rounds)
  {
    Console.Write($"{nameof(TestPqcMlDsaSaveLoadKeyPairs)}Aot: ");

    var rand = Random.Shared;
    var sw = Stopwatch.StartNew();
    for (int i = 0; i < rounds; i++)
    {
      var err = Native.CreateMlDsaKeyPairAot(
        out IntPtr priv_key_ptr, out int priv_key_length,
        out IntPtr pub_key_ptr, out int pub_key_length,
        out IntPtr guid_id_ptr, out int guid_id_length,
        out byte mldsa_param);
      AssertError(err);

      var privkey = new UsIPtr<byte>(ToBytes(priv_key_ptr, priv_key_length));
      Native.FreeBuffer(priv_key_ptr);

      var pubkey = ToBytes(pub_key_ptr, pub_key_length);
      Native.FreeBuffer(pub_key_ptr);

      var guid_bytes = ToBytes(guid_id_ptr, guid_id_length);
      Native.FreeBuffer(guid_id_ptr);

      var guid = new Guid(guid_bytes);
      var param = NetServicesCrypto.ToMLDsaAlgorithm((MLDsaParam)mldsa_param);

      //WICHTIG: Wenn 'with_priv_key = false', dann wird der
      //         PrivateKey nicht abgespeichert.

      //IMPORTANT: If 'with_priv_key = false', the private
      //           key will not be saved.
      var with_priv_key = int.IsEvenInteger(rand.Next());

      var kpfile = "mldsa_keypair.key"u8;
      err = Native.SavePqcMlDsaKeyPairAot(
        kpfile, kpfile.Length,
        privkey.ToBytes(), privkey.Length,
        pubkey, pubkey.Length,
        guid_bytes, guid_bytes.Length,
        mldsa_param, with_priv_key);
      AssertError(err);

      err = Native.LoadPqcMlDsaKeyPairAot(
        kpfile, kpfile.Length,
        out IntPtr priv_key_ptr2, out int priv_key_length2,
        out IntPtr pub_key_ptr2, out int pub_key_length2,
        out IntPtr guid_id_ptr2, out int guid_id_length2,
        out byte mldsa_param2);
      AssertError(err);

      var privkey2 = new UsIPtr<byte>(
        ToBytes(priv_key_ptr2, priv_key_length2));
      Native.FreeBuffer(priv_key_ptr2);

      var pubkey2 = ToBytes(pub_key_ptr2, pub_key_length2);
      Native.FreeBuffer(pub_key_ptr2);

      var guid_bytes2 = ToBytes(guid_id_ptr2, guid_id_length2);
      Native.FreeBuffer(guid_id_ptr2);

      var guid2 = new Guid(guid_bytes2);
      var param2 = NetServicesCrypto.
        ToMLDsaAlgorithm((MLDsaParam)mldsa_param2);

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

      if (param != param2)
        throw new Exception();

      if (i % (rounds / 10) == 0)
        Console.Write(".");
    }

    sw.Stop();

    var t = sw.ElapsedMilliseconds;
    Console.WriteLine($" rounds = {rounds}; t = {t}ms; td = {t / (double)rounds}ms");
  }

  private static void TestPqcMlDsaSingleSignature(int rounds)
  { 
    //For the temporary exchange, it would not be necessary to save
    //the keys, as all generated keys are only required for the
    //respective session.

    //Für den temporären Austausch, ist das Abspeichern der
    //Schlüssel nicht notwending, da alle generierten Schlüssel
    //nur für die jeweilige Sitzung benötigt werden.

    Console.Write($"{nameof(TestPqcMlDsaSingleSignature)}: ");

    if (rounds < 10) rounds = 10;
    var algos = NetServicesCrypto.ToMLDsaAlgorithm();

    var rand = Random.Shared;
    var sw = Stopwatch.StartNew();
    for (var i = 0; i < rounds; i++)
    {
      //Create a random Message
      var size = rand.Next(10, 128);
      var message = NetServicesCrypto.RngCryptoBytes(size);

      //ML-DSA Parameter select. Only the first 3 parameters
      var idx = rand.Next(algos.Length);
      var algo = algos[idx];

      //Create and Save a legal ML-DSA-KeyPair
      KeyPairParamInfo kpip = null!;
      var rnum = rand.Next(int.MaxValue);
      if (int.IsEvenInteger(rnum))
      {
        kpip = CreateNativeMlDsaKeyPair(); algo = kpip.Algo;
      }
      else kpip = CreateNativeMlDsaKeyPair(algo);

      var (pk, pubk) = (kpip.PrivKey, kpip.PubKey);

      using var privk = pk;
      var err = Native.PqcMlDsaSignAot(
        message, message.Length,
        privk.ToBytes(), privk.Length,
        (byte)NetServicesCrypto.FromMLDsaAlgorithm(algo),
        out IntPtr sign_ptr, out int signlength);
      AssertError(err);

      var signature = ToBytes(sign_ptr, signlength);
      Native.FreeBuffer(sign_ptr);

      var verify = Native.PqcMlDsaVerifyAot(
        message, message.Length,
        pubk, pubk.Length,
        signature, signature.Length,
        out err);
      AssertError(err);

      if (!verify)
        throw new Exception();

      if (i % (rounds / 10) == 0)
        Console.Write(".");
    }

    sw.Stop();

    var t = sw.ElapsedMilliseconds;
    Console.WriteLine($" rounds = {rounds}; t = {t}ms; td = {t / (double)rounds}ms");
  }

  private static void TestPqcMlDsaSingleSignatureKpiSaveLoad(int rounds)
  {
    //A new key pair is always created and saved. This takes time. 
    //For the temporary exchange, it would not be necessary to save
    //the keys, as all generated keys are only required for the
    //respective session.

    //Es wird immer wieder ein neuer Schlüsselpaar erstellt und
    //abgespeichert. Das braucht seine Zeit. 

    //Für den temporären Austausch, wäre das Abspeichern der
    //Schlüssel nicht notwending, da alle generierten Schlüssel
    //nur für die jeweilige Sitzung benötigt werden.

    Console.Write($"{nameof(TestPqcMlDsaSingleSignatureKpiSaveLoad)}: ");

    if (rounds < 10) rounds = 10;
    var algos = NetServicesCrypto.ToMLDsaAlgorithm();

    var rand = Random.Shared;
    var sw = Stopwatch.StartNew();
    for (var i = 0; i < rounds; i++)
    {
      //Create a random Message
      var size = rand.Next(10, 128);
      var message = NetServicesCrypto.RngCryptoBytes(size);

      //ML-DSA Parameter select. Only the first 3 parameters
      var idx = rand.Next(algos.Length);
      var algo = algos[idx];

      //Create and Save a legal ML-DSA-KeyPair
      KeyPairParamInfo kpip = null!;
      var kpfile = "mldsa_keypair.key";
      var rnum = rand.Next(int.MaxValue);
      if (int.IsEvenInteger(rnum))
      {
        kpip = CreateNativeMlDsaKeyPair(); algo = kpip.Algo;
      }
      else kpip = CreateNativeMlDsaKeyPair(algo);

      var (pk, pubk) = (kpip.PrivKey, kpip.PubKey);

      using var privk = pk;
      using var mldsainfo = new MlDsaKeyPairInfo(
        pubk, privk.ToBytes(), algo);
      mldsainfo.SaveKeyPair(kpfile, true);

      //Load KeyPairs Again
      using var info = MlDsaKeyPairInfo.Load_KeyPair(kpfile);
      if (!mldsainfo.Equals(info))
        throw new Exception();

      var err = Native.PqcMlDsaSignAot(
        message, message.Length,
        privk.ToBytes(), privk.Length,
        (byte)NetServicesCrypto.FromMLDsaAlgorithm(algo),
        out IntPtr sign_ptr, out int signlength);
      AssertError(err);

      var signature = ToBytes(sign_ptr, signlength);
      Native.FreeBuffer(sign_ptr);

      var verify = Native.PqcMlDsaVerifyAot(
        message, message.Length,
        pubk, pubk.Length,
        signature, signature.Length,
        out err);
      AssertError(err);

      if (!verify)
        throw new Exception();

      if (i % (rounds / 10) == 0)
        Console.Write(".");
    }

    sw.Stop();

    var t = sw.ElapsedMilliseconds;
    Console.WriteLine($" rounds = {rounds}; t = {t}ms; td = {t / (double)rounds}ms");
  }

  private static void TestPqcMlDsaSingleSignatureFile(int rounds)
  {
    //For the temporary exchange, it would not be necessary to save
    //the keys, as all generated keys are only required for the
    //respective session.

    //Für den temporären Austausch, ist das Abspeichern der
    //Schlüssel nicht notwending, da alle generierten Schlüssel
    //nur für die jeweilige Sitzung benötigt werden.

    Console.Write($"{nameof(TestPqcMlDsaSingleSignatureFile)}: ");

    if (rounds < 10) rounds = 10;

    var srcfile = "data";
    var rand = Random.Shared;
    var algos = NetServicesCrypto.ToMLDsaAlgorithm();

    var sw = Stopwatch.StartNew();
    for (var i = 0; i < rounds; i++)
    {
      //Create a rng-testfile 
      var max = (1 << 21) + 1024; //ca. 2Mb
      var size = Random.Shared.Next(max);
      SetRngFileData(srcfile, size);

      //ML-DSA Parameter select. Only the first 3 parameters
      var idx = rand.Next(algos.Length);
      var algo = algos[idx];

      //Create and Save a legal ML-DSA-KeyPair
      KeyPairParamInfo kpip = null!;
      var rnum = rand.Next(int.MaxValue);
      if (int.IsEvenInteger(rnum))
      {
        kpip = CreateNativeMlDsaKeyPair(); algo = kpip.Algo;
      }
      else kpip = CreateNativeMlDsaKeyPair(algo);

      var (pk, pubk) = (kpip.PrivKey, kpip.PubKey);

      using var privk = pk;
      var filename = Encoding.UTF8.GetBytes(srcfile);
      var err = Native.PqcMlDsaSignFileAot(
        filename, filename.Length,
        privk.ToBytes(), privk.Length,
        (byte)NetServicesCrypto.FromMLDsaAlgorithm(algo),
        out IntPtr sign_ptr, out int sign_length);
      AssertError(err);

      var signature = ToBytes(sign_ptr, sign_length);
      Native.FreeBuffer(sign_ptr);

      var verify = Native.PqcMlDsaVerifyFileAot(
        filename, filename.Length,
        pubk, pubk.Length,
        signature, signature.Length,
        out err);
      AssertError(err);

      if (!verify)
        throw new Exception();

      if (i % (rounds / 10) == 0)
        Console.Write(".");
    }
    sw.Stop();
    if (File.Exists(srcfile))
      File.Delete(srcfile);

    var t = sw.ElapsedMilliseconds;
    Console.WriteLine($" rounds = {rounds}; t = {t}ms; td = {t / (double)rounds}ms");
  }

  private static void TestPqcMlDsaSingleSignatureKpiSaveLoadFile(int rounds)
  {

    //A new key pair is always created and saved. This takes time. 
    //For the temporary exchange, it would not be necessary to save
    //the keys, as all generated keys are only required for the
    //respective session.

    //Es wird immer wieder ein neuer Schlüsselpaar erstellt und
    //abgespeichert. Das braucht seine Zeit. 

    //Für den temporären Austausch, wäre das Abspeichern der
    //Schlüssel nicht notwending, da alle generierten Schlüssel
    //nur für die jeweilige Sitzung benötigt werden.

    Console.Write($"{nameof(TestPqcMlDsaSingleSignatureKpiSaveLoadFile)}: ");

    if (rounds < 10) rounds = 10;

    var srcfile = "data";
    var algos = NetServicesCrypto.ToMLDsaAlgorithm();

    var rand = Random.Shared;
    var sw = Stopwatch.StartNew();
    for (var i = 0; i < rounds; i++)
    {
      //Create a rng-testfile 
      var max = (1 << 21) + 1024; //ca. 2Mb
      var size = Random.Shared.Next(max);
      SetRngFileData(srcfile, size);

      //ML-DSA Parameter select. Only the first 3 parameters
      var idx = rand.Next(algos.Length);
      var algo = algos[idx];

      //Create and Save a legal ML-DSA-KeyPair
      KeyPairParamInfo kpip = null!;
      var kpfile = "mldsa_keypair.key";
      var rnum = rand.Next(int.MaxValue);
      if (int.IsEvenInteger(rnum))
      {
        kpip = CreateNativeMlDsaKeyPair(); algo = kpip.Algo;
      }
      else kpip = CreateNativeMlDsaKeyPair(algo);

      var (pk, pubk) = (kpip.PrivKey, kpip.PubKey);

      using var privk = pk;
      using var mldsainfo = new MlDsaKeyPairInfo(
        pubk, privk.ToBytes(), algo);
      mldsainfo.SaveKeyPair(kpfile, true);

      //Load KeyPairs again
      using var info = MlDsaKeyPairInfo.Load_KeyPair(kpfile);
      if (!mldsainfo.Equals(info))
        throw new Exception();

      var filename = Encoding.UTF8.GetBytes(srcfile);
      var err = Native.PqcMlDsaSignFileAot(
        filename, filename.Length,
        privk.ToBytes(), privk.Length,
        (byte)NetServicesCrypto.FromMLDsaAlgorithm(algo),
        out IntPtr sign_ptr, out int sign_length);
      AssertError(err);

      var signature = ToBytes(sign_ptr, sign_length);
      Native.FreeBuffer(sign_ptr);

      var verify = Native.PqcMlDsaVerifyFileAot(
        filename, filename.Length,
        pubk, pubk.Length,
        signature, signature.Length,
        out err);
      AssertError(err);

      if (!verify)
        throw new Exception();

      if (i % (rounds / 10) == 0)
        Console.Write(".");
    }
    sw.Stop();
    if (File.Exists(srcfile))
      File.Delete(srcfile);

    var t = sw.ElapsedMilliseconds;
    Console.WriteLine($" rounds = {rounds}; t = {t}ms; td = {t / (double)rounds}ms");
  }


  private static KeyPairParamInfo CreateNativeMlDsaKeyPair()
  {
    var err = Native.CreateMlDsaKeyPairAot(
        out IntPtr priv_key_ptr, out int priv_key_length,
        out IntPtr pub_key_ptr, out int pub_key_length,
        out IntPtr guid_id_ptr, out int guid_id_length,
        out byte mldsa_param);
    AssertError(err);

    var privkey = new UsIPtr<byte>(ToBytes(
      priv_key_ptr, priv_key_length));
    Native.FreeBuffer(priv_key_ptr);

    var pubkey = ToBytes(pub_key_ptr, pub_key_length);
    Native.FreeBuffer(pub_key_ptr);

    var guid = ToBytes(guid_id_ptr, guid_id_length);
    Native.FreeBuffer(guid_id_ptr);

    var param = NetServicesCrypto.
      ToMLDsaAlgorithm((MLDsaParam)mldsa_param);

    return new KeyPairParamInfo()
    {
      Guid = guid,
      Algo = param,
      PubKey = pubkey,
      PrivKey = privkey,
    };
  }

  private static KeyPairParamInfo CreateNativeMlDsaKeyPair(
    MLDsaAlgorithm algo)
  {
    var err = Native.CreateMlDsaKeyPairParamAot(
        (byte)NetServicesCrypto.FromMLDsaAlgorithm(algo),
        out IntPtr priv_key_ptr, out int priv_key_length,
        out IntPtr pub_key_ptr, out int pub_key_length,
        out IntPtr guid_id_ptr, out int guid_id_length);
    AssertError(err);

    var privkey = new UsIPtr<byte>(ToBytes(
      priv_key_ptr, priv_key_length));
    Native.FreeBuffer(priv_key_ptr);

    var pubkey = ToBytes(pub_key_ptr, pub_key_length);
    Native.FreeBuffer(pub_key_ptr);

    var guid = ToBytes(guid_id_ptr, guid_id_length);
    Native.FreeBuffer(guid_id_ptr);

    return new KeyPairParamInfo()
    {
      Guid = guid,
      Algo = algo,
      PubKey = pubkey,
      PrivKey = privkey,
    };
  }

  private static bool IsNullOrEmpty<T>(ReadOnlySpan<T> bytes)
  where T : INumber<T> =>
    bytes.IsEmpty || bytes.Length == 0;

  private static bool IsNullOrEmpty<T>(ReadOnlyMemory<T> bytes)
  where T : INumber<T> =>
    bytes.IsEmpty || bytes.Length == 0;


  private class KeyPairParamInfo : IDisposable
  {
    public byte[] Guid { get; set; } = [];
    public byte[] PubKey { get; set; } = [];
    public MLDsaAlgorithm Algo { get; set; } = null!;
    public UsIPtr<byte> PrivKey { get; set; } = UsIPtr<byte>.Empty;

    public void Dispose()
    {
      this.PrivKey?.Dispose();

      this.Guid = [];
      this.PubKey = [];
      this.Algo = null!;
      this.PrivKey = UsIPtr<byte>.Empty;
    }
  }
}
