using Marketing.Infraestrutura.Contexto;
using Marketing.Mvc.Controllers;
using Marketing.WebApi.Extensoes;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var connectionString = builder.Configuration.GetConnectionString("WebApiSqlLiteDatabase");
builder.Services.AddDbContext<DataContext>(x => x.UseSqlite(connectionString));
builder.Services.AdicionarServicosAppIOC();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.AddApiServicosController();
app.UseHttpsRedirection();
app.Run();

