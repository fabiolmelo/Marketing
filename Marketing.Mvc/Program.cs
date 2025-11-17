using System.Threading.RateLimiting;
using Marketing.Infraestrutura.Contexto;
using Marketing.Mvc.Extensoes;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using NLog.Extensions.Logging;
using NLog.Web;

var builder = WebApplication.CreateBuilder(args);
var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

try
{
    // Configure NLog
    builder.Services.AddLogging(logging =>
    {
        logging.ClearProviders();
        logging.SetMinimumLevel(LogLevel.Trace);
    });
    builder.Services.AddSingleton<ILoggerProvider, NLogLoggerProvider>();

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
            var serverVersion = new MySqlServerVersion(new Version(11, 4));

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

    builder.Services.Configure<ForwardedHeadersOptions>(options =>
    {
        options.ForwardedHeaders =
            ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    });

    builder.WebHost.ConfigureKestrel(options =>
                options.ListenLocalhost(8000));

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

}
catch (Exception exception)
{
    // Loga erros de inicialização
    logger.Fatal(exception, "Aplicação encerrada inesperadamente durante a inicialização");
    throw;
}
finally
{
    NLog.LogManager.Shutdown();
}
