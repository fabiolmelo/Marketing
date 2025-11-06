using System.Threading.RateLimiting;
using Marketing.Infraestrutura.Contexto;
using Marketing.Mvc.Extensoes;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Rate limit
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("fixed", opt =>
    {
        opt.PermitLimit = 4;
        opt.Window = TimeSpan.FromSeconds(12);
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        opt.QueueLimit = 2;
    });
});

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddControllersWithViews();
builder.Services.AdicionarServicosAppIOC();
RegistrarServicos.ConfigureHttpClient(builder.Services, builder.Configuration);

string connectionString;
var bancoDeDados = builder.Configuration["BancoDeDados"] ?? "";

switch (bancoDeDados)
{
    case "SqLite":
        connectionString = builder.Configuration.GetConnectionString("WebApiSqlLiteDatabase") ?? "";
        builder.Services.AddDbContext<DataContext>(
            dbContextOptions => dbContextOptions
                .UseSqlite(connectionString)
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
            );
        break;
    default:
        connectionString = builder.Configuration.GetConnectionString("MySql") ?? "";
        var serverVersion = new MySqlServerVersion(new Version(10, 2));

        builder.Services.AddDbContext<DataContext>(
            dbContextOptions => dbContextOptions
                .UseMySql(connectionString, serverVersion)
                .LogTo(Console.WriteLine, LogLevel.Information)
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors()
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
        );
        break;
}

builder.WebHost.ConfigureKestrel(options =>
           options.ListenLocalhost(8080));     
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.UseRateLimiter();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}").RequireRateLimiting("fixed");
    
System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

app.Run();
