using System.Text.Json.Serialization;

namespace PetMcpServer.Models;

internal sealed class PetstorePetRequest
{
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    [JsonPropertyName("photoUrls")]
    public IReadOnlyList<string> PhotoUrls { get; init; } = [];

    [JsonPropertyName("status")]
    public required string Status { get; init; }
}
