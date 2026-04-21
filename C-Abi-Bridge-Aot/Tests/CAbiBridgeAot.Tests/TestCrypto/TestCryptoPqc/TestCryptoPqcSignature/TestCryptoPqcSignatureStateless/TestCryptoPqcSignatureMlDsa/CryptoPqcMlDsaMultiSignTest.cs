
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace michele.natale.Tests;

using Pointers;
using static CryptoTestUtils;


partial class CryptoPqcMlDsaTest
{
  public unsafe static void TestPqcMlDsaMultiSignKpf(int rounds)
  {
    Console.Write($"{nameof(TestPqcMlDsaMultiSignKpf)}Aot: ");

    var folder = "keypairs";
    var rand = Random.Shared;
    var sw = new Stopwatch();
    var signercounttotal = 0L;
    var projectname = "Project ML-DSA-Multi-Sign-Test";
    for (int i = 0; i < rounds; i++)
    {
      //Number of signatories
      var signatories = rand.Next(3, 16);
      signercounttotal += signatories;

      //Create SignerNames
      var signernames = Enumerable.Range(0, signatories)
        .Select(i => "Signer_" + i).ToArray();

      //Generate a random message with a maximum length of 2^20;
      //otherwise, switch to the file-based method.
      var size = rand.Next(10, 128);
      var message = new byte[size];
      rand.NextBytes(message);

      //Create a new MlDsa-KeyPair and save it.
      var dict = CreateMlDsaKeyPairAndSave(signernames, folder);

      sw.Start();

      //Load all MlDsa-KeyPairs.
      var kppis = MlDsaKeyPairLoad(folder);

      //IMPORTANT: The array must be sorted.
      var signerinfos = ExtractDataAndSort(
        kppis, dict, message, projectname);

      //From signerinfos - Resolution in bytes
      var guids = signerinfos.Select(x => x.SignerID).ToArray();
      var signs = signerinfos.Select(x => x.Signature).ToArray();
      var pubkeys = signerinfos.Select(x => x.PublicKey).ToArray();
      var signnames = signerinfos.Select(x => x.SignerName).ToArray();
      var signalgo = signerinfos.Select(x => x.PqcSignAlgo).ToArray();
      var projectnames = signerinfos.Select(x => x.ProjectName).ToArray();
      var signparams = signerinfos.Select(x => x.PqcSignAlgoParam).ToArray();

      //To Natives
      var guid_ptr = ToNative(guids, out var length); var guid_lengths = length;
      var sign_ptr = ToNative(signs, out length); var sign_lengths = length;
      var pubkey_ptr = ToNative(pubkeys, out length); var pubkey_lengths = length;
      var signname_ptr = ToNative(signnames, out length); var signname_lengths = length;
      var projectname_ptr = ToNative(projectnames, out length); var projectname_lengths = length;

      var msg_ptr = ToNative(message, length); var msg_lengths = *length;
      var signalgo_ptr = ToNative(signalgo, length); var signalgo_lengths = *length;
      var signparams_ptr = ToNative(signparams, length); var signparams_lengths = *length;

      var err = Native.PqcMlDsaMultiSignAot(
        msg_ptr, msg_lengths,

        guid_ptr, guid_lengths, guids.Length,
        sign_ptr, sign_lengths, signs.Length,
        pubkey_ptr, pubkey_lengths, pubkeys.Length,
        signname_ptr, signname_lengths, signnames.Length,
        projectname_ptr, projectname_lengths, projectnames.Length,

        signalgo_ptr, signalgo_lengths, signparams_ptr, signparams_lengths,

        out IntPtr multi_sign_ptr, out int multi_sign_length,
        out IntPtr multi_privkey_ptr, out int multi_privkey_length,
        out IntPtr multi_pubkey_ptr, out int multi_pubkey_length);
      AssertError(err);

      var multi_sign = ToBytes(multi_sign_ptr, multi_sign_length);
      Native.FreeBuffer(multi_sign_ptr);

      var multi_prikey = ToBytes(multi_privkey_ptr, multi_privkey_length);
      Native.FreeBuffer(multi_privkey_ptr);

      var multi_pubkey = ToBytes(multi_pubkey_ptr, multi_pubkey_length);
      Native.FreeBuffer(multi_pubkey_ptr);

      var verify = Native.PqcMlDsaVerifyAot(
       message, message.Length,
       multi_pubkey, multi_pubkey.Length,
       multi_sign, multi_sign.Length,
       out err);
      AssertError(err);

      if (!verify)
        throw new Exception();

      //What happens next:

      //Each signer receives the complete package(MlDsaSignerInfo), which includes
      //the message or message file, as well as the complete multi-signature data:
      //the multi-private key, multi-public key, and multi-MlDsa algorithm.

      //The MlDsaSignerInfo contains the GUID and name for each signer, the project
      //name, as well as each signer’s public key, signature, and the MlDSA algorithm
      //used.

      //This ensures that the process cannot be tampered with. Every single signature
      //can be traced back, and it must be ensured that the key pair remains available
      //for future use so that the relevant verifications can be reviewed and reconfirmed.
      //This could also be ensured by a neutral public authority that retains a complete
      //set of keys.


      //Wie geht es weiter:

      //Jeder Signer kriegt das komplette Packet (MlDsaSignerInfo), mit der Message bzw.
      //Messagedatei, sowie die kompletten Multi-Sign-Daten, also Multi-PrivKey,
      //Multi-PubKey, Multi-MlDsa-Algo.

      //Die MlDsaSignerInfo beinhaltet die Guid wie auch den Namen zu jeden Signer,
      //den Projektnamen, wie auch jeden PublicKey, jede Signatur und jeden verwendeten
      //mldsa-algorithm der Signer.

      //Somit ist gewährleistet, das keine Manipulation des Verfahrens gemacht werden kann.
      //Jede einzelne Signierung kann zurückverfolgt werden, wobei sichergestellt werden
      //muss, das der KeyPair - Schlüsselsatz auch für spätere Zwecke noch vorliegt, um
      //die entsprechenden Prüfungen nachzuvollziehen und nochmals sicherzustellen. Dies
      //könnte man auch mit einer neutralen Öffentlichen Instanz gewährleisten, die die
      //Wahrung einens kompletten Satzen für sich behält.


      sw.Stop();

      if (i % (rounds / 10) == 0)
        Console.Write(".");
    }

    var t = sw.ElapsedMilliseconds;
    var ssize = signercounttotal / rounds;
    Console.WriteLine($" rounds = {rounds}; signers = {ssize}; t = {t}ms; td = {t / (double)rounds}ms");
  }

  public async unsafe static void TestPqcMlDsaMultiSignFileKpfAsync(int rounds)
  {
    Console.Write($"{nameof(TestPqcMlDsaMultiSignFileKpfAsync)}Aot: ");

    var srcfile = "data";
    var filesizetotal = 0UL;
    var folder = "keypairs";
    var rand = Random.Shared;
    var sw = new Stopwatch();
    var signercounttotal = 0L;
    var projectname = "Project ML-DSA-Multi-Sign-File-Test";
    for (int i = 0; i < rounds; i++)
    {
      //Number of signatories
      var signatories = rand.Next(3, 16);
      signercounttotal += signatories;

      //Create SignerNames
      var signernames = Enumerable.Range(0, signatories)
        .Select(i => "Signer_" + i).ToArray();

      //Create a rng-testfile 
      var max = (1 << 20) + 1024; //ca. 2Mb
      var size = Random.Shared.Next(1000, max);
      SetRngFileData(srcfile, size);
      filesizetotal += (ulong)size;

      //Create a new MlDsa-KeyPair and save it.
      var dict = CreateMlDsaKeyPairAndSave(signernames, folder);

      sw.Start();

      //Load all MlDsa-KeyPairs.
      var kppis = MlDsaKeyPairLoad(folder);

      //IMPORTANT: The array must be sorted.
      var signerinfos = ExtractDataAndSort(
        kppis, dict, srcfile, projectname);

      //var signerinfos = kppis.Select(
      //  (k, idx) => ToMlDsaSignerInfo(k, srcfile,
      //    dict[new Guid(k.Guid)], projectname))
      //      .OrderBy(x => x.Signature, Comparer<byte[]>.Create((a, b) =>
      //      {
      //        int len = Math.Min(a.Length, b.Length);
      //        for (int i = 0; i < len; i++)
      //        {
      //          int diff = a[i].CompareTo(b[i]);
      //          if (diff != 0)
      //            return diff;
      //        }
      //        return a.Length.CompareTo(b.Length);
      //      })).ToArray();

      //From signerinfos - Resolution in bytes
      var guids = signerinfos.Select(x => x.SignerID).ToArray();
      var signs = signerinfos.Select(x => x.Signature).ToArray();
      var pubkeys = signerinfos.Select(x => x.PublicKey).ToArray();
      var signnames = signerinfos.Select(x => x.SignerName).ToArray();
      var signalgo = signerinfos.Select(x => x.PqcSignAlgo).ToArray();
      var projectnames = signerinfos.Select(x => x.ProjectName).ToArray();
      var signparams = signerinfos.Select(x => x.PqcSignAlgoParam).ToArray();

      //To Natives
      var guid_ptr = ToNative(guids, out var length); var guid_lengths = length;
      var sign_ptr = ToNative(signs, out length); var sign_lengths = length;
      var pubkey_ptr = ToNative(pubkeys, out length); var pubkey_lengths = length;
      var signname_ptr = ToNative(signnames, out length); var signname_lengths = length;
      var projectname_ptr = ToNative(projectnames, out length); var projectname_lengths = length;

      var fname = Encoding.UTF8.GetBytes(srcfile);
      var file_name_ptr = ToNative(fname, length); var file_name_lengths = *length;
      var signalgo_ptr = ToNative(signalgo, length); var signalgo_lengths = *length;
      var signparams_ptr = ToNative(signparams, length); var signparams_lengths = *length;

      var err = Native.PqcMlDsaMultiSignFileAot(
        file_name_ptr, file_name_lengths,

        guid_ptr, guid_lengths, guids.Length,
        sign_ptr, sign_lengths, signs.Length,
        pubkey_ptr, pubkey_lengths, pubkeys.Length,
        signname_ptr, signname_lengths, signnames.Length,
        projectname_ptr, projectname_lengths, projectnames.Length,

        signalgo_ptr, signalgo_lengths, signparams_ptr, signparams_lengths,

        out IntPtr multi_sign_ptr, out int multi_sign_length,
        out IntPtr multi_privkey_ptr, out int multi_privkey_length,
        out IntPtr multi_pubkey_ptr, out int multi_pubkey_length);
      AssertError(err);

      var multi_sign = ToBytes(multi_sign_ptr, multi_sign_length);
      Native.FreeBuffer(multi_sign_ptr);

      var multi_prikey = ToBytes(multi_privkey_ptr, multi_privkey_length);
      Native.FreeBuffer(multi_privkey_ptr);

      var multi_pubkey = ToBytes(multi_pubkey_ptr, multi_pubkey_length);
      Native.FreeBuffer(multi_pubkey_ptr);

      var verify = Native.PqcMlDsaVerifyFileAot(
       fname, fname.Length,
       multi_pubkey, multi_pubkey.Length,
       multi_sign, multi_sign.Length,
       out err);
      AssertError(err);

      if (!verify)
        throw new Exception();



      //What happens next:

      //Each signer receives the complete package(MlDsaSignerInfo), which includes
      //the message or message file, as well as the complete multi-signature data:
      //the multi-private key, multi-public key, and multi-MlDsa algorithm.

      //The MlDsaSignerInfo contains the GUID and name for each signer, the project
      //name, as well as each signer’s public key, signature, and the MlDSA algorithm
      //used.

      //This ensures that the process cannot be tampered with. Every single signature
      //can be traced back, and it must be ensured that the key pair remains available
      //for future use so that the relevant verifications can be reviewed and reconfirmed.
      //This could also be ensured by a neutral public authority that retains a complete
      //set of keys.


      //Wie geht es weiter:

      //Jeder Signer kriegt das komplette Packet (MlDsaSignerInfo), mit der Message bzw.
      //Messagedatei, sowie die kompletten Multi-Sign-Daten, also Multi-PrivKey,
      //Multi-PubKey, Multi-MlDsa-Algo.

      //Die MlDsaSignerInfo beinhaltet die Guid wie auch den Namen zu jeden Signer,
      //den Projektnamen, wie auch jeden PublicKey, jede Signatur und jeden verwendeten
      //mldsa-algorithm der Signer.

      //Somit ist gewährleistet, das keine Manipulation des Verfahrens gemacht werden kann.
      //Jede einzelne Signierung kann zurückverfolgt werden, wobei sichergestellt werden
      //muss, das der KeyPair - Schlüsselsatz auch für spätere Zwecke noch vorliegt, um
      //die entsprechenden Prüfungen nachzuvollziehen und nochmals sicherzustellen. Dies
      //könnte man auch mit einer neutralen Öffentlichen Instanz gewährleisten, die die
      //Wahrung einens kompletten Satzen für sich behält.


      sw.Stop();

      if (i % (rounds / 10) == 0)
        Console.Write(".");
    }

    var t = sw.ElapsedMilliseconds;
    var ssize = signercounttotal / rounds;
    var fsize = Math.Round((filesizetotal / 1024.0) / rounds, 3);
    Console.WriteLine($" rounds = {rounds}; signers = {ssize}; filesize = {fsize}kb; t = {t}ms; td = {t / (double)rounds}ms");
  }



  //private unsafe (byte** ptr, int* lengths) ToNative(byte[][] arr)
  //{
  //  int count = arr.Length;

  //  byte** pArray = (byte**)NativeMemory.Alloc((nuint)count, (nuint)sizeof(byte*));
  //  int* pLengths = (int*)NativeMemory.Alloc((nuint)count, (nuint)sizeof(int));

  //  for (int i = 0; i < count; i++)
  //  {
  //    byte[] src = arr[i];
  //    byte* p = (byte*)NativeMemory.Alloc((nuint)src.Length);
  //    src.CopyTo(new Span<byte>(p, src.Length));

  //    pArray[i] = p;
  //    pLengths[i] = src.Length;
  //  }

  //  return (pArray, pLengths);
  //}

  private static MlDsaSignerInfo ToMlDsaSignerInfo(
    KeyPairParamInfo kppi, ReadOnlySpan<byte> message,
    string signername, string projectname)
  {

    var err = Native.PqcMlDsaSignAot(
        message, message.Length,
        kppi.PrivKey.ToBytes(), kppi.PrivKey.Length,
        (byte)NetServicesCrypto.FromMLDsaAlgorithm(kppi.Algo),
        out IntPtr sign_ptr, out int signlength);
    AssertError(err);

    var result = new MlDsaSignerInfo()
    {
      Signature = ToBytes(sign_ptr, signlength),
    };
    Native.FreeBuffer(sign_ptr);

    result.SignerID = kppi.Guid;
    result.PublicKey = kppi.PubKey;

    result.SignerName = Encoding.UTF8.GetBytes(signername);
    result.ProjectName = Encoding.UTF8.GetBytes(projectname);

    result.PqcSignAlgo = (byte)PqcSignAlgo.ML_DSA; //Signer-Algo MlDsa, SlhDsa, lms etc.
    result.PqcSignAlgoParam = (byte)NetServicesCrypto.FromMLDsaAlgorithm(kppi.Algo);

    return result;
  }

  private static MlDsaSignerInfo ToMlDsaSignerInfo(
    KeyPairParamInfo kppi, string filedata,
    string signername, string projectname)
  {
    var filename = Encoding.UTF8.GetBytes(filedata);
    var err = Native.PqcMlDsaSignFileAot(
      filename, filename.Length,
      kppi.PrivKey.ToBytes(), kppi.PrivKey.Length,
      (byte)NetServicesCrypto.FromMLDsaAlgorithm(kppi.Algo),
      out IntPtr sign_ptr, out int signlength);
    AssertError(err);

    var result = new MlDsaSignerInfo()
    {
      Signature = ToBytes(sign_ptr, signlength),
    };
    Native.FreeBuffer(sign_ptr);

    result.SignerID = kppi.Guid;
    result.PublicKey = kppi.PubKey;

    result.SignerName = Encoding.UTF8.GetBytes(signername);
    result.ProjectName = Encoding.UTF8.GetBytes(projectname);

    result.PqcSignAlgo = (byte)PqcSignAlgo.ML_DSA; //Signer-Algo MlDsa, SlhDsa, lms etc.
    result.PqcSignAlgoParam = (byte)NetServicesCrypto.FromMLDsaAlgorithm(kppi.Algo);

    return result;
  }

  private static Dictionary<Guid, string> CreateMlDsaKeyPairAndSave(
    string[] signernames, string folder)
  {
    if (Directory.Exists(folder))
      Directory.Delete(folder, true);

    if (!Directory.Exists(folder))
      Directory.CreateDirectory(folder);

    var dict = new Dictionary<Guid, string>();
    foreach (var name in signernames)
    {
      var kppi = CreateNativeMlDsaKeyPair();

      var kpfile = Encoding.UTF8.GetBytes(
        $"{folder}\\mldsa_keypair-{name.ToLower()}.key");

      var err = Native.SavePqcMlDsaKeyPairAot(
        kpfile, kpfile.Length,
        kppi.PrivKey.ToBytes(), kppi.PrivKey.Length,
        kppi.PubKey, kppi.PubKey.Length,
        kppi.Guid, kppi.Guid.Length,
        (byte)NetServicesCrypto.FromMLDsaAlgorithm(kppi.Algo), true);
      AssertError(err);

      dict[new Guid(kppi.Guid)] = name;
    }

    return dict;
  }

  private static KeyPairParamInfo[] MlDsaKeyPairLoad(string folder)
  {
    var files = Directory.GetFiles(folder, "*")
      .OrderBy(x => x).ToArray();

    var result = new List<KeyPairParamInfo>(files.Length);
    foreach (var fname in files)
    {
      var filename = Encoding.UTF8.GetBytes(fname);
      var err = Native.LoadPqcMlDsaKeyPairAot(
        filename, filename.Length,
        out IntPtr priv_key_ptr, out int priv_key_length,
        out IntPtr pub_key_ptr, out int pub_key_length,
        out IntPtr guid_id_ptr, out int guid_id_length,
        out byte mldsa_param);
      AssertError(err);

      var kppi = new KeyPairParamInfo
      {
        PrivKey = new UsIPtr<byte>(
          ToBytes(priv_key_ptr, priv_key_length))
      };
      Native.FreeBuffer(priv_key_ptr);

      kppi.PubKey = ToBytes(pub_key_ptr, pub_key_length);
      Native.FreeBuffer(pub_key_ptr);

      kppi.Guid = ToBytes(guid_id_ptr, guid_id_length);
      Native.FreeBuffer(guid_id_ptr);

      kppi.Algo = NetServicesCrypto.
        ToMLDsaAlgorithm((MLDsaParam)mldsa_param);

      result.Add(kppi);
    }

    return [.. result];
  }

  private static unsafe byte** ToNative(byte[][] managed, out int* lengths)
  {
    int count = managed.Length;

    // 1. Array von byte* allokieren
    byte** result = (byte**)NativeMemory.Alloc(
        (nuint)count, (nuint)sizeof(byte*));

    // 2. Array von int für die Längen allokieren
    lengths = (int*)NativeMemory.Alloc(
        (nuint)count, (nuint)sizeof(int));

    // 3. Jedes Element kopieren
    for (var i = 0; i < count; i++)
    {
      var src = managed[i];

      byte* p = (byte*)NativeMemory.Alloc((nuint)src.Length);
      src.CopyTo(new Span<byte>(p, src.Length));

      *(result + i) = p;
      *(lengths + i) = src.Length;
    }

    return result; // und pLengths separat zurückgeben
  }

  private static unsafe byte* ToNative(byte[] managed, int* lengths)
  {
    var buffer = (byte*)NativeMemory.Alloc((nuint)managed.Length);
    managed.CopyTo(new Span<byte>(buffer, managed.Length));
    *lengths = managed.Length;

    return buffer;
  }

  private static MlDsaSignerInfo[] ExtractDataAndSort(
    //IMPORTANT: The array must be sorted.In this case, by signatures
    KeyPairParamInfo[] kppis, Dictionary<Guid, string> dict,
    string srcfile, string projectname) =>
    kppis.Select(
      (k, idx) => ToMlDsaSignerInfo(k, srcfile,
        dict[new Guid(k.Guid)], projectname))
          .OrderBy(x => x.Signature, Comparer<byte[]>.Create((a, b) =>
          {
            int len = Math.Min(a.Length, b.Length);
            for (int i = 0; i < len; i++)
            {
              int diff = a[i].CompareTo(b[i]);
              if (diff != 0)
                return diff;
            }
            return a.Length.CompareTo(b.Length);
          })).ToArray();

  private static MlDsaSignerInfo[] ExtractDataAndSort(
    //IMPORTANT: The array must be sorted.In this case, by signatures
    KeyPairParamInfo[] kppis, Dictionary<Guid, string> dict,
    byte[] message, string projectname) =>
    kppis.Select(
      (k, idx) => ToMlDsaSignerInfo(k, message,
        dict[new Guid(k.Guid)], projectname))
          .OrderBy(x => x.Signature, Comparer<byte[]>.Create((a, b) =>
          {
            int len = Math.Min(a.Length, b.Length);
            for (int i = 0; i < len; i++)
            {
              int diff = a[i].CompareTo(b[i]);
              if (diff != 0)
                return diff;
            }
            return a.Length.CompareTo(b.Length);
          })).ToArray();

}
