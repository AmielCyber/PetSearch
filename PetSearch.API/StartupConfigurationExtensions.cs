using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using PetSearch.API.Clients;
using PetSearch.API.Configurations;
using PetSearch.API.Problems;
using PetSearch.API.Middleware;
using PetSearch.API.Profiles;
using PetSearch.Data;
using PetSearch.Data.Services;
using PetSearch.Data.StronglyTypedConfigurations;

namespace PetSearch.API;

public static class StartupConfigurationExtensions
{
    private const string PetFinderTokenUrl = "https://api.petfinder.com/v2/oauth2/token";

    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddProblemDetails();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(ServiceConfiguration.SetSwaggerOptions());
        builder.Services.AddDbContext<PetSearchContext>(options =>
        {
            string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                                      throw new InvalidOperationException(
                                          "Connection string 'Default Connection not found!");
            options.UseMySQL(connectionString);
        });
        builder.Services.AddCors(ServiceConfiguration.SetCorsOptions());
        // Add Strongly type configuration.
        builder.Services.Configure<PetFinderConfiguration>(builder.Configuration.GetRequiredSection("PetFinder"));
        builder.Services.Configure<MapBoxConfiguration>(builder.Configuration.GetRequiredSection("MapBox"));
        // Add HTTPClient to DI container.
        builder.Services.AddHttpClient<IPetFinderClient, PetFinderClient>(client =>
        {
            // TODO: Add retry policy
            client.BaseAddress = new Uri(PetFinderConfiguration.Uri);
        });
        builder.Services.AddMemoryCache();
        builder.Services.AddHttpClient<TokenService>(client => { client.BaseAddress = new Uri(PetFinderTokenUrl); });
        builder.Services.AddTransient<ITokenService>(
            s => new CachedTokenService(s.GetRequiredService<TokenService>(), s.GetRequiredService<IMemoryCache>())
        );
        builder.Services.AddKeyedSingleton<IExpectedProblems, MapBoxProblems>("mapBoxProblems");
        builder.Services.AddKeyedSingleton<IExpectedProblems, PetFinderProblems>("petFinderProblems");
        builder.Services.AddSingleton<PetProfile>();
        builder.Services.AddSingleton<PaginationMetaDataProfile>();
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

        return builder;
    }


    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        if (app.Environment.IsProduction())
        {
            // Add HTTP Strict Transport Security. Sends the browser this header. 
            app.UseHsts();
            app.UseHttpsRedirection(); // Configure the HTTPS request pipeline.
        }

        app.UseMiddleware<ExceptionMiddleware>(); // Global error handling middleware.
        app.UseStatusCodePages(); // Add a problem details that have no response body and an error status code.
        app.UseSwagger(); // Expose swagger.
        app.UseSwaggerUI(); // Show swagger UI @ /swagger/index.html
        if (app.Environment.IsDevelopment()) // Use cors configuration to develop with our client app.
            app.UseCors(ServiceConfiguration.AllowLocalClientDevelopment);

        if (app.Environment.IsProduction())
        {
            app.UseCors(ServiceConfiguration.AllowReactProduction);
            app.UseCors(ServiceConfiguration.AllowAngularProduction);
            app.UseCors(ServiceConfiguration.AllowReactPreview);
        }

        return app;
    }
}