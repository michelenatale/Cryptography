//ML-KEM (Kyber)
//Module-Lattice-Based
//FIPS PUB 203
//https://pq-crystals.org/kyber/index.shtml
//https://nvlpubs.nist.gov/nistpubs/FIPS/NIST.FIPS.203.ipd.pdf


using System.Diagnostics;
using System.Security.Cryptography;

namespace michele.natale.TestMsPqcs;

using MsPqcs;
using Services; 

internal class TestMLKEM
{

  public static void Start()
  {
    var rounds = 10;
    Test_ML_KEM_SharedKey(10 * rounds);
    Test_ML_KEM_Single_Cryption(rounds);
    Test_ML_KEM_Single_Cryption_File(rounds);

    Test_ML_KEM_Alice_Bob_Cryption(rounds);

    Console.WriteLine();
  }

  #region ML-KEM Test-Methodes
  private static void Test_ML_KEM_SharedKey(int rounds)
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

    Console.Write($"{nameof(Test_ML_KEM_SharedKey)}: ");

    if (rounds < 10) rounds = 10; 
    var algos = MsPqcServices.ToMLKemAlgorithm();

    var sw = Stopwatch.StartNew();
    for (var i = 0; i < rounds; i++)
    {
      //ML-KEM Parameter select. Only the first 3 algos
      var idx = RandomNumberGenerator.GetInt32(algos.Length);
      var algo = algos[idx];

      //Symmetric CryptionAlgorithm select
      var cas = Enum.GetValues<CryptionAlgorithm>();
      idx = RandomNumberGenerator.GetInt32(cas.Length);
      var cryptoalgo = cas[idx];

      //Create and Save a legal ML-KEM-KeyPair
      var kpfile = "mlkem_keypair.key";
      using var kem = MLKem.GenerateKey(algo);
      var (privk, pubk) = MlKemEx.ToKeyPair(kem);

      using var mlkeminfo = new MlKemKeyPairInfo(
        pubk, privk.ToBytes(), algo, cryptoalgo);
      mlkeminfo.SaveKeyPair(kpfile, true);

      //Load KeyPairs again
      using var info = MlKemKeyPairInfo.Load_KeyPair(kpfile);
      if (!mlkeminfo.Equals(info))
        throw new Exception();

      //ML-KEM Encapsulation
      cryptoalgo = info.CryptAlgo;
      algo = info.ToAlgo();

      //Encapsulation
      var pubkey = info.PubKey;

      using var kem1 = MLKem.ImportEncapsulationKey(algo, pubkey);
      using var sharedkey1 = MlKemEx.ToSharedKey(
        kem1, out var capsulationkey);

      //Decapsulation 
      var privkey = info.ToPrivKey().ToBytes();
      using var kem2 = MLKem.ImportDecapsulationKey(algo, privkey);
      using var sharedkey2 = MlKemEx.ToSharedKey(
        kem2, capsulationkey);

      //Check equality.
      if (!sharedkey1.Equality(sharedkey2))
        throw new Exception();

      if (i % (rounds / 10) == 0)
        Console.Write(".");
    }
    sw.Stop();

    var t = sw.ElapsedMilliseconds;
    Console.WriteLine($" rounds = {rounds}, t = {t}ms, td = {t / (2 * rounds)}ms");
  }

  private static void Test_ML_KEM_Single_Cryption(int rounds)
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

    Console.Write($"{nameof(Test_ML_KEM_Single_Cryption)}: ");

    if (rounds < 10) rounds = 10;

    var sym_algo = string.Empty;
    var algos = MsPqcServices.ToMLKemAlgorithm();

    var sw = Stopwatch.StartNew();
    for (var i = 0; i < rounds; i++)
    {
      //Create a RngMessage
      var size = RandomNumberGenerator.GetInt32(10, 128);
      var message = MsPqcServices.RngCryptoBytes(size);

      //Create a associated 
      //size = rand.Next(8, 128);
      //var associated = RngBytes(rand, size);
      var associated = "© michele natale 2025"u8;

      //ML-KEM Parameter select
      var idx = RandomNumberGenerator.GetInt32(algos.Length);
      var algo = algos[idx];

      //Symmetric CryptionAlgorithm select
      var cas = Enum.GetValues<CryptionAlgorithm>();
      idx = RandomNumberGenerator.GetInt32(cas.Length);
      var cryptoalgo = cas[idx];
      sym_algo = cryptoalgo.ToString();

      //Create and Save a legal ML-KEM-KeyPair
      var kpfile = "mlkem_keypair.key";
      using var kem = MLKem.GenerateKey(algo);
      var (privkey, pubkey) = MlKemEx.ToKeyPair(kem);
      using var info = new MlKemKeyPairInfo(
        pubkey, privkey.ToBytes(), algo, cryptoalgo);
      info.SaveKeyPair(kpfile, true);

      //ML-KEM- and Symmetric Cryption
      var cipher = MlKemEx.MlKemEncryption(message, kpfile, associated);
      var decipher = MlKemEx.MlKemDecryption(cipher, kpfile, associated);

      //Check equality.
      if (!message.SequenceEqual(decipher))
        throw new Exception();

      if (i % (rounds / 10) == 0)
        Console.Write(".");
    }
    sw.Stop();

    var t = sw.ElapsedMilliseconds;
    Console.WriteLine($" rounds = {rounds}, lastalgo = {sym_algo}, t = {t}ms, td = {t / (2 * rounds)}ms");
  }

  private static void Test_ML_KEM_Single_Cryption_File(int rounds)
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

    Console.Write($"{nameof(Test_ML_KEM_Single_Cryption_File)}: ");

    if (rounds < 10) rounds = 10;

    var sym_algo = string.Empty; 
    var algos = MsPqcServices.ToMLKemAlgorithm();
    string srcfile = "data", destfile = "cipher", srcrfile = "datar";

    var sw = Stopwatch.StartNew();
    for (var i = 0; i < rounds; i++)
    {
      //Create a rng-testfile 
      var max = (1 << 21) + 1024;
      var size = Random.Shared.Next(max);
      SetRngFileData(srcfile, size);

      //Create a associated 
      size = RandomNumberGenerator.GetInt32(8, 128);
      var associated = MsPqcServices.RngCryptoBytes(size);
      //var associated = "© michele natale 2025"u8;

      //ML-KEM Parameter select
      var idx = RandomNumberGenerator.GetInt32(algos.Length);
      var algo = algos[idx];

      //Symmetric CryptionAlgorithm select
      var cas = Enum.GetValues<CryptionAlgorithm>();
      idx = RandomNumberGenerator.GetInt32(cas.Length);
      var cryptoalgo = cas[idx];
      sym_algo = cryptoalgo.ToString();

      //Create and Save a legal ML-KEM-KeyPair
      var kpfile = "mlkem_keypair.key";
      using var kem = MLKem.GenerateKey(algo);
      var (privkey, pubkey) = MlKemEx.ToKeyPair(kem);
      using var info = new MlKemKeyPairInfo(
        pubkey, privkey.ToBytes(), algo, cryptoalgo);
      info.SaveKeyPair(kpfile, true);

      //ML-KEM- and Symmetric Cryption
      MlKemEx.MlKemEncryptionFile(srcfile, destfile, kpfile, associated);
      MlKemEx.MlKemDecryptionFile(destfile, srcrfile, kpfile, associated);

      //Check both files for equality.
      if (!FileEquals(srcrfile, srcrfile))
        throw new Exception();

      if (i % (rounds / 10) == 0)
        Console.Write(".");
    }
    sw.Stop();

    var t = sw.ElapsedMilliseconds;
    Console.WriteLine($" rounds = {rounds}, lastalgo = {sym_algo}, t = {t}ms, td = {t / (2 * rounds)}ms");
  }

  private static void Test_ML_KEM_Alice_Bob_Cryption(int rounds)
  {
    //*****************************************************
    //The KeyPair is not stored on any hardware here.
    //Everything remains in temporary exchange, which
    //is why it is so fast.
    //*****************************************************

    Console.Write($"{nameof(Test_ML_KEM_Alice_Bob_Cryption)}: ");

    if (rounds < 10) rounds = 10;
    //rounds = rounds < 10 ? 10 : 10 * rounds;

    var sym_algo = string.Empty;
    var cas = Enum.GetValues<CryptionAlgorithm>();
    var parameters = MsPqcServices.ToMLKemAlgorithm();

    var sw = Stopwatch.StartNew();
    for (var i = 0; i < rounds; i++)
    {
      //Create a RngMessage
      var size = RandomNumberGenerator.GetInt32(10, 128);
      var message = MsPqcServices.RngCryptoBytes(size);

      //Create a associated 
      //size = rand.Next(8, 128);
      //var associated = RngBytes(rand, size);
      var associated = "© michele natale 2025"u8;

      //ML-KEM Parameter select
      var idx = RandomNumberGenerator.GetInt32(parameters.Length);
      var parameter = parameters[idx];

      //Symmetric CryptionAlgorithm select      
      idx = RandomNumberGenerator.GetInt32(cas.Length);
      var cryptoalgo = cas[idx];
      sym_algo = cryptoalgo.ToString();

      //Alice needs the MLKEMParameter for the PublicKey.
      using var alice = new AliceMLKEM(parameter);

      //Bob holt sich den PublicKey und den MLKEMParameter von Alice,
      //wählt einen CryptoAlgo, und lässt sich einen Sharedkey wie 
      //auch einen CapsulationKey generieren (Encapsulation), ...

      //Bob gets the PublicKey and the MLKEMParameter from Alice,
      //selects a CryptoAlgo, and generates a Sharedkey as well
      //as a CapsulationKey (Encapsulation), ...
      using var bob = new BobMLKEM(parameter);
      var capsulationkey = bob.GenerateSharedKey(alice.PubKey);

      // ... womit Alice den Sharedkey aus der CapsulationKey holt. 
      // Alice braucht dazu jedoch Ihren PrivateKey. (Decapsulation)
      // Nun kann Alice eine Verschlüsselung durchführen.

      // ... which Alice uses to get the Sharedkey from the CapsulationKey.
      // However, Alice needs your PrivateKey to do this. (Decapsulation)
      // Alice can now perform encryption.
      var cipher = alice.Encryption(message, capsulationkey, associated, cryptoalgo);

      //Bob besitzt nun alle relevanten Infos, um eine Entschlüsselung durchzuführen.

      //Bob now has all the relevant information to carry out decryption.
      var decipher = bob.Decryption(cipher, associated, cryptoalgo);

      //Check for equality.
      if (!decipher.SequenceEqual(message))
        throw new Exception();

      if (i % (rounds / 10) == 0)
        Console.Write(".");
    }
    sw.Stop();

    var t = sw.ElapsedMilliseconds;
    Console.WriteLine($" rounds = {rounds}, lastalgo = {sym_algo}, t = {t}ms, td = {t / (2 * rounds)}ms");
  }

  #endregion ML-KEM Test-Methodes

  #region Utils

  private static void SetRngFileData(string filename, int size)
  {
    using var fsout = new FileStream(filename, FileMode.Create, FileAccess.Write);

    var length = size < 1024 * 1024 ? size : 1024 * 1024;

    while (length > 0)
    {
      fsout.Write(MsPqcServices.RngCryptoBytes(length));
      size -= length; length = size < 1024 * 1024 ? size : 1024 * 1024;
    }
  }

  public static bool FileEquals(string leftfile, string rightfile)
  {
    using var fsleft = new FileStream(leftfile, FileMode.Open, FileAccess.Read);
    using var fsright = new FileStream(rightfile, FileMode.Open, FileAccess.Read);
    return SHA256.HashData(fsleft).SequenceEqual(SHA256.HashData(fsright));
  }

  #endregion  Utils
}
