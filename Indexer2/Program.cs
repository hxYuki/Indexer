using Microsoft.EntityFrameworkCore;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

var AppName = "Indexer";
var confPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), AppName, "host.json");
var jsonSerializerOption = new JsonSerializerOptions() { PropertyNameCaseInsensitive = false, WriteIndented = true };

if (!File.Exists(confPath))
{
    Directory.CreateDirectory(Path.GetDirectoryName(confPath)!);
    File.WriteAllText(confPath, JsonSerializer.Serialize(HostConfig.CreateUninitialized(), jsonSerializerOption));
}

builder.Configuration.AddJsonFile(confPath, optional: false, reloadOnChange: true);

//builder.Services.AddDbContext<IndexesContext>(opt=>);
var app = builder.Build();

app.Use(async (context, next) =>
{
    if(builder.Configuration.Get<HostConfig>().Host == "")
    {
        context.Response.StatusCode = 500;
        await context.Response.WriteAsync(
            $"App is not initialized with your host information.\nPlease edit `{confPath}` and continue.");
    }
    else await next.Invoke();
});

app.MapGet("/", () => "Hello World!");

app.MapGet("/recent/{page}", (int page) =>
{

    return Results.Ok("asd");
});

app.Run();


class IndexesContext : DbContext
{
    public IndexesContext(DbContextOptions<IndexesContext> options) : base(options) { }

    public DbSet<IndexItem> IndexItems => Set<IndexItem>();
}

record IndexItem(int Id, string Title = "", string Abstract = "", string Address = "");

record Poster(string Host, string Name, string Avatar);


public record HostConfig(string Host, string Name, string Avatar)
{
    public HostConfig(): this("","","") {}
    public static HostConfig CreateUninitialized() => new();
}