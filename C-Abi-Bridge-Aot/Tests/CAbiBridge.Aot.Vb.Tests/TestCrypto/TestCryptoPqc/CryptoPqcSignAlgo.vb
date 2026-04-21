Option Strict On
Option Explicit On


Namespace michele.natale.Tests
  Friend Enum CryptoPqcSignAlgo As Byte
    None = 0

    ML_DSA       'Stateless
    SLH_DSA      'Stateless

    LMS          'Stateful
    XMSS         'Stateful
  End Enum
End Namespace
