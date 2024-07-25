


namespace michele.natale.EcCurveDsaDhTest;

using michele.natale.EcCurveDsaDh;

public class EcConfiguration
{

  public static void StartEcConfig()
  {
    CreateConfigInfos();
  }

  public static void ResetEcConfig()
  {
    DeleteConfigInfos();
  }


  private static string CombineDirFile(
    string folder, string filename) =>
    Path.Combine(folder, filename);

  private static bool FolderExist(string folder) =>
    Directory.Exists(folder);

  private static void DeleteFolder(
    string folder, bool recursiv = true)
  {
    if (FolderExist(folder))
      Directory.Delete(folder, recursiv);
  }

  private static void CreateFolder(string folder)
  {
    if (!FolderExist(folder))
      Directory.CreateDirectory(folder);
  }

  private static void CreateConfigInfos()
  {
    CreateFolder(EcSettings.ToEcCurrentFolder);
    foreach (var mf in EcSettings.EcMasterFileName())
    {
      if (File.Exists(mf)) continue;
      var dir = Path.GetDirectoryName(mf);
      if (!Directory.Exists(dir))
        CreateFolder(dir!);
      CreateNewMasterFile(mf);
    }
  }

  private static void DeleteConfigInfos() =>
    DeleteFolder(EcSettings.ToEcCurrentFolder, true);

  private static void CreateNewMasterFile(string filename)
  {
    if (!File.Exists(filename))
    {
      var mpw = RngCryptoBytes(64, 128);
      var (h, f) = PMEI.EcMasterKeyPmeiHF();
      PMEI.SavePmeiToFile(filename, mpw, h, f);
    }
  }

  private static byte[] RngCryptoBytes(int minsize, int maxsize)
  {
    var sz = EcService.NextCryptoInt32(minsize, maxsize);
    return EcService.RngCryptoBytes(sz);
  }


}
