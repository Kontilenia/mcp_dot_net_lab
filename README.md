# Petstore MCP lab


This repository is a small .NET lab built around the public
[Swagger Petstore API](https://petstore.swagger.io/). It shows how an MCP server
can turn API operations into tools that an AI assistant can use.

The code is intentionally simple. It is meant for learning and experimenting,
not as a production application.

## Requirements

To work with this lab, you need:

- The .NET 10 SDK.
- Access to GitHub Copilot through an active license, with GitHub Copilot
  signed in and available in VS Code.
- Outbound network access to `api.nuget.org` for .NET packages and
  `petstore.swagger.io` for the public Petstore API.

## What is in the repository?

The solution contains two small applications:

- **PetMcpServer** is the main part of the lab. It connects to the public
  Petstore API and exposes tools for finding a pet by ID and adding a pet.
- **PetApi** is a separate, in-memory sample API used for practice. It does not
  call or replace the public Petstore API.

Each application has a matching test project:

```text
PetManagement/
├── src/
│   ├── PetApi/                 # Small practice API
│   │   ├── Models/             # Pet data shape
│   │   └── Services/           # In-memory pet logic
│   └── PetMcpServer/           # MCP server connected to Swagger Petstore
│       ├── Configuration/      # Petstore connection settings
│       ├── Models/             # Petstore request and response shapes
│       ├── Services/           # HTTP client for the Petstore API
│       └── Tools/              # Tools exposed through MCP
├── tests/
│   ├── PetApi.Tests/
│   └── PetMcpServer.Tests/
└── PetManagement.sln
```

In simple terms, an MCP tool receives a request, the `PetstoreClient` sends it
to the public Petstore API, and the result is returned in a short,
human-readable form.

The public Swagger definition used as the basis for this lab is available at
<https://petstore.swagger.io/v2/swagger.json>.
