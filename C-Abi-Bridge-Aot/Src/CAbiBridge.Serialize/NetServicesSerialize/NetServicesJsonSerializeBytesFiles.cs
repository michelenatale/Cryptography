
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace michele.natale;


[JsonSerializable(typeof(object))]
public sealed partial class NetJsonContext : JsonSerializerContext
{
}

partial class NetServicesSerialize
{
  public static byte[] SerializeJson(
    ReadOnlyMemory<byte> input)
  {
    // JSON → UTF8-Bytes (AOT-safe)
    var json = JsonSerializer.Serialize(
      input, NetJsonContext.Default.Object);
    return Encoding.UTF8.GetBytes(json);
  }

  public static byte[] DeserializeJson(
    ReadOnlySpan<byte> input)
  {
    // UTF8-Bytes → JSON → object (AOT-safe)
    var json = Encoding.UTF8.GetString(input);

    var obj = JsonSerializer.Deserialize(json,
        NetJsonContext.Default.Object);

    return obj as byte[]
      ?? throw new InvalidOperationException(
        "JSON did not contain a byte array");
  }

  public static async Task SerializeJsonAsync(
    string filesrc, string filedest)
  {
    // ----------------------------------------------------
    // FILE → JSON-FILE (async, stream-based, AOT-safe)
    // ----------------------------------------------------

    await using var inputstream = new FileStream(
      filesrc, FileMode.Open, FileAccess.Read);

    var input = new byte[inputstream.Length];
    await inputstream.ReadExactlyAsync(input);

    await using var outputstream = new FileStream(
      filedest, FileMode.Create, FileAccess.Write);

    // write JSON in Stream (AOT-safe)
    await JsonSerializer.SerializeAsync(
        outputstream, (object)input,
        NetJsonContext.Default.Object);
  }

  public static async Task DeserializeJsonAsync(
    string filesrc, string filedest)
  {
    // ----------------------------------------------------
    // JSON-FILE → FILE (async, stream-based, AOT-safe)
    // ----------------------------------------------------

    await using var inputstream = new FileStream(
      filesrc, FileMode.Open, FileAccess.Read);

    // read JSON from Stream (AOT-safe)
    var obj = await JsonSerializer.DeserializeAsync(
        inputstream, NetJsonContext.Default.Object);

    var output = obj as byte[]
        ?? throw new InvalidOperationException(
          "JSON did not contain a byte array");

    await using var outputstream = new FileStream(
      filedest, FileMode.Create, FileAccess.Write);
    await outputstream.WriteAsync(output);
  }
}





