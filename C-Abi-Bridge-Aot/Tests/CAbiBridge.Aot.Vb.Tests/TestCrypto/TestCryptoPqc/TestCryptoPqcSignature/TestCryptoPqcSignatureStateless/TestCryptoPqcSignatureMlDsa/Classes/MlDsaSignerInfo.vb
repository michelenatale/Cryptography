Option Strict On
Option Explicit On


Namespace michele.natale.Tests
  Friend Class MlDsaSignerInfo
    Public Property SignerID As Byte() = Array.Empty(Of Byte)     'Guid
    Public Property Signature As Byte() = Array.Empty(Of Byte)    'Sign + mldsa-algo

    Public Property PqcSignAlgo As Byte                           'Here mldsa
    Public Property PqcSignAlgoParam As Byte                      'mldsa-algo

    Public Property PublicKey As Byte() = Array.Empty(Of Byte)
    Public Property SignerName As Byte() = Array.Empty(Of Byte)   'Utf8
    Public Property ProjectName As Byte() = Array.Empty(Of Byte)  'Utf8
  End Class
End Namespace

