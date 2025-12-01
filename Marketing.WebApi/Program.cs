using Marketing.Infraestrutura.Contexto;
using Marketing.Mvc.Controllers;
using Marketing.WebApi.Extensoes;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AdicionarServicosAppIOC();
RegistrarServicos.ConfigureHttpClient(builder.Services, builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

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
                .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
                .LogTo(Console.WriteLine, LogLevel.Information)
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors()
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
        );
        break;
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.AddApiServicosController();
app.UseHttpsRedirection();
app.Run();

