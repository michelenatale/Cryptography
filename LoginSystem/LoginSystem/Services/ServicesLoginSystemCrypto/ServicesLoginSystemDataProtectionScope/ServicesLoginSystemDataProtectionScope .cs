
//using System.Security.Cryptography;

//namespace michele.natale.LoginSystems.Services;

//partial  class AppServices 
//{

//  public static byte[] ProtectMessageSyPw(byte[] bytes,byte[] entropy, bool change_keys = false)
//  {
//    var scope = DataProtectionScope.CurrentUser;
//    var keyadditional = change_keys ? ToSettingNewKeyRechange() : ToSettingNewKey();
//    var newentropy = RngNewKey(keyadditional, entropy, 0, 48);

//    return ProtectMsg(bytes, newentropy, scope);
//  }

//  public static byte[] UnprotectMessageSyPw(byte[] bytes, byte[] entropy, bool change_keys = false)
//  {
//    var scope = DataProtectionScope.CurrentUser;
//    var keyadditional = change_keys ? ToSettingNewKeyRechange() : ToSettingNewKey();
//    var newentropy = RngNewKey(keyadditional, entropy, 0, 48);

//    return UnprotectMsg(bytes, newentropy, scope);
//  }

//  public static byte[] ProtectMessage(
//    byte[] bytes, byte[] entropy, DataProtectionScope scope)
//  {
//    return ProtectMsg(bytes, entropy, scope);
//  }

//  public static byte[] UnprotectMessage(
//    byte[] bytes, byte[] entropy, DataProtectionScope scope)
//  {
//    return UnprotectMsg(bytes, entropy, scope);
//  }

//  private static byte[] ProtectMsg(
//    byte[] bytes, byte[] entropy, DataProtectionScope scope)
//  {
//    return ToBase64BytesUtf8(
//      ProtectedData.Protect(bytes, entropy ?? null, scope));
//  }

//  private static byte[] UnprotectMsg(
//    byte[] bytes, byte[] entropy, DataProtectionScope scope)
//  {
//    return ProtectedData.Unprotect(
//        FromBase64BytesUtf8(bytes), entropy ?? null, scope);
//  }

//}
