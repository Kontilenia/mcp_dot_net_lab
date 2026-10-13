using System.Net;
using System.Text;
using System.Text.Json;
using PetMcpServer.Services;
using PetMcpServer.Tools;

namespace PetMcpServer.Tests;

public class PetstoreClientTests
{
    [Fact]
    public async Task GetPetByIdAsync_DeserializesPet()
    {
        using var httpClient = CreateHttpClient((_, _) => new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(
                """{"id":7,"name":"Nova","status":"available"}""",
                Encoding.UTF8,
                "application/json")
        });
        var client = new PetstoreClient(httpClient);

        var pet = await client.GetPetByIdAsync(7);

        Assert.NotNull(pet);
        Assert.Equal(7, pet.Id);
        Assert.Equal("Nova", pet.Name);
        Assert.Equal("available", pet.Status);
    }

    [Fact]
    public async Task GetPetByIdAsync_ReturnsNullForNotFound()
    {
        using var httpClient = CreateHttpClient(
            (_, _) => new HttpResponseMessage(HttpStatusCode.NotFound));
        var client = new PetstoreClient(httpClient);

        var pet = await client.GetPetByIdAsync(404);

        Assert.Null(pet);
    }

    [Fact]
    public async Task GetPetById_PropagatesCancellationToHttpRequest()
    {
        using var httpClient = CreateHttpClient((_, cancellationToken) =>
        {
            cancellationToken.ThrowIfCancellationRequested();
            return new HttpResponseMessage(HttpStatusCode.OK);
        });
        var tools = new PetstoreTools(new PetstoreClient(httpClient));
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();

        await Assert.ThrowsAnyAsync<OperationCanceledException>(
            () => tools.GetPetById(1, cancellation.Token));
    }

    [Fact]
    public async Task AddPetAsync_PostsOnlyToolFieldsAndRequiredDefaults()
    {
        using var httpClient = CreateHttpClient((request, cancellationToken) =>
        {
            Assert.Equal(HttpMethod.Post, request.Method);
            Assert.Equal("https://petstore.example/v2/pet", request.RequestUri?.AbsoluteUri);

            var json = request.Content!.ReadAsStringAsync(cancellationToken)
                .GetAwaiter()
                .GetResult();
            using var document = JsonDocument.Parse(json);
            var properties = document.RootElement.EnumerateObject().ToArray();

            Assert.Equal(3, properties.Length);
            Assert.Equal("Nova", document.RootElement.GetProperty("name").GetString());
            Assert.Equal(
                JsonValueKind.Array,
                document.RootElement.GetProperty("photoUrls").ValueKind);
            Assert.Empty(document.RootElement.GetProperty("photoUrls").EnumerateArray());
            Assert.Equal("available", document.RootElement.GetProperty("status").GetString());

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(
                    """{"id":8,"name":"Nova","status":"available"}""",
                    Encoding.UTF8,
                    "application/json")
            };
        });
        var client = new PetstoreClient(httpClient);

        var pet = await client.AddPetAsync("Nova", "AVAILABLE");

        Assert.Equal(8, pet.Id);
        Assert.Equal("Nova", pet.Name);
        Assert.Equal("available", pet.Status);
    }

    [Fact]
    public async Task AddPetAsync_RejectsUnsupportedStatus()
    {
        using var httpClient = CreateHttpClient(
            (_, _) => throw new InvalidOperationException("No request should be sent."));
        var client = new PetstoreClient(httpClient);

        var exception = await Assert.ThrowsAsync<ArgumentOutOfRangeException>(
            () => client.AddPetAsync("Nova", "unknown"));

        Assert.Equal("status", exception.ParamName);
    }

    [Fact]
    public async Task AddPet_ReturnsCreatedPetSummary()
    {
        using var httpClient = CreateHttpClient((_, _) => new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(
                """{"id":9,"name":"Milo","status":"pending"}""",
                Encoding.UTF8,
                "application/json")
        });
        var tools = new PetstoreTools(new PetstoreClient(httpClient));

        var result = await tools.AddPet("Milo", "pending", CancellationToken.None);

        Assert.Equal("Added pet ID: 9, Name: Milo, Status: pending", result);
    }

    private static HttpClient CreateHttpClient(
        Func<HttpRequestMessage, CancellationToken, HttpResponseMessage> responseFactory)
    {
        return new HttpClient(new StubHttpMessageHandler(responseFactory))
        {
            BaseAddress = new Uri("https://petstore.example/v2/")
        };
    }

    private sealed class StubHttpMessageHandler(
        Func<HttpRequestMessage, CancellationToken, HttpResponseMessage> responseFactory)
        : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(responseFactory(request, cancellationToken));
        }
    }
}