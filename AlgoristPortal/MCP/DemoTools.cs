using ModelContextProtocol.Server;
using System.ComponentModel;
using System.Security.Cryptography;
using System.Text;

namespace AlgoristPortal.MCP;

[McpServerToolType]
public static class DemoTools
{
    [McpServerTool(Name = "GetServerInfo", ReadOnly = true)]
    [Description("Returns information about the Algorist MCP Server.")]
    public static object GetServerInfo (McpServer server)
    {
        var info = server.ServerOptions.ServerInfo;

        var serverTime = TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("America/Sao_Paulo"));

        return new
        {
            Name = info?.Name,
            Version = info?.Version,
            Framework = ".NET 10",
            Language = "C#",
            ServerDateTimeNow = serverTime.ToString("yyyy-MM-dd HH:mm:ss zzz"),
        };
    }

    [McpServerTool(Name = "GenerateAlgoristHashFromWord", ReadOnly = true)]
    [Description("Generates an Algorist hash from a word.")]
    public static string GenerateAlgoristHashFromWord ([Description("Word to hash")] string palavra)
    {
        ArgumentNullException.ThrowIfNull(palavra);

        palavra += "_algorist";

        byte[] bytes = Encoding.UTF8.GetBytes(palavra);
        byte[] hash = SHA256.HashData(bytes);

        var sb = new StringBuilder(hash.Length * 2);
        foreach (byte b in hash)
            sb.Append(b.ToString("x2"));

        return sb.ToString();
    }

    [McpServerTool(Name = "ConvertTemperature", ReadOnly = true)]
    [Description("Converts a temperature between Celsius, Fahrenheit and Kelvin.")]
    public static double ConvertTemperature (
        [Description("Source temperature")] double value,
        [Description("Source unit: C, F or K")] string from,
        [Description("Target unit: C, F or K")] string to)
    {
        from = from.ToUpperInvariant();
        to = to.ToUpperInvariant();

        var celsius = from switch
        {
            "C" => value,
            "F" => (value - 32) * 5 / 9,
            "K" => value - 273.15,
            _ => throw new ArgumentException("Invalid source unit.")
        };

        return to switch
        {
            "C" => celsius,
            "F" => celsius * 9 / 5 + 32,
            "K" => celsius + 273.15,
            _ => throw new ArgumentException("Invalid target unit.")
        };
    }
}