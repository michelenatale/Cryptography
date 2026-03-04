

using System.Security.Cryptography;

namespace michele.natale;

using Pointers;
public partial class NetServices
{
  //public static void EncryptionFileAesGcm(
  //string src, string dest,
  //UsIPtr<byte> key, ReadOnlySpan<byte> associated)
  //{
  //  AssertAesGcmEnc(src, dest, key);
  //  var associat = ToAssociated(associated, key);
  //  var buffer = new byte[AES_GCM_MAX_PLAIN_SIZE];

  //  using var fsin = new FileStream(src, FileMode.Open, FileAccess.Read);
  //  using var fsout = new FileStream(dest, FileMode.Create, FileAccess.ReadWrite);

  //  fsout.Position = 0;
  //  int readbytes = 0, length = buffer.Length;
  //  while ((readbytes = fsin.Read(buffer)) > 0)
  //  {
  //    if (readbytes != length)
  //      Array.Resize(ref buffer, readbytes);

  //    var cipher = EncAesGcmSingle(
  //      buffer, key, associat, out var tag, out var nonce);

  //    fsout.Write(tag);
  //    fsout.Write(nonce);
  //    fsout.Write(cipher);
  //    Array.Clear(buffer);
  //  }
  //}

  //public static void DecryptionFileAesGcm(
  //  string src, string dest,
  //  UsIPtr<byte> key, ReadOnlySpan<byte> associated)
  //{
  //  AssertAesGcmDec(src, dest, key);

  //  var associat = ToAssociated(associated, key);
  //  using var fsin = new FileStream(src, FileMode.Open, FileAccess.Read);
  //  using var fsout = new FileStream(dest, FileMode.Create, FileAccess.ReadWrite);

  //  try
  //  {
  //    int readbytes = 0; fsin.Position = 0;
  //    var start = AES_GCM_TAG_SIZE + AES_GCM_NONCE_SIZE;
  //    var buffer = new byte[start + AES_GCM_MAX_PLAIN_SIZE].AsSpan();
  //    while ((readbytes = fsin.Read(buffer)) > 0)
  //    {
  //      var decipher = DecAesGcmSingle(
  //        buffer[start..readbytes], key, associat,
  //        buffer[..AES_GCM_TAG_SIZE],
  //        buffer.Slice(AES_GCM_TAG_SIZE, AES_GCM_NONCE_SIZE));

  //      fsout.Write(decipher);
  //      buffer.Clear();
  //    }
  //    return;
  //  }
  //  catch { }

  //  throw new CryptographicException($"Verifiy {nameof(DecryptionAes)} failed!");
  //}

  public static async Task EncryptionFileAesGcmAsync(
    string src, string dest, UsIPtr<byte> key, 
    ReadOnlyMemory<byte> associated)
  {
    AssertAesGcmEnc(src, dest, key);
    var associat = ToAssociated(associated, key);
    var buffer = new byte[AES_GCM_MAX_PLAIN_SIZE];

    await using var fsin = new FileStream(src, FileMode.Open, FileAccess.Read, FileShare.Read, 8192, useAsync: true);
    await using var fsout = new FileStream(dest, FileMode.Create, FileAccess.ReadWrite, FileShare.None, 8192, useAsync: true);

    int readbytes;
    fsout.Position = 0;
    while ((readbytes = await fsin.ReadAsync(buffer.AsMemory(0, buffer.Length))) > 0)
    {
      if (readbytes != buffer.Length)
        buffer = buffer[..readbytes].ToArray();

      var cipher = EncAesGcmSingle(buffer.AsSpan(0, readbytes), key, associat, out var tag, out var nonce);

      await fsout.WriteAsync(tag);
      await fsout.WriteAsync(nonce);
      await fsout.WriteAsync(cipher);

      Array.Clear(buffer);
    }
  }

  public static async Task DecryptionFileAesGcmAsync(
      string src, string dest,
      UsIPtr<byte> key, ReadOnlyMemory<byte> associated)
  {
    AssertAesGcmDec(src, dest, key);

    var associat = ToAssociated(associated, key);
    await using var fsin = new FileStream(src, FileMode.Open, FileAccess.Read, FileShare.Read, 8192, useAsync: true);
    await using var fsout = new FileStream(dest, FileMode.Create, FileAccess.ReadWrite, FileShare.None, 8192, useAsync: true);

    var headerSize = AES_GCM_TAG_SIZE + AES_GCM_NONCE_SIZE;
    var buffer = new byte[headerSize + AES_GCM_MAX_PLAIN_SIZE].AsMemory();

    while (true)
    {
      var read = await fsin.ReadAsync(buffer);
      if (read == 0)
        break;

      var memo = buffer[..read];

      if (memo.Length < headerSize)
        throw new CryptographicException($"{nameof(DecryptionAesGcm)} failed (truncated).");

      var tag = memo[..AES_GCM_TAG_SIZE];
      var nonce = memo.Slice(AES_GCM_TAG_SIZE, AES_GCM_NONCE_SIZE);
      var cipher = memo[headerSize..];

      var decipher = DecAesGcmSingle(cipher.Span, key, associat, tag.Span, nonce.Span);
      await fsout.WriteAsync(decipher);
      memo.Span.Clear();
    }
  }



  //public static void EncryptionFileAesGcm(
  //  FileStream fsin, FileStream fsout,
  //  int startin, int lengthin, int startout,
  //  UsIPtr<byte> key, ReadOnlySpan<byte> associated)
  //{
  //  AssertAesGcmEnc(fsin, fsout, key, startin, lengthin, startout);
  //  var associat = ToAssociated(associated, key);
  //  var buffer = new byte[AES_GCM_MAX_PLAIN_SIZE];

  //  int readbytes, length = buffer.Length;
  //  fsin.Position = startin; fsout.Position = startout;
  //  while ((readbytes = fsin.Read(buffer)) > 0)
  //  {
  //    if (readbytes != length)
  //      Array.Resize(ref buffer, readbytes);

  //    var cipher = EncAesGcmSingle(
  //      buffer, key, associat, out var tag, out var nonce);

  //    fsout.Write(tag);
  //    fsout.Write(nonce);
  //    fsout.Write(cipher);
  //    Array.Clear(buffer);
  //  }
  //}

  //public static void DecryptionFileAesGcm(
  //  FileStream fsin, FileStream fsout,
  //  int startin, int lengthin, int startout,
  //  UsIPtr<byte> key, ReadOnlySpan<byte> associated)
  //{
  //  AssertAesGcmDec(fsin, fsout, key, startin, lengthin, startout);

  //  var associat = ToAssociated(associated, key);

  //  try
  //  {
  //    var start = AES_GCM_TAG_SIZE + AES_GCM_NONCE_SIZE;
  //    var buffer = new byte[start + AES_GCM_MAX_PLAIN_SIZE].AsSpan();
  //    int readbytes = 0; fsin.Position = startin; fsout.Position = startout;
  //    while ((readbytes = fsin.Read(buffer)) > 0)
  //    {
  //      var decipher = DecAesGcmSingle(
  //        buffer[start..readbytes], key, associat,
  //        buffer[..AES_GCM_TAG_SIZE],
  //        buffer.Slice(AES_GCM_TAG_SIZE, AES_GCM_NONCE_SIZE));

  //      fsout.Write(decipher);
  //      buffer.Clear();
  //    }
  //    return;
  //  }
  //  catch { }

  //  throw new CryptographicException($"Verifiy {nameof(DecryptionAes)} failed!");
  //}

  public static async Task EncryptionFileAesGcmAsync(
    FileStream fsin, FileStream fsout,
    int startin, int lengthin, int startout,
    UsIPtr<byte> key, ReadOnlyMemory<byte> associated)
  {
    AssertAesGcmEnc(fsin, fsout, key, startin, lengthin, startout);
    var associat = ToAssociated(associated, key);
    var buffer = new byte[AES_GCM_MAX_PLAIN_SIZE];

    fsin.Position = startin;
    fsout.Position = startout;

    int readbytes;
    while ((readbytes = await fsin.ReadAsync(buffer.AsMemory(0, buffer.Length))) > 0)
    {
      if (readbytes != buffer.Length)
        buffer = buffer[..readbytes].ToArray();

      var cipher = EncAesGcmSingle(buffer.AsSpan(0, readbytes), key, associat, out var tag, out var nonce);

      await fsout.WriteAsync(tag);
      await fsout.WriteAsync(nonce);
      await fsout.WriteAsync(cipher);

      Array.Clear(buffer);
    }
  }

  public static async Task DecryptionFileAesGcmAsync(
      FileStream fsin, FileStream fsout,
      int startin, int lengthin, int startout,
      UsIPtr<byte> key, ReadOnlyMemory<byte> associated)
  {
    AssertAesGcmDec(fsin, fsout, key, startin, lengthin, startout);
    var associat = ToAssociated(associated, key);

    var headerSize = AES_GCM_TAG_SIZE + AES_GCM_NONCE_SIZE;
    var buffer = new byte[headerSize + AES_GCM_MAX_PLAIN_SIZE].AsMemory();

    fsin.Position = startin;
    fsout.Position = startout;

    while (true)
    {
      var read = await fsin.ReadAsync(buffer);
      if (read == 0)
        break;

      var memo = buffer[..read];

      if (memo.Length < headerSize)
        throw new CryptographicException($"{nameof(DecryptionAesGcm)} failed (truncated).");

      var tag = memo[..AES_GCM_TAG_SIZE];
      var nonce = memo.Slice(AES_GCM_TAG_SIZE, AES_GCM_NONCE_SIZE);
      var cipher = memo[headerSize..];

      var decipher = DecAesGcmSingle(cipher.Span, key, associat, tag.Span, nonce.Span);
      await fsout.WriteAsync(decipher);

      memo.Span.Clear();
    }
  }
}
