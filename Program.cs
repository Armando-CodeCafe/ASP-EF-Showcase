
namespace ASPLes;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAuthorization();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        TestAppContext context = new TestAppContext();
        app.MapGet("/users/validate", (string token) =>
        {
            if (IsAdmin(context, token))
            {
                return Results.Ok("Admin");
            }
            return Results.BadRequest();
        });
        app.MapGet("/users/login", (string email, string password) =>
        {
            foreach (User u in context.Users)
            {
                if (u.Email == email && u.Password == password)
                {
                    return Results.Ok(u);
                }

            }
            return Results.BadRequest();
        });

        app.MapGet("/users", (string token) =>
        {
            if (IsAdmin(context, token))
            {
                return Results.Ok(context.Users.ToArray());

            }
            return Results.BadRequest();
        });
        app.MapGet("/users/{id}", (int id, string token) =>
        {
            if (!IsAdmin(context, token))
                return Results.BadRequest();


            return Results.Ok(context.Users.Find(id));

        });
        app.MapPost("/users", (User u) =>
        {
            context.Users.Add(u);
            context.SaveChanges();
            return u;
        });
        app.MapDelete("/users/{id}", (int id) =>
        {
            User? usertodelete = context.Users.Find(id);
            if (usertodelete != null)
                context.Users.Remove(usertodelete);
            context.SaveChanges();
        });



        app.Run();
    }
    public static bool IsAdmin(TestAppContext context, string token)
    {
        foreach (User u in context.Users)
        {
            if (u.Token == token && u.IsAdmin)
            {
                return true;
            }

        }
        return false;
    }
}
