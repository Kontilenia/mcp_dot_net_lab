using PetApi.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<PetService>();

var app = builder.Build();

app.MapGet("/pets", (PetService pets) => pets.GetAll());

app.MapGet("/pets/{id}", (long id, PetService pets) =>
    pets.GetById(id) is { } pet ? Results.Ok(pet) : Results.NotFound());

app.Run();