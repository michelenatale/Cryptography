namespace michele.natale.LoginSystems.Services;

partial class AppServices
{

  /// <summary>
  /// Returns the current working Directory.
  /// </summary>
  public string ToCurrentFolder => Directory.GetCurrentDirectory();

  /// <summary>
  /// Load data from File
  /// </summary>
  /// <param name="filename">Filename</param>
  /// <returns>Data as string</returns>
  public string LoadFromFile(string filename)
  {
    using var fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
    using var sr = new StreamReader(fs);
    return sr.ReadToEnd();
  }

  /// <summary>
  /// Load data from File
  /// </summary>
  /// <param name="filename">Filename</param>
  /// <returns>Data as Array of byte</returns>
  public byte[] LoadFromFileBytesUtf8(string filename)
  {
    using var ms = new MemoryStream();
    using var fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
    fs.CopyTo(ms);
    ms.Position = 0;
    return ms.ToArray();
  }

  /// <summary>
  /// Save data in a File
  /// </summary>
  /// <param name="filename">Filename</param>
  /// <param name="data">Data</param>
  public void SaveToFile(string filename, string data)
  {

    using FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.Write);
    using var sw = new StreamWriter(fs);
    sw.Write(data);
  }

  /// <summary>
  /// Save Data in a File
  /// </summary>
  /// <param name="filename">Filename</param>
  /// <param name="data">Data</param>
  public void SaveToFileBytesUtf8(string filename, byte[] data)
  {
    using var ms = new MemoryStream(data);
    using FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.Write);
    ms.CopyTo(fs);
  }

  /// <summary>
  /// Create a new Folder.
  /// </summary>
  /// <param name="folder">Path of folder</param>
  public static void CreateFolder(string folder)
  {
    if (Directory.Exists(folder)) return;
    Directory.CreateDirectory(folder);
  }

  /// <summary>
  /// Delete a File into a folder
  /// </summary>
  /// <param name="path_folder_file">Path of the file</param>
  public void DeleteFolderFile(string path_folder_file)
  {
    if (!File.Exists(path_folder_file)) return;
    File.Delete(path_folder_file);
  }
}
