using Microsoft.EntityFrameworkCore;
using PersonasCrud.Domain.API.Repositories;
using PersonasCrud.Domain.Impl.Context;
using PersonasCrud.Domain.Impl.Repositories;
using PersonasCrud.RestV1.Impl.Middleware;
using PersonasCrud.Service.API.Services;
using PersonasCrud.Service.Impl.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Configurar servicios
builder.Services.AddControllers();

// Configurar Entity Framework con base de datos en memoria
builder.Services.AddDbContext<PersonasDbContext>(options =>
    options.UseInMemoryDatabase("PersonasDb"));

// Configurar inyección de dependencias
builder.Services.AddScoped<IPersonasRepository, PersonasRepositoryEF>();
builder.Services.AddScoped<IPersonasService, PersonasService>();

// Configurar Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Personas CRUD API",
        Version = "v1",
        Description = "API RESTful para gestión de personas con arquitectura limpia",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Equipo de Desarrollo",
            Email = "dev@empresa.com"
        }
    });

    // Incluir comentarios XML de la documentación
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
    
    // Incluir comentarios XML del proyecto RestV1.Impl
    var restImplXmlFile = "PersonasCrud.RestV1.Impl.xml";
    var restImplXmlPath = Path.Combine(AppContext.BaseDirectory, restImplXmlFile);
    if (File.Exists(restImplXmlPath))
    {
        c.IncludeXmlComments(restImplXmlPath);
    }
    
    // Incluir comentarios XML del proyecto RestV1.API
    var restApiXmlFile = "PersonasCrud.RestV1.API.xml";
    var restApiXmlPath = Path.Combine(AppContext.BaseDirectory, restApiXmlFile);
    if (File.Exists(restApiXmlPath))
    {
        c.IncludeXmlComments(restApiXmlPath);
    }
});

var app = builder.Build();

// Configurar pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Personas CRUD API v1");
        c.RoutePrefix = "swagger";
        c.DisplayRequestDuration();
        c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.List);
    });
}

// Agregar middleware de manejo de excepciones
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();
app.UseRouting();

// Mapear controladores
app.MapControllers();

await app.RunAsync();
