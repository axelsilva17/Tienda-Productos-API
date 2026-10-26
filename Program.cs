using Microsoft.OpenApi;
using TiendaProductos.Api.Repositories;
using TiendaProductos.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// ---------- Capa de presentación ----------
builder.Services.AddControllers();

// ---------- Inyección de dependencias ----------
// Ciclo de vida Scoped: una instancia por request HTTP, compartida entre las capas
// que la piden dentro de ese mismo request.
builder.Services.AddScoped<IProductoRepository, ProductoRepositoryInMemory>();
builder.Services.AddScoped<IProductoService, ProductoService>();

// ---------- Swagger / OpenAPI ----------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opciones =>
{
    opciones.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Tienda Productos API",
        Version = "v1",
        Description = "Web API RESTful para la gestión de productos (Bloque 2). " +
                      "Arquitectura en 3 capas: Controller -> Service -> Repository."
    });
});

var app = builder.Build();

// Swagger habilitado siempre (no solo en Development) para que el docente
// pueda clonar, ejecutar y probar los endpoints sin configuración extra.
app.UseSwagger();
app.UseSwaggerUI(opciones =>
{
    opciones.SwaggerEndpoint("/swagger/v1/swagger.json", "Tienda Productos API v1");
    opciones.RoutePrefix = "swagger";
});

app.MapControllers();

app.Run();
