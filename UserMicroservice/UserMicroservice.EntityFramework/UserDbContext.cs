using Microsoft.EntityFrameworkCore;
using UserMicroservice.Domain.Models;

namespace UserMicroservice.EntityFramework
{
    public class UserDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        
        public UserDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /*modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.HasKey(e => e.TokenId);
                entity.ToTable("RefreshToken");
                entity.Property(e => e.TokenId).HasColumnName("token_id");
                entity.Property(e => e.ExpiryDate)
                    .HasColumnName("expiry_date")
                    ;
                entity.Property(e => e.Token)
                    .IsRequired()
                    .HasColumnName("token")
                    .HasMaxLength(200)
                    .IsUnicode(false);
                entity.Property(e => e.UserId).HasColumnName("user_id");
                entity.HasOne(d => d.User)
                    .WithMany(p => p.RefreshTokens)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__RefreshTo__user___60FC61CA");
            });
            
             modelBuilder.Entity<User>(entity =>
             {
                 entity.HasKey(e => e.Id)
                     .HasName("PK_user_id_2");
                     
                entity.ToTable("User");

                entity.Property(e => e.Id).HasColumnName("user_id");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email_address")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Address)
                    .HasColumnName("address")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.City)
                    .HasColumnName("city")
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.ZipCode)
                    .HasColumnName("zipcode")
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .IsFixedLength();
                
                entity.Property(e => e.Phone)
                    .HasColumnName("phone")
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .IsFixedLength();
                
                entity.Property(e => e.Phone2)
                    .HasColumnName("phone_2")
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .IsFixedLength();
                
                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasMaxLength(100)
                    .IsUnicode(false);
             });*/
            
            base.OnModelCreating(modelBuilder);
        }
    }
}