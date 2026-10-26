using TiendaProductos.Api.Models;

namespace TiendaProductos.Api.Repositories;

/// <summary>
/// Contrato de la capa de acceso a datos (Data Access Layer).
/// La capa de negocio depende de esta interfaz: hoy es memoria, mañana puede ser
/// EF Core o Dapper sin tocar el servicio ni el controlador.
/// </summary>
public interface IProductoRepository
{
    /// <summary>Devuelve todos los productos almacenados.</summary>
    List<Producto> ObtenerTodos();

    /// <summary>Busca un producto por Id. Devuelve <c>null</c> si no existe.</summary>
    Producto? ObtenerPorId(int id);

    /// <summary>Agrega un producto y devuelve la entidad con el Id asignado.</summary>
    Producto Agregar(Producto producto);

    /// <summary>Actualiza un producto existente. Devuelve <c>false</c> si no existe.</summary>
    bool Actualizar(Producto producto);

    /// <summary>Elimina un producto por Id. Devuelve <c>false</c> si no existe.</summary>
    bool Eliminar(int id);
}
