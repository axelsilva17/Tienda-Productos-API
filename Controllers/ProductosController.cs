using Microsoft.AspNetCore.Mvc;
using TiendaProductos.Api.Dtos;
using TiendaProductos.Api.Services;

namespace TiendaProductos.Api.Controllers;

// Endpoints REST del catálogo de productos (Presentation Layer).
// No contiene lógica de negocio: valida el contrato HTTP, delega al servicio
// y traduce el resultado a códigos de estado con IActionResult.
[ApiController]
[Route("api/productos")]
[Produces("application/json")]
public class ProductosController : ControllerBase
{
    private readonly IProductoService _service;

    public ProductosController(IProductoService service)
    {
        _service = service;
    }

    // Obtiene todos los productos del catálogo.
    // 200: Lista de productos (puede estar vacía).
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ProductoDto>), StatusCodes.Status200OK)]
    public IActionResult ObtenerTodos()
    {
        return Ok(_service.ObtenerTodos());
    }

    // Obtiene un producto por su Id.
    // id: Id del producto.
    // 200: Producto encontrado.
    // 404: No existe un producto con ese Id.
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ProductoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult ObtenerPorId(int id)
    {
        var producto = _service.ObtenerPorId(id);

        if (producto is null)
        {
            return NotFound(new { mensaje = $"No existe un producto con Id {id}." });
        }

        return Ok(producto);
    }

    // Crea un producto nuevo.
    // dto: Datos del producto a crear.
    // 201: Producto creado. El header Location apunta al recurso.
    // 400: Datos inválidos (Data Annotations).
    [HttpPost]
    [ProducesResponseType(typeof(ProductoDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public IActionResult Crear([FromBody] CreateProductoDto dto)
    {
        // Si el modelo es inválido, [ApiController] corta acá con 400 antes de entrar.
        var creado = _service.Crear(dto);

        return CreatedAtAction(
            nameof(ObtenerPorId),
            new { id = creado.Id },
            creado);
    }

    // Actualiza un producto existente.
    // id: Id del producto a actualizar.
    // dto: Nuevos datos del producto.
    // 200: Producto actualizado.
    // 400: Datos inválidos (Data Annotations).
    // 404: No existe un producto con ese Id.
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ProductoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Actualizar(int id, [FromBody] CreateProductoDto dto)
    {
        var actualizado = _service.Actualizar(id, dto);

        if (actualizado is null)
        {
            return NotFound(new { mensaje = $"No existe un producto con Id {id}." });
        }

        return Ok(actualizado);
    }

    // Elimina un producto por su Id.
    // id: Id del producto a eliminar.
    // 204: Producto eliminado.
    // 404: No existe un producto con ese Id.
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Eliminar(int id)
    {
        if (!_service.Eliminar(id))
        {
            return NotFound(new { mensaje = $"No existe un producto con Id {id}." });
        }

        return NoContent();
    }
}
