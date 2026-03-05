Option Strict On
Option Explicit On

Imports System.Text
Imports michele.natale
Imports michele.natale.Pointers
Imports System.Security.Cryptography

Namespace michele.natale.Tests
  Partial Class CryptoChaCha20Poly1305Test
    Public Shared Sub TestChaCha20Poly1305Bytes(rounds As Int32)
      Console.Write($"{NameOf(TestChaCha20Poly1305Bytes)}: ")

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
            outputLength:=NetServices.CHACHA_POLY_MAX_KEY_SIZE))

          Dim nonce = RngBytes(NetServices.CHACHA_POLY_NONCE_SIZE)
          Dim tag = New Byte(NetServices.CHACHA_POLY_TAG_SIZE - 1) {}

          size = rand.[Next](NetServices.CHACHA_POLY_MIN_PLAIN_SIZE,
            NetServices.CHACHA_POLY_MAX_PLAIN_SIZE)

          Dim plain = RngBytes(size)
          Dim associat = Encoding.UTF8.GetBytes("© Michele Natale 2021")

          Dim err = ChaCha20Poly1305EncryptAot(
            plain, plain.Length,
            key.Ptr, key.Length,
            associat, associat.Length,
            cipher_ptr, cipher_length)
          AssertError(err)

          Dim cipher = ToBytes(cipher_ptr, cipher_length)
          FreeBuffer(cipher_ptr)

          err = ChaCha20Poly1305DecryptAot(
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

    Public Shared Sub TestChaCha20Poly1305BytesStress()
      Console.Write($"{NameOf(TestChaCha20Poly1305BytesStress)}: ")

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
          outputLength:=NetServices.CHACHA_POLY_MAX_KEY_SIZE))

        Dim plain = RngBytes(1024 * 1024)
        Dim nonce = RngBytes(NetServices.CHACHA_POLY_NONCE_SIZE)
        Dim tag = New Byte(NetServices.CHACHA_POLY_TAG_SIZE - 1) {}
        Dim associat = Encoding.UTF8.GetBytes("© Michele Natale 2021")
        Dim cipher_ptr As IntPtr = Nothing, cipher_length As Int32 = Nothing

        Dim err = ChaCha20Poly1305EncryptAot(
          plain, plain.Length,
          key.Ptr, key.Length,
          associat, associat.Length,
          cipher_ptr, cipher_length)
        AssertError(err)

        Dim cipher = ToBytes(cipher_ptr, cipher_length)
        FreeBuffer(cipher_ptr)

        Dim decipher_ptr As IntPtr = Nothing, decipher_length As Int32 = Nothing
        err = ChaCha20Poly1305DecryptAot(
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
