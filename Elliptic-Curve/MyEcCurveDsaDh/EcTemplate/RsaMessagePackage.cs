


using System.Text;

namespace michele.natale.EcCurveDsaDh;

public class RsaMessagePackage
{
  public required string SenderPublicKey
  {
    get; init;
  }
  public required string CipherMessage
  {
    get; init;
  }
  public required string CipherSharedKey
  {
    get; init;
  }
  public required string Signature
  {
    get; init;
  }
  public required string RsaCryptionAlgo
  {
    get; init;
  }
  public required string Index
  {
    get; init;
  }


  public override string ToString()
  {
    var sb = new StringBuilder();
    sb.AppendLine($"Signature: {this.Index}");
    sb.AppendLine($"Signature: {this.Signature}");
    sb.AppendLine($"CipherMessage: {this.CipherMessage}");
    sb.AppendLine($"CipherSharedKey: {this.CipherSharedKey}");
    sb.AppendLine($"EcCryptionAlgo: {this.RsaCryptionAlgo}");
    sb.AppendLine($"SenderPublicKeyPmei: {SenderPublicKey}");
    return sb.ToString();
  }
}
