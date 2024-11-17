namespace MyCookies
{
    class Cookies
    {
        async Task GetCookies(HttpContext context)
        {
            // получаем куки
            context.Request.Cookies.TryGetValue("name", out string? name);
            context.Request.Cookies.TryGetValue("email", out string? email);

            context.Response.WriteAsync($"Name: {name}   Email:{email}");
        }
    }
}