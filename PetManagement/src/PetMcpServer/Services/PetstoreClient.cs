using System.Net;
using System.Net.Http.Json;
using PetMcpServer.Models;

namespace PetMcpServer.Services;

public sealed class PetstoreClient(HttpClient httpClient)
{
    private static readonly HashSet<string> ValidPetStatuses =
        new(["available", "pending", "sold"], StringComparer.Ordinal);

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

    public async Task<PetstorePet> AddPetAsync(
        string name,
        string status,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(status);

        var normalizedStatus = status.Trim().ToLowerInvariant();
        if (!ValidPetStatuses.Contains(normalizedStatus))
        {
            throw new ArgumentOutOfRangeException(
                nameof(status),
                status,
                "Status must be available, pending, or sold.");
        }

        var request = new PetstorePetRequest
        {
            Name = name,
            Status = normalizedStatus
        };

        using var response = await httpClient.PostAsJsonAsync(
            "pet",
            request,
            cancellationToken);

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<PetstorePet>(
                   cancellationToken: cancellationToken)
               ?? throw new InvalidDataException("Petstore returned an empty pet response.");
    }
}