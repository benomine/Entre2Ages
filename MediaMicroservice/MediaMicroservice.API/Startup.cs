using System;
using MediaMicroservice.EntityFramework;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Steeltoe.Discovery.Client;
using Steeltoe.Discovery.Consul;

namespace MediaMicroservice.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            var connectionString = Configuration.GetConnectionString("ConnectionString");
            Action<DbContextOptionsBuilder> configureDbContext = 
                o => o.UseNpgsql(connectionString).UseSnakeCaseNamingConvention();

            services.AddDbContext<MediaDbContext>(configureDbContext);
            services.AddSingleton(new MediaDbContextFactory(configureDbContext));
            services.AddControllers();
            services.AddDiscoveryClient(Configuration);
            services.AddServiceDiscovery(o => o.UseConsul());

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MediaService.API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MediaService.API v1"));
            }

            //app.UseHttpsRedirection();

            app.UseRouting();
            app.UseDiscoveryClient();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
