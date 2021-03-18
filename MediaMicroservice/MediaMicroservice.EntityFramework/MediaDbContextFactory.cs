using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MediaMicroservice.EntityFramework
{
    public class MediaDbContextFactory : IDesignTimeDbContextFactory<MediaDbContext>
    {
        private readonly Action<DbContextOptionsBuilder> _configureDbContext;

        public MediaDbContextFactory()
        {
            var connexionString = "Server=localhost;Username=postgres;Password=pass;Database=entre2ages;Port=5433";
            _configureDbContext = o => o.UseNpgsql(connexionString).UseSnakeCaseNamingConvention();
        }
        
        public MediaDbContextFactory(Action<DbContextOptionsBuilder> configureDbContext)
        {
            _configureDbContext = configureDbContext;
        }

        public MediaDbContext CreateDbContext(string[] args = null)
        {
            var options = new DbContextOptionsBuilder<MediaDbContext>();
            _configureDbContext(options);

            return new MediaDbContext(options.Options);
        }
    }
}
