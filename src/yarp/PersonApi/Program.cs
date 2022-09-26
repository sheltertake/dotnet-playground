var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/directors", () =>
{
    return new[]
    {
        "Kubrick", "Nolan", "Scorsese", "Coppola", "Spielberg"
    };
});

app.Run();