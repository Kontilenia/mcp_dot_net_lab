# Petstore MCP lab

This lab demonstrates how GitHub Copilot can use a local stdio MCP server while
working with an application. The local server exposes `get_pet_by_id`, which
calls the remote Swagger Petstore `GET /v2/pet/{petId}` operation, and
`add_pet`, which calls `POST /v2/pet`. The add tool accepts only a pet name and
status (`available`, `pending`, or `sold`), supplies an empty `photoUrls` array
required by the API, and omits the other optional Petstore fields.

## Run the lab

1. Open this repository root in VS Code and start the `petstore` MCP server
   from `.vscode/mcp.json`. VS Code runs the local server from source with
   `dotnet run`; no separate publish step is required.
2. Ask Copilot to look up a pet with the `get_pet_by_id` tool, for example pet
   ID `1`, or add one with the `add_pet` tool.
3. Run the small practice application with
   `dotnet run --project PetManagement/src/PetApi/PetApi.csproj`, then ask
   Copilot to make code changes informed by the live Petstore data.

The solution keeps independently runnable applications under `src/` and their
focused test projects under `tests/`:

```text
PetManagement/
├── src/
│   ├── PetApi/
│   └── PetMcpServer/
├── tests/
│   ├── PetApi.Tests/
│   └── PetMcpServer.Tests/
└── PetManagement.sln
```

Build and test everything with:

```bash
dotnet build PetManagement/PetManagement.sln
dotnet test PetManagement/PetManagement.sln
```

The server uses the demo API key documented by Petstore (`special-key`) and
validates its configuration at startup. Override the API key or base address
with `Petstore__ApiKey` or `Petstore__BaseAddress` environment variables.

`PetManagement/src/PetApi` is a separate in-memory practice app, not an
implementation or proxy of the Petstore API. The MCP server's `PetstoreClient`
is the small Swagger consumer in this lab, so `PetApi` does not need to be
running to use the MCP tool.

The Petstore Swagger definition is available at
<https://petstore.swagger.io/v2/swagger.json>.
