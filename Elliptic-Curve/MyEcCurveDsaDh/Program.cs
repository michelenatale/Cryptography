


//namespace michele.natale.EcCurveDsaDhTest;

//using michele.natale.EcCurveDsaDh;

//public class Program
//{
//  public static void Main()
//  {

//    //Set true, if is the first one.
//    var delete_all_configuration_data = false;

//    EcSettings.EcUsers = [AliceEcUser.UserName, BobEcUser.UserName];
//    StartConfiguration(delete_all_configuration_data);
//    StartEcCurveDsaDh(1);

//    Console.WriteLine();
//    Console.WriteLine("FINISH");
//    Console.ReadLine();
//  }

//  private static void StartEcCurveDsaDh(int rounds)
//  {
//    for (int i = 0; i < rounds; i++)
//    {
//      AliceEcUser.StartAlice();

//      BobEcUser.StartBob();
//    }
//  }

//  private static void StartConfiguration(bool reset_first)
//  {
//    ////Vorsicht! Alle Konfigurationen sowie
//    ////Schlüsselpaare werden gelöscht.
//    if (reset_first)
//      ResetAllConfiguration();

//    SetNewConfiguration();

//    //var mpw = EcSettings.ToMasterPassWord();
//  }

//  private static void SetNewConfiguration()
//  {
//    EcConfiguration.StartEcConfig();
//  }

//  private static void ResetAllConfiguration()
//  {
//    EcConfiguration.ResetEcConfig();
//  }



//  private static void Test1()
//  {
//  }
//}