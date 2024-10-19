using System.Net.Cache;

int id = 0;
List<Person> users = new List<Person>()
{
    new Person() { Id = id++, Name = "Dowran", Age = 20},
    new Person() { Id = id++, Name = "Dowlet", Age = 22},
    new Person() { Id = id++, Name = "Jeyran", Age= 23}
};

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("api/users", () =>  users);

app.MapGet("api/users/{id}", (int id) =>
{
    var user = users.FirstOrDefault(u => u.Id == id);
    if (user == null) return Results.NotFound(new { message = "User Not found" });
    return Results.Json(user);
});

app.MapDelete("api/users/{id}", (int id) =>
{
    var user = users.FirstOrDefault(u => u.Id == id);
    if (user == null) return Results.NotFound(new { message = "User Not found" });
    users.Remove(user);
    return Results.Json(user);
});

app.MapPost("api/users/", (Person user) =>
{
    if (user == null) return Results.NotFound(new { message = "User Not found" });
    user.Id = id++;
    users.Add(user);
    return Results.Json(user);
});

app.MapPut("api/users/{id}", (Person person) =>
{
    var user = users.FirstOrDefault(u => u.Id == id);
    if (user == null) return Results.NotFound(new { message = "User Not found" });
    user.Name = person.Name;
    user.Age = person.Age;
    return Results.Json(user);
});


app.Run();

class Person
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
}


