using Microsoft.Extensions.Options;
using PetAvailabilityApi.Configuration;
using PetAvailabilityApi.Services;

var builder = WebApplication.CreateBuilder(args);

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

var app = builder.Build();

app.MapGet("/pets/{petId:long}/available", async (
    long petId,
    PetstoreClient petstore,
    CancellationToken cancellationToken) =>
{
    try
    {
        return Results.Ok(await petstore.IsPetAvailableAsync(petId, cancellationToken));
    }
    catch (Exception ex) when (ex is HttpRequestException or InvalidDataException)
    {
        return Results.Problem(
            detail: ex.Message,
            statusCode: StatusCodes.Status502BadGateway);
    }
});

app.Run();

public partial class Program;
