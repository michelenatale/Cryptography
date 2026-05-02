Option Strict On
Option Explicit On

Namespace michele.natale.Tests


  ''' <summary>
  ''' Defines the type of FileCompressPackage (FCP) 
  ''' entry stored in the archive.
  ''' </summary>
  Friend Enum FcpType As Byte
    ''' <summary>
    ''' Represents a single file entry.  
    ''' The file data is stored in compressed form.
    ''' </summary>
    File = 1

    ''' <summary>
    ''' Represents a multi-file archive entry.  
    ''' The archive contains multiple files and directories packed together.
    ''' </summary>
    Archiv = 2
  End Enum
End Namespace