
using System.Globalization;

namespace michele.natale.LoginSystems.Services;

partial class AppServices
{

  /// <summary>
  /// Check is Internet online
  /// </summary>
  /// <param name="url">Urls for checking</param>
  /// <returns></returns>
  public async Task<bool> IsInternetConnectedAsync(string? url = null)
  {
    //https://stackoverflow.com/a/2031831
    try
    {
      url ??= CultureInfo.InstalledUICulture switch
      {
        { Name: var n } when n.StartsWith("de") => // German
            "http://www.google.de",
        { Name: var n } when n.StartsWith("en") => // English
            "http://www.google.com",
        { Name: var n } when n.StartsWith("fa") => // Iran
            "http://www.aparat.com",
        { Name: var n } when n.StartsWith("zh") => // China
            "http://www.baidu.com",
        _ => "http://www.google.com",
      };

      if (!string.IsNullOrEmpty(url))
        return await CheckInternetConnectionAsync(url);
    }
    catch
    {
    }
    return false;
  }

  /// <summary>
  /// Check is Internet Onlline
  /// </summary>
  /// <param name="url">Url for checking</param>
  /// <returns></returns>
  private async Task<bool> CheckInternetConnectionAsync(string url)
  {
    var surl = string.IsNullOrEmpty(url) ? "http://www.google.com" : url;
    using var client = new HttpClient();
    using var result = await client.GetAsync(surl);
    return true;
  }
}
