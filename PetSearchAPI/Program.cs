using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using PetSearchAPI.Clients;
using PetSearchAPI.Middleware;

const string petFinderUrl = "https://api.petfinder.com/v2/";
const string petFinderTokenUrl = "https://api.petfinder.com/v2/oauth2/token";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

/****************** Add Services ******************/
{
    // Adding swagger dependencies for swagger content.
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
            Scheme = JwtBearerDefaults.AuthenticationScheme,
            Reference = new OpenApiReference
            {
                Id = JwtBearerDefaults.AuthenticationScheme,
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
    // Add HTTP factory into our dependency injection.
    builder.Services.AddHttpClient<IPetFinderClient, PetFinderClient>(client =>
    {
        client.BaseAddress = new Uri(petFinderUrl);
    });
    builder.Services.AddHttpClient<ITokenClient, TokenClient>(client =>
    {
        client.BaseAddress = new Uri(petFinderTokenUrl);
    });
    // Web API controller for routes.
    builder.Services.AddControllers();
}
/****************** End of Services ******************/

// Build application and store the result in app.
var app = builder.Build();

/****************** Add Middleware ******************/
{
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(config =>
        {
            // Save authorization token in a cookie.
            config.ConfigObject.AdditionalItems.Add("persistAuthorization", "true");
        });
    }

    // Set up middleware to serve static content (React)
    // Looks for an html in wwwroot.
    app.UseDefaultFiles();
    // Tell our app to use static files to serve(React).
    app.UseStaticFiles();

    // Global error handling middleware.
    app.UseMiddleware<ExceptionMiddleware>();
    // Use cors configuration.
    app.UseCors(policy =>
    {
        // Allow all headers, any controller method. Allow client to pass cookies with allow credentials.
        policy.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins("http://localhost:5173");
    });

    // Redirects HTTP to HTTPS
    app.UseHttpsRedirection();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    // Endpoints of our client app
    // Tell our server how to handle paths that it doesnt know of but React does.
    // Our IndexController will handle these paths
    app.MapFallbackToController("Index", "Fallback");
    /****************** End of Middlewares ******************/
}

app.Run();