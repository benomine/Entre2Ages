using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;

namespace EventMicroservice.EntityFramework
{
    public class EventDbContextFactory : IDesignTimeDbContextFactory<EventDbContext>
    {
        private readonly Action<DbContextOptionsBuilder> _configureDbContext;

        public EventDbContextFactory()
        {
            var connexionString = "Server=localhost;Username=postgres;Password=pass;Database=entre2ages;Port=5431";
            _configureDbContext = o => o.UseNpgsql(connexionString).UseSnakeCaseNamingConvention();
        }

        public EventDbContextFactory(Action<DbContextOptionsBuilder> configureDbContext)
        {
            this._configureDbContext = configureDbContext;
        }

        public EventDbContext CreateDbContext(string[] args = null)
        {
            var options = new DbContextOptionsBuilder<EventDbContext>();

            _configureDbContext(options);

            return new EventDbContext(options.Options);
        }
    }
}
