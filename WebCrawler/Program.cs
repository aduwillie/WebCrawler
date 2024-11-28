using Cocona;

var builder = CoconaApp.CreateBuilder();

var app = builder.Build();

app.AddCommand("crawl", (string entrySite) =>
{
  Console.WriteLine($"Crawling website: {entrySite}");
});

await app.RunAsync();
