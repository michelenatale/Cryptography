


namespace michele.natale.EcCurveDsaDh;


public class EcSettings
{
  public const string EcMasterFile = "000000000";

  public const string EcCurrentFolderName = "EcParameters";

  public static string[] EcUsers { get; set; } = Array.Empty<string>();

  public static string CurrentFolder =>
    Directory.GetCurrentDirectory();
  public static string ToEcCurrentFolder =>
    Path.Combine(CurrentFolder, EcCurrentFolderName);
  public static string ToEcCurrentFolderUser(string user) =>
      Path.Combine(CurrentFolder, EcCurrentFolderName, user);

  public static string[] EcMasterFileName()
  {
    var result = Enumerable.Range(0, EcUsers.Length)
        .Select(str => string.Empty).ToArray();
    if (result != Array.Empty<string>())
    {
      for (var i = 0; i < result.Length; i++)
      {
        var folder = Path.Combine(ToEcCurrentFolder, EcUsers[i]);
        result[i] = Path.Combine(folder, EcMasterFile);
      }
      return result;
    }
    return [Path.Combine(ToEcCurrentFolder, EcMasterFile)];
  }

  public static byte[][] ToMasterKey()
  {
    var (h, f) = PMEI.EcMasterKeyPmeiHF();
    var result = new List<byte[]>();

    foreach (var mpwf in EcMasterFileName())
      result.Add(PMEI.LoadPmeiFromFile(mpwf, h, f).Message);
    return [.. result];
  }

  public static byte[] ToMasterKey(string username)
  {
    //var result = new List<byte[]>();
    var (h, f) = PMEI.EcMasterKeyPmeiHF();
    if (ToUserNameFromMasterPath().Contains(username))
    {
      foreach (var mpwf in EcMasterFileName())
      {
        var l1 = mpwf.Length;
        var s1 = mpwf.LastIndexOf('\\', l1 - 1);
        var s2 = mpwf.LastIndexOf('\\', s1 - 1);
        var un = mpwf.Substring(s2 + 1, s1 - s2 - 1);
        if (un == username)
          return PMEI.LoadPmeiFromFile(mpwf, h, f).Message;
      }
    }
    throw new Exception();
  }

  private static string[] ToUserNameFromMasterPath()
  {
    var result = new List<string>();
    var idxs = new List<List<int>>();
    foreach (var mpwf in EcMasterFileName())
    {
      var list = new List<int>() { mpwf.Length - 1 };
      for (var i = 0; i < 2; i++)
        list.Add(mpwf.LastIndexOf('\\', list[i] - 1));
      idxs.Add(list);
      result.Add(mpwf.Substring(list[^1] + 1, list[^2] - list[^1] - 1));
    }
    return result.ToArray();
  }

}
