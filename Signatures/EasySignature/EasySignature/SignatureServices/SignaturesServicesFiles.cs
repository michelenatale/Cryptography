﻿

namespace michele.natale.Cryptography.Signatures.Services;


partial class SignatureServices
{

  /// <summary>
  /// Returns the current working directory.
  /// </summary>
  public static string ToCurrentFolder => Directory.GetCurrentDirectory();

  //public static string LoadFromFile(string filename)
  //{
  //  using var fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
  //  using var sr = new StreamReader(fs);
  //  return sr.ReadToEnd();
  //}

  /// <summary>
  /// Returns the data of a file.
  /// </summary>
  /// <param name="filename">Desired Filename</param>
  /// <returns>Array of byte</returns>
  public static byte[] LoadFromFileBytesUtf8(string filename)
  {
    using var ms = new MemoryStream();
    using var fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
    fs.CopyTo(ms);
    ms.Position = 0;
    return ms.ToArray();
  }

  //public static void SaveToFile(string filename, string data)
  //{

  //  using FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.Write);
  //  using var sw = new StreamWriter(fs);
  //  sw.Write(data);
  //}

  /// <summary>
  /// Saves the data to a file.
  /// </summary>
  /// <param name="filename">Desired filename</param>
  /// <param name="data">Desired Data as array of byte</param>
  public static void SaveToFileBytesUtf8(string filename, byte[] data)
  {
    using var ms = new MemoryStream(data);
    using FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.Write);
    ms.CopyTo(fs);
  }
}