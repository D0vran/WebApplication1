using System.Net.Cache;
using System.Net;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using MyCookies;


/*List<Person> users = new List<Person>()
{
    new Person() { Id = id++, Name = "Dowran", Age = 20, info = new info{ adress = "Lukmanlar 11", phoneNumber = "+99363175332"} },
    new Person() { Id = id++, Name = "Dowlet", Age = 22,  info = new info{ adress = "Lukmanlar 11", phoneNumber = "+99363443874"}},
};*/


var builder = WebApplication.CreateBuilder();

string connection = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<DataBase>(options => options.UseSqlServer(connection));

var app = builder.Build();

app.MapGet("api/users", async (DataBase db) => Results.Json(db.Users));

app.MapGet("api/users/{id:int}", async(int id, DataBase db) =>
{
    
    var user = await db.Users.FirstOrDefaultAsync(u => u.Id == id);
    if (user == null) return Results.NotFound(new { message = "User Not found" });
    return Results.Json(user);
});

app.MapDelete("api/users/{id}", async(int id, DataBase db) =>
{
    Person? user = await db.Users.FirstOrDefaultAsync(u => u.Id == id);
    if (user == null) return Results.NotFound(new { message = "User Not found" });
    db.Users.Remove(user);
    await db.SaveChangesAsync();
    return Results.Json(user);
});

app.MapPost("api/users/", async (Person user, DataBase db) =>
{
    if (user == null) return Results.NotFound(new { message = "User Not found" });
    user.Id = DataBase.id++;
    await db.Users.AddAsync(user);
    await db.SaveChangesAsync();
    return Results.Json(user);
});

app.MapPut("api/users/", async(Person person, DataBase db) =>
{
    var user = await db.Users.FirstOrDefaultAsync(u => u.Id == person.Id);
    if (user == null) return Results.NotFound(new { message = "User Not found" });
    user.Name = person.Name;
    user.Age = person.Age;
    await db.SaveChangesAsync();
    return Results.Json(user);
});

app.MapPost("/data", async (HttpContext httpContext) =>
{
    using StreamReader filestr = new StreamReader(httpContext.Request.Body);
    string message = await filestr.ReadToEndAsync();
    await httpContext.Response.WriteAsync(message);
});

app.MapPost("/data/files", async(HttpContext httpContext) =>
{
    var files = httpContext.Request.Form.Files;
    var uploadPath = $"{Directory.GetCurrentDirectory()}/uploads";
    Directory.CreateDirectory(uploadPath);
    
    foreach (var file in files)
    {
        string fullPath = $"{uploadPath}/{file.FileName}";
        using (var fileStream = new FileStream(fullPath, FileMode.Create))
        {
            await file.CopyToAsync(fileStream);
        }
    }

    await httpContext.Response.WriteAsync("File is succesfully uploaded!");
});


app.MapPost("data/files", async(HttpContext context) =>           //this is for saving mixed content, for ex: dictionaries, collections;
{
    var form = context.Request.Form;

    string? username = form["username"];
    string? email = form["email"];

    IFormFileCollection formFiles = new FormFileCollection();

    var uploadPath = $"{Directory.GetCurrentDirectory()}/uploads";
    Directory.CreateDirectory(uploadPath);

    foreach(var file in formFiles)
    {
        string fullPath = $"{uploadPath}/{file.FileName}";

        using (var fileStream = new FileStream(fullPath, FileMode.Create))
        { await file.CopyToAsync(fileStream);}
    }

    return $"Data of user {username}: {email} is saved succesfully";
}); 

app.Run();


