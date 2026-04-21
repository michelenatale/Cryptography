
 

namespace michele.natale.Tests;


internal class MlDsaSignerInfo
{
  public byte[] SignerID
  {
    get; set; //Guid
  } = [];

  public byte[] Signature
  {
    get; set; //Sign + mldsa-algo
  } = [];

  public byte PqcSignAlgo //mldsa
  {
    get; set;
  } 

  public byte PqcSignAlgoParam // mldsa-algo
  {
    get; set;
  } 

  public byte[] PublicKey
  {
    get; set;
  } = [];

  public byte[] SignerName //Utf8
  {
    get; set;
  } = [];

  public byte[] ProjectName //Utf8
  {
    get; set;
  } = [];
}
