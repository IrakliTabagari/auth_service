using System.Runtime.InteropServices.ComTypes;
using AuthService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthService.Infrastructure.Data;

public class AuthDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    
    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(ConfigureUser);
    }

    private void ConfigureUser(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users", "public");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id");
        builder.Property(x => x.FirstName).HasColumnName("firstname");
        builder.Property(x => x.LastName).HasColumnName("lastname");
        builder.Property(x => x.Email).HasColumnName("email");
        builder.Property(x => x.Password).HasColumnName("password");
        builder.Property(x => x.IsActive).HasColumnName("is_active");
    }
}