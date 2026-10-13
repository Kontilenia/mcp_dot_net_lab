using System.Net;
using System.Text;
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