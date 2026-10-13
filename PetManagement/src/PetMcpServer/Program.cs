using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PetMcpServer.Configuration;
using PetMcpServer.Services;

var builder = Host.CreateApplicationBuilder(args);

// stdout carries the MCP protocol, so all logs go to stderr.
builder.Logging.AddConsole(options => options.LogToStandardErrorThreshold = LogLevel.Trace);

builder.Services
    .AddOptions<PetstoreOptions>()
    .BindConfiguration(PetstoreOptions.SectionName)
    .Validate(
        options => options.BaseAddress.IsAbsoluteUri
            && options.BaseAddress.Scheme is "http" or "https",
        "Petstore:BaseAddress must be an absolute HTTP or HTTPS URL.")
    .Validate(
        options => !string.IsNullOrWhiteSpace(options.ApiKey),
        "Petstore:ApiKey is required.")
    .ValidateOnStart();

builder.Services.AddHttpClient<PetstoreClient>((services, client) =>
{
    var options = services.GetRequiredService<IOptions<PetstoreOptions>>().Value;
    client.BaseAddress = options.BaseAddress;
    client.DefaultRequestHeaders.Add("api_key", options.ApiKey);
});

builder.Services
    .AddMcpServer()
    .WithStdioServerTransport()
    .WithToolsFromAssembly();

await builder.Build().RunAsync();