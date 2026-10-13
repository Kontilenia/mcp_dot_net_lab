namespace PetAvailabilityApi.Configuration;

public sealed class PetstoreOptions
{
    public const string SectionName = "Petstore";

    public Uri BaseAddress { get; init; } = new("https://petstore.swagger.io/v2/");

    public string ApiKey { get; init; } = "special-key";
}
