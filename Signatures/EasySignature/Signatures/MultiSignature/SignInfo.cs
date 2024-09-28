



namespace michele.natale.Signatures;

using SignatureServices;


public class SignInfo
{
  public Guid ID { get; set; } = Guid.Empty;
  public string Name { get; set; } = string.Empty;
  public string Sign { get; set; } = string.Empty;
  public string Seed { get; set; } = string.Empty;
  public string Message { get; set; } = string.Empty;
  public string PublicKey { get; set; } = string.Empty;

  public SignInfo(
    string name, byte[] seed, string msg,
    byte[] pupkey, byte[] sign)
  {
    this.Message = msg;
    this.ID = Guid.NewGuid();
    this.Sign = Convert.ToHexString(sign);
    this.Seed = Convert.ToHexString(seed);
    this.PublicKey = Convert.ToHexString(pupkey);
    this.Name = name ?? SignaturesServices.ToRngName();
  }


  public MultiSignInfo ToMultiSignInfo()
  {
    return new MultiSignInfo
    {
      ID = ID,
      Name = Name,
      Sign = Sign,
      Message = Message,
      PublicKey = PublicKey,
    };
  }

}
