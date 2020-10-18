using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimonsVossCodingCase.RegistrationService.Configuration;
using SimonsVossCodingCase.RegistrationService.Services;

namespace SimonsVossCodingCase.RegistrationService
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
    
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerGen();
            services.AddMvc();
            services.AddApplicationInsightsTelemetry();
            //Configs
            services.Configure<ApiConfiguration>(_configuration.GetSection(nameof(ApiConfiguration)));
            
            //DI
            services.AddScoped<IRemoteSignService, RemoteSignService>();
            services.AddScoped<ILicenseKeyValidatorService, LicenseKeyValidatorService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseRouting();
            
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}