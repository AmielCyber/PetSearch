using System.Reflection;
using Microsoft.OpenApi.Models;
using PetSearchAPI.Clients;
using PetSearchAPI.Middleware;
using PetSearchAPI.StronglyTypedConfigurations;

const string petFinderUrl = "https://api.petfinder.com/v2/";
const string petFinderTokenUrl = "https://api.petfinder.com/v2/oauth2/token";
const string myAllowSpecificOrigins = "myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // Read generated XML document
    var file = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, file));
    // Set up Swagger to use a token in our header.
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        BearerFormat = "JWT",
        Description = @"JWT Authorization header using the Bearer scheme.
                        You can get a token by running the React Application,
                        then go on dev tools on your browser and head to storage
                        ,then local storage.
                        The token is stored in the 'app-cache' storage key.

        Enter the bearer token value below:
        Example: `Bearer 87f6a729ee3e4d0f849f6a8992cd2e0a`",

        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        Reference = new OpenApiReference
        {
            Id = "Bearer",
            Type = ReferenceType.SecurityScheme
        }
    };
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Pet Search API", Version = "v1" });
    options.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            jwtSecurityScheme,
            Array.Empty<string>()
        }
    });
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
builder.Services.AddHttpClient<ITokenClient, TokenClient>(client =>
{
    client.BaseAddress = new Uri(petFinderTokenUrl);
});
// End of Services configuration.

var app = builder.Build();

// Register middleware.
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(config => { config.ConfigObject.AdditionalItems.Add("persistAuthorization", "true"); });
}

app.UseHttpsRedirection();
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