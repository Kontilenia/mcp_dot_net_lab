using System.Net;
using System.Net.Http.Json;
using PetMcpServer.Models;

namespace PetMcpServer.Services;

public sealed class PetstoreClient(HttpClient httpClient)
{
    public async Task<PetstorePet?> GetPetByIdAsync(
        long petId,
        CancellationToken cancellationToken = default)
    {
        using var response = await httpClient.GetAsync(
            $"pet/{petId}",
            cancellationToken);

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<PetstorePet>(
                   cancellationToken: cancellationToken)
               ?? throw new InvalidDataException("Petstore returned an empty pet response.");
    }
}
