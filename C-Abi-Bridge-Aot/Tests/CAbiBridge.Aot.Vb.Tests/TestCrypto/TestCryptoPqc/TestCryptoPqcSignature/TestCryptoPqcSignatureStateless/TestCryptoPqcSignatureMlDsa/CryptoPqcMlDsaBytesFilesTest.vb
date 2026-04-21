Option Strict On
Option Explicit On


Imports System.IO
Imports System.Text
Imports michele.natale
Imports michele.natale.Pointers
Imports System.Security.Cryptography


Namespace michele.natale.Tests
  Partial Module CryptoPqcMlDsaTest

    Public Sub TestPqcMlDsaCreateKeyPairs(rounds As Int32)
      Console.Write($"{NameOf(TestPqcMlDsaCreateKeyPairs)}Aot: ")

      Dim rand = Random.[Shared]
      Dim sw = Stopwatch.StartNew()

      Dim mldsa_param As Byte = 0
      Dim pub_key_length As Int32 = 0
      Dim guid_id_length As Int32 = 0
      Dim priv_key_length As Int32 = 0

      Dim pub_key_ptr As IntPtr = IntPtr.Zero
      Dim guid_id_ptr As IntPtr = IntPtr.Zero
      Dim priv_key_ptr As IntPtr = IntPtr.Zero

      For i = 0 To rounds - 1
        Dim err = Native.CreateMlDsaKeyPairAot(
          priv_key_ptr, priv_key_length,
          pub_key_ptr, pub_key_length,
          guid_id_ptr, guid_id_length,
          mldsa_param)
        AssertError(err)

        Dim privkey = New UsIPtr(Of Byte)(ToBytes(priv_key_ptr, priv_key_length))
        Native.FreeBuffer(priv_key_ptr)

        Dim pubkey = ToBytes(pub_key_ptr, pub_key_length)
        Native.FreeBuffer(pub_key_ptr)

        Dim guid = New Guid(ToBytes(guid_id_ptr, guid_id_length))
        Native.FreeBuffer(guid_id_ptr)

        Dim param = NetServicesCrypto.ToMLDsaAlgorithm(CType(mldsa_param, MLDsaParam))

        If i Mod rounds / 10 = 0 Then Console.Write(".")
      Next

      sw.Stop()

      Dim t = sw.ElapsedMilliseconds
      Console.WriteLine($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms")
    End Sub

    Public Sub TestPqcMlDsaCreateKeyPairsParam(rounds As Int32)
      Console.Write($"{NameOf(TestPqcMlDsaCreateKeyPairsParam)}Aot: ")

      Dim rand = Random.[Shared]
      Dim sw = Stopwatch.StartNew()

      Dim pub_key_ptr As IntPtr = IntPtr.Zero
      Dim guid_id_ptr As IntPtr = IntPtr.Zero
      Dim priv_key_ptr As IntPtr = IntPtr.Zero

      Dim guid_id_length As Int32 = Nothing
      Dim pub_key_length As Int32 = Nothing
      Dim priv_key_length As Int32 = Nothing

      For i = 0 To rounds - 1
        Dim mlkem_param = NetServicesCrypto.ToMLDsaAlgorithm()

        'ML-KEM Parameter select. Only the first 3 algos
        Dim idx = rand.[Next](mlkem_param.Length)
        Dim param = CByte(NetServicesCrypto.FromMLDsaAlgorithm(mlkem_param(idx)))

        Dim err = Native.CreateMlDsaKeyPairParamAot(
          param, priv_key_ptr, priv_key_length,
          pub_key_ptr, pub_key_length,
          guid_id_ptr, guid_id_length)
        AssertError(err)

        Dim privkey = New UsIPtr(Of Byte)(ToBytes(priv_key_ptr, priv_key_length))
        Native.FreeBuffer(priv_key_ptr)

        Dim pubkey = ToBytes(pub_key_ptr, pub_key_length)
        Native.FreeBuffer(pub_key_ptr)

        Dim guid = New Guid(ToBytes(guid_id_ptr, guid_id_length))
        Native.FreeBuffer(guid_id_ptr)

        If i Mod rounds / 10 = 0 Then Console.Write(".")
      Next

      sw.Stop()

      Dim t = sw.ElapsedMilliseconds
      Console.WriteLine($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms")
    End Sub

    Public Sub TestPqcMlDsaSaveLoadKeyPairs(rounds As Int32)
      Console.Write($"{NameOf(TestPqcMlDsaSaveLoadKeyPairs)}Aot: ")

      Dim rand = Random.[Shared]
      Dim sw = Stopwatch.StartNew()

      Dim mldsa_param As Byte = 0
      Dim mldsa_param2 As Byte = 0
      Dim pub_key_length As Int32 = 0
      Dim guid_id_length As Int32 = 0
      Dim priv_key_length As Int32 = 0
      Dim pub_key_length2 As Int32 = 0
      Dim guid_id_length2 As Int32 = 0
      Dim priv_key_length2 As Int32 = 0

      Dim guid_id_ptr As IntPtr = IntPtr.Zero
      Dim pub_key_ptr As IntPtr = IntPtr.Zero
      Dim priv_key_ptr As IntPtr = IntPtr.Zero
      Dim pub_key_ptr2 As IntPtr = IntPtr.Zero
      Dim guid_id_ptr2 As IntPtr = IntPtr.Zero
      Dim priv_key_ptr2 As IntPtr = IntPtr.Zero

      For i = 0 To rounds - 1
        Dim err = Native.CreateMlDsaKeyPairAot(
          priv_key_ptr, priv_key_length,
          pub_key_ptr, pub_key_length,
          guid_id_ptr, guid_id_length,
          mldsa_param)
        AssertError(err)

        Dim privkey = New UsIPtr(Of Byte)(ToBytes(priv_key_ptr, priv_key_length))
        Native.FreeBuffer(priv_key_ptr)

        Dim pubkey = ToBytes(pub_key_ptr, pub_key_length)
        Native.FreeBuffer(pub_key_ptr)

        Dim guid_bytes = ToBytes(guid_id_ptr, guid_id_length)
        Native.FreeBuffer(guid_id_ptr)

        Dim guid = New Guid(guid_bytes)
        Dim param = NetServicesCrypto.ToMLDsaAlgorithm(CType(mldsa_param, MLDsaParam))

        'WICHTIG: Wenn 'with_priv_key = false', dann wird der
        '         PrivateKey nicht abgespeichert.

        'IMPORTANT: If 'with_priv_key = false', the private
        '           key will not be saved.
        Dim with_priv_key = Int32.IsEvenInteger(rand.[Next]())
        Dim kpf = Encoding.UTF8.GetBytes("mldsa_keypair.key")

        Using builder = New NativeArrayBuilder()
          Dim kpfile = builder.Add(kpf)
          Dim pub_key = builder.Add(pubkey)
          Dim guidbytes = builder.Add(guid_bytes)

          err = Native.SavePqcMlDsaKeyPairAot(
            kpfile, kpf.Length,
            privkey.Ptr, privkey.Length,
            pub_key, pubkey.Length,
            guidbytes, guid_bytes.Length,
            mldsa_param, with_priv_key)
          AssertError(err)

          err = Native.LoadPqcMlDsaKeyPairAot(
            kpfile, kpf.Length,
            priv_key_ptr2, priv_key_length2,
            pub_key_ptr2, pub_key_length2,
            guid_id_ptr2, guid_id_length2,
            mldsa_param2)
          AssertError(err)
        End Using

        Dim privkey2 = New UsIPtr(Of Byte)(ToBytes(priv_key_ptr2, priv_key_length2))
        Native.FreeBuffer(priv_key_ptr2)

        Dim pubkey2 = ToBytes(pub_key_ptr2, pub_key_length2)
        Native.FreeBuffer(pub_key_ptr2)

        Dim guid_bytes2 = ToBytes(guid_id_ptr2, guid_id_length2)
        Native.FreeBuffer(guid_id_ptr2)

        Dim guid2 = New Guid(guid_bytes2)
        Dim param2 = NetServicesCrypto.ToMLDsaAlgorithm(CType(mldsa_param2, MLDsaParam))

        If with_priv_key AndAlso Not privkey.Equality(privkey2) Then
          Throw New Exception()
        ElseIf Not with_priv_key AndAlso Not privkey2.Length = 0 Then
          Throw New Exception()
        End If

        If Not pubkey.SequenceEqual(pubkey2) Then
          Throw New Exception()
        End If

        If Not guid_bytes.SequenceEqual(guid_bytes2) Then
          Throw New Exception()
        End If

        If Not guid = guid2 Then
          Throw New Exception()
        End If

        If param IsNot param2 Then
          Throw New Exception()
        End If

        If i Mod rounds / 10 = 0 Then
          Console.Write(".")
        End If
      Next

      sw.Stop()

      Dim t = sw.ElapsedMilliseconds
      Console.WriteLine($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms")
    End Sub

    Private Sub TestPqcMlDsaSingleSignature(rounds As Int32)
      'For the temporary exchange, it would not be necessary to save
      'the keys, as all generated keys are only required for the
      'respective session.

      'Für den temporären Austausch, ist das Abspeichern der
      'Schlüssel nicht notwending, da alle generierten Schlüssel
      'nur für die jeweilige Sitzung benötigt werden.

      Console.Write($"{NameOf(TestPqcMlDsaSingleSignature)}: ")

      If rounds < 10 Then rounds = 10
      Dim algos = NetServicesCrypto.ToMLDsaAlgorithm()

      Dim rand = Random.[Shared]
      Dim sw = Stopwatch.StartNew()

      Dim signlength As Int32 = 0
      Dim sign_ptr As IntPtr = IntPtr.Zero
      'Dim pkPubk As (pk As UsIPtr(Of Byte), pubk As Byte()) = Nothing

      For i = 0 To rounds - 1
        'Create a random Message
        Dim size = rand.[Next](10, 128)
        Dim message = NetServicesCrypto.RngCryptoBytes(size)

        'ML-DSA Parameter select. Only the first 3 parameters
        Dim idx = rand.[Next](algos.Length)

        'Create and Save a legal ML-DSA-KeyPair
        Dim algo = algos(idx)

        'Create And Save a legal ML-DSA-KeyPair
        Dim kpip As KeyPairParamInfo = Nothing
        Dim rnum = rand.[Next](Int32.MaxValue)
        If Int32.IsEvenInteger(rnum) Then
          kpip = CreateNativeMlDsaKeyPair()
          algo = kpip.Algo
        Else
          kpip = CreateNativeMlDsaKeyPair(algo)
        End If

        Dim pubk = kpip.PubKey
        Using privk = kpip.PrivKey
          Using builder = New NativeArrayBuilder()
            Dim msg = builder.Add(message)
            Dim pub_key = builder.Add(pubk)
            Dim mldsa_param = CByte(NetServicesCrypto.FromMLDsaAlgorithm(algo))

            Dim err = Native.PqcMlDsaSignAot(
              msg, message.Length,
              privk.Ptr, privk.Length,
              mldsa_param,
              sign_ptr, signlength)
            AssertError(err)

            Dim signature = ToBytes(sign_ptr, signlength)
            Native.FreeBuffer(sign_ptr)

            Dim sign = builder.Add(signature)

            ''mldsa-algi' is included in the signature.
            ''mldsa-algi' ist in der signatur integriert.
            Dim verify = Native.PqcMlDsaVerifyAot(
              msg, message.Length,
              pub_key, pubk.Length,
              sign, signature.Length, err)
            AssertError(err)

            If Not verify Then
              Throw New Exception()
            End If

          End Using
        End Using

        If i Mod rounds / 10 = 0 Then
          Console.Write(".")
        End If
      Next

      sw.Stop()

      Dim t = sw.ElapsedMilliseconds
      Console.WriteLine($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms")
    End Sub

    Private Sub TestPqcMlDsaSingleSignatureKpiSaveLoad(rounds As Int32)
      'A new key pair is always created and saved. This takes time. 
      'For the temporary exchange, it would not be necessary to save
      'the keys, as all generated keys are only required for the
      'respective session.

      'Es wird immer wieder ein neuer Schlüsselpaar erstellt und
      'abgespeichert. Das braucht seine Zeit. 

      'Für den temporären Austausch, wäre das Abspeichern der
      'Schlüssel nicht notwending, da alle generierten Schlüssel
      'nur für die jeweilige Sitzung benötigt werden.

      Console.Write($"{NameOf(TestPqcMlDsaSingleSignatureKpiSaveLoad)}: ")

      If rounds < 10 Then rounds = 10
      Dim algos = NetServicesCrypto.ToMLDsaAlgorithm()

      Dim rand = Random.[Shared]
      Dim sw = Stopwatch.StartNew()

      For i = 0 To rounds - 1
        'Create a random Message
        Dim size = rand.[Next](10, 128)
        Dim message = NetServicesCrypto.RngCryptoBytes(size)

        'ML-DSA Parameter select. Only the first 3 parameters
        Dim idx = rand.[Next](algos.Length)

        'Create and Save a legal ML-DSA-KeyPair
        Dim algo = algos(idx)

        'Create And Save a legal ML-DSA-KeyPair
        Dim kpip As KeyPairParamInfo = Nothing

        Dim kpfile = "mldsa_keypair.key"
        Dim rnum = rand.[Next](Int32.MaxValue)

        If Int32.IsEvenInteger(rnum) Then
          kpip = CreateNativeMlDsaKeyPair()
          algo = kpip.Algo
        Else kpip = CreateNativeMlDsaKeyPair(algo)
        End If

        Dim signlength As Int32 = 0
        Dim sign_ptr As IntPtr = IntPtr.Zero

        Using privk = kpip.PrivKey
          Using builder = New NativeArrayBuilder()

            Dim pubk = kpip.PubKey
            Dim mldsa_algo = CByte(NetServicesCrypto.FromMLDsaAlgorithm(algo))
            Dim mldsainfo = New MlDsaKeyPairInfo(pubk, privk.ToBytes(), algo)
            mldsainfo.SaveKeyPair(kpfile, True)

            'Load KeyPairs Again
            Dim info = MlDsaKeyPairInfo.Load_KeyPair(kpfile)
            If Not mldsainfo.Equals(info) Then Throw New Exception()

            Dim msg = builder.Add(message)

            Dim err = Native.PqcMlDsaSignAot(
              msg, message.Length,
              privk.Ptr, privk.Length,
              mldsa_algo, sign_ptr, signlength)
            AssertError(err)

            Dim signature = ToBytes(sign_ptr, signlength)
            Native.FreeBuffer(sign_ptr)

            Dim sign = builder.Add(signature)
            Dim pub_key = builder.Add(pubk)

            ''mldsa-algi' is included in the signature.
            ''mldsa-algi' ist in der signatur integriert.
            Dim verify = Native.PqcMlDsaVerifyAot(
              msg, message.Length,
              pub_key, pubk.Length,
              sign, signature.Length, err)
            AssertError(err)

            If Not verify Then
              Throw New Exception()
            End If

          End Using
        End Using

        If i Mod rounds / 10 = 0 Then
          Console.Write(".")
        End If
      Next

      sw.Stop()

      Dim t = sw.ElapsedMilliseconds
      Console.WriteLine($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms")
    End Sub

    Private Sub TestPqcMlDsaSingleSignatureFile(rounds As Int32)
      'For the temporary exchange, it would not be necessary to save
      'the keys, as all generated keys are only required for the
      'respective session.

      'Für den temporären Austausch, ist das Abspeichern der
      'Schlüssel nicht notwending, da alle generierten Schlüssel
      'nur für die jeweilige Sitzung benötigt werden.

      Console.Write($"{NameOf(TestPqcMlDsaSingleSignatureFile)}: ")

      If rounds < 10 Then rounds = 10

      Dim srcfile = "data"
      Dim rand = Random.[Shared]
      Dim algos = NetServicesCrypto.ToMLDsaAlgorithm()

      Dim sw = Stopwatch.StartNew()
      For i = 0 To rounds - 1
        'Create a rng-testfile 
        Dim max = (1 << 21) + 1024 'ca. 2Mb
        Dim size = Random.[Shared].[Next](max)
        SetRngFileData(srcfile, size)

        'ML-DSA Parameter select. Only the first 3 parameters
        Dim idx = rand.[Next](algos.Length)

        'Create and Save a legal ML-DSA-KeyPair
        Dim algo = algos(idx)

        'Create And Save a legal ML-DSA-KeyPair
        Dim kpip As KeyPairParamInfo = Nothing
        Dim rnum = rand.[Next](Int32.MaxValue)
        If Int32.IsEvenInteger(rnum) Then
          kpip = CreateNativeMlDsaKeyPair()
          algo = kpip.Algo
        Else kpip = CreateNativeMlDsaKeyPair(algo)
        End If
        Using privk = kpip.PrivKey
          Using builder As New NativeArrayBuilder

            Dim fname = Encoding.UTF8.GetBytes(srcfile)
            Dim filename = builder.Add(fname)
            Dim mldsa_algo = CByte(NetServicesCrypto.FromMLDsaAlgorithm(algo))

            Dim pubk = kpip.PubKey
            Dim sign_length As Int32 = 0
            Dim sign_ptr As IntPtr = IntPtr.Zero

            Dim err = Native.PqcMlDsaSignFileAot(
              filename, fname.Length,
              privk.Ptr, privk.Length,
              mldsa_algo,
              sign_ptr, sign_length)
            AssertError(err)

            Dim signature = ToBytes(sign_ptr, sign_length)
            Native.FreeBuffer(sign_ptr)

            Dim pub_key = builder.Add(pubk)
            Dim sign = builder.Add(signature)

            Dim verify = Native.PqcMlDsaVerifyFileAot(
              filename, fname.Length,
              pub_key, pubk.Length,
              sign, signature.Length, err)
            AssertError(err)

            If Not verify Then
              Throw New Exception()
            End If

          End Using
        End Using

        If i Mod rounds / 10 = 0 Then
          Console.Write(".")
        End If
      Next

      sw.Stop()

      Dim t = sw.ElapsedMilliseconds
      If File.Exists(srcfile) Then File.Delete(srcfile)
      Console.WriteLine($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms")
    End Sub

    Private Sub TestPqcMlDsaSingleSignatureKpiSaveLoadFile(rounds As Int32)

      'A new key pair is always created and saved. This takes time. 
      'For the temporary exchange, it would not be necessary to save
      'the keys, as all generated keys are only required for the
      'respective session.

      'Es wird immer wieder ein neuer Schlüsselpaar erstellt und
      'abgespeichert. Das braucht seine Zeit. 

      'Für den temporären Austausch, wäre das Abspeichern der
      'Schlüssel nicht notwending, da alle generierten Schlüssel
      'nur für die jeweilige Sitzung benötigt werden.

      Console.Write($"{NameOf(TestPqcMlDsaSingleSignatureKpiSaveLoadFile)}: ")

      If rounds < 10 Then rounds = 10

      Dim srcfile = "data"
      Dim algos = NetServicesCrypto.ToMLDsaAlgorithm()

      Dim rand = Random.[Shared]
      Dim sw = Stopwatch.StartNew()

      For i = 0 To rounds - 1
        'Create a rng-testfile 
        Dim max = (1 << 21) + 1024 'ca. 2Mb
        Dim size = Random.[Shared].[Next](max)
        SetRngFileData(srcfile, size)

        'ML-DSA Parameter select. Only the first 3 parameters
        Dim idx = rand.[Next](algos.Length)

        'Create and Save a legal ML-DSA-KeyPair
        Dim algo = algos(idx)

        'Create And Save a legal ML-DSA-KeyPair
        Dim kpip As KeyPairParamInfo = Nothing

        Dim kpfile = "mldsa_keypair.key"
        Dim rnum = rand.[Next](Int32.MaxValue)
        If Int32.IsEvenInteger(rnum) Then
          kpip = CreateNativeMlDsaKeyPair()
          algo = kpip.Algo
        Else
          kpip = CreateNativeMlDsaKeyPair(algo)
        End If

        Using privk = kpip.PrivKey
          Using builder = New NativeArrayBuilder

            Dim pubk = kpip.PubKey
            Dim mldsainfo = New MlDsaKeyPairInfo(
              pubk, privk.ToBytes(), algo)
            mldsainfo.SaveKeyPair(kpfile, True)

            'Load KeyPairs again
            Dim info = MlDsaKeyPairInfo.Load_KeyPair(kpfile)
            If Not mldsainfo.Equals(info) Then Throw New Exception()

            Dim sign_length As Int32 = Nothing
            Dim sign_ptr As IntPtr = IntPtr.Zero

            Dim fname = Encoding.UTF8.GetBytes(srcfile)
            Dim filename = builder.Add(fname)

            Dim mldsa_algo = CByte(NetServicesCrypto.FromMLDsaAlgorithm(algo))

            Dim err = Native.PqcMlDsaSignFileAot(
              filename, fname.Length,
              privk.Ptr, privk.Length,
              mldsa_algo,
              sign_ptr, sign_length)
            AssertError(err)

            Dim signature = ToBytes(sign_ptr, sign_length)
            Native.FreeBuffer(sign_ptr)

            Dim pub_key = builder.Add(pubk)
            Dim sign = builder.Add(signature)

            Dim verify = Native.PqcMlDsaVerifyFileAot(
              filename, fname.Length,
              pub_key, pubk.Length,
              sign, signature.Length, err)
            AssertError(err)

            If Not verify Then
              Throw New Exception()
            End If

          End Using
        End Using

        If i Mod rounds / 10 = 0 Then
          Console.Write(".")
        End If
      Next

      sw.Stop()

      Dim t = sw.ElapsedMilliseconds
      If File.Exists(srcfile) Then File.Delete(srcfile)
      Console.WriteLine($" rounds = {rounds}; t = {t}ms; td = {t / rounds}ms")
    End Sub

    Private Function CreateNativeMlDsaKeyPair() As KeyPairParamInfo

      Dim mldsa_param As Byte = Nothing
      Dim pub_key_length As Int32 = Nothing
      Dim guid_id_length As Int32 = Nothing
      Dim priv_key_length As Int32 = Nothing

      Dim pub_key_ptr As IntPtr = IntPtr.Zero
      Dim guid_id_ptr As IntPtr = IntPtr.Zero
      Dim priv_key_ptr As IntPtr = IntPtr.Zero

      Dim err = Native.CreateMlDsaKeyPairAot(
        priv_key_ptr, priv_key_length,
        pub_key_ptr, pub_key_length,
        guid_id_ptr, guid_id_length,
        mldsa_param)
      AssertError(err)

      Dim privkey = New UsIPtr(Of Byte)(
        ToBytes(priv_key_ptr, priv_key_length))
      Native.FreeBuffer(priv_key_ptr)

      Dim pubkey = ToBytes(pub_key_ptr, pub_key_length)
      Native.FreeBuffer(pub_key_ptr)

      Dim guid = ToBytes(guid_id_ptr, guid_id_length)
      Native.FreeBuffer(guid_id_ptr)

      Dim param = NetServicesCrypto.ToMLDsaAlgorithm(
        CType(mldsa_param, MLDsaParam))

      Return New KeyPairParamInfo() With
      {
        .Guid = guid,
        .Algo = param,
        .PubKey = pubkey,
        .PrivKey = privkey
      }
    End Function

    Private Function CreateNativeMlDsaKeyPair(
      algo As MLDsaAlgorithm) As KeyPairParamInfo

      Dim pub_key_length As Int32 = Nothing
      Dim guid_id_length As Int32 = Nothing
      Dim priv_key_length As Int32 = Nothing

      Dim pub_key_ptr As IntPtr = IntPtr.Zero
      Dim guid_id_ptr As IntPtr = IntPtr.Zero
      Dim priv_key_ptr As IntPtr = IntPtr.Zero

      Dim mldsa_algo = CByte(NetServicesCrypto.FromMLDsaAlgorithm(algo))
      Dim err = Native.CreateMlDsaKeyPairParamAot(
        mldsa_algo,
        priv_key_ptr, priv_key_length,
        pub_key_ptr, pub_key_length,
        guid_id_ptr, guid_id_length)
      AssertError(err)

      Dim privkey = New UsIPtr(Of Byte)(
        ToBytes(priv_key_ptr, priv_key_length))
      Native.FreeBuffer(priv_key_ptr)

      Dim pubkey = ToBytes(pub_key_ptr, pub_key_length)
      Native.FreeBuffer(pub_key_ptr)

      Dim guid = ToBytes(guid_id_ptr, guid_id_length)
      Native.FreeBuffer(guid_id_ptr)

      Return New KeyPairParamInfo() With {
          .Guid = guid,
          .Algo = algo,
          .PubKey = pubkey,
          .PrivKey = privkey
        }
    End Function

    Private Class KeyPairParamInfo
      Implements IDisposable

      Public Property IsDisposed As Boolean = False
      Public Property Algo As MLDsaAlgorithm = Nothing
      Public Property Guid As Byte() = Array.Empty(Of Byte)
      Public Property PubKey As Byte() = Array.Empty(Of Byte)
      Public Property PrivKey As UsIPtr(Of Byte) = UsIPtr(Of Byte).Empty

      Public Sub Clear()
        If Me.IsDisposed Then Return

        Me.PrivKey?.Dispose()

        Me.Algo = Nothing
        Me.Guid = Array.Empty(Of Byte)
        Me.PubKey = Array.Empty(Of Byte)
        Me.PrivKey = UsIPtr(Of Byte).Empty
      End Sub

      Private Sub Dispose(disposing As Boolean)
        If Not Me.IsDisposed Then
          If disposing Then
            Me.Clear()
          End If
        End If
        Me.IsDisposed = True
      End Sub

      Protected Overrides Sub Finalize()
        Me.Dispose(False)
        MyBase.Finalize()
      End Sub

      Public Sub Dispose() Implements IDisposable.Dispose
        Me.Dispose(True)
        GC.SuppressFinalize(Me)
      End Sub
    End Class

  End Module
End Namespace