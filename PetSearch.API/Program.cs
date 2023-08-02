using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PetSearch.API.Clients;
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

// Add services to the container.
builder.Services.AddControllers();
// Set up Database connection.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
// Set up for EF service
var serverVersion = new MySqlServerVersion(new Version(8, 0, 31));
builder.Services.AddDbContext<PetSearchContext>(
    // Specify the database provider
    options => options.UseMySql(connectionString, serverVersion));
// Set up Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // Read generated XML document
    var file = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, file));
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Pet Search API", Version = "v1" });
});

// Configure CORS for local development with React.
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: myAllowSpecificOrigins,
        policy =>
        {
            policy.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins("http://localhost:5173");
        });
});
// Add Strongly type configuration.
builder.Services.Configure<PetFinderConfiguration>(builder.Configuration.GetSection("PetFinder"));
builder.Services.Configure<MapBoxConfiguration>(builder.Configuration.GetSection("MapBox"));
// Add HTTPClient to DI container.
builder.Services.AddHttpClient<IPetFinderClient, PetFinderClient>(client =>
{
    client.BaseAddress = new Uri(petFinderUrl);
});
builder.Services.AddHttpClient<ITokenService, TokenService>(client =>
{
    client.BaseAddress = new Uri(petFinderTokenUrl);
});
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
app.UseSwagger(); // Expose swagger.
app.UseSwaggerUI(); // Show swagger UI @ /swagger/index.html
app.UseMiddleware<ExceptionMiddleware>(); // Global error handling middleware.
app.UseDefaultFiles(); // Serve default file from wwwroot w/o requesting URL file name.
app.UseStaticFiles(); // Set up middleware to serve static content (React).
app.UseRouting(); // Move default middleware below the client-app middleware to short-circuit client-app routes. 
if (app.Environment.IsDevelopment()) // Use cors configuration to develop with our client app.
{
    app.UseCors(myAllowSpecificOrigins);
}

app.UseAuthorization();
app.MapControllers();
app.MapFallbackToController("Index",
    "Fallback"); // Tell our server how to handle paths that it doesnt know of but React does.
// End up setting up middleware to the pipeline.

app.Run();