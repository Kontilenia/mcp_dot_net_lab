using System.Text.Json.Serialization;

namespace PetAvailabilityApi.Models;

public sealed class PetstorePet
{
    [JsonPropertyName("id")]
    public long Id { get; init; }

    [JsonPropertyName("status")]
    public string? Status { get; init; }
}
