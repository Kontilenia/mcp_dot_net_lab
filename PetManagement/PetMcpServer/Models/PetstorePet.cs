using System.Text.Json.Serialization;

namespace PetMcpServer.Models;

public sealed class PetstorePet
{
    [JsonPropertyName("id")]
    public long Id { get; init; }

    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("status")]
    public string? Status { get; init; }
}
