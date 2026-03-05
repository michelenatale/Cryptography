


namespace michele.natale;


using Pointers;

public partial class NetServices
{
  private static void AssertChaChaPolyEnc(
    ReadOnlySpan<byte> bytes, UsIPtr<byte> key)
  {
    var length = bytes.Length;
    if (length > CHACHA_POLY_MAX_PLAIN_SIZE) throw new ArgumentOutOfRangeException(nameof(bytes));
    if (length < CHACHA_POLY_MIN_PLAIN_SIZE) throw new ArgumentOutOfRangeException(nameof(bytes));

    length = key.Length;
    if (length != CHACHA_POLY_MIN_KEY_SIZE && length != CHACHA_POLY_MID_KEY_SIZE && length != CHACHA_POLY_MAX_KEY_SIZE)
      throw new ArgumentOutOfRangeException(nameof(key));
  }

  private static void AssertChaChaPolyDec(UsIPtr<byte> key)
  {
    var length = key.Length;
    if (length != CHACHA_POLY_MIN_KEY_SIZE && length != CHACHA_POLY_MID_KEY_SIZE && length != CHACHA_POLY_MAX_KEY_SIZE)
      throw new ArgumentOutOfRangeException(nameof(key));
  }

  private static void AssertChaChaPolyEnc(
    string src, string dest, UsIPtr<byte> key)
  {
    if (!File.Exists(src))
      throw new FileNotFoundException(nameof(src));

    if (File.Exists(dest))
      File.Delete(dest);

    if (key.Length != CHACHA_POLY_MAX_KEY_SIZE)
      throw new ArgumentOutOfRangeException(nameof(key));

  }

  private static void AssertChaChaPolyDec(
    string src, string dest, UsIPtr<byte> key)
  {
    AssertChaChaPolyDec(key);

    if (!File.Exists(src))
      throw new FileNotFoundException(nameof(src));

    if (File.Exists(dest))
      File.Delete(dest);
  }

  private static void AssertChaChaPolyEnc(
     FileStream fsin, FileStream fsout, UsIPtr<byte> key,
     int startin, int lengthin, int startout)
  {
    if (key.Length != CHACHA_POLY_MAX_KEY_SIZE)
      throw new ArgumentOutOfRangeException(nameof(key));

    ArgumentOutOfRangeException.ThrowIfNegative(startout);

    if (startin + lengthin < fsin.Length)
      throw new ArgumentOutOfRangeException(nameof(startin));

    if (!fsin.CanRead)
      throw new ArgumentException(
        $"Stream must CanRead, has failed!", nameof(fsin));

    if (!fsout.CanRead) throw new ArgumentException(
        $"Stream must CanRead, has failed!", nameof(fsout));

    if (!fsout.CanWrite) throw new ArgumentException(
        $"Stream must Canwrite, has failed!", nameof(fsout));
  }

  private static void AssertChaChaPolyDec(
     FileStream fsin, FileStream fsout, UsIPtr<byte> key,
     int startin, int lengthin, int startout)
  {
    if (key.Length != CHACHA_POLY_MAX_KEY_SIZE)
      throw new ArgumentOutOfRangeException(nameof(key));

    ArgumentOutOfRangeException.ThrowIfNegative(startout);

    if (startin + lengthin < fsin.Length)
      throw new ArgumentOutOfRangeException(nameof(startin));

    if (!fsin.CanRead)
      throw new ArgumentException(
        $"Stream must CanRead, has failed!", nameof(fsin));

    //if (!fsout.CanRead) throw new ArgumentException(
    //    $"Stream must CanRead, has failed!", nameof(fsout));

    if (!fsout.CanWrite) throw new ArgumentException(
      $"Stream must Canwrite, has failed!", nameof(fsout));
  }
}
