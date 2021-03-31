using DreamInMars.Dto;
using DreamInMars.Helpers;
using DreamInMars.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DreamInMars
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
    
        public void ConfigureServices(IServiceCollection services)
        {

            var dbConnectionString = Configuration.GetConnectionString("DreaminMarsDbContext");

            services
                .ConfigureCors(Configuration.GetValue<string>("Cors:Origins"))
                .ConfigureSwagger()
                .ConfigureSecurity(Configuration)
                .ConfigureDependencyInjection(dbConnectionString)
                .ConfigureLogging()
                .ConfigureHttpClients()
                .ConfigureDatabase(dbConnectionString)
                .ConfigureConfigurationOptions(Configuration)
                .AddAutoMapper(map => map.CreateMap<AccountInfo, Account>());
            
                     
            services.AddControllers();
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app
                .UseCors("AllowAll")
                .UseExceptionHandler("/error")
                .UseHttpsRedirection()
                .UseRouting()
                .UseAuthentication()
                .UseAuthorization()
                .UseEndpoints(endpoints => { endpoints.MapControllers(); })
                .UseSwagger()
                .UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "DreamInMars v1"); });
        }
    }
}

