using EventMicroservice.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventMicroservice.EntityFramework
{
    public class EventDbContext : DbContext
    {
        private readonly Action<DbContextOptionsBuilder> _configureDbContext;
        public DbSet<Event> Events { get;set;}

        public EventDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
