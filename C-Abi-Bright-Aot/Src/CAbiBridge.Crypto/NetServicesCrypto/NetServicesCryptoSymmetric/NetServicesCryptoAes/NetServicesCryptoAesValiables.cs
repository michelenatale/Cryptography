

namespace michele.natale;

partial class NetServices
{
  //um Timing Attacts entgegenzuwirken.
  private const long TimeSleep = 120; //ms
  private static readonly bool TimeLoc = true;

  public const int AES_IV_SIZE = 16;
  public const int AES_TAG_SIZE = 16;
  public const int AES_KEY_SIZE = 32;
  //public const int AES_NONCE_SIZE = 12;
  public const int AES_MIN_PLAIN_SIZE = 8;
  public const int AES_MAX_PLAIN_SIZE = 1024 * 1024;
}
