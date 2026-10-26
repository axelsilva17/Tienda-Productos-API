using TiendaProductos.Api.Models;

namespace TiendaProductos.Api.Repositories;

// Contrato de la capa de acceso a datos (Data Access Layer).
// La capa de negocio depende de esta interfaz: hoy es memoria, mañana puede ser
// EF Core o Dapper sin tocar el servicio ni el controlador.
public interface IProductoRepository
{
    // Devuelve todos los productos almacenados.
    List<Producto> ObtenerTodos();

    // Busca un producto por Id. Devuelve null si no existe.
    Producto? ObtenerPorId(int id);

    // Agrega un producto y devuelve la entidad con el Id asignado.
    Producto Agregar(Producto producto);

    // Actualiza un producto existente. Devuelve false si no existe.
    bool Actualizar(Producto producto);

    // Elimina un producto por Id. Devuelve false si no existe.
    bool Eliminar(int id);
}
