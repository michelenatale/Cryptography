

using System.Numerics;

namespace michele.natale.BitsBytesUtils;


partial class BitsBytesUtils
{
  public static bool IsUnsignedType<T>() where T : INumber<T> =>
    new Type[] { typeof(UInt128), typeof(ushort), typeof(uint), typeof(ulong), typeof(byte) }.Contains(typeof(T));

  public static bool IsUnsignedType<T>(T number) where T : INumber<T> =>
    number is UInt128 || number is ushort || number is uint || number is ulong || number is byte;
 
}
