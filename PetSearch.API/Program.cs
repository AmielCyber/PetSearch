using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PetSearch.API.Clients;
using PetSearch.API.Middleware;
using PetSearch.Data;
using PetSearch.Data.Services;
using PetSearch.Data.StronglyTypedConfigurations;

const string petFinderUrl = "https://api.petfinder.com/v2/";
const string petFinderTokenUrl = "https://api.petfinder.com/v2/oauth2/token";
const string myAllowSpecificOrigins = "myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Set up Database connection.
// Set up for EF service
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
var serverVersion = new MySqlServerVersion(new Version(8, 0, 31));
builder.Services.AddDbContext<PetSearchContext>(
    // Specify the database provider
    options => options.UseMySql(connectionString, serverVersion));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // Read generated XML document
    var file = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, file));
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Pet Search API", Version = "v1" });
});

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
builder.Services.AddHttpClient<IPetFinderClient, PetFinderClient>(client =>
{
    client.BaseAddress = new Uri(petFinderUrl);
});
builder.Services.AddHttpClient<ITokenService, TokenService>(client =>
{
    client.BaseAddress = new Uri(petFinderTokenUrl);
});
// End of Services configuration.

var app = builder.Build();

// Register middleware.

if (app.Environment.IsDevelopment())
{
}

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
// Expose swagger.
app.UseSwagger();
app.UseSwaggerUI(config => { config.ConfigObject.AdditionalItems.Add("persistAuthorization", "true"); });
// Global error handling middleware.
app.UseMiddleware<ExceptionMiddleware>();
// Set up middleware to serve static content (React)
app.UseDefaultFiles();
app.UseStaticFiles();
// Move default middleware below the client-app middleware to short-circuit client-app routes. 
app.UseRouting();
if (app.Environment.IsDevelopment())
{
    // Use cors configuration to develop with our client app.
    app.UseCors(myAllowSpecificOrigins);
}

app.UseAuthorization();
app.MapControllers();
// Tell our server how to handle paths that it doesnt know of but React does.
app.MapFallbackToController("Index", "Fallback");

app.Run();