

 
using System.Text; 


namespace michele.natale;

partial class NetServicesUtils
{
  public static byte[] ToBase64Utf8(ReadOnlySpan<byte> bytes)
  {
    return Encoding.UTF8.GetBytes(Convert.ToBase64String(bytes));
  }


  public static byte[] FromBase64Utf8(ReadOnlySpan<byte> bytes)
  {
    return Convert.FromBase64String(Encoding.UTF8.GetString(bytes));

  }
} 