using TiendaProductos.Api.Dtos;

namespace TiendaProductos.Api.Services;

// Contrato de la capa de negocio (Business Layer).
// Trabaja exclusivamente con DTOs: la entidad Producto no sale de acá.
public interface IProductoService
{
    // Devuelve el catálogo completo como DTOs de respuesta.
    List<ProductoDto> ObtenerTodos();

    // Devuelve un producto por Id, o null si no existe.
    ProductoDto? ObtenerPorId(int id);

    // Crea un producto a partir del DTO de entrada y devuelve el DTO de respuesta.
    ProductoDto Crear(CreateProductoDto dto);

    // Actualiza un producto existente. Devuelve null si el Id no existe.
    ProductoDto? Actualizar(int id, CreateProductoDto dto);

    // Elimina un producto. Devuelve false si el Id no existe.
    bool Eliminar(int id);
}
