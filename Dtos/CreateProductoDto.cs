using System.ComponentModel.DataAnnotations;

namespace TiendaProductos.Api.Dtos;

// DTO de entrada para crear o editar un producto.
// Las Data Annotations se validan automáticamente gracias a [ApiController]:
// si el modelo es inválido, la acción nunca se ejecuta y se devuelve 400 Bad Request.
public class CreateProductoDto
{
    // Nombre comercial del producto.
    // Ejemplo: Teclado mecánico.
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 100 caracteres.")]
    public string Nombre { get; set; } = string.Empty;

    // Precio unitario en pesos. Debe ser mayor a 0.
    // Ejemplo: 45999.90.
    [Required(ErrorMessage = "El precio es obligatorio.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0.")]
    public decimal Precio { get; set; }

    // Categoría a la que pertenece el producto.
    // Ejemplo: Periféricos.
    [Required(ErrorMessage = "La categoría es obligatoria.")]
    public string Categoria { get; set; } = string.Empty;

    // Indica si el producto está disponible para la venta.
    // Ejemplo: true.
    public bool Activo { get; set; } = true;
}
