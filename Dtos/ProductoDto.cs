namespace TiendaProductos.Api.Dtos;

/// <summary>
/// DTO de salida: la forma en que la API expone un producto.
/// El controlador retorna SIEMPRE este tipo, nunca la entidad <see cref="Models.Producto"/>.
/// Así el contrato público no queda atado al modelo interno.
/// </summary>
public class ProductoDto
{
    /// <summary>Identificador único del producto.</summary>
    /// <example>1</example>
    public int Id { get; set; }

    /// <summary>Nombre comercial del producto.</summary>
    /// <example>Teclado mecánico</example>
    public string Nombre { get; set; } = string.Empty;

    /// <summary>Precio unitario en pesos.</summary>
    /// <example>45999.90</example>
    public decimal Precio { get; set; }

    /// <summary>Categoría del producto.</summary>
    /// <example>Periféricos</example>
    public string Categoria { get; set; } = string.Empty;

    /// <summary>Indica si el producto está disponible para la venta.</summary>
    /// <example>true</example>
    public bool Activo { get; set; }
}
