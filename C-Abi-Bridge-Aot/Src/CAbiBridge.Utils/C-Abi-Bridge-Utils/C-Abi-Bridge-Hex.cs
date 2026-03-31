


using System.Text;

namespace michele.natale;

partial class NetServicesUtils
{
  public static byte[] ToHexUtf8(ReadOnlySpan<byte> bytes, bool lower)
  {
    if(lower)
      return Encoding.UTF8.GetBytes(Convert.ToHexStringLower(bytes));

    return Encoding.UTF8.GetBytes(Convert.ToHexString(bytes));
  }


  public static byte[] FromHexUtf8(ReadOnlySpan<byte> bytes)
  {
    return Convert.FromHexString(Encoding.UTF8.GetString(bytes));

  }
}