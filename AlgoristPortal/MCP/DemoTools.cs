using ModelContextProtocol.Server;
using System.ComponentModel;

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

    [McpServerTool(Name = "GenerateAlgoristGuid", ReadOnly = true)]
    [Description("Generates a new Algorist GUID.")]
    public static string GenerateAlgoristGuid ()
    {
        return Guid.NewGuid().ToString();
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