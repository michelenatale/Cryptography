
using System.Text;
using System.Security.Cryptography;

namespace michele.natale.EcCurveDsaDh;


public class AliceEcUser
{
  public const string UserName = "Alice";
  public static void StartAlice()
  {
    Console.WriteLine("Alice wants to send a secret message to Bob.");
    Console.WriteLine("Alice uses elliptic curve algorithms such as EcDsa, ");
    Console.WriteLine("EcDh and symmetric encryption such as Aes.");
    Console.WriteLine("***************************************************");
    Console.WriteLine();
    SampleEcDsaDhAliceBob();
  }


  #region Sample Elliptic Curve - Alice to Bob

  private static void SampleEcDsaDhAliceBob()
  {
    //1. Request temporary “Ec Public Key” from Bob.
    var msg = "Alice writes to Bob: Hello Bob, please give me an EC-Key. I would like to send you a message.";
    var ec_public_key_pmei_bob = EcPublicKeyRequest(msg);
    var ec_public_key_bob = EcPublicKey.FromEcPublicKeyPmei(ec_public_key_pmei_bob);
    var idx_pub_bob = ec_public_key_bob.EcIndex;

    //2. Generate a temporary 'EC Key Pair' (Ec Private Key)
    //   for Alice using Bob's 'EC Public Key'.
    var ec_key_pair_alice = GenerateEcKeyPair(ec_public_key_bob);
    var ec_public_key_alice = new EcPublicKey(ec_key_pair_alice)
    {
      EcIndex = idx_pub_bob  //Damit nachher bei Bob filename bekannt ist.
    };
    Console.WriteLine("With Bob's PublicKey, Alice can now generate a new Elliptic Curve KeyPair (PrivateKey).");

    //3. Generate an temporary 'ECDH Shared Key' with the 'Ec Private Key'
    //   from Alice and the 'Ec Public Key' from Bob
    var ec_dh_shared_key_alice = EcService.ToSharedKey(ec_key_pair_alice, ec_public_key_bob.PublicKey);
    Console.WriteLine("Alice generates now a SharedKey with her new PrivateKey and Bob's PublicKey. Alice uses the EC Diffie Hellman algorithm to do this.");

    //4. Encrypt Alice's 'secret message' with the 'Ec Shared Key'.
    var associated = ""u8.ToArray();
    var secret_message_alice = "Hallo Bob. This is my secret message!"u8.ToArray();
    var cipher = EcService.EncryptionAes(secret_message_alice, ec_dh_shared_key_alice, associated);
    var iv = cipher.Skip(EcService.AES_TAG_SIZE).Take(EcService.AES_IV_SIZE).ToArray();
    string cipher_message = Convert.ToHexString(iv).ToLower() + "."
              + Convert.ToHexString(cipher).ToLower();
    Console.WriteLine("Alice can now encrypt your secret message with the AES algorithm and your new SharedKey.");

    //5. Sign the secret message from Alice. 
    var sign_message_alice = EcService.SignEcDsa(ec_key_pair_alice, secret_message_alice);
    var message_package = EcService.ToEcMessagePackage(
      cipher_message, sign_message_alice.Signature,
      EcPublicKey.ToEcPublicKeyPmei(ec_public_key_alice), EcCryptionAlgorithm.AES);
    Console.WriteLine("Finally, Alice signs her secret message with the EC DSA algorithm.");

    var (h, f) = PMEI.EcSignaturPmeiHF();
    var pmei = PMEI.ToPmei(EcService.SerializeJson(message_package), h, f);
    Console.WriteLine("To send the entire packet to Bob, a Pmei (Private Message Encryption Information) is generated.");
    Console.WriteLine();

    //6. Send the 'secret message' as well as the 'Ec Public Key' from Alice to Bob
    var verify = SendMessageToBob(pmei);
    Console.WriteLine(verify ? "Verification has worked. :-)" : "Verification has failed. :-(");
    Console.WriteLine();

    Console.WriteLine($"Finish {nameof(SampleEcDsaDhAliceBob)}");
    Console.WriteLine();
    Console.WriteLine();
    Console.WriteLine("******************* ******************* ******************* ");
    Console.WriteLine("******************* ******************* ******************* ");
    Console.WriteLine();
    Console.WriteLine();
    Console.WriteLine();
  }

  private static string EcPublicKeyRequest(string public_message)
  {
    var (pmei, msg) = BobEcUser.PublicKeyRequirementEc(public_message, UserName);
    //var publickey = EcPublicKey.FromEcPublicKeyPmei(pmei);

    Console.WriteLine(msg);
    Console.WriteLine();
    return pmei;
  }

  private static ECParameters GenerateEcKeyPair(EcPublicKey public_key)
  {
    var ecurve = public_key.PublicKey.Curve;
    return EcService.GenerateEcDsaKeyPair(ecurve).PrivateKey;
  }

  private static bool SendMessageToBob(string message_package_pmei)
  {
    return BobEcUser.ReceiveAMessage(message_package_pmei);
  }

  #endregion Sample Elliptic Curve 


  #region Sample Rsa - Bob to Alice

  public static (string Pmei, string Msg) PublicKeyRequirementRsa(
      string public_message, string username)
  {
    Console.WriteLine(public_message);
    var (id, rsaparam) = EcService.GenerateRsaKeyPairSavePmei(
      UserName, ".priv", true, 2048);
    Console.WriteLine("Alice now generates a new Rsa KeyPair (PrivateKey) and saves it.");

    var pupkey = new RsaPublicKey(rsaparam, id);
    var (_, pmei) = RsaPublicKey.ToRsaPublicKeyPmei(pupkey);
    Console.WriteLine("With the new Rsa KeyPair, Alice derives a PublicKey and sends it directly to Bob.");

    var msg = $"Alice's message: Hello {username}, welcome. As requested, my temporary asymmetric key. ";
    return (pmei, msg);
  }
  public static bool ReceiveAMessage(string rsa_package)
  {
    //1.Notify Alice that a new message has arrived. 
    Console.WriteLine($"Alice's system: Hello Alice, you have received a new message.");

    //2. Deserialize the package from Alice.
    //var (hs, fs) = PMEI.EcSignaturPmeiHF(false);
    //var pmei_bytes = PMEI.FromPmei(ecdsa_package, hs, fs);
    var (hs, fs) = PMEI.RsaSignaturPmeiHF(false);
    var (_, msg) = PMEI.FromPmei(rsa_package, hs, fs);
    var msg_package = EcService.DeserializeJson<RsaMessagePackage>(msg);
    var index = msg_package!.Index;
    Console.Write("Alice extracts the complete message from Bob from the Pmei protocol ");

    //3. Load and extract the PrivateKeyMaterial from Alice from the original PMEI-KeyMaterial.
    var fp = EcSettings.ToEcCurrentFolderUser(UserName);
    var fn = Path.Combine(fp, index + ".priv");
    var (hpriv, fpriv) = PMEI.RsaPrivateKeyPmeiHF(false);
    var (_, privkey) = PMEI.LoadPmeiFromFile(fn, hpriv, fpriv);
    File.Delete(fn);
    Console.WriteLine("and retrieves the previously generated KeyPair from the HDD.");

    //4. Decrypted the PrivateKeyMaterial for Alice's PrivateKey.
    var mpw = SHA256.HashData(EcSettings.ToMasterKey(UserName));
    var sd = EcService.ToUserSystemData();
    var priv_key_original_alice = EcService.DecryptionWithEcCryptionAlgo(privkey, mpw, sd, EcCryptionAlgorithm.CHACHA20_POLY1305);
    var private_key_alice = RsaParametersInfo.DeserializeRsaParam(priv_key_original_alice);

    //5. Decrypt the Symmetric Key (SharedKey) with the Rsa-PrivateKey from Alice.
    var shared_key = EcService.DecryptionRsa(
      Convert.FromHexString(msg_package.CipherSharedKey), private_key_alice);
    Console.WriteLine($"Alice can now decrypt the symmetric key with her Rsa PrivateKey.");

    //6. Decrypted the cipher from Bob with the Sharedkey.
    var split = msg_package.CipherMessage.Split(".");
    //var iv = Convert.FromHexString(split[0]);
    var cipher = Convert.FromHexString(split[1]);

    var ec_crypt_algo = EcService.ToEcCryptionAlgorithm(msg_package.RsaCryptionAlgo);
    var decipher = EcService.DecryptionWithEcCryptionAlgo(cipher, shared_key, [], ec_crypt_algo);
    var txt = Encoding.UTF8.GetString(decipher);
    Console.WriteLine("Alice uses the Symmetric Key to decrypt Bob's secret message.");
    Console.WriteLine($"The secret message was: {txt}");

    //7. Check the signature using the EcSignedMessage-Material.
    var decipher_hash = SHA256.HashData(decipher);

    var signature = EcService.ToRsaSignedMessage(msg_package.SenderPublicKey,
      msg_package.Signature, decipher_hash);
    Console.WriteLine("Finally, Alice verifies the digital signature received from Bob.");

    var verify = EcService.VerifySignatureRsa(Encoding.UTF8.GetBytes(txt), signature);
    if (!verify) throw new CryptographicException($"{nameof(verify)} is failed!");

    return verify;
  }

  #endregion Sample Rsa 

}
