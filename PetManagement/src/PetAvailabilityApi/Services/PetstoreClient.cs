using System.Net;
using System.Net.Http.Json;
using PetAvailabilityApi.Models;

namespace PetAvailabilityApi.Services;

public sealed class PetstoreClient(HttpClient httpClient)
{
    public async Task<bool> IsPetAvailableAsync(
        long petId,
        CancellationToken cancellationToken = default)
    {
        using var response = await httpClient.GetAsync(
            $"pet/{petId}",
            cancellationToken);

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return false;
        }

        response.EnsureSuccessStatusCode();

        var pet = await response.Content.ReadFromJsonAsync<PetstorePet>(
                      cancellationToken: cancellationToken)
                  ?? throw new InvalidDataException("Petstore returned an empty pet response.");

        return string.Equals(pet.Status, "available", StringComparison.Ordinal);
    }
}
