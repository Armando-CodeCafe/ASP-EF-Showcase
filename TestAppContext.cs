using System;
using Microsoft.EntityFrameworkCore;

namespace ASPLes;

// Definieer de databasecontext voor de applicatie
public class TestAppContext : DbContext
{
    // Definieer een DbSet voor gebruikers
    public DbSet<User> Users { get; set; }

    // Configureer de databaseverbinding
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Stel de MySQL-databaseverbinding in met de juiste server, gebruiker en database
        optionsBuilder.UseMySql(@"Server=localhost;User =root;Database=aspShowcase", ServerVersion.Parse("8.0.21-mysql"));
    }
}