

namespace michele.natale;

public partial class NetServices
{
  public const int MIN_PW_SIZE = 10;
  public const int MAX_PW_SIZE = 256;

  public const int MIN_SALT_SIZE = 16;
  public const int MAX_SALT_SIZE = 128;

  public const int MIN_ITERATION = 1234;
  public const int MAX_ITERATION = 1 << 16;
}
