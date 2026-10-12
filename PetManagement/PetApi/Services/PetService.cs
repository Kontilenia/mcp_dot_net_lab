using PetApi.Models;

namespace PetApi.Services;

public class PetService
{
    private readonly List<Pet> _pets =
    [
        new Pet { Id = 1, Name = "Rex", Status = "available" },
        new Pet { Id = 2, Name = "Milo", Status = "pending" },
        new Pet { Id = 3, Name = "Luna", Status = "sold" }
    ];

    public List<Pet> GetAll() => _pets;

    public Pet? GetById(long id) => _pets.FirstOrDefault(p => p.Id == id);
}
