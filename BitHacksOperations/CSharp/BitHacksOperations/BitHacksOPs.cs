
using System.Text;
using System.Numerics;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace michele.natale.BitHacksOperations;


using Randoms;


//https://catonmat.net/low-level-bit-hacks
//https://learn.microsoft.com/en-us/dotnet/standard/generics/math#operator-interfaces


/// <summary>
/// A class with many extraordinary bit-hacking methods. 
/// </summary>
public class BitHacksOPs
{

  /// <summary>
  /// Converts a Number to its Binary Representation 
  /// </summary>
  /// <typeparam name="T">desiered Type</typeparam>
  /// <param name="number">desiered Number</param>
  /// <returns>string Bit Representation</returns>
  public static string ToBinary<T>(T number)
    where T : INumberBase<T>, IBitwiseOperators<T, T, T>, IMinMaxValue<T>,
              IShiftOperators<T, int, T>, IComparisonOperators<T, T, bool>
  {
    if (!T.IsInteger(number))
      throw new ArgumentException($"{nameof(number)} has failed!", nameof(number));

    if (number < T.Zero)
      return number.ToString("b", CultureInfo.InvariantCulture);

    var result = new StringBuilder();
    while (number != T.Zero)
    {
      result.Insert(0, number & T.One);
      number >>= 1;
    }
    return result.ToString();
  }

  /// <summary>
  /// Check is a binary string
  /// </summary>
  /// <param name="input">Desired Bit Representation.</param>
  /// <returns>True, if a bit-Representation, ortherwise false.</returns>
  public static bool IsBinary(string input)
  {
    var c1 = '1';
    var length = input.Length;
    for (var i = 0; i < length; i++)
      if (input[i] > c1) return false;
    return true;
    //return Regex.IsMatch(input, "^[01]+$");
  }

  /// <summary>
  /// Checks whether it is an odd number
  /// </summary>
  /// <typeparam name="T">Desired Type</typeparam>
  /// <param name="number">Desired Number</param>
  /// <returns>True, if a odd number, ortherwise false.</returns>
  public static bool IsOdd<T>(T number)
    where T : INumberBase<T>, IBitwiseOperators<T, T, T>
  {
    if (!T.IsInteger(number))
      throw new ArgumentException($"{nameof(number)} has failed!", nameof(number));

    //Bit Hack #1. Check if the integer is even or odd.
    //Bit Hack #1. Prüfen Sie, ob die Ganzzahl gerade oder ungerade ist.
    return (number & T.One) == T.One;
  }

  /// <summary>
  /// Checks whether it is an even number
  /// </summary>
  /// <typeparam name="T">Desired Type</typeparam>
  /// <param name="number">Desired Number</param>
  /// <returns>True, if is a event number, ortherwise false.</returns>
  public static bool IsEven<T>(T number)
    where T : INumberBase<T>, IBitwiseOperators<T, T, T>
  {
    if (!T.IsInteger(number))
      throw new ArgumentException($"{nameof(number)} has failed!", nameof(number));

    //Bit Hack #1. Check if the integer is even or odd.
    //Bit Hack #1. Prüfen Sie, ob die Ganzzahl gerade oder ungerade ist.
    return (number & T.One) != T.One;
  }

  /// <summary>
  /// Checks whether the index x is set in the bit representation.
  /// </summary>
  /// <typeparam name="T">Desred Type</typeparam>
  /// <param name="number">Desired Number</param>
  /// <param name="index">Desired Index</param>
  /// <returns>True, if the index x is setting</returns>
  public static bool IsPosXSet<T>(T number, int index)
    where T : INumber<T>, IBitwiseOperators<T, T, T>,
              IShiftOperators<T, int, T>, IComparisonOperators<T, T, bool>
  {
    if (!T.IsInteger(number))
      throw new ArgumentException($"{nameof(number)} has failed!", nameof(number));

    //Bit Hack #2. Test if the n-th bit is set.
    //Bit-Hack #2. Testen Sie, ob das n-te Bit gesetzt ist.
    return ((number) & (T.One << index)) > T.Zero;
  }

  /// <summary>
  /// Sets the index x in the bit representation.
  /// </summary>
  /// <typeparam name="T">Desired Type</typeparam>
  /// <param name="number">Desired Number</param>
  /// <param name="index">Desired Index</param>
  /// <returns>Return the new Value</returns>
  public static T SetPosX<T>(T number, int index)
    where T : INumber<T>, IBitwiseOperators<T, T, T>,
              IShiftOperators<T, int, T>
  {
    if (!T.IsInteger(number))
      throw new ArgumentException($"{nameof(number)} has failed!", nameof(number));

    //Bit Hack #3. Set the n-th bit.
    //Bit-Hack #3. Setze das n-te Bit.
    return number |= (T.One << index);
  }

  /// <summary>
  /// Unsets the index x in the bit representation.
  /// </summary>
  /// <typeparam name="T">Desired Type</typeparam>
  /// <param name="number">Desired Number</param>
  /// <param name="index">Desired Index</param>
  /// <returns>Return the new Number</returns>
  public static T UnSetPosX<T>(T number, int index)
    where T : INumber<T>, IBitwiseOperators<T, T, T>,
              IShiftOperators<T, int, T>
  {
    if (!T.IsInteger(number))
      throw new ArgumentException($"{nameof(number)} has failed!", nameof(number));

    //Bit Hack #4. Unset the n-th bit.
    //Bit-Hack #4. Lösche das n-te Bit.
    return number &= ~(T.One << index);
  }

  /// <summary>
  /// Toggle the index x in the bit representation.
  /// </summary>
  /// <typeparam name="T">Desired Type</typeparam>
  /// <param name="number">Desired Number</param>
  /// <param name="index">Desired Index</param>
  /// <returns>Return the new Number</returns>
  public static T TogglePosX<T>(T number, int index)
    where T : INumber<T>, IBitwiseOperators<T, T, T>,
              IShiftOperators<T, int, T>
  {
    if (!T.IsInteger(number))
      throw new ArgumentException($"{nameof(number)} has failed!", nameof(number));

    //Bit Hack #5. Toggle the n-th bit.
    //Bit-Hack #5. Schalte das n-te Bit um.
    return number ^= (T.One << index);
  }

  /// <summary>
  /// Searches for the first 1 from the right in the Bit-Representation and switches it off.
  /// </summary>
  /// <typeparam name="T">Desired Type</typeparam>
  /// <param name="number">Desired Numer</param>
  /// <returns>Return the new Number</returns>
  public static T TurnOffR1<T>(T number)
    where T : INumberBase<T>, IBitwiseOperators<T, T, T>
  {
    if (!T.IsInteger(number))
      throw new ArgumentException($"{nameof(number)} has failed!", nameof(number));

    //Bit Hack #6. Turn off the rightmost 1-bit.
    //Bit-Hack #6. Schalte das ganz rechte 1-Bit aus.
    return number &= (number - T.One);
  }

  /// <summary>
  /// Searches for the first 1 from the right in the Bit-Representation and isolates it on the same index.
  /// </summary>
  /// <typeparam name="T">Desired Type</typeparam>
  /// <param name="number">Desired Number</param>
  /// <returns>Return the new Number</returns>
  public static T IsolateR1<T>(T number)
    where T : INumber<T>, IBitwiseOperators<T, T, T>
  {
    if (!T.IsInteger(number))
      throw new ArgumentException($"{nameof(number)} has failed!", nameof(number));

    //Bit Hack #7. Isolate the rightmost 1-bit.
    //Bit-Hack #7.  Isolieren Sie das ganz rechte 1-Bit.
    return number &= -number; // The same as number &= ~(number - 1)
  }

  /// <summary>
  /// Searches for the first 1 from the right in the Bit-Representation and fills the preceding 0s with a 1.
  /// </summary>
  /// <typeparam name="T">Desired Type</typeparam>
  /// <param name="number">Desired Number</param>
  /// <returns>Return the new Number</returns>
  public static T PropagateR1<T>(T number)
    where T : INumber<T>, IBitwiseOperators<T, T, T>
  {
    if (!T.IsInteger(number))
      throw new ArgumentException($"{nameof(number)} has failed!", nameof(number));

    //Bit Hack #8. Right propagate the rightmost 1-bit.
    //Bit-Hack #8.  Propagiere das äußerste rechte 1-Bit nach rechts.
    return number |= number - T.One;
  }

  /// <summary>
  /// Searches for the first 0 from the right in the Bit-Representation 
  /// and isolates it on the same index as 1.
  /// </summary>
  /// <typeparam name="T">Desired Type</typeparam>
  /// <param name="number">Desired Number</param>
  /// <returns>Return the Power 2 Value</returns>
  public static T IsolateR0<T>(T number)
    where T : INumber<T>, IBitwiseOperators<T, T, T>
  {
    if (!T.IsInteger(number))
      throw new ArgumentException($"{nameof(number)} has failed!", nameof(number));

    //Bit Hack #9. Isolate the rightmost 0-bit.
    //Bit-Hack #9.  Isolieren Sie das 0-Bit ganz rechts.
    return number = ~number & (number + T.One);
  }

  /// <summary>
  /// Searches for the first 0 from the right in the Bit-Representation, switches it on.
  /// </summary>
  /// <typeparam name="T">Desired Type</typeparam>
  /// <param name="number">Desired Number</param>
  /// <returns>Return the new Number</returns>
  public static T TurnOnR0<T>(T number)
    where T : INumber<T>, IBitwiseOperators<T, T, T>
  {
    if (!T.IsInteger(number))
      throw new ArgumentException($"{nameof(number)} has failed!", nameof(number));

    //Bit Hack #10. Turn on the rightmost 0-bit.
    //Bit-Hack #10. Schalte das 0-Bit ganz rechts ein.
    return number |= (number + T.One);
  }

  /// <summary>
  /// Returns the bit value at position X.
  /// </summary>
  /// <typeparam name="T">Desired Type</typeparam>
  /// <param name="number">Desired Number</param>
  /// <param name="index">Desires Index</param>
  /// <returns>Return th new number</returns>
  public static T ToPosX<T>(T number, int index)
    where T : INumber<T>, IBitwiseOperators<T, T, T>, IShiftOperators<T, int, T>
  {
    if (!T.IsInteger(number))
      throw new ArgumentException($"{nameof(number)} has failed!", nameof(number));

    return ((number & (T.One << index)) > T.Zero) ? T.One : T.Zero;
  }

  /// <summary>
  /// Searches for the least significant bit set and returns it as a Bit significance.
  /// </summary>
  /// <typeparam name="T">Desired Type</typeparam>
  /// <param name="number">Ddesired Number</param>
  /// <returns>Return the new Number</returns>
  public static T ToLeastSignificantSetBit<T>(T number)
    where T : INumber<T>, IBitwiseOperators<T, T, T>, IShiftOperators<T, int, T>
  {
    //https://nordvpn.com/de/cybersecurity/glossary/least-significant-bit/
    return T.One << ToIndexRightmostSetBit(number); //Set bit
  }

  /// <summary>
  /// Searches for the most significant bit set and returns it as a Bit significance.
  /// </summary>
  /// <typeparam name="T">Desired Type</typeparam>
  /// <param name="number">Ddesired Number</param>
  /// <returns>Return the new Number</returns>
  public static T ToMostSignificantSetBit<T>(T number)
    where T : INumber<T>, IBitwiseOperators<T, T, T>, IShiftOperators<T, int, T>
  {
    //https://nordvpn.com/de/cybersecurity/glossary/most-significant-bit/ 
    return T.One << ToIndexLeftmostSetBit(number); //Set bit
  }

  /// <summary>
  /// Check if the Value is Power 2
  /// </summary>
  /// <typeparam name="T">Desired Type</typeparam>
  /// <param name="number">Desired Number</param>
  /// <returns>True, if value is Power 2, ortherwiese false.</returns>
  public static bool IsPower2<T>(T number)
    where T : INumber<T>, IBitwiseOperators<T, T, T>
  {
    if (!T.IsInteger(number))
      throw new ArgumentException($"{nameof(number)} has failed!", nameof(number));

    if (number == T.Zero) return false;
    return (number & (number - T.One)) == T.Zero;
  }

  /// <summary>
  /// Subsequent Incrementer with Step (++, x+)
  /// </summary>
  /// <typeparam name="T">Desired Type</typeparam>
  /// <param name="number">Desired Number</param>
  /// <param name="step">Desired step</param>
  /// <returns>The New Number</returns>
  public static T ITP<T>(T number, T step = default!)
    where T : INumberBase<T>, IEqualityOperators<T, T, bool>, IComparisonOperators<T, T, bool>
  {
    if (!T.IsInteger(number))
      throw new ArgumentException($"{nameof(number)} has failed!", nameof(number));

    var s = step == default || step < T.One ? T.One : step;
    return number + s;
  }

  /// <summary>
  /// Subsequent Decrementer  with Step (--, x-)
  /// </summary>
  /// <typeparam name="T">Desired Type</typeparam>
  /// <param name="number">Desired Number</param>
  /// <param name="step">Desired step</param>
  /// <returns>The New Number</returns>
  public static T ITM<T>(T number, T step = default!)
    where T : INumberBase<T>, IEqualityOperators<T, T, bool>, IComparisonOperators<T, T, bool>
  {
    if (!T.IsInteger(number))
      throw new ArgumentException($"{nameof(number)} has failed!", nameof(number));

    var s = step == default || step < T.One ? T.One : step;
    return number - s;
  }

  /// <summary>
  /// Subsequent Incrementer  with Step (++, x+)
  /// </summary>
  /// <typeparam name="T">Desired Type</typeparam>
  /// <param name="number">Desired Number</param>
  /// <param name="step">Desired step</param>
  public static void Op_ITP<T>(ref T number, T step = default!)
    where T : INumberBase<T>, IEqualityOperators<T, T, bool>, IComparisonOperators<T, T, bool>
  {
    if (!T.IsInteger(number))
      throw new ArgumentException($"{nameof(number)} has failed!", nameof(number));

    var s = step == default || step < T.One ? T.One : step;
    number += s;
  }

  /// <summary>
  /// Subsequent Decrementer with Step (--, x-)
  /// </summary>
  /// <typeparam name="T">Desired Type</typeparam>
  /// <param name="number">Desired Number</param>
  /// <param name="step">Desired step</param>
  public static void Op_ITM<T>(ref T number, T step = default!)
    where T : INumberBase<T>, IEqualityOperators<T, T, bool>, IComparisonOperators<T, T, bool>
  {
    if (!T.IsInteger(number))
      throw new ArgumentException($"{nameof(number)} has failed!", nameof(number));

    var s = step == default || step < T.One ? T.One : step;
    number -= s;
  }

  /// <summary>
  /// Subsequent Incrementer (++)
  /// </summary>
  /// <typeparam name="T">Desired Type</typeparam>
  /// <param name="number">Desired Number</param>
  /// <returns>Return the new Number</returns>
  public static T Increment<T>(T number)
    where T : INumber<T>, IBitwiseOperators<T, T, T>,
              IShiftOperators<T, int, T>, IComparisonOperators<T, T, bool>
  {
    if (!T.IsInteger(number))
      throw new ArgumentException($"{nameof(number)} has failed!", nameof(number));

    //Bitwise Increment
    var c = T.One;
    var bitsize = 8;
    var typesize = Unsafe.SizeOf<T>();
    var count = bitsize * typesize;

    for (var i = 0; i < count; i++)
    {
      var v = (number & (T.One << i)) > T.Zero ? T.One : T.Zero; //ToPosX
      number = (v ^ c) == T.Zero ? number & ~(T.One << i) : number | (T.One << i); //Wenn 0, dann UnSetPosX, ansonsten SetPosX
      c = v & c; //(v + c) / 2 
    }

    return number;
  }

  /// <summary>
  /// Subsequent Incrementer up to and with max. (++)
  /// </summary>
  /// <typeparam name="T">Desired Type</typeparam>
  /// <param name="number">Desired Number</param>
  /// <param name="max">Desired Maximal number</param>
  /// <returns>Return the new Number</returns>
  public static T Increment<T>(T number, T max)
      where T : INumber<T>, IBitwiseOperators<T, T, T>,
                IShiftOperators<T, int, T>, IComparisonOperators<T, T, bool> =>
    number >= max ? T.Zero : Increment(number);

  /// <summary>
  /// Counts all set bits in a number.
  /// </summary>
  /// <param name="number">Desired number</param>
  /// <returns></returns>
  public static int CountBitsSet<T>(T number)
      where T : INumber<T>, IBitwiseOperators<T, T, T>
  {
    if (!T.IsInteger(number))
      throw new ArgumentException($"{nameof(number)} has failed!", nameof(number));

    //http://graphics.stanford.edu/~seander/bithacks.html#CountBitsSetKernighan
    int result; // result accumulates the total bits set
    for (result = 0; number != T.Zero; result++)
      // Clears the lowest set bit
      number &= number - T.One;
    return result;

    //That would also be a possibility.
    //count += (number >> index) & 1;
  }

  /// <summary>
  /// Returns the Hamming Distance
  /// </summary>
  /// <typeparam name="T">Desired Type</typeparam>
  /// <param name="left">Desired Number left</param>
  /// <param name="right">Desired Number right</param>
  /// <returns>Return the Result as a int Value</returns>
  public static int ToHammingDistance<T>(T left, T right)
      where T : INumber<T>, IBitwiseOperators<T, T, T>
  {
    return CountBitsSet(left ^ right);
  }

  /// <summary>
  /// Calculates the Boolean Parity, where True = Odd and False = Even. 
  /// </summary>
  /// <typeparam name="T">Desired Type</typeparam>
  /// <param name="number">Desired Number</param>
  /// <returns>True, if result is odd, ortherwise false.</returns>
  public static bool ToParity<T>(T number)
      where T : INumber<T>, IBitwiseOperators<T, T, T>
  {
    if (!T.IsInteger(number) || T.IsNegative(number))
      throw new ArgumentException($"{nameof(number)} has failed!", nameof(number));

    bool result = false;  // parity 

    while (number != T.Zero)
    {
      result = !result;
      number &= number - T.One;
    }

    return result;
  }

  /// <summary>
  /// Swaps a sequence of bits, starting from a certain position.
  /// </summary>
  /// <typeparam name="T">Desired Type</typeparam>
  /// <param name="number">Desired Number</param>
  /// <param name="from">Desired From</param>
  /// <param name="to">Desired To</param>
  /// <param name="length">Desiered Legth</param>
  /// <returns>Return the new Number</returns>
  public static T SwapBits<T>(T number, int from, int to, int length)
      where T : INumber<T>, IBitwiseOperators<T, T, T>,
                IShiftOperators<T, int, T>
  {
    if (!T.IsInteger(number))
      throw new ArgumentException($"{nameof(number)} has failed!", nameof(number));

    T x = ((number >> from) ^ (number >> to)) & ((T.One << length) - T.One);
    return number ^ ((x << from) | (x << to));
  }

  /// <summary>
  /// Swaps a sequence of bits, starting from a certain position.
  /// </summary>
  /// <typeparam name="T">Desired Type</typeparam>
  /// <param name="number">Desired Number</param>
  /// <param name="from">Desired From</param>
  /// <param name="to">Desired To</param>
  /// <param name="length">Desiered Legth</param>
  public static void Op_SwapBits<T>(ref T number, int from, int to, int length)
      where T : INumber<T>, IBitwiseOperators<T, T, T>,
                IShiftOperators<T, int, T>
  {
    if (!T.IsInteger(number))
      throw new ArgumentException($"{nameof(number)} has failed!", nameof(number));

    T x = ((number >> from) ^ (number >> to)) & ((T.One << length) - T.One);
    number ^= (x << from) | (x << to);
  }

  /// <summary>
  /// Reverses the order of the bit-elements in a Number.
  /// </summary>
  /// <typeparam name="T">Desired Type</typeparam>
  /// <param name="number">Desired Number</param>
  /// <param name="start">Desired Start</param>
  /// <param name="length">Desired Co</param>
  /// <returns>Return the new Number</returns>
  public static T ReversBits<T>(T number, int start, int length)
      where T : INumber<T>, IBitwiseOperators<T, T, T>,
                IShiftOperators<T, int, T>
  {
    if (!T.IsInteger(number))
      throw new ArgumentException($"{nameof(number)} has failed!", nameof(number));

    if (length < 2) return number;
    var c = length / 2;
    var h = start + length - 1;
    for (var i = 0; i < c; i++)
      Op_SwapBits(ref number, h--, start++, 1);
    return number;
  }


  /// <summary>
  /// Calculates the modulo of a number. The modulo must be a power2, 
  /// otherwise an exception is thrown. 
  /// </summary>
  /// <typeparam name="T">Desired Type</typeparam>
  /// <param name="number">Desired number</param>
  /// <param name="modulo_power2">Desired Power2-Modulo-Value</param>
  /// <returns>Return the Modulo-Calculation</returns>
  /// <exception cref="ArgumentException">If moduo is not a power2!</exception>
  public static T ModuloPower2<T>(T number, T modulo_power2)
      where T : INumber<T>, IBitwiseOperators<T, T, T>
  {
    if (!T.IsInteger(number) || T.IsNegative(number))
      throw new ArgumentException($"{nameof(number)} has failed!", nameof(number));

    if (IsPower2(modulo_power2))
      return number & (modulo_power2 - T.One);

    throw new ArgumentException($"{nameof(modulo_power2)} has failed!");
  }

  /// <summary>
  /// Calculates the lexicographically next bitwise permutation with repetition. 
  /// <para>Notes: If a number consists only of 0s or 1s (MaxValue), an exception is thrown.</para>
  /// </summary>
  /// <typeparam name="T">Desired Type</typeparam>
  /// <param name="number">Desired number</param>
  /// <returns>Returns the next bitwise permutation with repetition.</returns>
  /// <exception cref="ArgumentException"></exception>
  public static T NextPermutationWR<T>(T number)
      where T : INumber<T>, IBitwiseOperators<T, T, T>,
                IShiftOperators<T, int, T>, IMinMaxValue<T>
  {
    //https://www.vb-paradise.de/index.php/Thread/95752-Permutationen/?postID=1171669#post1171669

    if (!T.IsInteger(number))
      throw new ArgumentException($"{nameof(number)} has failed!", nameof(number));

    if (number == T.Zero) throw new ArgumentException($"{nameof(number)} == 0 has failed!");
    if (number == T.MaxValue) throw new ArgumentException($"{nameof(number)} == MaxValue has failed!", nameof(number));

    //http://graphics.stanford.edu/~seander/bithacks.html#NextBitPermutation
    var t = (number | (number - T.One)) + T.One;
    return t | ((((t & -t) / (number & -number)) >> 1) - T.One);
  }

  /// <summary>
  /// Returns the index of the Rightmost Set Bit.
  /// </summary>
  /// <typeparam name="T">Desired Type</typeparam>
  /// <param name="number">Desired Number</param>
  /// <returns>Return the new Number</returns>
  public static int ToIndexRightmostSetBit<T>(T number)
    where T : INumber<T>, IBitwiseOperators<T, T, T>, IShiftOperators<T, int, T>
  {
    if (!T.IsInteger(number))
      throw new ArgumentException($"{nameof(number)} has failed!", nameof(number));

    if (number == T.Zero) return -1;
    var type_size = Unsafe.SizeOf<T>();
    var range_size = 8 * type_size;

    var result = 0;
    while (result < range_size && (number & (T.One << result)) <= T.Zero) result++;
    return result;
  }

  /// <summary>
  /// Returns the index of the Leftmost Set Bit.
  /// </summary>
  /// <typeparam name="T">Desired Type</typeparam>
  /// <param name="number">Desired Number</param>
  /// <returns>Return the new Number</returns>
  public static int ToIndexLeftmostSetBit<T>(T number)
    where T : INumber<T>, IBitwiseOperators<T, T, T>, IShiftOperators<T, int, T>
  {
    if (!T.IsInteger(number))
      throw new ArgumentException($"{nameof(number)} has failed!", nameof(number));

    if (number == T.Zero) return -1;
    var type_size = Unsafe.SizeOf<T>();
    var range_size = 8 * type_size;

    var result = range_size - 1;
    while (result >= 0 && (number & (T.One << result)) <= T.Zero) result--;
    return result;
  }

  /// <summary>
  /// Clears all bits of number from LSB to ith bit
  /// </summary>
  /// <typeparam name="T">Desired Type</typeparam>
  /// <param name="number">Desired Number</param>
  /// <param name="index">Desired Index</param>
  /// <returns>Return the new Number</returns>
  /// <exception cref="ArgumentException"></exception>
  public static T ClearsLsbToPosX<T>(T number, int index)
    where T : INumberBase<T>, IBitwiseOperators<T, T, T>,
              IShiftOperators<T, int, T>
  {
    if (!T.IsInteger(number))
      throw new ArgumentException($"{nameof(number)} has failed!", nameof(number));

    //index is 0-Based
    return number &= ~((T.One << (index + 1)) - T.One);
  }

  /// <summary>
  /// Clears all bits of number from MSB to ith bit
  /// </summary>
  /// <typeparam name="T">Desired Type</typeparam>
  /// <param name="number">Desired Number</param>
  /// <param name="index">Desired Index</param>
  /// <returns>Return the new number</returns>
  /// <exception cref="ArgumentException"></exception>
  public static T ClearsMsbToPosX<T>(T number, int index)
    where T : INumberBase<T>, IBitwiseOperators<T, T, T>,
              IShiftOperators<T, int, T>
  {
    if (!T.IsInteger(number))
      throw new ArgumentException($"{nameof(number)} has failed!", nameof(number));

    //index is 0-Based
    return number &= (T.One << index) - T.One;
  }

  /// <summary>
  /// Divides a number by 2 (very fast operation)
  /// </summary>
  /// <typeparam name="T">Desired Type</typeparam>
  /// <param name="number">Desired Number</param>
  /// <returns>Return the new Number</returns>
  public static T Divide2<T>(T number)
    where T : INumberBase<T>, IShiftOperators<T, T, T>
  {
    return number >> T.One;
  }

  /// <summary>
  /// Multiplies a number by 2 in the same Interger-Range (very fast operation)
  /// </summary>
  /// <typeparam name="T">Desired Type</typeparam>
  /// <param name="number">Desired Number</param>
  /// <returns>Return the new Number</returns>
  public static T Mult2<T>(T number)
    where T : INumberBase<T>, IShiftOperators<T, T, T>
  {
    return number << T.One;
  }

  /// <summary>
  /// Return all Lower and Upper Alpha
  /// </summary>
  /// <returns>return a new String with the Letters.</returns>
  public static string ToLowerUpperAlpha()
  {
    var result = new StringBuilder(52);
    result.Append(ToLowerAlpha());
    result.Append(ToUpperAlpha());
    return result.ToString();
  }

  /// <summary>
  /// Return all Lower and Numerics Alpha
  /// </summary>
  /// <returns>return a new String with the Letters.</returns>
  public static string ToLowerNumericAlpha()
  {
    var result = new StringBuilder(36);
    result.Append(ToLowerAlpha());
    result.Append(ToNumericAlpha());
    return result.ToString();
  }

  /// <summary>
  /// Return all Upper and Numerics Alpha
  /// </summary>
  /// <returns>return a new String with the Letters.</returns>
  public static string ToUpperNumericAlpha()
  {
    var result = new StringBuilder(36);
    result.Append(ToUpperAlpha());
    result.Append(ToNumericAlpha());
    return result.ToString();
  }

  /// <summary>
  /// Return all Lower, Upper and Numerics Alpha
  /// </summary>
  /// <returns>return a new String with the Letters.</returns>
  public static string ToLowerUpperNumericAlpha()
  {
    var result = new StringBuilder(62);
    result.Append(ToLowerAlpha());
    result.Append(ToUpperAlpha());
    result.Append(ToNumericAlpha());
    return result.ToString();
  }

  /// <summary>
  /// Return all Lower letter in Alphabeth
  /// </summary>
  /// <returns>Return a string with Lowers</returns>
  public static string ToLowerAlpha()
  {
    var result = new StringBuilder(26);
    for (char ch = 'a'; ch <= 'z'; ch++)
      result.Append(ch);
    return result.ToString();
  }

  /// <summary>
  /// Return all upper letter in Alphabeth
  /// </summary>
  /// <returns>Return a string with Uppers</returns>
  public static string ToUpperAlpha()
  {
    var result = new StringBuilder(26);
    for (char ch = 'A'; ch <= 'Z'; ch++)
      result.Append(ch);
    return result.ToString();
  }

  /// <summary>
  /// Return the Numeric in Alpha
  /// </summary>
  /// <returns>Return a string with Numerics</returns>
  public static string ToNumericAlpha()
  {
    var result = new StringBuilder(10);
    for (char ch = '0'; ch <= '9'; ch++)
      result.Append(ch);
    return result.ToString();
  }

  /// <summary>
  /// Return the Index of a letter in alphabeth 
  /// </summary>
  /// <param name="letter">Desired Letter</param>
  /// <returns>Return the Index of letter</returns>
  public static int IndexOfAlphabeth(char letter)
  {
    return letter switch
    {
      >= 'a' and <= 'z' => (letter & 31) - 1,
      >= 'A' and <= 'Z' => (letter & 31) - 1,
      >= '0' and <= '9' => (letter & 31) - 16,
      _ => -1,
    };
  }

  /// <summary>
  /// Upper case English alphabet to lower case
  /// </summary>
  /// <param name="chs">Desired English alphabetical letter</param>
  /// <returns>Return the new letter</returns>
  public static char ToLower(char chs)
  {
    if (chs < 'A' || chs > 'Z') return chs;
    return (char)((int)chs | (int)' ');
  }

  /// <summary>
  /// Lower case English alphabet to upper case
  /// </summary>
  /// <param name="chs">Desired English alphabetical letter</param>
  /// <returns>Return the new letter</returns>
  public static char ToUpper(char chs)
  {
    if (chs < 'a' || chs > 'z') return chs;
    return (char)((int)chs & (int)'_');
  }

  /// <summary>
  /// Upper case English alphabet to lower case
  /// </summary>
  /// <param name="chs">Desired English alphabetical letter</param>
  /// <returns>Return the new letter</returns>
  public static char[] ToLower(char[] chs)
  {
    var result = new char[chs.Length];
    for (var i = 0; i < result.Length; i++)
      result[i] = ToLower(chs[i]);
    return result;
  }

  /// <summary>
  /// Lower case English alphabet to upper case
  /// </summary>
  /// <param name="chs">Desired English alphabetical letter</param>
  /// <returns>Return the new letter</returns>
  public static char[] ToUpper(char[] chs)
  {
    var result = new char[chs.Length];
    for (var i = 0; i < result.Length; i++)
      result[i] = ToUpper(chs[i]);
    return result;
  }

  /// <summary>
  /// Upper case English alphabet to lower case
  /// </summary>
  /// <param name="chs">Desired English alphabetical letter</param>
  /// <returns>Return the new letter</returns>
  public static string ToLower(string chs)
  {
    var c = (int)' ';
    var result = new StringBuilder();
    for (var i = 0; i < result.Length; i++)
      result[i] = (char)((int)chs[i] | c);
    return result.ToString();
  }

  /// <summary>
  /// Lower case English alphabet to upper case
  /// </summary>
  /// <param name="chs">Desired English alphabetical letter</param>
  /// <returns>Return the new letter</returns>
  public static string ToUpper(string chs)
  {
    var c = (int)'_';
    var result = new StringBuilder();
    for (var i = 0; i < result.Length; i++)
      result[i] = (char)((int)chs[i] | c);
    return result.ToString();
  }

  /// <summary>
  /// Find the index for last set bit (leftmost)
  /// </summary>
  /// <param name="number">Desired Number</param>
  /// <returns>Return the new Number</returns>
  public static int IndexLastSetBit<T>(T number)
    where T : INumber<T>, IBitwiseOperators<T, T, T>,
              IShiftOperators<T, int, T>, IComparisonOperators<T, T, bool>
  {
    if (!T.IsInteger(number))
      throw new ArgumentException(
        $"{nameof(number)} has failed!", nameof(number));

    if (number == T.Zero) return -1;
    var range_size = 8 * Unsafe.SizeOf<T>();
    for (var i = 0; i < range_size; i++)
    {
      var truefalse = (number & (T.One << (range_size - 1 - i))) > T.Zero;
      if (truefalse) return range_size - 1 - i;
    }
    return -1;
  }

  /// <summary>
  /// Find the index for first set bit (rightmost)
  /// </summary>
  /// <param name="number">Desired Number</param>
  /// <returns>Return the new Number</returns>
  /// <exception cref="ArgumentException"></exception>
  public static int IndexFirstSetBit<T>(T number)
      where T : INumber<T>, IBitwiseOperators<T, T, T>
  {
    if (!T.IsInteger(number))
      throw new ArgumentException(
        $"{nameof(number)} has failed!", nameof(number));

    if (number == T.Zero) return -1;
    var k = int.CreateSaturating(number &= -number);
    return (int)Math.Log2(k);
  }

  /// <summary>
  /// Returns the negated value.
  /// </summary>
  /// <typeparam name="T">Desired Type</typeparam>
  /// <param name="number">Desired Number</param>
  /// <returns>Returns the nre Number</returns>
  /// <exception cref="ArgumentException"></exception>
  public static T Negate<T>(T number)
      where T : INumber<T>, IBitwiseOperators<T, T, T>
  {
    if (!T.IsInteger(number))
      throw new ArgumentException(
        $"{nameof(number)} has failed!", nameof(number));

    return (number ^ -T.One) + T.One;
  }

  /// <summary>
  /// Returns the modulo of the number, where 
  /// modulo d is the current range size (power of 2). 
  /// </summary>
  /// <typeparam name="T">Desired Type</typeparam>
  /// <param name="number">Desired Number</param>
  /// <returns>Returns the new Number</returns>
  /// <exception cref="ArgumentException"></exception>
  public static T ModuloRange<T>(T number)
      where T : INumber<T>, IBitwiseOperators<T, T, T>
  {
    if (!T.IsInteger(number))
      throw new ArgumentException(
        $"{nameof(number)} has failed!", nameof(number));

    var range_size = T.CreateSaturating(8 * Unsafe.SizeOf<T>());
    return number & (range_size - T.One);
  }

  /// <summary>
  /// Returns the sign of the number, where -1 is a 
  /// negative number, 1 is a positive number and 0 
  /// is the value 0.
  /// </summary>
  /// <typeparam name="T">Desired Type</typeparam>
  /// <param name="number">Desired Number</param>
  /// <returns>
  /// Returns -1 is a negative number, 1 is a 
  /// positive number and 0 is the value 0.
  /// </returns>
  /// <exception cref="ArgumentException"></exception>
  public static int Sign<T>(T number)
      where T : INumber<T>, IShiftOperators<T, int, T>
  {
    if (!T.IsInteger(number))
      throw new ArgumentException(
        $"{nameof(number)} has failed!", nameof(number));

    if (number == T.Zero) return 0;
    var range_size = 8 * Unsafe.SizeOf<T>();
    var result = number >> (range_size - 1);
    return result >= T.Zero ? 1 : -1;
  }

  /// <summary>
  /// Returns the absolute value of a number.
  /// </summary>
  /// <typeparam name="T">Desired Type</typeparam>
  /// <param name="number">Desired Number</param>
  /// <returns>Absolute Value</returns>
  /// <exception cref="ArgumentException"></exception>
  public static T Abs<T>(T number)
      where T : INumber<T>, IBitwiseOperators<T, T, T>,
      IShiftOperators<T, int, T>
  {
    //Wichtig: Das MinValue eines signed Integer kann nicht
    //         auf den Absolutwert gesetzt werden.

    //Important: The MinValue of a signed integer
    //           cannot be set to the absolute value.

    if (!T.IsInteger(number))
      throw new ArgumentException(
        $"{nameof(number)} has failed!", nameof(number));

    if (number == T.Zero) return T.Zero;
    var shift = 8 * Unsafe.SizeOf<T>() - 1;
    return (number + (number >> shift)) ^ (number >> shift);
  }

  /// <summary>
  /// Indicates whether a number contains 0 bytes.
  /// </summary>
  /// <typeparam name="T">Desired Type</typeparam>
  /// <param name="number">Desired NUmber</param>
  /// <returns>Return the new Number</returns>
  /// <exception cref="ArgumentException"></exception>
  public static bool HasZeroByte<T>(T number)
      where T : INumber<T>, IBitwiseOperators<T, T, T>,
      IShiftOperators<T, int, T>
  {
    if (!T.IsInteger(number))
      throw new ArgumentException(
        $"{nameof(number)} has failed!", nameof(number));

    if (number == T.Zero) return true;

    //Can be discarded immediately.
    if (typeof(T) == typeof(byte)) return false;
    if (typeof(T) == typeof(sbyte)) return false;

    T mask;
    var type_size = Unsafe.SizeOf<T>();

    for (var i = 0; i < type_size; i++)
    {
      mask = (T.One << ((i + 1) * 8)) - T.One;
      if (i > 0)
      {
        mask = mask == T.Zero ? mask - T.One : mask;
        T k = (T.One << (i * 8)) - T.One;
        mask -= k;
      }
      if ((number & mask) == T.Zero) return true;
    }

    return false;
  }

  /// <summary>
  /// Rotates the bit-representation to the left by a certain value.
  /// </summary>
  /// <typeparam name="T">Desired Type</typeparam>
  /// <param name="number">Desired Number</param>
  /// <param name="n">Shift-Value</param>
  /// <returns>Return the new Number.</returns>
  /// <exception cref="ArgumentException"></exception>
  public static T RotateLeft<T>(T number, int n)
      where T : INumber<T>, IBitwiseOperators<T, T, T>,
      IShiftOperators<T, int, T>
  {
    if (!T.IsInteger(number))
      throw new ArgumentException(
        $"{nameof(number)} has failed!", nameof(number));

    var type_size = 8 * Unsafe.SizeOf<T>();
    n = n % type_size;
    if (n < 0) RotateRight(number, -n);
    return (T)((number << n) | (number >> (type_size - n)));
  }

  /// <summary>
  /// Rotates the bit-representation to the right by a certain value.
  /// </summary>
  /// <typeparam name="T">Desired Type</typeparam>
  /// <param name="number">Desired Number</param>
  /// <param name="n">Shift value</param>
  /// <returns>Return the new Value.</returns>
  /// <exception cref="ArgumentException"></exception>
  public static T RotateRight<T>(T number, int n)
      where T : INumber<T>, IBitwiseOperators<T, T, T>,
      IShiftOperators<T, int, T>
  {
    if (!T.IsInteger(number))
      throw new ArgumentException(
        $"{nameof(number)} has failed!", nameof(number));

    var type_size = 8 * Unsafe.SizeOf<T>();
    n = n % type_size;
    if (n < 0) RotateLeft(number, -n);
    return (T)((number >> n) | (number << (type_size - n)));
  }

  /// <summary>
  /// Indicates whether both values have different signs.
  /// </summary>
  /// <typeparam name="T">Desired Type</typeparam>
  /// <param name="left">Desired Number</param>
  /// <param name="right">Desired NUmber</param>
  /// <returns>True if there are different signs, otherwise false.</returns>
  /// <exception cref="ArgumentException"></exception>
  public static bool IsOppositeSign<T>(T left, T right)
      where T : INumber<T>, IBitwiseOperators<T, T, T>
  {
    if (!T.IsInteger(left))
      throw new ArgumentException(
        $"{nameof(left)} has failed!", nameof(left));

    if (IsUnsignedType<T>()) return false;

    //Has left opposite sign than right?
    //Hat left ein anderes Vorzeichen als right?
    return (left ^ right) < T.Zero;
  }

  /// <summary>
  /// Indicates whether it is an unsigned DataType.
  /// </summary>
  /// <typeparam name="T">Desired Type</typeparam>
  /// <returns>True, is unsigned, ortherwise false.</returns>
  public static bool IsUnsignedType<T>()
    where T : INumber<T>
  {
    T obj = T.Zero;
    return IsUnsignedType(obj);
  }

  /// <summary>
  /// Indicates whether it is an unsigned DataType.
  /// </summary>
  /// <typeparam name="T">Desired Type</typeparam>
  /// <param name="obj">Desired Number</param>
  /// <returns>True, is unsigned, ortherwise false.</returns>
  public static bool IsUnsignedType<T>(T obj)
    where T : INumber<T>
  {
    return obj switch
    {
      byte => true,
      ushort => true,
      uint => true,
      ulong => true,
      nuint => true,
      UInt128 => true,
      _ => false,
    };
  }

  /// <summary>
  /// Returns the min value.
  /// </summary>
  /// <typeparam name="T">Desired Type</typeparam>
  /// <param name="left">Desired Number</param>
  /// <param name="right">Desired Number</param>
  /// <returns>Return the min value.</returns>
  /// <exception cref="ArgumentException"></exception>
  public static T Min<T>(T left, T right)
    where T : INumberBase<T>, IBitwiseOperators<T, T, T>,
              IComparisonOperators<T, T, bool>
  {
    if (!T.IsInteger(left))
      throw new ArgumentException(
        $"{nameof(left)} has failed!", nameof(left));

    var k = left < right ? -T.One : T.Zero;
    return right ^ ((left ^ right) & k);

    //Normally this would also work, but I am 
    //deliberately sticking to bitwise operations here.
    //return left < right, left, right;
  }

  /// <summary>
  /// Returns the max value.
  /// </summary>
  /// <typeparam name="T">Desired Type</typeparam>
  /// <param name="left">Desired Number</param>
  /// <param name="right">Desired Number</param>
  /// <returns>Return the max value.</returns>
  /// <exception cref="ArgumentException"></exception>
  public static T Max<T>(T left, T right)
    where T : INumberBase<T>, IBitwiseOperators<T, T, T>,
              IComparisonOperators<T, T, bool>
  {
    if (!T.IsInteger(left))
      throw new ArgumentException(
        $"{nameof(left)} has failed!", nameof(left));

    var k = left < right ? -T.One : T.Zero;
    return left ^ ((left ^ right) & k);

    //Normally this would also work, but I am 
    //deliberately sticking to bitwise operations here.
    // return left < right, right, left;
  }

  //public static T Min<T>(T a, T b)
  //  where T : IComparisonOperators<T, T, bool> =>
  //    IF(a < b, a, b);

  //public static T Max<T>(T a, T b)
  //  where T : IComparisonOperators<T, T, bool> =>
  //    IF(a < b, b, a);

  //private static T IF<T>(bool condition, T then, T @else) =>
  //  condition ? then : @else;

  /// <summary>
  /// Checks both values for equality.
  /// </summary>
  /// <typeparam name="T">Desired Type</typeparam>
  /// <param name="left">Deisred Number</param>
  /// <param name="right">Desired number</param>
  /// <returns>True if is Equals, ortherwise false.</returns>
  /// <exception cref="ArgumentException"></exception>
  public static bool Equals<T>(T left, T right)
    where T : INumber<T>, IBitwiseOperators<T, T, T>
  {
    if (!T.IsInteger(left))
      throw new ArgumentException(
        $"{nameof(left)} has failed!", nameof(left));

    return (left ^ right) == T.Zero;
  }

  /// <summary>
  /// Checks both values for equality (xor-check).
  /// </summary>
  /// <typeparam name="T">Desired Type</typeparam>
  /// <param name="left">Deisred Number</param>
  /// <param name="right">Desired number</param>
  /// <returns>True if is Equals, ortherwise false.</returns>
  /// <exception cref="ArgumentException"></exception>
  public static bool FixedTimeEquals<T>(
    ReadOnlySpan<T> left, ReadOnlySpan<T> right)
      where T : INumber<T>, IBitwiseOperators<T, T, T>
  {
    if (!T.IsInteger(left[0]))
      throw new ArgumentException(
        $"{nameof(left)} has failed!", nameof(left));

    if ((left.Length ^ right.Length) != 0)
      return false;

    var length = left.Length;
    for (var i = 0; i < length; i++)
      if ((left[i] ^ right[i]) != T.Zero)
        return false;

    return true;
  }

  /// <summary>
  /// Swap two values.
  /// </summary>
  /// <typeparam name="T">Desired Type</typeparam>
  /// <param name="left">Desired Number</param>
  /// <param name="right">Desired number</param>
  /// <exception cref="ArgumentException"></exception>
  public static void Swap<T>(ref T left, ref T right)
      where T : INumber<T>, IBitwiseOperators<T, T, T>
  {
    if (!T.IsInteger(left))
      throw new ArgumentException(
        $"{nameof(left)} has failed!", nameof(left));

    left ^= right; // int temp = b
    right ^= left; // b = a
    left ^= right; // a = temp
  }

  /// <summary>
  /// Shuffles the bit representation of a number.
  /// </summary>
  /// <typeparam name="T">Desired Type</typeparam>
  /// <param name="number">Desired Number</param>
  /// <returns>Return the new Number</returns>
  /// <exception cref="ArgumentException"></exception>
  public static T Shuffle<T>(T number)
      where T : INumber<T>, IShiftOperators<T, int, T>,
      IBitwiseOperators<T, T, T>, IMinMaxValue<T>
  {
    //The number of bits set is retained.
    //Anzahl gesetzte Bits bleiben bestehen.

    if (!T.IsInteger(number))
      throw new ArgumentException(
        $"{nameof(number)} has failed!", nameof(number));

    var range_size = 8 * Unsafe.SizeOf<T>();
    var shift = RandomHolder.NextInt(1, range_size);
    var result = RotateLeft(number, shift);

    var length = (int)Math.Log2(double.CreateSaturating(result)) + 1;
    result = ReversBits(result, 0, length);
    return NextPermutationWR(result);
  }

  /// <summary>
  /// Shuffles the bit representation of a number.
  /// </summary>
  /// <typeparam name="T">Desired Type</typeparam>
  /// <param name="number">Desired Number</param>
  /// <param name="entropie">Desired Entropie</param>
  /// <returns>Return the new Number</returns>
  /// <exception cref="ArgumentException"></exception>
  public static T Shuffle<T>(T number, T entropie)
      where T : INumber<T>, IShiftOperators<T, int, T>,
      IBitwiseOperators<T, T, T>, IMinMaxValue<T>
  {
    //The number of bits set is retained.
    //Anzahl gesetzte Bits bleiben bestehen.

    if (!T.IsInteger(number))
      throw new ArgumentException(
        $"{nameof(number)} has failed!", nameof(number));

    T result = number;
    var cnt = CountBitsSet(number + entropie);
    for (var i = 0; i < cnt; i++)
    {
      var length = (int)Math.Log2(double.CreateSaturating(result)) + 1;
      var shift = RandomHolder.NextInt(1, length);
      result = RotateLeft(result, shift);

      length = (int)Math.Log2(double.CreateSaturating(result)) + 1;
      result = ReversBits(result, 0, length);
      result = NextPermutationWR(result);
    }

    return result;
  }
}


