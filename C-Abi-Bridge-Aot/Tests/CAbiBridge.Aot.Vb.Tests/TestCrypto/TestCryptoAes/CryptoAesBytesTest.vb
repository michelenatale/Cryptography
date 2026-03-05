Option Strict On
Option Explicit On

Imports System.Text
Imports michele.natale
Imports michele.natale.Pointers
Imports System.Security.Cryptography

Namespace michele.natale.Tests
  Partial Class CryptoAesTest
    Public Shared Sub TestAesBytes(rounds As Int32)
      Console.Write($"{NameOf(TestAesBytes)}: ")

      Dim rand = Random.[Shared]
      Dim sw = Stopwatch.StartNew()
      Dim cipher_ptr As IntPtr = Nothing,
        cipher_length As Int32 = Nothing,
        decipher_ptr As IntPtr = Nothing,
        decipher_length As Int32 = Nothing

      For i As Int32 = 0 To rounds - 1
        Dim size = rand.[Next](NetServices.MIN_PW_SIZE, 17)
        Dim pw = RngBytes(size)
        size = rand.[Next](NetServices.MIN_SALT_SIZE, NetServices.MAX_SALT_SIZE)
        Dim salt = RngBytes(size)

        Using key = New UsIPtr(Of Byte)(
          Rfc2898DeriveBytes.Pbkdf2(
            password:=pw, salt:=salt,
            iterations:=NetServices.MIN_ITERATION,
            hashAlgorithm:=HashAlgorithmName.SHA512,
            outputLength:=NetServices.AES_KEY_SIZE))

          Dim nonce = RngBytes(NetServices.AES_IV_SIZE)
          Dim tag = New Byte(NetServices.AES_TAG_SIZE - 1) {}

          size = rand.[Next](NetServices.AES_MIN_PLAIN_SIZE,
            NetServices.AES_MAX_PLAIN_SIZE)

          Dim plain = RngBytes(size)
          Dim associat = Encoding.UTF8.GetBytes("© Michele Natale 2021")

          Dim err = AesEncryptAot(
            plain, plain.Length,
            key.Ptr, key.Length,
            associat, associat.Length,
            cipher_ptr, cipher_length)
          AssertError(err)

          Dim cipher = ToBytes(cipher_ptr, cipher_length)
          FreeBuffer(cipher_ptr)

          err = AesDecryptAot(
            cipher, cipher.Length,
            key.Ptr, key.Length,
            associat, associat.Length,
            decipher_ptr, decipher_length)
          AssertError(err)

          Dim decipher = ToBytes(decipher_ptr, decipher_length)
          FreeBuffer(decipher_ptr)

          If Not decipher.SequenceEqual(plain) Then
            Throw New Exception()
          End If

        End Using

        If i Mod (rounds / 10) = 0 Then
          Console.Write(".")
        End If

      Next

      sw.[Stop]()
      Dim t = sw.ElapsedMilliseconds
      Console.Write($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms")
      Console.WriteLine()
    End Sub

    Public Shared Sub TestAesBytesStress()
      Console.Write($"{NameOf(TestAesBytesStress)}: ")

      Dim rand = Random.[Shared]
      Dim sw = Stopwatch.StartNew()

      Dim size = rand.[Next](NetServices.MIN_PW_SIZE, 17)
      Dim pw = RngBytes(size)

      size = rand.[Next](NetServices.MIN_SALT_SIZE,
        NetServices.MAX_SALT_SIZE)
      Dim salt = RngBytes(size)

      Using key = New UsIPtr(Of Byte)(
        Rfc2898DeriveBytes.Pbkdf2(
          password:=pw, salt:=salt,
          iterations:=NetServices.MIN_ITERATION,
          hashAlgorithm:=HashAlgorithmName.SHA512,
          outputLength:=NetServices.AES_KEY_SIZE))

        Dim plain = RngBytes(1024 * 1024)
        Dim nonce = RngBytes(NetServices.AES_IV_SIZE)
        Dim tag = New Byte(NetServices.AES_TAG_SIZE - 1) {}
        Dim associat = Encoding.UTF8.GetBytes("© Michele Natale 2021")
        Dim cipher_ptr As IntPtr = Nothing, cipher_length As Int32 = Nothing

        Dim err = AesEncryptAot(
          plain, plain.Length,
          key.Ptr, key.Length,
          associat, associat.Length,
          cipher_ptr, cipher_length)
        AssertError(err)

        Dim cipher = ToBytes(cipher_ptr, cipher_length)
        FreeBuffer(cipher_ptr)

        Dim decipher_ptr As IntPtr = Nothing, decipher_length As Int32 = Nothing
        err = AesDecryptAot(
          cipher, cipher.Length,
          key.Ptr, key.Length,
          associat, associat.Length,
          decipher_ptr, decipher_length)
        AssertError(err)

        Dim decipher = ToBytes(decipher_ptr, decipher_length)
        FreeBuffer(decipher_ptr)

        sw.[Stop]()

        If Not decipher.SequenceEqual(plain) Then
          Throw New Exception()
        End If

      End Using

      Console.Write($"t = {sw.ElapsedMilliseconds}ms")
      Console.WriteLine()

    End Sub
  End Class
End Namespace
