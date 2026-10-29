using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace AlgoristPortal.MCP;

public sealed class McpTokenAuthHandler (
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder,
    IConfiguration config)
    : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    public const string Scheme = "McpToken";

    protected override Task<AuthenticateResult> HandleAuthenticateAsync ()
    {
        var expected = config["Mcp:Token"];
        if (string.IsNullOrWhiteSpace(expected))
            return Task.FromResult(AuthenticateResult.Fail("MCP Server Token not configured."));

        if (!Request.Headers.TryGetValue("Authorization", out var header))
            return Task.FromResult(AuthenticateResult.NoResult());

        if (!AuthenticationHeaderValue.TryParse(header, out var auth) ||
            !string.Equals(auth.Scheme, "Bearer", StringComparison.OrdinalIgnoreCase) ||
            string.IsNullOrEmpty(auth.Parameter))
            return Task.FromResult(AuthenticateResult.Fail("Invalid Authorization header."));

        // Compara hashes em tempo constante (evita timing attack e vazamento de tamanho)
        var received = SHA256.HashData(Encoding.UTF8.GetBytes(auth.Parameter));
        var wanted = SHA256.HashData(Encoding.UTF8.GetBytes(expected));

        if (!CryptographicOperations.FixedTimeEquals(received, wanted))
            return Task.FromResult(AuthenticateResult.Fail("Invalid Token."));

        var identity = new ClaimsIdentity(
            [new Claim(ClaimTypes.Name, "mcp-client")], Scheme);
        var ticket = new AuthenticationTicket(new ClaimsPrincipal(identity), Scheme);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}