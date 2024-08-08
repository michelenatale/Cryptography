using System.Security.Cryptography;
using System.Text;


namespace michele.natale.LoginSystems;


using Pointers;
using Services;
using System.Linq;
using static Services.AppServices;

/// <summary>
/// One of the classes that will be used for the registry.
/// </summary>
internal class SecureDataInfo
{
  public const int MPW_SIZE = 32;
  public byte[] EMail { get; set; } = [];
  public byte[] Username { get; set; } = [];
  public UsIPtr<byte> MPw { get; set; } = UsIPtr<byte>.Empty;
  public UsIPtr<byte> Password { get; set; } = UsIPtr<byte>.Empty;

  public SecureDataInfo(UsIPtr<byte> input_serialize) =>
    this.Deserialize(input_serialize);

  public SecureDataInfo(SecureDataInfo input)
    : this(input.Username, input.Password, input.EMail, input.MPw)
  {
  }

  public SecureDataInfo(
    (string uname, UsIPtr<byte> pw, string email, UsIPtr<byte> mpw) input)
    : this(B(input.uname), input.pw, B(input.email), input.mpw)
  {
  }

  public SecureDataInfo(
    byte[] uname, UsIPtr<byte> pw, byte[] email, UsIPtr<byte> mpw) =>
      this.SetDatas(uname, pw, email, mpw);


  public void CreateNewMPw(int size = MPW_SIZE)
  {
    size = size < MPW_SIZE ? MPW_SIZE : size;
    if (!this.MPw.IsEmpty) this.MPw.Dispose();
    var bytes = new byte[size];
    var rand = RandomHolder.Instance;
    rand.FillBytes(bytes);
    this.MPw = new UsIPtr<byte>(bytes);
  }

  public void Reset()
  {
    AppServicesHolder.ClearPrimitives(this.EMail, this.Username);
    this.MPw.Dispose(); this.MPw = UsIPtr<byte>.Empty;
    this.Password.Dispose(); this.Password = UsIPtr<byte>.Empty;
    this.EMail = this.Username = [];
  }

  public UsIPtr<byte> Serialize()
  {
    var data = new byte[][]
    {
      this.Username,this.Password.ToArray(),this.EMail,this.MPw.ToArray(),
    };
    return new(AppServicesHolder.SerializeJson(data));
  }

  public (byte[] UnamePw, byte[] EMailpw) ToHashes(
    HashAlgorithmName hname = default)
  {
    var ats = Encoding.UTF8.GetBytes(['@']);
    var empw = AppServicesHolder.IsNullOrEmpty(this.EMail) ? null : this.EMail?.Concat(ats).Concat(this.Password.ToArray()).ToArray();
    var unpw = AppServicesHolder.IsNullOrEmpty(this.Username) ? null : this.Username?.Concat(ats).Concat(this.Password.ToArray()).ToArray();
    hname = hname == default ? DEFAULT_H_NAME : hname;

    if (AppServicesHolder.IsNullOrEmpty(empw!)) return (AppServicesHolder.HashDataAlgo(unpw, hname), null!);
    if (AppServicesHolder.IsNullOrEmpty(unpw!)) return (null!, AppServicesHolder.HashDataAlgo(empw, hname));
    return (AppServicesHolder.HashDataAlgo(unpw, hname), AppServicesHolder.HashDataAlgo(empw, hname));
  }

  private void Deserialize(UsIPtr<byte> input_serialize)
  {
    var data = AppServicesHolder.DeserializeJson<byte[][]>(input_serialize.ToArray());
    this.EMail = data![2];
    this.Username = data![0];
    this.MPw = new UsIPtr<byte>(data![3]);
    this.Password = new UsIPtr<byte>(data[1]);
  }

  private void SetDatas(
    byte[] uname, UsIPtr<byte> pw,
    byte[] email, UsIPtr<byte> mpw = null!)
  {
    this.EMail = email;
    this.Password = pw;
    this.Username = uname;
    this.MPw = mpw ?? UsIPtr<byte>.Empty;
  }

  private static byte[] B(string str) =>
   string.IsNullOrEmpty(str) ? []
    : Encoding.UTF8.GetBytes(str.ToLower().Trim());

}
