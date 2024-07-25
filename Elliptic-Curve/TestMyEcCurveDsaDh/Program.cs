


namespace michele.natale.EcCurveDsaDhTest;

using michele.natale.EcCurveDsaDh;


//Was slightly adapted in 2024 and updated to DotNet8.0.

//Shows in a simple way how the sender and receiver function
//of Alice and Bob works in encrypted form. 

//The ECDiffieHellman (ec key exchange) and ECDSA (ec digital signing)
//algorithms are used here.

//The elliptic curves are always selected randomly so that
//everything in the temporary area is used.

public class Program
{
  public static void Main()
  {
    //Set to 'true' if you are using it for the first
    //time, or if you want to reset the configuration.

    //if is 'true' a new master password is saved and 
    //all unused temporary KeyPairs are deleted.
    var delete_all_configuration_data = false;

    EcSettings.EcUsers = [AliceEcUser.UserName, BobEcUser.UserName];

    StartConfiguration(delete_all_configuration_data);

    StartEcCurveDsaDh(1); //1 Rounds

    Console.WriteLine();
    Console.WriteLine("FINISH");
    Console.ReadLine();
  }

  private static void StartEcCurveDsaDh(int rounds = 1)
  {
    for (int i = 0; i < rounds; i++)
    {
      AliceEcUser.StartAlice(); //Elliptic Curve, EcDsa, EcDh

      BobEcUser.StartBob();     //Rsa, Rsa-Sign
    }
  }

  private static void StartConfiguration(bool reset_first)
  {
    //Vorsicht! Sofern 'true': Alle Konfigurationen sowie 
    //Schlüsselpaare werden gelöscht.

    //Caution! If is 'true':
    //All configurations and KeyPairs will be deleted.
    if (reset_first)
      ResetAllConfiguration();

    SetNewConfiguration();
  }

  private static void SetNewConfiguration()
  {
    EcConfiguration.StartEcConfig();
  }

  private static void ResetAllConfiguration()
  {
    EcConfiguration.ResetEcConfig();
  }
}