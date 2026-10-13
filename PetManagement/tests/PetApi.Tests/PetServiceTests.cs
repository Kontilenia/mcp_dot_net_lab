using PetApi.Services;

namespace PetApi.Tests;

public class PetServiceTests
{
    [Fact]
    public void GetById_ReturnsMatchingPet()
    {
        var service = new PetService();

        var pet = service.GetById(2);

        Assert.NotNull(pet);
        Assert.Equal("Milo", pet.Name);
        Assert.Equal("pending", pet.Status);
    }

    [Fact]
    public void GetById_ReturnsNullForUnknownPet()
    {
        var service = new PetService();

        Assert.Null(service.GetById(999));
    }
}