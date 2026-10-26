using Microsoft.AspNetCore.Mvc;
using TiendaProductos.Api.Dtos;
using TiendaProductos.Api.Services;

namespace TiendaProductos.Api.Controllers;

/// <summary>
/// Endpoints REST del catálogo de productos (Presentation Layer).
/// No contiene lógica de negocio: valida el contrato HTTP, delega al servicio
/// y traduce el resultado a códigos de estado con <see cref="IActionResult"/>.
/// </summary>
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

    /// <summary>Obtiene todos los productos del catálogo.</summary>
    /// <response code="200">Lista de productos (puede estar vacía).</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ProductoDto>), StatusCodes.Status200OK)]
    public IActionResult ObtenerTodos()
    {
        return Ok(_service.ObtenerTodos());
    }

    /// <summary>Obtiene un producto por su Id.</summary>
    /// <param name="id">Id del producto.</param>
    /// <response code="200">Producto encontrado.</response>
    /// <response code="404">No existe un producto con ese Id.</response>
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

    /// <summary>Crea un producto nuevo.</summary>
    /// <param name="dto">Datos del producto a crear.</param>
    /// <response code="201">Producto creado. El header Location apunta al recurso.</response>
    /// <response code="400">Datos inválidos (Data Annotations).</response>
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

    /// <summary>Actualiza un producto existente.</summary>
    /// <param name="id">Id del producto a actualizar.</param>
    /// <param name="dto">Nuevos datos del producto.</param>
    /// <response code="200">Producto actualizado.</response>
    /// <response code="400">Datos inválidos (Data Annotations).</response>
    /// <response code="404">No existe un producto con ese Id.</response>
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

    /// <summary>Elimina un producto por su Id.</summary>
    /// <param name="id">Id del producto a eliminar.</param>
    /// <response code="204">Producto eliminado.</response>
    /// <response code="404">No existe un producto con ese Id.</response>
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
