# AlgoristPortal

Blazor WebAssembly (WASM) application that serves as a portal for the Algorist website and demonstration applications.


# Setup

## Local MCP Server Token Setup

To use the MCP Server locally, you first need to create a token locally.

In development it doesn't need to be a super secure token; any string will do.
Example: `local-token`

In the terminal, navigate to the AlgoristPortal project folder (where the AlgoristPortal.csproj is located) and run:
```powershell
dotnet user-secrets set "Mcp:Token" "local-token"
```

The McpTokenAuthHandler will use this localy defined token to authenticate requests to the local MCP Server.

This `user-secrets` configuration of your local token is for local development only and is not needed for production.
And this local token stays only on the local machine; it is not sent to the project repository.
