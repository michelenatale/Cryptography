Option Strict On
Option Explicit On

Imports System.IO
Imports System.Text
Imports michele.natale
Imports michele.natale.Pointers
Imports System.Runtime.InteropServices

Namespace michele.natale.Tests

  Partial Module CryptoPqcMlDsaTest

    Public Sub TestPqcMlDsaMultiSignKpf(rounds As Int32)
      Console.Write($"{NameOf(TestPqcMlDsaMultiSignKpf)}Aot: ")

      'Ein bisschen anders gelöst, über 'GCHandle.Pinned'
      'Handled a little differently, via 'GCHandle.Pinned'

      Dim folder = "keypairs"
      Dim rand = Random.Shared
      Dim sw As New Stopwatch()
      Dim signercounttotal As Int64 = 0
      Dim projectname = "Project ML-DSA-Multi-Sign-Test"

      For i = 0 To rounds - 1
        Dim signatories = rand.Next(3, 16)
        signercounttotal += signatories

        Dim signernames = Enumerable.Range(0, signatories).
          Select(Function(idx) $"Signer_{idx}").ToArray()

        Dim size = rand.Next(10, 128)
        Dim message(size - 1) As Byte
        rand.NextBytes(message)

        Dim dict = CreateMlDsaKeyPairAndSave(signernames, folder)

        sw.Start()

        Dim kppis = MlDsaKeyPairLoad(folder)
        Dim signerinfos = ExtractDataAndSort(kppis, dict, message, projectname)

        Dim guids = signerinfos.Select(Function(x) x.SignerID).ToArray()
        Dim signs = signerinfos.Select(Function(x) x.Signature).ToArray()
        Dim pubkeys = signerinfos.Select(Function(x) x.PublicKey).ToArray()
        Dim signnames = signerinfos.Select(Function(x) x.SignerName).ToArray()
        Dim signalgo = signerinfos.Select(Function(x) x.PqcSignAlgo).ToArray()
        Dim projectnames = signerinfos.Select(Function(x) x.ProjectName).ToArray()
        Dim signparams = signerinfos.Select(Function(x) x.PqcSignAlgoParam).ToArray()

        Using guidbuilder As New NativeArrayBuilder(),
          signbuilder As New NativeArrayBuilder(),
          pubbuilder As New NativeArrayBuilder(),
          namebuilder As New NativeArrayBuilder(),
          projbuilder As New NativeArrayBuilder()

          Dim guid_ptr = guidbuilder.Add(guids)
          Dim guid_lengths = guidbuilder.GetLengthsPtr()

          Dim sign_ptr = signbuilder.Add(signs)
          Dim sign_lengths = signbuilder.GetLengthsPtr()

          Dim pubkey_ptr = pubbuilder.Add(pubkeys)
          Dim pubkey_lengths = pubbuilder.GetLengthsPtr()

          Dim signname_ptr = namebuilder.Add(signnames)
          Dim signname_lengths = namebuilder.GetLengthsPtr()

          Dim projectname_ptr = projbuilder.Add(projectnames)
          Dim projectname_lengths = projbuilder.GetLengthsPtr()

          ' message, algo, params: flache Arrays → pinnen
          Dim msghandle = GCHandle.Alloc(message, GCHandleType.Pinned)
          Dim msg_ptr = msgHandle.AddrOfPinnedObject()
          Dim msg_length = message.Length

          Dim algohandle = GCHandle.Alloc(signalgo, GCHandleType.Pinned)
          Dim signalgo_ptr = algoHandle.AddrOfPinnedObject()
          Dim signalgo_length = signalgo.Length

          Dim paramhandle = GCHandle.Alloc(signparams, GCHandleType.Pinned)
          Dim signparams_ptr = paramHandle.AddrOfPinnedObject()
          Dim signparams_length = signparams.Length

          Dim multi_sign_length As Int32
          Dim multi_pubkey_length As Int32
          Dim multi_privkey_length As Int32

          Dim multi_sign_ptr As IntPtr = IntPtr.Zero
          Dim multi_pubkey_ptr As IntPtr = IntPtr.Zero
          Dim multi_privkey_ptr As IntPtr = IntPtr.Zero

          Dim err = Native.PqcMlDsaMultiSignAot(
            msg_ptr, msg_length,
            guid_ptr, guid_lengths, guids.Length,
            sign_ptr, sign_lengths, signs.Length,
            pubkey_ptr, pubkey_lengths, pubkeys.Length,
            signname_ptr, signname_lengths, signnames.Length,
            projectname_ptr, projectname_lengths, projectnames.Length,
            signalgo_ptr, signalgo_length,
            signparams_ptr, signparams_length,
            multi_sign_ptr, multi_sign_length,
            multi_privkey_ptr, multi_privkey_length,
            multi_pubkey_ptr, multi_pubkey_length)
          AssertError(err)

          msghandle.Free() : algohandle.Free() : paramhandle.Free()

          Dim multi_sign = ToBytes(multi_sign_ptr, multi_sign_length)
          Native.FreeBuffer(multi_sign_ptr)

          Dim multi_prikey = ToBytes(multi_privkey_ptr, multi_privkey_length)
          Native.FreeBuffer(multi_privkey_ptr)

          Dim multi_pubkey = ToBytes(multi_pubkey_ptr, multi_pubkey_length)
          Native.FreeBuffer(multi_pubkey_ptr)

          msghandle = GCHandle.Alloc(message, GCHandleType.Pinned)
          Dim sighandle = GCHandle.Alloc(multi_sign, GCHandleType.Pinned)
          Dim pubhandle = GCHandle.Alloc(multi_pubkey, GCHandleType.Pinned)

          Dim verify = Native.PqcMlDsaVerifyAot(
            msghandle.AddrOfPinnedObject(), message.Length,
            pubhandle.AddrOfPinnedObject(), multi_pubkey.Length,
            sighandle.AddrOfPinnedObject(), multi_sign.Length,
            err)
          AssertError(err)

          msghandle.Free() : pubhandle.Free() : sighandle.Free()

          If Not verify Then
            Throw New Exception("Verification failed")
          End If
        End Using

        sw.Stop()

        If rounds > 0 AndAlso i Mod Math.Max(1, rounds \ 10) = 0 Then
          Console.Write(".")
        End If
      Next

      Dim t = sw.ElapsedMilliseconds
      Dim ssize = signercounttotal / rounds
      Console.WriteLine($" rounds = {rounds}; signers = {ssize}; t = {t}ms; td = {t / CDbl(rounds)}ms")
    End Sub

    Public Sub TestPqcMlDsaMultiSignFileKpf(rounds As Int32)
      Console.Write($"{NameOf(TestPqcMlDsaMultiSignFileKpf)}Aot: ")

      'Ein bisschen anders gelöst, über 'GCHandle.Pinned'
      'Handled a little differently, via 'GCHandle.Pinned'

      Dim srcfile = "data"
      Dim filesizetotal As UInt64 = 0
      Dim folder = "keypairs"
      Dim rand = Random.Shared
      Dim sw As New Stopwatch()
      Dim signercounttotal As Int64 = 0
      Dim projectname = "Project ML-DSA-Multi-Sign-File-Test"

      For i = 0 To rounds - 1
        Dim signatories = rand.Next(3, 16)
        signercounttotal += signatories

        Dim signernames = Enumerable.Range(0, signatories).
          Select(Function(idx) $"Signer_{idx}").ToArray()

        Dim max = (1 << 20) + 1024
        Dim size = Random.Shared.Next(1000, max)
        SetRngFileData(srcfile, size)
        filesizetotal += CULng(size)

        Dim dict = CreateMlDsaKeyPairAndSave(signernames, folder)

        sw.Start()

        Dim kppis = MlDsaKeyPairLoad(folder)
        Dim signerinfos = ExtractDataAndSort(kppis, dict, srcfile, projectname)

        Dim guids = signerinfos.Select(Function(x) x.SignerID).ToArray()
        Dim signs = signerinfos.Select(Function(x) x.Signature).ToArray()
        Dim pubkeys = signerinfos.Select(Function(x) x.PublicKey).ToArray()
        Dim signnames = signerinfos.Select(Function(x) x.SignerName).ToArray()
        Dim signalgo = signerinfos.Select(Function(x) x.PqcSignAlgo).ToArray() 'here mldsa
        Dim projectnames = signerinfos.Select(Function(x) x.ProjectName).ToArray()
        Dim signparams = signerinfos.Select(Function(x) x.PqcSignAlgoParam).ToArray()

        Using guidbuilder As New NativeArrayBuilder(),
          signbuilder As New NativeArrayBuilder(),
          pubbuilder As New NativeArrayBuilder(),
          namebuilder As New NativeArrayBuilder(),
          projbuilder As New NativeArrayBuilder()

          Dim guid_ptr = guidbuilder.Add(guids)
          Dim guid_lengths = guidbuilder.GetLengthsPtr()

          Dim sign_ptr = signbuilder.Add(signs)
          Dim sign_lengths = signbuilder.GetLengthsPtr()

          Dim pubkey_ptr = pubbuilder.Add(pubkeys)
          Dim pubkey_lengths = pubbuilder.GetLengthsPtr()

          Dim signname_ptr = namebuilder.Add(signnames)
          Dim signname_lengths = namebuilder.GetLengthsPtr()

          Dim projectname_ptr = projbuilder.Add(projectnames)
          Dim projectname_lengths = projbuilder.GetLengthsPtr()

          Dim fname = Encoding.UTF8.GetBytes(srcfile)
          Dim fhandle = GCHandle.Alloc(fname, GCHandleType.Pinned)
          Dim file_name_ptr = fhandle.AddrOfPinnedObject()
          Dim file_name_lengths = fname.Length

          Dim algohandle = GCHandle.Alloc(signalgo, GCHandleType.Pinned)
          Dim signalgo_ptr = algohandle.AddrOfPinnedObject()
          Dim signalgo_lengths = signalgo.Length

          Dim paramhandle = GCHandle.Alloc(signparams, GCHandleType.Pinned)
          Dim signparams_ptr = paramhandle.AddrOfPinnedObject()
          Dim signparams_lengths = signparams.Length

          Dim multi_sign_length As Int32 = 0
          Dim multi_pubkey_length As Int32 = 0
          Dim multi_privkey_length As Int32 = 0

          Dim multi_sign_ptr As IntPtr = IntPtr.Zero
          Dim multi_pubkey_ptr As IntPtr = IntPtr.Zero
          Dim multi_privkey_ptr As IntPtr = IntPtr.Zero

          Dim err = Native.PqcMlDsaMultiSignFileAot(
            file_name_ptr, file_name_lengths,
            guid_ptr, guid_lengths, guids.Length,
            sign_ptr, sign_lengths, signs.Length,
            pubkey_ptr, pubkey_lengths, pubkeys.Length,
            signname_ptr, signname_lengths, signnames.Length,
            projectname_ptr, projectname_lengths, projectnames.Length,
            signalgo_ptr, signalgo_lengths,
            signparams_ptr, signparams_lengths,
            multi_sign_ptr, multi_sign_length,
            multi_privkey_ptr, multi_privkey_length,
            multi_pubkey_ptr, multi_pubkey_length)
          AssertError(err)

          fhandle.Free() : algohandle.Free() : paramhandle.Free()

          Dim multi_sign = ToBytes(multi_sign_ptr, multi_sign_length)
          Native.FreeBuffer(multi_sign_ptr)

          Dim multi_prikey = ToBytes(multi_privkey_ptr, multi_privkey_length)
          Native.FreeBuffer(multi_privkey_ptr)

          Dim multi_pubkey = ToBytes(multi_pubkey_ptr, multi_pubkey_length)
          Native.FreeBuffer(multi_pubkey_ptr)

          fhandle = GCHandle.Alloc(fname, GCHandleType.Pinned)
          Dim sighandle = GCHandle.Alloc(multi_sign, GCHandleType.Pinned)
          Dim pubhandle = GCHandle.Alloc(multi_pubkey, GCHandleType.Pinned)

          Dim verify = Native.PqcMlDsaVerifyFileAot(
            fhandle.AddrOfPinnedObject(), fname.Length,
            pubhandle.AddrOfPinnedObject(), multi_pubkey.Length,
            sighandle.AddrOfPinnedObject(), multi_sign.Length,
            err)
          AssertError(err)

          fhandle.Free() : pubhandle.Free() : sighandle.Free()

          If Not verify Then
            Throw New Exception("Verification failed")
          End If
        End Using

        sw.Stop()

        If rounds > 0 AndAlso i Mod Math.Max(1, rounds \ 10) = 0 Then
          Console.Write(".")
        End If
      Next

      Dim t = sw.ElapsedMilliseconds
      Dim ssize = signercounttotal / rounds
      Dim fsize = Math.Round((filesizetotal / 1024.0) / rounds, 3)
      Console.WriteLine($" rounds = {rounds}; signers = {ssize}; filesize = {fsize}kb; t = {t}ms; td = {t / CDbl(rounds)}ms")
    End Sub

    Private Function ToMlDsaSignerInfo(
      kppi As KeyPairParamInfo, message As Byte(),
      signername As String, projectname As String) As MlDsaSignerInfo

      Dim priv = kppi.PrivKey.ToBytes()
      Dim privhandle = GCHandle.Alloc(priv, GCHandleType.Pinned)
      Dim msghandle = GCHandle.Alloc(message, GCHandleType.Pinned)

      Dim signlength As Int32 = 0
      Dim sign_ptr As IntPtr = IntPtr.Zero

      Dim err = Native.PqcMlDsaSignAot(
        msghandle.AddrOfPinnedObject(), message.Length,
        privhandle.AddrOfPinnedObject(), priv.Length,
        CByte(NetServicesCrypto.FromMLDsaAlgorithm(kppi.Algo)),
        sign_ptr, signlength)
      AssertError(err)

      Dim result As New MlDsaSignerInfo() With {
        .Signature = ToBytes(sign_ptr, signlength)
      }
      Native.FreeBuffer(sign_ptr)

      msghandle.Free() : privhandle.Free()

      result.SignerID = kppi.Guid
      result.PublicKey = kppi.PubKey
      result.PqcSignAlgo = CByte(PqcSignAlgo.ML_DSA)
      result.SignerName = Encoding.UTF8.GetBytes(signername)
      result.ProjectName = Encoding.UTF8.GetBytes(projectname)
      result.PqcSignAlgoParam = CByte(NetServicesCrypto.FromMLDsaAlgorithm(kppi.Algo))

      Return result
    End Function

    Private Function ToMlDsaSignerInfo(
      kppi As KeyPairParamInfo, filedata As String,
      signername As String, projectname As String) As MlDsaSignerInfo

      Dim filename = Encoding.UTF8.GetBytes(filedata)
      Dim fhandle = GCHandle.Alloc(filename, GCHandleType.Pinned)

      Dim priv = kppi.PrivKey.ToBytes()
      Dim privhandle = GCHandle.Alloc(priv, GCHandleType.Pinned)

      Dim signlength As Int32 = 0
      Dim sign_ptr As IntPtr = IntPtr.Zero

      Dim err = Native.PqcMlDsaSignFileAot(
        fhandle.AddrOfPinnedObject(), filename.Length,
        privhandle.AddrOfPinnedObject(), priv.Length,
        CByte(NetServicesCrypto.FromMLDsaAlgorithm(kppi.Algo)),
        sign_ptr, signlength)
      AssertError(err)

      Dim result As New MlDsaSignerInfo() With {
        .Signature = ToBytes(sign_ptr, signlength)
      }
      Native.FreeBuffer(sign_ptr)

      fhandle.Free() : privhandle.Free()

      result.SignerID = kppi.Guid
      result.PublicKey = kppi.PubKey
      result.PqcSignAlgo = CByte(PqcSignAlgo.ML_DSA)
      result.SignerName = Encoding.UTF8.GetBytes(signername)
      result.ProjectName = Encoding.UTF8.GetBytes(projectname)
      result.PqcSignAlgoParam = CByte(NetServicesCrypto.FromMLDsaAlgorithm(kppi.Algo))

      Return result
    End Function

    Private Function CreateMlDsaKeyPairAndSave(
        signernames As String(),
        folder As String) As Dictionary(Of Guid, String)

      If Directory.Exists(folder) Then
        Directory.Delete(folder, True)
      End If

      If Not Directory.Exists(folder) Then
        Directory.CreateDirectory(folder)
      End If

      Dim dict As New Dictionary(Of Guid, String)()

      For Each name In signernames
        Dim kppi = CreateNativeMlDsaKeyPair()

        Dim kpfile = Encoding.UTF8.GetBytes(
          $"{folder}\mldsa_keypair-{name.ToLower()}.key")

        Dim priv = kppi.PrivKey.ToBytes()
        Dim kphandle = GCHandle.Alloc(kpfile, GCHandleType.Pinned)
        Dim privhandle = GCHandle.Alloc(priv, GCHandleType.Pinned)
        Dim pubhandle = GCHandle.Alloc(kppi.PubKey, GCHandleType.Pinned)
        Dim guidhandle = GCHandle.Alloc(kppi.Guid, GCHandleType.Pinned)

        Dim err = Native.SavePqcMlDsaKeyPairAot(
          kphandle.AddrOfPinnedObject(), kpfile.Length,
          privhandle.AddrOfPinnedObject(), priv.Length,
          pubhandle.AddrOfPinnedObject(), kppi.PubKey.Length,
          guidhandle.AddrOfPinnedObject(), kppi.Guid.Length,
          CByte(NetServicesCrypto.FromMLDsaAlgorithm(kppi.Algo)),
          True)
        AssertError(err)

        kphandle.Free() : privhandle.Free()
        pubhandle.Free() : guidhandle.Free()

        dict(New Guid(kppi.Guid)) = name
      Next

      Return dict
    End Function

    Private Function MlDsaKeyPairLoad(folder As String) As KeyPairParamInfo()
      Dim files = Directory.GetFiles(folder, "*").
            OrderBy(Function(x) x).ToArray()

      Dim result As New List(Of KeyPairParamInfo)(files.Length)

      For Each fname In files
        Dim filename = Encoding.UTF8.GetBytes(fname)
        Dim fhandle = GCHandle.Alloc(filename, GCHandleType.Pinned)

        Dim mldsa_param As Byte = 0

        Dim pub_key_length As Int32 = 0
        Dim guid_id_length As Int32 = 0
        Dim priv_key_length As Int32 = 0

        Dim guid_id_ptr As IntPtr = IntPtr.Zero
        Dim pub_key_ptr As IntPtr = IntPtr.Zero
        Dim priv_key_ptr As IntPtr = IntPtr.Zero

        Dim err = Native.LoadPqcMlDsaKeyPairAot(
          fhandle.AddrOfPinnedObject(), filename.Length,
          priv_key_ptr, priv_key_length,                  'out
          pub_key_ptr, pub_key_length,                    'out
          guid_id_ptr, guid_id_length,                    'out
          mldsa_param)                                    'out
        AssertError(err)

        fhandle.Free()
        Dim kppi As New KeyPairParamInfo With {
          .PrivKey = New UsIPtr(Of Byte)(
            ToBytes(priv_key_ptr, priv_key_length))
        }
        Native.FreeBuffer(priv_key_ptr)

        kppi.PubKey = ToBytes(pub_key_ptr, pub_key_length)
        Native.FreeBuffer(pub_key_ptr)

        kppi.Guid = ToBytes(guid_id_ptr, guid_id_length)
        Native.FreeBuffer(guid_id_ptr)

        kppi.Algo = NetServicesCrypto.
          ToMLDsaAlgorithm(CType(mldsa_param, MLDsaParam))

        result.Add(kppi)
      Next

      Return result.ToArray()
    End Function

    Private Function ExtractDataAndSort(
        kppis As KeyPairParamInfo(),
        dict As Dictionary(Of Guid, String),
        srcfile As String,
        projectname As String) As MlDsaSignerInfo()

      Return kppis.
        Select(Function(k, idx) ToMlDsaSignerInfo(
          k, srcfile,
          dict(New Guid(k.Guid)), projectname)).
            OrderBy(Function(x) x.Signature,
              Comparer(Of Byte()).Create(
                Function(a, b)
                  Dim len = Math.Min(a.Length, b.Length)
                  For i = 0 To len - 1
                    Dim diff = a(i).CompareTo(b(i))
                    If diff <> 0 Then Return diff
                  Next
                  Return a.Length.CompareTo(b.Length)
                End Function)).ToArray()
    End Function

    Private Function ExtractDataAndSort(
        kppis As KeyPairParamInfo(),
        dict As Dictionary(Of Guid, String),
        message As Byte(),
        projectname As String) As MlDsaSignerInfo()

      Return kppis.
        Select(Function(k, idx) ToMlDsaSignerInfo(
          k, message,
          dict(New Guid(k.Guid)), projectname)).
            OrderBy(Function(x) x.Signature,
              Comparer(Of Byte()).Create(
                Function(a, b)
                  Dim len = Math.Min(a.Length, b.Length)
                  For i = 0 To len - 1
                    Dim diff = a(i).CompareTo(b(i))
                    If diff <> 0 Then Return diff
                  Next
                  Return a.Length.CompareTo(b.Length)
                End Function)).ToArray()
    End Function

  End Module

End Namespace
