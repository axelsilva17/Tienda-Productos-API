namespace TiendaProductos.Api.Models;

/// <summary>
/// Entidad de dominio que representa un producto del catálogo.
/// Es la pieza compartida por las tres capas: el repositorio la guarda,
/// el servicio la transforma a DTO y el controlador nunca la expone.
/// </summary>
public class Producto
{
    public int Id { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public decimal Precio { get; set; }

    public string Categoria { get; set; } = string.Empty;

    public bool Activo { get; set; } = true;
}
