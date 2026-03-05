

using System.Security.Cryptography;

namespace michele.natale;

using Pointers;
public partial class NetServices
{ 

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
