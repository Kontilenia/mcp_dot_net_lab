using System.ComponentModel;
using ModelContextProtocol.Server;
using PetMcpServer.Services;

namespace PetMcpServer.Tools;

[McpServerToolType]
public class PetstoreTools(PetstoreClient petstoreClient)
{
    [McpServerTool(Name = "get_pet_by_id")]
    [Description("Gets a pet from the Swagger Petstore API by its ID and returns its ID, name and status.")]
    public async Task<string> GetPetById(
        [Description("The pet ID to look up.")] long petId,
        CancellationToken cancellationToken)
    {
        var pet = await petstoreClient.GetPetByIdAsync(petId, cancellationToken);
        return pet is null
            ? $"Pet {petId} was not found."
            : $"Pet ID: {pet.Id}, Name: {pet.Name}, Status: {pet.Status}";
    }

    [McpServerTool(Name = "add_pet")]
    [Description("Adds a pet to the Swagger Petstore API with a name and status.")]
    public async Task<string> AddPet(
        [Description("The pet name.")] string name,
        [Description("The pet status: available, pending, or sold.")] string status,
        CancellationToken cancellationToken)
    {
        var pet = await petstoreClient.AddPetAsync(name, status, cancellationToken);
        return $"Added pet ID: {pet.Id}, Name: {pet.Name}, Status: {pet.Status}";
    }
}