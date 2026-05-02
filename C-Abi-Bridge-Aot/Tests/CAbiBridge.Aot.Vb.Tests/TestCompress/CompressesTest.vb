Option Strict On
Option Explicit On

Namespace michele.natale.Tests

  Partial Friend NotInheritable Class CompressesTest
    Public Shared Sub StartNative(rounds As Int32)
      StartCompress(rounds)
      StartFileCompressPackage(rounds)
    End Sub
  End Class
End Namespace
