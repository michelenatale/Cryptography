

//ML-KEM (Kyber)
//Module-Lattice-Based
//FIPS PUB 203
//https://nvlpubs.nist.gov/nistpubs/FIPS/NIST.FIPS.203.ipd.pdf


using System.Security.Cryptography;


namespace michele.natale.MsPqcs;

using Pointers;
using Services;

/// <summary>
/// Provides cryption methods around the ML-KEM algorithm.
/// </summary>
public sealed class MlKemEx : IMlKemEx
{

  #region ML-KEM En- / Decryption

  #region ML-KEM-Single-Cryption
  public static byte[] MlKemEncryption(
    ReadOnlySpan<byte> bytes, string keypairfile,
    ReadOnlySpan<byte> associated)
  {
    using var info = MlKemKeyPairInfo.Load_KeyPair(keypairfile);

    //ML-KEM Encapsulation
    using var kem = ToMLKemEncapsulation(info);
    using var sharedkey = ToSharedKey(kem, out var capsulationkey);

    //ML-KEM Symmetric Encryption
    var cipher = MsPqcServices.EncryptionWithCryptionAlgo(
      bytes, sharedkey, associated, info.CryptAlgo);

    var ofs = 7;
    var capslength = BitConverter.GetBytes(capsulationkey.Length);
    var idxca = (byte)(byte.MaxValue - (byte)(ofs + (byte)info.CryptAlgo));
    var idxparam = (byte)(byte.MaxValue - (byte)(ofs + ToIndex(info.ToAlgo())));

    var result = new byte[6 + capsulationkey.Length + cipher.Length];
    result[0] = idxca; result[1] = idxparam;
    Array.Copy(capslength, 0, result, 2, capslength.Length);
    Array.Copy(capsulationkey, 0, result, 6, capsulationkey.Length);
    Array.Copy(cipher, 0, result, 6 + capsulationkey.Length, cipher.Length);
    MsPqcServices.MemoryClear(capslength, capsulationkey, cipher);

    return result;
  }

  public static byte[] MlKemDecryption(
    ReadOnlySpan<byte> bytes, string keypairfile,
    ReadOnlySpan<byte> associated)
  {
    var ofs = 7;
    var calgo = (CryptionAlgorithm)(byte.MaxValue - ofs - bytes[0]);
    //var parameter = ToMLKemAlgorithm(byte.MaxValue - ofs - bytes[1]);
    var capslenght = BitConverter.ToInt32(bytes.Slice(2, 4));
    var capsulationkey = bytes.Slice(6, capslenght);

    //ML-KEM Decapsulation
    using var info = MlKemKeyPairInfo.Load_KeyPair(keypairfile);
    using var kem = ToMLKemDecapsulation(info);
    using var sharedkey = ToSharedKey(kem, capsulationkey);

    //ML-KEM Symmetric Decryption 
    return MsPqcServices.DecryptionWithCryptionAlgo(
      bytes[(6 + capslenght)..], sharedkey, associated, calgo);
  }
  #endregion ML-KEM-Single-Cryption

  #region ML-KEM-Single-Cryption File 
  public static void MlKemEncryptionFile(
    string src, string dest, string keypairfile,
    ReadOnlySpan<byte> associated)
  {
    using var info = MlKemKeyPairInfo.Load_KeyPair(keypairfile);

    //ML-KEM Encapsulation
    using var kem = ToMLKemEncapsulation(info);
    using var sharedkey = ToSharedKey(kem, out var capsulationkey);

    var ofs = 7;
    var capslength = BitConverter.GetBytes(capsulationkey.Length);
    var idxca = (byte)(byte.MaxValue - (byte)(ofs + (byte)info.CryptAlgo));
    var idxparam = (byte)(byte.MaxValue - (byte)(ofs + ToIndex(info.ToAlgo())));

    using var fsin = new FileStream(src, FileMode.Open, FileAccess.Read);
    using var fsout = new FileStream(dest, FileMode.Create, FileAccess.ReadWrite);
    fsout.WriteByte(idxca); fsout.WriteByte(idxparam);
    fsout.Write(capslength); fsout.Write(capsulationkey);

    //ML-KEM Symmetric Encryption
    MsPqcServices.EncryptionFileWithCryptionAlgo(
      fsin, fsout, 0, (int)fsin.Length, (int)fsout.Position,
      sharedkey, associated, info.CryptAlgo);
  }

  public static void MlKemDecryptionFile(
    string src, string dest, string keypairfile,
    ReadOnlySpan<byte> associated)
  {
    using var fsin = new FileStream(src, FileMode.Open, FileAccess.Read);
    using var fsout = new FileStream(dest, FileMode.Create, FileAccess.Write);

    var ofs = 7;
    var calgo = (CryptionAlgorithm)(byte.MaxValue - ofs - fsin.ReadByte());
    var parameter = ToMLKemAlgorithm(byte.MaxValue - ofs - fsin.ReadByte());

    var buffer = new byte[4];
    var n = fsin.Read(buffer);
    n = BitConverter.ToInt32(buffer);

    buffer = new byte[n];
    fsin.ReadExactly(buffer);
    var capsulationkey = buffer.ToArray();
    int startin = (int)fsin.Position, lengthin = (int)(fsin.Length - startin);

    //ML-KEM Decapsulation
    using var info = MlKemKeyPairInfo.Load_KeyPair(keypairfile);
    using var kem = ToMLKemDecapsulation(info);
    using var sharedkey = ToSharedKey(kem, capsulationkey);

    //ML-KEM Decryption
    var startout = 0;
    MsPqcServices.DecryptionFileWithCryptionAlgo(
      fsin, fsout, startin, lengthin, startout, sharedkey, associated, info.CryptAlgo);
  }
  #endregion ML-KEM-Single-Cryption File

  #region ML-KEM KeyPair, SharedKey generate 

  public static (UsIPtr<byte> PrivKey, byte[] PubKey) ToKeyPair(MLKem kem)
  {
    var pubcab = kem.ExportEncapsulationKey();
    var privcab = kem.ExportDecapsulationKey();

    var result = (new UsIPtr<byte>(privcab), pubcab);
    Array.Clear(privcab);
    return result;
  }

  public static UsIPtr<byte> ToSharedKey(
    MLKem bob, out byte[] bob_capsulation_key)
  {
    bob.Encapsulate(out bob_capsulation_key, out byte[] sharedkey);

    var result = new UsIPtr<byte>(sharedkey);
    Array.Clear(sharedkey);
    return result;
  }

  public static UsIPtr<byte> ToSharedKey(
    MLKem alice, ReadOnlySpan<byte> bob_capsulation_key)
  {
    var alice_secret_shared_key =
      alice.Decapsulate(bob_capsulation_key.ToArray());

    var result = new UsIPtr<byte>(alice_secret_shared_key);
    Array.Clear(alice_secret_shared_key);
    return result;
  }

  private static MLKem ToMLKemEncapsulation(MlKemKeyPairInfo info) =>
    ToMLKemPub(info.ToAlgo(), info.PubKey);

  private static MLKem ToMLKemDecapsulation(MlKemKeyPairInfo info)
  {
    using var privkey = info.ToPrivKey();
    return ToMLKemPriv(info.ToAlgo(), privkey);
  }

  private static MLKem ToMLKemPub(
    MLKemAlgorithm algo, ReadOnlySpan<byte> pubkey) =>
      MLKem.ImportEncapsulationKey(algo, pubkey);

  private static MLKem ToMLKemPriv(
    MLKemAlgorithm algo, UsIPtr<byte> privkey) =>
      MLKem.ImportDecapsulationKey(algo, privkey.ToBytes());

  #endregion ML-KEM KeyPair, SharedKey generate

  #endregion ML-KEM En- / Decryption

  #region Utils

  public static MLKemAlgorithm ToMLKemAlgorithm(int idx)
  {
    var a = MLKemAlgorithm.MLKem512;
    var b = MLKemAlgorithm.MLKem768;
    var c = MLKemAlgorithm.MLKem1024;
    MLKemAlgorithm[] result = [a, b, c];

    return result[idx];
  }

  public static int ToIndex(MLKemAlgorithm parameter)
  {
    var a = MLKemAlgorithm.MLKem512;
    var b = MLKemAlgorithm.MLKem768;
    var c = MLKemAlgorithm.MLKem1024;
    MLKemAlgorithm[] parameters = [a, b, c];

    var result = Array.IndexOf(parameters, parameter);
    if (result >= 0) return result;

    throw new KeyNotFoundException(nameof(parameter));
  }

  #endregion  Utils
}