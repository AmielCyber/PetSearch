using System.Reflection;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace PetSearch.API.Configurations;

public static class ServiceConfiguration
{
    public const string AllowAngularProduction = "AllowAngularProduction";
    public const string AngularProductionUri = "https://pet-search-angular.vercel.app";

    public const string AllowReactProduction = "AllowReactProduction";
    public const string AllowReactPreview = "AllowReactPreview";
    private const string ReactProductionUri = "https://pet-search-react.netlify.app";
    private const string ReactPreviewUri = "https://dev--pet-search-react.netlify.app";

    public const string AllowLocalClientDevelopment = "AllowLocalClientDevelopment";

    public static Action<SwaggerGenOptions> SetSwaggerOptions()
    {
        return opts =>
        {
            opts.SwaggerDoc("v1", new OpenApiInfo()
            {
                Title = "Pet Search",
                Description = "An ASP.NET Core Web API for searching available pets within a local area.",
                Version = "1.0",
                Contact = new OpenApiContact
                {
                    Name = "Amiel",
                    Url = new Uri("https://github.com/AmielCyber")
                },
            });
            var file = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            opts.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, file));
        };
    }

    public static Action<CorsOptions> SetCorsOptions()
    {
        return options =>
        {
            options.AddPolicy(name: AllowReactProduction,
                policy =>
                {
                    policy.WithOrigins(ReactProductionUri)
                        .WithExposedHeaders("X-Pagination")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                }
            );
            options.AddPolicy(name: AllowAngularProduction,
                policy =>
                {
                    policy.WithOrigins(AngularProductionUri)
                        .WithExposedHeaders("X-Pagination")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                }
            );
            options.AddPolicy(name: AllowReactPreview,
                policy =>
                {
                    policy.WithOrigins(ReactPreviewUri)
                        .WithExposedHeaders("X-Pagination")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                }
            );
            options.AddPolicy(name: AllowLocalClientDevelopment,
                policy =>
                {
                    policy.WithOrigins("http://localhost:5173")
                        .WithExposedHeaders("X-Pagination")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                    policy.WithOrigins("http://localhost:4200")
                        .WithExposedHeaders("X-Pagination")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                }
            );
        };
    }

    public static Func<OpenApiOperation, OpenApiOperation> GetPetsOpenApiConfiguration()
    {
        return o =>
        {
            // Doing this ugly workaround since swagger currently does not support xml docs with AsParameters objects.
            o.Parameters[0].Description = "Search for either 'dog' or 'cat'.";
            o.Parameters[1].Description = "Pets around the given zip code. Only 5 digit zip codes are supported.";
            o.Parameters[2].Description = "Page number in the pet list.";
            o.Parameters[3].Description = "The distance range from the passed zip code.";
            o.Parameters[4].Description = "Sort value (- Descending): distance, -distance, recent, -recent";
            return o;
        };
    }
}