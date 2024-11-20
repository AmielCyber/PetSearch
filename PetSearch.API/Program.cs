using PetSearch.API;
using PetSearch.API.Configurations;
using PetSearch.API.Handlers;
using PetSearch.API.Models;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.ConfigureServices();

WebApplication app = builder.Build();
app.ConfigurePipeline();

RouteGroupBuilder petsApi = app.MapGroup("/api/pets").WithParameterValidation().WithTags("pets");
RouteGroupBuilder locationApi = app.MapGroup("/api/location").WithParameterValidation().WithTags("location");

if (app.Environment.IsProduction())
    app.MapGet("/", () => Results.Redirect(ServiceConfiguration.AngularProductionUri, true))
        .ExcludeFromDescription();

petsApi.MapGet("/", PetHandler.GetPetsAsync)
    .WithName("GetPets")
    .WithOpenApi(ServiceConfiguration.GetPetsOpenApiConfiguration())
    .Produces<List<PetDto>>()
    .ProducesValidationProblem();

petsApi.MapGet("/{id}", PetHandler.GetSinglePetAsync)
    .WithName("GetPet")
    .Produces<PetDto>()
    .ProducesValidationProblem()
    .ProducesProblem(StatusCodes.Status404NotFound);

locationApi.MapGet("/zipcode/{zipcode}", LocationHandler.GetLocationFromZipCodeAsync)
    .WithName("GetLocationFromZipCode")
    .ProducesValidationProblem()
    .ProducesProblem(StatusCodes.Status404NotFound);

locationApi.MapGet("/coordinates", LocationHandler.GetLocationFromCoordinatesAsync)
    .WithName("GetLocationFromCoordinates")
    .ProducesValidationProblem()
    .ProducesProblem(StatusCodes.Status404NotFound);

app.Run();