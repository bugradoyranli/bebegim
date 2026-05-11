using bebegim.Exceptions;
using bebegim.Data;
using DotNetEnv;
using System.Reflection; // Bunu en üste ekleyin
using Microsoft.EntityFrameworkCore;

Env.Load();
var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL");

var builder = WebApplication.CreateBuilder(args);

// --- SERVİSLER ---
builder.Services.AddControllers();

// .NET 9'un ana OpenAPI servisi (Swashbuckle 10 bunu kullanır)

// Swagger üreticisi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(options =>
{
    // XML dosyasının yolunu bul ve Swagger'a dahil et
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
    options.IncludeXmlComments(xmlPath);
});

builder.Services.AddDbContext<BebegimDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

var app = builder.Build();

// --- PIPELINE ---
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    // .NET 9 OpenAPI dökümanını oluşturur
    
    // Klasik Swagger arayüzünü açar
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();