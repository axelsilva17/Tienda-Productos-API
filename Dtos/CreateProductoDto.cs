using System.ComponentModel.DataAnnotations;

namespace TiendaProductos.Api.Dtos;

/// <summary>
/// DTO de entrada para crear o editar un producto.
/// Las Data Annotations se validan automáticamente gracias a [ApiController]:
/// si el modelo es inválido, la acción nunca se ejecuta y se devuelve 400 Bad Request.
/// </summary>
public class CreateProductoDto
{
    /// <summary>Nombre comercial del producto.</summary>
    /// <example>Teclado mecánico</example>
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 100 caracteres.")]
    public string Nombre { get; set; } = string.Empty;

    /// <summary>Precio unitario en pesos. Debe ser mayor a 0.</summary>
    /// <example>45999.90</example>
    [Required(ErrorMessage = "El precio es obligatorio.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0.")]
    public decimal Precio { get; set; }

    /// <summary>Categoría a la que pertenece el producto.</summary>
    /// <example>Periféricos</example>
    [Required(ErrorMessage = "La categoría es obligatoria.")]
    public string Categoria { get; set; } = string.Empty;

    /// <summary>Indica si el producto está disponible para la venta.</summary>
    /// <example>true</example>
    public bool Activo { get; set; } = true;
}
