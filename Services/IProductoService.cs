using TiendaProductos.Api.Dtos;

namespace TiendaProductos.Api.Services;

/// <summary>
/// Contrato de la capa de negocio (Business Layer).
/// Trabaja exclusivamente con DTOs: la entidad <c>Producto</c> no sale de acá.
/// </summary>
public interface IProductoService
{
    /// <summary>Devuelve el catálogo completo como DTOs de respuesta.</summary>
    List<ProductoDto> ObtenerTodos();

    /// <summary>Devuelve un producto por Id, o <c>null</c> si no existe.</summary>
    ProductoDto? ObtenerPorId(int id);

    /// <summary>Crea un producto a partir del DTO de entrada y devuelve el DTO de respuesta.</summary>
    ProductoDto Crear(CreateProductoDto dto);

    /// <summary>Actualiza un producto existente. Devuelve <c>null</c> si el Id no existe.</summary>
    ProductoDto? Actualizar(int id, CreateProductoDto dto);

    /// <summary>Elimina un producto. Devuelve <c>false</c> si el Id no existe.</summary>
    bool Eliminar(int id);
}
