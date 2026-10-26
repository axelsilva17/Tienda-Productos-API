using TiendaProductos.Api.Dtos;
using TiendaProductos.Api.Models;
using TiendaProductos.Api.Repositories;

namespace TiendaProductos.Api.Services;

// Implementación de la capa de negocio.
// Responsabilidades:
// - Transformar entre entidades y DTOs (mapeo).
// - Aplicar reglas de negocio (normalizar textos, decidir qué se actualiza).
// - Traducir "no encontrado" a null/false, que el controlador convierte en 404.
//   El servicio no conoce HTTP.
public class ProductoService : IProductoService
{
    private readonly IProductoRepository _repository;

    // El repositorio llega por inyección de dependencias desde el contenedor.
    public ProductoService(IProductoRepository repository)
    {
        _repository = repository;
    }

    public List<ProductoDto> ObtenerTodos()
    {
        return _repository.ObtenerTodos()
            .Select(ADto)
            .ToList();
    }

    public ProductoDto? ObtenerPorId(int id)
    {
        var producto = _repository.ObtenerPorId(id);

        return producto is null ? null : ADto(producto);
    }

    public ProductoDto Crear(CreateProductoDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        var producto = ADominio(dto);
        var creado = _repository.Agregar(producto);

        return ADto(creado);
    }

    public ProductoDto? Actualizar(int id, CreateProductoDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        var producto = ADominio(dto);
        producto.Id = id;

        return _repository.Actualizar(producto) ? ADto(producto) : null;
    }

    public bool Eliminar(int id)
    {
        return _repository.Eliminar(id);
    }

    // ---------- Mapeo entidad <-> DTO ----------

    private static Producto ADominio(CreateProductoDto dto) => new()
    {
        Nombre = dto.Nombre.Trim(),
        Precio = dto.Precio,
        Categoria = dto.Categoria.Trim(),
        Activo = dto.Activo
    };

    private static ProductoDto ADto(Producto producto) => new()
    {
        Id = producto.Id,
        Nombre = producto.Nombre,
        Precio = producto.Precio,
        Categoria = producto.Categoria,
        Activo = producto.Activo
    };
}
