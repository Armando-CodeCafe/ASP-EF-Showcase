namespace ASPLes;

public class Program
{
    public static void Main(string[] args)
    {
        // Maak een nieuwe builder voor de webapplicatie aan
        var builder = WebApplication.CreateBuilder(args);

        // Voeg services toe aan de container.
        // Voeg autorisatie-services toe
        builder.Services.AddAuthorization();

        // Leer meer over het configureren van Swagger/OpenAPI op https://aka.ms/aspnetcore/swashbuckle
        // Voeg ondersteuning voor het verkennen van eindpunten toe
        builder.Services.AddEndpointsApiExplorer();
        // Voeg Swagger-generatie toe voor API-documentatie
        builder.Services.AddSwaggerGen();

        // Bouw de applicatie
        var app = builder.Build();

        // Configureer de HTTP-verzoekpijplijn.
        // Als de omgeving in ontwikkeling is, gebruik Swagger en de Swagger UI
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // Forceer HTTPS-omleiding
        app.UseHttpsRedirection();

        // Maak een nieuwe context voor de testapplicatie aan
        TestAppContext context = new TestAppContext();

        // Definieer een GET-eindpunt voor gebruikersvalidatie
        app.MapGet("/users/validate", (string token) =>
        {
            // Controleer of de gebruiker een admin is
            if (IsAdmin(context, token))
            {
                return Results.Ok("Admin");
            }
            return Results.BadRequest();
        });

        // Definieer een GET-eindpunt voor gebruikerslogin
        app.MapGet("/users/login", (string email, string password) =>
        {
            // Controleer de inloggegevens van de gebruiker
            foreach (User u in context.Users)
            {
                if (u.Email == email && u.Password == password)
                {
                    return Results.Ok(u);
                }
            }
            return Results.BadRequest();
        });

        // Definieer een GET-eindpunt om alle gebruikers op te halen
        app.MapGet("/users", (string token) =>
        {
            // Controleer of de gebruiker een admin is
            if (IsAdmin(context, token))
            {
                return Results.Ok(context.Users.ToArray());
            }
            return Results.BadRequest();
        });

        // Definieer een GET-eindpunt om een specifieke gebruiker op te halen op basis van ID
        app.MapGet("/users/{id}", (int id, string token) =>
        {
            // Controleer of de gebruiker een admin is
            if (!IsAdmin(context, token))
                return Results.BadRequest();

            // Zoek en retourneer de gebruiker met het opgegeven ID
            return Results.Ok(context.Users.Find(id));
        });

        // Definieer een POST-eindpunt om een nieuwe gebruiker toe te voegen
        app.MapPost("/users", (User u) =>
        {
            // Voeg de nieuwe gebruiker toe aan de context en sla de wijzigingen op
            context.Users.Add(u);
            context.SaveChanges();
            return u;
        });

        // Definieer een DELETE-eindpunt om een gebruiker te verwijderen op basis van ID
        app.MapDelete("/users/{id}", (int id) =>
        {
            // Zoek de gebruiker die moet worden verwijderd
            User? usertodelete = context.Users.Find(id);
            if (usertodelete != null)
                context.Users.Remove(usertodelete);
            // Sla de wijzigingen op
            context.SaveChanges();
        });

        // Start de applicatie
        app.Run();
    }

    // Methode om te controleren of een gebruiker een admin is op basis van het token
    public static bool IsAdmin(TestAppContext context, string token)
    {
        foreach (User u in context.Users)
        {
            // Controleer of het token overeenkomt met een admin-gebruiker
            if (u.Token == token && u.IsAdmin)
            {
                return true;
            }
        }
        return false;
    }
}