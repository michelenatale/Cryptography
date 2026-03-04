

using System.Diagnostics;
using System.Security.Cryptography;

namespace michele.natale;

using Pointers;

partial class NetServices
{
  #region AES File En- & Decryption

  #region AES File En- & Decryption

  //public static void EncryptionFileAes(
  //  string src, string dest, UsIPtr<byte> key,
  //  ReadOnlySpan<byte> associated)
  //{
  //  AssertAesEnc(src, dest, key);
  //  var iv = RngCryptoBytes(AES_IV_SIZE);
  //  var associat = ToAssociated(associated, key);
  //  var buffer = new byte[AES_MAX_PLAIN_SIZE];

  //  var sw = Stopwatch.StartNew();
  //  using var fsin = new FileStream(src, FileMode.Open, FileAccess.Read);
  //  using var fsout = new FileStream(dest, FileMode.Create, FileAccess.ReadWrite);

  //  int readbytes = 0, length = buffer.Length;
  //  fsout.Position = AES_TAG_SIZE + AES_IV_SIZE;
  //  //var cnt = fsin.Length / (AES_MAX_PLAIN_SIZE - 1) + 1;
  //  while ((readbytes = fsin.Read(buffer)) > 0)
  //  {
  //    if (readbytes != length)
  //      Array.Resize(ref buffer, readbytes);

  //    var cipher = EncryptionAesSingle(
  //      buffer, key.ToArray(), iv, associat);

  //    fsout.Write(cipher);
  //    Array.Clear(buffer);
  //  }

  //  var pos = AES_TAG_SIZE + AES_IV_SIZE;
  //  fsout.Position = AES_TAG_SIZE + AES_IV_SIZE;
  //  var tag = ToTagAsync(fsout, key.ToArray(), associat, pos);

  //  fsout.Position = 0; fsout.Write(tag); fsout.Write(iv);
  //  MemoryClear(tag, iv, associat);

  //  var deltatime = (int)(TimeSleep - sw.ElapsedMilliseconds);
  //  if (TimeLoc)
  //    if (deltatime > 0)
  //      Thread.Sleep(deltatime);
  //}

  public static async Task EncryptionFileAesAsync(
    string src, string dest, UsIPtr<byte> key,
    ReadOnlyMemory<byte> associated)
  {
    AssertAesEnc(src, dest, key);

    var iv = RngCryptoBytes(AES_IV_SIZE);
    var associat = ToAssociated(associated, key);
    var buffer = new byte[AES_MAX_PLAIN_SIZE];

    var sw = Stopwatch.StartNew();
    await using var fsin = new FileStream(src, FileMode.Open, FileAccess.Read, FileShare.Read, 8192, true);
    await using var fsout = new FileStream(dest, FileMode.Create, FileAccess.ReadWrite, FileShare.None, 8192, true);

    fsout.Position = AES_TAG_SIZE + AES_IV_SIZE;

    int readbytes;
    while ((readbytes = await fsin.ReadAsync(buffer.AsMemory(0, buffer.Length))) > 0)
    {
      var chunk = buffer.AsSpan(0, readbytes).ToArray();
      var cipher = EncryptionAesSingle(chunk, key.ToArray(), iv, associat);

      await fsout.WriteAsync(cipher.AsMemory(0, cipher.Length));
      Array.Clear(buffer);
    }

    var pos = AES_TAG_SIZE + AES_IV_SIZE;
    fsout.Position = pos;

    var tag = await ToTagAsync(fsout, key.ToArray(), associat, pos);
    fsout.Position = 0;
    await fsout.WriteAsync(tag);
    await fsout.WriteAsync(iv);

    MemoryClear(tag, iv, associat);

    var deltatime = (int)(TimeSleep - sw.ElapsedMilliseconds);
    if (TimeLoc)
      if (deltatime > 0)
        Thread.Sleep(deltatime);
  }



  //public static void DecryptionFileAes(
  //  string src, string dest, UsIPtr<byte> key,
  //  ReadOnlySpan<byte> associated)
  //{
  //  AssertAesDec(src, dest, key);

  //  var associat = ToAssociated(associated, key);
  //  byte[] tag = new byte[AES_TAG_SIZE], iv = new byte[AES_IV_SIZE];
  //  using var fsin = new FileStream(src, FileMode.Open, FileAccess.Read);
  //  using var fsout = new FileStream(dest, FileMode.Create, FileAccess.Write);
  //  var cnt = fsin.Read(tag); cnt += fsin.Read(iv);

  //  try
  //  {
  //    fsin.Position = AES_TAG_SIZE + AES_IV_SIZE;
  //    if (VerifyAsync(fsin, key.ToArray(), tag, associat, cnt))
  //    {
  //      fsin.Position = AES_IV_SIZE + AES_TAG_SIZE;
  //      var buffer = new byte[AES_MAX_PLAIN_SIZE + 16];
  //      int readbytes = 0, length = AES_MAX_PLAIN_SIZE + 16;
  //      while ((readbytes = fsin.Read(buffer)) > 0)
  //      {
  //        if (readbytes != length)
  //          Array.Resize(ref buffer, readbytes);

  //        var decipher = DecryptionAesSingle(
  //          buffer, key.ToArray(), iv, associat);

  //        fsout.Write(decipher);
  //        Array.Clear(buffer);

  //        if (buffer.Length != length)
  //          Array.Resize(ref buffer, length);
  //      }
  //      return;
  //    }
  //  }
  //  catch { MemoryClear(associat, tag, iv); }
  //  finally { MemoryClear(associat, tag, iv); }

  //  throw new CryptographicException($"Verifiy {nameof(DecryptionAes)} failed!");
  //}


  public static async Task DecryptionFileAesAsync(
    string src, string dest, UsIPtr<byte> key,
    ReadOnlyMemory<byte> associated)
  {
    AssertAesDec(src, dest, key);

    var associat = ToAssociated(associated, key);
    byte[] tag = new byte[AES_TAG_SIZE], iv = new byte[AES_IV_SIZE];

    await using var fsin = new FileStream(src, FileMode.Open, FileAccess.Read, FileShare.Read, 8192, true);
    await using var fsout = new FileStream(dest, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true);

    var cnt = await fsin.ReadAsync(tag); cnt += await fsin.ReadAsync(iv);

    try
    {
      fsin.Position = AES_TAG_SIZE + AES_IV_SIZE;

      if (await VerifyAsync(fsin, key.ToArray(), tag, associat, cnt))
      {
        int readbytes;
        fsin.Position = AES_TAG_SIZE + AES_IV_SIZE;
        var buffer = new byte[AES_MAX_PLAIN_SIZE + 16];
        while ((readbytes = await fsin.ReadAsync(buffer.AsMemory(0, buffer.Length))) > 0)
        {
          var chunk = buffer.AsSpan(0, readbytes).ToArray();
          var decipher = DecryptionAesSingle(chunk, key.ToArray(), iv, associat);
          await fsout.WriteAsync(decipher);
          Array.Clear(buffer);
        }
        return;
      }
    }
    catch { MemoryClear(associat, tag, iv); }
    finally { MemoryClear(associat, tag, iv); }

    throw new CryptographicException($"Verify {nameof(DecryptionAes)} failed!");
  }

  #endregion AES File En- & Decryption

  #region AES Transfer File Stream En- & Decryption

  public static async Task EncryptionFileAesAsync(
    FileStream fsin, FileStream fsout,
    int startin, int lengthin, int startout,
    UsIPtr<byte> key, ReadOnlyMemory<byte> associated)
  {
    AssertAesEnc(fsin, fsout, key, startin, lengthin, startout);
    fsin.Position = startin; fsout.Position = startout;

    var iv = RngCryptoBytes(AES_IV_SIZE);
    var associat = ToAssociated(associated, key);
    var buffer = new byte[AES_MAX_PLAIN_SIZE];

    var sw = Stopwatch.StartNew();
    int readbytes, length = buffer.Length;
    fsout.Position = startout + AES_TAG_SIZE + AES_IV_SIZE;
    while ((readbytes = await fsin.ReadAsync(buffer)) > 0)
    {
      if (readbytes != length)
        Array.Resize(ref buffer, readbytes);

      var cipher = EncryptionAesSingle(
        buffer, key.ToArray(), iv, associat);

      await fsout.WriteAsync(cipher);
      Array.Clear(buffer);
    }

    var pos = startout + AES_TAG_SIZE + AES_IV_SIZE;
    fsout.Position = startout + AES_TAG_SIZE + AES_IV_SIZE;
    var tag = await ToTagAsync(fsout, key.ToArray(), associat, pos);

    fsout.Position = startout;
    await fsout.WriteAsync(tag); await fsout.WriteAsync(iv);
    MemoryClear(tag, iv, associat);

    var deltatime = (int)(TimeSleep - sw.ElapsedMilliseconds);
    if (TimeLoc)
      if (deltatime > 0)
        Thread.Sleep(deltatime);
  }

  public static async Task DecryptionFileAesAsync(
    FileStream fsin, FileStream fsout,
    int startin, int lengthin, int startout,
    UsIPtr<byte> key, ReadOnlyMemory<byte> associated)
  {
    AssertAesDec(fsin, fsout, key, startin, lengthin, startout);

    var sw = Stopwatch.StartNew();
    var associat = ToAssociated(associated, key);
    byte[] tag = new byte[AES_TAG_SIZE], iv = new byte[AES_IV_SIZE];
    var cnt = await fsin.ReadAsync(tag); cnt += await fsin.ReadAsync(iv);

    try
    {
      var pos = fsin.Position = startin + AES_TAG_SIZE + AES_IV_SIZE;
      if (await VerifyAsync(fsin, key.ToArray(), tag, associat, (int)pos))
      {
        fsin.Position = pos; fsout.Position = startout;
        var buffer = new byte[AES_MAX_PLAIN_SIZE + 16];
        int readbytes = 0, length = AES_MAX_PLAIN_SIZE + 16;
        while ((readbytes = await fsin.ReadAsync(buffer)) > 0)
        {
          if (readbytes != length)
            Array.Resize(ref buffer, readbytes);

          var decipher = DecryptionAesSingle(
            buffer, key.ToArray(), iv, associat);

          await fsout.WriteAsync(decipher);
          Array.Clear(buffer);

          if (buffer.Length != length)
            Array.Resize(ref buffer, length);
        }

        var deltatime = (int)(TimeSleep - sw.ElapsedMilliseconds);
        if (TimeLoc)
          if (deltatime > 0)
            Thread.Sleep(deltatime);

        return;
      }
    }
    catch { MemoryClear(associat, tag, iv); }
    finally { MemoryClear(associat, tag, iv); }

    throw new CryptographicException($"Verifiy {nameof(DecryptionAes)} failed!");
  }

  #endregion AES Transfer File Stream En- & Decryption

  #endregion AES File En- & Decryption
}
