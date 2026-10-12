using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PetMcpServer.Services;

var builder = Host.CreateApplicationBuilder(args);

// stdout carries the MCP protocol, so all logs go to stderr.
builder.Logging.AddConsole(options => options.LogToStandardErrorThreshold = LogLevel.Trace);

builder.Services.AddHttpClient<PetstoreClient>(client =>
{
    client.BaseAddress = new Uri("https://petstore.swagger.io/v2/");
    client.DefaultRequestHeaders.Add(
        "api_key",
        builder.Configuration["Petstore:ApiKey"] ?? "special-key");
});

builder.Services
    .AddMcpServer()
    .WithStdioServerTransport()
    .WithToolsFromAssembly();

await builder.Build().RunAsync();
