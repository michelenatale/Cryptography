Option Strict On
Option Explicit On



Imports TestMyMsPQCVb.michele.natale.MsPqcs
Imports TestMyMsPQCVb.michele.natale.TestMsPqcs

Namespace MyMsPQCTest

  Public Module Program

    Public Sub Main()
      TestBcPqc().GetAwaiter().GetResult()
    End Sub

    Private Async Function TestBcPqc() As Task
      TestBcCrypto()

      Await TestBcSignatureStateless()
    End Function

    Private Sub TestBcCrypto()
      TestMLKEM.Start()
    End Sub

    Private Async Function TestBcSignatureStateless() As Task
      Await TestMLDSA.Start()
      'TestSLHDSA.Start()
    End Function

  End Module
End Namespace
