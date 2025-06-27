

//ML-KEM (Kyber)
//Module-Lattice-Based
//FIPS PUB 203
//https://nvlpubs.nist.gov/nistpubs/FIPS/NIST.FIPS.203.ipd.pdf


using Org.BouncyCastle.Security;
using Org.BouncyCastle.Crypto.Kems;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Generators;

namespace michele.natale.BcPqcs;

using Pointers;
using Services;

/// <summary>
/// Provides cryption methods around the ML-KEM algorithm.
/// </summary>
public sealed class MLKEM : IMLKEM
{

  #region ML-KEM En- / Decryption

  #region ML-KEM-Single-Cryption
  public static byte[] MlKemEncryption(
    ReadOnlySpan<byte> bytes, string keypairfile,
    ReadOnlySpan<byte> associated, SecureRandom rand)
  {
    var info = MlKemKeyPairInfo.Load_KeyPair(keypairfile);

    //ML-KEM Encapsulation
    var cryptalgo = info.CryptAlgo;
    var parameter = info.ToParameter();
    var pubkey = MLKemPublicKeyParameters
      .FromEncoding(parameter, info.PubKey);

    var sharedkey = ToSharedKey(pubkey, parameter, rand, out var capsulationkey);

    //ML-KEM Symmetric Encryption
    var cipher = BcPqcServices.EncryptionWithCryptionAlgo(
      bytes, sharedkey, associated, cryptalgo);

    var ofs = 7;
    var capslength = BitConverter.GetBytes(capsulationkey.Length);
    var idxca = (byte)(byte.MaxValue - (byte)(ofs + (byte)cryptalgo));
    var idxparam = (byte)(byte.MaxValue - (byte)(ofs + ToIndex(parameter)));

    var result = new byte[6 + capsulationkey.Length + cipher.Length];
    result[0] = idxca; result[1] = idxparam;
    Array.Copy(capslength, 0, result, 2, capslength.Length);
    Array.Copy(capsulationkey, 0, result, 6, capsulationkey.Length);
    Array.Copy(cipher, 0, result, 6 + capsulationkey.Length, cipher.Length);

    return result;
  }

  public static byte[] MlKemDecryption(
    ReadOnlySpan<byte> bytes, string keypairfile,
    ReadOnlySpan<byte> associated)
  {
    var ofs = 7;
    var calgo = (CryptionAlgorithm)(byte.MaxValue - ofs - bytes[0]);
    var parameter = ToMLKemParameter(byte.MaxValue - ofs - bytes[1]);
    var capslenght = BitConverter.ToInt32(bytes.Slice(2, 4));
    var capsulationkey = bytes.Slice(6, capslenght);

    //ML-KEM Decapsulation
    var info = MlKemKeyPairInfo.Load_KeyPair(keypairfile);
    var privkey = MLKemPrivateKeyParameters
      .FromEncoding(parameter, info.ToPrivKey().ToBytes());
    var sharedkey = ToSharedKey(privkey, parameter, capsulationkey);

    //ML-KEM Symmetric Decryption 
    return BcPqcServices.DecryptionWithCryptionAlgo(
      bytes[(6 + capslenght)..], sharedkey, associated, calgo);
  }
  #endregion ML-KEM-Single-Cryption

  #region ML-KEM-Single-Cryption File 
  public static void MlKemEncryptionFile(
    string src, string dest, string keypairfile,
    ReadOnlySpan<byte> associated, SecureRandom rand)
  {
    var info = MlKemKeyPairInfo.Load_KeyPair(keypairfile);

    //ML-KEM Encapsulation
    var cryptalgo = info.CryptAlgo;
    var parameter = info.ToParameter();
    var pubkey = MLKemPublicKeyParameters
      .FromEncoding(parameter, info.PubKey);

    var sharedkey = ToSharedKey(pubkey, parameter, rand, out var capsulationkey);

    var ofs = 7;
    var capslength = BitConverter.GetBytes(capsulationkey.Length);
    var idxca = (byte)(byte.MaxValue - (byte)(ofs + (byte)cryptalgo));
    var idxparam = (byte)(byte.MaxValue - (byte)(ofs + ToIndex(parameter)));

    using var fsin = new FileStream(src, FileMode.Open, FileAccess.Read);
    using var fsout = new FileStream(dest, FileMode.Create, FileAccess.ReadWrite);
    fsout.WriteByte(idxca); fsout.WriteByte(idxparam);
    fsout.Write(capslength); fsout.Write(capsulationkey);

    //ML-KEM Symmetric Encryption
    BcPqcServices.EncryptionFileWithCryptionAlgo(
      fsin, fsout, 0, (int)fsin.Length, (int)fsout.Position,
      sharedkey, associated, cryptalgo);
  }

  public static void MlKemDecryptionFile(
    string src, string dest, string keypairfile,
    ReadOnlySpan<byte> associated)
  {
    using var fsin = new FileStream(src, FileMode.Open, FileAccess.Read);
    using var fsout = new FileStream(dest, FileMode.Create, FileAccess.Write);

    var ofs = 7;
    var calgo = (CryptionAlgorithm)(byte.MaxValue - ofs - fsin.ReadByte());
    var parameter = ToMLKemParameter(byte.MaxValue - ofs - fsin.ReadByte());

    var buffer = new byte[4];
    var n = fsin.Read(buffer);
    n = BitConverter.ToInt32(buffer);

    buffer = new byte[n];
    fsin.ReadExactly(buffer);
    var capsulationkey = buffer.ToArray();
    int startin = (int)fsin.Position, lengthin = (int)(fsin.Length - startin);

    //ML-KEM Decapsulation
    var info = MlKemKeyPairInfo.Load_KeyPair(keypairfile);
    var privkey = MLKemPrivateKeyParameters
      .FromEncoding(info.ToParameter(), info.ToPrivKey().ToBytes());
    var sharedkey = ToSharedKey(privkey, parameter, capsulationkey);

    //ML-KEM Decryption
    var startout = 0;
    BcPqcServices.DecryptionFileWithCryptionAlgo(
      fsin, fsout, startin, lengthin, startout, sharedkey, associated, info.CryptAlgo);
  }
  #endregion ML-KEM-Single-Cryption File

  #region ML-KEM KeyPair, SharedKey generate 
  public static (byte[] PrivKey, byte[] PubKey) ToKeyPair(
    SecureRandom rand, MLKemParameters parameter)
  {
    var keypair_generator = new MLKemKeyPairGenerator();
    var generator = new MLKemKeyGenerationParameters(rand, parameter);
    keypair_generator.Init(generator);

    var keypair = keypair_generator.GenerateKeyPair();
    var mlk_pubkey = (MLKemPublicKeyParameters)keypair.Public;
    var mlk_privkey = (MLKemPrivateKeyParameters)keypair.Private;

    return (mlk_privkey.GetEncoded(), mlk_pubkey.GetEncoded());
  }

  public static UsIPtr<byte> ToSharedKey(
    MLKemPublicKeyParameters pubkey, MLKemParameters parameter,
    SecureRandom rand, out byte[] capsulationkey)
  {
    //ML-KEM Encapsulation
    var encapsulator = new MLKemEncapsulator(parameter);
    encapsulator.Init(new ParametersWithRandom(pubkey, rand));

    var sharedkey = new byte[encapsulator.SecretLength];
    capsulationkey = new byte[encapsulator.EncapsulationLength];
    encapsulator.Encapsulate(capsulationkey, 0, capsulationkey.Length, sharedkey, 0, sharedkey.Length);

    return new UsIPtr<byte>(sharedkey);
  }

  public static UsIPtr<byte> ToSharedKey(
    MLKemPrivateKeyParameters privkey,
    MLKemParameters parameter,
    ReadOnlySpan<byte> capsulationkey)
  {
    //ML-KEM Decapsulation
    var decapsulator = new MLKemDecapsulator(parameter);
    decapsulator.Init(privkey);

    var sharedkey = new byte[decapsulator.SecretLength];
    decapsulator.Decapsulate(capsulationkey.ToArray(), 0, capsulationkey.Length, sharedkey, 0, sharedkey.Length);

    return new UsIPtr<byte>(sharedkey);
  }

  #endregion ML-KEM KeyPair, SharedKey generate

  #endregion ML-KEM En- / Decryption

  #region Utils

  public static MLKemParameters ToMLKemParameter(int idx)
  {
    var a = MLKemParameters.ml_kem_512;
    var b = MLKemParameters.ml_kem_768;
    var c = MLKemParameters.ml_kem_1024;
    MLKemParameters[] result = [a, b, c];

    return result[idx];
  }

  public static int ToIndex(MLKemParameters parameter)
  {
    var a = MLKemParameters.ml_kem_512;
    var b = MLKemParameters.ml_kem_768;
    var c = MLKemParameters.ml_kem_1024;
    MLKemParameters[] parameters = [a, b, c];

    var result = Array.IndexOf(parameters, parameter);
    if (result >= 0) return result;

    throw new InvalidKeyException(nameof(parameter));
  }

  #endregion  Utils
}