
Imports michele.natale.BitHacksOperations

Imports System.Runtime.CompilerServices

Namespace michele.natale.BitHacksOperationsTest

  Public Module Program

    Public Sub Main()
      Dim sw = Stopwatch.StartNew

      Test()

      sw.Stop()
      Console.WriteLine(sw.ElapsedMilliseconds)

      Console.WriteLine()
      Console.WriteLine("FINISH")
      Console.WriteLine()
      Console.ReadLine()
    End Sub

    Private Sub Test()
      'Thanks to https://catonmat.net/low-level-bit-hacks
      TestIsBinary()       'Bit-Hack #0.  Bitdarstellung prüfen.
      TestIsOdd()          'Bit-Hack #1.  Prüfe, ob die Ganzzahl gerade ist.
      TestIsEven()         'Bit-Hack #1.  Prüfe, ob die Ganzzahl ungerade ist.
      TestIsPosXSet()      'Bit-Hack #2.  Testen, ob das n-te Bit gesetzt ist.
      TestPosXSet()        'Bit-Hack #3.  Set das n-te Bit.
      TestPosXReset()      'Bit-Hack #4.  Reset das n-te Bit.
      TestPosXTOGGLE()     'Bit-Hack #5.  Schalten Sie das n-te Bit um.
      TestTurnOffR1()      'Bit-Hack #6.  Schalte das ganz rechte 1-Bit aus.
      TestIsolateR1()      'Bit-Hack #7.  Isolieren Sie das ganz rechte 1-Bit.
      TestPropagateR1()    'Bit-Hack #8.  Propagiere das äußerste rechte 1-Bit nach rechts.
      TestIsolateR0()      'Bit-Hack #9.  Isolieren Sie das 0-Bit ganz rechts.
      TestTurnOnR0()       'Bit-Hack #10. Schalte das 0-Bit ganz rechts ein.


      'Many other interesting examples. (Full Generic)
      TestToPosX()
      TestIsPower2()
      TestIncrement()
      TestToBinaryString()
      TestCountBitSet()
      TestToParity()
      TestBitSwap()
      TestReversBits()
      TestModuloPower2()
      TestNextPermutationWR()
      TestLeastMostSignificantSetBit() '    Same Bit-Hack #7, but with lsb msb
      TestClearsLsbMsbToPosX()
      TestDivide2Mult2()
      TestToLowerToUpper()
      TestLowerUpperNumericAlpha()
      TestIndexFirstLastSetBit() '          Same Bit-Hack #7, but with index  TestNegate();
      TestNegate()
      TestModuloRange()
      TestSign()
      TestAbs()
      TestHasZeroBytes()
      TestRotateLeftRightBitwise()
      TestIsUnsignedType()
      TestIsOppositeSign()
      TestMinMax()
      TestEquality()
      TestSwap()
      TestShuffle()
    End Sub

    Private Sub TestIsBinary()
      'Bit-Hack #0.  Bitdarstellung prüfen.

      Dim rand = Random.Shared

      For i = 0 To 999
        Dim n = rand.[Next]()
        Dim s1 = Convert.ToString(n, 2)
        Dim s2 = BitHacksOPs.ToBinary(n)
        If Not s1.SequenceEqual(s2) Then Throw New Exception()
        If Not BitHacksOPs.IsBinary(s2) Then Throw New Exception()
      Next

    End Sub

    Private Sub TestIsOdd()
      'Bit-Hack #1.  Prüfe, ob die Ganzzahl ungerade ist.

      Dim rand = Random.Shared

      For i = 0 To 999
        Dim n = rand.[Next]()
        If (n And 1) <> 1 Then n += 1
        If Not BitHacksOPs.IsOdd(n) Then Throw New Exception()
      Next

    End Sub

    Private Sub TestIsEven()
      'Bit-Hack #1.  Prüfe, ob die Ganzzahl gerade ist.

      Dim rand = Random.Shared

      For i = 0 To 999
        Dim n = rand.[Next]()
        If (n And 1) = 1 Then n += 1
        If Not BitHacksOPs.IsEven(n) Then Throw New Exception()
      Next

    End Sub

    Private Sub TestIsPosXSet()
      'Bit-Hack #2.  Testen, ob das n-te Bit gesetzt ist.

      Dim rand = Random.Shared

      For i = 0 To 999
        Dim number = rand.[Next]()
        Dim idx = rand.[Next](0, 31)
        If Not BitHacksOPs.IsPosXSet(number, idx) Then number = BitHacksOPs.SetPosX(number, idx)
        If Not BitHacksOPs.IsPosXSet(number, idx) Then Throw New Exception()
      Next

    End Sub

    Private Sub TestPosXSet()
      'Bit-Hack #3.  Set das n-te Bit.

      Dim rand = Random.Shared

      For i = 0 To 999
        Dim number = rand.[Next]()
        Dim idx = rand.[Next](0, 31)
        If Not BitHacksOPs.IsPosXSet(number, idx) Then number = BitHacksOPs.SetPosX(number, idx)
        If Not BitHacksOPs.IsPosXSet(number, idx) Then Throw New Exception()
      Next

    End Sub

    Private Sub TestPosXReset()
      'Bit-Hack #4.  Reset das n-te Bit.

      Dim rand = Random.Shared

      For i = 0 To 999
        Dim number = rand.[Next]()
        Dim idx = rand.[Next](0, 31)
        If BitHacksOPs.IsPosXSet(number, idx) Then number = BitHacksOPs.UnSetPosX(number, idx)
        If BitHacksOPs.IsPosXSet(number, idx) Then Throw New Exception()
      Next

    End Sub

    Private Sub TestPosXTOGGLE()
      'Bit-Hack #5.  Schalten Sie das n-te Bit um.

      Dim rand = Random.Shared

      For i = 0 To 999
        Dim number = rand.[Next]()
        Dim idx = rand.[Next](0, 31)
        Dim tf = BitHacksOPs.IsPosXSet(number, idx)
        number = BitHacksOPs.TOGGLEPosX(number, idx)
        If BitHacksOPs.IsPosXSet(number, idx) = tf Then Throw New Exception()
      Next

    End Sub

    Private Sub TestTurnOffR1()
      'Bit-Hack #6.  Schalte das ganz rechte 1-Bit aus.

      Dim rand = Random.Shared

      For i = 0 To 999
        Dim idx = 0
        Dim number = rand.[Next]()
        For j = 0 To 31
          If BitHacksOPs.IsPosXSet(number, j) Then
            idx = j
            Exit For
          End If
        Next

        If Not BitHacksOPs.IsPosXSet(number, idx) Then Throw New Exception()

        number = BitHacksOPs.TurnOffR1(number)
        If BitHacksOPs.IsPosXSet(number, idx) Then Throw New Exception()
      Next

    End Sub

    Private Sub TestIsolateR1()
      'Bit-Hack #7.  Isoliert das ganz rechte 1-Bit.

      'Gibt den Positionswert des 1-Bit ganz rechts zurück (1 << idx) 
      'was einem power 2 entspricht.

      'Returns the position value of the rightmost 1-bit (1 << idx),
      'which corresponds to a power 2.

      Dim rand = Random.Shared

      For i = 0 To 999
        Dim idx = 0
        Dim number = rand.[Next]()
        For j = 0 To 31
          If BitHacksOPs.IsPosXSet(number, j) Then
            idx = j
            Exit For
          End If
        Next

        If Not BitHacksOPs.IsPosXSet(number, idx) Then Throw New Exception()

        number = BitHacksOPs.IsolateR1(number)

        If Not Integer.IsPow2(number) Then Throw New Exception()
        If number <> 1 << idx Then Throw New Exception()
      Next

    End Sub

    Private Sub TestPropagateR1()
      'Bit-Hack #8.  Propagiere das äußerste rechte 1-Bit nach rechts.

      Dim rand = Random.Shared

      For i = 0 To 999
        Dim idx = 0
        Dim number = rand.[Next]()
        For j = 0 To 31
          If BitHacksOPs.IsPosXSet(number, j) Then
            idx = j
            Exit For
          End If
        Next

        If Not BitHacksOPs.IsPosXSet(number, idx) Then Throw New Exception()

        number = BitHacksOPs.PropagateR1(number)

        For j = 0 To idx + 1 - 1
          If Not BitHacksOPs.IsPosXSet(number, j) Then Throw New Exception()
        Next
      Next

    End Sub

    Private Sub TestIsolateR0()
      'Bit-Hack #9.  Isolieren Sie das 0-Bit ganz rechts.

      Dim rand = Random.Shared

      For i = 0 To 999
        Dim idx = 0
        Dim number = rand.[Next]()
        For j = 0 To 31
          If Not BitHacksOPs.IsPosXSet(number, j) Then
            idx = j
            Exit For
          End If
        Next

        If BitHacksOPs.IsPosXSet(number, idx) Then Throw New Exception()

        number = BitHacksOPs.IsolateR0(number)

        If Not Integer.IsPow2(number) Then Throw New Exception()
        If number <> 1 << idx Then Throw New Exception()
      Next

    End Sub

    Private Sub TestTurnOnR0()
      'Bit-Hack #10. Schalte das 0-Bit ganz rechts ein.

      Dim rand = Random.Shared

      For i = 0 To 999
        Dim idx = 0
        Dim number = rand.[Next]()
        For j = 0 To 31
          If Not BitHacksOPs.IsPosXSet(number, j) Then
            idx = j
            Exit For
          End If
        Next

        If BitHacksOPs.IsPosXSet(number, idx) Then Throw New Exception()

        number = BitHacksOPs.TurnOnR0(number)
        If Not BitHacksOPs.IsPosXSet(number, idx) Then Throw New Exception()
      Next

    End Sub

    Private Sub TestToPosX()
      Dim rand = Random.[Shared]

      Dim typesize = Unsafe.SizeOf(Of Int32)()
      Dim rangesize = 8 * typesize

      Dim number = rand.[Next]()
      Dim bitstr = BitHacksOPs.ToBinary(number)
      Dim lsb = BitHacksOPs.ToPosX(number, 0)
      Dim msb = BitHacksOPs.ToPosX(number, rangesize - 1)

      For i = 0 To 999
        number = rand.[Next]()
        Dim idx = rand.[Next](0, rangesize)
        bitstr = BitHacksOPs.ToBinary(number)
        Dim length = bitstr.Length
        Dim bitvalue = BitHacksOPs.ToPosX(number, idx)
        If idx < length AndAlso bitvalue <> Int32.Parse(bitstr(length - 1 - idx).ToString()) Then
          Throw New Exception()
        End If
      Next
    End Sub

    Private Sub TestIsPower2()
      Dim list = New List(Of Int32)
      For i = 0 To 70_000
        If BitHacksOPs.IsPower2(i) Then
          list.Add(i)
        End If
      Next
    End Sub

    Private Sub TestIncrement()
      Dim iterul As ULong = 0
      For i = 0 To 9
        iterul = BitHacksOPs.ITP(iterul)
      Next

      iterul = 0
      For i = 0 To 9
        iterul = BitHacksOPs.ITP(iterul, 2UL)
      Next

      Dim iterui As UInteger = 0
      For i = 0 To 9
        iterui = BitHacksOPs.ITM(iterui)
      Next

      iterul = 0
      For i = 0 To 9
        iterui = BitHacksOPs.ITM(iterui, 5UI)
      Next

      'Bitwise Increment
      iterul = ULong.MaxValue - 500
      For i = 0 To 999
        iterul = BitHacksOPs.Increment(iterul)
        If i > 500 AndAlso iterul <> CULng(i Mod 500) Then Throw New Exception()
      Next

      'Bitwise Increment (+1)
      iterul = ULong.MaxValue - 500
      For i = 0 To 999
        iterul = BitHacksOPs.Increment(iterul, 500UL)
        If i > 500 AndAlso iterul <> CULng((i - 1) Mod 500) Then Throw New Exception()
      Next

    End Sub

    Private Sub TestToBinaryString()
      Dim rand = Random.[Shared]

      For i = 0 To 999
        Dim n = rand.[Next]()
        Dim s1 = Convert.ToString(n, 2)
        Dim s2 = BitHacksOPs.ToBinary(n)
        If Not s1.SequenceEqual(s2) Then Throw New Exception()
      Next
    End Sub

    Private Sub TestCountBitSet()
      Dim rand = Random.[Shared]

      For i = 0 To 999
        Dim n = rand.[Next]()
        Dim bitstr = BitHacksOPs.ToBinary(n)
        Dim cnt = BitHacksOPs.CountBitsSet(n)
        If Not cnt = bitstr.Count(Function(x) x = "1"c) Then
          Throw New Exception()
        End If
      Next
    End Sub

    Private Sub TestToParity()
      Dim rand = Random.[Shared]

      For i = 0 To 999
        Dim n = rand.[Next]()
        Dim is_parity_odd = BitHacksOPs.ToParity(n) 'parity ungerade?
        Dim bitstr_parity = (BitHacksOPs.ToBinary(n).Count(Function(x) x = "1"c) And 1) = 1
        If is_parity_odd <> bitstr_parity Then Throw New Exception()
      Next

      For i = 0 To 999
        Dim n = CUInt(rand.[Next]())
        Dim is_parity_odd = BitHacksOPs.ToParity(n) 'parity ungerade?
        Dim bitstr_parity = (BitHacksOPs.ToBinary(n).Count(Function(x) x = "1"c) And 1) = 1
        If is_parity_odd <> bitstr_parity Then Throw New Exception()
      Next
    End Sub

    Private Sub TestBitSwap()
      Dim rand = Random.[Shared]

      For i = 0 To 999
        Dim n = rand.[Next]()

        Dim from = 1, [to] = 5, length = 3
        Dim bitstr1 = BitHacksOPs.ToBinary(n)
        Dim swap = BitHacksOPs.SwapBits(n, from, [to], length) 'Von rechts gesehen
        Dim bitstr2 = BitHacksOPs.ToBinary(swap)

        Dim sub10 = bitstr1.Substring(bitstr1.Length - from - length, length)
        Dim sub20 = bitstr2.Substring(bitstr2.Length - from - length, length)

        Dim sub11 = bitstr1.Substring(bitstr1.Length - [to] - length, length)
        Dim sub21 = bitstr2.Substring(bitstr2.Length - [to] - length, length)

        If Not sub10.SequenceEqual(sub21) Then Throw New Exception()
        If Not sub11.SequenceEqual(sub20) Then Throw New Exception()

        swap = BitHacksOPs.SwapBits(swap, from, [to], length) 'Von rechts gesehen
        If Not n = swap Then Throw New Exception()
      Next


      For i = 0 To 999
        Dim n = CULng(rand.NextInt64()) + CULng(rand.NextInt64())

        Dim from = 1, [to] = 5, length = 3
        Dim bitstr1 = BitHacksOPs.ToBinary(n)
        Dim swap = BitHacksOPs.SwapBits(n, from, [to], length) 'Von rechts gesehen
        Dim bitstr2 = BitHacksOPs.ToBinary(swap)

        Dim sub10 = bitstr1.Substring(bitstr1.Length - from - length, length)
        Dim sub20 = bitstr2.Substring(bitstr2.Length - from - length, length)

        Dim sub11 = bitstr1.Substring(bitstr1.Length - [to] - length, length)
        Dim sub21 = bitstr2.Substring(bitstr2.Length - [to] - length, length)

        If Not sub10.SequenceEqual(sub21) Then Throw New Exception()
        If Not sub11.SequenceEqual(sub20) Then Throw New Exception()

        swap = BitHacksOPs.SwapBits(swap, from, [to], length) 'Von rechts gesehen
        If Not n = swap Then Throw New Exception()
      Next

    End Sub

    Private Sub TestReversBits()
      Dim rand = Random.[Shared]

      For i = 0 To 999
        Dim n = rand.[Next]()
        Dim start = rand.Next(0, 10)
        Dim length = rand.Next(2, Convert.ToInt32(Math.Log2(n)) - start)

        Dim bitstr1 = BitHacksOPs.ToBinary(n)
        Dim revers = BitHacksOPs.ReversBits(n, start, length) 'Von rechts gesehen
        Dim bitstr2 = BitHacksOPs.ToBinary(revers)

        Dim bitstr3 = bitstr1.ToCharArray()
        Array.Reverse(bitstr3, bitstr3.Length - start - length, length)

        If Not bitstr2.SequenceEqual(bitstr3) Then
          Throw New Exception()
        End If

        revers = BitHacksOPs.ReversBits(revers, start, length)
        bitstr2 = BitHacksOPs.ToBinary(revers)

        If Not bitstr2.SequenceEqual(bitstr1) Then
          Throw New Exception()
        End If

        If Not n = revers Then
          Throw New Exception()
        End If
      Next
    End Sub

    Private Sub TestModuloPower2()
      Dim rand = Random.[Shared]

      For i = 0 To 999
        Dim n = rand.[Next]()
        Dim s = rand.[Next](1, 31)
        Dim m = 1 << s 'Power2

        'm must be a power 2, ortherwise Exception is thrown
        Dim modulo = BitHacksOPs.ModuloPower2(n, m)

        If Not modulo = n Mod m Then
          Throw New Exception()
        End If
      Next
    End Sub

    Private Sub TestNextPermutationWR()

      'https://www.vb-paradise.de/index.php/Thread/95752-Permutationen/?postID=1171669#post1171669

      Dim cnt = 100
      Dim rand = Random.[Shared]

      For i = 0 To cnt - 1
        Dim n = 0
        While n = 0 OrElse n = Integer.MaxValue
          n = rand.[Next]()
        End While

        Dim perm = BitHacksOPs.NextPermutationWR(n)
        If perm = n Then Throw New Exception()
      Next

      For i = 0 To cnt - 1
        Dim n As UInteger = 0
        While n = 0 OrElse n = UInteger.MaxValue
          n = CUInt(rand.[Next]()) + CUInt(rand.[Next]())
        End While

        Dim perm = BitHacksOPs.NextPermutationWR(n)
        If perm = n Then Throw New Exception()
      Next

      For i = 0 To cnt - 1
        Dim n As Long = 0
        While n = 0 OrElse n = Long.MaxValue
          n = rand.NextInt64()
        End While

        Dim perm = BitHacksOPs.NextPermutationWR(n)
        If perm = n Then Throw New Exception()
      Next

      For i = 0 To cnt - 1
        Dim n As ULong = 0
        While n = 0 OrElse n = ULong.MaxValue
          n = CULng(rand.NextInt64()) + CULng(rand.NextInt64())
        End While

        Dim perm = BitHacksOPs.NextPermutationWR(n)
        If perm = n Then Throw New Exception()
      Next
    End Sub

    Private Sub TestLeastMostSignificantSetBit()
      Dim cnt = 100
      Dim rand = Random.[Shared]

      'The position value is calculated e.g. 100 = 4
      'Es wird der Positions-Wert ausgerechnet z.b. 100 = 4

      For i = 0 To cnt - 1
        Dim n = rand.[Next]()
        Dim str = Convert.ToString(n, 2)
        Dim irm = 1 << BitHacksOPs.ToIndexRightmostSetBit(n)
        Dim lsb = BitHacksOPs.ToLeastSignificantSetBit(n)
        Dim msb = BitHacksOPs.ToMostSignificantSetBit(n)
        Dim ilm = 1 << BitHacksOPs.ToIndexLeftmostSetBit(n)

        If Not irm = lsb Then Throw New Exception()
        If Not ilm = msb Then Throw New Exception()
      Next

      For i = 0 To cnt - 1
        Dim n = CUInt(rand.[Next]()) + CUInt(rand.[Next]())
        'Dim str = Convert.ToString(n, 2)
        Dim irm = 1UI << BitHacksOPs.ToIndexRightmostSetBit(n)
        Dim lsb = BitHacksOPs.ToLeastSignificantSetBit(n)
        Dim msb = BitHacksOPs.ToMostSignificantSetBit(n)
        Dim ilm = 1UI << BitHacksOPs.ToIndexLeftmostSetBit(n)

        If Not irm = lsb Then Throw New Exception()
        If Not ilm = msb Then Throw New Exception()
      Next

      For i = 0 To cnt - 1
        Dim n = rand.NextInt64()
        'Dim str = Convert.ToString(n, 2)
        Dim irm = 1L << BitHacksOPs.ToIndexRightmostSetBit(n)
        Dim lsb = BitHacksOPs.ToLeastSignificantSetBit(n)
        Dim msb = BitHacksOPs.ToMostSignificantSetBit(n)
        Dim ilm = 1L << BitHacksOPs.ToIndexLeftmostSetBit(n)

        If Not irm = lsb Then Throw New Exception()
        If Not ilm = msb Then Throw New Exception()
      Next

      For i = 0 To cnt - 1
        Dim n = CULng(rand.NextInt64()) + CULng(rand.NextInt64())
        'Dim str = n.ToString("B", CultureInfo.InvariantCulture)
        Dim irm = 1UL << BitHacksOPs.ToIndexRightmostSetBit(n)
        Dim lsb = BitHacksOPs.ToLeastSignificantSetBit(n)
        Dim msb = BitHacksOPs.ToMostSignificantSetBit(n)
        Dim ilm = 1UL << BitHacksOPs.ToIndexLeftmostSetBit(n)

        If Not irm = lsb Then Throw New Exception()
        If Not ilm = msb Then Throw New Exception()
      Next
    End Sub


    Public Sub TestClearsLsbMsbToPosX()
      Dim cnt = 1_000
      Dim rand = Random.[Shared]

      For i = 0 To cnt - 1
        Dim n = rand.[Next]()
        Dim str = Convert.ToString(n, 2)
        Dim length = CInt(Math.Floor(Math.Log2(n))) + 1

        Dim i1 = rand.[Next](0, length)
        Dim n1a = BitHacksOPs.ClearsLsbToPosX(n, i1)
        Dim l1 = If(n1a = 0, 1, CInt(Math.Floor(Math.Log2(n1a))) + 1)
        Dim s1a = Convert.ToString(n1a, 2)
        Dim s1b = str.Substring(0, str.Length - 1 - i1) & New String("0"c, i1 + 1)
        Dim n1b = Convert.ToInt32(s1b, 2)

        If Not n1a = n1b Then Throw New Exception()

        Dim i2 = rand.[Next](0, length)
        Dim n2a = BitHacksOPs.ClearsMsbToPosX(n, i2)
        Dim l2 = If(n2a = 0, 1, CInt(Math.Log2(n2a)) + 1)
        Dim s2a = Convert.ToString(n2a, 2)
        Dim s2b = str.Substring(str.Length - i2, i2)
        s2b = If(String.IsNullOrEmpty(s2b), "0", s2b)
        Dim n2b = Convert.ToInt32(s2b, 2)

        If Not n2a = n2b Then Throw New Exception()
      Next

      For i = 0 To cnt - 1
        Dim n = rand.NextInt64()
        Dim str = Convert.ToString(n, 2)
        Dim length = CInt(Math.Floor(Math.Log2(n))) + 1

        Dim i1 = rand.[Next](0, length)
        Dim n1a = BitHacksOPs.ClearsLsbToPosX(n, i1)
        Dim l1 = If(n1a = 0, 1, CInt(Math.Floor(Math.Log2(n1a))) + 1)
        Dim s1a = Convert.ToString(n1a, 2)
        Dim s1b = str.Substring(0, str.Length - 1 - i1) & New String("0"c, i1 + 1)
        Dim n1b = Convert.ToInt64(s1b, 2)

        If Not n1a = n1b Then Throw New Exception()

        Dim i2 = rand.[Next](0, length)
        Dim n2a = BitHacksOPs.ClearsMsbToPosX(n, i2)
        Dim l2 = If(n2a = 0, 1, CInt(Math.Floor(Math.Log2(n2a))) + 1)
        Dim s2a = Convert.ToString(n2a, 2)
        Dim s2b = str.Substring(str.Length - i2, i2)
        s2b = If(String.IsNullOrEmpty(s2b), "0", s2b)
        Dim n2b = Convert.ToInt64(s2b, 2)

        If Not n2a = n2b Then Throw New Exception()
      Next
    End Sub

    Public Sub TestDivide2Mult2()
      Dim cnt = 1_000
      Dim rand = Random.[Shared]

      For i = 0 To cnt - 1
        Dim n = rand.[Next]()

        Dim nm2 = BitHacksOPs.Mult2(n)
        Dim nd2 = BitHacksOPs.Divide2(n)

        Dim nn = (n * 2L) And Int32.MaxValue
        If nn < n Then nn = nn - Int32.MaxValue - 1
        If Not nm2 = nn And Int32.MaxValue Then Throw New Exception()
        If Not nd2 = n \ 2 Then Throw New Exception()
      Next
    End Sub

    Public Sub TestToLowerToUpper()
      Dim cnt = 1_000
      Dim rand = Random.[Shared]

      Dim chin = "中文", kore = "한국어"
      Dim abc = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789"

      For i = 0 To cnt - 1
        Dim n = rand.GetItems(Of Char)(abc, 1).First()
        Dim lower = BitHacksOPs.ToLower(n)
        Dim upper = BitHacksOPs.ToUpper(n)

        If lower <> Char.ToLower(n) Then Throw New Exception()
        If upper <> Char.ToUpper(n) Then Throw New Exception()
      Next

      For i = 0 To cnt - 1
        Dim length = rand.[Next](1, 10)

        Dim n = rand.GetItems(Of Char)(abc, length)
        Dim lower = BitHacksOPs.ToLower(n)
        Dim upper = BitHacksOPs.ToUpper(n)

        Dim str = New String(n)

        If Not lower.SequenceEqual(str.ToLower()) Then Throw New Exception()
        If Not upper.SequenceEqual(str.ToUpper()) Then Throw New Exception()
      Next

      Dim abcd = (chin & kore & abc & chin & kore).ToCharArray()
      rand.Shuffle(abcd)

      For i = 0 To cnt - 1
        Dim length = rand.[Next](1, 10)

        Dim n = rand.GetItems(abcd, length)
        Dim lower = BitHacksOPs.ToLower(n)
        Dim upper = BitHacksOPs.ToUpper(n)

        Dim str = New String(n)

        If Not lower.SequenceEqual(str.ToLower()) Then Throw New Exception()
        If Not upper.SequenceEqual(str.ToUpper()) Then Throw New Exception()
      Next
    End Sub

    Public Sub TestLowerUpperNumericAlpha()
      Dim lower_alpha = BitHacksOPs.ToLowerAlpha()
      Dim upper_alpha = BitHacksOPs.ToUpperAlpha()
      Dim numeric_alpha = BitHacksOPs.ToNumericAlpha()
      Dim lower_upper_alpha = BitHacksOPs.ToLowerUpperAlpha()
      Dim lower_numeric_alpha = BitHacksOPs.ToLowerNumericAlpha()
      Dim upper_numeric_alpha = BitHacksOPs.ToUpperNumericAlpha()
      Dim lower_upper_numeric_alpha = BitHacksOPs.ToLowerUpperNumericAlpha()

      Dim lIdx_a = BitHacksOPs.IndexOfAlphabeth("a"c)
      Dim idx_A = BitHacksOPs.IndexOfAlphabeth("A"c)
      Dim idx_0 = BitHacksOPs.IndexOfAlphabeth("0"c)
      Dim lIdx_z = BitHacksOPs.IndexOfAlphabeth("z"c)
      Dim idx_Z = BitHacksOPs.IndexOfAlphabeth("Z"c)
      Dim idx_9 = BitHacksOPs.IndexOfAlphabeth("9"c)
    End Sub

    Private Sub TestIndexFirstLastSetBit()
      Dim cnt = 1_000
      Dim rand = Random.[Shared]

      'The index (0-base) is calculated here.
      'Der Index (0-Base) wird hier ausgerechnet.

      For i = 0 To cnt - 1
        Dim n = rand.[Next]()
        'var str = Convert.ToString(n, 2); 
        Dim first = BitHacksOPs.IndexFirstSetBit(n)
        Dim last = BitHacksOPs.IndexLastSetBit(n)

        If Not BitHacksOPs.IsPosXSet(n, first) Then Throw New Exception()
        If Not BitHacksOPs.IsPosXSet(n, last) Then Throw New Exception()
      Next
    End Sub


    Private Sub TestNegate()
      Dim cnt = 1_000
      Dim rand = Random.[Shared]

      For i = 0 To cnt - 1
        Dim ni32 = rand.[Next]()
        Dim n1 = BitHacksOPs.Negate(ni32)
        Dim stri32 = Convert.ToString(ni32, 2)
        Dim stri32n = Convert.ToString(n1, 2)

        Dim ni64 = rand.NextInt64()
        Dim n2 = BitHacksOPs.Negate(ni64)
        Dim stri64 = Convert.ToString(ni64, 2)
        Dim stri64n = Convert.ToString(n2, 2)

        Dim nui32 = CUInt(rand.Next())
        Dim n3 = BitHacksOPs.Negate(nui32)
        Dim strui32 = Convert.ToString(nui32, 2)
        Dim strui32n = Convert.ToString(n3, 2)

        Dim nui64 = CULng(rand.NextInt64()) + CULng(rand.NextInt64())
        Dim n4 = BitHacksOPs.Negate(nui64)
        Dim strui64 = BitHacksOPs.ToBinary(nui64)
        Dim strui64n = BitHacksOPs.ToBinary(n4)

        If ni32 = -ni32 Then Throw New Exception()
        If nui32 = -nui32 Then Throw New Exception()
        If ni64 = -ni64 Then Throw New Exception()
        If nui64 = -CType(nui64, Int128) Then Throw New Exception()
      Next
    End Sub

    Private Sub TestModuloRange()
      Dim cnt = 1_000
      Dim rand = Random.[Shared]

      For i = 0 To cnt - 1
        Dim ni32 = rand.[Next]()
        Dim n1 = BitHacksOPs.ModuloRange(ni32)
        Dim m1 = ni32 Mod 32

        Dim ni64 = rand.NextInt64()
        Dim n2 = BitHacksOPs.ModuloRange(ni64)
        Dim m2 = ni64 Mod 64

        Dim nui32 = CUInt(rand.Next())
        Dim n3 = BitHacksOPs.ModuloRange(nui32)
        Dim m3 = nui32 Mod 32

        Dim nui64 = CULng(rand.NextInt64()) + CULng(rand.NextInt64())
        Dim n4 = BitHacksOPs.ModuloRange(nui64)
        Dim m4 = nui64 Mod 64

        If Not n1 = m1 Then Throw New Exception()
        If Not n2 = m2 Then Throw New Exception()
        If Not n3 = m3 Then Throw New Exception()
        If Not n4 = m4 Then Throw New Exception()
      Next
    End Sub

    Private Sub TestSign()
      Dim cnt = 1_000
      Dim rand = Random.[Shared]

      For i = 0 To cnt - 1
        Dim ni8 = CSByte(rand.[Next]() And SByte.MaxValue)
        Dim n1 = BitHacksOPs.Sign(ni8)
        Dim m1 = SByte.Sign(ni8)

        Dim ni16 = CShort(rand.[Next]() And Short.MaxValue)
        Dim n2 = BitHacksOPs.Sign(ni16)
        Dim m2 = Short.Sign(ni16)

        Dim ni32 = rand.[Next]()
        If Integer.IsOddInteger(rand.[Next]()) Then ni32 = -ni32
        Dim n3 = BitHacksOPs.Sign(ni32)
        Dim m3 = Integer.Sign(ni32)

        Dim ni64 = rand.NextInt64()
        If Integer.IsOddInteger(rand.[Next]()) Then ni64 = -ni64
        Dim n4 = BitHacksOPs.Sign(ni64)
        Dim m4 = Long.Sign(ni64)

        If n1 <> m1 Then Throw New Exception()
        If n2 <> m2 Then Throw New Exception()
        If n3 <> m3 Then Throw New Exception()
        If n4 <> m4 Then Throw New Exception()
      Next
    End Sub

    Private Sub TestAbs()
      Dim cnt = 1_000
      Dim rand = Random.[Shared]

      For i = 0 To cnt - 1
        Dim ni8 = CSByte(rand.[Next]() And SByte.MaxValue)
        Dim n1 = BitHacksOPs.Abs(ni8)
        Dim m1 = If(ni8 = SByte.MinValue, ni8, SByte.Abs(ni8))

        Dim ni16 = CShort(rand.[Next]() And Short.MaxValue)
        Dim n2 = BitHacksOPs.Abs(ni16)
        Dim m2 = If(ni16 = Short.MinValue, ni16, Short.Abs(ni16))

        Dim ni32 = rand.[Next]()
        If Integer.IsOddInteger(rand.[Next]()) Then ni32 = -ni32
        Dim n3 = BitHacksOPs.Abs(ni32)
        Dim m3 = Integer.Abs(ni32)

        Dim ni64 = rand.NextInt64()
        If Integer.IsOddInteger(rand.[Next]()) Then ni64 = -ni64
        Dim n4 = BitHacksOPs.Abs(ni64)
        Dim m4 = Long.Abs(ni64)

        If n1 <> m1 Then Throw New Exception()
        If n2 <> m2 Then Throw New Exception()
        If n3 <> m3 Then Throw New Exception()
        If n4 <> m4 Then Throw New Exception()
      Next
    End Sub

    Private Sub TestHasZeroBytes()
      Dim cnt = 1_000
      Dim v = "00000000"
      Dim rand = Random.[Shared]

      For i = 0 To cnt - 1
        Dim m1 = False
        Dim ni8 = CSByte(rand.[Next]() And SByte.MaxValue)
        Dim strni8 = Convert.ToString(ni8, 2).PadLeft(8, "0"c).ToCharArray()
        Dim n1 = BitHacksOPs.HasZeroByte(ni8)
        Array.Reverse(strni8)
        For j = 0 To strni8.Length - 1 Step 8
          Dim buffer = New Char(7) {}
          Array.Copy(strni8, j, buffer, 0, 8)
          If v.SequenceEqual(buffer) Then
            m1 = True
            Exit For
          End If
        Next
        If n1 <> m1 Then Throw New Exception()

        Dim m2 = False
        Dim ni16 = CShort(rand.[Next]() And Short.MaxValue)
        Dim strni16 = Convert.ToString(ni16, 2).PadLeft(16, "0"c).ToCharArray()
        Dim n2 = BitHacksOPs.HasZeroByte(ni16)
        Array.Reverse(strni16)
        For j = 0 To strni16.Length - 1 Step 8
          Dim buffer = New Char(7) {}
          Array.Copy(strni16, j, buffer, 0, 8)
          If v.SequenceEqual(buffer) Then
            m2 = True
            Exit For
          End If
        Next
        If n2 <> m2 Then Throw New Exception()

        Dim m3 = False
        Dim ni32 = rand.[Next]()
        If Integer.IsOddInteger(rand.[Next]()) Then ni32 = -ni32
        Dim strni32 = Convert.ToString(ni32, 2).PadLeft(32, "0"c).ToCharArray()
        Dim n3 = BitHacksOPs.HasZeroByte(ni32)
        Array.Reverse(strni32)
        For j = 0 To strni32.Length - 1 Step 8
          Dim buffer = New Char(7) {}
          Array.Copy(strni32, j, buffer, 0, 8)
          If v.SequenceEqual(buffer) Then
            m3 = True
            Exit For
          End If
        Next
        If n3 <> m3 Then Throw New Exception()

        Dim m4 = False
        Dim ni64 = rand.NextInt64()
        If Integer.IsOddInteger(rand.[Next]()) Then ni64 = -ni64
        Dim strni64 = Convert.ToString(ni64, 2).PadLeft(64, "0"c).ToCharArray()
        Dim n4 = BitHacksOPs.HasZeroByte(ni64)
        Array.Reverse(strni64)
        For j = 0 To strni64.Length - 1 Step 8
          Dim buffer = New Char(7) {}
          Array.Copy(strni64, j, buffer, 0, 8)
          If v.SequenceEqual(buffer) Then
            m4 = True
            Exit For
          End If
        Next
        If n4 <> m4 Then Throw New Exception()

        Dim m5 = False
        Dim nui8 = CByte(rand.[Next]() And Byte.MaxValue)
        Dim strnui8 = Convert.ToString(nui8, 2).PadLeft(8, "0"c).ToCharArray()
        Dim n5 = BitHacksOPs.HasZeroByte(nui8)
        Array.Reverse(strnui8)
        For j = 0 To strnui8.Length - 1 Step 8
          Dim buffer = New Char(7) {}
          Array.Copy(strnui8, j, buffer, 0, 8)
          If v.SequenceEqual(buffer) Then
            m5 = True
            Exit For
          End If
        Next
        If n5 <> m5 Then Throw New Exception()

        Dim m6 = False
        Dim nui16 = CUShort(rand.[Next]() And Short.MaxValue)
        Dim strnui16 = Convert.ToString(nui16, 2).PadLeft(16, "0"c).ToCharArray()
        Dim n6 = BitHacksOPs.HasZeroByte(nui16)
        Array.Reverse(strnui16)
        For j = 0 To strnui16.Length - 1 Step 8
          Dim buffer = New Char(7) {}
          Array.Copy(strnui16, j, buffer, 0, 8)
          If v.SequenceEqual(buffer) Then
            m6 = True
            Exit For
          End If
        Next
        If n6 <> m6 Then Throw New Exception()

        Dim m7 = False
        Dim nui32 = CUInt(rand.[Next]()) + CUInt(rand.[Next]())
        Dim strnui32 = Convert.ToString(nui32, 2).PadLeft(32, "0"c).ToCharArray()
        Dim n7 = BitHacksOPs.HasZeroByte(nui32)
        Array.Reverse(strnui32)
        For j = 0 To strnui32.Length - 1 Step 8
          Dim buffer = New Char(7) {}
          Array.Copy(strnui32, j, buffer, 0, 8)
          If v.SequenceEqual(buffer) Then
            m7 = True
            Exit For
          End If
        Next
        If n7 <> m7 Then Throw New Exception()

        Dim m8 = False
        Dim nui64 = CULng(rand.NextInt64()) + CULng(rand.NextInt64())
        Dim strnui64 = BitHacksOPs.ToBinary(nui64).PadLeft(64, "0"c).ToCharArray()
        Dim n8 = BitHacksOPs.HasZeroByte(nui64)
        Array.Reverse(strnui64)
        For j = 0 To strnui64.Length - 1 Step 8
          Dim buffer = New Char(7) {}
          Array.Copy(strnui64, j, buffer, 0, 8)
          If v.SequenceEqual(buffer) Then
            m8 = True
            Exit For
          End If
        Next
        If n8 <> m8 Then Throw New Exception()
      Next
    End Sub

    Private Sub TestRotateLeftRightBitwise()
      Dim cnt = 1_000
      Dim rand = Random.[Shared]

      For i = 0 To cnt - 1
        Dim ni8 = CSByte(rand.[Next]() And SByte.MaxValue)
        'var org_bits = Convert.ToString(ni8, 2);

        Dim shift = rand.[Next](1, 8)
        Dim n1 = BitHacksOPs.RotateLeft(ni8, shift)
        'var str_bits = Convert.ToString(n1, 2);

        shift = rand.[Next](1, 8)
        Dim n2 = BitHacksOPs.RotateRight(n1, shift)
        'str_bits = Convert.ToString(n2, 2);

        Dim ni16 = CShort(rand.[Next]() And Short.MaxValue)

        shift = rand.[Next](1, 16)
        Dim n3 = BitHacksOPs.RotateLeft(ni16, shift)

        shift = rand.[Next](1, 16)
        Dim n4 = BitHacksOPs.RotateRight(ni16, shift)


        Dim ni32 = rand.[Next]()
        If Integer.IsOddInteger(ni32) Then ni32 = -ni32

        shift = rand.[Next](1, 32)
        Dim n5 = BitHacksOPs.RotateLeft(ni32, shift)

        shift = rand.[Next](1, 32)
        Dim n6 = BitHacksOPs.RotateRight(ni32, shift)


        Dim ni64 = rand.NextInt64()
        If Long.IsOddInteger(ni64) Then ni64 = -ni64

        shift = rand.[Next](1, 64)
        Dim n7 = BitHacksOPs.RotateLeft(ni64, shift)

        shift = rand.[Next](1, 64)
        Dim n8 = BitHacksOPs.RotateRight(ni64, shift)


        Dim nui32 = CUInt(rand.[Next]()) + CUInt(rand.[Next]())

        shift = rand.[Next](1, 32)
        Dim n9 = BitHacksOPs.RotateLeft(nui32, shift)

        shift = rand.[Next](1, 32)
        Dim n10 = BitHacksOPs.RotateRight(nui32, shift)


        Dim nui64 = rand.NextInt64()

        shift = rand.[Next](1, 64)
        Dim n11 = BitHacksOPs.RotateLeft(nui64, shift)

        shift = rand.[Next](1, 64)
        Dim n12 = BitHacksOPs.RotateRight(nui64, shift)

      Next
    End Sub

    Public Sub TestIsUnsignedType()
      Dim cnt = 1_000
      Dim ui8 As Byte = 0
      Dim i8 As SByte = 0
      Dim i16 As Short = 0
      Dim ui16 As UShort = 0
      Dim i32 = 0
      Dim ui32 As UInteger = 0
      Dim i64 As Long = 0
      Dim ui64 As ULong = 0
      'Dim ni As System.nint = 0
      'Dim nui As System.nuint = 0
      Dim i128 As Int128 = 0
      Dim ui128 As UInt128 = 0
      For i = 0 To cnt - 1
        Dim rui8 = BitHacksOPs.IsUnsignedType(ui8)
        Dim ri8 = BitHacksOPs.IsUnsignedType(i8)
        Dim ri16 = BitHacksOPs.IsUnsignedType(i16)
        Dim rui16 = BitHacksOPs.IsUnsignedType(ui16)
        Dim ri32 = BitHacksOPs.IsUnsignedType(i32)
        Dim rui32 = BitHacksOPs.IsUnsignedType(ui32)
        Dim ri64 = BitHacksOPs.IsUnsignedType(i64)
        Dim rui64 = BitHacksOPs.IsUnsignedType(ui64)
        'Dim rni = BitHacksOPs.IsUnsignedType(ni)
        'Dim rnui = BitHacksOPs.IsUnsignedType(nui)
        Dim ri128 = BitHacksOPs.IsUnsignedType(i128)
        Dim rui128 = BitHacksOPs.IsUnsignedType(ui128)
      Next
    End Sub

    Public Sub TestIsOppositeSign()
      Dim cnt = 1_000
      Dim rand = Random.[Shared]

      For i = 0 To cnt - 1
        Dim li8 = CSByte(rand.[Next]() And SByte.MaxValue)
        Dim ri8 = CSByte(rand.[Next]() And SByte.MaxValue)

        Dim is_usign_type = BitHacksOPs.IsUnsignedType(li8)
        Dim resi8 = BitHacksOPs.IsOppositeSign(li8, ri8)

        Dim lui8 = CByte(rand.[Next]() And SByte.MaxValue)
        Dim rui8 = CByte(rand.[Next]() And SByte.MaxValue)

        is_usign_type = BitHacksOPs.IsUnsignedType(lui8)
        Dim resui8 = BitHacksOPs.IsOppositeSign(lui8, rui8)
      Next
    End Sub

    Public Sub TestMinMax()
      Dim cnt = 1_000 / 2
      Dim rand = Random.[Shared]

      For i = 0 To cnt - 1
        Dim l = -rand.[Next](0, 100)
        Dim r = -rand.[Next](100, 1000)

        Dim min_int = BitHacksOPs.Min(l, r)
        Dim max_int = BitHacksOPs.Max(l, r)

        If Not min_int = r Then Throw New Exception()
        If Not max_int = l Then Throw New Exception()


        l = -rand.[Next](0, 100)
        r = rand.[Next](100, 1000)

        min_int = BitHacksOPs.Min(l, r)
        max_int = BitHacksOPs.Max(l, r)

        If Not min_int = l Then Throw New Exception()
        If Not max_int = r Then Throw New Exception()


        l = rand.[Next](0, 100)
        r = rand.[Next](100, 1000)

        min_int = BitHacksOPs.Min(l, r)
        max_int = BitHacksOPs.Max(l, r)

        If min_int <> Integer.Min(l, r) Then Throw New Exception()
        If max_int <> Integer.Max(l, r) Then Throw New Exception()
      Next

      For i = 0 To cnt - 1
        Dim l = CUInt(rand.[Next](0, 100))
        Dim r = CUInt(rand.[Next](100, 1000))

        Dim min_int = BitHacksOPs.Min(l, r)
        Dim max_int = BitHacksOPs.Max(l, r)
        If min_int <> l Then Throw New Exception()
        If max_int <> r Then Throw New Exception()


        l = CUInt(rand.[Next](0, 1000))
        r = CUInt(rand.[Next](0, 1000))

        min_int = BitHacksOPs.Min(l, r)
        max_int = BitHacksOPs.Max(l, r)

        If min_int <> UInteger.Min(l, r) Then Throw New Exception()
        If max_int <> UInteger.Max(l, r) Then Throw New Exception()
      Next
    End Sub

    Public Sub TestEquality()
      Dim cnt = 100
      Dim rand = Random.[Shared]

      For i = 0 To cnt - 1
        Dim l = rand.[Next](0, 1024)
        Dim r = rand.[Next](0, 1024)
        Dim truefalse = BitHacksOPs.Equals(l, r)

        l = r
        truefalse = BitHacksOPs.Equals(l, r)

        Dim a = New Byte(1023) {}
        Dim b = New Byte(1023) {}
        rand.NextBytes(a)
        rand.NextBytes(b)
        truefalse = BitHacksOPs.FixedTimeEquals(Of Byte)(a, b)

        Dim ai = New Integer(255) {}
        Dim bi = New Integer(255) {}
        Buffer.BlockCopy(a, 0, ai, 0, a.Length)
        Buffer.BlockCopy(b, 0, bi, 0, b.Length)
        truefalse = BitHacksOPs.FixedTimeEquals(Of Integer)(ai, bi)

        ai = bi.ToArray()
        truefalse = BitHacksOPs.FixedTimeEquals(Of Integer)(ai, bi)
      Next
    End Sub

    Public Sub TestSwap()
      Dim cnt = 1_000
      Dim rand = Random.Shared

      For i = 0 To cnt - 1
        Dim l = rand.Next(0, 1024)
        Dim r = rand.Next(0, 1024)
        BitHacksOPs.Swap(l, r)

        l = r
        BitHacksOPs.Swap(l, r)

        Dim lul = CULng(rand.NextInt64(0, 1024))
        Dim rul = CULng(rand.NextInt64(0, 1024))
        BitHacksOPs.Swap(lul, rul)

        lul = rul
        BitHacksOPs.Swap(lul, rul)
      Next
    End Sub

    Public Sub TestShuffle()
      Dim cnt = 100
      Dim rand = Random.[Shared]

      For i = 0 To cnt - 1
        Dim n = rand.[Next](0, 1024)

        'number must have 0's as well as 1's.
        If n = 0 OrElse n = -1 OrElse n = Integer.MaxValue OrElse n = Integer.MinValue Then Continue For

        Dim nset = BitHacksOPs.CountBitsSet(n)

        'Entropie
        Dim en = rand.[Next](0, 1024)

        'Sample without entropie
        Dim shl1 = BitHacksOPs.Shuffle(n)

        'number must have 0's as well as 1's. 
        If shl1 = 0 OrElse shl1 = -1 OrElse shl1 = Integer.MaxValue OrElse shl1 = Integer.MinValue Then Continue For

        Dim nset_shl1 = BitHacksOPs.CountBitsSet(shl1)


        'Sample with entropie
        Dim shl2 = BitHacksOPs.Shuffle(n, en)

        'number must have 0's as well as 1's. 
        If shl2 = 0 OrElse shl2 = -1 OrElse shl2 = Integer.MaxValue OrElse shl2 = Integer.MinValue Then Continue For

        Dim nset_shl2 = BitHacksOPs.CountBitsSet(shl2)

        'Checks
        If Not nset = nset_shl1 Then Throw New Exception()
        If Not nset = nset_shl2 Then Throw New Exception()

      Next

      For i = 0 To cnt - 1
        Dim n = CULng(rand.[Next](0, 1024))

        'number must have 0's as well as 1's. 
        If n = ULong.MaxValue OrElse n = ULong.MinValue Then Continue For

        Dim nset = BitHacksOPs.CountBitsSet(n)

        'Entropie
        Dim en = CULng(rand.[Next](0, 1024))

        'Sample without entropie
        Dim shl1 = BitHacksOPs.Shuffle(n)

        'number must have 0's as well as 1's. 
        If shl1 = ULong.MaxValue OrElse shl1 = ULong.MinValue Then Continue For

        Dim nset_shl1 = BitHacksOPs.CountBitsSet(shl1)


        'Sample with entropie
        Dim shl2 = BitHacksOPs.Shuffle(n, en)

        'number must have 0's as well as 1's. 
        If shl2 = ULong.MaxValue OrElse shl2 = ULong.MinValue Then Continue For

        Dim nset_shl2 = BitHacksOPs.CountBitsSet(shl2)

        'Checks
        If Not nset = nset_shl1 Then Throw New Exception()
        If Not nset = nset_shl2 Then Throw New Exception()

      Next
    End Sub

  End Module

End Namespace