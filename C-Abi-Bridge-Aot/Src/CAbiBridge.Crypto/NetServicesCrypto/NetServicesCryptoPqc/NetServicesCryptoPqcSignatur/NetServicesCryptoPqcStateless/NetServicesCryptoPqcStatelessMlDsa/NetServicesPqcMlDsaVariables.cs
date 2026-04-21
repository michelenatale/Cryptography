//ML-DSA
//Module-Lattice-Based
//FIPS PUB 204 
//https://nvlpubs.nist.gov/nistpubs/fips/nist.fips.204.pdf
 


namespace michele.natale;


partial class NetServicesCrypto
{
  public const int ML_DSA_MIN_PLAIN_SIZE = 8;
  public const int ML_DSA_MAX_PLAIN_SIZE = 1024 * 1024;
  public const long ML_DSA_MAX_FILE_SIZE_CHANCE = 1 << 26; //64 MB
}