


using System.Text;

namespace michele.natale.EcCurveDsaDh;

public class EcMessagePackage
{
  public required string SenderPublicKeyPmei
  {
    get; init;
  }
  public required string Cipher
  {
    get; init;
  }
  public required string Signature
  {
    get; init;
  }
  public required string EcCryptionAlgo
  {
    get; init;
  }

  public override string ToString()
  {
    var sb = new StringBuilder();
    sb.AppendLine($"Cipher: {this.Cipher}");
    sb.AppendLine($"Signature: {this.Signature}");
    sb.AppendLine($"EcCryptionAlgo: {this.EcCryptionAlgo}");
    sb.AppendLine($"SenderPublicKeyPmei: {SenderPublicKeyPmei}");
    return sb.ToString();
  }
}
