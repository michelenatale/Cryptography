

namespace michele.natale;


public enum PqcSignAlgo : byte
{
  None = 0,

  ML_DSA,       //Stateless
  SLH_DSA,      //Stateless

  LMS,          //Stateful
  XMSS,         //Stateful
}