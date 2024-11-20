using System;
using Microsoft.EntityFrameworkCore;
namespace ASPLes;


public class TestAppContext : DbContext
{
    public DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySql(@"Server=localhost;User=root;Database=aspShowcase", ServerVersion.Parse("8.0.21-mysql"));
    }
}
