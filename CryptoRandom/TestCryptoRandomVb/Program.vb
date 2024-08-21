Option Strict On
Option Explicit On

Imports System.Threading
Imports michele.natale.Cryptography.Randoms

Namespace CryptoRandomTest

  Public Module Program

    Private ReadOnly TLock As New Object()
    Private Property Rand As CryptoRandom = CryptoRandom.Instance


    Public Sub Main()
      Dim rounds As Int32 = 100
      Dim sw As Stopwatch = Stopwatch.StartNew()
      For i As Int32 = 0 To rounds - 1
        TestCryptoRandom()
        TestThreadSave()
        If (i Mod (rounds \ 10)) = 0 Then
          Console.Write(".")
        End If
      Next
      Console.WriteLine()
      Console.WriteLine($"rounds = {rounds}; t = {sw.ElapsedMilliseconds}ms")

      Console.ReadLine()
    End Sub

    Private Sub TestCryptoRandom()
      TestBoolean()
      TestByte()
      TestDouble()
      TestInteger()
      TestUInteger()
      TestLong()
      TestULong()
      TestI128()
      TestUI128()
      TestString()
    End Sub

    Private Sub TestBoolean()
      Dim bool1 As Boolean = Rand.NextCryptoBool()
      Dim buffer As Boolean() = New Boolean(999) {}
      Rand.FillCryptoBools(buffer)
      buffer = Rand.RngCryptoBools(1000)
      buffer = Enumerable.Range(0, buffer.Length).Select(Function(x) (x And 1) = 0).ToArray()
      Rand.Shuffle(buffer)
    End Sub

    Private Sub TestDouble()
      Dim double0 As Double = Rand.NextCryptoDouble()
      Dim double1 As Double = Rand.NextCryptoDouble()
      Dim double2 As Double = Rand.NextCryptoDouble(-316542.0, 316542.0)
      Dim double3 As Double = Rand.NextCryptoDouble(-316542.0, -310000.0)
      For i As Int32 = 0 To 9
        Dim dbl As Double = Rand.NextCryptoDouble(-316542.0, -310000.0)
        If dbl < -316542.0 OrElse dbl >= -310000.0 Then
          Throw New Exception()
        End If
      Next

      Dim buffer As Double() = New Double(999) {}
      Rand.FillCryptoDoubles(buffer)
      Rand.FillCryptoDoubles(buffer, -316542.0, 316542.0)
      Dim min As Double = buffer.Min()
      Dim max As Double = buffer.Max()
      If min < -316542.0 OrElse max >= 316542.0 Then
        Throw New Exception()
      End If

      buffer = Rand.RngCryptoDouble(1000, -316542.0, 316542.0)
      min = buffer.Min()
      max = buffer.Max()
      If min < -316542.0 OrElse max >= 316542.0 Then
        Throw New Exception()
      End If

      buffer = Enumerable.Range(0, buffer.Length).Select(Function(x) CDbl(x)).ToArray()
      Rand.Shuffle(buffer)
    End Sub

    Private Sub TestByte()
      Dim byte0 = Rand.NextCryptoByte(5, 15)
      Dim byte1 = Rand.RngCryptoBytes(10)

      Dim buffer(999) As Byte
      Rand.FillCryptoBytes(buffer)

      Array.Clear(buffer, 0, buffer.Length)
      Rand.FillCryptoBytes(buffer, 5, 10)
      Dim min = buffer.Min()
      Dim max = buffer.Max()
      If min < 5 OrElse max >= 10 Then
        Throw New Exception()
      End If

      buffer = Rand.RngCryptoBytes(1000, 5, 10)
      min = buffer.Min()
      max = buffer.Max()
      If min < 5 OrElse max >= 10 Then
        Throw New Exception()
      End If

      buffer = Enumerable.Range(0, buffer.Length).Select(Function(x) Convert.ToByte(x And 255)).ToArray()
      Rand.Shuffle(buffer)
    End Sub

    Private Sub TestInteger()
      Dim integer0 = Rand.NextCryptoInt(Of Int32)()
      Dim integer1 = Rand.NextCryptoInt(316542)
      Dim integer2 = Rand.NextCryptoInt(-316542, 316542)
      If integer2 < -316542 OrElse integer2 >= 316542 Then
        Throw New Exception()
      End If

      Dim buffer = New Int32(999) {}
      Rand.FillCryptoInts(Of Int32)(buffer)
      Rand.FillCryptoInts(Of Int32)(buffer, 316542)
      Rand.FillCryptoInts(Of Int32)(buffer, -316542, 316542)
      Dim min = buffer.Min()
      Dim max = buffer.Max()
      If min < -316542 OrElse max >= 316542 Then
        Throw New Exception()
      End If

      buffer = Rand.RngCryptoInts(1000, -316542, 316542)
      min = buffer.Min()
      max = buffer.Max()
      If min < -316542 OrElse max >= 316542 Then
        Throw New Exception()
      End If

      buffer = Enumerable.Range(0, buffer.Length).ToArray()
      Rand.Shuffle(buffer)
    End Sub

    Private Sub TestUInteger()
      Dim uinteger0 = Rand.NextCryptoInt(Of UInt32)()
      Dim uinteger1 = Rand.NextCryptoInt(Of UInt32)(316542)
      Dim uinteger2 = Rand.NextCryptoInt(Of UInt32)(316542, 2 * 316542)
      If uinteger2 < 316542 OrElse uinteger2 >= 2 * 316542 Then
        Throw New Exception()
      End If

      Dim buffer(999) As UInt32
      Rand.FillCryptoInts(Of UInt32)(buffer)
      Rand.FillCryptoInts(Of UInt32)(buffer, 316542)
      Rand.FillCryptoInts(Of UInt32)(buffer, 316542, 2 * 316542)
      Dim min = buffer.Min()
      Dim max = buffer.Max()
      If min < 316542 OrElse max >= 2 * 316542 Then
        Throw New Exception()
      End If

      buffer = Rand.RngCryptoInts(Of UInt32)(1000, 316542, 2 * 316542)
      min = buffer.Min()
      max = buffer.Max()
      If min < 316542 OrElse max >= 2 * 316542 Then
        Throw New Exception()
      End If

      buffer = Enumerable.Range(0, buffer.Length).Select(Function(x) CType(x, UInt32)).ToArray()
      Rand.Shuffle(buffer)
    End Sub

    Private Sub TestLong()
      Dim long0 = Rand.NextCryptoInt(Of Int64)()
      Dim long1 = Rand.NextCryptoInt(Of Int64)(316542)
      Dim long2 = Rand.NextCryptoInt(Of Int64)(-316542, 316542)
      If long2 < -316542 OrElse long2 >= 316542 Then
        Throw New Exception()
      End If

      Dim buffer(999) As Int64
      Rand.FillCryptoInts(Of Int64)(buffer)
      Rand.FillCryptoInts(Of Int64)(buffer, 316542)
      Rand.FillCryptoInts(Of Int64)(buffer, -316542, 316542)
      Dim min = buffer.Min()
      Dim max = buffer.Max()
      If min < -316542 OrElse max >= 316542 Then
        Throw New Exception()
      End If

      buffer = Rand.RngCryptoInts(Of Int64)(1000, -316542, 316542)
      min = buffer.Min()
      max = buffer.Max()
      If min < -316542 OrElse max >= 316542 Then
        Throw New Exception()
      End If

      buffer = Enumerable.Range(0, buffer.Length).Select(Function(x) CType(x, Int64)).ToArray()
      Rand.Shuffle(buffer)
    End Sub

    Private Sub TestULong()
      Dim ulong0 = Rand.NextCryptoInt(Of UInt64)()
      Dim ulong1 = Rand.NextCryptoInt(Of UInt64)(316542)
      Dim ulong2 = Rand.NextCryptoInt(Of UInt64)(316542, 316542 * 2)
      If ulong2 < 316542 OrElse ulong2 >= 2 * 316542 Then
        Throw New Exception()
      End If

      Dim buffer = New UInt64(999) {}
      Rand.FillCryptoInts(Of UInt64)(buffer)
      Rand.FillCryptoInts(Of UInt64)(buffer, 316542)
      Rand.FillCryptoInts(Of UInt64)(buffer, 316542, 316542 * 2)
      Dim min = buffer.Min()
      Dim max = buffer.Max()
      If min < 316542 OrElse max >= 2 * 316542 Then
        Throw New Exception()
      End If

      buffer = Rand.RngCryptoInts(Of UInt64)(1000, 316542, 2 * 316542)
      min = buffer.Min()
      max = buffer.Max()
      If min < 316542 OrElse max >= 2 * 316542 Then
        Throw New Exception()
      End If

      buffer = Enumerable.Range(0, buffer.Length).Select(Function(x) CType(x, UInt64)).ToArray()
      Rand.Shuffle(buffer)
    End Sub

    Private Sub TestI128()
      Dim int128_0 = Rand.NextCryptoInt(Of Int128)()
      Dim int128_1 = Rand.NextCryptoInt(Of Int128)(316542)
      Dim int128_2 = Rand.NextCryptoInt(Of Int128)(-316542, 316542 * 2)
      If int128_2 < -316542 OrElse int128_2 >= 2 * 316542 Then
        Throw New Exception()
      End If

      Dim buffer = New Int128(999) {}
      Rand.FillCryptoInts(Of Int128)(buffer)
      Rand.FillCryptoInts(Of Int128)(buffer, 316542)
      Rand.FillCryptoInts(Of Int128)(buffer, -316542, 316542 * 2)
      Dim min = buffer.Min()
      Dim max = buffer.Max()
      If min < -316542 OrElse max >= 2 * 316542 Then
        Throw New Exception()
      End If

      buffer = Rand.RngCryptoInts(Of Int128)(1000, -316542, 316542)
      min = buffer.Min()
      max = buffer.Max()
      If min < -316542 OrElse max >= 316542 Then
        Throw New Exception()
      End If

      buffer = Enumerable.Range(0, buffer.Length).Select(Function(x) CType(x, Int128)).ToArray()
      Rand.Shuffle(buffer)
    End Sub

    Private Sub TestUI128()
      Dim uint128_0 = Rand.NextCryptoInt(Of UInt128)()
      Dim uint128_1 = Rand.NextCryptoInt(Of UInt128)(316542UL)
      Dim uint128_2 = Rand.NextCryptoInt(Of UInt128)(316542UL, 316542UL * 2UL)
      If uint128_2 < 316542UL OrElse uint128_2 >= 2UL * 316542UL Then
        Throw New Exception()
      End If

      Dim buffer = New UInt128(999) {}
      Rand.FillCryptoInts(Of UInt128)(buffer)
      Rand.FillCryptoInts(Of UInt128)(buffer, 316542UL)
      Rand.FillCryptoInts(Of UInt128)(buffer, 316542UL, 316542UL * 2UL)
      Dim min = buffer.Min()
      Dim max = buffer.Max()
      If min < 316542UL OrElse max >= 2UL * 316542UL Then
        Throw New Exception()
      End If

      buffer = Rand.RngCryptoInts(Of UInt128)(1000, 316542UL, 2UL * 316542UL)
      min = buffer.Min()
      max = buffer.Max()
      If min < 316542UL OrElse max >= 2UL * 316542UL Then
        Throw New Exception()
      End If

      buffer = Enumerable.Range(0, buffer.Length).Select(Function(x) CType(x, UInt128)).ToArray()
      Rand.Shuffle(buffer)
    End Sub

    Private Sub TestString()
      Dim string1 = Rand.NextString(3, "YourBeechBarSalad")
      Dim string2 = Rand.NextString(10, ICryptoRandom.AlphaLowerUpperNumeric)

      Dim alum = ICryptoRandom.AlphaLowerUpperNumeric
      Rand.Shuffle(alum)
    End Sub

    Private Sub TestThreadSave()
      TestParallelFor()
      TestTaskFactor()
      TestMultiThread()
    End Sub

    Private Sub TestParallelFor()
      Dim cnt As Int32 = 10
      Dim crand = CryptoRandom.Shared
      Dim parallelOptions As New ParallelOptions With {
            .MaxDegreeOfParallelism = Environment.ProcessorCount
        }

      Dim rarray As Int32()() = Enumerable.Range(0, cnt).Select(Function(x) Ints(cnt)).ToArray()

      Parallel.For(0, cnt, parallelOptions,
        Sub(i)
          SyncLock TLock
            crand.FillCryptoInts(Of Int32)(rarray(i))
          End SyncLock
        End Sub)

      If rarray(crand.NextCryptoInt(rarray.Length))(crand.NextCryptoInt(rarray.First().Length)) = 0 Then
        Throw New Exception()
      End If
    End Sub

    Private Sub TestTaskFactor()
      Dim cnt As Int32 = 10
      Dim crand = CryptoRandom.Shared
      Dim cts As New CancellationTokenSource()
      Dim rarray1 As Int32()() = Enumerable.Range(0, cnt).Select(Function(x) Ints(cnt)).ToArray()
      Dim rarray2 As Int32()() = Enumerable.Range(0, cnt).Select(Function(x) Ints(cnt)).ToArray()
      Dim rarray3 As Int32()() = Enumerable.Range(0, cnt).Select(Function(x) Ints(cnt)).ToArray()

      Dim tasks As Task() = {
            Task.Factory.StartNew(Sub() FWorker(rarray1), cts.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default),
            Task.Factory.StartNew(Sub() FWorker(rarray2), cts.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default),
            Task.Factory.StartNew(Sub() FWorker(rarray3), cts.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default)
        }

      Task.WaitAll(tasks)
      If rarray1(crand.NextCryptoInt(rarray1.Length))(crand.NextCryptoInt(rarray1.First().Length)) = 0 Then
        Throw New Exception()
      End If
      If rarray2(crand.NextCryptoInt(rarray2.Length))(crand.NextCryptoInt(rarray2.First().Length)) = 0 Then
        Throw New Exception()
      End If
      If rarray3(crand.NextCryptoInt(rarray3.Length))(crand.NextCryptoInt(rarray3.First().Length)) = 0 Then
        Throw New Exception()
      End If
    End Sub

    Private Sub TestMultiThread()
      Dim cnt As Int32 = 10
      Dim crand = CryptoRandom.Shared
      Dim rarray1 As Int32()() = Enumerable.Range(0, cnt).Select(Function(x) Ints(cnt)).ToArray()
      Dim rarray2 As Int32()() = Enumerable.Range(0, cnt).Select(Function(x) Ints(cnt)).ToArray()
      Dim rarray3 As Int32()() = Enumerable.Range(0, cnt).Select(Function(x) Ints(cnt)).ToArray()
      Dim t1 As New Thread(AddressOf TWorker)
      Dim t2 As New Thread(AddressOf TWorker)
      Dim t3 As New Thread(AddressOf TWorker)
      t1.Start(rarray1)
      t2.Start(rarray2)
      t3.Start(rarray3)

      t1.Join()
      t2.Join()
      t3.Join()

      If rarray1(crand.NextCryptoInt(rarray1.Length))(crand.NextCryptoInt(rarray1.First().Length)) = 0 Then
        Throw New Exception()
      End If
      If rarray2(crand.NextCryptoInt(rarray2.Length))(crand.NextCryptoInt(rarray2.First().Length)) = 0 Then
        Throw New Exception()
      End If
      If rarray3(crand.NextCryptoInt(rarray3.Length))(crand.NextCryptoInt(rarray3.First().Length)) = 0 Then
        Throw New Exception()
      End If
    End Sub

    Private Sub FWorker(rarray As Int32()())
      Dim crand = CryptoRandom.Shared
      For i As Int32 = 0 To rarray.Length - 1
        SyncLock TLock
          crand.FillCryptoInts(Of Int32)(rarray(i))
        End SyncLock
      Next
    End Sub

    Private Sub TWorker(obj As Object)
      Dim rarray As Int32()() = CType(obj, Int32()())
      Dim crand = CryptoRandom.Shared
      For i As Int32 = 0 To rarray.Length - 1
        SyncLock TLock
          crand.FillCryptoInts(Of Int32)(rarray(i))
        End SyncLock
      Next
    End Sub

    Private Function Ints(size As Int32) As Int32()
      Return New Int32(size - 1) {}
    End Function
  End Module

End Namespace