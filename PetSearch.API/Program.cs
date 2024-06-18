using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.OpenApi.Models;
using PetSearch.API.Clients;
using PetSearch.API.Handlers;
using PetSearch.API.Middleware;
using PetSearch.API.Configurations;
using PetSearch.Data;
using PetSearch.Data.Services;
using PetSearch.Data.StronglyTypedConfigurations;

const string petFinderUrl = "https://api.petfinder.com/v2/";
const string petFinderTokenUrl = "https://api.petfinder.com/v2/oauth2/token";
const string allowReactProduction = "AllowReactProduction";
const string allowAngularProduction = "AllowAngularProduction";
const string allowReactPreview = "AllowReactPreview";
const string allowLocalClientDevelopment = "AllowLocalClientDevelopment";

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddProblemDetails();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opts =>
{
    opts.SwaggerDoc("v1", new OpenApiInfo()
    {
        Title = "Pet Search",
        Description = "An ASP.NET Core Web API for searching available pets within a local area.",
        Version = "1.1",
        Contact = new OpenApiContact
        {
            Name = "Amiel",
            Url = new Uri("https://github.com/AmielCyber")
        },
    });
    var file = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    opts.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, file));
});
// Set up database connection.
// Set up for EF service
builder.Services.AddDbContext<PetSearchContext>(options =>
{
    string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                              throw new InvalidOperationException("Connection string 'Default Connection not found!");
    options.UseMySQL(connectionString);
});
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: allowReactProduction,
        policy =>
        {
            policy.WithOrigins("https://pet-search-react.netlify.app")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        }
    );
    options.AddPolicy(name: allowAngularProduction,
        policy =>
        {
            policy.WithOrigins("https://pet-search-angular.vercel.app")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        }
    );
    options.AddPolicy(name: allowReactPreview,
        policy =>
        {
            policy.WithOrigins("https://dev--pet-search-react.netlify.app")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        }
    );
    options.AddPolicy(name: allowLocalClientDevelopment,
        policy =>
        {
            policy.WithOrigins("http://localhost:5173")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
            policy.WithOrigins("http://localhost:4200")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        }
    );
});
// Add Strongly type configuration.
builder.Services.Configure<PetFinderConfiguration>(builder.Configuration.GetRequiredSection("PetFinder"));
builder.Services.Configure<MapBoxConfiguration>(builder.Configuration.GetRequiredSection("MapBox"));
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
builder.Services.AddHttpClient<IMapBoxClient, MapBoxClient>(client =>
{
    client.BaseAddress = new Uri(MapBoxConfiguration.Url);
});
builder.Services.AddHsts(options =>
{
    options.Preload = true;
    options.IncludeSubDomains = true;
    options.MaxAge = TimeSpan.FromDays(30);
});
// End of Services configuration.

var app = builder.Build();

if (app.Environment.IsProduction())
{
    // Add HTTP Strict Transport Security. Sends the browser this header. 
    app.UseHsts();
}

app.UseHttpsRedirection(); // Configure the HTTP request pipeline.
// Ensure Database is created.
{
    using var scope = app.Services.CreateScope();
    using var context = scope.ServiceProvider.GetRequiredService<PetSearchContext>();
    context.Database.EnsureCreated();
}
app.UseMiddleware<ExceptionMiddleware>(); // Global error handling middleware.
app.UseStatusCodePages(); // Add a problem details that have no response body and an error status code.
app.UseSwagger(); // Expose swagger.
app.UseSwaggerUI(); // Show swagger UI @ /swagger/index.html
app.UseRouting(); // Move default middleware below the client-app middleware to short-circuit client-app routes. 
if (app.Environment.IsDevelopment()) // Use cors configuration to develop with our client app.
{
    app.UseCors(allowLocalClientDevelopment);
}

if (app.Environment.IsProduction())
{
    app.UseCors(allowReactProduction);
    app.UseCors(allowAngularProduction);
    app.UseCors(allowReactPreview);
}

// Register endpoint groups.
RouteGroupBuilder petsApi = app.MapGroup("/api/pets").WithParameterValidation();
RouteGroupBuilder locationApi = app.MapGroup("/api/location").WithParameterValidation();

if (app.Environment.IsProduction())
{
    // Redirect to the new React application URL, since we are not serving React files anymore.
    app.MapGet("/", () => Results.Redirect("https://pet-search-react.netlify.app", true))
        .ExcludeFromDescription();
}

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
petsApi.MapGet("/{id}", PetHandler.GetSinglePet).WithName("GetSinglePet");
locationApi.MapGet("/zipcode/{zipcode}", LocationHandler.GetLocationFromZipCode).WithName("GetLocationFromZipCode");
locationApi.MapGet("/coordinates", LocationHandler.GetLocationFromCoordinates).WithName("GetLocationFromCoordinates");

app.Run();