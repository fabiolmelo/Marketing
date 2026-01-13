using System.Globalization;
using System.Threading.RateLimiting;
using Marketing.Infraestrutura.Contexto;
using Marketing.Mvc.Extensoes;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using NLog.Extensions.Logging;
using NLog.Web;

var builder = WebApplication.CreateBuilder(args);
var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();


// Configure NLog
builder.Services.AddLogging(logging =>
{
    logging.ClearProviders();
    logging.SetMinimumLevel(LogLevel.Warning);
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

builder.Services.AddLocalization();

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[] { new CultureInfo("pt-BR") };
    options.DefaultRequestCulture = new RequestCulture(culture: "pt-BR", uiCulture: "pt-BR");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});

var connectionStringSqLite = builder.Configuration.GetConnectionString("WebApiSqlLiteDatabase") ?? "";
builder.Services.AddDbContext<DataContext>(
    dbContextOptions => dbContextOptions
        .UseSqlite(connectionStringSqLite)
        .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
    );
    
var connectionStringMySql = builder.Configuration.GetConnectionString("MySql") ?? "";
builder.Services.AddDbContext<DataContextMySql>(
    dbContextOptions => dbContextOptions
        .UseMySql(connectionStringMySql, ServerVersion.AutoDetect(connectionStringMySql))
        .LogTo(Console.WriteLine, LogLevel.Warning)
        .EnableDetailedErrors()
        .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
);

builder.Services.AddIdentity<UsuarioEntity, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;
})
.AddEntityFrameworkStores<DataContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";          // para não autenticados
    // options.AccessDeniedPath = "/Account/Denied";  // para sem permissão
    options.AccessDeniedPath = "/Account/Login";  // para sem permissão
});


builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
});

builder.WebHost.ConfigureKestrel(options =>
            options.ListenLocalhost(8000));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<UsuarioEntity>>();

    string[] roles = new[] { "Root", "Admin", "DashBoard" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));
    }

    // Criar usuário root
    var rootEmail = "fabiolmelo30@gmail.com";
    var rootUser = await userManager.FindByEmailAsync(rootEmail);

    if (rootUser == null)
    {
        rootUser = new UsuarioEntity
        {
            UserName = rootEmail,
            Nome = "Fabio Melo",
            Email = rootEmail,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(rootUser, "Mkk182627@");

        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(rootUser, "Root");
        }
    }

    // Criar usuário root
    rootEmail = "dash@dash.com";
    rootUser = await userManager.FindByEmailAsync(rootEmail);

    if (rootUser == null)
    {
        rootUser = new UsuarioEntity
        {
            UserName = rootEmail,
            Nome = "DashBoard",
            Email = rootEmail,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(rootUser, "Mkk182627@");

        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(rootUser, "DashBoard");
        }
    }

    // Criar usuário root
    rootEmail = "admin@admin.com";
    rootUser = await userManager.FindByEmailAsync(rootEmail);

    if (rootUser == null)
    {
        rootUser = new UsuarioEntity
        {
            UserName = rootEmail,
            Nome = "Administrador",
            Email = rootEmail,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(rootUser, "Mkk182627@");

        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(rootUser, "Admin");
        }
    }

}



if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseRequestLocalization();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseRateLimiter();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}").RequireRateLimiting("fixed");
    
System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

app.Run();

