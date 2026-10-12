# Petstore MCP lab

This lab demonstrates how GitHub Copilot can use an MCP server while working
with an application. The MCP server exposes one tool, `get_pet_by_id`, which
calls the Swagger Petstore `GET /v2/pet/{petId}` operation. The Petstore client
and response model intentionally implement only the fields needed by this
example.

## Run the lab

1. Open this repository or its `PetManagement` folder in VS Code. Start the
   `petstore` MCP server from the workspace's `.vscode/mcp.json`. The first run
   restores and builds the server.
2. Ask Copilot to look up a pet with the `get_pet_by_id` tool, for example pet
   ID `1`.
3. Run the small practice application with
   `dotnet run --project PetManagement/PetApi/PetApi.csproj`, then ask Copilot
   to make code changes informed by the live Petstore data.

The server uses the demo API key documented by Petstore (`special-key`). To
override it, set the `Petstore:ApiKey` configuration value, for example with
the `Petstore__ApiKey` environment variable.

`PetManagement/PetApi` is a separate in-memory practice app, not an
implementation or proxy of the Petstore API. The MCP server's `PetstoreClient`
is the small Swagger consumer in this lab.

The Petstore Swagger definition is available at
<https://petstore.swagger.io/v2/swagger.json>.
