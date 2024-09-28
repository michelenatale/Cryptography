




namespace michele.natale.Signatures;

using SignatureServices;

public class MultiSignInfo
{
  public Guid ID { get; set; } = Guid.Empty;
  public string Name { get; set; } = string.Empty;
  public string Sign { get; set; } = string.Empty;
  public string Message { get; set; } = string.Empty;
  public string PublicKey { get; set; } = string.Empty;

  public MultiSignInfo()
  {
  }

  public MultiSignInfo(
    string name, string msg,
    byte[] pupkey, byte[] sign)
  {
    this.Message = msg;
    this.ID = Guid.NewGuid();
    this.Sign = Convert.ToHexString(sign);
    this.PublicKey = Convert.ToHexString(pupkey);
    this.Name = name ?? SignaturesServices.ToRngName();
  }


}
