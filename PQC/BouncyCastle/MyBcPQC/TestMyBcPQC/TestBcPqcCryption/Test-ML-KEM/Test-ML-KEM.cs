

//ML-KEM (Kyber)
//Module-Lattice-Based
//FIPS PUB 203
//https://nvlpubs.nist.gov/nistpubs/FIPS/NIST.FIPS.203.ipd.pdf


using System.Diagnostics;
using Org.BouncyCastle.Security;
using System.Security.Cryptography;
using Org.BouncyCastle.Crypto.Parameters;

namespace michele.natale.TestBcPqcs;

using BcPqcs;
using Services;

public class TestMLKEM
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
    //In practice, I prefer the temporary exchange, as no keys
    //have to be saved for this, but the exchange of all newly
    //generated keys is carried out per session.

    //Es wird immer wieder ein neuer Schlüsselpaar erstellt und
    //abgespeichert. Das braucht seine Zeit. 
    //In der Praxis bevorzuge ich den temporären Austausch, da 
    //dafür keine Schlüssel abgespeichert werden müssen, sondern
    //der Austausch aller neu generierter Schlüssel, pro Sitzung
    //durchgeführt wird.

    Console.Write($"{nameof(Test_ML_KEM_SharedKey)}: ");

    if (rounds < 10) rounds = 10;
    var rand = new SecureRandom();
    var parameters = BcPqcServices.ToMLKemParameters();

    var sw = Stopwatch.StartNew();
    for (var i = 0; i < rounds; i++)
    {
      //ML-KEM Parameter select
      var idx = rand.Next(parameters.Length);
      var parameter = parameters[idx];

      //Symmetric CryptionAlgorithm select
      var cas = Enum.GetValues<CryptionAlgorithm>();
      idx = rand.Next(cas.Length);
      var cryptoalgo = cas[idx];

      //Create and Save a legal ML-KEM-KeyPair
      var kpfile = "mlkem_keypair.key";
      var (privk, pubk) = MLKEM.ToKeyPair(rand, parameter);
      var mlkeminfo = new MlKemKeyPairInfo(
        pubk, privk, parameter, cryptoalgo);
      mlkeminfo.SaveKeyPair(kpfile, true);

      //Load KeyPairs again
      var info = MlKemKeyPairInfo.Load_KeyPair(kpfile);
      if (!mlkeminfo.Equals(info))
        throw new Exception();

      //ML-KEM Encapsulation
      cryptoalgo = info.CryptAlgo;
      parameter = info.ToParameter;

      //Encapsulation
      var pubkey = MLKemPublicKeyParameters
        .FromEncoding(parameter, info.PubKey);

      var sharedkey1 = MLKEM.ToSharedKey(
        pubkey, parameter, rand, out var capsulationkey);

      //Decapsulation 
      var privkey = MLKemPrivateKeyParameters
        .FromEncoding(parameter, info.PrivKey);

      var sharedkey2 = MLKEM.ToSharedKey(
        privkey, parameter, capsulationkey);

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
    //In practice, I prefer the temporary exchange, as no keys
    //have to be saved for this, but the exchange of all newly
    //generated keys is carried out per session.

    //Es wird immer wieder ein neuer Schlüsselpaar erstellt und
    //abgespeichert. Das braucht seine Zeit. 
    //In der Praxis bevorzuge ich den temporären Austausch, da 
    //dafür keine Schlüssel abgespeichert werden müssen, sondern
    //der Austausch aller neu generierter Schlüssel, pro Sitzung
    //durchgeführt wird.

    Console.Write($"{nameof(Test_ML_KEM_Single_Cryption)}: ");

    if (rounds < 10) rounds = 10;

    var sym_algo = string.Empty;
    var rand = new SecureRandom();
    var parameters = BcPqcServices.ToMLKemParameters();

    var sw = Stopwatch.StartNew();
    for (var i = 0; i < rounds; i++)
    {
      //Create a RngMessage
      var size = rand.Next(10, 128);
      var message = RngBytes(rand, size);

      //Create a associated 
      //size = rand.Next(8, 128);
      //var associated = RngBytes(rand, size);
      var associated = "© michele natale 2025"u8;

      //ML-KEM Parameter select
      var idx = rand.Next(parameters.Length);
      var parameter = parameters[idx];

      //Symmetric CryptionAlgorithm select
      var cas = Enum.GetValues<CryptionAlgorithm>();
      idx = rand.Next(cas.Length);
      var cryptoalgo = cas[idx];
      sym_algo = cryptoalgo.ToString();

      //Create and Save a legal ML-KEM-KeyPair
      var kpfile = "mlkem_keypair.key";
      var (privkey, pubkey) = MLKEM.ToKeyPair(rand, parameter);
      var info = new MlKemKeyPairInfo(
        pubkey, privkey, parameter, cryptoalgo);
      info.SaveKeyPair(kpfile, true);

      //ML-KEM- and Symmetric Cryption
      var cipher = MLKEM.MlKemEncryption(message, kpfile, associated, rand);
      var decipher = MLKEM.MlKemDecryption(cipher, kpfile, associated);

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
    Console.Write($"{nameof(Test_ML_KEM_Single_Cryption_File)}: ");

    if (rounds < 10) rounds = 10;

    var sym_algo = string.Empty;
    var rand = new SecureRandom();
    var parameters = BcPqcServices.ToMLKemParameters();
    string srcfile = "data", destfile = "cipher", srcrfile = "datar";

    var sw = Stopwatch.StartNew();
    for (var i = 0; i < rounds; i++)
    {
      //Create a rng-testfile 
      var max = (1 << 21) + 1024;
      var size = Random.Shared.Next(max);
      SetRngFileData(srcfile, size);

      //Create a associated 
      size = rand.Next(8, 128);
      var associated = RngBytes(rand, size);
      //var associated = "© michele natale 2025"u8;

      //ML-KEM Parameter select
      var idx = rand.Next(parameters.Length);
      var parameter = parameters[idx];

      //Symmetric CryptionAlgorithm select
      var cas = Enum.GetValues<CryptionAlgorithm>();
      idx = rand.Next(cas.Length);
      var cryptoalgo = cas[idx];
      sym_algo = cryptoalgo.ToString();

      //Create and Save a legal ML-KEM-KeyPair
      var kpfile = "mlkem_keypair.key";
      var (privkey, pubkey) = MLKEM.ToKeyPair(rand, parameter);
      var info = new MlKemKeyPairInfo(
        pubkey, privkey, parameter, cryptoalgo);
      info.SaveKeyPair(kpfile, true);

      //ML-KEM- and Symmetric Cryption
      MLKEM.MlKemEncryptionFile(srcfile, destfile, kpfile, associated, rand);
      MLKEM.MlKemDecryptionFile(destfile, srcrfile, kpfile, associated);

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
    //The KeyPair is not saved here. Everything remains in
    //temporary exchange, which is why it is so fast.
    //*****************************************************

    Console.Write($"{nameof(Test_ML_KEM_Alice_Bob_Cryption)}: ");

    if (rounds < 10) rounds = 10;

    var sym_algo = string.Empty;
    var rand = new SecureRandom();
    var cas = Enum.GetValues<CryptionAlgorithm>();
    var parameters = BcPqcServices.ToMLKemParameters();

    var sw = Stopwatch.StartNew();
    for (var i = 0; i < rounds; i++)
    {
      //Create a RngMessage
      var size = rand.Next(10, 128);
      var message = RngBytes(rand, size);

      //Create a associated 
      //size = rand.Next(8, 128);
      //var associated = RngBytes(rand, size);
      var associated = "© michele natale 2025"u8;

      //ML-KEM Parameter select
      var idx = rand.Next(parameters.Length);
      var parameter = parameters[idx];

      //Symmetric CryptionAlgorithm select      
      idx = rand.Next(cas.Length);
      var cryptoalgo = cas[idx];
      sym_algo = cryptoalgo.ToString();

      //Alice needs the MLKEMParameter for the PublicKey.
      var alice = new AliceMLKEM(parameter);

      //Bob holt sich den PublicKey und den MLKEMParameter von Alice,
      //wählt einen CryptoAlgo, und lässt sich einen Sharedkey wie 
      //auch einen CapsulationKey generieren (Encapsulation), ...

      //Bob gets the PublicKey and the MLKEM parameter from Alice,
      //selects a CryptoAlgo, and generates a Sharedkey as well
      //as a CapsulationKey (Encapsulation), ...
      var bob = new BobMLKEM(parameter);
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
      fsout.Write(RngBytes(length));
      size -= length; length = size < 1024 * 1024 ? size : 1024 * 1024;
    }
  }

  public static bool FileEquals(string leftfile, string rightfile)
  {
    using var fsleft = new FileStream(leftfile, FileMode.Open, FileAccess.Read);
    using var fsright = new FileStream(rightfile, FileMode.Open, FileAccess.Read);
    return SHA256.HashData(fsleft).SequenceEqual(SHA256.HashData(fsright));
  }

  private static byte[] RngBytes(SecureRandom rand, int size)
  {
    var result = new byte[size];
    rand.NextBytes(result);
    if (result[0] == 0) result[0]++;
    return result;
  }

  private static byte[] RngBytes(int size)
  {
    var rand = Random.Shared;
    var result = new byte[size];
    rand.NextBytes(result);
    if (result[0] == 0) result[0]++;
    return result;
  }

  #endregion  Utils
}