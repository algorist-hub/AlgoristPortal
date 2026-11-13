namespace AlgoristPortal.Endpoints;

public static class DemoAppsEndpoints
{
    public static IEndpointRouteBuilder MapDemoAppsEndpoints (
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/demoapps/support-chat-assistant", () =>
            Results.Redirect("https://algorist-support-bjhdcrhkdadgc5d4.eastus2-01.azurewebsites.net/"));

        return endpoints;
    }
}