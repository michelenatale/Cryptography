

Imports TestMyBcPQCVb.michele.natale.BcPqcs
Imports TestMyBcPQCVb.michele.natale.TestBcPqcs

Namespace MyBcPQCTest
  Public Module Program
    Public Sub Main()
      TestBcPqc()
    End Sub

    Private Sub TestBcPqc()
      TestBcCrypto()

      TestBcSignatureStateless()

      TestBcSignatureStateful()
    End Sub

    Private Sub TestBcCrypto()
      TestMLKEM.Start()
    End Sub

    Private Sub TestBcSignatureStateless()
      TestMLDSA.Start()
      TestSLHDSA.Start()
    End Sub

    Private Sub TestBcSignatureStateful()
      TestLMS.Start()
    End Sub
  End Module
End Namespace
