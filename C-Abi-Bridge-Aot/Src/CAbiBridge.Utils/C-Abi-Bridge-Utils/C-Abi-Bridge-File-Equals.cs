


using System.Text;
using System.Security.Cryptography;

namespace michele.natale;

partial class NetServicesUtils
{


  #region Equality File

  public static bool FileEquals(
    ReadOnlySpan<byte> leftfile, ReadOnlySpan<byte> rightfile) =>
     EqualFiles(Encoding.UTF8.GetString(leftfile),
      Encoding.UTF8.GetString(rightfile));


  public static bool EqualFiles(string leftfile, string rightfile)
  {
    using var fsleft = new FileStream(leftfile, FileMode.Open, FileAccess.Read);
    using var fsright = new FileStream(rightfile, FileMode.Open, FileAccess.Read);
    return SHA256.HashData(fsleft).SequenceEqual(SHA256.HashData(fsright));
  }
  #endregion Equality File
}