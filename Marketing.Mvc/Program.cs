
using System.Globalization;
using Marketing.Infraestrutura.Contexto;
using Marketing.Mvc.Extensoes;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using NLog.Web;

var builder = WebApplication.CreateBuilder(args);

#region Logging (NLog)

builder.Logging.ClearProviders();
builder.Host.UseNLog();

#endregion

#region Kestrel

builder.WebHost.ConfigureKestrel(options =>
{
    options.AddServerHeader = false;
    options.ListenAnyIP(8000);
});

#endregion

#region Database

var connectionStringSqLite =
    builder.Configuration.GetConnectionString("WebApiSqlLiteDatabase")
    ?? throw new InvalidOperationException("Connection string SQLite not configured.");

builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlite(connectionStringSqLite);
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

    if (builder.Environment.IsDevelopment())
        options.EnableDetailedErrors();
});

var connectionStringMySql =
    builder.Configuration.GetConnectionString("MySql")
    ?? throw new InvalidOperationException("Connection string MySql not configured.");

builder.Services.AddDbContext<DataContextMySql>(options =>
{
    options.UseMySql(connectionStringMySql, ServerVersion.AutoDetect(connectionStringMySql));
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

    if (builder.Environment.IsDevelopment())
        options.EnableDetailedErrors();
});

#endregion

#region Identity

builder.Services.AddIdentity<UsuarioEntity, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 10;

    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);

    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<DataContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/Login";

    options.Cookie.HttpOnly = true;

    // 🔥 Correção principal
    options.Cookie.SecurePolicy = builder.Environment.IsDevelopment()
        ? CookieSecurePolicy.SameAsRequest
        : CookieSecurePolicy.Always;

    options.Cookie.SameSite = SameSiteMode.Lax;

    options.SlidingExpiration = true;
    options.ExpireTimeSpan = TimeSpan.FromHours(2);
});

#endregion

#region MVC + API

builder.Services.AddControllersWithViews();
builder.Services.AddControllers();

#endregion

#region Localization

builder.Services.AddLocalization();

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[] { new CultureInfo("pt-BR") };

    options.DefaultRequestCulture = new RequestCulture("pt-BR");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});

#endregion

#region Proxy / Forward Headers

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;

    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});

#endregion

#region Infra

builder.Services.AdicionarServicosAppIOC();
RegistrarServicos.ConfigureHttpClient(builder.Services, builder.Configuration);

builder.Services.AddHealthChecks();
builder.Services.AddResponseCompression();

#endregion

var app = builder.Build();

#region Middleware Pipeline

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseForwardedHeaders();
app.UseRequestLocalization();

// 🔥 HTTPS apenas em produção
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseResponseCompression();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Security Headers
app.Use(async (context, next) =>
{
    context.Response.Headers.TryAdd("X-Content-Type-Options", "nosniff");
    context.Response.Headers.TryAdd("X-Frame-Options", "DENY");
    context.Response.Headers.TryAdd("Referrer-Policy", "no-referrer");
    context.Response.Headers.TryAdd("X-XSS-Protection", "1; mode=block");
    context.Response.Headers.TryAdd("Content-Security-Policy",
        "default-src 'self'; object-src 'none'; frame-ancestors 'none';");

    await next();
});

#endregion

#region Endpoints

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.MapControllers()
    .RequireRateLimiting("api");

app.MapHealthChecks("/health");

#endregion

System.Text.Encoding.RegisterProvider(
    System.Text.CodePagesEncodingProvider.Instance);

#region Seed Roles + Users

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

    async Task CriarUsuario(string email, string nome, string role)
    {
        var user = await userManager.FindByEmailAsync(email);

        if (user == null)
        {
            user = new UsuarioEntity
            {
                UserName = email,
                Nome = nome,
                Email = email,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(user, "Mkk182627@");

            if (result.Succeeded)
                await userManager.AddToRoleAsync(user, role);
        }
    }

    await CriarUsuario("fabiolmelo30@gmail.com", "Fabio Melo", "Root");
    await CriarUsuario("admin@admin.com", "Administrador", "Admin");
    await CriarUsuario("dash@dash.com", "DashBoard", "DashBoard");
}

#endregion

app.Run();