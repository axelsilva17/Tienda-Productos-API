using TiendaProductos.Api.Models;

namespace TiendaProductos.Api.Repositories;

// Implementación en memoria de IProductoRepository.
//
// El repositorio se registra como Scoped (una instancia por request HTTP).
// Si la lista fuera un campo de instancia, cada request arrancaría con el seed data
// y un POST seguido de un GET no encontraría nada: los datos no sobrevivirían entre
// llamadas. Por eso el almacenamiento es static (compartido por todo el proceso)
// mientras la instancia del repositorio sigue siendo Scoped, como pide la consigna.
// En un repositorio real con base de datos ese estado lo aporta la DB.
public class ProductoRepositoryInMemory : IProductoRepository
{
    // Cantidad fija y conocida: List<T> + seed data en memoria.
    private static readonly List<Producto> _productos = new()
    {
        new() { Id = 1, Nombre = "Teclado mecánico",      Precio = 45999.90m,  Categoria = "Periféricos", Activo = true },
        new() { Id = 2, Nombre = "Mouse inalámbrico",     Precio = 18999.50m,  Categoria = "Periféricos", Activo = true },
        new() { Id = 3, Nombre = "Monitor 24\" Full HD",  Precio = 189999.00m, Categoria = "Monitores",   Activo = true },
        new() { Id = 4, Nombre = "Auriculares Bluetooth", Precio = 32999.00m,  Categoria = "Audio",       Activo = false }
    };

    // Los endpoints pueden atender requests concurrentes: sin lock, dos POST
    // simultáneos podrían generar el mismo Id o corromper la lista.
    private static readonly object _candado = new();

    private static int _ultimoId = _productos.Max(p => p.Id);

    public List<Producto> ObtenerTodos()
    {
        lock (_candado)
        {
            // ToList() devuelve una copia: nadie de afuera muta la lista interna.
            return _productos.ToList();
        }
    }

    public Producto? ObtenerPorId(int id)
    {
        lock (_candado)
        {
            return _productos.FirstOrDefault(p => p.Id == id);
        }
    }

    public Producto Agregar(Producto producto)
    {
        ArgumentNullException.ThrowIfNull(producto);

        lock (_candado)
        {
            producto.Id = ++_ultimoId;
            _productos.Add(producto);
            return producto;
        }
    }

    public bool Actualizar(Producto producto)
    {
        ArgumentNullException.ThrowIfNull(producto);

        lock (_candado)
        {
            var existente = _productos.FirstOrDefault(p => p.Id == producto.Id);

            if (existente is null)
            {
                return false;
            }

            existente.Nombre = producto.Nombre;
            existente.Precio = producto.Precio;
            existente.Categoria = producto.Categoria;
            existente.Activo = producto.Activo;

            return true;
        }
    }

    public bool Eliminar(int id)
    {
        lock (_candado)
        {
            var producto = _productos.FirstOrDefault(p => p.Id == id);

            return producto is not null && _productos.Remove(producto);
        }
    }
}
