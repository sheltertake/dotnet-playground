var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/titles", () =>
{
    return new[]
    {
        "Inception", "Alien"
    };
});

app.Run();