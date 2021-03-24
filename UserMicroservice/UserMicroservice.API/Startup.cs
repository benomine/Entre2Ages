using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Steeltoe.Discovery.Client;
using Steeltoe.Discovery.Consul;
using UserMicroservice.Domain.Models;
using UserMicroservice.EntityFramework;

namespace UserMicroservice.API
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

            services.AddDbContext<UserDbContext>(configureDbContext);
            services.AddSingleton(new UserDbContextFactory(configureDbContext));
            services.AddControllers();
            services.AddDiscoveryClient(Configuration);
            services.AddServiceDiscovery(o => o.UseConsul());
            
            var jwtSection = Configuration.GetSection("JwtSettings");
            services.Configure<JwtSettings>(jwtSection);

            var appSettings = jwtSection.Get<JwtSettings>();
            services.AddSingleton<JwtSettings>(appSettings);
            var key = Encoding.ASCII.GetBytes(appSettings.SecretKey);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = true;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };
            });
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "UserMicroservice", Version = "v1"});
                c.EnableAnnotations();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "UserMicroservice v1"));
            }

            //app.UseHttpsRedirection();
            app.UseDiscoveryClient();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}