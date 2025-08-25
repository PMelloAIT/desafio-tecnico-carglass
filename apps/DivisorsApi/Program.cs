using System.Diagnostics;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Caching.Memory;
using NumberTools;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Divisors API",
        Version = "v1",
        Description = "Serviço para decompor um número em seus divisores e divisores primos.",
        Contact = new OpenApiContact
        {
            Name = "Paulo Mello",
            Email = "pmello@melloait.com.br",
        },
        License = new OpenApiLicense
        {
            Name = "MIT",
            Url = new Uri("https://opensource.org/licenses/MIT")
        }
    });
    var xml = Path.ChangeExtension(typeof(Program).Assembly.Location, ".xml");
    if (File.Exists(xml)) c.IncludeXmlComments(xml, includeControllerXmlComments: true);
});


builder.Services.AddMemoryCache(o =>
{
    o.SizeLimit = 10_000; // entradas
});

builder.Services.AddRateLimiter(o =>
{
    o.AddFixedWindowLimiter(policyName: "fixed", options =>
    {
        options.PermitLimit = 100;           // 100 req por janela
        options.Window = TimeSpan.FromSeconds(10);
        options.QueueLimit = 0;
    });
});

var app = builder.Build();

app.UseRateLimiter();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Divisors API v1");
    c.RoutePrefix = "docs"; // UI em /docs
});

app.MapGet("/health", () => Results.Ok(new { status = "ok", utc = DateTime.UtcNow }));


// GET /api/v1/divisors/{n}
app.MapGet("/api/v1/divisors/{n:long}", (
    long n,
    IMemoryCache cache,
    HttpContext ctx) =>
{
    if (n < 1) return Results.BadRequest(new { error = "n deve ser >= 1" });

    string cacheKey = $"divs:{n}";
    if (!cache.TryGetValue(cacheKey, out object? payload))
    {
        var sw = Stopwatch.StartNew();
        var divisors = DivisorsCalculator.GetDivisors(n);
        var primeDivisors = DivisorsCalculator.GetPrimeDivisors(n, includeOneInPrimeDivisors: true);
        sw.Stop();

        payload = new
        {
            input = n,
            divisors,
            primeDivisors,
            elapsedMs = Math.Round(sw.Elapsed.TotalMilliseconds, 3),
            cached = false
        };

        cache.Set(cacheKey, payload, new MemoryCacheEntryOptions
        {
            SlidingExpiration = TimeSpan.FromMinutes(2),
            Size = 1
        });
    }
    else
    {
        dynamic? d = payload;
        payload = new { d?.input, d?.divisors, d?.primeDivisors, d?.elapsedMs, cached = true };
    }

    return Results.Ok(payload);
})
.WithName("GetDivisors")
.WithSummary("Obtém divisores e divisores primos de um número.")
.WithDescription("Calcula em O(√n) os divisores de **n** e os divisores que são primos. Inclui `1` na lista de primos para alinhar ao enunciado.")
.RequireRateLimiting("fixed");

app.MapGet("/", (HttpContext ctx) =>
{
    var basePath = ctx.Request.PathBase.HasValue ? ctx.Request.PathBase.Value : "";
    return Results.Redirect($"{basePath}/docs", permanent: false);
})
.ExcludeFromDescription();

app.Run();
