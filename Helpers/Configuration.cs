using DreamInMars.Client;
using DreamInMars.Configuration;
using DreamInMars.Logic;
using DreamInMars.Model;
using DreamInMars.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace DreamInMars.Helpers
{
    public static class Configuration
    {
        public static IServiceCollection ConfigureCors(this IServiceCollection services, string origins) =>
            services.AddCors(options =>
             {
                 options.AddPolicy(
                     "AllowAll",
                     builder => builder.WithOrigins(origins)
                                       .AllowAnyMethod()
                                       .AllowAnyHeader());
             });

        public static IServiceCollection ConfigureSwagger(this IServiceCollection services) =>
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Dream In Mars",
                    Contact = new OpenApiContact
                    {
                        Name = "Jason Sacks",
                        Email = "jason.samuel.sacks@gmail.com",
                    }
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. 
                                    Enter 'Bearer' [space] and then your token in the text input below. 
                                    Example: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                  {
                    {
                      new OpenApiSecurityScheme
                      {
                        Reference = new OpenApiReference
                          {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                          },
                          Scheme = "oauth2",
                          Name = "Bearer",
                          In = ParameterLocation.Header,

                        },
                        new List<string>()
                      }
                    });
            });


        public static IServiceCollection ConfigureSecurity(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = false,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["Authentication:JwtSecret"])),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                })
                .AddGoogle(options =>
                {
                    options.ClientId = configuration["Authentication:ClientId"];
                    options.ClientSecret = configuration["Authentication:ClientSecret"];
                });
                                 
                return services.AddAuthorization();
        }

        public static IServiceCollection ConfigureDependencyInjection(this IServiceCollection services, string dbConnectionString)=>
            services
                .AddTransient<IDeepAiClient, DeepAiClient>()
                .AddTransient<INasaMarsRoverClient, NasaMarsRoverClient>()
                .AddTransient<IAuthenticationLogic, AuthenticationLogic>()
                .AddTransient<IDbConnection>(provider => new SqlConnection(dbConnectionString))
                .AddTransient<IAccountRepository, AccountRepository>()
                .AddTransient<IGalleryImageRepository, GalleryImageRepository>()
                .AddTransient<ICreditRepository, CreditRepository>()
                .AddTransient<IAccountLogic, AccountLogic>()
                .AddTransient<IDeepDreamImageLogic, DeepDreamImageLogic>()
                .AddTransient<IFileStorageLogic, AzureStorageLogic>()
                .AddTransient<ICreditLogic,CreditLogic>();


        public static IServiceCollection ConfigureLogging(this IServiceCollection services) => 
            services.AddLogging(options =>
            {
                options.AddConsole();
                options.AddAzureWebAppDiagnostics();
                options.SetMinimumLevel(LogLevel.Information);
            });

        public static IServiceCollection ConfigureHttpClients(this IServiceCollection services)
        {
            services.AddHttpClient<DeepAiClient>();
            services.AddHttpClient<NasaMarsRoverClient>();
            return services;
        }  

        public static IServiceCollection ConfigureConfigurationOptions(this IServiceCollection services, IConfiguration configuration ) =>
            services
                .Configure<AzureConfiguration>(configuration.GetSection("Azure"))
                .Configure<CreditConfiguration>(configuration.GetSection("Credits"))
                .Configure<DeepDreamConfiguration>(configuration.GetSection("DeepDream"))
                .Configure<AuthenticationConfiguration>(configuration.GetSection("Authentication"))
                .Configure<NasaConfiguration>(configuration.GetSection("Nasa"));

        public static IServiceCollection ConfigureDatabase(this IServiceCollection services, string dbConnectionString)
        {

            services.AddDbContextPool<DreamInMarsDbContext>(options =>
                options.UseSqlServer(dbConnectionString));

            services.AddIdentityCore<DreamUser>()
                        .AddEntityFrameworkStores<DreamInMarsDbContext>();
            return services;
        }
    }
}
