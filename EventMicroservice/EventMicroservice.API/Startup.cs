using System;
using EventMicroservice.EntityFramework;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Steeltoe.Discovery.Client;
using Steeltoe.Discovery.Consul;

namespace EventMicroservice.API
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
            string connexionString = Configuration.GetConnectionString("ConnectionString");
            Action<DbContextOptionsBuilder> configureDbContext =
                o => o.UseNpgsql(connexionString).UseSnakeCaseNamingConvention();

            services.AddDbContext<EventDbContext>(configureDbContext);
            services.AddSingleton(new EventDbContextFactory(configureDbContext));
            services.AddControllers();
            services.AddDiscoveryClient(Configuration);
            services.AddServiceDiscovery(o => o.UseConsul());
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "EventMicroservice.API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "EventMicroservice.API v1"));
            }

            app.UseHttpsRedirection();

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
