//ML-KEM (Kyber)
//Module-Lattice-Based
//FIPS PUB 203
//https://nvlpubs.nist.gov/nistpubs/FIPS/NIST.FIPS.203.ipd.pdf


using System.Security.Cryptography;


namespace michele.natale;

using Pointers;

partial class NetServicesCrypto : IMlKemEx
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
    using var sharedkey = ToSharedKey(kem, out var capsulation);

    //ML-KEM Symmetric Encryption
    var cipher = EncryptionWithCryptionAlgo(
      bytes, sharedkey, associated, info.CryptAlgo);

    var ofs = 7;
    var capslength = BitConverter.GetBytes(capsulation.Length);
    var idxca = (byte)(byte.MaxValue - (byte)(ofs + (byte)info.CryptAlgo));
    var idxparam = (byte)(byte.MaxValue - (byte)(ofs + ToIndex(info.ToAlgo())));

    var result = new byte[6 + capsulation.Length + cipher.Length];
    result[0] = idxca; result[1] = idxparam;
    Array.Copy(capslength, 0, result, 2, capslength.Length);
    Array.Copy(capsulation, 0, result, 6, capsulation.Length);
    Array.Copy(cipher, 0, result, 6 + capsulation.Length, cipher.Length);
    MemoryClear(capslength, capsulation, cipher);

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
    var capsulation = bytes.Slice(6, capslenght);

    //ML-KEM Decapsulation
    using var info = MlKemKeyPairInfo.Load_KeyPair(keypairfile);
    using var kem = ToMLKemDecapsulation(info);
    using var sharedkey = ToSharedKey(kem, capsulation);

    //ML-KEM Symmetric Decryption 
    return DecryptionWithCryptionAlgo(
      bytes[(6 + capslenght)..], sharedkey, associated, calgo);
  }
  #endregion ML-KEM-Single-Cryption

  #region ML-KEM-Single-Cryption File 

  public async static Task MlKemEncryptionFileAsync(
    string src, string dest, UsIPtr<byte> private_key,
    ReadOnlyMemory<byte> capsulation, ReadOnlyMemory<byte> associated,
    MLKemAlgorithm mlkem_param, CryptionAlgorithm crypto_algo,
    CancellationToken ct = default)
  {
    //ML-KEM Encapsulation
    var kem = MLKem.ImportDecapsulationKey(mlkem_param, private_key.ToBytes());
    var sharedkey = new UsIPtr<byte>(kem.Decapsulate(capsulation.ToArray()));

    var ofs = 7;
    var capslength = BitConverter.GetBytes(capsulation.Length);
    var idxca = (byte)(byte.MaxValue - (byte)(ofs + (byte)crypto_algo));
    var idxparam = (byte)(byte.MaxValue - (byte)(ofs + ToIndex(mlkem_param)));

    await using var fsin = new FileStream(src, FileMode.Open, FileAccess.Read);
    await using var fsout = new FileStream(dest, FileMode.Create, FileAccess.ReadWrite);
    fsout.WriteByte(idxca); fsout.WriteByte(idxparam);
    fsout.Write(capslength); fsout.Write(capsulation.Span);

    //ML-KEM Symmetric Encryption
    await EncryptionFileWithCryptionAlgoAsync(
      fsin, fsout, 0, (int)fsin.Length, (int)fsout.Position,
      sharedkey, associated, crypto_algo, ct);
  }

  public async static Task MlKemEncryptionFileAsync(
    string src, string dest, string keypairfile,
    ReadOnlyMemory<byte> associated, CancellationToken ct = default)
  {
    using var info = MlKemKeyPairInfo.Load_KeyPair(keypairfile);

    //ML-KEM Encapsulation
    using var kem = ToMLKemEncapsulation(info);
    using var sharedkey = ToSharedKey(kem, out var capsulation);

    var ofs = 7;
    var capslength = BitConverter.GetBytes(capsulation.Length);
    var idxca = (byte)(byte.MaxValue - (byte)(ofs + (byte)info.CryptAlgo));
    var idxparam = (byte)(byte.MaxValue - (byte)(ofs + ToIndex(info.ToAlgo())));

    await using var fsin = new FileStream(src, FileMode.Open, FileAccess.Read);
    await using var fsout = new FileStream(dest, FileMode.Create, FileAccess.ReadWrite);
    fsout.WriteByte(idxca); fsout.WriteByte(idxparam);
    fsout.Write(capslength); fsout.Write(capsulation);

    //ML-KEM Symmetric Encryption
    await EncryptionFileWithCryptionAlgoAsync(
      fsin, fsout, 0, (int)fsin.Length, (int)fsout.Position,
      sharedkey, associated, info.CryptAlgo, ct);
  }



  public async static Task MlKemDecryptionFileAsync(
    string src, string dest, UsIPtr<byte> sharedkey,
    ReadOnlyMemory<byte> associated, CancellationToken ct = default)
  {

    await using var fsin = new FileStream(src, FileMode.Open, FileAccess.Read);
    await using var fsout = new FileStream(dest, FileMode.Create, FileAccess.Write);

    var ofs = 7;
    var calgo = (CryptionAlgorithm)(byte.MaxValue - ofs - fsin.ReadByte());
    var parameter = ToMLKemAlgorithm(byte.MaxValue - ofs - fsin.ReadByte());

    var buffer = new byte[4];
    var n = fsin.Read(buffer);
    n = BitConverter.ToInt32(buffer);

    buffer = new byte[n];
    fsin.ReadExactly(buffer);
    var capsulation = buffer.ToArray();
    int startin = (int)fsin.Position, lengthin = (int)(fsin.Length - startin);

    //ML-KEM Decryption
    var startout = 0;
    await DecryptionFileWithCryptionAlgoAsync(
      fsin, fsout, startin, lengthin, startout,
      sharedkey, associated, calgo, ct);
  }

  public async static Task MlKemDecryptionFileAsync(
    string src, string dest, string keypairfile,
    ReadOnlyMemory<byte> associated, CancellationToken ct = default)
  {
    await using var fsin = new FileStream(src, FileMode.Open, FileAccess.Read);
    await using var fsout = new FileStream(dest, FileMode.Create, FileAccess.Write);

    var ofs = 7;
    var calgo = (CryptionAlgorithm)(byte.MaxValue - ofs - fsin.ReadByte());
    var parameter = ToMLKemAlgorithm(byte.MaxValue - ofs - fsin.ReadByte());

    var buffer = new byte[4];
    var n = fsin.Read(buffer);
    n = BitConverter.ToInt32(buffer);

    buffer = new byte[n];
    fsin.ReadExactly(buffer);
    var capsulation = buffer.ToArray();
    int startin = (int)fsin.Position, lengthin = (int)(fsin.Length - startin);

    //ML-KEM Decapsulation
    using var info = MlKemKeyPairInfo.Load_KeyPair(keypairfile);
    using var kem = ToMLKemDecapsulation(info);
    using var sharedkey = ToSharedKey(kem, capsulation);

    //ML-KEM Decryption
    var startout = 0;
    await DecryptionFileWithCryptionAlgoAsync(
      fsin, fsout, startin, lengthin, startout, 
      sharedkey, associated, info.CryptAlgo, ct);
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
    MLKem bob, out byte[] bob_capsulation)
  {
    bob.Encapsulate(out bob_capsulation, out byte[] sharedkey);

    var result = new UsIPtr<byte>(sharedkey);
    Array.Clear(sharedkey);
    return result;
  }

  public static UsIPtr<byte> ToSharedKey(
    MLKem alice, ReadOnlySpan<byte> bob_capsulation)
  {
    var alice_secret_shared_key =
      alice.Decapsulate(bob_capsulation.ToArray());

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
