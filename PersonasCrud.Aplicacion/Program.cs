using Microsoft.EntityFrameworkCore;
using PersonasCrud.Domain.API.Repositories;
using PersonasCrud.Domain.Impl.Context;
using PersonasCrud.Domain.Impl.Repositories;
using PersonasCrud.RestV1.Impl.Middleware;
using PersonasCrud.Service.API.Services;
using PersonasCrud.Service.Impl.Services;

var builder = WebApplication.CreateBuilder(args);

// Configurar servicios
builder.Services.AddControllers();

// Configurar Entity Framework con base de datos en memoria
builder.Services.AddDbContext<PersonasDbContext>(options =>
    options.UseInMemoryDatabase("PersonasDb"));

// Configurar inyección de dependencias
builder.Services.AddScoped<IPersonasRepository, PersonasRepositoryEF>();
builder.Services.AddScoped<IPersonasService, PersonasService>();

// Configurar OpenAPI para desarrollo
builder.Services.AddOpenApi();

var app = builder.Build();

// Configurar pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Agregar middleware de manejo de excepciones
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();
app.UseRouting();

// Mapear controladores
app.MapControllers();

await app.RunAsync();
