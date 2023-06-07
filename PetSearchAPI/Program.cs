using Microsoft.OpenApi.Models;
using PetSearchAPI.Clients;
using PetSearchAPI.Middleware;

const string petFinderUrl = "https://api.petfinder.com/v2/";
const string petFinderTokenUrl = "https://api.petfinder.com/v2/oauth2/token";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // Set up Swagger to use a token in our header.
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        BearerFormat = "JWT",
        Description = @"JWT Authorization header using the Bearer scheme.
                        Enter the bearer token value below:
                        Example: `87f6a729ee3e4d0f849f6a8992cd2e0a`",
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
builder.Services.AddCors();
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
    app.UseSwaggerUI();
}

// Set up middleware to serve static content (React)
app.UseDefaultFiles();
app.UseStaticFiles();
// Move default middleware below the client-app middleware to short-circuit client-app routes. 
app.UseRouting();
// Global error handling middleware.
app.UseMiddleware<ExceptionMiddleware>();
// Use cors configuration.
app.UseCors(policy =>
{
    // Allow all headers, any controller method. Allow client to pass cookies with allow credentials.
    policy.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins("http://localhost:5173");
});
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
// Tell our server how to handle paths that it doesnt know of but React does.
app.MapFallbackToController("Index", "Fallback");

app.Run();