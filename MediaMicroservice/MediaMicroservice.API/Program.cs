using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using MediaMicroservice.API;
using MediaMicroservice.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MediaService.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            using (IServiceScope scope = host.Services.CreateScope())
            {
                using (MediaDbContext context = scope.ServiceProvider.GetRequiredService<MediaDbContext>())
                {
                    context.Database.Migrate();
                }
            }
            
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
