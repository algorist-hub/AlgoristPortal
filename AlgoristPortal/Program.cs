using AlgoristPortal.Components;
using AlgoristPortal.MCP;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddMcpServer(options => options.ServerInfo = new()
    {
        Name = "Algorist MCP Server",
        Version = "1.0.0"
    })
    .WithHttpTransport(options => options.Stateless = true)
    .WithToolsFromAssembly();

builder.Services
    .AddAuthentication()
    .AddScheme<AuthenticationSchemeOptions, McpTokenAuthHandler>(McpTokenAuthHandler.Scheme, null);

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("Mcp", policy => policy
        .AddAuthenticationSchemes(McpTokenAuthHandler.Scheme)
        .RequireAuthenticatedUser());

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(AlgoristPortal.Client._Imports).Assembly);

app.UseAuthentication();
app.UseAuthorization();

app.MapMcp("/mcp").RequireAuthorization("Mcp");

app.Run();
