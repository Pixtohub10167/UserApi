using Microsoft.EntityFrameworkCore;
using UserApi.Models;

namespace UserApi.Data;

public class UserDbContext : DbContext
{
    public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<User>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Login).HasMaxLength(50).IsRequired();
            e.Property(x => x.PassHash).HasMaxLength(64).IsRequired();
            // Уникальность логина обеспечивается на уровне СУБД,
            // а не только проверкой в коде — защита от гонки запросов
            e.HasIndex(x => x.Login).IsUnique();
        });
    }
}
