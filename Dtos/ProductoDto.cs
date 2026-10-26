namespace TiendaProductos.Api.Dtos;

// DTO de salida: la forma en que la API expone un producto.
// El controlador retorna SIEMPRE este tipo, nunca la entidad Models.Producto.
// Así el contrato público no queda atado al modelo interno.
public class ProductoDto
{
    // Identificador único del producto.
    // Ejemplo: 1.
    public int Id { get; set; }

    // Nombre comercial del producto.
    // Ejemplo: Teclado mecánico.
    public string Nombre { get; set; } = string.Empty;

    // Precio unitario en pesos.
    // Ejemplo: 45999.90.
    public decimal Precio { get; set; }

    // Categoría del producto.
    // Ejemplo: Periféricos.
    public string Categoria { get; set; } = string.Empty;

    // Indica si el producto está disponible para la venta.
    // Ejemplo: true.
    public bool Activo { get; set; } = true;
}
