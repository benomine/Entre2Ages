using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediaMicroservice.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace MediaMicroservice.EntityFramework
{
    public class MediaDbContext : DbContext
    {
        public DbSet<Media> Medias { get; set; }

        public MediaDbContext(DbContextOptions options) : base(options) { }
    }
}
