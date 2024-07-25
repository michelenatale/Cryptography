

namespace michele.natale.EcCurveDsaDh;

partial class EcService
{
  public static string ToCurrentFolder => Directory.GetCurrentDirectory();

  public static string LoadFromFile(string filename)
  {
    using var fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
    using var sr = new StreamReader(fs);
    return sr.ReadToEnd();
  }

  public static byte[] LoadFromFileBytesUtf8(string filename)
  {
    using var ms = new MemoryStream();
    using var fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
    fs.CopyTo(ms);
    ms.Position = 0;
    return ms.ToArray();
  }

  public static void SaveToFile(string filename, string data)
  {

    using FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.Write);
    using var sw = new StreamWriter(fs);
    sw.Write(data);
  }

  public static void SaveToFileBytesUtf8(string filename, byte[] data)
  {
    using var ms = new MemoryStream(data);
    using FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.Write);
    ms.CopyTo(fs);
  }


}
