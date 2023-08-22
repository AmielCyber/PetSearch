using System.Net.Mime;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.OpenApi.Models;
using PetSearch.API.Clients;
using PetSearch.API.Handlers;
using PetSearch.API.Middleware;
using PetSearch.API.StronglyTypedConfigurations;
using PetSearch.Data;
using PetSearch.Data.Services;
using PetSearch.Data.StronglyTypedConfigurations;

const string petFinderUrl = "https://api.petfinder.com/v2/";
const string petFinderTokenUrl = "https://api.petfinder.com/v2/oauth2/token";
const string mapBoxUrl = "https://api.mapbox.com/geocoding/v5/mapbox.places/";
const string myAllowSpecificOrigins = "myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddProblemDetails();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opts =>
{
    opts.SwaggerDoc("v1", new OpenApiInfo()
    {
        Title = "Pet Search",
        Description = "An API to search available pets around a local area.",
        Version = "1.0",
        Contact = new OpenApiContact
        {
            Name = "Amiel",
            Url = new Uri("https://github.com/AmielCyber")
        }
    });
    var file = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    opts.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, file));
});
// Set up database connection.
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                          throw new InvalidOperationException("Connection string 'Default Connection not found!");
// Set up for EF service
MySqlServerVersion serverVersion = new MySqlServerVersion(new Version(8, 0, 31));
builder.Services.AddDbContext<PetSearchContext>(options => options.UseMySql(connectionString, serverVersion));
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: myAllowSpecificOrigins,
        policy => { policy.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins("http://localhost:5173"); }
    );
});
// Add Strongly type configuration.
builder.Services.Configure<PetFinderConfiguration>(builder.Configuration.GetSection("PetFinder"));
builder.Services.Configure<MapBoxConfiguration>(builder.Configuration.GetSection("MapBox"));
// Add HTTPClient to DI container.
builder.Services.AddHttpClient<IPetFinderClient, PetFinderClient>(client =>
{
    client.BaseAddress = new Uri(petFinderUrl);
});
builder.Services.AddMemoryCache();
builder.Services.AddHttpClient<TokenService>(client => { client.BaseAddress = new Uri(petFinderTokenUrl); });
builder.Services.AddTransient<ITokenService>(
    s => new CachedTokenService(s.GetRequiredService<TokenService>(), s.GetRequiredService<IMemoryCache>())
);
builder.Services.AddHttpClient<IMapBoxClient, MapBoxClient>(client => { client.BaseAddress = new Uri(mapBoxUrl); });
// End of Services configuration.

var app = builder.Build();

// Ensure Database is created.
{
    using var scope = app.Services.CreateScope();
    using var context = scope.ServiceProvider.GetRequiredService<PetSearchContext>();
    context.Database.EnsureCreated();
}
app.UseHttpsRedirection(); // Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionMiddleware>(); // Global error handling middleware.
app.UseDefaultFiles(); // Serve default file from wwwroot w/o requesting URL file name.
app.UseStaticFiles(); // Set up middleware to serve static content (React).
app.UseStatusCodePages(); // Add a problem details that have no response body and an error status code.
app.UseSwagger(); // Expose swagger.
app.UseSwaggerUI(); // Show swagger UI @ /swagger/index.html
app.UseRouting(); // Move default middleware below the client-app middleware to short-circuit client-app routes. 
if (app.Environment.IsDevelopment()) // Use cors configuration to develop with our client app.
{
    app.UseCors(myAllowSpecificOrigins);
}
// Fallback handler for 
app.MapFallback(() =>
    Results.File(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "index.html"), MediaTypeNames.Text.Html));

// Register endpoint groups.
RouteGroupBuilder petsApi = app.MapGroup("/api/pets").WithParameterValidation();
RouteGroupBuilder locationApi = app.MapGroup("/api/location").WithParameterValidation();

// Register endpoints with their handlers.
petsApi.MapGet("/", PetHandler.GetPets).WithName("GetPets").WithOpenApi(o =>
{
    // Doing this ugly workaround since swagger currently does not support xml docs with AsParameters objects.
    o.Parameters[0].Description = "Search for either 'dog' or 'cat'.";
    o.Parameters[1].Description = "Pets around the given zip code. Only 5 digit zip codes are supported.";
    o.Parameters[2].Description = "Page number in the pet list.";
    o.Parameters[3].Description = "The distance range from the passed zip code.";
    o.Parameters[4].Description = "Sort value (- Descending): distance, -distance, recent, -recent";
    return o;
});
// Tell our server how to handle paths that it doesnt know of but React does.
petsApi.MapGet("/{id}", PetHandler.GetSinglePet).WithName("GetSinglePet");
locationApi.MapGet("/zipcode/{zipcode}", LocationHandler.GetLocationFromZipCode).WithName("GetLocationFromZipCode");
locationApi.MapGet("/coordinates", LocationHandler.GetLocationFromCoordinates).WithName("GetLocationFromCoordinates");

app.Run();