 
using System.Text;
using System.Security.Cryptography;


namespace michele.natale.EcCurveDsaDh;


public class BobEcUser
{
  public const string UserName = "Bob";
  public static void StartBob()
  {
    Console.WriteLine("Bob wants to send a secret message to Alice.");
    Console.WriteLine("Bob uses Rsa, Rsa signing and symmetric encryption such as AesGcm.");
    Console.WriteLine("******************************************************************");
    Console.WriteLine();
    SampleRsaDsaBobAlice();
  }


  #region Sample Rsa - Bob to Alice
  private static void SampleRsaDsaBobAlice()
  {
    //1. Request temporary “Ec Public Key” from Alice.
    var msg = "Bob writes to Alice: Hello Alice, please give me an RSA-Key. I would like to send you a message.";
    var rsa_public_key_pmei_alice = RsaPublicKeyRequest(msg);
    var rsa_public_key_alice = RsaPublicKey.FromRsaPublicKeyPmei(rsa_public_key_pmei_alice);
    var idx_pub_alice = rsa_public_key_alice.RsaIndex;

    //2. Generate a temporary 'Rsa Key Pair' (Rsa Private Key)
    var rsa_key_pair_bob = EcService.GenerateRsaKeyPair(2048);
    var rsa_public_key_bob = new RsaPublicKey(rsa_key_pair_bob.PublicKey)
    {
      RsaIndex = idx_pub_alice,  //Damit nachher bei Alice filename bekannt ist.
    };
    Console.WriteLine("In the meantime, Bob has also had an Rsa KeyPair (PrivateKey) generated.");

    //3. Create an temporary symmetric Key (SharedKey)
    var shared_key = EcService.RngBytes(EcService.AES_GCM_MAX_KEY_SIZE);
    Console.WriteLine("Bob can also generate a new key for symmetric encryption.");

    //4. Encrypt Alice's 'secret message' with the 'SharedKey'.
    var associated = ""u8.ToArray();
    var secret_message_bob = "Hallo Alice. This is my secret message!"u8.ToArray();
    var cipher_msg = EcService.EncryptionAesGcm(secret_message_bob, shared_key, associated);
    var iv = cipher_msg.Skip(EcService.AES_GCM_TAG_SIZE).Take(EcService.AES_GCM_NONCE_SIZE).ToArray();
    string cipher_message = Convert.ToHexString(iv).ToLower() + "."
              + Convert.ToHexString(cipher_msg).ToLower();
    Console.WriteLine("Bob encrypts his secret message with the new symmetric key.");

    //5. Encryption the SharedKey with the Rsa PublicKey from Alice
    var cipher_shared_key = EcService.EncryptionRsa(shared_key, rsa_public_key_alice.PublicKey);
    Console.WriteLine("The symmetric key is subsequently encrypted with Alice's Rsa PublicKey.");

    //6. Sign the plain message from Bob. 
    var sign_bob = EcService.SignRsa(secret_message_bob, rsa_key_pair_bob.PrivateKey);
    var message_package = EcService.ToRsaMessagePackage(
      cipher_message, Convert.ToHexString(cipher_shared_key),
      sign_bob.Signature, sign_bob.PublicKey, idx_pub_alice,
      EcCryptionAlgorithm.AES_GCM);
    Console.WriteLine("Finally, Bob signs his secret message with the 'Rsa Signing Algorithm' and his Rsa PrivateKey.");

    var (h, f) = PMEI.RsaSignaturPmeiHF();
    var pmei = PMEI.ToPmei(EcService.SerializeJson(message_package), h, f);
    Console.WriteLine("To send the entire packet to Alice, a Pmei (Private Message Encryption Information) is generated.");
    Console.WriteLine();

    //7. Send the 'secret message' as well as the 'Rsa Public Key' from Alice to Bob
    var verify = SendMessageToAlice(pmei);
    Console.WriteLine(verify ? "Verification has worked. :-)" : "Verification has failed. :-(");
    Console.WriteLine();

    Console.WriteLine($"Finish {nameof(SampleRsaDsaBobAlice)}");
    Console.WriteLine();
    Console.WriteLine();
    Console.WriteLine("******************* ******************* ******************* ");
    Console.WriteLine("******************* ******************* ******************* ");
    Console.WriteLine();
    Console.WriteLine();
    Console.WriteLine();
  }


  public static (string Pmei, string Msg) PublicKeyRequirementEc(
    string public_message, string username)
  {
    Console.WriteLine(public_message);
    var (id, ecparam) = EcService.GenerateEcKeyPairSavePmei(UserName);
    Console.WriteLine("Bob now generates a new KeyPair from elliptic curves (PrivateKey) and saves it.");

    var pupkey = new EcPublicKey((id, ecparam));
    var pmei = EcPublicKey.ToEcPublicKeyPmei(pupkey);
    Console.WriteLine("With the new KeyPair, Bob derives a PublicKey and sends it directly to Alice.");

    var msg = $"Bob's message: Hello {username}, welcome. As requested, my temporary asymmetric key. ";
    return (pmei, msg);
  }


  public static bool ReceiveAMessage(string ecdsa_package)
  {
    //1.Notify Bob that a new message has arrived. 
    Console.WriteLine($"Bob's system: Hello Bob, you have received a new message.");

    //2. Deserialize the package from Alice.
    var (hs, fs) = PMEI.EcSignaturPmeiHF(false);
    var pmei_bytes = PMEI.FromPmei(ecdsa_package, hs, fs);
    var msg_package = EcService.DeserializeJson<EcMessagePackage>(pmei_bytes.Message);
    Console.WriteLine("Bob extracts the complete message from Alice from the Pmei protocol.");

    //3. Extract the PublicKey from Alice.
    var pub_key_alice = new EcPublicKey(msg_package!.SenderPublicKeyPmei);
    var index = pub_key_alice.EcIndex;
    Console.Write("Bob takes over the PublicKey from Alice ");

    //4. Load and extract the PrivateKeyMaterial from Bob from the original PMEI-KeyMaterial.
    var fp = EcSettings.ToEcCurrentFolderUser(UserName);
    var fn = Path.Combine(fp, index + ".priv");
    var (hpriv, fpriv) = PMEI.EcPrivateKeyPmeiHF();
    var (_, privkey) = PMEI.LoadPmeiFromFile(fn, hpriv, fpriv);
    File.Delete(fn);

    //5. Decrypted the PrivateKeyMaterial for Bob's PrivateKey.
    var mpw = SHA256.HashData(EcSettings.ToMasterKey(UserName));
    var sd = EcService.ToUserSystemData();
    var priv_key_original_bob = EcService.DecryptionWithEcCryptionAlgo(privkey, mpw, sd, EcCryptionAlgorithm.CHACHA20_POLY1305);
    var private_key_bob = EcParametersInfo.DeserializeEcParam(priv_key_original_bob);
    Console.WriteLine("and retrieves the previously generated KeyPair (PrivateKey).");

    //6. Create the SharedKey from the PrivateKey of bob and the Publickey of Alice.
    var shared_key = EcService.ToSharedKey(private_key_bob, pub_key_alice.PublicKey);
    Console.WriteLine("Now Bob can also use his PrivateKey and Alice's PublicKey to derive a SharadKey.  ");

    //7. Decrypted the cipher from Alice.
    var split = msg_package.Cipher.Split(".");
    //var iv = Convert.FromHexString(split[0]);
    var cipher = Convert.FromHexString(split[1]);

    var ec_crypt_algo = EcService.ToEcCryptionAlgorithm(msg_package.EcCryptionAlgo);
    var decipher = EcService.DecryptionWithEcCryptionAlgo(cipher, shared_key, [], ec_crypt_algo);
    var txt = Encoding.UTF8.GetString(decipher);
    Console.WriteLine("Bob uses the SharedKey to decrypt Alice's secret message.");
    Console.WriteLine($"The secret message was: {txt}");

    //8. Check the signature using the EcSignedMessage-Material.
    var decipher_hash = SHA256.HashData(decipher);

    var signature = EcService.ToEcSignedMessage(msg_package.SenderPublicKeyPmei,
      msg_package.Signature, decipher_hash);
    Console.WriteLine("Finally, Bob verifies the digital signature received from Alice.");

    var verify = EcService.VerifyEcDsa(signature);
    if (!verify) throw new CryptographicException($"{nameof(verify)} is failed!");

    return verify;
  }

  #endregion Sample Elliptic Curve


  #region Sample Elliptic Curve - Alice to Bob

  private static string RsaPublicKeyRequest(string public_message)
  {
    var (pmei, msg) = AliceEcUser.PublicKeyRequirementRsa(public_message, UserName);
    //var publickey = RsaPublicKey.FromRsaPublicKeyPmei(pmei);

    Console.WriteLine(msg);
    Console.WriteLine();
    return pmei;
  }

  private static bool SendMessageToAlice(string message_package_pmei)
  {
    return AliceEcUser.ReceiveAMessage(message_package_pmei);
  }

  #endregion Sample Rsa 
}
