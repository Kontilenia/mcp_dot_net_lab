# Petstore MCP Lab Guidelines

## Project Purpose

This repository is a small .NET 10 lab showing how GitHub Copilot can call a
local MCP server while helping with a separate practice application.

- `PetManagement/src/PetApi` is an ASP.NET Core minimal API backed by an
  in-memory `PetService`.
- `PetManagement/src/PetMcpServer` is a local stdio MCP server that exposes
  remote Swagger Petstore data to Copilot.
- These projects intentionally have separate models and data sources. Do not
  turn `PetApi` into a Petstore proxy unless the task explicitly requires it.
- `.vscode/mcp.json` is the workspace registration for the `petstore` MCP
  server.

Read `README.md` for the lab workflow and configuration details.

## Architecture and Conventions

- Target `net10.0` and preserve nullable reference types and implicit usings.
- Follow the existing file-scoped namespaces, primary constructors, async
  methods, and collection-expression style.
- Keep minimal API route wiring in `src/PetApi/Program.cs`; put reusable
  in-memory behavior in `src/PetApi/Services` and DTOs in `src/PetApi/Models`.
- Keep external HTTP behavior in
  `src/PetMcpServer/Services/PetstoreClient.cs`, response DTOs in
  `src/PetMcpServer/Models`, and MCP tool definitions in
  `src/PetMcpServer/Tools`.
- Add MCP operations as focused `[McpServerTool]` methods with stable explicit
  tool names and useful `[Description]` metadata for both methods and
  parameters.
- Keep the Petstore response model intentionally small. Add fields only when a
  tool needs them, and map JSON names explicitly with `[JsonPropertyName]`.

## MCP Server Requirements

- The MCP transport owns standard output. Never write logs, diagnostics, or
  application messages to stdout; keep logging directed to stderr.
- Keep the server compatible with local stdio startup through
  `.vscode/mcp.json`. The lab intentionally uses `dotnet run` so starting the
  server builds it from source without a separate publish task.
- Use the configured `HttpClient` and its relative `v2/` paths rather than
  creating ad hoc clients.
- Pass cancellation tokens through asynchronous HTTP and JSON operations when
  available.
- Treat Petstore `404 Not Found` as a missing pet. Surface other unsuccessful
  HTTP responses and invalid or empty successful responses instead of
  returning success-shaped fallback data.
- Read the API key from `Petstore:ApiKey` (environment variable
  `Petstore__ApiKey`) and never commit real credentials.

## Build and Validation

Run commands from the repository root:

```bash
dotnet build PetManagement/PetManagement.sln
dotnet test PetManagement/PetManagement.sln
dotnet run --project PetManagement/src/PetApi/PetApi.csproj
dotnet run --project PetManagement/src/PetMcpServer/PetMcpServer.csproj
```

- Build the solution after C# or project-file changes.
- Run the focused projects under `PetManagement/tests` after changing API or
  MCP behavior. Add focused tests for new non-trivial behavior.
- For `PetApi` changes, verify both successful and not-found endpoint behavior.
- For MCP changes, verify that the server starts through the workspace MCP
  configuration and that tool output remains concise and human-readable.
- Update `README.md` when setup, configuration, exposed tools, or the lab
  workflow changes.
