Option Strict On
Option Explicit On
Imports System.Security.Cryptography
Imports michele.natale

Namespace michele.natale.Tests
  Partial Class CryptoPqcMlKemUtilsTest
    Public Shared Function ToMLKemAlgorithm() As MLKemAlgorithm()
      Dim a = MLKemAlgorithm.MLKem512
      Dim b = MLKemAlgorithm.MLKem768
      Dim c = MLKemAlgorithm.MLKem1024

      Return {a, b, c}
    End Function

    Public Shared Function ToMLKemAlgorithm(param As MLKemParam) As MLKemAlgorithm
      Select Case param
        Case MLKemParam.Ml_Kem_512 : Return MLKemAlgorithm.MLKem512
        Case MLKemParam.Ml_Kem_768 : Return MLKemAlgorithm.MLKem768
        Case MLKemParam.Ml_Kem_1024 : Return MLKemAlgorithm.MLKem1024
        Case Else : Throw New Exception()
      End Select
    End Function

    Public Shared Function FromMLKemAlgorithm(parameter As MLKemAlgorithm) As MLKemParam
      ArgumentNullException.ThrowIfNull(parameter)

      If parameter Is MLKemAlgorithm.MLKem512 Then
        Return MLKemParam.Ml_Kem_512
      ElseIf parameter Is MLKemAlgorithm.MLKem768 Then
        Return MLKemParam.Ml_Kem_768
      ElseIf parameter Is MLKemAlgorithm.MLKem1024 Then
        Return MLKemParam.Ml_Kem_1024
      End If

      Throw New ArgumentOutOfRangeException(NameOf(parameter), $"{NameOf(parameter)} has failded!")
    End Function
  End Class
End Namespace
