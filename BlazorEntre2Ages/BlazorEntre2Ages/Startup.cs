using System.Net.Http;
using Blazored.LocalStorage;
using BlazorEntre2Ages.Authentication;
using BlazorEntre2Ages.Handlers;
using BlazorEntre2Ages.Models;
using BlazorEntre2Ages.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Radzen;

namespace BlazorEntre2Ages
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
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.Configure<AppSettings>(Configuration.GetSection("Services"));
            services.Configure<RabbitSettings>(Configuration.GetSection("RabbitSettings"));
            
            services.AddSingleton<HttpClient>();
            services.AddSingleton<IChatService, ChatService>();
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<IEventService, EventService>();
            services.AddSingleton<IMessageService, MessageService>();
            services.AddSingleton<IMediaService, MediaService>();
            services.AddSingleton<Rabbit>();
            
            services.AddBlazoredLocalStorage();
            
            services.AddScoped<DialogService>();
            services.AddScoped<NotificationService>();
            services.AddScoped<TooltipService>();
            services.AddScoped<ContextMenuService>();
            services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

            services.AddHttpClient<IUserService, UserService>();
            services.AddHttpClient<IEventService, EventService>();
            services.AddHttpClient<IMessageService, MessageService>();
            services.AddHttpClient<IMediaService, MediaService>();
            
            services.AddHostedService<Rabbit>();
            
            services.AddTransient<ValidateHeaderHandler>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
