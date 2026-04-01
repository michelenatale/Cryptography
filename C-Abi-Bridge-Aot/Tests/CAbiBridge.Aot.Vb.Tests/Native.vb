Option Strict On
Option Explicit On

Imports michele.natale
Imports System.Runtime.InteropServices

Namespace michele.natale.Tests
  Friend Module Native

    Const DllName As String = "C-Abi-Bridge.Aot.N.dll"

    <DllImport(DllName, EntryPoint:="free_buffer_aot")>
    Public Sub FreeBuffer(ptr As IntPtr)
    End Sub


#Region "Crypto"

    <DllImport(DllName, EntryPoint:="aes_encrypt_aot")>
    Public Function AesEncryptAot(
      bytes As Byte(), bytes_length As Int32,
      key As IntPtr, key_length As Int32,
      associated As Byte(), associated_length As Int32,
      <Out> ByRef output As IntPtr, <Out> ByRef output_length As Int32) As CError
    End Function

    <DllImport(DllName, EntryPoint:="aes_decrypt_aot")>
    Public Function AesDecryptAot(
      bytes As Byte(), bytes_length As Int32,
      key As IntPtr, key_length As Int32,
      associated As Byte(), associated_length As Int32,
      <Out> ByRef output As IntPtr, <Out> ByRef output_length As Int32) As CError
    End Function

    <DllImport(DllName, EntryPoint:="aes_encrypt_file_aot")>
    Public Function AesEncryptFileAot(
      src As Byte(), src_length As Int32,
      dest As Byte(), dest_length As Int32,
      key As IntPtr, key_length As Int32,
      associated As Byte(), associated_length As Int32) As CError
    End Function

    <DllImport(DllName, EntryPoint:="aes_decrypt_file_aot")>
    Public Function AesDecryptFileAot(
      src As Byte(), src_length As Int32,
      dest As Byte(), dest_length As Int32,
      key As IntPtr, key_length As Int32,
      associated As Byte(), associated_length As Int32) As CError
    End Function

    <DllImport(DllName, EntryPoint:="aes_gcm_encrypt_aot")>
    Public Function AesGcmEncryptAot(
      bytes As Byte(), bytes_length As Int32,
      key As IntPtr, key_length As Int32,
      associated As Byte(), associated_length As Int32,
      <Out> ByRef output As IntPtr, <Out> ByRef output_length As Int32) As CError
    End Function

    <DllImport(DllName, EntryPoint:="aes_gcm_decrypt_aot")>
    Public Function AesGcmDecryptAot(bytes As Byte(), bytes_length As Int32, key As IntPtr, key_length As Int32, associated As Byte(), associated_length As Int32, <Out> ByRef output As IntPtr, <Out> ByRef output_length As Int32) As CError
    End Function

    <DllImport(DllName, EntryPoint:="aes_gcm_encrypt_file_aot")>
    Public Function AesGcmEncryptFileAot(
      src As Byte(), src_length As Int32,
      dest As Byte(), dest_length As Int32,
      key As IntPtr, key_length As Int32,
      associated As Byte(), associated_length As Int32) As CError
    End Function

    <DllImport(DllName, EntryPoint:="aes_gcm_decrypt_file_aot")>
    Public Function AesGcmDecryptFileAot(
      src As Byte(), src_length As Int32,
      dest As Byte(), dest_length As Int32,
      key As IntPtr, key_length As Int32,
      associated As Byte(), associated_length As Int32) As CError
    End Function

    <DllImport(DllName, EntryPoint:="chacha20_poly1305_encrypt_aot")>
    Public Function ChaCha20Poly1305EncryptAot(
      bytes As Byte(), bytes_length As Int32,
      key As IntPtr, key_length As Int32,
      associated As Byte(), associated_length As Int32,
      <Out> ByRef output As IntPtr, <Out> ByRef output_length As Int32) As CError
    End Function

    <DllImport(DllName, EntryPoint:="chacha20_poly1305_decrypt_aot")>
    Public Function ChaCha20Poly1305DecryptAot(
      bytes As Byte(), bytes_length As Int32,
      key As IntPtr, key_length As Int32,
      associated As Byte(), associated_length As Int32,
      <Out> ByRef output As IntPtr, <Out> ByRef output_length As Int32) As CError
    End Function

    <DllImport(DllName, EntryPoint:="chacha20_poly1305_encrypt_file_aot")>
    Public Function ChaCha20Poly1305EncryptFileAot(
      src As Byte(), src_length As Int32,
      dest As Byte(), dest_length As Int32,
      key As IntPtr, key_length As Int32,
      associated As Byte(), associated_length As Int32) As CError
    End Function

    <DllImport(DllName, EntryPoint:="chacha20_poly1305_decrypt_file_aot")>
    Public Function ChaCha20Poly1305DecryptFileAot(
      src As Byte(), src_length As Int32,
      dest As Byte(), dest_length As Int32,
      key As IntPtr, key_length As Int32,
      associated As Byte(), associated_length As Int32) As CError
    End Function

#End Region

#Region "Crypto Random"

    <DllImport(DllName, EntryPoint:="rng_crypto_bytes_aot")>
    Public Function RngCryptoBytesAot(
      size As Int32, <Out> ByRef out_ptr As IntPtr) As CError
    End Function

    <DllImport(DllName, EntryPoint:="fill_crypto_bytes_aot")>
    Public Function FillCryptoBytesAot(
      bytes As Byte(), length As Int32) As CError
    End Function


    <DllImport(DllName, EntryPoint:="next_crypto_bool_aot")>
    Public Function NextCryptoBoolAot(<Out> ByRef err As CError) As Boolean
    End Function

    <DllImport(DllName, EntryPoint:="rng_crypto_bool_aot")>
    Public Function RngCryptoBoolAot(
      size As Int32, <Out> ByRef out_ptr As IntPtr) As CError
    End Function

    <DllImport(DllName, EntryPoint:="next_crypto_int32_aot")>
    Public Function NextCryptoInt32Aot(<Out> ByRef err As CError) As Int32
    End Function

    <DllImport(DllName, EntryPoint:="next_crypto_int32_max_aot")>
    Public Function NextCryptoInt32MaxAot(
      max As Int32, <Out> ByRef err As CError) As Int32
    End Function

    <DllImport(DllName, EntryPoint:="next_crypto_int32_min_max_aot")>
    Public Function NextCryptoInt32MinMaxAot(
      min As Int32, max As Int32, <Out> ByRef err As CError) As Int32
    End Function

    <DllImport(DllName, EntryPoint:="rng_crypto_int32_aot")>
    Public Function RngCryptoInt32Aot(
      size As Int32, <Out> ByRef out_ptr As IntPtr) As CError
    End Function

    <DllImport(DllName, EntryPoint:="rng_crypto_int32_max_aot")>
    Public Function RngCryptoInt32MaxAot(
      size As Int32, max As Int32, <Out> ByRef out_ptr As IntPtr) As CError
    End Function

    <DllImport(DllName, EntryPoint:="rng_crypto_int32_min_max_aot")>
    Public Function RngCryptoInt32MinMaxAot(
      size As Int32, min As Int32,
      max As Int32, <Out> ByRef out_ptr As IntPtr) As CError
    End Function

    <DllImport(DllName, EntryPoint:="next_crypto_int64_aot")>
    Public Function NextCryptoInt64Aot(<Out> ByRef err As CError) As Int64
    End Function

    <DllImport(DllName, EntryPoint:="next_crypto_int64_max_aot")>
    Public Function NextCryptoInt64MaxAot(max As Int64, <Out> ByRef err As CError) As Int64
    End Function

    <DllImport(DllName, EntryPoint:="next_crypto_int64_min_max_aot")>
    Public Function NextCryptoInt64MinMaxAot(
      min As Int64, max As Int64, <Out> ByRef err As CError) As Int64
    End Function

    <DllImport(DllName, EntryPoint:="rng_crypto_int64_aot")>
    Public Function RngCryptoInt64Aot(
      size As Int32, <Out> ByRef out_ptr As IntPtr) As CError
    End Function

    <DllImport(DllName, EntryPoint:="rng_crypto_int64_max_aot")>
    Public Function RngCryptoInt64MaxAot(
      size As Int32, max As Int64, <Out> ByRef out_ptr As IntPtr) As CError
    End Function

    <DllImport(DllName, EntryPoint:="rng_crypto_int64_min_max_aot")>
    Public Function RngCryptoInt64MinMaxAot(
      size As Int32, min As Int64, max As Int64, <Out> ByRef out_ptr As IntPtr) As CError
    End Function

    <DllImport(DllName, EntryPoint:="next_crypto_double_aot")>
    Public Function NextCryptoDoubleAot(<Out> ByRef err As CError) As Double
    End Function

    <DllImport(DllName, EntryPoint:="next_crypto_double_max_aot")>
    Public Function NextCryptoDoubleMaxAot(
      max As Double, <Out> ByRef err As CError) As Double
    End Function

    <DllImport(DllName, EntryPoint:="next_crypto_double_min_max_aot")>
    Public Function NextCryptoDoubleMinMaxAot(
      min As Double, max As Double, <Out> ByRef err As CError) As Double
    End Function

    <DllImport(DllName, EntryPoint:="rng_crypto_double_aot")>
    Public Function RngCryptoDoubleAot(
      size As Int32, <Out> ByRef out_ptr As IntPtr) As CError
    End Function

    <DllImport(DllName, EntryPoint:="rng_crypto_double_max_aot")>
    Public Function RngCryptoDoubleMaxAot(
      size As Int32, max As Double, <Out> ByRef out_ptr As IntPtr) As CError
    End Function

    <DllImport(DllName, EntryPoint:="rng_crypto_double_min_max_aot")>
    Public Function RngCryptoDoubleMinMaxAot(
      size As Int32, min As Double,
      max As Double, <Out> ByRef out_ptr As IntPtr) As CError
    End Function


    <DllImport(DllName, EntryPoint:="next_crypto_single_aot")>
    Public Function NextCryptoSingleAot(<Out> ByRef err As CError) As Single
    End Function

    <DllImport(DllName, EntryPoint:="next_crypto_single_max_aot")>
    Public Function NextCryptoSingleMaxAot(
      max As Single, <Out> ByRef err As CError) As Single
    End Function

    <DllImport(DllName, EntryPoint:="next_crypto_single_min_max_aot")>
    Public Function NextCryptoSingleMinMaxAot(
      min As Single, max As Single, <Out> ByRef err As CError) As Single
    End Function

    <DllImport(DllName, EntryPoint:="rng_crypto_single_aot")>
    Public Function RngCryptoSingleAot(
      size As Int32, <Out> ByRef out_ptr As IntPtr) As CError
    End Function

    <DllImport(DllName, EntryPoint:="rng_crypto_single_max_aot")>
    Public Function RngCryptoSingleMaxAot(
      size As Int32, max As Single, <Out> ByRef out_ptr As IntPtr) As CError
    End Function

    <DllImport(DllName, EntryPoint:="rng_crypto_single_min_max_aot")>
    Public Function RngCryptoSingleMinMaxAot(
      size As Int32, min As Single,
      max As Single, <Out> ByRef out_ptr As IntPtr) As CError
    End Function

    <DllImport(DllName, EntryPoint:="next_crypto_decimal_aot")>
    Public Function NextCryptoDecimalAot(<Out> ByRef err As CError) As Decimal
    End Function

    <DllImport(DllName, EntryPoint:="next_crypto_decimal_max_aot")>
    Public Function NextCryptoDecimalMaxAot(
      max As Decimal, <Out> ByRef err As CError) As Decimal
    End Function

    <DllImport(DllName, EntryPoint:="next_crypto_decimal_min_max_aot")>
    Public Function NextCryptoDecimalMinMaxAot(
      min As Decimal, max As Decimal, <Out> ByRef err As CError) As Decimal
    End Function

    <DllImport(DllName, EntryPoint:="rng_crypto_decimal_aot")>
    Public Function RngCryptoDecimalAot(
      size As Int32, <Out> ByRef out_ptr As IntPtr) As CError
    End Function

    <DllImport(DllName, EntryPoint:="rng_crypto_decimal_max_aot")>
    Public Function RngCryptoDecimalMaxAot(
      size As Int32, max As Decimal, <Out> ByRef out_ptr As IntPtr) As CError
    End Function

    <DllImport(DllName, EntryPoint:="rng_crypto_decimal_min_max_aot")>
    Public Function RngCryptoDecimalMinMaxAot(
      size As Int32, min As Decimal,
      max As Decimal, <Out> ByRef out_ptr As IntPtr) As CError
    End Function

#End Region

#Region "Crypto Hash - Hmac"

#Region " Crypto Hash"

    <DllImport(DllName, EntryPoint:="sha_256_hash_data_aot")>
    Public Function Sha256HashDataAot(
       bytes As Byte(),
       bytes_length As Int32,
       <Out> ByRef output As IntPtr,
       <Out> ByRef output_length As Int32) As CError
    End Function

    <DllImport(DllName, EntryPoint:="sha_384_hash_data_aot")>
    Public Function Sha384HashDataAot(
       bytes As Byte(),
       bytes_length As Int32,
       <Out> ByRef output As IntPtr,
       <Out> ByRef output_length As Int32) As CError
    End Function

    <DllImport(DllName, EntryPoint:="sha_512_hash_data_aot")>
    Public Function Sha512HashDataAot(
       bytes As Byte(),
       bytes_length As Int32,
       <Out> ByRef output As IntPtr,
       <Out> ByRef output_length As Int32) As CError
    End Function

    <DllImport(DllName, EntryPoint:="sha1_hash_data_aot")>
    Public Function Sha1HashDataAot(
       bytes As Byte(),
       bytes_length As Int32,
       <Out> ByRef output As IntPtr,
       <Out> ByRef output_length As Int32) As CError
    End Function

    <DllImport(DllName, EntryPoint:="md5_hash_data_aot")>
    Public Function Md5HashDataAot(
       bytes As Byte(),
       bytes_length As Int32,
       <Out> ByRef output As IntPtr,
       <Out> ByRef output_length As Int32) As CError
    End Function

#End Region

#Region " Crypto Hmac"

    <DllImport(DllName, EntryPoint:="hmac_sha_256_hash_data_aot")>
    Public Function HmacSha256HashDataAot(
       bytes As Byte(),
       bytes_length As Int32,
       key As Byte(),
       key_length As Int32,
       <Out> ByRef output As IntPtr,
       <Out> ByRef output_length As Int32) As CError
    End Function

    <DllImport(DllName, EntryPoint:="hmac_sha_384_hash_data_aot")>
    Public Function HmacSha384HashDataAot(
       bytes As Byte(),
       bytes_length As Int32,
       key As Byte(),
       key_length As Int32,
       <Out> ByRef output As IntPtr,
       <Out> ByRef output_length As Int32) As CError
    End Function

    <DllImport(DllName, EntryPoint:="hmac_sha_512_hash_data_aot")>
    Public Function HmacSha512HashDataAot(
       bytes As Byte(),
       bytes_length As Int32,
       key As Byte(),
       key_length As Int32,
       <Out> ByRef output As IntPtr,
       <Out> ByRef output_length As Int32) As CError
    End Function

    <DllImport(DllName, EntryPoint:="hmac_sha1_hash_data_aot")>
    Public Function HmacSha1HashDataAot(
       bytes As Byte(),
       bytes_length As Int32,
       key As Byte(),
       key_length As Int32,
       <Out> ByRef output As IntPtr,
       <Out> ByRef output_length As Int32) As CError
    End Function

    <DllImport(DllName, EntryPoint:="hmac_md5_hash_data_aot")>
    Public Function HmacMd5HashDataAot(
       bytes As Byte(),
       bytes_length As Int32,
       key As Byte(),
       key_length As Int32,
       <Out> ByRef output As IntPtr,
       <Out> ByRef output_length As Int32) As CError
    End Function

#End Region

#End Region

#Region "Crypto PQC"

#Region "Crypto PQC Cryption"

#Region "Crypto PQC Cryption ML-KEM"

#Region "Crypto PQC Cryption ML-KEM Bytes"

    <DllImport(DllName, EntryPoint:="create_mlkem_key_pair_aot")>
    Public Function CreateMlKemKeyPairAot(
       <Out> ByRef priv_key_ptr As IntPtr, <Out> ByRef priv_key_length As Int32,
       <Out> ByRef pub_key_ptr As IntPtr, <Out> ByRef pub_key_length As Int32,
       <Out> ByRef guid_id_ptr As IntPtr, <Out> ByRef guid_id_length As Int32,
       <Out> ByRef mlkem_param As Byte, <Out> ByRef crypto_algo As Byte) As CError
    End Function

    <DllImport(DllName, EntryPoint:="create_mlkem_key_pair_param_aot")>
    Public Function CreateMlKemKeyPairParamAot(
       mlkem_param As Byte, crypto_algo As Byte,
       <Out> ByRef priv_key_ptr As IntPtr, <Out> ByRef priv_key_length As Int32,
       <Out> ByRef pub_key_ptr As IntPtr, <Out> ByRef pub_key_length As Int32,
       <Out> ByRef guid_id_ptr As IntPtr, <Out> ByRef guid_id_length As Int32) As CError
    End Function

    <DllImport(DllName, EntryPoint:="save_pqc_mlkem_key_pair_aot")>
    Public Function SavePqcMlKemKeyPairAot(
       src As Byte(), src_length As Int32,
       priv_key As Byte(), priv_key_length As Int32,
       pub_key As Byte(), pub_key_length As Int32,
       guid_id As Byte(), guid_id_length As Int32,
       mlkem_param As Byte, crypto_algo As Byte,
       <MarshalAs(UnmanagedType.U1)> save_private_key As Boolean) As CError
    End Function
    <DllImport(DllName, EntryPoint:="load_pqc_mlkem_key_pair_aot")>
    Public Function LoadPqcMlKemKeyPairAot(
       src As Byte(), src_length As Int32, <Out>
       ByRef priv_key_ptr As IntPtr, <Out> ByRef priv_key_length As Int32,
       <Out> ByRef pub_key_ptr As IntPtr, <Out> ByRef pub_key_length As Int32,
       <Out> ByRef guid_id_ptr As IntPtr, <Out> ByRef guid_id_length As Int32,
       <Out> ByRef mlkem_param As Byte, <Out> ByRef crypto_algo As Byte) As CError
    End Function

    <DllImport(DllName, EntryPoint:="to_pqc_mlkem_capsulation_from_pub_key_aot")>
    Public Function ToPqcMlKemCapsulationFromPubKeyAot(
       alice_public_key As Byte(), alice_public_key_length As Int32,
       mlkem_param As Byte,
       <Out> ByRef shared_key_ptr As IntPtr, <Out> ByRef shared_key_length As Int32,
       <Out> ByRef capsulation_ptr As IntPtr, <Out> ByRef capsulation_length As Int32) As CError
    End Function

    <DllImport(DllName, EntryPoint:="to_pqc_mlkem_shared_key_from_private_key_aot")>
    Public Function ToPqcMlKemSharedKeyFromPrivateKeyAot(
       alice_private_key As Byte(), alice_private_key_length As Int32,
       capsulation As Byte(), capsulation_length As Int32,
       mlkem_param As Byte,
       <Out> ByRef [shared] As IntPtr, <Out> ByRef shared_length As Int32) As CError
    End Function

    <DllImport(DllName, EntryPoint:="pqc_mlkem_encryption_aot")>
    Public Function PqcMlKemEncryptionAot(
       message As Byte(), message_length As Int32,
       private_key As Byte(), private_key_length As Int32,
       capsulation As Byte(), capsulation_length As Int32,
       associated As Byte(), associated_length As Int32,
       mlkem_param As Byte, crypto_algo As Byte,
       <Out> ByRef cipher As IntPtr, <Out> ByRef cipher_length As Int32) As CError
    End Function
    <DllImport(DllName, EntryPoint:="pqc_mlkem_decryption_aot")>
    Public Function PqcMlKemDecryptionAot(
       cipher As Byte(), cipher_length As Int32,
       shared_key As Byte(), shared_key_length As Int32,
       associated As Byte(), associated_length As Int32,
       mlkem_param As Byte, crypto_algo As Byte,
       <Out> ByRef decipher_ptr As IntPtr, <Out> ByRef decipher_length As Int32) As CError
    End Function
#End Region

#Region "Crypto PQC Cryption ML-KEM File"

    <DllImport(DllName, EntryPoint:="pqc_mlkem_encryption_file_aot")>
    Public Function PqcMlKemEncryptionFileAot(
       src_file As Byte(), src_file_length As Int32,
       dest_file As Byte(), dest_file_length As Int32,
       private_key As Byte(), private_key_length As Int32,
       capsulation As Byte(), capsulation_length As Int32,
       associated As Byte(), associated_length As Int32,
       mlkem_param As Byte, crypto_algo As Byte) As CError
    End Function

    <DllImport(DllName, EntryPoint:="pqc_mlkem_decryption_file_aot")>
    Public Function PqcMlKemDecryptionFileAot(
       src_file As Byte(), src_file_length As Int32,
       dest_file As Byte(), dest_file_length As Int32,
       shared_key As Byte(), shared_key_length As Int32,
       associated As Byte(), associated_length As Int32) As CError
    End Function

#End Region

#End Region

#End Region

#Region "Crypto PQC Signature"

#End Region

#End Region

#Region "Convert - Encoding"

#Region "Base646"

    <DllImport(DllName, EntryPoint:="to_base_64_utf8_aot")>
    Public Function ToBase64Aot(
      bytes As Byte(), bytes_length As Int32,
      <Out> ByRef output As IntPtr, <Out> ByRef output_length As Int32) As CError
    End Function

    <DllImport(DllName, EntryPoint:="from_base_64_utf8_aot")>
    Public Function FromBase64Aot(
      bytes As Byte(), bytes_length As Int32,
      <Out> ByRef output As IntPtr, <Out> ByRef output_length As Int32) As CError
    End Function

#End Region

#Region "Hex"

    <DllImport(DllName, EntryPoint:="to_hex_utf8_aot")>
    Public Function ToHexAot(
      bytes As Byte(), bytes_length As Int32,
      <Out> ByRef output As IntPtr, <Out> ByRef output_length As Int32,
      <MarshalAs(UnmanagedType.U1)> Optional lower As Boolean = True) As CError
    End Function

    <DllImport(DllName, EntryPoint:="from_hex_utf8_aot")>
    Public Function FromHexAot(
      bytes As Byte(), bytes_length As Int32,
      <Out> ByRef output As IntPtr, <Out> ByRef output_length As Int32) As CError
    End Function

#End Region

#End Region

#Region "Compress"

#End Region

#Region "Serialize"

#End Region

  End Module
End Namespace
