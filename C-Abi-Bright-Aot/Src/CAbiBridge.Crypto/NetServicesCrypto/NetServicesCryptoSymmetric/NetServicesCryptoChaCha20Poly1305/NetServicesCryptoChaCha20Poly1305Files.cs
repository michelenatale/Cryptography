


using System.Security.Cryptography;

namespace michele.natale;

using Pointers;

public partial class NetServices
{ 

  public static async Task EncryptionFileChaCha20Poly1305Async(
    string src, string dest, UsIPtr<byte> key,
    ReadOnlyMemory<byte> associated)
  {
    AssertChaChaPolyEnc(src, dest, key);
    var associat = ToAssociated(associated, key);
    var buffer = new byte[CHACHA_POLY_MAX_PLAIN_SIZE];

    using var fsin = new FileStream(src, FileMode.Open, FileAccess.Read, FileShare.Read, 8192, useAsync: true);
    using var fsout = new FileStream(dest, FileMode.Create, FileAccess.ReadWrite, FileShare.None, 8192, useAsync: true);

    int readbytes;
    fsout.Position = 0;
    while ((readbytes = await fsin.ReadAsync(buffer.AsMemory(0, buffer.Length))) > 0)
    {
      var memo = buffer.AsMemory(0, readbytes);

      //es ist kein async-await für EncChaCha20Poly1305Single
      //nötig, da diese Methode cpu-komform ist.
      var cipher = EncChaCha20Poly1305Single(
          memo.Span, key, associat, out var tag, out var nonce);

      await fsout.WriteAsync(tag);
      await fsout.WriteAsync(nonce);
      await fsout.WriteAsync(cipher);

      memo.Span.Clear();
    }
  }

  public static async Task DecryptionFileChaCha20Poly1305Async(
      string src, string dest, UsIPtr<byte> key,
      ReadOnlyMemory<byte> associated)
  {
    AssertChaChaPolyDec(src, dest, key);

    var associat = ToAssociated(associated, key);
    await using var fsin = new FileStream(src, FileMode.Open, FileAccess.Read, FileShare.Read, 8192, useAsync: true);
    await using var fsout = new FileStream(dest, FileMode.Create, FileAccess.ReadWrite, FileShare.None, 8192, useAsync: true);

    var headerSize = CHACHA_POLY_TAG_SIZE + CHACHA_POLY_NONCE_SIZE;
    var buffer = new byte[headerSize + CHACHA_POLY_MAX_PLAIN_SIZE].AsMemory();

    while (true)
    {
      var read = await fsin.ReadAsync(buffer);
      if (read == 0) break;

      var memo = buffer[..read];
      if (memo.Length < headerSize)
        throw new CryptographicException($"{nameof(DecryptionChaCha20Poly1305)} failed (truncated).");

      var tag = memo[..CHACHA_POLY_TAG_SIZE];
      var nonce = memo.Slice(CHACHA_POLY_TAG_SIZE, CHACHA_POLY_NONCE_SIZE);
      var cipher = memo[headerSize..];

      //es ist kein async-await für DecChaCha20Poly1305Single
      //nöting, da diese Methode cpu-komform ist.
      var decipher = DecChaCha20Poly1305Single(cipher.Span, key, associat, tag.Span, nonce.Span);
      await fsout.WriteAsync(decipher);

      memo.Span.Clear();
    }
  }

   

  public static async Task EncryptionFileChaCha20Poly1305Async(
    FileStream fsin, FileStream fsout,
    int startin, int lengthin, int startout,
    UsIPtr<byte> key, ReadOnlyMemory<byte> associated)
  {
    AssertChaChaPolyEnc(fsin, fsout, key, startin, lengthin, startout);
    var associat = ToAssociated(associated, key);
    var buffer = new byte[CHACHA_POLY_MAX_PLAIN_SIZE];

    fsin.Position = startin;
    fsout.Position = startout;

    int readbytes;
    while ((readbytes = await fsin.ReadAsync(buffer.AsMemory(0, buffer.Length))) > 0)
    {
      var memo = buffer.AsMemory(0, readbytes);

      //es ist kein async-await für EncChaCha20Poly1305Single
      //nötig, da diese Methode cpu-komform ist.
      var cipher = EncChaCha20Poly1305Single(
          memo.Span, key, associat, out var tag, out var nonce);

      await fsout.WriteAsync(tag);
      await fsout.WriteAsync(nonce);
      await fsout.WriteAsync(cipher);

      memo.Span.Clear();
    }
  }

  public static async Task DecryptionFileChaCha20Poly1305Async(
      FileStream fsin, FileStream fsout,
      int startin, int lengthin, int startout,
      UsIPtr<byte> key, ReadOnlyMemory<byte> associated)
  {
    AssertChaChaPolyDec(fsin, fsout, key, startin, lengthin, startout);
    var associat = ToAssociated(associated, key);

    var headerSize = CHACHA_POLY_TAG_SIZE + CHACHA_POLY_NONCE_SIZE;
    var buffer = new byte[headerSize + CHACHA_POLY_MAX_PLAIN_SIZE].AsMemory();

    fsin.Position = startin;
    fsout.Position = startout;

    while (true)
    {
      var read = await fsin.ReadAsync(buffer);
      if (read == 0)
        break;

      var span = buffer[..read];
      if (span.Length < headerSize)
        throw new CryptographicException($"{nameof(DecryptionChaCha20Poly1305)} failed (truncated).");

      var tag = span[..CHACHA_POLY_TAG_SIZE];
      var nonce = span.Slice(CHACHA_POLY_TAG_SIZE, CHACHA_POLY_NONCE_SIZE);
      var cipher = span[headerSize..];

      //es ist kein async-await für DecChaCha20Poly1305Single, da
      //diese Methode cpu-komform ist.
      var decipher = DecChaCha20Poly1305Single(cipher.Span, key, associat, tag.Span, nonce.Span);
      await fsout.WriteAsync(decipher);

      span.Span.Clear();
    }
  }
}
