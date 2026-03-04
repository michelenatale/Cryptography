
using System.Runtime.InteropServices;

namespace michele.natale.CAbiBridge;

[StructLayout(LayoutKind.Sequential)]
public struct CError
{
  public int error_code;
  public IntPtr message;
}


public enum CErrorCode : int
{
  Ok = 0,
  NullPointer = -1,
  InvalidLength = -2,
  IoError = -3,
  CryptoError = -4,
  OutOfRange = -5,
  UnknownError = -99
}
