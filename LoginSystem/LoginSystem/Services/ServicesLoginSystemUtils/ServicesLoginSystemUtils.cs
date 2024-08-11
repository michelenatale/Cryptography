
using michele.natale.Pointers;
using System.Numerics;
using System.Text;

namespace michele.natale.LoginSystems.Services;

partial class AppServices
{
  /// <summary>
  /// Clear a string atomar
  /// </summary>
  /// <param name="txts"></param>
  public unsafe void ResetText(params string[] txts)
  {
    for (int i = 0; i < txts.Length; i++)
      fixed (char* ptr = txts[i])
        for (int j = 0; j < txts[i].Length; j++)
          ptr[j] = char.MinValue!;
  }

  /// <summary>
  /// Clear a Array of T (generic)
  /// </summary>
  /// <typeparam name="T"></typeparam>
  /// <param name="nums"></param>
  public void ClearPrimitives<T>(params T[][] nums)
    where T : INumber<T>, IMinMaxValue<T>
  {
    for (var i = 0; i < nums.Length; i++)
      this.ClearPrimitives(nums[i]);
  }

  /// <summary>
  /// Clear a Array of T (generic)
  /// </summary>
  /// <typeparam name="T"></typeparam>
  /// <param name="num"></param>
  public void ClearPrimitives<T>(T[] num)
    where T : INumber<T>, IMinMaxValue<T>
  {
    Array.Clear(num);
  }

  /// <summary>
  /// Check is Reference Null or Empty
  /// </summary>
  /// <typeparam name="T"></typeparam>
  /// <param name="numbers"></param>
  /// <returns></returns>
  public bool IsNullOrEmpty<T>(T[][] numbers)
      where T : INumber<T>, IMinMaxValue<T>
  {
    foreach (var lines in numbers)
      if (!this.IsNullOrEmpty(lines))
        return false;
    return true;
  }

  /// <summary>
  /// Chek is reference is null or empty
  /// </summary>
  /// <typeparam name="T"></typeparam>
  /// <param name="numbers"></param>
  /// <returns></returns>
  public bool IsNullOrEmpty<T>(T[] numbers)
    where T : INumber<T>, IMinMaxValue<T>
  {
    if (numbers is null) return true;
    return numbers.Length == 0;
  }

  /// <summary>
  /// Check is reference null or empty or zeros
  /// </summary>
  /// <typeparam name="T"></typeparam>
  /// <param name="numbers"></param>
  /// <returns></returns>
  public bool IsNullOrEmptyOrZero<T>(T[][] numbers)
      where T : INumber<T>, IMinMaxValue<T>
  {
    foreach (var lines in numbers)
      if (!this.IsNullOrEmpty(lines))
        return false;
    return true;
  }

  /// <summary>
  /// Check is reference null or empty or zeros
  /// </summary>
  /// <typeparam name="T"></typeparam>
  /// <param name="numbers"></param>
  /// <returns></returns>
  public bool IsNullOrEmptyOrZero<T>(T[] numbers)
    where T : INumber<T>, IMinMaxValue<T>
  {
    if (numbers is null) return true;
    if (numbers.Length == 0) return true;
    return new T[numbers.Length].SequenceEqual(numbers);
  }
  
  /// <summary>
  /// Check is reference null or empty or zeros
  /// </summary>
  /// <param name="instance"></param>
  /// <returns></returns>
  public bool IsNullOrEmpty(UsIPtr<byte> instance)
  {
    if (instance is null) return true;
    return instance.IsEmpty;
  }

  /// <summary>
  /// To Base64
  /// </summary>
  /// <param name="bytes"></param>
  /// <returns></returns>
  public byte[] ToBase64BytesUtf8(byte[] bytes)
  {
    return Encoding.UTF8.GetBytes(Convert.ToBase64String(bytes));
  }

  /// <summary>
  /// From Base64
  /// </summary>
  /// <param name="bytes"></param>
  /// <returns></returns>
  public byte[] FromBase64BytesUtf8(byte[] bytes)
  {
    return Convert.FromBase64String(Encoding.UTF8.GetString(bytes));
  }


  /// <summary>
  /// Return the Alphastring
  /// </summary>
  /// <returns></returns>
  public string ToAlphaString()
  {
    var z = "09AZaz"u8.ToArray();
    var result = new StringBuilder(62);
    //byte[] z = new byte[] { 48, 57, 65, 90, 97, 122 };
    for (int i = 0; i < z.Length; i += 2)
      for (byte j = z[i]; j < z[i + 1]; j++)
        result.Append(Convert.ToChar(j));
    return result.ToString();
  }

}
